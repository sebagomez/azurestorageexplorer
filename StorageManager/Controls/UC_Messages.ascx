<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Messages.ascx.cs" Inherits="StorageManager.Controls.UC_Messages" %>
<table style="width:100%;">
	<tr>
		<td>
<asp:TextBox ID="txtMessage" runat="server" Width="290px"></asp:TextBox>
<asp:Button ID="btnMessage" runat="server" Text="Add Message" 
	onclick="btnMessage_Click" />
		</td>
	</tr>
	<tr>
		<td>
<asp:Label ID="lblMessage" runat="server" Font-Names="Calibri" 
	Font-Size="Small"></asp:Label>
<asp:Label ID="lblError" runat="server" Font-Names="Calibri" Font-Size="Small" 
	ForeColor="Red"></asp:Label>
	
		</td>
	</tr>
	<tr>
		<td>
	
<asp:GridView ID="grdMessages" runat="server" AutoGenerateColumns="False" 
	DataSourceID="MessagesDataSource" onrowcommand="grdMessages_RowCommand" 
	DataKeyNames="ID" Width="100%" CellPadding="4" GridLines="None">
	<Columns>
		<asp:ButtonField ButtonType="Image" ImageUrl="~/images/delete.ico" 
			Text="Delete" CommandName="DeleteMessage"  />
		<asp:BoundField DataField="Id" HeaderText="ID" Visible="False" />
		<asp:BoundField DataField="Content" HeaderText="Content" 
			SortExpression="Content" />
	</Columns>
	<RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" Font-Names="Calibri" />
	<HeaderStyle BackColor="#007FFF" Font-Bold="False" ForeColor="White" Font-Size="Medium" Font-Names="Calibri"  />
	<AlternatingRowStyle BackColor="White" />
</asp:GridView>

		</td>
	</tr>
</table>

<asp:ObjectDataSource ID="MessagesDataSource" runat="server" 
	SelectMethod="GetAll" TypeName="StorageManager.Helpers.MessageHelper">
	<SelectParameters>
		<asp:QueryStringParameter Name="queueName" QueryStringField="queue" 
			Type="String" />
		<asp:CookieParameter CookieName="account" Name="account" Type="String" />
		<asp:CookieParameter CookieName="key" Name="key" Type="String" />
	</SelectParameters>
</asp:ObjectDataSource>
