<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BulkTransactions.aspx.vb" Inherits="BulkTransactions" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bulk Transactions</title>
 <link rel="stylesheet" href="css/layout.css" />   
 <link rel="stylesheet" type="text/css" href="css/ApplyCSS.css"/>
 <link href="Css/ApplyCSS1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
	<tr class="tdcolbg">
		<td>
		   <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
		 <tr  class="trheaderbg">
                <td colspan="2" align="left">
                Select Assets 
                    </td>
                
	    </tr>

 <tr class="whitebg">
		<td colspan="2" style="width:60%;" class="tdtext">
           <asp:CheckBoxList ID="chkassets" runat="server" RepeatColumns="6" RepeatDirection="Horizontal"></asp:CheckBoxList>
       </td>
	</tr>
 <tr class="whitebg">
		<td colspan="2" align="center">           
		 <asp:Button ID="btnSave" runat="server" Text="Update" ValidationGroup="assetname" 
                CssClass="lButton" Width="80px" />
         <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="lButton"  
                Width="80px" onclientclick="window.close();" />
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
