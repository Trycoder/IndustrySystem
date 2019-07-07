<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewConsumables.aspx.vb" Inherits="ViewConsumables" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Consumables</title>
     <link rel="stylesheet" href="css/grid.css" />   
  <link rel="stylesheet" href="Css/ApplyCSS1.css" /> 
    </head>
<body>
  <form id="form1" runat="server"> 
  <table width="100%">
<tr>
<td align="center">
<asp:GridView ID="grdconsumables" runat="server"
               CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" Width="100%"  EmptyDataText="No Consumables Found" AllowSorting="true">
            <HeaderStyle Font-Size="Medium" Font-Names="Trebuchet MS,Geneva, Arial, helvetica, sans-serif" />  
    <Columns>  
          <asp:BoundField DataField="Location" HeaderText="Location Name" SortExpression="Location" />  
        <asp:BoundField DataField="SubLocation" HeaderText="Sub Location Name" SortExpression="SubLocation" />  
        <asp:BoundField DataField="quantity" HeaderText="Quantity" SortExpression="quantity" />  
   
    </Columns>  
</asp:GridView> 
<%--<asp:SqlDataSource ID="sqlconsumables" runat="server" ConnectionString="<%$ ConnectionStrings:ISMSCon %>"></asp:SqlDataSource>  
<asp:GridView ID="grdconsumables" runat="server"
   AutoGenerateColumns="False"  
    GridLines="None"  
    CssClass="mGrid"  
    AlternatingRowStyle-CssClass="alt" Font-Size="Small" Width="400px" EmptyDataText="No Consumables Found">  
    </asp:GridView> 
<asp:SqlDataSource ID="Sqlconsumables1" runat="server" ConnectionString="<%$ ConnectionStrings:ISMSCon %>"></asp:SqlDataSource>  --%>
</td>
</tr>
</table>
  </form>
</body>    
</html>
