using Daikin.BusinessLogics.Common.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Daikin.BusinessLogics.Common
{
    public class Aprroval
    {
        static DatabaseManager db = new DatabaseManager();
        static SqlConnection conn = new SqlConnection();
        static SqlDataReader reader = null;
        static DataTable dt = new DataTable();
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

        public static List<ListDataID> GetListDataID(string ListName, int ItemID)
        {
            dt = new DataTable();
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "usp_GetDataByItemID";
                db.cmd.CommandType = CommandType.StoredProcedure;

                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ListName", ListName);
                db.AddInParameter(db.cmd, "ItemID", ItemID);
                reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);

                var list = Utility.ConvertDataTableToList<ListDataID>(dt);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.CloseConnection(ref conn);
            }
        }

        public static IEnumerable<dynamic> GetTasks(string token, string NAC_Guid)
        {
            string url = "https://au.nintex.io/workflows/v2/tasks?status=active" + "&workflowInstanceId=" + NAC_Guid;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = client.GetAsync(url);

            var responseJson = response.Result.Content.ReadAsStringAsync().Result;

            dynamic responseObject = new JavaScriptSerializer().Deserialize<dynamic>(responseJson);

            var tasks = responseObject["tasks"];

            return tasks;
        }


        //public async Task<object> GetTasks(string token)
        //{
        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = new Uri("https://au.nintex.io/workflows/v2/tasks?status=active"),
        //        Headers =
        //        {
        //            { "Accept", "application/json, application/problem+json" },
        //            { "Authorization", $"Bearer {token}" },
        //        },
        //    };
        //    using (var response = await client.SendAsync(request))
        //    {
        //        response.EnsureSuccessStatusCode();
        //        var body = await response.Content.ReadAsStringAsync();
        //        var responseObject = new JavaScriptSerializer().Deserialize<dynamic>(body);
        //        var tasks = responseObject["tasks"];
        //        return tasks;
        //    }

        //}


    }
}
