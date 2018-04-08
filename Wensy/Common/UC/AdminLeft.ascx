<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminLeft.ascx.cs" Inherits="ServicePoint.Common.UC.AdminLeft" %>
<asp:Panel ID="pnl_Setting" runat="server" Visible="false">
    <div class="list-group" id="menuLeft">
        <a class="list-group-item active">사용자 관리</a>
        <a href="/Setting/GroupMember.aspx" class="list-group-item" id="groupmember">그룹 멤버 관리</a>
        <a class="list-group-item   active" id="serveradmin">서버 관리</a>
        <a href="/Setting/ServerList.aspx" class="list-group-item " id="serveradd">서버 등록/삭제</a>
        <a href="/Setting/ServerMember.aspx" class="list-group-item" id="servermember">서버 관리자 등록</a>
        <a class="list-group-item   active" id="monitoringadmin">모니터링 관리</a>
        <a href="/Setting/RuleEditor.aspx" class="list-group-item " id="serveralarm">서버 알림 관리</a>
        <%--<a href="/Setting/RuleEditorByPCID.aspx" class="list-group-item " id="ruleeditorbypcid">성능 기준 기준 알림 관리</a>--%>
       <%-- <a href="/Setting/ProcessInstance.aspx" class="list-group-item " id="processinstance">모니터링 항목추가</a>--%>
        <%--<a class="list-group-item   active">환경 설정</a>
        <a href="/setting/user.aspx" class="list-group-item" id="userinfo">사용자 정보</a>--%>
       <%-- <a href="#" class="list-group-item" id="download">다운로드</a>--%>
    </div>
</asp:Panel>
<asp:Panel ID="pnl_Report" runat="server" Visible="false">
   
    <div class="list-group" id="menuLeft">
        <a class="list-group-item active">Performance Metric</a>
        <a href="/Report/PerfmonChart.aspx" class="list-group-item" id="winperformence">Chart</a>
        <a href="/Report/Win_CounterReport.aspx" class="list-group-item" id="wincounter">Chart Search</a>
       <%-- <a class="list-group-item active">웹서버</a>
        <a href="/Report/Web_PerformanceReport.aspx" class="list-group-item" id="webperformence">성능 분석 보고서</a>
        <a href="/Report/Web_ServiceReport.aspx" class="list-group-item" id="webservice">서비스 분석 보고서</a>--%>
        <%--<a class="list-group-item active">SQL Server</a>--%>
       <%-- <a href="/Report/SQL_PerformanceReport.aspx" class="list-group-item" id="sqlperformence">성능 분석 보고서</a>--%>
       <%-- <a href="/Report/SQL_Cpu.aspx" class="list-group-item" id="sqlcpu">Top Query</a>--%>
       <%-- <a href="/Report/SQL_Memory.aspx" class="list-group-item" id="sqlmemory">SQL MEMORY</a>
        <a href="/Report/SQL_Disk.aspx" class="list-group-item" id="sqldisk">SQL DISK</a>
        <a href="/Report/SQL_Configuration.aspx" class="list-group-item" id="sqlconfiguration">SQL CONFIGURATION</a>
        <a href="/Report/SQL_Lock.aspx" class="list-group-item" id="sqllock">SQL LOCK</a>--%>
       <%-- <a href="/Report/SQL_Database.aspx" class="list-group-item" id="sqldetail">SQL DATABASE</a>--%>
    <%--    <a class="list-group-item active">기타</a>--%>
        <%--<a href="/Report/UserReport.aspx" class="list-group-item" id="userreport">사용자 정의 보고서</a>--%>
    </div>
</asp:Panel>
