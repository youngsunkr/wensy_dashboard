<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="ServicePoint.LogIn.Search" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frm" runat="server">
        <div class="row-fluid">
            <div class="col-lg-4"></div>
            <div class="col-lg-4 form-horizontal well">
                <fieldset>
                    <div class="form-group">
                        <h3 class="text-center">비밀번호 찾기</h3>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strEmail">
                            E-Mail 계정
                        </label>
                        <div class="controls col-sm-9 col-xs-9">

                            <asp:TextBox ID="txt_Email" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strTel_Confirm">
                            연락처 입력
                        </label>
                        <div class="controls col-sm-9 col-xs-9">

                            <asp:TextBox ID="txt_strTel_Confirm" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lbl_Text" runat="server" CssClass="col-sm-12 col-xs-12 text-center text-danger "></asp:Label>
                    </div>
                    <div class="form-group">
                        <div class="controls col-sm-12 col-xs-12">
                            <asp:Button ID="btn_Search" runat="server" CssClass="form-control btn btn-primary" Text="비밀번호찾기" OnClick="btn_Search_Click" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="controls col-sm-12 col-xs-12">
                            <input type="button" id="btn_Login" value="LogIn" class="form-control btn btn-primary"  onclick="location.href = '/login/login.aspx';" />
                        </div>
                    </div>
                </fieldset>
            </div>

            <div class="col-lg-4"></div>
        </div>
        <asp:Literal ID="litScript" runat="server"></asp:Literal>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
