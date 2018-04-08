<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="RuleEditorByPCID.aspx.cs" Inherits="ServicePoint.Setting.RuleEditorByPCID" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript">
        chkAuth();
        $(document).ready(function () {
            activeMenu('menuLeft', 'ruleeditorbypcid');
        });//]]
    </script>
     <form id="frm" runat="server">
        <div class="row-fluid">
            <asp:HiddenField ID="hdn_ServerNum" runat="server" />
             <asp:HiddenField ID="hdn_DisplayName" runat="server" />
            <div class="col-lg-12">
                <div class="row-fluid">
                    <div class="col-lg-4">
                        <h4>서버선택</h4>
                       <asp:DropDownList ID="ddl_Server" runat="server" OnSelectedIndexChanged="ddl_Server_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </div>
                    <div class="col-lg-4">
                        <h4>성능오브젝트 선택</h4>
                       <asp:DropDownList ID="ddl_Object" runat="server"  OnSelectedIndexChanged="ddl_Object_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </div>
                    <div class="col-lg-4">
                        <h4>성능카운터 선택</h4>
                       <asp:DropDownList ID="ddl_Counter" runat="server" OnSelectedIndexChanged="ddl_Counter_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="col-lg-12">
                        <h4>등록된 인스턴스</h4>
                       <asp:GridView ID="gv_List" runat="server">
                           <Columns></Columns>
                           <EmptyDataTemplate>NO Data</EmptyDataTemplate>
                       </asp:GridView>
                    </div>
                    <div class="col-lg-12">
                        <h4>알림리스트</h4>
                       <asp:GridView ID="gv_List_Rule" runat="server">
                           <Columns></Columns>
                           <EmptyDataTemplate>NO Data</EmptyDataTemplate>
                       </asp:GridView>
                    </div>
                </div>
            </div>
           
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
