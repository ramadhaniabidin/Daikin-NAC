<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeWebPart.ascx.cs" Inherits="Daikin.Application.HomeWebPart.HomeWebPart" %>

<link href="../_layouts/15/Daikin.Application/Assets/nav.css?id=<%= Guid.NewGuid().ToString() %>" rel="stylesheet" />


<div class="">
    <img src="/_layouts/15/Daikin.Application/Assets/logowhite.png" class="logoimage">
    <header>
          <div class="six">
            <h1>
              DID Nintex Portal
            </h1>
          </div>
        </header>
    <nav class="main-nav">
      <ul>
        
        <li class="first-level left">
          <i class="badge" runat="server" id="TotalPendingTaskList">99+</i>
          <img src="../_layouts/15/Daikin.Application/Assets/Task List/Icon Daikin Nintex-29.png" 
              width="90px"  class="mt-1" onclick="location.href='/_layouts/15/Daikin.Application/Modules/PendingTask/PendingTaskList.aspx'">
          <!-- <i class="fa-solid fa-file-invoice-dollar icon"></i> -->
          <span>Pending Task List</span>
        </li>
        <li class="first-level left">
          <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-05.png" width="90px" class="mt-1">
          <span class="mb-1">Claim & Reimbursement</span>
          <ul class="submenu" >
            <li class="submenu-col">
              <a href="/_layouts/15/Daikin.Application/Modules/ClaimReimbursement/Memo.aspx?module=M001">
                <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-06.png" width="90px" />
                <div class="submenu-title">Business Relation</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="/Lists/Travel%20Expense/NewForm.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-07.png" width="90px" />
                <div class="submenu-title">Travel Expense</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="/Lists/Unloading%20Fee/NewForm.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-08.png" width="90px" />
                <div class="submenu-title">Unloading Fee</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="/Lists/Modern%20Channel/NewForm.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-09.png" width="90px" />
                <div class="submenu-title">Modern Channel</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="/Lists/General%20Rebate/NewForm.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-10.png"  width="90px" />
                <div class="submenu-title">General Rebate</div>
              </a>
            </li>
            <%--<li class="submenu-col">
              <a href="/Lists/Revise%20Rebate/NewForm.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-11.png"  width="90px" />
                <div class="submenu-title">Revise Rebate</div>
              </a>
            </li>--%>
            <li class="submenu-col">
              <a href="/Lists/Claim%20Rebate/NewForm.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-12.png"  width="90px" />
                <div class="submenu-title">Claim Rebate</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="/_layouts/15/Daikin.Application/Modules/ClaimReimbursement/List.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Claim%20And%20Reimbursement/Icon%20Daikin%20Nintex-13.png"  width="90px" />
                <div class="submenu-title">Claim & Reimbursement <br> List</div>
              </a>
            </li>
          </ul>
        </li>

        <li class="first-level left">
          <img src="../_layouts/15/Daikin.Application/Assets/Commercial/Icon Daikin Nintex-14.png" width="90px" class="mt-1">
          <span class="mb-2">Commercial</span>
          <ul class="submenu">
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_layouts/15/Daikin.WebApps/Commercials/FOB.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Commercial/Icon Daikin Nintex-15.png" width="90px" class="mt-1">
                <div class="submenu-title">FOB</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_layouts/15/Daikin.WebApps/Commercials/ServiceCost.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Commercial/Icon Daikin Nintex-16.png" width="90px" class="mt-1">
                <div class="submenu-title">Service Cost</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/Lists/PIB/NewForm.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Commercial/Icon Daikin Nintex-17.png" width="90px" class="mt-1">
                <div class="submenu-title">PIB</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/Lists/Commercials/NewForm.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Commercial/Icon Daikin Nintex-18.png" width="90px" class="mt-1">
                <div class="submenu-title">PO Subcon</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_Layouts/15/Daikin.WebApps/Commercials/List.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Commercial/Icon Daikin Nintex-19.png" width="90px" class="mt-1">
                <div class="submenu-title">Commercial List</div>
              </a>
            </li>
          </ul>
        </li>

        <li class="first-level left">
          <img src="../_layouts/15/Daikin.Application/Assets/Non Commercial/Icon Daikin Nintex-20.png" width="90px"  class="mt-1">
          <span class="mb-2">Non Commercial</span>
          <ul class="submenu">
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_Layouts/15/Daikin.WebApps/NonCommercials/Contract.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Non Commercial/Icon Daikin Nintex-21.png" width="90px"  class="mt-1">
                <div class="submenu-title">Cotract</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_Layouts/15/Daikin.WebApps/NonCommercials/POContract.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Non Commercial/Icon Daikin Nintex-22.png" width="90px"  class="mt-1">
                <div class="submenu-title">PO Contract</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_layouts/15/Daikin.WebApps/Master/Display.aspx?module=M016">
                <img src="../_layouts/15/Daikin.Application/Assets/Non Commercial/Icon Daikin Nintex-23.png" width="90px"  class="mt-1">
                <div class="submenu-title">Purchase Request</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_Layouts/15/Daikin.WebApps/NonCommercials/List.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Non Commercial/Icon Daikin Nintex-24.png" width="90px"  class="mt-1">
                <div class="submenu-title">QCF</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_layouts/15/Daikin.WebApps/Master/Display.aspx?module=M016">
                <img src="../_layouts/15/Daikin.Application/Assets/Non Commercial/Icon Daikin Nintex-25.png" width="90px"  class="mt-1">
                <div class="submenu-title">Purchase Order</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_Layouts/15/Daikin.WebApps/NonCommercials/List.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Non Commercial/Icon Daikin Nintex-26.png" width="90px"  class="mt-1">
                <div class="submenu-title">PO Release</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_Layouts/15/Daikin.WebApps/NonCommercials/List.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Non Commercial/Icon Daikin Nintex-27.png" width="90px"  class="mt-1">
                <div class="submenu-title">Non Commercial List</div>
              </a>
            </li>
          </ul>
        </li>

        <li class="first-level right">
          <img src="../_layouts/15/Daikin.Application/Assets/Finance Payment/Icon Daikin Nintex-01.png" width="90px" class="mt-1">
          <span class="mb-2">Payment Menu</span>
          <ul class="submenu">
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_layouts/15/Daikin.WebApps/FinanceMenu/List.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Finance Payment/Icon Daikin Nintex-02.png" width="90px" class="mt-1">
                <div class="submenu-title">Finance Payment</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_layouts/15/Daikin.WebApps/FinanceMenu/List.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Finance Payment/Icon Daikin Nintex-03.png" width="90px" class="mt-1">
                <div class="submenu-title">Finance Confirmation</div>
              </a>
            </li>
            <li class="submenu-col">
              <a href="https://sp3.daikin.co.id:8443/_layouts/15/Daikin.WebApps/FinanceMenu/List.aspx">
                <img src="../_layouts/15/Daikin.Application/Assets/Finance Payment/Icon Daikin Nintex-04.png" width="90px" class="mt-1">
                <div class="submenu-title">Schedule Payment</div>
              </a>
            </li>
          </ul>
        </li>

      </ul>
    </nav>
</div>