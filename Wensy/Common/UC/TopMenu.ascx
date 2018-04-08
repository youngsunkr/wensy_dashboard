<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenu.ascx.cs" Inherits="ServicePoint.Common.UC.TopMenu" %>
<div class="container-fluid">
    <div class="row-fluid">
        <nav id="navid" class="navbar navbar-inverse" role="navigation">

            <!-- 모바일 화면에서 메뉴라인 표시 -->
            <div class="navbar-header page-scroll">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="/Default.aspx">Wensy</a>
            </div>
            <!-- //모바일 화면에서 메뉴라인 표시 -->
            <!-- 메뉴 -->
            <div class="collapse navbar-collapse" id="bs-navbar-collapse">
                <ul class="nav navbar-nav navbar-left">
                    <%--<li class="dropdown">--%>
                    <li><a href="/Pop_Alert.aspx" target="_blank"><i class="fa fa-th"></i> ALERT MESSAGE</a>
                      <%--  <ul class="dropdown-menu" role="menu">
                           <li><a href="/Default.aspx?Type=Default"><i class="fa fa-th"></i>3 X 3</a></li>
                            <li><a href="/Default.aspx?Type=Small"><i class="fa fa-th"></i>6 X 4</a></li>
                        </ul>--%>
                    </li>
                    <%--<li class="line-vertical">--%>
                   <%-- <li><a href="/OverView/Serveroverview.aspx">Servers Overview</a></li>
                    <li><a href="/Overview/WebOverview.aspx">Web Servers</a></li>--%>
                    <li><a href="/Overview/SqlOverview.aspx">Sql Servers</a></li>
                    <%--<li class="line-vertical">--%>
                    <li><a href="/Report/PerfmonChart.aspx"><i class="fa fa-file-text"></i> Reports</a></li>
                    <li><a href="/Setting/GroupMember.aspx"><i class="fa fa-gear"></i> Settings</a></li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="/Setting/user.aspx"><i class="fa fa-user"></i> <%= strUserEmail %></a></li>
                    <li><a href="/login/logout.aspx"><i class="fa fa-sign-out"></i> Logout</a></li>
                </ul>
            </div>
            <!-- //메뉴 -->

        </nav>
    </div>
</div>
<%--<nav class="navbar navbar-inverse">
    <div class="container-fluid">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-9">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>

            </button>
            <a class="navbar-brand" style="color: #ffffff;" href="#">Admin</a>
        </div>
        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-9">
            <ul class="nav navbar-nav"  id="menutop">
                <li class="dropdown" id="server"><a href="#" class="dropdown-toggle" data-toggle="dropdown">서버관리</a>
                    <ul class="dropdown-menu" role="menu">
                        <li id="serverlist"><a href="#">서버리스트</a></li>
                        <li id="serverlog"><a href="#">서버로그</a></li>
                         <li id="keylist"><a href="/Server/KeyList">제품키관리</a></li>
                    </ul>
                </li>
                <li class="dropdown" id="Point"><a href="#" class="dropdown-toggle" data-toggle="dropdown">결제관리</a>
                    <ul class="dropdown-menu" role="menu">
                        <li><a href="#">결제내역</a></li>
                        <li><a href="#">쿠폰사용내역</a></li>
                        <li><a href="#">가상계좌로그</a></li>
                    </ul>
                </li>
                <li class="dropdown"id="member"><a href="#" class="dropdown-toggle" data-toggle="dropdown">회원관리</a>
                    <ul class="dropdown-menu" role="menu">
                        <li><a href="#">신규회원리스트</a></li>
                        <li><a href="#">젠체회원리스트</a></li>
                        <li><a href="#">고객사관리</a></li>
                    </ul>

                </li>
            </ul>
        </div>

    </div>
</nav>
--%>
