using Daikin.BusinessLogics.Apps.ClaimReimbursement.Controller;
using Daikin.BusinessLogics.Apps.ClaimReimbursement.Model;
using Daikin.BusinessLogics.Common;
using Daikin.BusinessLogics.Common.Model;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Daikin.Application.Layouts.Daikin.Application.WebServices
{
    [WebService(Namespace = "http://spdev:3473/",
                Description = "This is a demonstration WebService.")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class PopList : WebService
    {
        ClaimReimbursementController ctrlGen = new ClaimReimbursementController();
        SharePointManager sp = new SharePointManager();

        string sqlConnection = "Data Source=SPDEV;Initial Catalog=Daikin_Nintex_Dev;User ID=sa;Password=7CxNsjEhHcWRgd*4FNY^";

        [WebMethod]
        public string PopUpListData(PopList_Input input, PopList_Output output)
        {
            try
            {
                var Logs = new PopListController(sqlConnection).ListData(input, output);
                var result = new
                {
                    Logs = Logs,
                    TotalRecords = output.RecordCount,
                    TotalPages = Math.Ceiling((double)output.RecordCount / input.pageSize),
                    ProcessSuccess = true
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

        //[WebMethod]
        //public string ListData(FilterHeaderSearchModel model)
        //{
        //    try
        //    {
        //        int RecordCount = 0;
        //        decimal GrandTotal = 0;
        //        string CurrentLoginWithoutPattern = sp.GetCurrentUserLogin(SPContext.Current.Site.Url, false);
        //        string CurrentLogin = "";
        //        CurrentLogin = sp.GetCurrentUserLogin(SPContext.Current.Site.Url);
        //        model.CurrentLogin = CurrentLogin;
        //        var items = ctrlGen.ListData(model, out RecordCount, out GrandTotal);

        //        var result = new
        //        {
        //            ProcessSuccess = true,
        //            InfoMessage = "OK",
        //            Items = items,
        //            RecordCount = RecordCount,
        //            GrandTotal = GrandTotal,
        //            PageIndex = model.PageIndex,
        //            PageSize = 10,
        //            CurrentLogin = CurrentLogin,
        //            model = model,
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

        [WebMethod]
        public string GetHistoryLog(string Form_No, string Module_Code)
        {
            try
            {
                var Logs = ctrlGen.GetHistoryLog(Form_No, Module_Code);
                var result = new
                {
                    ProcessSuccess = true,
                    InfoMessage = "OK",
                    Logs = Logs
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

        [WebMethod]
        public string GetHistoryLogByTransactionID(int Transaction_ID, string Module_Code)
        {
            try
            {
                var Logs = ctrlGen.GetHistoryLogByTransaction_ID(Transaction_ID, Module_Code);
                var result = new
                {
                    ProcessSuccess = true,
                    InfoMessage = "OK",
                    Logs = Logs
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

        [WebMethod]
        public string ModuleOptions(string ListName)
        {
            try
            {
                var items = ctrlGen.ModuleOptions(ListName);
                var listBranch = new CommonLogic().DropDownLists("MasterBusinessArea",1, "All","", "Code","Branches");
                SPUser user = SPContext.Current.Web.CurrentUser;
                int SPUserId = user.ID;

                //List<string> listBranchName = sp.GetBranchFinance(SPContext.Current.Web, SPUserId, "Branch");

                List<string> listBranchName = new Utility().GetMappingBranchCurrentLogin(user.LoginName);
                //var listApproverRole = sp.BindingMasterApproverSPListCondition(ListName);


                string Branch = "";
                if (listBranchName.Count > 0)
                {
                    Branch = listBranchName[0];
                }
                else
                {
                    Branch = new Utility().GetBranchCurrentLogin(user.LoginName);
                    if (!string.IsNullOrEmpty(Branch)) listBranch = listBranch.Where(w => w.Code == Branch).ToList();
                }

                var result = new
                {
                    ProcessSuccess = true,
                    InfoMessage = "OK",
                    Items = items,
                    listBranch = listBranch,
                    Branch = Branch,
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
