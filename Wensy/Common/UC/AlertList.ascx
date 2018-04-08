<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlertList.ascx.cs" Inherits="ServicePoint.Common.UC.AlertList" %>

<div class="col-md-12 col-lg-12 col-xs-12">
    <div class="row">
        <div class="col-md-6 col-lg-6">
            <asp:DropDownList ID="ddl_Server" runat="server" AutoPostBack="true" CssClass=" pull-right"></asp:DropDownList>
        </div>

        <div class="col-md-6 col-lg-6  alert-check  pull-right">

            <ul class="list-unstyled list-inline">
                <li>
                    <asp:CheckBox runat="server" ID="chk_Critical" AutoPostBack="true" Checked="true" />
                    Critical</li>
                <li>
                    <asp:CheckBox runat="server" ID="chk_Warning" AutoPostBack="true" Checked="true" />
                    Warning</li>
                <li>
                    <asp:CheckBox runat="server" ID="chk_Information" AutoPostBack="true" Checked="true" />
                    Information</li>
            </ul>

        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-lg-12">
            <div class="table-responsive">
                <div class="contents">
                    <asp:GridView ID="gv_List" runat="server" EnableViewState="false" AutoGenerateColumns="false" HeaderStyle-BackColor="#333333" HeaderStyle-ForeColor="White" RowStyle-ForeColor="Black" RowStyle-BackColor="WhiteSmoke" RowStyle-HorizontalAlign="Center" CssClass=" table table-bordered col-md-12 nopadding " HeaderStyle-CssClass="nopadding" OnRowDataBound="gv_List_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="DisplayName" HeaderText="Server" ItemStyle-CssClass="col-md-1" />
                            <asp:TemplateField HeaderText="Alert Description" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="col-md-3">
                                <ItemTemplate>
                                    <%--<a onclick="window.open('<%# RedirectToAlertDetailPage(Eval("ServerType"), Eval("DisplayName"), Eval("ReasonCode"), Eval("InstanceName"), Eval("ServerNum"), Eval("TimeIn"), Eval("TimeIn_UTC") ) %>', '_blank', 'scrollbars=yes resizable=yes width=1200 height=900')" style="color: #1d1d1d;"><%# Eval("AlertDescription") %></a>--%>
                                    <%# Eval("AlertDescription") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="InstanceName" HeaderText="Instance" SortExpression="InstanceName" ItemStyle-CssClass="col-md-2" HeaderStyle-HorizontalAlign="Center" />
                            <%--<asp:BoundField DataField="RepeatCnt" HeaderText="#Counts" ItemStyle-CssClass="col-md-1" SortExpression="#Counts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />--%>
                            <asp:BoundField DataField="TimeIn" HeaderText="TimeIn" ItemStyle-CssClass="col-md-3" SortExpression="TimeIn" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                            <asp:BoundField DataField="AlertLevel" HeaderText="Level" SortExpression="AlertLevel" ItemStyle-CssClass="col-md-2" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeader="true" ItemStyle-ForeColor="White" />
                        </Columns>
                        <EmptyDataTemplate>
                            No Data
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</div>

