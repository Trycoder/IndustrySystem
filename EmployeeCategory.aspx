<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="EmployeeCategory.aspx.vb" Inherits="EmployeeCategory" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table align="center" cellpadding="0" cellspacing="0" width="60%">
<tr class="tdcolbg">
    <td valign="top">
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	<tr class="trheaderbg">
		<td  colspan = "4">		 Employee Category
		
		    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		</td>
	</tr>
	<tr class="whitebg">
		<td align="right" style="width:20%;" class="tdtext">Category ode:</td>
		<td style="width:30%;" class="tdtext">
            <asp:TextBox ID="txtcategorycode" runat="server" 
                Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                Display="None" ErrorMessage="Enter Cat Code" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="txtcategorycode"></asp:RequiredFieldValidator>
        </td>
	
		<td align="right" style="width:20%;"  class="tdtext">Category Name :</td>
		<td class="tdtext" style="width:30%;" ><asp:TextBox ID="txtcategorydesc" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Enter Category Name" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="txtcategorydesc" 
                Display="None"></asp:RequiredFieldValidator>
        </td>
	</tr>


        <tr class="whitebg">
		<td align="right" style="width:20%;" class="tdtext">Working Hrs/Day </td>
		<td style="width:30%;" class="tdtext">
            <asp:TextBox ID="TxtWHrs" runat="server" 
                Width="50px" CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                Display="None" ErrorMessage="Enter Working Hrs" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="txtWHrs"></asp:RequiredFieldValidator>
        </td>
	
		<td align="right" style="width:20%;"  class="tdtext">Working Days/Month :</td>
		<td class="tdtext" style="width:30%;" ><asp:TextBox ID="TxtWDays" runat="server" Width="50px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ErrorMessage="Enter Working Days" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtWDays" 
                Display="None"></asp:RequiredFieldValidator>
        </td>
	</tr>


                <tr class="whitebg">
		<td align="right" style="width:20%;" class="tdtext">Attendance Bonus</td>
		<td style="width:30%;" class="tdtext">
            <asp:TextBox ID="TxtAB" runat="server" 
                Width="50px" CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                Display="None" ErrorMessage="Enter Working Hrs" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtAB"></asp:RequiredFieldValidator>
        </td>
	
		<td align="right" style="width:20%;"  class="tdtext">Sunday Bonus:</td>
		<td class="tdtext" style="width:30%;" ><asp:TextBox ID="TxtSB" runat="server" Width="50px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                ErrorMessage="Enter Working Days" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtSB" 
                Display="None"></asp:RequiredFieldValidator>
        </td>
	</tr>

            <tr class="whitebg">
		<td align="right" style="width:20%;" class="tdtext">Lunch Deduction/Day </td>
		<td style="width:30%;" class="tdtext" colspan="3">
            <asp:TextBox ID="TxtLD" runat="server" 
                Width="50px" CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                Display="None" ErrorMessage="Enter Working Hrs" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtLD"></asp:RequiredFieldValidator>
        </td>
	
    </tr>


    
	<tr class="whitebg">
		<td align="center" class="tdtext" colspan="4">&nbsp;
		<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="lButton" 
                ValidationGroup="category" Width ="80px" />&nbsp;
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
		    <asp:DropDownList ID="drpreportsearch" runat="server" CssClass="control"> 
		         <asp:ListItem Value="empcatcode">Cagegory Code</asp:ListItem>
                 <asp:ListItem Value="empcatname">Category Name</asp:ListItem>
            </asp:DropDownList></td>
		    <td class="tdtext">
		        <asp:TextBox ID="txtshsearch" runat="server" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		    <td class="tdtext">
		    <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
</tr>
	        </table>
	        </td>
</tr>
        </table>
        <table align="center" cellpadding="0" cellspacing="0" width="100%" style="background-color:white;">
	<tr class="">
	    <td class="tdtext">
	    <table align="center" cellpadding="0" cellspacing="0" width="100%" style="background-color:white;">
	        <tr class="">
	            <td>
                    <asp:GridView ID="grdreportcategory" runat="server"  DataKeyNames="empcatid" 
                    CssClass="mGrid"
                    GridLines="None"
                    AlternatingRowStyle-CssClass="alt"
                    AllowSorting="true"
                    PagerStyle-CssClass="pgr"  
                    AllowPaging="true" 
                    AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !">
                     <Columns>
                          <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate >S.No</HeaderTemplate>
                          <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Category Code" HeaderStyle-HorizontalAlign="Center" DataField="empcatcode" SortExpression="empcatcode" ReadOnly="true" />
                            <asp:BoundField HeaderText="Category Name" HeaderStyle-HorizontalAlign="Center" DataField="empcatname" SortExpression="empcatname" />
                            <asp:BoundField HeaderText="Working Hrs/Day" HeaderStyle-HorizontalAlign="Center" DataField="whrs" SortExpression="whrs" />
                            <asp:BoundField HeaderText="Working Days/Month" HeaderStyle-HorizontalAlign="Center" DataField="wdays" SortExpression="wdays" />
                            <asp:BoundField HeaderText="Att. Bonus" HeaderStyle-HorizontalAlign="Center" DataField="attbonus" SortExpression="attbonus" />
                            <asp:BoundField HeaderText="Sunday Bonus" HeaderStyle-HorizontalAlign="Center" DataField="sundaybonus" SortExpression="sundaybonus" />
                            <asp:BoundField HeaderText="Lunch Ded" HeaderStyle-HorizontalAlign="Center" DataField="lunchded" SortExpression="lunchded" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Edit</HeaderTemplate>
                          <ItemTemplate>
                          <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                          </ItemTemplate>
                             </asp:TemplateField>
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("empcatid")%>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
                          </ItemTemplate>
                             </asp:TemplateField>
                        </Columns>
                    </asp:GridView>	   
	            </td>
	        </tr>
	    </table>
	    </td>
	</tr>
	</table>
</asp:Content>








