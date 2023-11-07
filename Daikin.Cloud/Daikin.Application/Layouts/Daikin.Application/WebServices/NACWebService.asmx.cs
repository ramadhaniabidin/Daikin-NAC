using Daikin.BusinessLogics.Common;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;
using Daikin.BusinessLogics.Common.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Daikin.Application.Layouts.WebServices {
    [WebService(Namespace = "http://spdev:3473/",
                Description = "This is a demonstration WebService.")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class NACWebService: WebService {
        CommonLogic func = new BusinessLogics.Common.CommonLogic();

        [WebMethod]
        public void SaveUpdate(string ListName, int ListItemID)
        {
            SPWeb web = new SPSite(Utility.SpSiteUrl_DEV).OpenWeb();
            SPList list = web.Lists[ListName];
            web.AllowUnsafeUpdates = true;
            
            //string form_status = "";
            //int ID = 0;

            string fullQuery = string.Empty;
            int lastloop = 0;
            string lastcol = "";
            string tblType = "";
            int header_id = 0;

            // temp var utk query Details yg sedang dijalankan
            var detailQuery = "";
            try
            {
                SPListItem listItem = list.GetItemById(ListItemID);
                DataTable dt = func.GetMappingAttributes(ListName);
                DataTable dtAttrDetails = func.GetMappingAttributeDetails(ListName);

                //get form status && Approval Status
                string formStatus = (String)listItem["Form_x0020_Status"];
                string approvalStatus = (String)listItem["Approval_x0020_Status"];

                string sql = string.Empty;
                List<string> values = new List<string>();

                int i = 0;
                tblType = "Table Header";
                if (formStatus == "Start" &&  (approvalStatus == "" || approvalStatus == null))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lastloop = i;
                        lastcol = Utility.GetStringValue(row, "Sharepoint_Column_Name");

                        int SPColumnType = Utility.GetIntValue(row, "Sharepoint_Column_Type");
                        if (i == 0)
                        {
                            string SQLTableName = Utility.GetStringValue(row, "Table_Header");
                            sql = "insert into " + SQLTableName;
                            sql += "(" + Utility.GetStringValue(row, "Database_Column_Name");
                        }
                        else
                        {
                            sql += "," + Utility.GetStringValue(row, "Database_Column_Name");
                        }

                        values.Add(Utility.getColumnValue(SPColumnType, row, listItem));

                        i++;
                    }

                    sql += ")";

                    string combineValues = string.Join(",", values);

                    fullQuery = sql + " OUTPUT inserted.ID VALUES (" + combineValues + ")";
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lastloop = i;
                        lastcol = Utility.GetStringValue(row, "Sharepoint_Column_Name");

                        int SPColumnType = Utility.GetIntValue(row, "Sharepoint_Column_Type");
                        if (i == 0)
                        {
                            string SQLTableName = Utility.GetStringValue(row, "Table_Header");
                            sql = "update " + SQLTableName;
                            sql += " set " + Utility.GetStringValue(row, "Database_Column_Name") + "=" + Utility.getColumnValue(SPColumnType, row, listItem);
                        }
                        else
                        {
                            sql += ", " + Utility.GetStringValue(row, "Database_Column_Name") + "=" + Utility.getColumnValue(SPColumnType, row, listItem);
                        }

                        i++;
                    }
                    

                    fullQuery = sql + " OUTPUT inserted.ID WHERE Item_ID = " + ListItemID;
                }

                detailQuery = fullQuery;

                lastloop = 0;
                tblType = "Execute Query Header";
                if (i > 0)
                {
                    header_id = func.ExecQueryWithReturnID(fullQuery);
                }

                #region Save XMl Repeationg Section to Table Details
                tblType = "Generating Query Detail";
                i = 0;
                if (dtAttrDetails.Rows.Count > 0)
                {
                    DataView view = new DataView(dtAttrDetails);
                    DataTable distinctValues = view.ToTable(true, "Sharepoint_Column", "Database_Column", "Table_Detail"); //distint berdasarkan SPColumnName & TableName
                    DataTable dtAttrDetailsFilter = dtAttrDetails.Clone(); //clone table distint, nnti buat store data specific DetailCode

                    DefaultAttrModel attr = new DefaultAttrModel();
                    attr.code = listItem["Title"].ToString();
                    attr.item_id = listItem.ID;
                    attr.TableName = Utility.GetStringValue(distinctValues.Rows[1], "Table_Detail");
                    attr.SPColumnName = "Details";
                    attr.XMLString = listItem["Details"].ToString();

                    // dia ngematching di table mapping/filter berdasarkan defined data (TableName, SPColumnname)
                    DataRow[] matchingRows = dtAttrDetails.Select();
                    dtAttrDetailsFilter.Clear();

                    if (!dtAttrDetails.Columns.Contains("ID"))
                    {
                        dtAttrDetailsFilter.Rows.Add("", "ID", "ID");
                    }

                    if (!dtAttrDetails.Columns.Contains("Header_ID"))
                    {
                        dtAttrDetailsFilter.Rows.Add("", "Header_ID", "Header_ID");
                    }

                    //dtAttrDetailsFilter.
                    foreach (DataRow matchingRow in matchingRows)
                    {
                        // dimasukkin ke cloning table di atas, jd isi cloning table itu sama kaya table mappingan cuman udh filtern
                        dtAttrDetailsFilter.ImportRow(matchingRow);
                    }

                    XElement root = XElement.Parse(attr.XMLString);
                    foreach (var item in root.Descendants("Item"))
                    {
                        if (XMLManager.GetValue(item, "ID") == "NOT EXISTS")
                        {
                            XMLManager.AddAttribute(item, "ID", "");
                        }

                        if (XMLManager.GetValue(item, "Header_ID") == "NOT EXISTS")
                        {
                            XMLManager.AddAttribute(item, "Header_ID", header_id.ToString());
                        }

                        if (string.IsNullOrEmpty(XMLManager.GetValue(item, "ID")))
                        {
                            item.Element("ID").Value = Guid.NewGuid().ToString();
                        }
                    }

                    attr.XMLString = root.ToString();
                    List<string> listQueryDetails = XMLManager.ConvertDataTableToSQL(dtAttrDetailsFilter, attr);
                    detailQuery = "DELETE " + attr.TableName + " WHERE Header_ID = " + header_id.ToString();
                    func.ExecQuery(detailQuery);

                    tblType = "Executing Query Detail";
                    foreach (string query in listQueryDetails)
                    {
                        lastloop = i;
                        detailQuery = query;
                        fullQuery = query; //Save Log
                        func.ExecQuery(query);

                        i++;
                        //perlu create log untuk simpen row detail mana yg fail 
                    }
                    detailQuery = "";
                }

                #endregion

                #region InsertAutoCodeBatch
                if (formStatus == "Start" && (approvalStatus == "" || approvalStatus == null))
                {
                    func.AutoCodeBatch(ListName, ListItemID, header_id);
                }
                #endregion

                var successLog = new LogModel()
                {
                    EventName = (formStatus == "Start" && approvalStatus == "")? "Item Added" : "Item Updated",
                    ListItemId = ListItemID,
                    ListName = ListName,
                    SysMessage = (formStatus == "Start" && approvalStatus == "")? ("Insert data success" + ((dtAttrDetails.Rows.Count > 0) ? ", with details." : ".")) : "Update data success",
                    CompiledQuery = ""
                };
                func.InsertLog(successLog);
            }
            catch (Exception ex)
            {
                #region Log EventReceivers
                var inner = ex.InnerException;
                LogModel log = new LogModel();
                log.EventName = "ItemAdded";
                log.ListItemId = ListItemID;
                log.ListName = ListName;
                log.SysMessage = ex.GetType().Name + " : " + ex.Message + " | Stopped at " + (tblType == "Table Header" ? ("column " + lastcol + " at ") : "") + tblType + " (loop: " + lastloop + ")";
                log.CompiledQuery = !string.IsNullOrWhiteSpace(detailQuery) ? detailQuery : fullQuery;

                func.InsertLog(log);
                #endregion
                throw ex;
            }
        }

        [WebMethod]
        public string ApproveRequest(string approvalValue, string ListName, int ListItemID)
        {

            try
            {
                string token = Aprroval.GetToken();
                var tasks = Aprroval.GetTasks();

                Console.WriteLine(JsonConvert.SerializeObject(tasks));
                var task = tasks.FirstOrDefault(t => t.name.ToString().Contains($"{ListItemID}"));

                string task_id = Convert.ToString(task["id"]);
                string assignmentID = Convert.ToString(task["taskAssignments"][0]["id"]);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var payload = new { outcome = approvalValue };
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var stringContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                string url = $"https://us.nintex.io/workflows/v2/tasks/{task_id}/assignments/{assignmentID}";
                
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                request.Content = stringContent;

                var response = client.SendAsync(request).Result;


                var responseBody = response.Content.ReadAsStringAsync();

                var result = new
                {
                    ProcessSuccess = true,
                    InfoMessage = "OK"
                };
                return new JavaScriptSerializer().Serialize(result);
            }

            catch (Exception ex)
            {
                var result = new
                {
                    ProcessSuccess = false,
                    InfoMessage = ex.Message
                };
                return new JavaScriptSerializer().Serialize(result);
            }
        }
    }
}
