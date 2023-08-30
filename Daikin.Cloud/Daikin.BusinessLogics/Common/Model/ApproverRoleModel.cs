using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daikin.BusinessLogics.Common.Model
{
    public class ApproverRoleModel
    {
        public int Position_ID { get; set; }
        public string Position_Name { get; set; }
        public int Order_ID { get; set; }
        public string Module_Code { get; set; }
    }
}