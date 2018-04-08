<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SQL_MemoryUsage.ascx.cs" Inherits="ServicePoint.Common.UC.Dashboard.Detail.SQL_MemoryUsage" %>
<div class="col-lg-12">
    <div class="row-fluid">
        <h3 class="text-center">SQL Memory Usage</h3>
    </div>
    <div class="row-fluid">
        <div class="col-lg-8">
            <div class="row-fluid">
                <div class="col-lg-12 ">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_COMMITTED_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Committed Memory</div>
                    </div>
                </div>
                <div class="col-lg-12">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_AVAILABLE_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Available Memory</div>
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