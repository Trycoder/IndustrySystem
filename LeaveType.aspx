﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="LeaveType.aspx.vb" Inherits="LeaveType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table align="center" cellpadding="0" cellspacing="0" width="60%">
<tr class="tdcolbg">
    <td valign="top">
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	<tr class="trheaderbg">
		<td  colspan = "2">		 Leave Types
		
		    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		</td>
	</tr>
	<tr class="whitebg">
		<td align="right" style="width:50%;" class="tdtext">Leave Type Code :</td>
		<td style="width:50%;" class="tdtext">
            <asp:TextBox ID="txtcategorycode" runat="server" 
                Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                Display="None" ErrorMessage="Enter Leave Type Code" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="txtcategorycode"></asp:RequiredFieldValidator>
        </td>
	</tr>
    <tr class="whitebg">
		<td align="right" class="tdtext">Leave Type Description :</td>
		<td class="tdtext"><asp:TextBox ID="txtcategorydesc" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Enter Leave Type Description" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="txtcategorydesc" 
                Display="None"></asp:RequiredFieldValidator>
        </td>
	</tr>
    
	<tr class="whitebg">
		<td align="center" class="tdtext" colspan="2">&nbsp;
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
		         <asp:ListItem Value="leavetypecode">Leave Type Code</asp:ListItem>
                 <asp:ListItem Value="leavetypename">Leave Type Description</asp:ListItem>
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
	<tr class="">
	    <td class="tdtext">
	    <table align="center" cellpadding="0" cellspacing="0" width="100%" style="background-color:white;">
	        <tr class="">
	            <td>
                    <asp:GridView ID="grdreportcategory" runat="server"  DataKeyNames="leavetypeid" 
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
                            <asp:BoundField HeaderText="Leave Type Name" HeaderStyle-HorizontalAlign="Center" DataField="leavetypecode" SortExpression="leavetypecode" ReadOnly="true" />
                            <asp:BoundField HeaderText="Leave Type Description" HeaderStyle-HorizontalAlign="Center" DataField="leavetypename" SortExpression="leavetypename" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Edit</HeaderTemplate>
                          <ItemTemplate>
                          <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                          </ItemTemplate>
                             </asp:TemplateField>
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("leavetypeid")%>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
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






