using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Daikin.BusinessLogics.Common
{
    public class NintexCloudManager
    {
        public string GetToken()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["NAC:token_url"].ToString();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();

                var requestBody = new
                {
                    client_id = System.Configuration.ConfigurationManager.AppSettings["NAC:client_id"].ToString(),
                    client_secret = System.Configuration.ConfigurationManager.AppSettings["NAC:client_secret"].ToString(),
                    grant_type = System.Configuration.ConfigurationManager.AppSettings["NAC:grant_type"].ToString()
                };

                var jsonBody = new JavaScriptSerializer().Serialize(requestBody);
                var HttpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, HttpContent).Result;
                var responseJson = response.Content.ReadAsStringAsync().Result;
                var responseObject = new JavaScriptSerializer().Deserialize<dynamic>(responseJson);
                string accessToken = responseObject.access_token;


                return accessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IEnumerable<dynamic> GetTasks()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["NAC:task_url"].ToString();
                string token = GetToken();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var response = client.GetAsync(url).Result;

                var responseJson = response.Content.ReadAsStringAsync().Result;

                dynamic responseObject = new JavaScriptSerializer().Deserialize<dynamic>(responseJson);

                var tasks = responseObject.tasks;

                return tasks;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string ApproveRequest(string approval_value, string task_id, string assignment_id)
        {

            try
            {
                string token = GetToken();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var payload = new { outcome = approval_value };
                var jsonPayload = new JavaScriptSerializer().Serialize(payload);
                var stringContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                string url = $"https://us.nintex.io/workflows/v2/tasks/{task_id}/assignments/{assignment_id}";


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
