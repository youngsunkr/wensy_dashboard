<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="ProcessInstance.aspx.cs" Inherits="ServicePoint.Setting.ProcessInstance" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        chkAuth();
        $(document).ready(function () {
            activeMenu('menuLeft', 'processinstance');
        });//]]
        function SelectServer(ServerNum,strDisplayName) {
            $('#MainContent_hdn_ServerNum').val(ServerNum);
            $('#MainContent_hdn_DisplayName').val(strDisplayName);
            $('#frm').submit();
            //location.href = "/Setting/ServerMember.aspx?ServerNum="+ServerNum;
        }
        function DelServerInstance(ServerNum,strPCID,strInstanceName) {
            $.getJSON('/Common/Proc/Admin.ashx?callback=?', {command:'delserverinstance',hdn_numServer:ServerNum,hdn_strPCID:strPCID,hdn_strInstanceName:strInstanceName}, function (json) {
                if (1 == json.error) {
                    $('#frm').submit();
                } else {
                    alert(json.desc);
                   
                }
            }).error(function (xhr) {
                alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
            });
        }
    </script>
    <form id="frm" runat="server">
        <div class="row-fluid">
            <asp:HiddenField ID="hdn_ServerNum" runat="server" />
             <asp:HiddenField ID="hdn_DisplayName" runat="server" />
            <div class="col-lg-4">
                <div class="row-fluid">
                    <div class="col-lg-12">
                        <h4>서버리스트</h4>
                        <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"  HeaderStyle-BackColor="LightGray">
                            <Columns>
                                <asp:BoundField DataField="DisplayName" HeaderText="이름" />
                                <asp:BoundField DataField="ServerType" HeaderText="타입" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input type="button" class="btn" id="btnSelect" name="btnSelect" value="사용자보기" onclick="SelectServer(<%#Eval("ServerNum").ToString()%>,'<%#Eval("DisplayName").ToString()%>');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <asp:Panel ID="pnl_Proess" runat="server" Visible="false">

                    <div class="col-lg-12">
                        <h4>모니터링 항목</h4>
                        <asp:GridView ID="gv_List_Process" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"  HeaderStyle-BackColor="LightGray">
                            <Columns>
                                <asp:BoundField DataField="PCountername" HeaderText="카운터명" />
                                <asp:BoundField DataField="PCID" HeaderText="PCID" />
                            </Columns>
                        </asp:GridView>
                    </div>

                </asp:Panel>
            </div>
            <div class="col-lg-4">
                <asp:Panel ID="pnl_Instance" CssClass="" runat="server" Visible="false">

                    <div class="col-lg-12">
                        <h4>모니터링 대상 프로세스 목록</h4>
                        <asp:GridView ID="gv_List_Instance" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"  HeaderStyle-BackColor="LightGray">
                            <Columns>
                                <asp:BoundField DataField="PCID" HeaderText="PCID" />
                                <asp:BoundField DataField="InstanceName" HeaderText="인스턴스명" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input type="button" id="btn_Del" class="btn btn-danger" value="삭제" onclick="DelServerInstance(<%#Eval("ServerNum").ToString()%>,'<%#Eval("PCID").ToString()%>','<%#Eval("InstanceName").ToString()%>');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-lg-12  well">
                        <div class="row-fluid ">
                            <div class="col-lg-5">
                                <asp:Label ID="lbl_Server" runat="server">선택한서버</asp:Label>
                                <asp:TextBox ID="txt_Server" CssClass="input-sm" runat="server" disabled="true"></asp:TextBox>
                            </div>
                            <div class="col-lg-5">
                                <asp:Label ID="lbl_InstanceName" runat="server">프로세스명</asp:Label>
                                <asp:TextBox ID="txt_InstanceName" CssClass="input-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-2">
                                <asp:Button ID="btn_Add" CssClass="btn btn-lg btn-primary pull-right" Text="추가"  runat="server" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
