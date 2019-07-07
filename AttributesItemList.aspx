<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AttributesItemList.aspx.vb" Inherits="AttributesItemList" EnableEventValidation="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Item List</title>
 <link rel="stylesheet" href="css/grid.css" />   
  <link rel="stylesheet" href="Css/ApplyCSS1.css" />   
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
	<tr class="tdcolbg">
		<td>
		   <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
		 <tr class="trheaderbg">
	<td colspan="2">
				 Attribute Items for :&nbsp;&nbsp;
            <asp:Label ID="lblassetname" runat="server" Text="Label"></asp:Label>
		    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="assetname" />
		</td>
	</tr>
 <tr class="whitebg">
		<td style="width:40%;" class="tdtext">New Item</td>
		<td style="width:60%;" class="tdtext">
            <asp:TextBox ID="txtitemname" runat="server" Width="200px" 
                ValidationGroup="assetname" CssClass="control" ></asp:TextBox>
       </td>
	</tr>
 <tr class="whitebg">
		<td colspan="2" align="center" class="tdtext">           
		 <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="assetname" CssClass="lButton" Width="80px" />
         <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="lButton"  Width="80px" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtitemname" Display="None" ErrorMessage="Enter Attribute Description" 
                SetFocusOnError="True" ValidationGroup="assetname"></asp:RequiredFieldValidator>
                              </td>
	</tr>
 <tr class="whitebg">
	    <td colspan="2" class="tdtext">
            <asp:GridView ID="grdattributes" runat="server"  DataKeyNames="id" 
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" >
             <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>Sno</HeaderTemplate>
                  <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Attribute Name" DataField="attdesc" />
                  <asp:CommandField ShowDeleteButton="true" HeaderText="Delete" ButtonType="Link" ItemStyle-HorizontalAlign="Center"  />
                     </Columns>

<AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

            </asp:GridView>	   
	    </td>
	</tr>
		   </table>
		</td>
	</tr>

	</table>
    </div>
    </form>
</body>
</html>
