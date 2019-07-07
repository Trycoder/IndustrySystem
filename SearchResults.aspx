<%@ Page Language="VB" EnableEventValidation="false"  MasterPageFile="~/Master.master" AutoEventWireup="true" CodeFile="SearchResults.aspx.vb" Inherits="SearchResults" title="Search Results" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
<table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%" >
    <tr class="tdcolbg">
    <td>
    <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
	<tr class="trheaderbg">
		<td>&nbsp; Search Results of :&nbsp;<asp:Label ID="lblSearchString" runat="server" Text="" ></asp:Label></td>
	</tr>
	<tr class="whitebg">
		<td class="tdtext">
            <asp:GridView ID="grdassets" runat="server"   CssClass="mGrid"
            GridLines="None"   DataKeyNames="assetid"
            AlternatingRowStyle-CssClass="alt"
             AllowSorting="true"
            AutoGenerateColumns="false" Width="100%"  >
                  <Columns>
                    <asp:TemplateField HeaderText="S.No"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="ICT Asset No" HeaderStyle-HorizontalAlign="Center" SortExpression="att1" >
                  <ItemTemplate>
                  <a href="#" onclick="javascript:window.open('ReportInfo.aspx?assetid=<%# Eval("Assetid") %>','ReportInfo','width=950px,height=800px,location=no,scrollbars=yes,left=200,top=75,screenX=0,screenY=100,resizable=1');return false;"><%# Eval("att1") %></a>
                     </ItemTemplate>
                     </asp:TemplateField>  
                    <asp:BoundField SortExpression="Emp_Number" DataField="Emp_Number" HeaderText="Emp No" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" />
                    <asp:BoundField SortExpression="EmpName" DataField="EmpName" HeaderText="Employee Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="center" />
                    <asp:BoundField SortExpression="Dep_Name" DataField="Dep_Name" HeaderText="Department" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="center" />
                    <asp:BoundField SortExpression="Emp_Phone_Ext" DataField="Emp_Phone_Ext" HeaderText="Phone" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"/>
                    <asp:BoundField SortExpression="BuildingUnit" DataField="BuildingUnit" HeaderText="Location" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="center"/>
                    <asp:BoundField SortExpression="status" DataField="status" HeaderText="Status" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" />
                     <asp:TemplateField HeaderText="Deploy / UnDeploy" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgdeploy" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Repair(In)" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgrepair" runat="server" ImageUrl="~/Images/Repair.png"  AlternateText="Repair" /> 
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Return" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgreturn" runat="server" ImageUrl="~/Images/Repair.png"  AlternateText="Return" /> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgchlocation" runat="server" ImageUrl="~/Images/location.png"  AlternateText="Change Location" /> 
                        </ItemTemplate>
                    </asp:TemplateField>
                  </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID = "SQL1" runat="server">
	</asp:SqlDataSource>
           </td>
	</tr>
	
</table>
    </td>
    </tr>
 </table>

</asp:Content>

