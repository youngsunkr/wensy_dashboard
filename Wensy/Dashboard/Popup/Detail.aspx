<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ServicePoint.Dashboard.Popup.Detail" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frm" runat="server" enableviewstate="false">
        <div class="col-12">
            <asp:HiddenField ID="hdn_ServerNum" runat="server" />
            <asp:PlaceHolder ID="phd" runat="server"></asp:PlaceHolder>
        </div>
            <asp:ScriptManager ID="scrMgr" runat="server"></asp:ScriptManager>
        <asp:Timer ID="tmr" runat="server"></asp:Timer>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
