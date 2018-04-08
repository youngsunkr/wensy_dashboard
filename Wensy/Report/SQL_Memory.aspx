<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="SQL_Memory.aspx.cs" Inherits="ServicePoint.Report.SQL_Memory" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            activeMenu('menuLeft', 'sqlmemory');
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
                        <div id="grp_<%= ServerNum.ToString()%>_TARGETSERVERMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Target Server Memory</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_TOTALSERVERMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Total Server Memory</div>
                </div>
            </div>
             <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_COMMITTEDBYTES_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Committed Bytes</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_AVAILABLEMBYTESBYTES_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Available Mbytes Bytes</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_BUFFER_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Buffer / Plan Cache Hit</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_PAGELIFE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Page life Expectancy</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_USERCONNECTIONS_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">User Connections</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_CONNECTIONMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Connection Memory (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_OPTIMIZERMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Optimizer Memory (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_LOCKMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Lock Memory (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_DATABASECACHE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Database Cache Memory (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_FREEMEMORY_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Free Memory (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_GRANTEDWORKSPACE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Granted Workspace Memory (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_MAXIMUMWORKSPACE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Maximum Workspace Memory (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_OUTSTANDING_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Memory Grants Outstanding</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_PENDING_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Memory Grants Pending</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_RESERVEDSERVER_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Reserved Server Memory</div>
                </div>
            </div>
             <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_SQLCACHE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">SQL Cache Memory</div>
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
