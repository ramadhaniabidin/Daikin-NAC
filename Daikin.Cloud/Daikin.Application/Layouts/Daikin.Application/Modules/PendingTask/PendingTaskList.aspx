<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Src="~/_controltemplates/15/Daikin.Application/Loading.ascx" TagPrefix="uc1" TagName="Loading" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PendingTaskList.aspx.cs" Inherits="Daikin.Application.Layouts.Daikin.Application.Modules.PendingTask.PendingTaskList" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/15/Daikin.Application/JS/jquery-3.3.1.min.js"></script>

    <link href="/_layouts/15/Daikin.Application/CSS/jquery-ui.css" rel="stylesheet" />
    <script src="/_layouts/15/Daikin.Application/JS/jquery-ui.min.js"></script>

    <link type="text/css" rel="Stylesheet" href="/_layouts/15/Daikin.Application/CSS/style.css?id=<%= Guid.NewGuid().ToString() %>" />
    <link type="text/css" rel="Stylesheet" href="/_layouts/15/Daikin.Application/CSS/daikin.css?id=<%= Guid.NewGuid().ToString() %>" />
    <link type="text/css" rel="Stylesheet" href="/_layouts/15/Daikin.Application/CSS/font-awesome/css/font-awesome.min.css?id=<%= Guid.NewGuid().ToString() %>" />

    <script type="text/javascript" src="/_layouts/15/Daikin.Application/JS/angular.min.js"></script>
    <script src="/_layouts/15/Daikin.Application/JS/ng-file-upload.min.js"></script>
    <script src="/_layouts/15/Daikin.Application/JS/Pager.js?id=<%= Guid.NewGuid().ToString() %>"></script>
    <script src="/_layouts/15/Daikin.Application/JS/BaseLogic.js?id=<%= Guid.NewGuid().ToString() %>"></script>
    <script src="/_layouts/15/Daikin.Application/JS/Modules/PendingTaskList.js?id=<%= Guid.NewGuid().ToString() %>"></script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="container" ng-app="app" ng-controller="ctrl">
        <uc1:Loading runat="server" ID="Loading" />
        <div class="container-search">
            <div class="form-style-2" style="float: left; width: 35%">
                <label>
                    <span>Search By </span>
                    <select class="select-field" ng-model="SearchBy" ng-options="o as o.Name for o in ddlSearchBy"></select>
                </label>
            </div>
            <div class="form-style-2" style="float: left; width: 35%;">
                <label>
                    <span>Keywords </span>
                    <input type="text" ng-model="Keywords" class="input-field" ng-keypress="SearchHelper($event)" />
                </label>
            </div>
            <button type="button" class="btn btn-primary button-search" ng-click="Search()">Search</button>
        </div>

        <div class="form-style-2" style="width: 95%;">
            <table class="table2">
                <thead class="table-primary" style="font-size: 12px;">
                    <tr>
                        <th>Nintex No.</th>
                        <th>Requestor</th>
                        <th>Approver Role</th>
                        <th>Branch</th>
                        <th>Module</th>
                        <th>Submitted Date</th>
                    </tr>
                </thead>
                <tbody>
                    <tr ng-repeat="PendingTask in PendingTasks">
                        <td><a ng-href="{{PendingTask.ApprovalUrl}}" >{{PendingTask.ItemTitle}}</a></td>
                        <td>{{PendingTask.RequestorName}}</td>
                        <td>{{PendingTask.PendingApproverRole}}</td>
                        <td>{{PendingTask.Branch}}</td>
                        <td>{{PendingTask.Module}}</td>
                        <td>{{PendingTask.SubmittedDate}}</td>
                    </tr>
                </tbody>
            </table>
            <div class="Pager" ng-hide="PendingTasks.length <= 0" style="margin-top: 50px;"></div>
        </div>

    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Pending Task List
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Pending Task List
</asp:Content>
