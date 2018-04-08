<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="AlertDetail.aspx.cs" Inherits="ServicePoint.Common.UC.AlertDetail" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frm" runat="server">
        <asp:Panel ID="pnl_SQL" runat="server" Visible="false">
            <div class="col-lg-12">

                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_SQL_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <asp:Label ID="lbl_Sql" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-lg-12">
                <h3>Queries running, when the alert triggered</h3>
            </div>
            <div class="col-lg-12">
                <div class="table-responsive">
                    <asp:Repeater ID="rpt_Query" runat="server" EnableViewState="false">
                        <HeaderTemplate>
                            <table class="table table-bordered" cellspacing="0" rules="all" border="1" id="MainContent_gv_Sql_Class" style="border-collapse: collapse;">
                                <tr style="background-color: LightGrey;">
                                    <th scope="col">Time</th>
                                    <th scope="col">DBName</th>
                                    <th scope="col">CPU(ms)</th>
                                    <th scope="col">Elapsed(ms)</th>
                                    <th scope="col">Logical Read</th>
                                    <th scope="col">Reads</th>
                                    <th scope="col">Writes</th>
                                    <th scope="col">Blocking SSID</th>
                                    <th scope="col">Wait Type</th>
                                    <th scope="col">Wait Time</th>
                                    <th scope="col">Wait resource</th>
                                    <th scope="col">#Alloc pages</th>
                                    <th scope="col">Elapsed#Dealloc pages</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("TimeIn").ToString() %></td>
                                <td><%# Eval("db_name").ToString() %></td>
                                <td><%# Eval("cpu_time").ToString() %></td>
                                <td><%# Eval("total_elapsed_time").ToString() %></td>
                                <td><%# Eval("logical_reads").ToString() %></td>
                                <td><%# Eval("reads").ToString() %></td>
                                <td><%# Eval("writes").ToString() %></td>
                                <td><%# Eval("blocking_session_id").ToString() %></td>
                                <td><%# Eval("wait_type").ToString() %></td>
                                <td><%# Eval("wait_time").ToString() %></td>
                                <td><%# Eval("wait_resource").ToString() %></td>
                                <td><%# Eval("user_objects_alloc_page_count").ToString() %></td>
                                <td><%# Eval("user_objects_dealloc_page_count").ToString() %></td>
                            </tr>
                            <tr>
                                <td>Query Text</td>
                                <td colspan="12"><textarea rows="5" class="input-sm span12"><%# Eval("full_query_text").ToString() %></textarea> </td>

                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <%--<asp:GridView ID="gv_Sql_Class" runat="server" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="URI" DataField="URI" />
                            <asp:BoundField HeaderText="ClientLocation" DataField="ClientLocation" />
                            <asp:BoundField HeaderText="RunningTime" DataField="RunningTime" />
                            <asp:BoundField HeaderText="TimeIn" DataField="TimeIn" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div>
                                <table class="table">
                                    <tr style="background-color: LightGrey;">
                                        <th scope="col">URI</th>
                                        <th scope="col">ClientLocation</th>
                                        <th scope="col">RunningTime</th>
                                        <th scope="col">TimeIn</th>
                                    </tr>
                                    <tr>
                                        <td colspan="4">No Data</td>
                                    </tr>
                                </table>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>--%>
                </div>
            </div>
            <div class="col-lg-12">
                <h4>Related Performance Values -<asp:Label ID="lbl_Sql_Sub" runat="server"></asp:Label>
                </h4>
            </div>
            <div class="col-lg-12">

                <div class="table-responsive">
                    <asp:GridView ID="gv_List_Sql" runat="server" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray">
                        <Columns></Columns>
                        <EmptyDataTemplate>No Data</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnl_Window" runat="server" Visible="false">
            <div class="col-lg-6">

                <div class="panel panel-default nodpadding">
                    <div class="panel-body">
                        <div id="grp_<%= ServerNum.ToString()%>_WINDOW_LINE_CHART"></div>
                    </div>
                    <div class="panel-footer">
                        <asp:Label ID="lbl_Window" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="table-responsive">
                    <asp:GridView ID="gv_Window_Class" runat="server" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="URI" DataField="URI" />
                            <asp:BoundField HeaderText="ClientLocation" DataField="ClientLocation" />
                            <asp:BoundField HeaderText="RunningTime" DataField="RunningTime" />
                            <asp:BoundField HeaderText="TimeIn" DataField="TimeIn" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div>
                                <table class="table">
                                    <tr style="background-color: LightGrey;">
                                        <th scope="col">URI</th>
                                        <th scope="col">ClientLocation</th>
                                        <th scope="col">RunningTime</th>
                                        <th scope="col">TimeIn</th>
                                    </tr>
                                    <tr>
                                        <td colspan="4">No Data</td>
                                    </tr>
                                </table>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <div class="col-lg-12">
                <h4>Related Performance Values -<asp:Label ID="lbl_Window_Sub" runat="server"></asp:Label>
                </h4>
            </div>
            <div class="col-lg-12">

                <div class="table-responsive">
                    <asp:GridView ID="gv_List_Window" runat="server" CssClass="table table-bordered" HeaderStyle-BackColor="LightGray">
                        <Columns></Columns>
                        <EmptyDataTemplate>No Data</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>

    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <asp:Literal ID="litScript_Pop" runat="server"></asp:Literal>
</asp:Content>
