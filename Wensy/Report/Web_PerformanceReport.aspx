<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="Web_PerformanceReport.aspx.cs" Inherits="ServicePoint.Report.Web_PerformanceReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            activeMenu('menuLeft', 'webperformence');
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
                    <asp:BoundField HeaderText="RAM Size" DataField="RAMSize" />
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
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body  ">
                        <div id="grp_<%= ServerNum.ToString()%>_PROCESSORQUEUELENGTH_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">CPU - Processor Queue Length</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_ASPREQUEST_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">ASP/ASP.NET Requests</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_CURRENTCONNECTIONREQUEST_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Current Connections, and Request Execution Time</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_COMMITEDBYTE_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Committed MBytes(MB) , Available MBytes</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_W3WP_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">W3WP - Worker Processes Memory(MB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_NETWORKMAX_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Network Interface Max (KB)</div>
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_NETWORKWEB_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Network Interface Web (KB)</div>
                </div>
            </div>
            <div class="col-lg-5 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_DISKTIME_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Disk Time(%), and Avg Disk Read Time(ms)</div>
                </div>
            </div>
            <div class="col-lg-4 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_FREEDISK_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">Free Disk Space</div>
                </div>
            </div>
            <div class="col-lg-3 ">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_FREEDISK_BAR_CHART"></div>
                    </div>
                    <div class="panel-footer">Free Disk Space</div>
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
