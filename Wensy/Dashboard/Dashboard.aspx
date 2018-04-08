<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ServicePoint.Dashboard.Dashboard" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        //var oWidth = $(window).width();
        //$(document).ready(function () {
        //    setInterval(function () {
        //        onChangeWidth();
        //    }, 2000);
        //})
        function onChangeWidth() {
               location.reload();           
        }
        $(window).bind('resize', function () {  
            setTimeout(onChangeWidth, 2000);
        });
    </script>
    <form id="frm" runat="server">
        <div class="col-md-6 nopadding" <%-- style="background-color: ActiveBorder;"--%>>
            <asp:PlaceHolder ID="phd" runat="server"></asp:PlaceHolder>
        </div>
        <div class="col-md-6 nopadding" <%-- style="background-color: ActiveCaption;"--%>>
            <div class="row ">
               <%-- <Alert:AlertList ID="AlertList" runat="server" />--%>
            </div>
        </div> 
        <asp:ScriptManager ID="scrMgr" runat="server"></asp:ScriptManager>
        <asp:Timer ID="tmr" runat="server"></asp:Timer>
    </form>
</asp:Content>
<asp:Content ID="ModalContent" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
</asp:Content>
