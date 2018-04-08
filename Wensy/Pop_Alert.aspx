<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="Pop_Alert.aspx.cs" Inherits="ServicePoint.Pop_Alert" %>

    <asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
        <form id="frm" runat="server">

            <div>

                <div class="col-md-12  col-lg-12 row">
                    <div class="">
                        <Alert:AlertList ID="AlertList" runat="server" EnableViewState="true" />
                    </div>
                </div>
            </div>
            <asp:ScriptManager ID="scrMgr" runat="server" EnableViewState="false"></asp:ScriptManager>
            <asp:Timer ID="tmr" runat="server" EnableViewState="false"></asp:Timer>
        </form>
    </asp:Content>
    <asp:Content ID="ModalContent" ContentPlaceHolderID="ModalContent" runat="server">
    </asp:Content>
    <asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
        <asp:Literal runat="server" ID="litScript"></asp:Literal>
    </asp:Content>
