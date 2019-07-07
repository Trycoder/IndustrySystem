<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="ConsumableStockList.aspx.vb" Inherits="ConsumableStockList" title="Consumable Available List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="90%" align="center"  border="0" cellpadding="0" cellspacing="0">
<tr class="tdcolbg">
<td>
 <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
	<tr class="trheaderbg">
		<td><b>Consumables Available List</b></td>
	</tr>
		<tr class="whitebg">
	    <td >
            <asp:GridView ID="grdconsumables" runat="server"  DataKeyNames="Consumableid"  
               CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" Width="100%" AllowSorting="true" >
                 <Columns>
                  <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                        <HeaderTemplate>S.No</HeaderTemplate>
                        <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Consumable Name" SortExpression="ConsumableName">
                    <ItemStyle ForeColor="Red" />
                    <ItemTemplate>
                    <%--javascript:window.open('ViewConsumables1.aspx?Cid=<%#Eval("consumableid")%>&subcat=<%#Eval("consumableid")%>','TestWindow','height=200,width=500,scrollbars=yes')--%>
                    <a href="#" onclick="javascript:window.open('ViewConsumables.aspx?CId=<%#Eval("consumableid")%>','TestWindow','height=600,width=750,resizable=1,scrollbars')"><%#Eval("ConsumableName")%></a>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Quantity" DataField="Quantity" SortExpression="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                   </Columns>

            </asp:GridView>	   
	    </td>
	</tr>
	</table>
</td>
	</tr>
	</table>
</asp:Content>

