<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="LocationsubMaster.aspx.vb" Inherits="LocationsubMaster"  %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add New Location</title>
 <link rel="stylesheet" type="text/css" href="css/ApplyCSS1.css"/>
     <link href="Css/grid.css" rel="Stylesheet" type="text/css" />
 <script type="text/javascript">
 function closewindow()
 {
window.opener.location.href = window.opener.location.href;
if (window.opener.progressWindow)
{
window.opener.progressWindow.close()
}
window.close();
} 
 </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
	<tr class="tdcolbg">
		<td>
		   <table border="0" align="right" cellpadding="4" cellspacing="1" width="100%">
		 <tr class="trheaderbg">
	<td colspan="2" style="font-size:11px;">
			<asp:Label ID="lblerror" runat="server"  Text=" New Location"></asp:Label>
		    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="location" />
		</td>
	</tr>
 <tr class="whitebg">
		<td style="width:50%;" class="tdtext" align="right">Location 1:</td>
		<td style="width:50%;" class="tdtext" >
        <asp:DropDownList ID="drploc1" Width="250" class="control" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="">--Select--</asp:ListItem>
                    <asp:ListItem Value="1">SIP-YG</asp:ListItem>
                    <asp:ListItem Value="2">SIP-CG</asp:ListItem>
                    <asp:ListItem Value="3">SIP-IH</asp:ListItem>
                    <asp:ListItem Value="4">NDPC</asp:ListItem>
                </asp:DropDownList></td>
	</tr>
 <tr class="whitebg">
		<td style="width:40%;" class="tdtext" align="right">Location 2:</td>
		<td style="width:60%;" class="tdtext">
            <asp:TextBox ID="txtloc2" runat="server" Width="200px" 
                ValidationGroup="assetname" CssClass="control" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtloc2" Display="None" ErrorMessage="Enter Location Name" 
                SetFocusOnError="True" ValidationGroup="location"></asp:RequiredFieldValidator>
                                </td>
	</tr>
 <tr class="whitebg">
		<td colspan="2" align="center" class="tdtext">           
		 <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="location" CssClass="lButton" Width="80px" />
         <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="lButton"  Width="80px" OnClientClick="closewindow();" />
		                       </td>
	</tr>
 <tr class="whitebg">
	    <td colspan="2" class="tdtext">
            <asp:GridView ID="grdlocations" runat="server"  DataKeyNames="locid" 
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" >
             <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                  <HeaderTemplate>S.No</HeaderTemplate>
                  <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
               <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Location Name" DataField="locname" />
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("locid") %>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
                          </ItemTemplate>
                             </asp:TemplateField>
                  </Columns>

<AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

            </asp:GridView>	   </td>
	</tr>
		   </table>
		</td>
	</tr>

	</table>
    </div>
    </form>
</body>
</html>

