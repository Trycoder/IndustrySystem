<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="Material.aspx.vb" Inherits="Material" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table align="center" cellpadding="0" cellspacing="0" width="80%">
<tr class="tdcolbg">
    <td valign="top">
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="trheaderbg">
		    <td  colspan = "4">		 Supplier
		        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		    </td>
	    </tr>
    	
        <tr class="whitebg">
	    	<td align="right" style="width:30%;" class="tdtext">Material Code :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="TxtMaterialCode" runat="server" 
                Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                Display="None" ErrorMessage="Enter Material Code" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtMaterialCode"></asp:RequiredFieldValidator>
            </td>
	    
		    <td align="right" style="width:30%;" class="tdtext">Material Name :</td>
		    <td class="tdtext"style="width:30%;" ><asp:TextBox ID="TxtMaterialName" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Enter Material Name" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtMaterialName" 
                Display="None"></asp:RequiredFieldValidator>
            </td>
	    </tr>
    
        <tr class="whitebg">
		    <td align="right" class="tdtext">Unit1 :</td>
		    <td class="tdtext">
                <asp:DropDownList ID="CboUnit1" runat="server" Width="150" class="control" AutoPostBack="true">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                            </asp:DropDownList>
            </td>
		    <td align="right" class="tdtext">Unit2 :</td>
		    <td class="tdtext">
                                <asp:DropDownList ID="CboUnit2" runat="server" Width="150" class="control" AutoPostBack="true">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                            </asp:DropDownList>
            </td>
	    </tr>
        <tr class="whitebg">
		    <td align="right" class="tdtext">Min Order Qty :</td>
		    <td class="tdtext" ><asp:TextBox ID="TxtMinQty" runat="server" Width="80px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                ErrorMessage="Enter Min Qty" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtMinQty" 
                Display="None"></asp:RequiredFieldValidator>
            </td>

            <td align="right" class="tdtext">Re-Order Qty :</td>
		    <td class="tdtext" ><asp:TextBox ID="TxtReOrderQty" runat="server" Width="80px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ErrorMessage="Enter Min Qty" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtReorderQty" 
                Display="None"></asp:RequiredFieldValidator>
            </td>


	    </tr>


        <tr class="whitebg">
		    <td align="right" class="tdtext">Price :</td>
		    <td class="tdtext" ><asp:TextBox ID="TxtPrice1" runat="server" Width="80px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="Enter Price1" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtPrice1" 
                Display="None"></asp:RequiredFieldValidator>
                <asp:Label id="LblUnit1P" runat="server" Width="100" Text=""></asp:Label>
            </td>

            <td align="right" class="tdtext">Price :</td>
		    <td class="tdtext" ><asp:TextBox ID="TxtPrice2" runat="server" Width="80px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ErrorMessage="Enter Price1" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtPrice2" 
                Display="None"></asp:RequiredFieldValidator>
                <asp:Label id="LblUnit2P" runat="server" Width="100" Text="" ></asp:Label>
            </td>


	    </tr>

        
        <tr class="whitebg">
		    <td align="right" class="tdtext" >Supplier :</td>
		    <td class="tdtext" align ="left">
                 <asp:DropDownList ID="CboSupplier" runat="server" Width="220" class="control" AutoPostBack="true">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>


            
            <td align="right" class="tdtext">Conversion :</td>
		    
            <td class="tdtext" >
                <asp:Label id="LblUnit1C" runat="server" Width="52px" align="left"></asp:Label>
                <asp:TextBox ID="TxtU1U2" runat="server" Width="80px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                ErrorMessage="Enter Price1" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtU1U2" 
                Display="None"></asp:RequiredFieldValidator>
                <asp:Label id="LblUnit2C" runat="server" Width="100" Text=""></asp:Label>
            </td>

	    </tr>
    
    	<tr class="whitebg">
	    	<td align="center" colspan="4" class="tdtext">&nbsp;
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
		         <asp:ListItem Value="matcode">Material Code</asp:ListItem>
                 <asp:ListItem Value="matdesc">Material Name</asp:ListItem>
                <asp:ListItem Value="suppliername">Supplier Name</asp:ListItem>
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
                    <asp:GridView ID="grdreportcategory" runat="server"  DataKeyNames="matid" 
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
                            <asp:BoundField HeaderText="Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width ="5%" DataField="matcode" SortExpression="matcode" ReadOnly="true"/>
                            <asp:BoundField HeaderText="Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="35%" DataField="matdesc" SortExpression="matdesc" />
                            <asp:BoundField HeaderText="Unit1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" DataField="unit1" SortExpression="unit1" />
                            <asp:BoundField HeaderText="Unit2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" DataField="unit2" SortExpression="unit2" />
                            <asp:BoundField HeaderText="Supplier" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" DataField="suppliername" SortExpression="suppliername" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate>Edit</HeaderTemplate>
                          <ItemTemplate>
                          <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                          </ItemTemplate>
                             </asp:TemplateField>
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("matid")%>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
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


