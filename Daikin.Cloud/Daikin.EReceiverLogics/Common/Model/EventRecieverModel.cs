using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daikin.EReceiverLogics.Common.Model
{
    public class EventReceiverLogModel
    {
        public string ListName { get; set; }
        public int ListItemId { get; set; }
        public string EventName { get; set; }
        public string CompiledQuery { get; set; }
        public string SysMessage { get; set; }
    }

    public class CheckApproval
    {
        public string ModuleCode { get; set; }
        public string ListName { get; set; }
        public string SQLTableName { get; set; }
        public string ViewPageUrl { get; set; }
        public bool RequireApproval { get; set; }
    }

    public class GetApprovalData
    {
        public string ModuleCode { get; set; }
        public string ListName { get; set; }
        public string SQLTableName { get; set; }
        public string ViewPageUrl { get; set; }
        public bool RequireApproval { get; set; }
        public string FormNo { get; set; }
        public int HeaderID { get; set; }
        public int ListID { get; set; }
        public string CreatedUserName { get; set; }
        public string CreatedName { get; set; }
        public string CreatedRole { get; set; }
        public string CreatedEmail { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApproverGroup { get; set; }
    }
}
