<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="StorageManager.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Names="Calibri" Font-Size="Medium" 
    Text="Oooops! something has gone terribly wrong! :("></asp:Label>
    <br />
<br />
<asp:Label ID="lblError" runat="server" Font-Names="Calibri" Font-Size="Medium" 
    ForeColor="Red"></asp:Label>
</asp:Content>
