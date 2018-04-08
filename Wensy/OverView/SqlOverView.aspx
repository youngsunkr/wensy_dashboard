<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SqlOverView.aspx.cs" Inherits="ServicePoint.OverView.SqlOverView" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server" enableviewstate="false">
        <div class="col-md-12 col-lg-12 text-left nopadding">
            <h3>SQL Servers Overview</h3>
        </div>
        <div class="col-md-12 col-lg-12  nopadding">
            <div class="table-responsive">
                <div class="contents">
                    <asp:GridView ID="gvList" runat="server" EnableViewState="false" AutoGenerateColumns="false" HeaderStyle-BackColor="#333333" HeaderStyle-ForeColor="White" RowStyle-ForeColor="Black" RowStyle-BackColor="WhiteSmoke" RowStyle-HorizontalAlign="Center" CssClass=" table table-bordered col-md-12 nopadding " HeaderStyle-CssClass="nopadding" OnRowDataBound="gvList_RowDataBound" OnRowCreated="gvList_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="Server" ItemStyle-Width="130" ItemStyle-ForeColor="White">
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
                            <asp:TemplateField HeaderText="QueueLength" AccessibleHeaderText="CPU_QUEUE_COL_CHART" ItemStyle-Width="50">
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
                                                <div class="col-md-6 text-left nopadding ">-<%= ConfigurationManager.AppSettings["ChartDataDuration"].ToString() %>min</div>
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
                            <asp:TemplateField HeaderText="%CPU(SQL)" AccessibleHeaderText="SQL_CPU_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100</div>
                                                <div class="col-md-6 text-right  nopadding "><%#ServicePoint.Lib.Util.ConvertRound( Eval("CPU-SQL"),2 ) %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_SQL_CPU_LINE_CHART"></div>
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
                            <asp:TemplateField HeaderText="Allocated/Used (GB)" AccessibleHeaderText="SQL_ALLOCATE_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding "><%#ServicePoint.Lib.Util.ConvertSize( Eval("RAMSize"),(1024*1024*1024),2 ) %>GB</div>
                                                <div class="col-md-6 text-right  nopadding "><%#  Math.Round(Convert.ToDouble(Eval("SQLTargetServerMemory"))/(1024*1024),2).ToString()  %>GB</div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_SQL_ALLOCATE_LINE_CHART"></div>
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
                            <asp:TemplateField HeaderText="PhysicalIdleTime(%)" AccessibleHeaderText="SQL_IDLETIME_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100</div>
                                                <div class="col-md-6 text-right  nopadding "><%# Math.Round(Convert.ToDouble(Eval("PhysicalDiskIdleTime")),2).ToString() %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_SQL_IDLETIME_LINE_CHART"></div>
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
                            <asp:TemplateField HeaderText="Buffer cache hit (%)" AccessibleHeaderText="SQL_BUFFER_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">100</div>
                                                <div class="col-md-6 text-right  nopadding "><%# Math.Round(Convert.ToDouble(Eval("SQLPlanCacheHit")),2).ToString() %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_SQL_BUFFER_LINE_CHART"></div>
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
                            <asp:TemplateField HeaderText="SQL Statistics">
                                <ItemTemplate>
                                    <div class="row-fluid">
                                        <div class="col-lg-12">&nbsp;</div>
                                        <div class="col-md-12">
                                            <div class="col-md-8 text-left">Batch Requests:</div>
                                            <div class="col-md-4  text-left"><%#  Math.Round(Convert.ToDouble(Eval("SQLBatchRequests")),2).ToString()  %></div>
                                        </div>
                                        <div class="col-lg-12">&nbsp;</div>
                                        <div class="col-md-12">
                                            <div class="col-md-8 text-left">Compilations:</div>
                                            <div class="col-md-4 text-left"><%# Math.Round(Convert.ToDouble(Eval("SQLCompilations")),2).ToString()  %></div>
                                        </div>
                                        <div class="col-lg-12">&nbsp;</div>
                                        <div class="col-md-12">
                                            <div class="col-md-8 text-left">Recompilations:</div>
                                            <div class="col-md-4 text-left"><%# Math.Round(Convert.ToDouble(Eval("SQLReCompilations")),2).ToString()  %></div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Page life expectancy" AccessibleHeaderText="SQL_PAGELIFE_LINE_CHART" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <div class="text-small nopadding container-fluid">
                                        <div class="row-fluid">
                                            <div class="col-md-12 nopadding ">
                                                <div class="col-md-6 text-left nopadding ">&nbsp;</div>
                                                <div class="col-md-6 text-right  nopadding "><%# Eval("SQLPageLifeExpectancy").ToString()  %></div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div id="grp_<%# Eval("ServerNum").ToString() %>_SQL_PAGELIFE_LINE_CHART"></div>
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
      <%--  <div class="col-md-6 ">
            <Alert:AlertSlide ID="AlertSlide" runat="server" EnableViewState="false" />
        </div>--%>
        <div class="col-md-6  nopadding">
           <%-- <Alert:AlertList ID="AlertList" runat="server" EnableViewState="false" />--%>
        </div>

        <asp:ScriptManager ID="scrMgr" runat="server" EnableViewState="false"></asp:ScriptManager>
        <asp:Timer ID="tmr" runat="server" EnableViewState="false"></asp:Timer>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <asp:Literal runat="server" ID="litScript"></asp:Literal>
</asp:Content>
