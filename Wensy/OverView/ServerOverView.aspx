<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServerOverView.aspx.cs" Inherits="ServicePoint.OverView.ServerOverView" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frm" runat="server" EnableViewState="false">

        <div>
            <div class="col-md-12 col-lg-12 contents">
                <h3>Servers Overview</h3>
                <div class="table-responsive">
                    <asp:GridView ID="gvList" runat="server" EnableViewState="false" AutoGenerateColumns="false" HeaderStyle-BackColor="#333333" HeaderStyle-ForeColor="White" RowStyle-ForeColor="Black" RowStyle-BackColor="WhiteSmoke" RowStyle-HorizontalAlign="Center" CssClass=" table table-bordered nopadding " HeaderStyle-CssClass="nopadding" OnRowDataBound="gvList_RowDataBound" OnRowCreated="gvList_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="Server" ItemStyle-ForeColor="White">
                                <ItemTemplate>
                                    <%--<a href='#'>
                                            <div style="text-align: center; width: 100px; height: 120px;">
                                                <b style="color: linen;"><%# Eval("DisplayGroup") %></b><br />
                                                <img src='/images/<%# Eval("ServerType").ToString() %>.png' style="width: 50px; height: 50px;"><br />
                                                <span style="color: white;"><%# Eval("IPAddress") %><br />
                                                    <b style="color: white;"><%# Eval("DisplayName") %></b></span>
                                            </div>
                                        </a>--%>

                                    <a href="/dashboard/dashboard.aspx?ServerType=<%# Eval("ServerType").ToString() %>&ServerNum=<%# Eval("ServerNum").ToString() %>" class="text-white">
                                        <ul class="list-unstyled">
                                            <li>
                                                <h5><%# Eval("DisplayGroup") %></h5>
                                            </li>
                                            <li>
                                                <img src="/images/<%# Eval("ServerType").ToString() %>.png" /></li>
                                            <li><%# Eval("IPAddress") %></li>
                                            <li class="bold"><%# Eval("DisplayName") %></li>
                                        </ul>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="%CPU" AccessibleHeaderText="CPU_LINE_CHART">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100</div>
                                                <div class="col-md-6 text-right  nopadding "><%# ServicePoint.Lib.Util.ConvertRound(Eval("CPU-Total"),2) %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_CPU_LINE_CHART"></div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">-<%= ConfigurationManager.AppSettings["ChartDataDuration"].ToString() %> min</div>
                                                <div class="col-md-6 text-right  nopadding ">0</div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QueueLength" AccessibleHeaderText="CPU_QUEUE_COL_CHART">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left  nopadding">max</div>
                                                <div class="col-md-6 text-right  nopadding">32</div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_CPU_QUEUE_COL_CHART" style="width: 100%; height: 100%"></div>
                                        </div>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Committed">
                                <ItemTemplate>
                                    <div class="progress progress-striped active" style="margin-top: 45px; width: 100px;">
                                        <div class="progress-bar progress-bar-danger progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: <%# ServicePoint.Lib.Util.GetRate( (Convert.ToDouble(Eval("CommittedMemory"))/(1024*1024)) , (Convert.ToDouble(Eval("RAMSize"))/(1024*1024) )) %>%">
                                        </div>
                                        <div class="progress-bar progress-bar-warning" style="width: <%# (100-ServicePoint.Lib.Util.GetRate((Convert.ToDouble(Eval("CommittedMemory"))/(1024*1024)) , (Convert.ToDouble(Eval("RAMSize"))/(1024*1024) ))) %>%"></div>
                                    </div>
                                    <div class="text-center">
                                        <%# ServicePoint.Lib.Util.FuncMemoryValue(Convert.ToDouble(Eval("CommittedMemory")),"CommittedMemory")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Available">
                                <ItemTemplate>

                                    <div class="progress progress-striped active" style="margin-top: 45px; width: 100px;">
                                        <div class="progress-bar progress-bar-success progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: <%# ServicePoint.Lib.Util.GetRate(Convert.ToDouble(Eval("AvailableMemory")), Convert.ToDouble(Eval("RAMSize") )/(1024*1024)) %>%">
                                        </div>
                                        <div class="progress-bar progress-bar-warning" style="width: <%# (100-ServicePoint.Lib.Util.GetRate(Convert.ToDouble(Eval("AvailableMemory")), Convert.ToDouble(Eval("RAMSize") )/(1024*1024))) %>%"></div>
                                    </div>
                                    <div class="text-center">
                                        <%# ServicePoint.Lib.Util.FuncMemoryValue(Convert.ToDouble(Eval("AvailableMemory")),"AvailableMemory")%>
                                    </div>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FREE DISK" AccessibleHeaderText="FREEDISK_BAR_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">&nbsp;</div>
                                                <div class="col-md-6 text-right  nopadding "></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_FREEDISK_BAR_CHART"></div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">&nbsp;</div>
                                                <div class="col-md-6 text-right  nopadding "></div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Read Time<br>(ms)" AccessibleHeaderText="READTIME_LINE_CHART">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100</div>
                                                <div class="col-md-6 text-right  nopadding "><%#ServicePoint.Lib.Util.ConvertRound( Eval("LogicalDiskAvgRead"),2 ) %></div>
                                                
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_READTIME_LINE_CHART"></div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">-<%= ConfigurationManager.AppSettings["ChartDataDuration"].ToString() %> min</div>
                                                <div class="col-md-6 text-right  nopadding ">0</div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bytes Total<br>/sec(MB)" AccessibleHeaderText="BYTESTOTALSEC_LINE_CHART">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100MB</div>
                                                <div class="col-md-6 text-right  nopadding "><%#  Math.Round(Convert.ToDouble(Eval("NetworkBytes-Total"))/(1024*1024),2).ToString()  %>MB</div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_BYTESTOTALSEC_LINE_CHART"></div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">-<%# ConfigurationManager.AppSettings["ChartDataDuration"].ToString() %> min</div>
                                                <div class="col-md-6 text-right  nopadding ">0</div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>No Data</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
           <%-- <div class="col-md-5  col-lg-5 row">
                <div class="">
                    <Alert:AlertList ID="AlertList" runat="server" EnableViewState="false" />
                </div>
            </div>--%>
        </div>
        <asp:ScriptManager ID="scrMgr" runat="server" EnableViewState="false"></asp:ScriptManager>
        <asp:Timer ID="tmr" runat="server" EnableViewState="false"></asp:Timer>
    </form>
</asp:Content>
<asp:Content ID="ModalContent" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
    <asp:Literal runat="server" ID="litScript"></asp:Literal>
</asp:Content>
