<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="EditAssetAttributes.aspx.vb" Inherits="EditAssetAttributes" title="Edit Asset Attributes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%" >
	<tr class="tdcolbg">
		<td>
		<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
			<tr class="trheaderbg">
		<td colspan="2">Edit Assets 
			    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="assetname" />
                  <asp:Label ID="lblmessage" runat="server"></asp:Label>
		</td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" align="right" class="tdtext">Select Category :</td>
	<td style="width:50%;" class="tdtext">
                <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="True" CssClass="control">
                 <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
        </td>
	</tr>
        <tr class="whitebg"><td  align="right" class="tdtext">
                Select Assets :</td>
            <td>
        <asp:DropDownList ID="drpAssetType"  CssClass="control" runat="server"  AutoPostBack="true">
                 <asp:ListItem Value="">--Select--</asp:ListItem>
        </asp:DropDownList>
            </td>
        </tr>
         <tr runat="server" class="whitebg">
            <td  align="right" class="tdtext">
                Asset No :</td>
            <td >
                <asp:DropDownList ID="drpassetno" runat="server" AutoPostBack="true" 
                    CssClass="control">
                    <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
         <tr class="whitebg">
            <td  align="center" colspan="2" runat="server" id="tddata" class="tdtext">
           </td>
        </tr>
            <tr class="whitebg">
   <td colspan="2">
     <table style="width: 100%">
        <tr>
            <td  class="tdtext" align="left" style="padding-left:450px;">
                <asp:Button ID="btnsave" runat="server" Text="Update" ValidationGroup="assets" 
                    Visible ="false" CssClass="lButton" Width ="80px" />
&nbsp;<asp:Button ID="btncancel" runat="server" Text="Cancel" Visible ="false" CssClass="lButton" Width ="80px"  />
            </td>
        </tr>
        </table>
   </td>
   </tr>
	</table>
		</td>
	</tr>


	</table>
	    
    
</asp:Content>
