<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="EditSoftwares.aspx.vb" Inherits="EditSoftwares" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="upanel1" runat="server">
	    <ContentTemplate>
	    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%" >
	<tr class="tdcolbg">
		<td>
		<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
			<tr class="trheaderbg">
		<td colspan="2">Edit Software(s)
			    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="assetname" />
		</td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" align="right" class="tdtext">Select Category :</td>
	<td style="width:50%;" class="tdtext">
                <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="True" CssClass="control">
                 <asp:ListItem>--Select--</asp:ListItem>
                </asp:DropDownList>
        </td>
	</tr>
        <tr class="whitebg">
            <td  align="right" class="tdtext">
                Select Assets :</td>
            <td>
        <asp:DropDownList ID="drpAssetType"  CssClass="control" runat="server"  AutoPostBack="true">
                 <asp:ListItem>--Select--</asp:ListItem>
        </asp:DropDownList>
            </td>
        </tr>
<%--         <tr>
            <td  align="right">
            Asset Count
               </td>
            <td align="center" style="font-weight:bold;">
                :</td>
            <td>
             <asp:TextBox ID="txtassetcount" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
         <tr runat="server" id="assetno" visible="false" class="whitebg">
            <td  align="right" class="tdtext">
                Asset No :</td>
            <td >
            <table>
            <tr>
            <td class="tdtext">From :</td>
            <td><asp:DropDownList ID="drpassetfrom" runat="server" CssClass="control">
                </asp:DropDownList></td>
             <td class="tdtext">To :</td>
             <td><asp:DropDownList ID="drpassetto" runat="server" CssClass="control">
                </asp:DropDownList></td>
                <td>
                <asp:Button ID="btnsearch" Text="Search" runat="server" CssClass="lButton" Width ="80px"  />
                </td>
                <td>
		    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
            </tr>
            </table>
            </td>
        </tr>
         <tr class="whitebg">
            <td  align="center" colspan="2" runat="server" id="tddata">
           </td>
        </tr>
	</table>
		</td>
	</tr>


	</table>
	    </ContentTemplate>
</asp:UpdatePanel>
	    
</asp:Content>

