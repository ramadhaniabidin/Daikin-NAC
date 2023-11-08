using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Daikin.API.Areas.Common.Models
{
    public class SharepointModel
    {
    }

    public class CommonUpdateModel
    {
        public string ListName { get; set; }
        public int ListItemId { get; set; }
        public string ApprovalStatus { get; set; }
        public string WorkflowStatus { get; set; }
        public string FormStatus { get; set; }
    }
}