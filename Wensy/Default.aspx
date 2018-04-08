<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ServicePoint.Default" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frm" runat="server" enableviewstate="false">
        <script type="text/javascript">
     <!--
    //-->
    function OpenAlert(servername) {
        window.open("/Pop_Alert.aspx?grname="+servername, servername, "width=1000,height=700,scrollbars=yes,location=no,left=150,top=150"
    );
           
    }
        </script>

        <div class="col-md-12 col-xs-12">
            <asp:Repeater ID="rpt_ServerStatus" runat="server" EnableViewState="false" ><%--OnItemCommand="rpt_ServerStatus_ItemCommand">--%>
                <ItemTemplate>
                    <section id="section_<%# Container.DataItem.ToString() %>" class="status">
                        <div class="row">
                            <div class="col-md-12">
                                <h3><%# Container.DataItem.ToString() %>
                                    <%--<button type="button" id="btn_<%# Container.DataItem.ToString() %>" class="btn btn-warning" onclick="OpenAlert('<%# GetUrlEncodingValue(Container.DataItem.ToString()) %>');">ALERT MESSAGE</button></h3>--%>
                                
                            </div>
                        </div>
                        <%# GetServerList(Container.DataItem.ToString()) %>
                    </section>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="No Data" />
                </FooterTemplate>
            </asp:Repeater>
            <%-- <div>
                <div class="row">
                    <div class="col-lg-12">
                        <h3 class="no-bold">Alerts in last <%= ConfigurationManager.AppSettings["AlertDataDuration"].ToString() %> minutes</h3>
                    </div>
                </div>
                <div class="alerts-contents">
                    <asp:Repeater ID="rpt_Alert" runat="server" EnableViewState="false">
                        <ItemTemplate>
                            <section id="alert-app-servers_<%# Container.DataItem.ToString() %>" class="alerts">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-1 nopadding">
                                            <div class="col-sm-12 col-md-12 bg-alert-default text-center box">
                                                <h4><%# Container.DataItem.ToString() %></h4>
                                            </div>
                                        </div>
                                        <div class="col-md-11 nopadding">
                                            <div class="col-sm-2 col-md-2 bg-alert-text box">
                                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                                    <div class="carousel-inner">
                                                        <%# GetAlertList(Container.DataItem.ToString(),0) %>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-2 col-md-2 bg-alert-text box">
                                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                                    <div class="carousel-inner">
                                                        <%# GetAlertList(Container.DataItem.ToString(),1) %>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-2 col-md-2 bg-alert-text box">
                                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                                    <div class="carousel-inner">
                                                        <%# GetAlertList(Container.DataItem.ToString(),2) %>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-2 col-md-2 bg-alert-text box">
                                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                                    <div class="carousel-inner">
                                                        <%# GetAlertList(Container.DataItem.ToString(),3) %>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-2 col-md-2 bg-alert-text box">
                                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                                    <div class="carousel-inner">
                                                        <%# GetAlertList(Container.DataItem.ToString(),4) %>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-2 col-md-2 bg-alert-text box">
                                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                                    <div class="carousel-inner">
                                                        <%# GetAlertList(Container.DataItem.ToString(),5) %>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </ItemTemplate>
                            <FooterTemplate>
                    <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="No Data" />
                </FooterTemplate>
                    </asp:Repeater>

                </div>
            </div>--%>
        </div>
        <asp:ScriptManager ID="scrMgr" runat="server" EnableViewState="false"></asp:ScriptManager>
        <asp:Timer ID="tmr" runat="server" EnableViewState="false"></asp:Timer>
    </form>
</asp:Content>
