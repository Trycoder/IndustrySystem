<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="ChangeAssetStatus.aspx.vb" Inherits="ChangeAssetStatus" title="Change Asset Status" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript">
</script>
	<asp:UpdatePanel ID="upanel1" runat="server">
	    <ContentTemplate>
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
		<td>
		<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
			<tr  class="trheaderbg">
		<td colspan="2">Change Asset Status</td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" align="right" class="tdtext">Select Category :</td>
	   <td style="width:50%;">
                <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="True" CssClass="control">
                 <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
        </td>
	</tr>
        <tr class="whitebg">
            <td class="tdtext" align="right">
                Select Assets :</td>
            <td>
        <asp:DropDownList ID="drpAssetType"  CssClass="control" runat="server"  AutoPostBack="true">
                 <asp:ListItem Value="">--Select--</asp:ListItem>
        </asp:DropDownList>
            </td>
        </tr>
         <tr  class="whitebg" runat="server" id="assetno" visible="false">
            <td class="tdtext" align="right">
                Asset No :</td>
           <td>
            <table>
            <tr>
            <td class="tdtext">From :</td>
            <td><asp:DropDownList ID="drpassetfrom" runat="server" CssClass="control">
                </asp:DropDownList></td>
             <td class="tdtext">To :</td>
             <td class="tdtext"><asp:DropDownList ID="drpassetto" runat="server" CssClass="control">
                </asp:DropDownList></td>
                <td>
                <asp:Button ID="btnsearch" Text="Search" runat="server" CssClass="lButton"  />
                </td>
            </tr>
            </table>
            </td>
        </tr>
      <tr class="whitebg"  runat="server" id="tddata1" visible ="false">
            <td class="tdtext" align="right" colspan="3">
             <asp:CheckBox ID="chkall" runat="server" Text="Check All" AutoPostBack="true" Checked="false"   />
            </td>
        </tr>
         <tr class="whitebg">
            <td class="tdtext" align="center" colspan="3" runat="server" id="tddata">
            <%--<asp:Panel ID="panel_asset_count" runat="server">
--%>
           <asp:GridView ID="grdassets" runat="server"  DataKeyNames="id" 
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" Width="100%" >
        <Columns>
<%--        <asp:TemplateField>
         <HeaderTemplate>Select</HeaderTemplate>   
 
        <ItemTemplate>
        <asp:CheckBox ID="checboxcolumn" runat="server" />       
        
        </ItemTemplate>
        </asp:TemplateField>
        --%>
        </Columns>




<AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

            </asp:GridView>
           <%-- </asp:Panel>--%>

            </td>
            
        </tr>
	<tr class="whitebg" runat="server" id="trupdate" visible ="false">
		<td class="tdtext" colspan="3" align="center">
            <asp:Button ID="btnupdate" runat="server" CssClass="lButton" Text="Update" />
            &nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" /></td>
	</tr>
		</table>
		</td>
	</tr>

	</table>
	</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>

