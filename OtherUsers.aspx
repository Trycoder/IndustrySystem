<%@ Page Title="OtherUsers" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="OtherUsers.aspx.vb" Inherits="OtherUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="90%" align="center"  border="0" cellpadding="0" cellspacing="0">
<tr class="tdcolbg">
<td>
<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">	
	<tr class="trheaderbg">
		<td colspan="2">Other Users 
				    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="otheruser" />
		</td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" class="tdtext"  align="right">User Name :</td>
		<td style="width:50%;" class="tdtext" >
            <asp:TextBox ID="txtusername" runat="server" Width="300px" 
                ValidationGroup="otheruser" CssClass="control" ></asp:TextBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtusername" Display="None" ErrorMessage="Enter Username" 
                SetFocusOnError="True" ValidationGroup="otheruser"></asp:RequiredFieldValidator>
        </td>
	</tr>
	
		<tr class="whitebg">
		<td  align="right" class="tdtext">Department :</td>
		<td class="tdtext">
            <asp:TextBox ID="txtdepartment" runat="server" Width="300px" 
                ValidationGroup="otheruser" CssClass="control" ></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="txtdepartment" Display="None" ErrorMessage="Enter Department Name" 
                SetFocusOnError="True" ValidationGroup="otheruser"></asp:RequiredFieldValidator>
       </td>
	    </tr>
	<tr class="whitebg">
		<td align="right" class="tdtext">Location :</td>
		<td class="tdtext">
            <asp:TextBox ID="txtlocation" runat="server" Width="300px" 
                ValidationGroup="otheruser" CssClass="control" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="txtlocation" Display="None" ErrorMessage="Enter Location" 
                SetFocusOnError="True" ValidationGroup="otheruser"></asp:RequiredFieldValidator>
                              </td>
	</tr>
	<tr class="whitebg">
		<td align="right" class="tdtext">Status :</td>
		<td class="tdtext">
 <asp:DropDownList ID="drpstatus" runat="server" CssClass="control">
 <asp:ListItem Value="A">Active</asp:ListItem>
 <asp:ListItem Value="I">Inactive</asp:ListItem>
 </asp:DropDownList></td>
	</tr>
	<tr class="whitebg">
		<td colspan="2" align="center" class="tdtext">&nbsp;
		<asp:Button ID="btnSave" runat="server" Text="Save" 
                ValidationGroup="otheruser" CssClass="lButton" Width ="80px" />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>

    </table>
</td>
</tr>
<tr>
		<td colspan="2" align="left">
		<table style="padding-top:5px;" >
		<tr class="">
		    <td class="tdtext">Search By :</td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpotherusersearch" runat="server" CssClass="control" AutoPostBack="true"> 
		         <asp:ListItem Value="Emp_Number">Employee ID</asp:ListItem>
                 <asp:ListItem Value="Emp_Name">Employee Name</asp:ListItem>
                 <asp:ListItem Value="Dep_Name">Department</asp:ListItem>
                 <asp:ListItem Value="BuildingUnit">Location</asp:ListItem>
                 <asp:ListItem Value="Emp_Status">Status</asp:ListItem>
            </asp:DropDownList></td>
		    <td class="tdtext">
		        <asp:TextBox ID="txtshusers" runat="server" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		    <td class="tdtext">
		         <asp:DropDownList ID="drpshstatus" runat="server" CssClass="control" Visible = "false">
                     <asp:ListItem Value="A">Active</asp:ListItem>
                     <asp:ListItem Value="I">Inactive</asp:ListItem>
                     </asp:DropDownList>
		    </td>
		    <td class="tdtext">
		    <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
</tr>
	        </table>
	        </td>
</tr>
<tr class="">
    <td class="tdtext">
    <table align="center" cellpadding="0" cellspacing="0" width="100%" style="background-color:white;">
        <tr class="">
            <td>
                <asp:GridView ID="grdLocation" runat="server" 
                GridLines="None" DataKeyNames="Emp_Number"
                CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt"
                AllowSorting="true"
                AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !">
                
                <Columns>
                      <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                      <HeaderTemplate>S.No</HeaderTemplate>
                      <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                      </asp:TemplateField>
                      <asp:BoundField HeaderText="Emp ID" HeaderStyle-HorizontalAlign="Center" DataField="Emp_Number" SortExpression="Emp_Number"/>
                      <asp:BoundField HeaderText="User Name" HeaderStyle-HorizontalAlign="Center" DataField="Emp_Name" SortExpression="Emp_Name" />
                      <asp:BoundField HeaderText="Department" HeaderStyle-HorizontalAlign="Center" DataField="Dep_Name" SortExpression="Dep_Name"/>
                      <asp:BoundField HeaderText="Location" HeaderStyle-HorizontalAlign="Center" DataField="BuildingUnit" SortExpression="BuildingUnit"/>
                      <asp:BoundField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" DataField="Emp_Status" SortExpression="Emp_Status"/>
                      <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                         <ItemTemplate>
                             <asp:HyperLink ID="imgEdit" runat="server" ImageUrl="~/Images/edit.png"></asp:HyperLink>
                         </ItemTemplate>
                      </asp:TemplateField>                                 
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("Emp_Number") %>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
                          </ItemTemplate>
                             </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="HiddenField1" runat="server" />
            </td>
        </tr>
    </table>
    </td>
</tr>
</table>
</asp:Content>
