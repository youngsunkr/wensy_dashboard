<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="PopChart.aspx.cs" Inherits="ServicePoint.PopChart" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        //var ctname = $("#MainContent_ct_name").val();
        //alert(ctname);
        //var x = window.opener.NO_0_LINE_CHARTValue;
        //alert(x);

    </script>
    <form id="frm" runat="server">
        <div id="grp_LINE_CHART"></div>
        <asp:HiddenField ID="ct_name" runat="server" />
        <asp:Literal ID="litScript" runat="server"></asp:Literal>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
