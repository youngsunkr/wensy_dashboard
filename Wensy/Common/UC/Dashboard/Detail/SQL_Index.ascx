<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SQL_Index.ascx.cs" Inherits="ServicePoint.Common.UC.Dashboard.Detail.SQL_Index" %>
<div class="col-lg-12">
    <div class="row-fluid">
        <h3 class="text-center">SQL Index Flagmentation</h3>
    </div>
    <div class="row-fluid">
        <div class="cold-lg-12">
            <div class="table-responsive">
                <asp:GridView ID="gv_List" runat="server" CssClass="table table-border" HeaderStyle-BackColor="LightGray">
                    <Columns>
                    </Columns>
                    <EmptyDataTemplate>No Data</EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <asp:Literal ID="litScript_Pop" runat="server"></asp:Literal>
    </div>
</div>
