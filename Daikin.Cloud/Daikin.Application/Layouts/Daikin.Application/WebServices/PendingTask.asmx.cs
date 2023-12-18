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
using Daikin.BusinessLogics.Apps.PendingTask.Controller;
using Daikin.BusinessLogics.Apps.ClaimReimbursement.Model;

namespace Daikin.Application.Layouts.WebServices {
    [WebService(Namespace = "http://spdev:3473/",
                Description = "This is a demonstration WebService.")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    [ScriptService]
    public class PendingTask : WebService {
    CommonLogic func = new BusinessLogics.Common.CommonLogic();
    PendingTaskController pta = new PendingTaskController();

    [WebMethod]
    public string GetCurrentLogin()
    {
        SPWeb oWeb = SPContext.Current.Web;
        string CurrentLogin = oWeb.CurrentUser.Name;
        return CurrentLogin;
    }

    [WebMethod]
    public string TaskApprovalByCurrentLogin(FilterHeaderSearchModel model)
    {
        try
        {
            SPWeb oWeb = SPContext.Current.Web;
            string SiteUrl = SPContext.Current.Site.Url;
                
            string CurrentLogin = oWeb.CurrentUser.Name;
                int RecordCount = 0;
            var data = pta.GetPendingTaskList(SiteUrl, oWeb, CurrentLogin, model, out RecordCount);

            var result = new
            {
                ProcessSuccess = true,
                InfoMessage = "OK",
                RecordCount = RecordCount,
                CurrentLogin = CurrentLogin,
                SiteUrl = SiteUrl,
                //datas = datas,
                data = data,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,

            };

            return new JavaScriptSerializer().Serialize(result);

        }
        catch (Exception ex)
        {
            var result = new
            {
                ProcessSuccess = false,
                InfoMessage = ex.Message,
            };
            return new JavaScriptSerializer().Serialize(result);
        }
    }



    }
}
