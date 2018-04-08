<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="ServerMember.aspx.cs" Inherits="ServicePoint.Setting.ServerMember" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        chkAuth();
        $(document).ready(function () {
            activeMenu('menuLeft', 'servermember');
        });//]]
        function SelectServer(ServerNum) {
            $('#MainContent_hdn_ServerNum').val(ServerNum);
            $('#frm').submit();
            //location.href = "/Setting/ServerMember.aspx?ServerNum="+ServerNum;
        }
        function AddServer(ServerNum,MemberNum) {
            $.getJSON('/Common/Proc/Admin.ashx?callback=?', {command:'addservermember',hdn_MemberNum_Target:MemberNum,hdn_numServer:ServerNum}, function (json) {
                if (1 == json.error) {
                   $('#frm').submit();
                } else {
                    alert(json.desc);
                   
                }
            }).error(function (xhr) {
                alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
            });
        }
        function DelServer(ServerNum,MemberNum) {
            $.getJSON('/Common/Proc/Admin.ashx?callback=?', {command:'delservermember',hdn_MemberNum_Target:MemberNum,hdn_numServer:ServerNum}, function (json) {
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
            <div class="col-lg-6">
                <div class="col-lg-12">
                    <h4>서버선택</h4>
                    <asp:HiddenField ID="hdn_ServerNum" runat="server" />
                    <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray" OnRowDataBound="gv_List_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="서버이름" DataField="DisplayName" />
                            <asp:BoundField HeaderText="TYPE" DataField="ServerType" />
                            <asp:TemplateField  ItemStyle-Width="150px">
                                <ItemTemplate >
                                    <input type="button" class="btn" id="btnSelect" name="btnSelect" value="사용자보기" onclick="SelectServer(<%#Eval("ServerNum").ToString()%>);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>No Data</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <div class="col-lg-6">
                <asp:Panel ID="pnl_View" runat="server" Visible="false">
                    <div class="row-fluid">
                        <div class="col-lg-12">
                            <h4>할당된 사용자 목록</h4>
                            <asp:GridView ID="gv_List_Selected" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField HeaderText="NAME" DataField="MemberName" />
                                    <asp:BoundField HeaderText="EMAIL" DataField="Email" />
                                    <asp:TemplateField  ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <input type="button" class="btn" id="btnSelect" name="btnSelect" value="삭제" onclick="DelServer(<%#Eval("ServerNum").ToString()%>,<%#Eval("MemberNum").ToString()%>);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="col-lg-12">
                            <h4>전체 사용자 목록</h4>
                            <asp:GridView ID="gv_List_UnSelected" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField HeaderText="NAME" DataField="MemberName" />
                                    <asp:TemplateField ItemStyle-Width="100px" >
                                        <ItemTemplate>
                                            <input type="button" class="btn" id="btnSelect" name="btnSelect" value="추가" onclick="AddServer(<%#Eval("ServerNum").ToString()%>,<%#Eval("MemberNum").ToString()%>);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="ModalContent" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
