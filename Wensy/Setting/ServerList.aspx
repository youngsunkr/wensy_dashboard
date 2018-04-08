<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="ServerList.aspx.cs" Inherits="ServicePoint.Setting.ServerList" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        chkAuth();
        $(document).ready(function () {
            activeMenu('menuLeft', 'serveradd');
        });//]]
    </script>
    <form id="frm" runat="server">
        <div class="row-fluid">
            <div class="table-responsive">
                <div class="col-lg-12">
                    <input type="button" class="btn btn-primary pull-right text-info" name="btn_Add" value="서버등록" onclick="modalAddServer();" />
                </div>
                <div class="col-lg-12">
                    <asp:GridView ID="gv_List" runat="server" CssClass="table table-hover table-bordered" HeaderStyle-BackColor="LightGray" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="서버명" DataField="DisplayName" />
                            <asp:BoundField HeaderText="그룹명" DataField="DisplayGroup" />
                            <asp:BoundField HeaderText="서버유형" DataField="ServerType" />
                            <asp:BoundField HeaderText="등록일" DataField="RegDate" />
                            <asp:BoundField HeaderText="서버키" DataField="ProductKey" />
                            <asp:BoundField HeaderText="언어" DataField="RegionCode" />
                            <asp:BoundField HeaderText="에이전트버전" DataField="AgentVer" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <input type="button" class="btn btn-sm btn-warning" value="수정" onclick="modalUpdateServer('<%#Eval("Displayname").ToString()%>','<%#Eval("DisplayGroup").ToString()%>','<%#Eval("ServerType").ToString()%>','<%#Eval("ServerNum").ToString()%>','<%#Eval("RegionCode").ToString()%>')" />
                                    <input type="button" class="btn btn-sm btn-danger" value="삭제" onclick="DelServer(<%# Eval("ServerNum").ToString()%>)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>No data</EmptyDataTemplate>
                    </asp:GridView>

                </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
    <Modal:ServerAdd ID="ServerAdd" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
