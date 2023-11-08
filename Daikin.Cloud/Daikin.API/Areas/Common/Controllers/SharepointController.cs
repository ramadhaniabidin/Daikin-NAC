using Daikin.API.Areas.Common.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;

namespace Daikin.API.Areas.Common.Controllers
{
    public class SharepointController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public void UpdateStatus(CommonUpdateModel model)
        {
            try
            {
                string password = "Aircon123";
                //Create the client context object and set the credentials  
                ClientContext clientContext = new ClientContext("http://spdev:3473/");
                SecureString securePassword = new SecureString();
                foreach (char c in password.ToCharArray()) securePassword.AppendChar(c);
                clientContext.Credentials = new SharePointOnlineCredentials("lrosandy@daikin.co.id", securePassword);

                List oList = clientContext.Web.Lists.GetByTitle("Announcements");
                ListItem oListItem = oList.GetItemById(7);
                oListItem["Title"] = "Custom Title updated Programmatically.";
                oListItem.Update();
                clientContext.ExecuteQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error occured " + ex.Message);
                Console.ReadLine();
            }
        }
    }
}