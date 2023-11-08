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
    [System.Web.Http.RoutePrefix("api/Sharepoint")]
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
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("UpdateStatus")]
        public IHttpActionResult UpdateStatus([FromBody]CommonUpdateModel model)
        {
            try
            {
                string password = "Mq151k.$rqUd";
                //Create the client context object and set the credentials  
                ClientContext clientContext = new ClientContext("http://spdev:3473/");
                SecureString securePassword = new SecureString();
                foreach (char c in password.ToCharArray()) securePassword.AppendChar(c);
                //clientContext.Credentials = new SharePointOnlineCredentials("nintex2021@daikin.co.id", securePassword);
                clientContext.Credentials = new NetworkCredential("nintex2021@daikin.co.id", securePassword);

                List oList = clientContext.Web.Lists.GetByTitle(model.ListName);
                ListItem oListItem = oList.GetItemById(model.ListItemId);
                oListItem["Form_x0020_Status"] = model.FormStatus;
                oListItem["Workflow_x0020_Status"] = model.WorkflowStatus;
                oListItem["Approval_x0020_Status"] = model.ApprovalStatus;
                oListItem.Update();
                clientContext.ExecuteQuery();

                var obj = new
                {
                    ProcessSuccess = true,
                    InfoMessage = "Success",
                    Model = model
                };
                return Ok(obj);

            }
            catch (Exception ex)
            {
                var obj = new
                {
                    ProcessSuccess = false,
                    InfoMessage = ex.Message,
                };
                return Ok(obj);
            }
        }
    }
}