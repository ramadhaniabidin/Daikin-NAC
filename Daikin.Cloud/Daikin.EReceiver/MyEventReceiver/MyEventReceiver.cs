using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Daikin.ERecieverLogics.Common;
using System.Runtime.InteropServices;
using Daikin.EReceiverLogics.Common.Model;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using Daikin.EReceiverLogics.Common;

namespace Daikin.EReceiver.MyEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    [Guid("C6847B7A-91B8-4E9E-AE45-F5359A2070EC")]
    public class MyEventReceiver : SPItemEventReceiver
    {
        CommonLogic func = new ERecieverLogics.Common.CommonLogic();
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            string ListName = properties.List.Title;
            string form_status = properties.ListItem["Form_x0020_Status"]?.ToString();

            int ID = properties.ListItem.ID;
            string fullQuery = string.Empty;
            int lastloop = 0;
            string lastcol = "";
            string tblType = "";
            int header_id = 0;

            // temp var utk query Details yg sedang dijalankan
            var detailQuery = "";
            try
            {
                SPListItem listItem = properties.ListItem;
                DataTable dt = func.GetMappingAttributes(ListName);
                DataTable dtAttrDetails = func.GetMappingAttributeDetails(ListName);

                string sql = string.Empty;
                List<string> values = new List<string>();

                int i = 0;
                tblType = "Table Header";
                foreach (DataRow row in dt.Rows)
                {
                    lastloop = i;
                    lastcol = Utility.GetStringValue(row, "Sharepoint_Column_Name");

                    int SPColumnType = Utility.GetIntValue(row, "Sharepoint_Column_Type");
                    if (i == 0)
                    {
                        string SQLTableName = Utility.GetStringValue(row, "Table_Header");
                        sql = "insert into " + SQLTableName;
                        sql += "(" + Utility.GetStringValue(row, "Database_Column_Name");
                    }
                    else
                    {
                        sql += "," + Utility.GetStringValue(row, "Database_Column_Name");
                    }

                    #region Values
                    if (SPColumnType == 1 || SPColumnType == 2 || SPColumnType == 3) //Single or Multiple Line of Text or Number
                    {
                        values.Add(listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")] == null ?
                                 "NULL " : "'" + listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")] + "'");
                    }
                    else if (SPColumnType == 4) //datetime
                    {

                        DateTime dateTime = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")] == null ?
                               DateTime.Parse("1900-01-01") : (DateTime)listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")];

                        values.Add("'" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");

                    }
                    else if (SPColumnType == 6) //Yes or No
                    {
                        if (listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")].ToString().ToUpper() == "TRUE")
                        {
                            values.Add("1");
                        }
                        else
                        {
                            values.Add("0");
                        }
                    }
                    else if (SPColumnType == 7 || SPColumnType == 5) //Person or Group || Lookup
                    {
                        //string originalValue = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")].ToString();
                        //Console.WriteLine(originalValue);
                        string currentValue = "";
                        var gets = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")]?.ToString().Split('#');
                        if (gets?.Length > 1)
                        {
                            // Temporary bypass
                            currentValue = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")]?.ToString().Split('#')[1];
                            values.Add("'" + currentValue + "'");
                        }
                        else
                        {
                            values.Add("NULL");
                        }

                        //string currentValue = listItem[Utility.GetStringValue(row, "Sharepoint_Column_Name")].ToString().Split('#')[1];
                        //SPUser user = properties.Web.Users.GetByID(new SPFieldLookupValue(currentValue).LookupId);
                    }
                    else if (SPColumnType == 9) // Choice
                    {
                        string currVal = Utility.GetStringValue(row, "Sharepoint_Column_Name")?.ToString();
                        if (!string.IsNullOrWhiteSpace(currVal))
                        {
                            values.Add($"'{currVal}'");
                        }
                        else
                        {
                            values.Add("NULL");
                        }
                    }
                    #endregion
                    i++;
                }

                sql += ")";

                string combineValues = string.Join(",", values);

                fullQuery = sql + " OUTPUT inserted.ID VALUES (" + combineValues + ")";
                detailQuery = fullQuery;

                lastloop = 0;
                tblType = "Execute Query Header";
                if (i > 0)
                {
                    header_id = func.ExecQueryWithReturnID(fullQuery);
                }

                #region Save XMl Repeationg Section to Table Details
                tblType = "Generating Query Detail";
                i = 0;
                if (dtAttrDetails.Rows.Count > 0)
                {
                    DataView view = new DataView(dtAttrDetails);
                    DataTable distinctValues = view.ToTable(true, "Sharepoint_Column", "Database_Column", "Table_Detail"); //distint berdasarkan SPColumnName & TableName
                    DataTable dtAttrDetailsFilter = dtAttrDetails.Clone(); //clone table distint, nnti buat store data specific DetailCode

                    foreach (DataRow rowDistinct in distinctValues.Rows) //looping ke each distinct values buat get data (TableName, SPColumnName)
                    {
                        DefaultAttrModel attr = new DefaultAttrModel();
                        // define Table Name, SpColumnName dr current distinct
                        attr.code = listItem["Title"].ToString();
                        attr.item_id = listItem.ID;
                        attr.TableName = Utility.GetStringValue(rowDistinct, "Table_Detail");
                        attr.SPColumnName = "Details";
                        attr.XMLString = listItem[Utility.GetStringValue(rowDistinct, "Sharepoint_Column")].ToString();

                        // dia ngematching di table mapping/filter berdasarkan defined data (TableName, SPColumnname)
                        DataRow[] matchingRows = dtAttrDetails.Select($"Sharepoint_Column = '{attr.SPColumnName}' AND Database_Column = '{attr.TableName}'");
                        dtAttrDetailsFilter.Clear();

                        if (dtAttrDetails.Columns.Contains("ID"))
                        {
                            dtAttrDetailsFilter.Rows.Add("", "ID", "ID");
                        }

                        if (dtAttrDetails.Columns.Contains("Header_ID"))
                        {
                            dtAttrDetailsFilter.Rows.Add("", "Header_ID", "Header_ID");
                        }

                        //dtAttrDetailsFilter.
                        foreach (DataRow matchingRow in matchingRows)
                        {
                            // dimasukkin ke cloning table di atas, jd isi cloning table itu sama kaya table mappingan cuman udh filtern
                            dtAttrDetailsFilter.ImportRow(matchingRow);
                        }

                        XElement root = XElement.Parse(attr.XMLString);
                        foreach (var item in root.Descendants("Item"))
                        {
                            if (XMLManager.GetValue(item, "ID") == "NOT EXISTS")
                            {
                                XMLManager.AddAttribute(item, "ID", "");
                            }

                            if (XMLManager.GetValue(item, "Header_ID") == "NOT EXISTS")
                            {
                                XMLManager.AddAttribute(item, "Header_ID", header_id.ToString());
                            }

                            ////set value
                            //item.Element("Header_ID").Value = header_id.ToString();

                            if (string.IsNullOrEmpty(XMLManager.GetValue(item, "ID")))
                            {
                                item.Element("ID").Value = Guid.NewGuid().ToString();
                            }
                        }

                        attr.XMLString = root.ToString();
                        List<string> listQueryDetails = XMLManager.ConvertDataTableToSQL(dtAttrDetailsFilter, attr);
                        detailQuery = "DELETE " + attr.TableName + " WHERE Header_ID = " + header_id.ToString();
                        func.ExecQuery(detailQuery);

                        #region AutoCode

                        #endregion

                        tblType = "Executing Query Detail";
                        foreach (string query in listQueryDetails)
                        {
                            lastloop = i;
                            detailQuery = query;
                            fullQuery = query; //Save Log
                            func.ExecQuery(query);

                            i++;
                            //perlu create log untuk simpen row detail mana yg fail 
                        }
                    }
                    detailQuery = "";
                }

                #endregion 

                var successLog = new EventReceiverLogModel()
                {
                    EventName = nameof(ItemAdded),
                    ListItemId = ID,
                    ListName = ListName,
                    SysMessage = "Insert data success" + ((dtAttrDetails.Rows.Count > 0) ? ", with details." : "."),
                    CompiledQuery = ""
                };
                func.InsertEventReceiverLog(successLog);
            }
            catch (Exception ex)
            {
                #region Log EventReceivers
                var inner = ex.InnerException;
                EventReceiverLogModel log = new EventReceiverLogModel();
                log.EventName = "ItemAdded";
                log.ListItemId = ID;
                log.ListName = ListName;
                log.SysMessage = ex.GetType().Name + " : " + ex.Message + " | Stopped at " + (tblType == "Table Header" ? ("column " + lastcol + " at ") : "") + tblType + " (loop: " + lastloop + ")";
                log.CompiledQuery = !string.IsNullOrWhiteSpace(detailQuery) ? detailQuery : fullQuery;

                func.InsertEventReceiverLog(log);
                #endregion
                throw ex;
            }
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            EventReceiverLogModel log = new EventReceiverLogModel();
            log.EventName = "ITEM UPDATING YOW";
            log.ListItemId = 1;
            log.ListName = "TEST";
            log.SysMessage = "UPDATED EUY";
            func.InsertEventReceiverLog(log);
        }


    }
}