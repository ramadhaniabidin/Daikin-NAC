using Daikin.API.Areas.Common.Models;
using Daikin.APILogics.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Daikin.API.Areas.Common.Controllers
{
    [System.Web.Http.RoutePrefix("api/SAPBatch")]
    public class SAPBatchController : ApiController
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
        [System.Web.Http.Route("CreateBatchFile")]
        public IHttpActionResult CreateBatchFile([FromBody]SAPBatchModel model)
        {
            try
            {
                SAPBatchLogic ctrl = new SAPBatchLogic();
                ctrl.CreateBatchFile(model.SAPFolderID, model.headerID, model.fileName);
                var result = new
                {
                    ProcessSuccess = true,
                    InfoMessage = "OK"
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    ProcessSuccess = false,
                    InfoMessage = ex.Message
                };
                return Ok(result);
            }
        }

    }
}