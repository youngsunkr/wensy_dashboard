<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlDashboard.ascx.cs" Inherits="ServicePoint.Common.UC.Dashboard.SqlDashboard" %>
<script type="text/javascript">
    function Detail(typeName, serverNum)
    {
        window.open('/Dashboard/popup/Detail.aspx?TypeName='+typeName+'&ServerNum='+serverNum, '_blank', 'scrollbars=yes resizable=yes width=1400 height=840');
    }
</script>
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
                        <li>SQL Version : <span>
                            <asp:Label ID="lbl_SQLVersion" runat="server"></asp:Label></span></li>
                        <li>SQL Edition : <span>
                            <asp:Label ID="lbl_SQLEdition" runat="server"></asp:Label></span></li>
                        <li>SQL Product Level : <span>
                            <asp:Label ID="lbl_SQLProductLevel" runat="server"></asp:Label></span></li>
                        <li>SQL Collation : <span>
                            <asp:Label ID="SQLCOllation" runat="server"></asp:Label></span></li>

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
                        <li><a href="#" onclick="Detail('SQL_CPU',<%=Servernum %>)">Total :</a><span>
                            <asp:Label ID="lbl_TotalCPU" runat="server"></asp:Label></span>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_CPU',<%=Servernum %>)">Systm : </a><span>
                            <asp:Label ID="lbl_KenelCPU" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_CPU',<%=Servernum %>)">User : </a><span>
                            <asp:Label ID="lbl_UserCPU" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_CPU',<%=Servernum %>)">SQL :</a><span>
                            <asp:Label ID="lbl_SQL" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_CPU',<%=Servernum %>)">Processor Queue Length :</a> <span>
                            <asp:Label ID="lbl_PQL" runat="server"></asp:Label></span></li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProcessPQL(numPQL,(numProcess * 2))  %></div>
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
                        <li>Output Queue Length :  <span>
                            <asp:Label ID="lbl_OutputQueueLength" runat="server"></asp:Label></span></li>
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
                        <li><a href="#" onclick="Detail('SQL_MemoryUsage',<%=Servernum %>)">Commited Bytes(GB)</a> </li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numCommited,numRamSize,"GB") %></div>
                        </li>
                         <li>Available Bytes(GB) </li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numAvailable,numRamSize,"GB") %></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_MemoryUsage',<%=Servernum %>)">SQL Target Server Memory</a> </li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numSQLTargetServerMemoryGB,numRamSize,"GB") %></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_MemoryUsage',<%=Servernum %>)">SQL Total Server Memory </a></li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Value(numSQLTOtalServerMemoryGB,numRamSize,"GB") %></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_Memory',<%=Servernum %>)">Batch Request:</a><span><asp:Label ID="lbl_BatchRequest" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_BATCHREQUEST_LINE_CHART"></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_Memory',<%=Servernum %>)">Buffer Cache Size:</a><span><asp:Label ID="lbl_BufferCacheSize" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_Memory',<%=Servernum %>)">Buffer Cache Hit(%):</a><span><asp:Label ID="lbl_BufferCacheHit" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_BUFFERCACHEHIT_LINE_CHART"></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_Memory',<%=Servernum %>)">Procedure Cache Size:</a><span><asp:Label ID="lbl_ProcedureCacheSize" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_Memory',<%=Servernum %>)">Procedure Cache Hit:</a><span><asp:Label ID="lbl_ProcedureCacheHit" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_PROCEDURECACHEHIT_LINE_CHART"></div>
                        </li>
                        <li>Page life Expectancy :<span><asp:Label ID="lbl_PagelifeExpectancy" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_PAGELIFEEXPECTANCY_COLS_CHART"></div>
                        </li>
                        <li>Compilations/sec :<span><asp:Label ID="lbl_CompilationSec" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_COMPILATIONSEC_COLS_CHART"></div>
                        </li>
                        <li>Re-Compilations/sec :<span><asp:Label ID="lbl_ReCompilationSec" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_RECOMPILATIONSEC_COLS_CHART"></div>
                        </li>
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
                        <li><a href="#" onclick="Detail('SQL_Disk',<%=Servernum %>)">Disk Free Space</a></li>
                        <li>
                            <div id="grp_<%= Servernum %>_FREEDISK_BAR_CHART"></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_Disk',<%=Servernum %>)">Max Disk time(%) : </a><span>
                            <asp:Label ID="lbl_MaxDiskTime" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_Disk',<%=Servernum %>)">Min Disk Idle Time(%) : </a> <span>
                            <asp:Label ID="lbl_MinDiskIdleTime" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_Disk',<%=Servernum %>)">Max Disk Queue Length :  </a><span>
                            <asp:Label ID="lbl_MaxDisk_Queuelength" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_Disk',<%=Servernum %>)">Physical Read/sec(Total) :  </a><span>
                            <asp:Label ID="lbl_PhysicalRead" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_Disk',<%=Servernum %>)">Physical Write/sec(Total) :  </a><span>
                            <asp:Label ID="lbl_PhysicalWrite" runat="server"></asp:Label></span></li>
                        <li>CheckPoint Page/sec :<span><asp:Label ID="lbl_CheckPointPageSec" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_CHECKPOINTPAGESEC_LINE_CHART"></div>
                        </li>
                        <li>Log Flush/sec :<span><asp:Label ID="lbl_LogFlushSec" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_LOGFLUSHSEC_LINE_CHART"></div>
                        </li>
                        <li>Lazy Write/sec :<span><asp:Label ID="lbl_LazyWriteSec" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_LAZYWRITESEC_LINE_CHART"></div>
                        </li>
                        <li>Read Ahead page :<span><asp:Label ID="lbl_ReadAheadPage" runat="server"></asp:Label></span></li>
                        <li>
                            <div id="grp_<%= Servernum %>_READAHEADPAGE_LINE_CHART"></div>
                        </li>
                    </ul>
                </div>
            </div>
        </section>
    </div>
    <div class="col-md-3">
        <section id="Session" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>Session</h5>
                    </div>
                </div>
                <div class="row contents bodybackground">
                    <ul class="list-unstyled">
                        <li>Response Time :<span><asp:Label ID="lbl_ResponseTime" runat="server"></asp:Label></span></li>
                        
                        <li><a href="#" onclick="Detail('SQL_Session',<%=Servernum %>)">Sessions : </a><span>
                            <asp:Label ID="lbl_Sessioin" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_Session',<%=Servernum %>)">Active Sessions : </a><span>
                            <asp:Label ID="lbl_ActiveSessions" runat="server"></asp:Label></span></li>
                        <li><a href="#" onclick="Detail('SQL_Session',<%=Servernum %>)">User Connections : </a><span>
                            <asp:Label ID="lbl_UserConnections" runat="server"></asp:Label></span></li>
                        <li> <div id="grp_<%= Servernum %>_USERCONNECTIONS_LINE_CHART"></div></li>
                       
                    </ul>
                </div>
            </div>
        </section>
        <%--<section id="Databases" class="dashboard">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h5>Databases</h5>
                    </div>
                </div>
                <div class="row contents  bodybackground">
                    <ul class="list-unstyled">
                        <li><a href="#" onclick="Detail('SQL_DatabasesFile',<%=Servernum %>)">Databases :</a><span><asp:Label ID="lbl_Databases" runat="server"></asp:Label></span></li>
                        
                        <li><a href="#" onclick="Detail('SQL_DatabasesFile',<%=Servernum %>)">DataFile Size :</a> <span><asp:Label ID="lbl_DataFileSize" runat="server"></asp:Label></span></li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Per(numDataFileSize_Use,numDataFileSize_Total,"%") %></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_DatabasesFile',<%=Servernum %>)">LogFile Size :</a><span><asp:Label ID="lbl_LogFileSieze" runat="server"></asp:Label></span></li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Per(numLogFileSize_Use,numLogFileSize_Total,"%") %></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_Index',<%=Servernum %>)">Max Index Flagmentation :</a><span><asp:Label ID="lbl_MaxIndexFlagmentation" runat="server"></asp:Label></span> </li>
                        <li>
                            <div class="nopadding"><%= ServicePoint.Lib.ChartClass.SetProgressBar_Per(numIndexFlag,100,"%") %></div>
                        </li>
                        <li><a href="#" onclick="Detail('SQL_DatabasesFile',<%=Servernum %>)">Max VLF : </a><span>
                            <asp:Label ID="lbl_MaxVLF" runat="server"></asp:Label></span></li>
                        <li>Active Transaction : <span>
                            <asp:Label ID="lbl_ActiveTransaction" runat="server"></asp:Label></span></li>
                        <li> <div id="grp_<%= Servernum %>_ACTIVETRANSACTION_LINE_CHART"></div></li>
                        <li>Active Temptable : <span>
                            <asp:Label ID="lbl_ActiveTempTable" runat="server"></asp:Label></span></li>
                        <li> <div id="grp_<%= Servernum %>_ACTIVETEMPTABLE_LINE_CHART"></div></li>
                        <li><a href="#" onclick="Detail('SQL_ServiceStatus',<%=Servernum %>)"><div><%= ServicePoint.Lib.ChartClass.SetCheckColor(numSQLService) %> &nbsp; SQL Service Status</div></a></li>
                        <li><a href="#" onclick="Detail('SQL_Agent',<%=Servernum %>)"><div><%= ServicePoint.Lib.ChartClass.SetCheckColor(numSQLAgent) %> &nbsp; SQL Job Agent Health</div></a></li>
                        <li><a href="#" onclick="Detail('SQL_Link',<%=Servernum %>)"><div><%= ServicePoint.Lib.ChartClass.SetCheckColor(numSQLLinked) %> &nbsp; SQL Linked Server Status</div></a></li>
                        <li><a href="#" onclick="Detail('SQL_Error',<%=Servernum %>)"><div><%= ServicePoint.Lib.ChartClass.SetCheckColor(numErrorLog) %> &nbsp; Errors in SQL Server Logs</div></a></li>
                    </ul>
                </div>
            </div>
        </section>--%>
    </div>
</div>
<asp:Literal ID="litScript_UC" runat="server"></asp:Literal>