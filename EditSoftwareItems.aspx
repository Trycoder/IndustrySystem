<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditSoftwareItems.aspx.vb" Inherits="EditSoftwareItems" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Software Items</title>
     <link rel="stylesheet" href="css/layout.css" />   
 <link rel="stylesheet" type="text/css" href="css/ApplyCSS1.css"/>
</head>
<body>
    <form id="form1" runat="server">
 <div>
        <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
       <tr class="tdcolbg">
            <td>
            	<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
               <tr class="trheaderbg">
                <td colspan="3">
                    Edit Software Type for :&nbsp;&nbsp;
                    <asp:Label ID="lblassetname" runat="server" Text="Label"></asp:Label>
                    &nbsp;&nbsp; From( <asp:Label ID="lblfrom" runat="server"></asp:Label> ) To ( 
                    <asp:Label ID="lblto" runat="server"></asp:Label> )
                </td>
            </tr>
           <tr class="whitebg">
                <td class="tdtext" runat="server" id="tddata" colspan="3">
                </td>
            </tr>
         <tr class="whitebg">
                <td class="tdtext" colspan="3" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="assetname" CssClass="lButton" Width="80px"/>
                    <asp:Button ID="btncancel" runat="server" Text="Cancel" OnClientClick="window.close();"  CssClass="lButton" Width="80px" />
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
