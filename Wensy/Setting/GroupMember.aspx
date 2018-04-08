<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="GroupMember.aspx.cs" Inherits="ServicePoint.Setting.GroupMember" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        chkAuth();
        $(document).ready(function () {
            activeMenu('menuLeft','groupmember');
        });//]]
    </script>
    <form id="frm" runat="server">


        <div class="row-fluid">
            <div class="col-md-6 col-xs-12 ">
                <div class="panel panel-default">
                    <div class="panel-heading panel-info  ">회원 등급</div>
                    <div class="panel-body text-info ">

                        <h6>Owner:그룹 최고 관리자</h6>
                        <h6>Managet:그룹 관리 기능을 수행 할 수 있습니다.</h6>
                        <h6>Operator:기본적인 모니터링만 가능 합니다.</h6>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-xs-12 ">
                <div class="panel panel-default">
                    <div class="panel-heading panel-info  ">사용자 등록</div>
                    <div class="panel-body text-info ">

                        <h6>모니터링할 사용자를 추가합니다. </h6>
                        <h6>등급수정은 Manager 권한 이상 가능합니다</h6>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="col-lg-12">

                <div class="table-responsive">
                    <input type="button" class="btn btn-primary pull-right text-info" value="등록" onclick="modalAddMember();" />
                    <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="LightGray" CssClass="table table-hover table-bordered" OnRowDataBound="gv_List_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Email" HeaderText="E-Mail계정" />
                            <asp:BoundField DataField="Membername" HeaderText="이름" />
                            <asp:TemplateField HeaderText="멤버등급">
                                <ItemTemplate>
                                    <%# GetMemberGrade(Eval("Grade")) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <input type="button" class="btn btn-sm btn-warning" value="수정" onclick="modalUpdateMember('<%#Eval("Email").ToString()%>','<%#Eval("MemberName").ToString()%>',<%#Eval("MemberNum").ToString()%>,<%#Eval("Grade").ToString()%>)" />
                                    <input type="button" class="btn btn-sm btn-danger" value="삭제" onclick="DelMember(<%# Eval("MemberNum").ToString()%>)" />
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
<asp:Content ID="ModalContent" ContentPlaceHolderID="ModalContent" runat="server">
    <Modal:Groupmember ID="groupmember" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
