<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="Supplier.aspx.vb" Inherits="Supplier" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table align="center" cellpadding="0" cellspacing="0" width="60%">
<tr class="tdcolbg">
    <td valign="top">
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="trheaderbg">
		    <td  colspan = "4">		 Supplier
		        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		    </td>
	    </tr>
    	
        <tr class="whitebg">
	    	<td align="right" style="width:30%;" class="tdtext">Supplier Code :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="TxtSupplierCode" runat="server" 
                Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                Display="None" ErrorMessage="Enter Supplier Code" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtSupplierCode"></asp:RequiredFieldValidator>
            </td>
	    
		    <td align="right" style="width:30%;" class="tdtext">Suppler Name :</td>
		    <td class="tdtext"style="width:30%;" ><asp:TextBox ID="TxtSupplierName" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Enter Supplier Name" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtSupplierName" 
                Display="None"></asp:RequiredFieldValidator>
            </td>
	    </tr>
    
        <tr class="whitebg">
		    <td align="right" class="tdtext">Contact Person :</td>
		    <td class="tdtext"><asp:TextBox ID="TxtContactPerson" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ErrorMessage="Enter Supplier Address" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtContactPerson" 
                Display="None"></asp:RequiredFieldValidator>
            </td>
	    
		    <td align="right" class="tdtext">Phone No :</td>
		    <td class="tdtext"><asp:TextBox ID="TxtContactNo" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ErrorMessage="Enter Supplier Address" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtContactNo" 
                Display="None"></asp:RequiredFieldValidator>
            </td>
	    </tr>
        <tr class="whitebg">
		    <td align="right" class="tdtext">GST No :</td>
		    <td class="tdtext" colspan="3"><asp:TextBox ID="TxtGSTNo" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                ErrorMessage="Enter Supplier Address" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtGSTNo" 
                Display="None"></asp:RequiredFieldValidator>
            </td>
	    </tr>

        
        <tr class="whitebg">
		    <td align="right" class="tdtext" >Address :</td>
		    <td class="tdtext" align ="left" colspan ="3"><asp:TextBox ID="TxtSupplierAddress" runat="server" Width="500px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="Enter Supplier Address" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtSupplierAddress" 
                Display="None"></asp:RequiredFieldValidator>
            </td>
	    </tr>
    
    	<tr class="whitebg">
	    	<td align="center" colspan="4" class="tdtext" colspan="2">&nbsp;
		    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="lButton" 
                ValidationGroup="category" Width ="80px" />&nbsp;
		    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	    </tr>
	</table>
    </td>
	</tr>
    </table>
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	<tr>
		<td colspan="2" align="left">
		
            
    <table style="padding-top:5px;" width =" "100%">
		<tr class="">
		    <td class="tdtext">Search By :</td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpreportsearch" runat="server" CssClass="control"> 
		         <asp:ListItem Value="suppliercode">Supplier Code</asp:ListItem>
                 <asp:ListItem Value="suppliername">Supplier Name</asp:ListItem>
                 <asp:ListItem Value="SupplierContactPerson">Contact Person</asp:ListItem>
                <asp:ListItem Value="SupplierPhoneNo">Contact No</asp:ListItem>
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
                    <asp:GridView ID="grdreportcategory" runat="server"  DataKeyNames="supplierid" 
                    CssClass="mGrid"
                    GridLines="None"
                    AlternatingRowStyle-CssClass="alt"
                    PagerStyle-CssClass="pgr"  
                    AllowPaging="true" 
                    AllowSorting="true"
                    AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !">
                     <Columns>
                          <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate >S.No</HeaderTemplate>
                          <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width ="5%" DataField="suppliercode" SortExpression="suppliercode" ReadOnly="true"/>
                            <asp:BoundField HeaderText="Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%" DataField="supplierName" SortExpression="supplierName" />
                            <asp:BoundField HeaderText="Contact Person" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" DataField="SupplierContactPerson" SortExpression="SupplierContactPerson" />
                            <asp:BoundField HeaderText="Phone No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" DataField="SupplierPhoneNo" SortExpression="SupplierPhoneNo" />
                            <asp:BoundField HeaderText="GST No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" DataField="SupplierGSTNo" SortExpression="SupplierGSTNo" />
                         <asp:BoundField HeaderText="Address" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%" DataField="SupplierAddress" SortExpression="SupplierAddress" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate>Edit</HeaderTemplate>
                          <ItemTemplate>
                          <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                          </ItemTemplate>
                             </asp:TemplateField>
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("supplierid")%>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
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

