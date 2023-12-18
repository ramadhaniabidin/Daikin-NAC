using Daikin.API.Areas.Common.Models;
using Microsoft.SharePoint;
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
                //Create the client context object and set the credentials  
                string url = System.Configuration.ConfigurationManager.AppSettings["SP:url"].ToString();
                string username = System.Configuration.ConfigurationManager.AppSettings["SP:username"].ToString();
                string password = System.Configuration.ConfigurationManager.AppSettings["SP:password"].ToString();
                string domain = System.Configuration.ConfigurationManager.AppSettings["SP:domain"].ToString();
                //ClientContext clientContext = new ClientContext(url);
                //SecureString securePassword = new SecureString();
                //foreach (char c in password.ToCharArray()) securePassword.AppendChar(c);

                //clientContext.Credentials = new NetworkCredential(url, securePassword, domain);
                //clientContext.AuthenticationMode = ClientAuthenticationMode.Default;

                //List oList = clientContext.Web.Lists.GetByTitle(model.ListName);
                //ListItem oListItem = oList.GetItemById(model.ListItemId);
                //oListItem["Form_x0020_Status"] = model.FormStatus;
                //oListItem["Workflow_x0020_Status"] = model.WorkflowStatus;
                //oListItem["Approval_x0020_Status"] = model.ApprovalStatus;
                //oListItem.Update();
                //clientContext.ExecuteQuery();


                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPList list = web.Lists[model.ListName];

                            SPListItem oListItem = list.GetItemById(model.ListItemId);
                            oListItem["Form_x0020_Status"] = model.FormStatus;
                            oListItem["Workflow_x0020_Status"] = model.WorkflowStatus;
                            oListItem["Approval_x0020_Status"] = model.ApprovalStatus;
                            oListItem.Update();
                            web.AllowUnsafeUpdates = false;

                        }
                    }
                });


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