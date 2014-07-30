<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Queues.ascx.cs" Inherits="StorageManager.Controls.UC_Queues" %>

<asp:Label ID="lblTitle" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="Black">Queues Manager</asp:Label>
<br />

<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
<asp:Button ID="btnCreate" runat="server" Text="New Queue" OnClick="btnCreate_Click" />

  <br />
        <asp:Label ID="lblMessage" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="Black"></asp:Label>
        <asp:Label ID="lblError" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="Red"></asp:Label>

                <asp:DataList ID="DataList1" runat="server" DataSourceID="QueuesDataSource" OnItemCommand="ItemCommand" Font-Names="Calibri" >
            <ItemTemplate>
                <br />
                <asp:ImageButton ID="imgB" runat="server" CommandName="DeleteQueue" CommandArgument='<%# Eval("Name") %>' ImageUrl="~/images/delete.ico" AlternateText='Delete <%# Eval("Name") %>' />
                <asp:LinkButton runat="server" ID="lnk" CommandName="QueueClick" CommandArgument='<%# Eval("Name") %>' Text='<%# Eval("Name") %>' />
                <br />
            </ItemTemplate>
        </asp:DataList>

        <asp:ObjectDataSource ID="QueuesDataSource" runat="server" 
    SelectMethod="GetAll" TypeName="StorageManager.Helpers.QueueHelper">
            <SelectParameters>
                <asp:CookieParameter CookieName="account" Name="account" Type="String" />
                <asp:CookieParameter CookieName="key" Name="key" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>