<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="StorageManager.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	<link rel="stylesheet" type="text/css" href="Resources/subModal.css" />
	<script type="text/javascript" src="Resources/common.js"></script>
	<script type="text/javascript" src="Resources/subModal.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table style="width: 100%;">
		<tr>
			<td>
				<asp:Label ID="Label1" runat="server" Text="Account" Font-Names="Calibri"></asp:Label>
				<a href="javascript:showPopWin('images/new-account.png', 550, 350, null);" style="font-size: x-small; font-family: Arial, Helvetica, sans-serif;">(What's this?)</a>
			</td>
			<td>
				<asp:TextBox ID="txtAccount" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
					ControlToValidate="txtAccount" ErrorMessage="Account is required"
					Font-Names="Calibri"></asp:RequiredFieldValidator>
			</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="Label2" runat="server" Text="Key" Font-Names="Calibri"></asp:Label>
				<a href="javascript:showPopWin('images/new-key.png', 550, 350, null);" style="font-size: x-small; font-family: Arial, Helvetica, sans-serif;">(What's this?)</a>
			</td>
			<td>
				<asp:TextBox ID="txtKey" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
					ControlToValidate="txtKey" ErrorMessage="Secret key is required"
					Font-Names="Calibri"></asp:RequiredFieldValidator>
			</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td>
				<a href="http://www.microsoft.com/windowsazure/getstarted/" style="font-size: x-small; font-family: Arial, Helvetica, sans-serif;" target="_blank">(Need an account?)</a>
			</td>
			<td align="left">
				<asp:Button ID="btnOK" runat="server" Text="Enter" OnClick="btnOK_Click"
					Font-Names="Calibri" />
			</td>
			<td>
				<asp:Label ID="lblError" runat="server" Font-Names="Calibri" Font-Size="Small"
					ForeColor="Red"></asp:Label>
			</td>
		</tr>
	</table>
</asp:Content>
