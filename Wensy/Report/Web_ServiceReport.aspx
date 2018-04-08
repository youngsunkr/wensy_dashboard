<%@ Page Title="" Language="C#" MasterPageFile="~/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="Web_ServiceReport.aspx.cs" Inherits="ServicePoint.Report.Web_ServiceReport" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            activeMenu('menuLeft', 'webservice');
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
                        <div id="grp_<%= ServerNum.ToString()%>_AVGTIMETAKEN_COLSHORIZON_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>AVG Time Taken Analysis(ms,Top 20)
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_AvgTimeTaken" aria-controls="collapse_gv_List_AvgTimeTaken">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                            </h4>
                        </div>
                        <div class="collapse" id="collapse_gv_List_AvgTimeTaken">
                            <asp:GridView ID="gv_List_AvgTimeTaken" runat="server" CssClass="table table-bordered " AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField DataField="URI" HeaderText="URI" />
                                    <asp:BoundField DataField="Average Time Taken" HeaderText="Average Time Taken" />
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body  ">
                        <div id="grp_<%= ServerNum.ToString()%>_MAXTIMETAKEN_COLSHORIZON_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>MAX Time Taken Analysis(ms,Top 20)
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_MaxTimeTaken" aria-controls="collapse_gv_List_MaxTimeTaken">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                            </h4>
                        </div>
                        <div class="collapse" id="collapse_gv_List_MaxTimeTaken">
                            <asp:GridView ID="gv_List_MaxTimeTaken" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField DataField="URI" HeaderText="URI" />
                                    <asp:BoundField DataField="Max Time Taken" HeaderText="Max Time Taken" />
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-6 ">
                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_TOTALBYTES_COLSHORIZON_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>Server To Client Bytes (top 20) - total byte
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_TotalBytes" aria-controls="collapse_gv_List_TotalBytes">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                            </h4>
                        </div>
                        <div class="collapse" id="collapse_gv_List_TotalBytes">
                            <asp:GridView ID="gv_List_TotalBytes" runat="server" CssClass="table table-bordered " AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField DataField="URI" HeaderText="URI" />
                                    <asp:BoundField DataField="Total Bytes from Server" HeaderText="Total Bytes from Server" />
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body  ">
                        <div id="grp_<%= ServerNum.ToString()%>_TOTALHIT_COLSHORIZON_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>Server To Client Bytes (top 20) - total Hit
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_TotalHit" aria-controls="collapse_gv_List_TotalHit">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                            </h4>
                        </div>
                        <div class="collapse" id="collapse_gv_List_TotalHit">
                            <asp:GridView ID="gv_List_TotalHit" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField DataField="URI" HeaderText="URI" />
                                    <asp:BoundField DataField="Total Hits" HeaderText="Total Hits" />
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_HIT_COLSHORIZON_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>top 20 - HIT
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_Hit" aria-controls="collapse_gv_List_Hit">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                            </h4>
                        </div>
                        <div class="collapse" id="collapse_gv_List_Hit">
                            <asp:GridView ID="gv_List_Hit" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField DataField="hit" HeaderText="LogValue" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_APPHIT_COLSHORIZON_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>top 20 - Application HIT (asp,asp.net,asmx)
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_App_Hit" aria-controls="collapse_gv_List_App_Hit">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                            </h4>
                        </div>
                        <div class="collapse" id="collapse_gv_List_App_Hit">
                            <asp:GridView ID="gv_List_App_Hit" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField DataField="App_hit" HeaderText="LogValue" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_IP_COLSHORIZON_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>top 20 - IP Addresses
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_Ip" aria-controls="collapse_gv_List_Ip">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                            </h4>
                        </div>
                        <div class="collapse" id="collapse_gv_List_Ip">
                            <asp:GridView ID="gv_List_IP" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField DataField="IP" HeaderText="LogValue" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_BYTEPEREXTENSION_COLSHORIZON_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>top 20 - Bytes per Extension
                                <button type="button" class="pull-right btn btn-primary" data-toggle="collapse" data-target="#collapse_gv_List_BytePerExtenstion" aria-controls="collapse_gv_List_BytePerExtenstion">
                                    <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                                </button>
                            </h4>
                        </div>
                        <div class="collapse" id="collapse_gv_List_BytePerExtenstion">
                            <asp:GridView ID="gv_List_BytePerExtenstion" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                                <Columns>
                                    <asp:BoundField DataField="BytePerExtension" HeaderText="LogValue" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                </Columns>
                                <EmptyDataTemplate>No Data</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body table-responsive">
                        <asp:GridView ID="gv_List_Err" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="URI" HeaderText="URI" />
                                <asp:BoundField DataField="Total Hits" HeaderText="Total Hits" />
                                <asp:BoundField DataField="Status Code" HeaderText="Status Code" />
                                <asp:BoundField DataField="Win32 Status Code" HeaderText="Win32 Status Code" />
                            </Columns>
                            <EmptyDataTemplate>No Data</EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>Error Log
                            </h4>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-body table-responsive">
                        <asp:GridView ID="gv_List_ServiceStatus" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  HeaderStyle-BackColor="LightGray">
                            <Columns>
                                <asp:BoundField DataField="Total Hits" HeaderText="Total Hits" />
                                <asp:BoundField DataField="Total Bytes from Server" HeaderText="Total Bytes from Server" />
                                <asp:BoundField DataField="Total Bytes from Clients" HeaderText="Total Bytes from Clients" />
                                <asp:BoundField DataField="Total Client IP" HeaderText="Total Client IP" />
                                <asp:BoundField DataField="Total Errors" HeaderText="Total Errors" />
                            </Columns>
                            <EmptyDataTemplate>No Data</EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                    <div class="panel-footer">
                        <div>
                            <h4>Request Summary
                            </h4>
                        </div>
                    </div>
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
