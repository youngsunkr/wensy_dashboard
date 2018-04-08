<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="RuleEditor.aspx.cs" Inherits="ServicePoint.Setting.RuleEditor" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        chkAuth();
        $(document).ready(function () {
            activeMenu('menuLeft', 'serveralarm');
        });//]]
        function SetAlarmState() {
            var CompanyNum = getCookie('CompanyNum');
            var PushInterval = $('#MainContent_ddl_Delay').val();
            var PushMaxOccurs = $('#MainContent_ddl_Cnt').val();
            var PushResetInterval = $('#MainContent_ddl_Reset').val();
            var UsePushAlert = $('#MainContent_chk_Use').is(':checked');
            $.getJSON('/Common/Proc/Admin.ashx?callback=?', { command: 'updatealertoptions', hdn_PushInterval: PushInterval, hdn_PushMaxOccurs: PushMaxOccurs, hdn_PushResetInterval: PushResetInterval, hdn_UsePushAlert: UsePushAlert }, function (json) {
                if (1 == json.error) {
                    alert(json.desc);
                } else {
                    alert(json.desc);
                }
            }).error(function (xhr) {
                alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
            });
        }
    </script>
    <form id="frm" runat="server">
        <div class="col-lg-12 well  well-sm" style="display: none;">
            <div class="col-lg-2">
                <span class="glyphicon glyphicon-alert" aria-hidden="true">&nbsp;알림사용</span>
                    <asp:CheckBox ID="chk_Use" runat="server" CssClass="input-large" />
            </div>
            <div class="col-lg-2">
                <span class="glyphicon glyphicon-dashboard text-info" aria-hidden="true">&nbsp;검색주기(min) </span>
                    <asp:DropDownList ID="ddl_Delay" runat="server" CssClass="small"></asp:DropDownList>

            </div>
            <div class="col-lg-3">
                <span class="glyphicon glyphicon-refresh text-info" aria-hidden="true">&nbsp;최대반복알림(Count) </span>
                    <asp:DropDownList ID="ddl_Cnt" runat="server" CssClass="small"></asp:DropDownList>
            </div>
            <div class="col-lg-3">
               <span class="glyphicon glyphicon-time text-info" aria-hidden="true">&nbsp;반복카운터초기화주기(hour) </span>
                    <asp:DropDownList ID="ddl_Reset" runat="server" CssClass="small"></asp:DropDownList>
            </div>
            <div class="col-lg-2">
                <button type="button" class="btn btn-primary btn-sm" onclick="SetAlarmState();"><span class="glyphicon glyphicon-save-file" aria-hidden="true"></span>저장</button>
            </div>
        </div>
        <div class="col-lg-12 well well-sm">
            <div class="col-lg-2"><span class="h5">서버선택</span></div>
            <div class="col-lg-2">
                <asp:DropDownList ID="ddlList" runat="server" CssClass="input-sm"></asp:DropDownList>
            </div>
            <div class="col-lg-8">
                <asp:Button ID="btn_Search" runat="server" class="btn btn-sm btn-primary" Text="조회"></asp:Button>

            </div>
        </div>
        <div class="col-lg-12">
            <div class="table-responsive">
                <asp:ObjectDataSource ID="ods_List" runat="server"></asp:ObjectDataSource>
            <asp:GridView ID="gv_List" runat="server" CssClass="table table-bordered " EnableViewState="false" AllowSorting="True" HeaderStyle-BackColor="LightGray" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="PObjectName" HeaderText="성능객체" SortExpression="PObjectName"/>
                    <asp:BoundField DataField="PCounterName" HeaderText="성능카운터" SortExpression="PCounterName" />
                    <asp:BoundField DataField="AlertLevel" HeaderText="알림수준" SortExpression="AlertLevel" />
                    <asp:BoundField DataField="ReasonCodeDesc" HeaderText="상세" />
                    <asp:BoundField DataField="InstanceName" HeaderText="인스턴스" />
                    <asp:BoundField DataField="Threshold" HeaderText="임계값"  />
                    <asp:BoundField DataField="Duration" HeaderText="수집기간(sec)" />
                    <asp:BoundField DataField="RecordApps" HeaderText="세부실행기록" />
                    <asp:BoundField DataField="IsEnabled" HeaderText="규칙활성화" />
                    <asp:BoundField DataField="MobileAlert" HeaderText="모바일알림사용" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <input type="button" class="btn btn-warning" id="btnSelect" name="btnSelect" value="수정" onclick="modalUpdateRule('<%#Eval("PObjectName").ToString()%>','<%#Eval("PCounterName").ToString()%>','<%#Eval("AlertLevel").ToString()%>','<%#Eval("ReasonCodeDesc").ToString()%>','<%#Eval("InstanceName").ToString()%>','<%#Eval("Threshold").ToString()%>','<%#Eval("Duration").ToString()%>','<%#Eval("RecordApps").ToString()%>','<%#Eval("IsEnabled").ToString()%>','<%#Eval("MobileAlert").ToString()%>','<%#Eval("ReasonCode")%>','<%=numServer%>');" />
</ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No Data</EmptyDataTemplate>
            </asp:GridView>
                </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
    <Modal:ServerRule ID="Servereditor" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
