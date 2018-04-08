<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebOverView.aspx.cs" Inherits="ServicePoint.OverView.WebOverView" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server" ViewStateMode="Disabled">
    <form id="form1" runat="server" enableviewstate="false">
        <div class="col-md-12 col-lg-12 text-left nopadding">
            <h3>Web Servers Overview</h3>
        </div>
        <div class="col-md-12 col-lg-12  nopadding">
            <div class="table-responsive">
                <div class="contents">
                    <asp:GridView ID="gvList" runat="server" EnableViewState="false" AutoGenerateColumns="false" HeaderStyle-BackColor="#333333" HeaderStyle-ForeColor="White" RowStyle-ForeColor="Black" RowStyle-BackColor="WhiteSmoke" RowStyle-HorizontalAlign="Center" CssClass=" table table-bordered col-md-12 nopadding " HeaderStyle-CssClass="nopadding" OnRowDataBound="gvList_RowDataBound" OnRowCreated="gvList_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="Server" ItemStyle-ForeColor="White" ItemStyle-Width="130">
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
                            <asp:TemplateField HeaderText="%CPU" AccessibleHeaderText="CPU_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100</div>
                                                <div class="col-md-6 text-right  nopadding "><%# ServicePoint.Lib.Util.ConvertRound( Eval("CPU-Total"),2 ) %></div>
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
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_CPU_QUEUE_COL_CHART"></div>
                                        </div>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Committed" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="progress progress-striped active" style="margin-top: 45px; width: 100px;">
                                        <div class="progress-bar progress-bar-danger progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: <%# ServicePoint.Lib.Util.GetRate( (Convert.ToDouble(Eval("CommittedMemory"))/(1024*1024)) , (Convert.ToDouble(Eval("RAMSize"))/(1024*1024) )) %>%">
                                        </div>
                                        <div class="progress-bar progress-bar-warning" style="width: <%# (100-ServicePoint.Lib.Util.GetRate( (Convert.ToDouble(Eval("CommittedMemory"))/(1024*1024)) , (Convert.ToDouble(Eval("RAMSize"))/(1024*1024) ))) %>%"></div>
                                    </div>
                                    <div class="text-center">
                                        <%# ServicePoint.Lib.Util.FuncMemoryValue(Convert.ToDouble(Eval("CommittedMemory")),"CommittedMemory")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Available" ItemStyle-Width="100">
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
                            <asp:TemplateField HeaderText="FREE DISK" AccessibleHeaderText="FREEDISK_BAR_CHART" ItemStyle-Width="150">
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
                            <asp:TemplateField HeaderText="Read Time<br>(ms)" AccessibleHeaderText="READTIME_LINE_CHART" ItemStyle-Width="100">
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
                            <asp:TemplateField HeaderText="Bytes Total<br>/sec(MB)" AccessibleHeaderText="BYTESTOTALSEC_LINE_CHART" ItemStyle-Width="100">
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
                            <asp:TemplateField HeaderText="Total Connections" AccessibleHeaderText="TOTALCONNECTION_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">1000</div>
                                                <div class="col-md-6 text-right  nopadding "><%#  Eval("IISCurrentConnection")  %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_TOTALCONNECTION_LINE_CHART"></div>
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
                            <asp:TemplateField HeaderText="Total Bytes/sec(MB)" AccessibleHeaderText="TOTALBYTESSEC_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100MB</div>
                                                <div class="col-md-6 text-right  nopadding "><%# Math.Round(Convert.ToDouble(Eval("NetworkBytes-IISTotal"))/(1024*1024),2).ToString()  %>MB</div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_TOTALBYTESSEC_LINE_CHART"></div>
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
                            <asp:TemplateField HeaderText="%CPU Time (W3WP)" AccessibleHeaderText="CPUTIME_W3WP_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100</div>
                                                <div class="col-md-6 text-right  nopadding "><%#  Math.Round(Convert.ToDouble(Eval("CPU-W3WP")),2).ToString() %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_CPUTIME_W3WP_LINE_CHART"></div>
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
                            <asp:TemplateField HeaderText="W3WP Memory Size(MB)" ItemStyle-Width="100">
                                <ItemTemplate>

                                    <div class="progress progress-striped active" style="margin-top: 45px; width: 100px;">
                                        <div class="progress-bar progress-bar-danger progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: <%# (100 - ServicePoint.Lib.Util.FuncCommittedByAvailable(Eval("w3wpMaxWorkProcessMemory"), Eval("w3wpTotalWorkProcessMemory"))).ToString() %>%">
                                        </div>
                                        <div class="progress-bar progress-bar-warning progress-bar-striped active" style="width: <%# ServicePoint.Lib.Util.FuncCommittedByAvailable(Eval("w3wpMaxWorkProcessMemory"), Eval("w3wpTotalWorkProcessMemory")).ToString() %>%">
                                        </div>
                                    </div>
                                    <div class="text-center">
                                        Total:<%# Math.Round(Convert.ToDouble(Eval("w3wpTotalWorkProcessMemory"))/(1024*1024),2).ToString() %> Max:<%# Math.Round(Convert.ToDouble(Eval("w3wpMaxWorkProcessMemory"))/(1024*1024),2).ToString() %>
                                    </div>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requests Executing" AccessibleHeaderText="REQUESTSEXECUTING_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">50</div>
                                                <div class="col-md-6 text-right  nopadding "><%# ServicePoint.Lib.Util.ConvertToFileSizeWitFormatWithNullCheck( Eval("ASPRequestsExecuting") ) %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_REQUESTSEXECUTING_LINE_CHART"></div>
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
                            <asp:TemplateField HeaderText="Requests Queued" AccessibleHeaderText="REQUESTSQUEUED_COL_CHART">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left  nopadding">&nbsp;</div>
                                                <div class="col-md-6 text-right  nopadding"></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_REQUESTSQUEUED_COL_CHART"></div>
                                        </div>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requests /sec" AccessibleHeaderText="REQUESTSSEC_COL_CHART">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left  nopadding">&nbsp;</div>
                                                <div class="col-md-6 text-right  nopadding"></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_REQUESTSSEC_COL_CHART"></div>
                                        </div>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Request Execution Time(ms)" AccessibleHeaderText="REQUESTEXECUTIONTIME_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">10 Sec</div>
                                                <div class="col-md-6 text-right  nopadding "><%# ServicePoint.Lib.Util.ConvertToFileSizeWitFormatWithNullCheck( Eval("ASPRequestExecutionTime") ) %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_REQUESTEXECUTIONTIME_LINE_CHART"></div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">-<%# ConfigurationManager.AppSettings["AlertDataDuration"].ToString() %> min</div>
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
        </div>
        <div class="col-md-12">&nbsp;</div>
        <%--<div class="col-md-6 ">
            <Alert:AlertSlide ID="AlertSlide" runat="server"  EnableViewState="false"/>
        </div>--%>
        <div class="col-md-6  nopadding">
           <%-- <Alert:AlertList ID="AlertList" runat="server"  EnableViewState="false" />--%>
        </div>
        <asp:ScriptManager ID="scrMgr" runat="server"  EnableViewState="false"></asp:ScriptManager>
        <asp:Timer ID="tmr" runat="server"  EnableViewState="false"></asp:Timer>
    </form>
</asp:Content>
<asp:Content ID="ModalContent" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
    <asp:Literal runat="server" ID="litScript"></asp:Literal>
</asp:Content>
