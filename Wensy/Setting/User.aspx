<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="ServicePoint.Setting.User" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            activeMenu('menuLeft', 'userinfo');
        });//]]
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <form id="frm" runat="server" class="form-horizontal">
        <asp:HiddenField ID="hdn_Email" runat="server" />
        <asp:Literal ID="litScript" runat="server"></asp:Literal>

        <div class="row-fluid">
           
            
            <div class="col-lg-12 well">
                 <div class="col-lg-12">

                <div class="">
                    <div class="form-group"><h4>개인정보변경</h4></div>
                </div>

            </div>
                <div class="form-group">
                    <asp:Label ID="lbl_Email" runat="server" for="txt_Email" CssClass="col-lg-2 control-label">Email</asp:Label>
                    <div class="col-lg-10">
                        <asp:TextBox ID="txt_Email" runat="server" CssClass="form-control" ReadOnly="true" TextMode="Email" disabled="true"></asp:TextBox>
                    </div>

                </div>
                 <div class="form-group">
                    <asp:Label ID="Label3" runat="server" CssClass="col-lg-2 control-label">등록일</asp:Label>
                    <div class="col-lg-10">
                        <asp:TextBox ID="txt_dtmReg" runat="server" CssClass="form-control" ReadOnly="true" disabled="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="lbl_Tel" runat="server" CssClass="col-lg-2 control-label">휴대폰 번호</asp:Label>
                    <div class="col-lg-10">
                        <asp:TextBox ID="txt_Tel" runat="server" TextMode="Number" CssClass="form-control" onkeypress="return allowOnlyNumber(event)" placeholder="숫자만 입력해주세요!"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="lbl_Name" runat="server" CssClass="col-lg-2 control-label">이름</asp:Label>
                    <div class="col-lg-10">
                        <asp:TextBox ID="txt_Name" runat="server" CssClass="form-control" placeholder="이름"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="lbl_Company" runat="server" CssClass="col-lg-2 control-label">회사명</asp:Label>
                    <div class="col-lg-10">
                        <asp:TextBox ID="txt_Company" runat="server" CssClass="form-control" placeholder="회사명"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label2" runat="server" CssClass="col-lg-2 control-label">비밀번호</asp:Label>
                    <div class="col-lg-10">
                        <asp:TextBox ID="txt_Pass" runat="server" TextMode="Password" CssClass="form-control" placeholder="비밀번호를 입력해주세요"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-lg-4"></div>
                    <div class="col-lg-8">
                        <asp:Button ID="btn_Update" runat="server" CssClass="btn btn-primary" Text="개인정보수정하기" OnClick="btn_Update_Click" />
                    </div>
                </div>
                
             
            </div>
            
           
            <div class="col-lg-12 well well-sm" >
                 <div class="col-lg-12">

                <div class="">
                    <div class="form-group"><h4>비밀번호변경</h4></div>
                </div>

            </div>
                <div class="form-group">
                    <asp:Label ID="lbl_Pass_Confirm" runat="server" CssClass="col-lg-2 control-label">비밀번호변경</asp:Label>
                    <div class="col-lg-10">
                        <asp:TextBox ID="txt_Pass_new" runat="server" TextMode="Password" CssClass="form-control" placeholder="최대 25자 이내"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label1" runat="server" CssClass="col-lg-2 control-label">비밀번호변경 확인</asp:Label>
                    <div class="col-lg-10">
                        <asp:TextBox ID="txt_Pass_new_confirm" runat="server" TextMode="Password" CssClass="form-control" placeholder="최대 25자 이내"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-lg-4"></div>
                    <div class="col-lg-8">
                        <asp:Button ID="btn_PassUpdate" runat="server" CssClass="btn btn-primary" Text="비밀번호수정하기" OnClick="btn_PassUpdate_Click" />
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