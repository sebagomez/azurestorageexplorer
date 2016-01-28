<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Entities.ascx.cs" Inherits="StorageManager.Controls.UC_Entities" %>

<table style="width:100%;">
	<tr>
		<td colspan="2">
			<asp:Label ID="lblQuery" runat="server" Font-Names="Calibri" Font-Size="Small" Text="Query:"></asp:Label>
			<a href="javascript:showPopWin('images/help_insert.png', 365, 175, null);" style="font-size: x-small; font-family: Arial, Helvetica, sans-serif;">(Insert help)</a>
			<a href="javascript:showPopWin('images/help_query.png', 365, 175, null);" style="font-size: x-small; font-family: Arial, Helvetica, sans-serif;">(Query help)</a>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<asp:TextBox ID="txtQuery" runat="server" Height="100px" Width="100%" 
				MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td style="width:50%">
			<asp:Button ID="btnInsert" runat="server" Font-Names="Calibri" onclick="btnInsert_Click" Text="Insert Data" />
			<asp:Button ID="btnExecute" runat="server" Font-Names="Calibri" Text="Execute Query" onclick="btnExecute_Click" />
		</td>
		<td style="text-align:left;">
        	<asp:Label ID="lblMessage" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="Black"></asp:Label>
        	<asp:Label ID="lblError" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="Red"></asp:Label>
        </td>
	</tr>
	<tr>
		<td colspan="2">
			<div style="overflow:scroll;width:74vw;height:40vw;">
				<asp:GridView ID="grdEntities" runat="server" CellPadding="4" ForeColor="#333333" 
					GridLines="None" DataKeyNames="PartitionKey,RowKey" 
					onrowcommand="grdEntities_RowCommand" Width="100%">
					<RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" Font-Names="Calibri" />
					<Columns>
						<asp:ButtonField ButtonType="Image" CommandName="DeleteEntity" 
							ImageUrl="~/images/delete.ico" Text="Delete Entity" />
					</Columns>
					<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
					<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
					<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
					<HeaderStyle BackColor="#007FFF" Font-Bold="False" ForeColor="White" Font-Size="Medium" Font-Names="Calibri"  />
					<EditRowStyle BackColor="#2461BF" />
					<AlternatingRowStyle BackColor="White" />
				</asp:GridView>
				<br />
			</div>
		</td>
	</tr>
</table>
