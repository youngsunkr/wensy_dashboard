﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebDashboard.ascx.cs" Inherits="ServicePoint.Common.UC.Dashboard.WebDashboard" %>
<div class="row-fluid">
    <div class="col-md-3">
        <section id="system" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>System</h5>
                    </div>
                </div>
                <div class="row contents bodybackground">
                    <ul class="list-unstyled">
                        <li>HostName : <span>
                            <asp:Label ID="lbl_HostName" runat="server"></asp:Label></span></li>
                        <li>CPU Cores : <span>
                            <asp:Label ID="lbl_CPUCore" runat="server"></asp:Label></span></li>
                        <li>RAM Size : <span>
                            <asp:Label ID="lbl_RAMSize" runat="server"></asp:Label>
                        </span></li>
                        <li>OS Version : <span>
                            <asp:Label ID="lbl_OSVersion" runat="server"></asp:Label></span></li>

                    </ul>
                </div>
            </div>
        </section>
        <section id="process" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>Process</h5>
                    </div>
                </div>
                <div class="row contents bodybackground">
                    <ul class="list-unstyled">
                        <li>
                            <div id="grp_<%= Servernum %>_TOTALCPU_COLS_CHART"></div>
                        </li>
                        <li>Total : <span>
                            <asp:Label ID="lbl_TotalCPU" runat="server"></asp:Label></span>
                        </li>
                        <li>Systm : <span>
                            <asp:Label ID="lbl_KenelCPU" runat="server"></asp:Label></span></li>
                        <li>User : <span>
                            <asp:Label ID="lbl_UserCPU" runat="server"></asp:Label></span></li>
                        <li>IIS : <span>
                            <asp:Label ID="lbl_WorkerProcesses" runat="server"></asp:Label></span>
                        </li>
                        <li>
                            <div id="grp_<%= Servernum %>_WORKERPROCESSES_LINE_CHART"></div>
                        </li>
                        <li>Processor Queue Length : <span>
                            <asp:Label ID="lbl_PQL" runat="server"></asp:Label></span></li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProcessPQL(numPQL,(numProcess * 2))  %></div>
                        </li>

                    </ul>
                </div>
            </div>
        </section>
    </div>
    <div class="col-md-3">
        <section id="memory" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>Memory</h5>
                    </div>
                </div>
                <div class="row contents bodybackground">
                    <ul class="list-unstyled">
                        <li>Commited Bytes(GB) </li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numCommited,numRamSize,"GB") %></div>
                        </li>
                        <li>Available Bytes(GB) </li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numAvailable,numRamSize,"GB") %></div>
                        </li>
                        <li>IIS Total Worker Memory</li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numTotalWorketProcessmemory,numRamSize,"GB") %></div>
                        </li>
                        <li>IIS Max Worker Memory</li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numMaxWorkerProcessmemory,numRamSize,"GB") %></div>
                        </li>
                    </ul>
                </div>
            </div>
        </section>
        <section id="network" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>NetWork</h5>
                    </div>
                </div>
                <div class="row contents bodybackground">
                    <ul class="list-unstyled">
                        <li>Bytes Total/sec :  <span>
                            <asp:Label ID="lbl_BytesTotal" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_BYTESTOTAL_LINE_CHART"></div>
                        </li>

                        <li>Bytes Sent/sec :  <span>
                            <asp:Label ID="lbl_BytesSent" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_BYTESSENT_LINE_CHART"></div>
                        </li>
                        <li>Bytes Received/sec :  <span>
                            <asp:Label ID="lbl_BytesReceived" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_BYTESRECEIVED_LINE_CHART"></div>
                        </li>
                        <li>Bytes total/sec (IIS) : <span>
                            <asp:Label ID="lbl_BytesTotalSecWeb" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_BYTESTOTALSECWEB_LINE_CHART"></div>
                        </li>
                        <li>Output Queue Length :  <span>
                            <asp:Label ID="lbl_OutputQueueLength" runat="server"></asp:Label></span></li>
                    </ul>
                </div>
            </div>
        </section>
    </div>
    <div class="col-md-3">
        <section id="disk" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>Disk</h5>
                    </div>
                </div>
                <div class="row contents bodybackground">
                    <ul class="list-unstyled">
                        <li>Disk Free Space</li>
                        <li>
                            <div id="grp_<%= Servernum %>_FREEDISK_BAR_CHART"></div>
                        </li>
                        <li>Max Disk time(%) : <span>
                            <asp:Label ID="lbl_MaxDiskTime" runat="server"></asp:Label></span></li>
                        <li>Min Disk Idle Time(%) : <span>
                            <asp:Label ID="lbl_MinDiskIdleTime" runat="server"></asp:Label></span></li>
                        <li>Max Disk Queue Length : <span>
                            <asp:Label ID="lbl_MaxDisk_Queuelength" runat="server"></asp:Label></span></li>
                        <li>Physical Read/sec(Total) : <span>
                            <asp:Label ID="lbl_PhysicalRead" runat="server"></asp:Label></span></li>
                        <li>Physical Write/sec(Total) : <span>
                            <asp:Label ID="lbl_PhysicalWrite" runat="server"></asp:Label></span></li>
                    </ul>
                </div>
            </div>
        </section>
    </div>
    <div class="col-md-3">
        <section id="WebService" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>WEB Service</h5>
                    </div>
                </div>
                <div class="row contents bodybackground">
                    <ul class="list-unstyled">
                        <li>Requests/sec : <span>
                            <asp:Label ID="lbl_RequestSec" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_REQUESTSEC_LINE_CHART"></div>
                        </li>
                        <li>Active Requests : <span>
                            <asp:Label ID="lbl_ActiveRequest" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_ACTIVEREQUEST_COLS_CHART"></div>
                        </li>
                        <li>Current Connections : <span>
                            <asp:Label ID="lbl_CurrentConnection" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_CURRENTCONNECTION_COLS_CHART"></div>
                        </li>
                        <li>Current Users : <span>
                            <asp:Label ID="lbl_CurrentUsers" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_CURRENTUSERS_COLS_CHART"></div>
                        </li>
                        <li>Web to DB Connections : <span>
                            <asp:Label ID="lbl_WebToDbConnections" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_WEBTODBCONNECTIONS_LINE_CHART"></div>
                        </li>
                    </ul>
                </div>
            </div>
        </section>
        <section id="ASP_NET" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>ASP.NET</h5>
                    </div>
                </div>
                <div class="row contents bodybackground">
                    <ul class="list-unstyled">
                        <li>Request Execution Time : <span>
                            <asp:Label ID="lbl_RequestExecutionTime" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_REQUESTEXECUTIONTIME_LINE_CHART"></div>
                        </li>
                        <li>Requests Executing : <span>
                            <asp:Label ID="lbl_RequestsExecutiong" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_REQUESTSEXECUTIONG_LINE_CHART"></div>
                        </li>
                        <li>Requests Queued </li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numRequestsQueued,numRamSize) %></div>
                        </li>
                        <li>Requests/Sec : <span>
                            <asp:Label ID="lbl_RequestsSec" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_REQUESTSSEC_COLS_CHART"></div>
                        </li>
                    </ul>
                </div>
            </div>
        </section>
    </div>
</div>
<asp:Literal ID="litScript_UC" runat="server"></asp:Literal>
