<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="Win_CounterReport.aspx.cs" Inherits="ServicePoint.Report.Win_CounterReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            activeMenu('menuLeft', 'wincounter');
        });//-->
        function chgPeriod(obj) {
            var endTime = new Date();

            var startDate = new Date(endTime.getTime() - (obj.value * 60 * 1000));
            var sday = startDate.getDate();
            var shour = startDate.getHours();
            var smin = startDate.getMinutes();

            $('#MainContent_txt_dtmStart').val(startDate.toString('yyyy-MM-dd'));
            $('#MainContent_txt_dtmEnd').val(endTime.toString('yyyy-MM-dd'));
            $('#MainContent_ddl_Hour_Start').val(shour);
            $('#MainContent_ddl_Min_Start').val(smin);
        }
    </script>

    <form id="frm" runat="server">
        <div class="row-fluid well well-sm">
            <div class="col-lg-12">
                <div class="col-lg-1">서버 선택</div>
                <div class="col-lg-2">
                    <asp:DropDownList ID="ddl_Server" runat="server" CssClass="input-sm" AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="col-lg-1">조회기간</div>
                <div class="col-lg-2">
                    <asp:DropDownList ID="ddl_Time" runat="server" CssClass="input-sm" onchange="chgPeriod(this);"></asp:DropDownList>
                </div>

                <div class="col-lg-1">시작일</div>
                <div class="col-lg-2">
                    <asp:TextBox ID="txt_dtmStart" runat="server" CssClass="input-sm date"></asp:TextBox>
                    <asp:DropDownList ID="ddl_Hour_Start" runat="server" CssClass="input-sm "></asp:DropDownList>
                    <asp:DropDownList ID="ddl_Min_Start" runat="server" CssClass="input-sm"></asp:DropDownList>
                </div>
                <div class="col-lg-1">종료일</div>
                <div class="col-lg-2">
                    <asp:TextBox ID="txt_dtmEnd" runat="server" CssClass="input-sm date"></asp:TextBox>
                    <asp:DropDownList ID="ddl_Hour_End" runat="server" CssClass="input-sm "></asp:DropDownList>
                    <asp:DropDownList ID="ddl_Min_End" runat="server" CssClass="input-sm "></asp:DropDownList>
                </div>
                <div class="col-lg-1">인스턴스</div>
                <div class="col-lg-8">
                    <asp:DropDownList ID="ddl_Instance" runat="server" CssClass="input-sm"></asp:DropDownList>
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btn_Search" runat="server" CssClass="btn btn-default" Text="조회" OnClick="btn_Search_Click" />
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="table-responsive">
                <asp:GridView ID="gv_Info" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray">
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
            <div class="col-lg-12 ">
                <asp:Panel ID="pnl_chart" runat="server" Visible="false">
                    <div class="panel panel-default nodpadding">
                        <div class="panel-body">
                            <div id="grp_<%= ServerNum.ToString()%>_LINE_CHART"></div>
                        </div>
                        <div class="panel-footer">
                            <div>
                                <h4>Data List
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_AvgTimeTaken" aria-controls="collapse_gv_List_AvgTimeTaken">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                                </h4>
                            </div>
                            <div class="collapse" id="collapse_gv_List_AvgTimeTaken">
                                <asp:GridView ID="gv_List" runat="server" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray">
                                    <Columns>
                                    </Columns>
                                    <EmptyDataTemplate>No Data</EmptyDataTemplate>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <asp:Literal ID="litScript" runat="server"></asp:Literal>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
