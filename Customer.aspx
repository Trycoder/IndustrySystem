<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="Customer.aspx.vb" Inherits="Customer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table align="center" cellpadding="0" cellspacing="0" width="60%">
<tr class="tdcolbg">
    <td valign="top">
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="trheaderbg">
		    <td  colspan = "4">		 Customer
		        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		    </td>
	    </tr>
    	
        <tr class="whitebg">
	    	<td align="right" style="width:33%;" class="tdtext">Cust. Code :</td>
		    <td style="width:7%;" class="tdtext">
                <asp:TextBox ID="TxtSupplierCode" runat="server" 
                Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                Display="None" ErrorMessage="Enter Customre Code" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtSupplierCode"></asp:RequiredFieldValidator>
            </td>
	    
		    <td align="right" style="width:30%;" class="tdtext">Cust. Name :</td>
		    <td class="tdtext"style="width:30%;" ><asp:TextBox ID="TxtSupplierName" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Enter Customer Name" SetFocusOnError="True" 
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
	    	<td align="center" colspan="4" class="tdtext" >&nbsp;
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
		         <asp:ListItem Value="customercode">Customer Code</asp:ListItem>
                 <asp:ListItem Value="customername">Customer Name</asp:ListItem>
                 <asp:ListItem Value="CustomerContactPerson">Contact Person</asp:ListItem>
                <asp:ListItem Value="CustomerPhoneNo">Contact No</asp:ListItem>
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
                    <asp:GridView ID="grdreportcategory" runat="server"  DataKeyNames="customerid" 
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
                            <asp:BoundField HeaderText="Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width ="5%" DataField="customercode" SortExpression="customercode" ReadOnly="true"/>
                            <asp:BoundField HeaderText="Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%" DataField="customername" SortExpression="customername" />
                            <asp:BoundField HeaderText="Contact Person" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" DataField="customerContactPerson" SortExpression="customerContactPerson" />
                            <asp:BoundField HeaderText="Phone No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" DataField="customerPhoneNo" SortExpression="CustomerPhoneNo" />
                            <asp:BoundField HeaderText="GST No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" DataField="customerGSTNo" SortExpression="customerGSTNo" />
                         <asp:BoundField HeaderText="Address" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%" DataField="customerAddress" SortExpression="customerAddress" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate>Edit</HeaderTemplate>
                          <ItemTemplate>
                          <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                          </ItemTemplate>
                             </asp:TemplateField>
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("customerid")%>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
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


