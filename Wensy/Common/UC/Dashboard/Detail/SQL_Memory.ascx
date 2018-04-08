<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SQL_Memory.ascx.cs" Inherits="ServicePoint.Common.UC.Dashboard.Detail.SQL_Memory" %>
<div class="col-lg-12">
    <div class="row-fluid">
        <h3 class="text-center">SQL Memory</h3>
    </div>
    <div class="row-fluid">
        <div class="col-lg-12">
            <div class="row-fluid">
                <div class="col-lg-6 ">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_SERVERMEMORY_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Server Memory</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_MEMORYAREAS_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Memory Areas</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_CACHERATES_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Cache Rates</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_CACHESIZE_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Cache Size</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_PROCCACHESIZE_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Procedure Cache Size by Object Type</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_PROCCACHEHIT_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Procedure Cache hit by Object Type</div>
                    </div>
                </div>
            </div>
        </div>

        <asp:Literal ID="litScript_Pop" runat="server"></asp:Literal>
    </div>
</div>
