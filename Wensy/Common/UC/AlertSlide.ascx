<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlertSlide.ascx.cs" Inherits="ServicePoint.Common.UC.AlertSlide" %>
<div class="col-md-12 row">
    <h3 class="no-bold">Alerts in last <%= ConfigurationManager.AppSettings["AlertDataDuration"].ToString() %> minutes</h3>
</div>
<div class="col-md-12">
    <asp:Repeater ID="rpt_Alert" runat="server">
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
                            <div class="col-sm-4 col-md-42 bg-alert-text box">
                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                    <div class="carousel-inner">
                                        <%# GetAlertList(Container.DataItem.ToString(),0) %>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-4 bg-alert-text box">
                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                    <div class="carousel-inner">
                                        <%# GetAlertList(Container.DataItem.ToString(),1) %>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-4 bg-alert-text box">
                                <div class="carousel slide" data-interval="3000" data-ride="carousel">
                                    <div class="carousel-inner">
                                        <%# GetAlertList(Container.DataItem.ToString(),2) %>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ItemTemplate>
        <FooterTemplate>
            <asp:Label ID="lblEmptyData" runat="server" CssClass="col-md-12 text-info" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' BackColor="White" Text="No Data" />
        </FooterTemplate>
    </asp:Repeater>
</div>
