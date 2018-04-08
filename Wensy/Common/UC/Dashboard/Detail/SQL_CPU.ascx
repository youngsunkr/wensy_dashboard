<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SQL_CPU.ascx.cs" Inherits="ServicePoint.Common.UC.Dashboard.Detail.SQL_CPU" %>
<div class="col-lg-12">
    <div class="row-fluid">
        <h3 class="text-center">SQL CPU</h3>
    </div>
    <div class="row-fluid">
        <div class="col-lg-8">
            <div class="row-fluid">
                <div class="col-lg-12 ">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_CPU_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">CPU (%)</div>
                    </div>
                </div>
                <div class="col-lg-12">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_QUEUE_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Process Queue Length</div>
                    </div>
                </div>

            </div>
        </div>
        <div class="cold-lg-4">
            <div class="table-responsive">
                <asp:GridView ID="gv_List" runat="server" CssClass="table table-border"  HeaderStyle-BackColor="LightGray">
                    <Columns>
                    </Columns>
                    <EmptyDataTemplate>No Data</EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <asp:Literal ID="litScript_Pop" runat="server"></asp:Literal>
    </div>
</div>
