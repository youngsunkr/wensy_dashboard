<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SQL_DatabasesFile.ascx.cs" Inherits="ServicePoint.Common.UC.Dashboard.Detail.SQL_DatabasesFile" %>
<div class="col-lg-12">
    <div class="row-fluid">
        <h3 class="text-center">SQL Databases File</h3>
    </div>

    <div class="row-fluid">
        <div class="col-lg-12">
            <div class="row-fluid">
                <div class="table-responsive">
                    <asp:GridView ID="gv_List" runat="server" CssClass="table table-border" HeaderStyle-BackColor="LightGray">
                        <Columns></Columns>
                        <EmptyDataTemplate>No Data</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <div class="row-fluid">
                <div class="col-lg-6 ">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_TOTALDATABASESSIZEMB_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Total_Databases_Size_MB</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_DATAFILESIZEMB_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">DataFile_Size_MB</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_LOGSIZEMB_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Log_Size_MB</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_TOTALVLFCOUNT_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Total_Vlf_Count</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_ACTIVEVLFCOUNT_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Active_Vlf_Count</div>
                    </div>
                </div>
            </div>
        </div>

        <asp:Literal ID="litScript_Pop" runat="server"></asp:Literal>
    </div>
</div>
