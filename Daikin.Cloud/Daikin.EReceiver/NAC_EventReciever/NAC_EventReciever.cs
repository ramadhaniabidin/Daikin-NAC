using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Data;
using System.Collections.Generic;
using Daikin.ERecieverLogics.Common;
using Daikin.EReceiverLogics.Common.Model;
using System.Runtime.InteropServices;

namespace Daikin.EReceiver.NAC_EventReciever
{
    /// <summary>
    /// List Item Events
    /// </summary>
    /// 
    [Guid("20070215-0D7E-4F93-A9F2-FBF38478F05E")]
    public class NAC_EventReciever : SPItemEventReceiver
    {
        CommonLogic func = new ERecieverLogics.Common.CommonLogic();

        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            var log = new EventReceiverLogModel();

            log.ListItemId = 0;
            log.ListName = "TEST";
            log.SysMessage = "SUCCESS!";
            log.EventName = "ITEM ADDED";
            log.CompiledQuery = "";


            func.InsertEventReceiverLog(log);

            #region SaveHeader

            //string ListName = properties.List.Title;

            //int ID = properties.ListItem.ID;
            //string fullQuery = string.Empty;
            //int lastloop = 0;
            //string lastcol = "";
            //string tblType = "";

            //// temp var utk query Details yg sedang dijalankan
            //var detailQuery = "";

            //try
            //{
            //    SPListItem listItem = properties.ListItem;
            //    DataTable dt = func.GetMappingAttributes(ListName);
            //    DataTable dtAttrDetails = func.GetMappingAttributeDetails(ListName);

            //    string sql = string.Empty;
            //    List<string> values = new List<string>();

            //    int i = 0;
            //    tblType = "Table Header";
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        lastloop = i;
            //        lastcol = Utility.GetStringValue(row, "Sharepoint_Column_Name");

            //        int Sharepoint_Column_Type = Utility.GetIntValue(row, "Sharepoint_Column_Type");
            //        if (i == 0)
            //        {
            //            string SQLTableName = Utility.GetStringValue(row, "SQLTableName");
            //            sql = "insert into " + SQLTableName;
            //            sql += "(" + Utility.GetStringValue(row, "Database_Column_Name");
            //        }
            //        else
            //        {
            //            sql += "," + Utility.GetStringValue(row, "Database_Column_Name");
            //        }
            //        #region Values
            //        if (Sharepoint_Column_Type == 1 || Sharepoint_Column_Type == 2 || Sharepoint_Column_Type == 3) //Single or Multiple Line of Text or Number
            //        {
            //            values.Add(listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")] == null ?
            //                     "NULL " : "'" + listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")] + "'");
            //        }
            //        else if (Sharepoint_Column_Type == 4) //datetime
            //        {

            //            DateTime dateTime = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")] == null ?
            //                   DateTime.Parse("1900-01-01") : (DateTime)listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")];

            //            values.Add("'" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");

            //        }
            //        else if (Sharepoint_Column_Type == 6) //Yes or No
            //        {
            //            if (listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")].ToString().ToUpper() == "TRUE")
            //            {
            //                values.Add("1");
            //            }
            //            else
            //            {
            //                values.Add("0");
            //            }
            //        }
            //        else if (Sharepoint_Column_Type == 7 || Sharepoint_Column_Type == 5) //Person or Group || Lookup
            //        {
            //            //string originalValue = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")].ToString();
            //            //Console.WriteLine(originalValue);
            //            string currentValue = "";
            //            var gets = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")]?.ToString().Split('#');
            //            if (gets?.Length > 1)
            //            {
            //                // Temporary bypass
            //                currentValue = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")]?.ToString().Split('#')[1];
            //                values.Add("'" + currentValue + "'");
            //            }
            //            else
            //            {
            //                values.Add("NULL");
            //            }

            //            //string currentValue = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")].ToString().Split('#')[1];
            //            //SPUser user = properties.Web.Users.GetByID(new SPFieldLookupValue(currentValue).LookupId);
            //        }
            //        else if (Sharepoint_Column_Type == 9) // Choice
            //        {
            //            string currVal = Utility.GetStringValue(row, "Sharepoint_Column_Name")?.ToString();
            //            if (!string.IsNullOrWhiteSpace(currVal))
            //            {
            //                values.Add($"'{currVal}'");
            //            }
            //            else
            //            {
            //                values.Add("NULL");
            //            }
            //        }
            //        #endregion
            //        i++;
            //    }

            //    sql += ")";

            //    string combineValues = string.Join(",", values);

            //    fullQuery = sql + " VALUES (" + combineValues + ")";
            //    detailQuery = fullQuery;

            //    lastloop = 0;
            //    tblType = "Execute Query Header";
            //    if (i > 0)
            //    {
            //        func.ExecQuery(fullQuery);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    #region Log EventReceivers
            //    var inner = ex.InnerException;
            //    log.EventName = "ItemAdded";
            //    log.ListItemId = ID;
            //    log.ListName = ListName;
            //    log.SysMessage = ex.GetType().Name + " : " + ex.Message + " | Stopped at " + (tblType == "Table Header" ? ("column " + lastcol + " at ") : "") + tblType + " (loop: " + lastloop + ")";
            //    log.CompiledQuery = !string.IsNullOrWhiteSpace(detailQuery) ? detailQuery : fullQuery;

            //    //while (inner != null)
            //    //{
            //    //    log.SysMessage += $"||(Inner){ex.GetType().Name}, {ex.Message}. Source : {ex.Source}";
            //    //    inner = inner.InnerException;
            //    //}

            //    func.InsertEventReceiverLog(log);
            //    #endregion
            //    throw ex;

            //    //#if DEBUG
            //    //                Debug.WriteLine($"{GetType().Name}.{nameof(ItemAdded)}() {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff")} | {ex.GetType().Name}, {ex.Message}\nSource : {ex.Source}\nStacktrace : {ex.StackTrace}\n--- END OF STACKTRACE---\n");
            //    //#endif
            //}
            #endregion SaveHeader

        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            var log = new EventReceiverLogModel();

            log.ListItemId = 0;
            log.ListName = "TEST";
            log.SysMessage = "UPDATED!";
            log.EventName = "ITEM UPDATED";
            log.CompiledQuery = "";


            func.InsertEventReceiverLog(log);
        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }

        /// <summary>
        /// An attachment was added to the item.
        /// </summary>
        public override void ItemAttachmentAdded(SPItemEventProperties properties)
        {
            base.ItemAttachmentAdded(properties);
        }

        /// <summary>
        /// An attachment was removed from the item.
        /// </summary>
        public override void ItemAttachmentDeleted(SPItemEventProperties properties)
        {
            base.ItemAttachmentDeleted(properties);
        }


    }
}