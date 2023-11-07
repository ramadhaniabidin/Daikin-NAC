using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Daikin.BusinessLogics.Common
{
    public class Approval
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
            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var HttpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, HttpContent).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseJson);
            string accessToken = responseObject.access_token;


            return accessToken;
        }

        public IEnumerable<dynamic> GetTasks()
        {
            string url = "https://us.nintex.io/workflows/v2/tasks";
            string token = GetToken();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = client.GetAsync(url).Result;

            var responseJson = response.Content.ReadAsStringAsync().Result;

            dynamic responseObject = JsonConvert.DeserializeObject(responseJson);

            var tasks = responseObject.tasks;

            return tasks;
        }
    }
}
