<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="SQL_PerformanceReport.aspx.cs" Inherits="ServicePoint.Report.SQL_PerformanceReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            activeMenu('menuLeft', 'sqlperformence');
        });//-->
        function chgPeriod(obj) {
            var endTime = new Date();

            var startDate = new Date(endTime.getTime() - (obj.value * 60 * 60 * 1000));
            var sday = startDate.getDate();
            var shour = startDate.getHours();

            $('#MainContent_txt_dtmStart').val(startDate.toString('yyyy-MM-dd'));
            $('#MainContent_txt_dtmEnd').val(endTime.toString('yyyy-MM-dd'));
            $('#MainContent_ddl_Hour_Start').val(shour);
        }
    </script>
    <form id="frm" runat="server">
        <div class="row-fluid well well-sm">
            <div class="col-lg-12">
                <div class="col-lg-1">서버 선택</div>
                <div class="col-lg-3">
                    <asp:DropDownList ID="ddl_Server" runat="server" CssClass="input-sm"></asp:DropDownList>
                </div>
                <div class="col-lg-1">조회기간</div>
                <div class="col-lg-3">
                    <asp:DropDownList ID="ddl_Time" runat="server" CssClass="input-sm" onchange="chgPeriod(this);"></asp:DropDownList>
                </div>
                <div class="col-lg-4">
                    <asp:Button ID="btn_Search" runat="server" CssClass="btn btn-default" Text="조회" />
                </div>
                <div class="col-lg-1">시작일</div>
                <div class="col-lg-3">
                    <asp:TextBox ID="txt_dtmStart" runat="server" CssClass="input-sm date"></asp:TextBox>
                    <asp:DropDownList ID="ddl_Hour_Start" runat="server" CssClass="input-sm "></asp:DropDownList>
                    <asp:DropDownList ID="ddl_Min_Start" runat="server" CssClass="input-sm"></asp:DropDownList>
                </div>
                <div class="col-lg-1">종료일</div>
                <div class="col-lg-3">
                    <asp:TextBox ID="txt_dtmEnd" runat="server" CssClass="input-sm date"></asp:TextBox>
                    <asp:DropDownList ID="ddl_Hour_End" runat="server" CssClass="input-sm "></asp:DropDownList>
                    <asp:DropDownList ID="ddl_Min_End" runat="server" CssClass="input-sm "></asp:DropDownList>
                </div>
                <div class="col-lg-4"></div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="table-responsive">
                <asp:GridView ID="gv_Info" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"  HeaderStyle-BackColor="LightGray">
                    <Columns>
                        <asp:BoundField HeaderText="Display Name" DataField="DisplayName" />
                        <asp:BoundField HeaderText="Host Name" DataField="HostName" />
                        <asp:BoundField HeaderText="Server Type" DataField="ServerType" />
                        <asp:BoundField HeaderText="Windows Version" DataField="Winver" />
                        <asp:TemplateField HeaderText="RAM Size">
                            <ItemTemplate>
                                <%# ServicePoint.Lib.Util.FormatBytes(Convert.ToInt64(Eval("RamSize"))) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="IP Address" DataField="IPAddress" />
                    </Columns>
                    <EmptyDataTemplate>No Data</EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <div class="row-fluid">
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_CPU_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">CPU</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_PROCESSQUEUELENGTH_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Process Queue Length</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_BATCHREQUEST_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Batch Requests/sec</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_PAGELIFEEXPECTANCY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Page Lif Expectancy</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_BUFFERPLANCACHE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Buffer / Plan Cache(%)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_COMPILATIONPERSEC_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Compilations / Re-Compilations per second</div>
                </div>
            </div>
            <div class="col-lg-5 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_NETWORK_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Network (KB)</div>
                </div>
            </div>
            <div class="col-lg-3 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_NETWORK_PIE_CHART"></div>
                    </div>
                    <div class="panel-footer">Network Bytes (KB)</div>
                </div>
            </div>
            <div class="col-lg-3 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_COMPILATION_PIE_CHART"></div>
                    </div>
                    <div class="panel-footer">Compilations / Re-Compilations</div>
                </div>
            </div>
            <div class="col-lg-12 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_SERVERMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Total Server Memory </div>
                </div>
            </div>
            <div class="col-lg-12 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_MEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Committed Memory (MB) / Target(SQL) Memory (MB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_ACTIVETMPTABLE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Active Temp Table</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_CREATETMPTABLE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Create Temp Table</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_USERCONNECTION_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">User Connection</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_CONNECTIONMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Connection Memory(KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_LOCKMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Lock Memory(KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_OPTIMIZEMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Optimize Memory (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_PAGESPLITS_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Page Splits / sec</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_CHECKPOINTPAGES_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">CheckPoint Pages/sec</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_TABLELOCK_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Table Lock Escalations/sec</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_LAZYWRITE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Lazy Writes/sec</div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_CONTTECTIONRESET_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Connection Reset/sec</div>
                </div>
            </div>
             <div class="col-lg-6">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_WORKFILES_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Workfiles </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_WORKTABLES_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Worktables Cerated/sec</div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_PAGEREADWRITE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Page Reads / Writes /sec</div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_PAGELOOKUP_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Page Lookups/sec</div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_READAHEADPAGE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Readahead Pages/sec</div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_LOGINLOGOUT_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Logins / Logouts /sec</div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_MEMORYGRANTSOUTSTANDING_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Memory Grants Outstanding / Pending</div>
                </div>
            </div>

        </div>
        <asp:Literal ID="litScript" runat="server"></asp:Literal>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
