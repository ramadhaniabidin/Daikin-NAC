using Daikin.BusinessLogics.Apps.ClaimReimbursement.Model;
using Daikin.BusinessLogics.Common.Model;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daikin.BusinessLogics.Common
{
    public class CommonLogic
    {
        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();

        #region Daikin Used

        public void InsertLog(LogModel log)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_EventReceiverLog_Insert";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                db.AddInParameter(db.cmd, "ListName", log.ListName);
                db.AddInParameter(db.cmd, "ListItemId", log.ListItemId);
                db.AddInParameter(db.cmd, "SysMessage", log.SysMessage);
                db.AddInParameter(db.cmd, "EventName", log.EventName);
                db.AddInParameter(db.cmd, "CompiledQuery", log.CompiledQuery);

                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn);
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public DataTable GetMappingAttributes(string ListName)
        {
            try
            {
                DataTable dt = new DataTable();

                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_MasterMappingColumn_GetByListName";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ListName", ListName);
                SqlDataReader reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                return dt;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public DataTable GetMappingAttributeDetails(string ListName)
        {
            try
            {
                DataTable dt = new DataTable();

                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_MasterMappingColumnDetail_GetByListName";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ListName", ListName);
                SqlDataReader reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                return dt;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }


        #endregion

        public bool IsDataExists(string TableName, string FieldKey, string FieldValue)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_Utility_CheckExistingData";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                db.AddInParameter(db.cmd, "TableName", TableName);
                db.AddInParameter(db.cmd, "FieldKey", FieldKey);
                db.AddInParameter(db.cmd, "FieldValue", FieldValue);

                SqlDataReader reader = db.cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                return dt.Rows.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public void ExecQuery(string fullQuery)
        {
            try
            {
                db.OpenConnection(ref conn, true);
                db.cmd.CommandText = fullQuery;
                db.cmd.CommandType = CommandType.Text;
                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn, true);
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public int ExecQueryWithReturnID(string fullQuery)
        {
            try
            {
                db.OpenConnection(ref conn, true);
                db.cmd.CommandText = fullQuery;
                db.cmd.CommandType = CommandType.Text;

                SqlDataReader reader = db.cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn, true);

                var id = dt.Rows[0].Field<int>(0);

                return id;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public void InsertAutoCodeBatch(string TableName, string DetailName, int TransID, string LastName, int ItemID, string ColumnName, string Format, int LenghtOfString)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_AutoCodeBatch_Save";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "TableName", TableName);
                db.AddInParameter(db.cmd, "DetailName", DetailName);
                db.AddInParameter(db.cmd, "TransID", TransID);
                db.AddInParameter(db.cmd, "LastName", LastName);
                db.AddInParameter(db.cmd, "ItemID", ItemID);
                db.AddInParameter(db.cmd, "ColumnName", ColumnName);
                db.AddInParameter(db.cmd, "Format", Format);
                db.AddInParameter(db.cmd, "LenghtOfString", LenghtOfString);

                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn);
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public void InsertApprovalLog(string listName, int listItemID, int action, string CurrentLogin, string CurrentLoginName, string currentLayer, string comments)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "[dbo].[usp_NWC_insertHistoryLog]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                db.AddInParameter(db.cmd, "listName", listName);
                db.AddInParameter(db.cmd, "listItemID", listItemID);
                db.AddInParameter(db.cmd, "action", action);
                db.AddInParameter(db.cmd, "currentLogin", CurrentLogin);
                db.AddInParameter(db.cmd, "currentLoginName", CurrentLoginName);
                db.AddInParameter(db.cmd, "currentLayer", currentLayer);
                db.AddInParameter(db.cmd, "comments", comments);

                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn);
            }

            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public DataTable GetMappingAdditionalSP(string ListName)
        {
            try
            {
                DataTable dt = new DataTable();

                db.OpenConnection(ref conn);
                db.cmd.CommandText = "[dbo].[usp_AdditionalSP_GetByListName]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ListName", ListName);
                SqlDataReader reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                return dt;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public void ExecAdditionalSP(int ListItemID, string SPName)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = SPName;
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ListItemID", ListItemID);

                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn);
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public void SaveUpdateAttachments(AttachmentModel attc, bool Insert)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_Attachment_Insert";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                db.AddInParameter(db.cmd, "module_code", attc.module_code);
                db.AddInParameter(db.cmd, "transaction_code", attc.transaction_code);
                db.AddInParameter(db.cmd, "list_name", attc.list_name);
                db.AddInParameter(db.cmd, "list_item_id", attc.list_item_id);
                db.AddInParameter(db.cmd, "file_name", attc.file_name);
                db.AddInParameter(db.cmd, "file_url", attc.file_url);
                db.AddInParameter(db.cmd, "uploaded_by", attc.uploaded_by);

                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn);
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public List<OptionModel> DropDownLists(string Table, int NeedFirstOption, string FirstDescription, string FirstValue, string FieldCode, string FieldName)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_Utility_DropDowns";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "Table", Table);
                db.AddInParameter(db.cmd, "NeedFirstOption", NeedFirstOption);
                db.AddInParameter(db.cmd, "FirstDescription", FirstDescription);
                db.AddInParameter(db.cmd, "FirstValue", FirstValue);
                db.AddInParameter(db.cmd, "FieldCode", FieldCode);
                db.AddInParameter(db.cmd, "FieldName", FieldName);

                SqlDataReader reader = db.cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                db.CloseConnection(ref conn);
                return Utility.ConvertDataTableToList<OptionModel>(dt);
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }


        public void AutoCodeBatch(string ListName, int ItemID, int TransID)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_AutoCode_Insert";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ListName", ListName);
                db.AddInParameter(db.cmd, "ItemID", ItemID);
                db.AddInParameter(db.cmd, "TransID", TransID);

                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn);
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
        
        public string GetModuleCode(string ListName)
        {
            try
            {
                db.OpenConnection(ref conn, true);
                db.cmd.CommandText = "usp_MasterModule_GetModuleCode";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.AddInParameter(db.cmd, "List_Name", ListName);

                SqlDataReader reader = db.cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn, true);

                var Module_Code = dt.Rows[0].Field<string>(0);

                return Module_Code;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }

        public List<CheckApproval> CheckApproval(string ListName)
        {
            var dt = new DataTable();
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "usp_Approval_CheckApprovalbyListName";
                db.cmd.CommandType = CommandType.StoredProcedure;

                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ListName", ListName);
                //db.AddInParameter(db.cmd, "TransactionId", TransactionID);

                SqlDataReader reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);

                for (var colIdx = 0; colIdx < dt.Columns.Count; colIdx++)
                {
                    if (dt.Rows[0][colIdx] == System.DBNull.Value)
                    {
                        dt.Rows[0][colIdx] = "";
                    }
                }

                var approvalCheck = Utility.ConvertDataTableToList<CheckApproval>(dt);
                return approvalCheck;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.CloseConnection(ref conn);
            }
        }

        public List<GetApprovalData> GetApprovalData(string ModuleCode, int ListItemID, int HeaderID)
        {
            var dt = new DataTable();
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "usp_Approval_GetData";
                db.cmd.CommandType = CommandType.StoredProcedure;

                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ModuleCode", ModuleCode);
                db.AddInParameter(db.cmd, "ListItemID", ListItemID);
                db.AddInParameter(db.cmd, "HeaderID", HeaderID);

                SqlDataReader reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);

                if (dt.Rows.Count > 0)
                {
                    for (var rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                    {
                        for (var colIdx = 0; colIdx < dt.Columns.Count; colIdx++)
                        {
                            if (dt.Rows[rowIdx][colIdx] == System.DBNull.Value)
                            {
                                dt.Rows[rowIdx][colIdx] = "";
                            }
                        }
                    }
                }

                var approvalData = Utility.ConvertDataTableToList<GetApprovalData>(dt);
                return approvalData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.CloseConnection(ref conn);
            }
        }

        public void ApprovalLog(string ModuleCode, int HeaderID, string FormNo, string PICName, string PICUsername, DateTime ActionDate, string PICRole, string ApproverGroup)
        {
            try
            {
                db.OpenConnection(ref conn);

                db.cmd.CommandText = "usp_Approval_Log";
                db.cmd.CommandType = CommandType.StoredProcedure;

                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "ModuleCode", ModuleCode);
                db.AddInParameter(db.cmd, "TransactionHeaderID", HeaderID);
                db.AddInParameter(db.cmd, "FormNo", FormNo);
                db.AddInParameter(db.cmd, "ApprovalListItemID", 0);
                db.AddInParameter(db.cmd, "PICName", PICName);
                db.AddInParameter(db.cmd, "PICUsername", PICUsername);
                db.AddInParameter(db.cmd, "ActionID", 0);
                db.AddInParameter(db.cmd, "ActionName", "Submit Revise");
                db.AddInParameter(db.cmd, "ActionDate", ActionDate);
                db.AddInParameter(db.cmd, "Comments", "");
                db.AddInParameter(db.cmd, "CurrentLayer", 0);
                db.AddInParameter(db.cmd, "PICRole", PICRole);
                db.AddInParameter(db.cmd, "ApproverGroup", ApproverGroup);

                db.cmd.ExecuteNonQuery();
                db.CloseConnection(ref conn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GeneralHistoryLogModel> GetHistoryLog(string Form_No, string ModuleCode)
        {

            var dt = new DataTable();
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.usp_Utility_GetHistoryLogByTransId";
                db.cmd.CommandType = CommandType.StoredProcedure;

                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "Form_No", Form_No);
                db.AddInParameter(db.cmd, "Module_Code", ModuleCode);

                SqlDataReader reader = db.cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                db.CloseDataReader(reader);

                db.CloseConnection(ref conn);
                return Utility.ConvertDataTableToList<GeneralHistoryLogModel>(dt);


            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
    }
}
