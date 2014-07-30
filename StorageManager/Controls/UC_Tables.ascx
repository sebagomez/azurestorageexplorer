<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Tables.ascx.cs" Inherits="StorageManager.Controls.UC_Tables" %>
<asp:Label ID="lblTitle" runat="server" Font-Names="Calibri" 
	Font-Size="Small" ForeColor="Black">Table Manager</asp:Label>
        <br />
<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
<asp:Button ID="btnCreate" runat="server" Text="New Table" OnClick="btnCreate_Click" />

<!-- Messages -->
<%-- <asp:ScriptManager ID="ScriptManager1" runat="server"/> %>
<%--<asp:UpdatePanel ID="messagesPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <br />
        <asp:Label ID="lblMessage" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="Black"></asp:Label>
        <asp:Label ID="lblError" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="Red"></asp:Label>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>

<%--<asp:UpdatePanel ID="panelList" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <asp:DataList ID="DataList1" runat="server" DataSourceID="TablesDataSource" OnItemCommand="ItemCommand" Font-Names="Calibri" >
            <ItemTemplate>
                <br />
                <asp:ImageButton ID="imgB" runat="server" CommandName="DeleteTable" CommandArgument='<%# Eval("Name") %>' ImageUrl="~/images/delete.ico" AlternateText='Delete <%# Eval("Name") %>' />
                <asp:LinkButton runat="server" ID="lnk" CommandName="TableClick" CommandArgument='<%# Eval("Name") %>' Text='<%# Eval("Name") %>' />
                <br />
            </ItemTemplate>
        </asp:DataList>

        <asp:ObjectDataSource ID="TablesDataSource" runat="server" 
    SelectMethod="GetAll" TypeName="StorageManager.Helpers.TableHelper">
            <SelectParameters>
                <asp:CookieParameter CookieName="account" DefaultValue="" Name="account" 
                    Type="String" />
                <asp:CookieParameter CookieName="key" Name="key" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>

    <%--</ContentTemplate>
</asp:UpdatePanel>--%>