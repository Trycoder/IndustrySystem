<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AssetandTransactionwiseview.aspx.vb" Inherits="AssetandTransactionwiseview" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Asset Count Details</title>
 <link rel="stylesheet" type="text/css" href="css/ApplyCSS1.css"/> 
  <link rel="stylesheet" type="text/css" href="Css/grid.css"/> 
 <link rel="Stylesheet" type="text/css" href="css/tabs.css" />
  </head>
<body style="margin-top:0; margin-left:0;">
 <form id="form1"  runat="server">
 <center>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr class="tdcolbg">
            <td align=  "center" class="tdtextbold" >
            <asp:Label ID ="LblCaption"  runat="server"> </asp:Label>
            <asp:Label ID ="LblSubCaption"  runat="server"> </asp:Label>
            </td>
        </tr>
    <tr>
    <td>
   <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
   		<tr  class="trheaderbg">
		<td align="center" style="font-family:Verdana;font-size:11px;"><asp:Label ID="lblmsg" runat="server"></asp:Label></td>
	    </tr>
        <tr class="whitebg">
            <td  align="center" class="tdtextbold" >
               <div id="" style="height:auto; overflow-x: hidden">
                <table width="100%" border="0" style="border-collapse:collapse">
                <asp:GridView ID="grdassets" runat="server"              CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AllowSorting="true"
            AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !" >
              <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>S.No</HeaderTemplate>
                  <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AssetType" SortExpression="assettypecode">
                             <ItemTemplate>
                                 <asp:Label id="lblassettype" CssClass="tdtext" runat ="server" text='<%# Eval("assettypecode")%>' />
                             </ItemTemplate>
                      </asp:TemplateField> 
                     <asp:BoundField HeaderText="Cond#1" DataField="att1" SortExpression="att1" />
                     <asp:BoundField HeaderText="Cond#2" DataField="att2" SortExpression="att2" />
                     <asp:BoundField HeaderText="Cond#3" DataField="att3" SortExpression="att3" />
                     <asp:BoundField HeaderText="Location" DataField="location" SortExpression="location" />
                     <asp:BoundField HeaderText="SubLocation" DataField="sublocation" SortExpression="sublocation" />
                     <asp:BoundField HeaderText="Deploy Type" DataField="deploytype" SortExpression="deploytype" />
                     <asp:BoundField ItemStyle-HorizontalAlign="Center" HeaderText="Quantity" DataField="qty" SortExpression="qty" />
                     <asp:BoundField HeaderText="Complaint Details" DataField="complaintdetails" SortExpression="complaintdetails" />
                     <asp:BoundField HeaderText="Request Details" DataField="requestdetails" SortExpression="requestdetails" />
                  </Columns>
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
           </asp:GridView>	 
              </table> 
               <table width="100%" border="0" style="border-collapse:collapse">
                <asp:GridView ID="GridView1" runat="server"              CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AllowSorting="true"
            AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !" >
              <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>S.No</HeaderTemplate>
                  <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AssetType" SortExpression="assettypecode">
                             <ItemTemplate>
                                 <asp:Label id="lblassettype" CssClass="tdtext" runat ="server" text='<%# Eval("assettypecode")%>' />
                             </ItemTemplate>
                      </asp:TemplateField> 
                      <asp:BoundField ItemStyle-HorizontalAlign="left" HeaderText="Model" DataField="att1" SortExpression="att1" />
                      <asp:BoundField ItemStyle-HorizontalAlign="Center" HeaderText="Quantity" DataField="quantity" SortExpression="quantity" />
                     <asp:BoundField ItemStyle-HorizontalAlign="Center" HeaderText="TransType" DataField="transtype" SortExpression="transtype" />
                     <asp:BoundField ItemStyle-HorizontalAlign="Center" HeaderText="TransDate" DataField="Transdate" SortExpression="Transdate" />
                     <asp:BoundField HeaderText="Transaction By" DataField="transby" SortExpression="transby" />
                   </Columns>
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
           </asp:GridView>	 
              </table> 
             <table width="100%" border="0" style="border-collapse:collapse">
                <asp:GridView ID="GridView2" runat="server"              CssClass="mGrid"
                    GridLines="None"  
                    AlternatingRowStyle-CssClass="alt"
                    AllowPaging="false"
                    AllowSorting="true"
                    AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !" >
              <Columns>
              </Columns>
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
           </asp:GridView>	 
              </table> 
                </div>               
            </td>
        </tr>
            <tr  class="whitebg">
            <td  align="center" class="tdtextbold" >
               <div id="Div1" style="height:auto; overflow-x: hidden">
                <table width="100%" border="0" style="border-collapse:collapse">
                <tr>
                 <td runat="server" id="tddata" class="tdtextbold">
                
                </td>
                </tr>
              </table> 
                </div>               
            </td>
             </tr>
    </table>
    </td>
    </tr>
    </table>
</center>
</form>
</body>
</html>
