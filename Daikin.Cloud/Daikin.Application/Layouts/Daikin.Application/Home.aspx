<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Daikin.Application.Layouts.Daikin.Application.Home" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<link href="assets/nav.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <img src="assets/logowhite.png" class="logoimage">
    <header>
      <div class="six">
        <h1>
          DID Nintex Portal
        </h1>
      </div>
    </header>
    <nav>
      <ul class="main-nav">
        
        <li class="first-level left">
          <i class="badge">99+</i>
          <img src="assets/Task List/Icon Daikin Nintex-29.png" width="90px"  class="mt-1">
          <!-- <i class="fa-solid fa-file-invoice-dollar icon"></i> -->
          <span>Pending Task List</span>
        </li>

        <li class="first-level left">
          <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-05.png" width="90px" class="mt-1">
          <span class="mb-1">Claim & Reimbursement</span>
          <ul class="submenu">
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-06.png" width="90px" />
                <div class="submenu-title">Business Relation</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-07.png" width="90px" />
                <div class="submenu-title">Travel Expense</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-08.png" width="90px" />
                <div class="submenu-title">Unloading Fee</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-09.png" width="90px" />
                <div class="submenu-title">Modern Channel</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-10.png"  width="90px" />
                <div class="submenu-title">General Rebate</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-11.png"  width="90px" />
                <div class="submenu-title">Revise Rebate</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-12.png"  width="90px" />
                <div class="submenu-title">Claim Rebate</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Claim & Reimbursement/Icon Daikin Nintex-13.png"  width="90px" />
                <div class="submenu-title">Claim & Reimbursement <br> List</div>
              </a>
            </li>
          </ul>
        </li>

        <li class="first-level left">
          <img src="assets/Commercial/Icon Daikin Nintex-14.png" width="90px" class="mt-1">
          <span class="mb-2">Commercial</span>
          <ul class="submenu">
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Commercial/Icon Daikin Nintex-15.png" width="90px" class="mt-1">
                <div class="submenu-title">FOB</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Commercial/Icon Daikin Nintex-16.png" width="90px" class="mt-1">
                <div class="submenu-title">Service Cost</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Commercial/Icon Daikin Nintex-17.png" width="90px" class="mt-1">
                <div class="submenu-title">PIB</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Commercial/Icon Daikin Nintex-18.png" width="90px" class="mt-1">
                <div class="submenu-title">PO Subcon</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Commercial/Icon Daikin Nintex-19.png" width="90px" class="mt-1">
                <div class="submenu-title">Commercial List</div>
              </a>
            </li>
          </ul>
        </li>

        <li class="first-level left">
          <img src="assets/Non Commercial/Icon Daikin Nintex-20.png" width="90px"  class="mt-1">
          <span class="mb-2">Non Commercial</span>
          <ul class="submenu">
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Non Commercial/Icon Daikin Nintex-21.png" width="90px"  class="mt-1">
                <div class="submenu-title">Cotract</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Non Commercial/Icon Daikin Nintex-22.png" width="90px"  class="mt-1">
                <div class="submenu-title">PO Contract</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Non Commercial/Icon Daikin Nintex-23.png" width="90px"  class="mt-1">
                <div class="submenu-title">Purchase Request</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Non Commercial/Icon Daikin Nintex-24.png" width="90px"  class="mt-1">
                <div class="submenu-title">QCF</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Non Commercial/Icon Daikin Nintex-25.png" width="90px"  class="mt-1">
                <div class="submenu-title">Purchase Order</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Non Commercial/Icon Daikin Nintex-26.png" width="90px"  class="mt-1">
                <div class="submenu-title">PO Release</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Non Commercial/Icon Daikin Nintex-27.png" width="90px"  class="mt-1">
                <div class="submenu-title">Non Commercial List</div>
              </a>
            </li>
          </ul>
        </li>

        <li class="first-level right">
          <img src="assets/Finance Payment/Icon Daikin Nintex-01.png" width="90px" class="mt-1">
          <span class="mb-2">Payment Menu</span>
          <ul class="submenu">
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Finance Payment/Icon Daikin Nintex-02.png" width="90px" class="mt-1">
                <div class="submenu-title">Finance Payment</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Finance Payment/Icon Daikin Nintex-03.png" width="90px" class="mt-1">
                <div class="submenu-title">Finance Confirmation</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="#">
                <img src="assets/Finance Payment/Icon Daikin Nintex-04.png" width="90px" class="mt-1">
                <div class="submenu-title">Schedule Payment</div>
              </a>
            </li>
          </ul>
        </li>

      </ul>
    </nav>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Home
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Home
</asp:Content>
