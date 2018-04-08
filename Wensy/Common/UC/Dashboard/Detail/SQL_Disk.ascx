<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SQL_Disk.ascx.cs" Inherits="ServicePoint.Common.UC.Dashboard.Detail.SQL_Disk" %>
<div class="col-lg-12">
    <div class="row-fluid">
        <h3 class="text-center">SQL Disk</h3>
    </div>
    <div class="row-fluid">
        <div class="col-lg-12">
            <div class="row-fluid">
                <div class="col-lg-6 ">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_DISKTIME_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">DiskTime</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_DISKIDLETIME_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Disk Idle Time</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_CURRENTDISKQUEUELENGTH_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Current Disk Queue Length</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_AVGDISKQUEUE_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Avg. Disk Queue Length</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_AVGDISKBYTESREAD_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Avg. Disk Bytes / Read</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_AVGDISKBYTESWRITE_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Avg. Disk Bytes / Write</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_FREEDISKSPACEMB_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Free Disk Space(MB)</div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_FREEDISKSPACEPER_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">Free Disk Space(%)</div>
                    </div>
                </div>
            </div>
        </div>

        <asp:Literal ID="litScript_Pop" runat="server"></asp:Literal>
    </div>
</div>
