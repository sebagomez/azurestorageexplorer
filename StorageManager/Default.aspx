<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StorageManager._Default" %>
<%@ Register src="Controls/UC_Containers.ascx" tagname="UC_Containers" tagprefix="uc1" %>
<%@ Register src="Controls/UC_Blob.ascx" tagname="UC_Blob" tagprefix="uc2" %>
<%@ Register src="Controls/UC_Tables.ascx" tagname="UC_Tables" tagprefix="uc3" %>
<%@ Register src="Controls/UC_Entities.ascx" tagname="UC_Entities" tagprefix="uc4" %>
<%@ Register src="Controls/UC_Queues.ascx" tagname="UC_Queues" tagprefix="uc5" %>
<%@ Register src="Controls/UC_Messages.ascx" tagname="UC_Messages" tagprefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link rel="stylesheet" type="text/css" href="Resources/subModal.css" />    
<script type="text/javascript" src="Resources/common.js"></script>    
<script type="text/javascript" src="Resources/subModal.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <table style="width: 100%; height:100%">
            <tr>
                <td style="width:35%" valign="top">
                    <uc1:UC_Containers ID="UC_Containers1" runat="server" />
                    <uc3:UC_Tables ID="UC_Tables1" runat="server" />
                    <uc5:UC_Queues ID="UC_Queues1" runat="server" />
                </td>
                <td valign="top">
                    <uc2:UC_Blob ID="UC_Blob1" runat="server" />
                    <uc4:UC_Entities ID="UC_Entities1" runat="server" />
                    <uc6:UC_Messages ID="UC_Messages1" runat="server" />
                </td>
            </tr>
            </table>
</asp:Content>