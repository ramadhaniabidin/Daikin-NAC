using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Daikin.BusinessLogics.Common
{
    public class Aprroval
    {
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
        public static IEnumerable<dynamic> GetTasks()
        {
            string url = "https://us.nintex.io/workflows/v1/tasks";
            string token = GetToken();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = client.GetAsync(url).Result;

            var responseJson = response.Content.ReadAsStringAsync().Result;

            dynamic responseObject = new JavaScriptSerializer().Deserialize<dynamic>(responseJson);

            var tasks = responseObject["tasks"];

            return tasks;
        }

        //public static string GetTaskAndAssignmentID(int item_id)
        //{
        //    try
        //    {
        //        //string token = controller.GetToken();
        //        var tasks = GetTasks();
        //        Console.WriteLine(JsonConvert.SerializeObject(tasks));
        //        var task = tasks.FirstOrDefault(t => t.name.ToString().Contains($"{item_id}"));

        //        string task_id = Convert.ToString(task["id"]);
        //        string assignmentID = Convert.ToString(task["taskAssignments"][0]["id"]);
        //        var result = new
        //        {
        //            ProcessSuccess = true,
        //            InfoMessage = "OK",
        //            task_id = task_id,
        //            assignment_id = assignmentID
        //        };

        //        return new JavaScriptSerializer().Serialize(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        var result = new
        //        {
        //            ProcessSuccess = false,
        //            InfoMessage = ex.Message
        //        };
        //        return new JavaScriptSerializer().Serialize(result);
        //    }
        //}


    }
}
