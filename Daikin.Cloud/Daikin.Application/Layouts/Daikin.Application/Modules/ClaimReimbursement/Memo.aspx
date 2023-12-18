<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Memo.aspx.cs" Inherits="Daikin.Application.Layouts.Daikin.Application.Modules.ClaimReimbursement.Memo" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
   <link type="text/css" rel="Stylesheet" href="/_layouts/15/Daikin.Application/CSS/navigation.css?id=<%= Guid.NewGuid().ToString() %>" />
    <link type="text/css" rel="Stylesheet" href="/_layouts/15/Daikin.Application/CSS/master.css?id=<%= Guid.NewGuid().ToString() %>" />

    <script src="../../JS/jquery-3.3.1.min.js"></script>
    <script src="../../JS/angular.min.js"></script>
    <script src="../../JS/Modules/Memo.js?id=<%= Guid.NewGuid().ToString() %>"></script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="container mt-5" ng-app="app" ng-controller="ctrl">
        <div class="row" style="height: 700px; display: flex; justify-content:center">
            <div class="col-6 text-center">
                <img src="/SiteAssets/InternalMemo.png" alt="Memo1" />
            </div>
        </div>
        <div class="row" style="margin-top:50px; display: flex; justify-content:center">
            <div class="col text-center">
                <div class="form-check">
                    <label for="Agree" class="form-check-label">
                        <input type="checkbox" id="Agree" class="form-check-input" ng-model="Agree" />
                        By ticking this box, I declared that I have read and agree to Daikin Internal Policy and Procedure​
                    </label>
                </div>
            </div>
        </div>
        <div class="row" style="display: flex; justify-content:center">
            <div class="col text-center">
                <button type="button" class="btn btn-primary" ng-show="Agree" ng-click="SelectDisplay()">OK</button>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Internal Memo
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Internal Memo
</asp:Content>
