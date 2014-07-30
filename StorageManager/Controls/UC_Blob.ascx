<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Blob.ascx.cs" Inherits="StorageManager.Controls.UC_Blob" %>

<table style="width:100%;">
	<tr>
		<td>
<asp:FileUpload ID="FileUpload" runat="server" />
<asp:Button ID="btnUpload" runat="server" onclick="btnUpload_Click" 
    Text="Upload" />
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
<asp:GridView ID="grdBlobs" runat="server" AutoGenerateColumns="False" 
    DataSourceID="BlobDataSource" CellPadding="4" ForeColor="#333333" 
    GridLines="None" DataKeyNames="Url" onrowcommand="GridView1_RowCommand" 
	Width="100%" onrowdatabound="grdBlobs_RowDataBound">
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" />
    <Columns>
        <asp:ButtonField ButtonType="Image" CommandName="DeleteBlob" 
            ImageUrl="~/images/delete.ico" Text="Delete File" />
        <asp:ButtonField ButtonType="Image" CommandName="DownloadBlob" 
            ImageUrl="~/images/download.ico" Text="Download File" />
        <asp:BoundField DataField="Url" HeaderText="Blob" SortExpression="Url" >
        <HeaderStyle Width="0px" Font-Names="Calibri" />
        <ItemStyle Width="0px" Font-Names="Calibri" />
        </asp:BoundField>
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" 
            Visible="False" >
        <HeaderStyle Font-Names="Calibri" />
        <ItemStyle Font-Names="Calibri" />
        </asp:BoundField>
        <asp:BoundField DataField="Size" HeaderText="Size (bytes)" 
            SortExpression="Size" >
        <HeaderStyle Font-Names="Calibri" />
        <ItemStyle Font-Names="Calibri" />
        </asp:BoundField>
        <asp:BoundField DataField="Type" HeaderText="Type" 
            SortExpression="Type" Visible="False" >
            <HeaderStyle Font-Names="Calibri" />
        <ItemStyle Font-Names="Calibri" />
        </asp:BoundField>
        <asp:BoundField DataField="BlobType" HeaderText="BlobType" 
            SortExpression="BlobType" Visible="False" >
            <HeaderStyle Font-Names="Calibri" />
        <ItemStyle Font-Names="Calibri" />
        </asp:BoundField>
        <asp:BoundField DataField="LastModfied" HeaderText="LastModfied" 
            SortExpression="LastModfied" >
            <HeaderStyle Font-Names="Calibri" />
        <ItemStyle Font-Names="Calibri" />
        </asp:BoundField>
        <asp:BoundField DataField="ETag" HeaderText="ETag" SortExpression="ETag" >
                    <HeaderStyle Font-Names="Calibri" />
        <ItemStyle Font-Names="Calibri" />
        </asp:BoundField>
    </Columns>
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#007FFF" Font-Bold="False" ForeColor="White" 
        Font-Size="Medium" />
    <EditRowStyle BackColor="#999999" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>

		</td>
	</tr>
</table>

<asp:ObjectDataSource ID="BlobDataSource" runat="server" SelectMethod="GetAll" 
    TypeName="StorageManager.Helpers.BlobHelper" DeleteMethod="Delete">
    <DeleteParameters>
        <asp:CookieParameter CookieName="account" Name="account" Type="String" />
        <asp:CookieParameter CookieName="key" Name="key" Type="String" />
        <asp:ControlParameter ControlID="GridView1" Name="url" 
            PropertyName="SelectedValue" Type="String" />
    </DeleteParameters>
    <SelectParameters>
        <asp:QueryStringParameter Name="container" QueryStringField="container" 
            Type="String" />
        <asp:CookieParameter CookieName="account" Name="account" Type="String" />
        <asp:CookieParameter CookieName="key" DefaultValue="" Name="key" 
            Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>


