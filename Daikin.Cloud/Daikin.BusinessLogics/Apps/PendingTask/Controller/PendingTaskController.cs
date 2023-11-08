using Daikin.BusinessLogics.Apps.PendingTask.Model;
using Daikin.BusinessLogics.Common;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daikin.BusinessLogics.Apps.PendingTask.Controller
{
    public class PendingTaskController
    {
        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        SqlDataReader reader = null;
        DataTable dt = new DataTable();

        public List<PendingTaskModel> GetPendingTaskList(string SiteUrl, SPWeb oWeb, string CurrentLogin)
        {
            dt = new DataTable();
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_NWC_ListPendingApproval";
                db.cmd.CommandType = CommandType.StoredProcedure;

                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "CurrentUserLogin", CurrentLogin);
                
                reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);

                return dt.Rows.Count > 0 ? Utility.ConvertDataTableToList<PendingTaskModel>(dt) : new List<PendingTaskModel>();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
