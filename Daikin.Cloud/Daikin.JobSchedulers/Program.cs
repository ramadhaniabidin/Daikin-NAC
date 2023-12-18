using Daikin.JobSchedulers.Common;
using Daikin.JobSchedulersLogic.Common.Model;
using Daikin.JobSchedulersLogic.Model;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Daikin.JobSchedulersLogic.Controller;

namespace Daikin.JobSchedulers
{
    class Program
    {
        static DatabaseManager db = new DatabaseManager();
        static SqlConnection conn = new SqlConnection();
        static SqlDataReader reader = null;
        static DataTable dt = new DataTable();

        public static string GetSiteURL()
        {
            try
            {
                return ConfigurationManager.AppSettings["SiteUrl"];
            }
            catch
            {
                return "https://sp3.daikin.co.id:8443";
            }
        }
        public static string GetFunctionCode()
        {
            try
            {
                return ConfigurationManager.AppSettings["FunctionCode"];
            }
            catch
            {
                return "0";
            }
        }

        static void Main(string[] args)
        {
            try
            {
                string F_Code = GetFunctionCode();
                Console.WriteLine(F_Code);
                if (F_Code == "0")
                {
                    var siteURL = GetSiteURL();
                    var result = GenerateCode(siteURL);

                    WriteToFile(DateTime.Now.ToString("dd MMM yyyy HH:mm:ss tt") + " - Successfully Generate Code " + result + " items");
                }
                else if (F_Code == "33") //Batch 1 Feedback 
                {
                    var count = 0;
                    var total = 0;
                    SAP2Controller ctrl = new SAP2Controller();
                    try
                    {
                        ctrl.ReadBatchFeedbacks(ref total, ref count);
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToFile(ex.ToString());
                    }
                    finally
                    {
                        Utility.WriteToFile("Batch 1 - Successfully Read SAP Posting Feedbacks " + count + " of " + total + (total > 1 ? " files" : " file"));
                    }

                }
                else if (F_Code == "34") //Batch 2 Feedback
                {
                    SAP3Controller ctrl = new SAP3Controller();

                    var count = 0;
                    var total = 0;

                    try
                    {
                        ctrl.ReadBatchFeedbacks(ref total, ref count);
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToFile(ex.ToString());
                    }
                    finally
                    {
                        Utility.WriteToFile("Batch 2 - Successfully Read SAP Feedbacks " + count + " of " + total + (total > 1 ? " files" : " file"));
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToFile(DateTime.Now.ToString("dd MMM yyyy HH:mm:ss tt") + " - " + ex);
            }
            finally
            {
                //Console.ReadLine();
            }

        }

        public static List<AutoCodeBatch> GetListData()
        {
            dt = new DataTable();
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "usp_AutoCodeBatch_GetList";
                db.cmd.CommandType = CommandType.StoredProcedure;

                db.cmd.Parameters.Clear();

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                var list = Utility.ConvertDataTableToList<AutoCodeBatch>(dt);
                return list;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public static string GenerateCode(string SiteUrl)
        {
            var list = GetListData();
            var total = list.Count;
            var count = 0;
            foreach (var item in list)
            {
                var isError = false;
                var errorMessage = "";

                SPSite spSite = null;
                SPListItem spItem = null;

                var isTrans = true;
                try
                {
                    db.OpenConnection(ref conn, isTrans);
                    #region GenerateCode
                    db.cmd.CommandText = "usp_Utility_AutoCounter";
                    db.cmd.CommandType = CommandType.StoredProcedure;

                    db.cmd.Parameters.Clear();
                    db.AddInParameter(db.cmd, "FieldName", item.ColumnName);
                    db.AddInParameter(db.cmd, "TableName", item.TableName);
                    db.AddInParameter(db.cmd, "FieldCriteria", item.ColumnName);
                    db.AddInParameter(db.cmd, "ValueCriteria", item.Format);
                    db.AddInParameter(db.cmd, "LengthOfString", item.LengthOfString);
                    //db.AddOutParameter(db.cmd, "AutoCode", SqlDbType.VarChar);

                    reader = db.cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(reader);
                    db.CloseDataReader(reader);

                    string autoCode = Utility.ConvertDataTableToList<GeneratedCode>(dt)[0].AutoCode;
                    #endregion

                    #region Update AutoCodeBatch
                    db.cmd.CommandText = "usp_AutoCodeBatch_UpdateFlag";
                    db.cmd.CommandType = CommandType.StoredProcedure;

                    db.cmd.Parameters.Clear();
                    db.AddInParameter(db.cmd, "ID", item.ID);
                    db.AddInParameter(db.cmd, "Generated", 1);
                    db.AddInParameter(db.cmd, "SysMessage", "Generated");

                    db.cmd.ExecuteNonQuery();
                    #endregion

                    #region Update Item
                    db.cmd.CommandText = "usp_AutoCodeBatch_UpdateItem";
                    db.cmd.CommandType = CommandType.StoredProcedure;

                    db.cmd.Parameters.Clear();
                    db.AddInParameter(db.cmd, "TransID", item.TransID);
                    db.AddInParameter(db.cmd, "TableName", item.TableName);
                    db.AddInParameter(db.cmd, "DetailName", item.DetailName);
                    db.AddInParameter(db.cmd, "ColumnName", item.ColumnName);
                    db.AddInParameter(db.cmd, "AutoCode", autoCode);

                    db.cmd.ExecuteNonQuery();
                    #endregion

                    db.CloseConnection(ref conn, isTrans);


                    #region Update ListItem
                    SPSecurity.RunWithElevatedPrivileges(delegate () {
                        spSite = new SPSite(SiteUrl);
                        SPWeb spWeb = spSite.OpenWeb();
                        SPList spList = spWeb.Lists.TryGetList(item.ListName);

                        spWeb.AllowUnsafeUpdates = true;
                        spItem = spList.GetItemById(item.ItemID.Value);
                        spItem["Title"] = autoCode;
                        spItem["Request No"] = autoCode;
                        spItem["Approval Status"] = "Generated";
                        spItem["Approval Status ID"] = 3;
                        spItem["Workflow Status"] = "Approval";
                        spItem["Form Status"] = "Start";
                        spItem.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    });
                    Console.WriteLine(autoCode);
                    #endregion

                    #region InsertHistoryLog
                    SPWeb web = new SPSite(Utility.SpSiteUrl_DEV).OpenWeb();
                    SPList listData = web.Lists[item.ListName];
                    web.AllowUnsafeUpdates = true;

                    SPListItem listItem = listData.GetItemById(item.ItemID.Value);

                    string CurrentLogin = listItem["Author"].ToString();
                    string CurrentLoginName = listItem["Requester_x0020_Name"].ToString();

                    try
                    {
                        db.OpenConnection(ref conn);
                        db.cmd.CommandText = "[dbo].[usp_NWC_insertHistoryLog]";
                        db.cmd.CommandType = CommandType.StoredProcedure;
                        db.cmd.Parameters.Clear();

                        db.AddInParameter(db.cmd, "listName", item.ListName);
                        db.AddInParameter(db.cmd, "listItemID", item.ItemID.Value);
                        db.AddInParameter(db.cmd, "action", 1);
                        db.AddInParameter(db.cmd, "currentLogin", CurrentLogin);
                        db.AddInParameter(db.cmd, "currentLoginName", CurrentLoginName);
                        db.AddInParameter(db.cmd, "currentLayer", "0");

                        db.cmd.ExecuteNonQuery();
                        db.CloseConnection(ref conn);
                    }

                    catch (Exception ex)
                    {
                        db.CloseConnection(ref conn);
                        throw ex;
                    }
                    
                    #endregion

                    #region Start Workflow

                    StartData startData = new StartData();
                    startData.se_headerid = (int)item.ItemID;
                    startData.se_tablename = item.ModuleCode;

                    NWCParamModel nwcParamModel = new NWCParamModel();
                    nwcParamModel.startData = startData;

                    NintexWorkflowCloud nwc = new NintexWorkflowCloud();
                    nwc.url = "https://daikin.workflowcloud.com/api/v1/workflow/published/e13eb321-e44c-4242-a21e-ae024e620017/instances?token=CL9crPPiU0OJMX16I2Imuqm61SlxPsXNYNw4zePxxT3l0K1j5umw8PIafK5BpoYpL4fitN";
                    nwc.param = nwcParamModel;

                    //StartWorkflowBySystemAccount(nwc);
                    Task.Run(async () => { await new Program().StartNWC(nwc); }).Wait();
                    #endregion


                    count++;
                }
                catch (Exception ex)
                {
                    isTrans = false;
                    WriteToFile(DateTime.Now.ToString("dd MMM yyyy HH:mm:ss") + " - " + ex.Message);
                    WriteToFile("   " + SiteUrl + " - " + item.ListName + " - " + item.ItemID.Value);
                    WriteToFile("   " + item.TableName + " - " + item.DetailName + " - " + item.TransID);

                    isError = true;
                    errorMessage = ex.ToString();
                }
                finally
                {
                    db.CloseConnection(ref conn, isTrans);
                }

                if (isError)
                {
                    isTrans = true;
                    db.OpenConnection(ref conn, isTrans);
                    try
                    {
                        #region Update AutoCodeBatch if error
                        db.cmd.CommandText = "usp_AutoCodeBatch_UpdateFlag";
                        db.cmd.CommandType = CommandType.StoredProcedure;

                        db.cmd.Parameters.Clear();
                        db.AddInParameter(db.cmd, "ID", item.ID);
                        db.AddInParameter(db.cmd, "Generated", 2);
                        db.AddInParameter(db.cmd, "SysMessage", errorMessage);

                        db.cmd.ExecuteNonQuery();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        isTrans = false;
                        WriteToFile(DateTime.Now.ToString("dd MMM yyyy HH:mm:ss tt") + " - " + ex.Message);
                    }
                    finally
                    {
                        db.CloseConnection(ref conn, isTrans);
                    }
                }
                else
                {
                    #region Start Workflow
                    //var StartData = new
                    //{
                    //    se_headerid = item.ItemID,
                    //    se_tablename = item.TableName
                    //};

                    //var NWCParamModel = new
                    //{
                    //    StartData
                    //};

                    //var NintexWorkflowCloud = new
                    //{
                    //    url = "https://daikin.workflowcloud.com/api/v1/workflow/published/e13eb321-e44c-4242-a21e-ae024e620017/swagger.json?token=CL9crPPiU0OJMX16I2Imuqm61SlxPsXNYNw4zePxxT3l0K1j5umw8PIafK5BpoYpL4fitN",
                    //    NWCParamModel
                    //};

                    StartData startData = new StartData();
                    startData.se_headerid = (int)item.ItemID;
                    startData.se_tablename = item.TableName;

                    NWCParamModel nwcParamModel = new NWCParamModel();
                    nwcParamModel.startData = startData;

                    //string token = Program.GetToken();
                    NintexWorkflowCloud nwc = new NintexWorkflowCloud();
                    nwc.url = "https://au.nintex.io/workflows/v1/designs/e13eb321-e44c-4242-a21e-ae024e620017/instances";
                    nwc.param = nwcParamModel;

                    //Task.Run(async () => { await new Program().StartNWC(nwc); }).Wait();
                    //StartWorkflowBySystemAccount(nwc);
                    #endregion
                }

            }

            return count + " of " + total;
        }

        public static void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\SchedulerLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        public async static void StartWorkflowBySystemAccount(NintexWorkflowCloud nwc)
        {
            try
            {
                //string sBody = JsonConvert.SerializeObject(nwc.param);
                string sBody = new JavaScriptSerializer().Serialize(nwc.param);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(nwc.url);

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT Header

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, nwc.url);

                //HttpContent request1 = new StringContent(sBody, Encoding.UTF8, "application/json");
                //string jsonContent = request1.ReadAsStringAsync().Result;

                request.Content = new StringContent(sBody, Encoding.UTF8, "application/json");

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    //return result; //instance guid
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task StartNWC(NintexWorkflowCloud nwc)
        {

            string sBody = new JavaScriptSerializer().Serialize(nwc.param);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(nwc.url);

            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT Header
            //string token = Program.GetToken();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, nwc.url);

            request.Content = new StringContent(sBody, Encoding.UTF8, "application/json");

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                //return result; //instance guid
            }

        }

        public void InsertApprovalLog(string listName, int listItemID, int action, string CurrentLogin, string CurrentLoginName, string currentLayer)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "[dbo].[usp_NWC_insertHistoryLog]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                db.AddInParameter(db.cmd, "listName", listName);
                db.AddInParameter(db.cmd, "listItemID", listItemID);
                db.AddInParameter(db.cmd, "action", action);
                db.AddInParameter(db.cmd, "currentLogin", CurrentLogin);
                db.AddInParameter(db.cmd, "currentLoginName", CurrentLoginName);
                db.AddInParameter(db.cmd, "currentLayer", currentLayer);

                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn);
            }

            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public static string GetToken()
        {
            string url = "https://us.nintex.io/authentication/v1/token";

            HttpClient client = new HttpClient();
            var requestBody = new
            {
                client_id = "bd3797e5-2c76-4834-90f1-70060fe10844",
                client_secret = "sKtUsKRNNtWsJLROQtUsPMtWVsMNtRTsLOtWRWsPtVsRtRsNtSPsMLItT2IsRtVsFRtTsNtUsFMOtUsOFtRsQRJFtT2utUsItRsOtSVsO2N",
                grant_type = "client_credentials"
            };
            var jsonBody = new JavaScriptSerializer().Serialize(requestBody);
            var HttpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, HttpContent).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var responseObject = new JavaScriptSerializer().Deserialize<dynamic>(responseJson);
            Console.WriteLine(responseObject);
            string accessToken = responseObject["access_token"];

            return accessToken;
        }

    }
}
