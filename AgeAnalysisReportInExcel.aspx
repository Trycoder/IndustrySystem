<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AgeAnalysisReportInExcel.aspx.vb" Inherits="AgeAnalysisReportInExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%" >
 <tr class="tdcolbg">
<td>
<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
    <tr  class="trheaderbg">
    <td >
        <asp:Label ID="lblmessage" runat="server"></asp:Label>
</td>
</tr>
    <tr class="whitebg">
<td class="blue_text">
  <asp:LinkButton ID="lnkback" runat="server" Text="Back"></asp:LinkButton>
</td>
</tr>
    <tr class="whitebg" runat="server" id="trreports" visible="false">
<td class="blue_text">
</td>
</tr>
	<tr class="whitebg">
      <!-- Age Analysis Report and Employee More Then Asset In Excel !-->
                    <td  align="center" style="width:100%" runat="server" id="tddata">
            	        <asp:GridView ID="grditems" runat="server"  
                            GridLines="Both"  
                            AlternatingRowStyle-CssClass="alt"
                            AllowPaging="false"
                            AutoGenerateColumns="false" Width="100%" >
                            <Columns>
                                        <asp:TemplateField>
                                        <HeaderTemplate>S.No</HeaderTemplate>
                                        <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                                    </asp:TemplateField> 
                           </Columns>
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
