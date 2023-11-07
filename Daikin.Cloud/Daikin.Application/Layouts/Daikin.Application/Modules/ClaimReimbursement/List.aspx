<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Src="~/_controltemplates/15/Daikin.Application/Loading.ascx" TagPrefix="uc1" TagName="Loading" %>

<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Daikin.Application.Layouts.Daikin.Application.Modules.ClaimReimbursement.List" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/15/Daikin.Application/JS/jquery-3.3.1.min.js"></script>


    <link href="/_layouts/15/Daikin.Application/CSS/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="/_layouts/15/Daikin.Application/JS/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/Daikin.Application/JS/BaseLogic.js?id=<%= Guid.NewGuid().ToString() %>"></script>
    <link type="text/css" rel="Stylesheet" href="/_layouts/15/Daikin.Application/CSS/style.css?id=<%= Guid.NewGuid().ToString() %>" />
    <link type="text/css" rel="Stylesheet" href="/_layouts/15/Daikin.Application/CSS/daikin.css?id=<%= Guid.NewGuid().ToString() %>" />


    <script type="text/javascript" src="/_layouts/15/Daikin.Application/JS/angular.min.js"></script>
    <script src="/_layouts/15/Daikin.Application/JS/Pager.js?id=<%= Guid.NewGuid().ToString() %>"></script>
    <script src="/_layouts/15/Daikin.Application/JS/BaseLogic.js?id=<%= Guid.NewGuid().ToString() %>"></script>
    <script type="text/javascript" src="/_layouts/15/Daikin.Application/JS/Modules/ClaimReimbursement.js?id=<%= Guid.NewGuid().ToString() %>"></script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div ng-app="app" ng-controller="ctrl">
        <!-- The Modal -->
        <div id="appModal" class="modal" ng-style="{display: showModal}">
            <div class="modal-content">
                <div class="modal-header">
                    <span class="close" ng-click="closeModal()">&times;</span>
                    <h2 style="color: white;">Approval History Log - {{ Module_Name }} - {{ Nintex_No }}</h2>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <thead class="table-primary" style="font-size: 12px;">
                            <tr>
                                <th>Person</th>
                                <th>Role</th>
                                <th>Comments</th>
                                <th>Action</th>
                                <th>Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="i in Logs">
                                <td bgcolor="{{i.colorCode}}">{{i.Personal_Name}}</td>
                                <td bgcolor="{{i.colorCode}}">{{i.Position}}</td>
                                <td bgcolor="{{i.colorCode}}">{{i.Comments}}</td>
                                <td bgcolor="{{i.colorCode}}">{{i.Action_Name}}</td>
                                <td bgcolor="{{i.colorCode}}">{{i.Action_Date}}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <label>Next Approver: {{ Pending_Approver_Name }} - {{Pending_Approver_Role}}</label>
                </div>
            </div>
        </div>

        <div class="container">
            <uc1:Loading runat="server" id="Loading" />

            <button class="collapsible" style="margin-top: 5px; padding: 10px;">Filter Search</button>
            <div class="content">
                <div class="form-style-2" style="float: left;">

                    <label>
                        <span>Module</span>
                        <select class="select-field" ng-model="Module" ng-options="o as o.Name for o in ddlModule"></select>
                    </label>

                    <label>
                        <span>Branch</span>
                        <select class="select-field" ng-model="Branch" ng-options="o as o.Name for o in ddlBranch"></select>
                    </label>

                    <label>
                        <span>Filter By</span>
                        <select class="select-field" ng-model="FilterBy" ng-options="o as o.Name for o in ddlFilterBy"></select>
                    </label>

                    <label>
                        <span>Status</span>
                        <select class="select-field" ng-model="PostingStatus" ng-options="o as o.Name for o in ddlPostingStatus" ng-change="onChangeDDLPostingStatus();"></select>

                    </label>
                    <label>
                        <span ng-show="PostingStatus.Code == 4">Pending Approval Role</span>
                        <select ng-show="PostingStatus.Code == 4" class="select-field" ng-model="PendingApprovalRole" ng-options="o as o.Name for o in ddlPendingApprovalRole"></select>
                    </label>
                </div>

                <div class="form-style-2" style="float: left">
                    <label>
                        <span>Payment Status</span>
                        <select class="select-field" ng-model="PaymentStatus" ng-options="o as o.Name for o in ddlPaymentStatus"></select>
                    </label>

                    <label>
                        <span>Search By</span>
                        <select class="select-field" ng-model="SearchBy" ng-options="o as o.Name for o in ddlSearchBy"></select>
                    </label>

                    <label>
                        <span>Keywords</span>
                        <input type="text" ng-model="Keywords" class="input-field" ng-keypress="SearchHelper($event)" />
                    </label>

                    <label>
                        <span>Period</span>
                        <input type="text" class="date-range-field" ng-model="Date.End" datepicker />

                        <div class="inline-input" style="float: left;">
                            <span style="float: right; padding: 10px">To</span>
                            <input type="text" class="date-range-field" ng-model="Date.Start" datepicker />
                        </div>
                        <button type="button" class="btn btn-primary" ng-click="Search()">Search</button>

                    </label>

                </div>


            </div>

            <div class="form-style-2">
                <button type="button" class="btn btn-primary" ng-click="Received()" ng-disabled="selected.length <= 0">Document Received</button>
                <button type="button" class="btn btn-primary" ng-disabled="Module.Code==''" ng-click="CreateNewForm()">(+) Create New Form</button>
            </div>

            <table class="table">
                <thead class="table-primary" style="font-size: 12px;">
                    <tr>
                        <th>
                            <input type="checkbox" ng-click="selectAll($event)" ng-checked="isSelectedAll()" />
                        </th>
                        <th>No</th>
                        <th>Nintex number</th>
                        <th>Requester</th>
                        <th>Branch</th>
                        <th>Created Date</th>
                        <th>Amount</th>
                        <th>Approval Date</th>
                        <th>Posting Date</th>
                        <th>Scheduled Payment Date</th>
                        <th>Actual Payment Date</th>
                        <th>Approval Status</th>
                    </tr>
                </thead>
                <tbody>
                    <tr ng-repeat="i in Items" ng-class="getSelectedClass(i)">
                        <td>
                            <input type="checkbox" name="selected" ng-checked="isSelected(i.Item_ID)" ng-disabled="i.Current_Index_Approver != 2"
                                ng-click="updateSelection($event, i.Item_ID)" /></td>
                        <td bgcolor="{{i.colorCode}}">{{i.No}}</td>
                        <td bgcolor="{{i.colorCode}}" ng-show="i.Approval_Status == '5'"><a ng-href="/Lists/Claim%20Reimbursement/EditForm.aspx?ID={{i.Item_ID}}">{{i.Form_No}}</a></td>
                        <td bgcolor="{{i.colorCode}}" ng-show="i.Approval_Status == '8'"><a ng-href="/Lists/Claim%20Reimbursement/EditForm.aspx?ID={{i.Item_ID}}">DRAFT</a></td>
                        <td bgcolor="{{i.colorCode}}" ng-show="i.Approval_Status == '2'"><a ng-href="/Lists/Claim%20Reimbursement/DispForm.aspx?ID={{i.Item_ID}}">ON GENERATING</a></td>
                        <td bgcolor="{{i.colorCode}}" ng-show="['1','6','7','4'].indexOf(i.Approval_Status)>=0"><a ng-href="/Lists/Claim%20Reimbursement/DispForm.aspx?ID={{i.Item_ID}}">{{ i.Form_No }}</a></td>
                        <td bgcolor="{{i.colorCode}}">{{i.Requester_Name}}</td>
                        <td bgcolor="{{i.colorCode}}">{{i.Branch}}</td>

                        <td bgcolor="{{i.colorCode}}">{{i.Created_Date | date : 'dd-MMM-yyyy HH:mm'}}</td>
                        <td bgcolor="{{i.colorCode}}">{{i.Grand_Total | number}}</td>
                        <td style="cursor: pointer" ng-click="ApproverLog(i)" ng-show="i.Approval_Date !== null" bgcolor="{{i.colorCode}}"><a>{{i.Approval_Date | date : 'dd-MMM-yyyy HH:mm'}}</a></td>
                        <td style="cursor: pointer" ng-click="ApproverLog(i)" ng-show="i.Approval_Date === null" bgcolor="{{i.colorCode}}"><a>{{ i.Approval_Status_Name }}</a></td>
                        <td bgcolor="{{i.colorCode}}">{{i.Posting_Date | date : 'dd-MMM-yyyy HH:mm'}}</td>
                        <td bgcolor="{{i.colorCode}}">{{i.Scheduled_Payment_Date | date : 'dd-MMM-yyyy HH:mm'}}</td>
                        <td bgcolor="{{i.colorCode}}">{{i.Actual_Payment_Date | date : 'dd-MMM-yyyy HH:mm'}}</td>
                        <td bgcolor="{{i.colorCode}}">{{i.Approval_Status_Name}}</td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <th colspan="5" style="text-align: left; padding: 10px">Total</th>
                        <th style="text-align: left">{{Total | number}}</th>
                    </tr>
                    <tr>
                        <th colspan="5" style="text-align: left; padding: 10px">Grand Total</th>
                        <th style="text-align: left">{{GrandTotal | number}}</th>
                    </tr>

                </tfoot>
            </table>

            <div class="Pager">
            </div>

        </div>
    </div>


    <script type="text/javascript">
        var coll = document.getElementsByClassName("collapsible");
        var i;

        for (i = 0; i < coll.length; i++) {
            coll[i].addEventListener("click", function (e) {
                e.preventDefault();
                this.classList.toggle("active");
                var content = this.nextElementSibling;
                if (content.style.maxHeight) {
                    content.style.maxHeight = null;
                } else {
                    //content.style.maxHeight = content.scrollHeight + "px";
                    content.style.maxHeight = "270px";

                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Claim Reimbursement
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Claim Reimbursement
</asp:Content>
