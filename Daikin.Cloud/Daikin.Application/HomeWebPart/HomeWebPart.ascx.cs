using Daikin.BusinessLogics.Apps.ClaimReimbursement.Model;
using Daikin.BusinessLogics.Apps.PendingTask.Controller;
using Daikin.BusinessLogics.Apps.PendingTask.Model;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Daikin.Application.HomeWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class HomeWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public HomeWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    int RecordCount = 0;
                    List<PendingTaskModel> lists = new List<PendingTaskModel>();
                    FilterHeaderSearchModel model = new FilterHeaderSearchModel();
                    model.PageIndex = 1;
                    model.PageSize = 10000;
                    lists = new PendingTaskController().GetPendingTaskList(SPContext.Current.Site.Url, SPContext.Current.Web,
                        SPContext.Current.Web.CurrentUser.Name, model, out RecordCount);
                    TotalPendingTaskList.InnerHtml = RecordCount.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
