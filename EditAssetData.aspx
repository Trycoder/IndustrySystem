<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="EditAssetData.aspx.vb" Inherits="EditAssetData" title="Edit Asset Data" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript">
</script>
	<asp:UpdatePanel ID="upanel2" runat="server">
	    <ContentTemplate>
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
		<td>
		<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
			<tr  class="trheaderbg">
		<td colspan="4">Edit Assets</td>
	</tr>
    <tr class="tdtext" runat="server" id="trmessage" visible ="false">
		<td colspan="4"><asp:Label ID="lblmessage" runat="server"></asp:Label></td>
	</tr>
	<tr class="whitebg">
		<td style="width:25%;" align="right" class="tdtext">Select Category :</td>
	   <td style="width:15%;">
                <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="True" CssClass="control">
                 <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
        </td>
       <td style="width:15%;" class="tdtext" align="right">
                Select Asset Type :</td>
            <td style="width:45%;">
        <asp:DropDownList ID="drpAssetType"  CssClass="control" runat="server"  AutoPostBack="true">
                 <asp:ListItem Value="">--Select--</asp:ListItem>
        </asp:DropDownList>
            </td>
	</tr>
         <tr  class="whitebg" runat="server" id="assetno">
            <td class="tdtext" align="right">
                Select Asset :</td>
           <td>
               <asp:DropDownList ID="drpassets" runat="server" 
                   CssClass="control" AutoPostBack="true">
                   <asp:ListItem Value="">--Select--</asp:ListItem>
               </asp:DropDownList>
            </td>
                     <td class="tdtext" align="right">
                Select Attributes :</td>
           <td>
               <asp:DropDownList ID="drpattributes" runat="server" AutoPostBack="true" 
                   CssClass="control">
                   <asp:ListItem Value="">--Select--</asp:ListItem>
               </asp:DropDownList>
            </td>
        </tr>
        <tr id="trassetvalue" class="whitebg"  runat="server"  visible ="false">
            <td class="tdtext" align="left" colspan="4" style="padding-left:200px;">
            <table>
            <tr>
           <td class="tdtext" align="right">Existing Value :</td>
            <td class="tdtext" align="left">
                <asp:TextBox ID="txtexistvalue" runat="server" class="tdtext"></asp:TextBox>
            </td>
            <td class="tdtext" align="right">New Value :</td>
            <td class="tdtext" align="left">
                <asp:TextBox ID="txtnewvalue" runat="server" class="tdtext"></asp:TextBox>
            </td>
           <td class="tdtext" align="left">
                 <asp:Button ID="btnupdate" runat="server" CssClass="lButton" Text="Update" Width="80px" />
            </td>
                    </tr>
                </table>
            </td>
        </tr>
         <tr  class="whitebg">
            <td class="tdtext" align="right">
               Remarks :</td>
           <td colspan="3">
              <asp:TextBox ID="txtremarks" runat="server" Width="450px" Height="70px" TextMode="MultiLine" class="tdtext"> </asp:TextBox>
            </td>
        </tr>
	<tr class="whitebg" runat="server" id="trupdate" visible ="false">
		<td class="tdtext" colspan="4" align="center">
            <asp:Button ID="btnsubmit" runat="server" CssClass="lButton" Text="Save" Width="80px" />
            &nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width="80px" /></td>
	</tr>
		</table>
		</td>
	</tr>

	</table>
	</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>


