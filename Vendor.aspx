<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="Vendor.aspx.vb" Inherits="Vendor" title="Vendor List" %>
<%@ Register Src="VendorListUserControl.ascx" TagName="DetailCon" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table align="center" cellpadding="0" cellspacing="0"  width="90%" >
<tr class="tdcolbg">
<td>
<table align="center" width="100%" cellpadding="4" cellspacing="1">
<tr class="tdcolbg">
		<td class="trheaderbg" colspan="2"><b>Add New Vendors</b>
		<asp:ValidationSummary 
                        ID="ValidationSummary1" runat="server" ShowMessageBox="True" 
                        ShowSummary="False" ValidationGroup="grpvendor" /></td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" class="tdtext" align="right">Vendor Name :</td>
		<td style="width:50%;">
            <asp:TextBox ID="txtvendorname" runat="server" ValidationGroup="grpvendor" 
                Width="200px" CssClass="control"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtvendorname" Display="Dynamic" 
                        ErrorMessage="Enter Vendor Name" SetFocusOnError="True" 
                        ValidationGroup="grpvendor" CssClass="control">*</asp:RequiredFieldValidator> 
            <asp:ImageButton ID="imgsrch" style="width: 16px; height: 16px" runat="server" ImageUrl="~/Images/searchs.png" OnClientClick="searchVendors()"  ></asp:ImageButton>
		   
	</tr>
	<tr class="whitebg">
		<td style="width:20%;" class="tdtext" align="right">Status :</td>
				<td style="width:80%;">
            <asp:DropDownList ID="drpstatus" runat="server" CssClass="control">
                <asp:ListItem Value="A">Active</asp:ListItem>
                <asp:ListItem Value="I">Inactive</asp:ListItem>
            </asp:DropDownList>
		</td>
	</tr>
	<tr class="whitebg">
		<td colspan="2" align="center">&nbsp;<asp:Button ID="btnSave" runat="server" 
                Text="Save" ValidationGroup="grpvendor" CssClass="lButton" Width ="80px"  />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px"  /></td>
	</tr>
	
</table>
</td>
</tr>
<tr class="" >
		<td align="left">
		<table style="padding-top:5px;" >
		<tr class="">
           
		    <td class="tdtext">Search By :</td>
		    <td class="tdtext"><asp:DropDownList ID="drpsearchvendor" runat="server" CssClass="control" AutoPostBack="true"> 
		    <asp:ListItem Value="VendorName">Vendorname</asp:ListItem>
		    <asp:ListItem Value="status">Status</asp:ListItem>
		    </asp:DropDownList></td>
		    <td class="tdtext"><asp:TextBox ID="txtvendorsearch" runat="server" Width="150px" CssClass="control"></asp:TextBox></td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpstatus1" runat="server" Visible="false" Width="150px" CssClass="control">
		    <asp:ListItem Value="A">Active</asp:ListItem>
		    <asp:ListItem Value="I">Inactive</asp:ListItem>
		    </asp:DropDownList></td>
		    <td class="tdtext"><asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
</tr>
	        </table>
	        </td>
	        </tr>
        <tr>
            <td>
                <uc1:DetailCon ID="ucModal" runat="server"  />
            </td>
        </tr>
         
	<tr class="">
	    <td>
	    
	   <asp:Panel ID="pnlvendor" runat="server">
            
	     <asp:GridView ID="grdvendor" runat="server"  DataKeyNames="VendorID" AutoGenerateColumns="false" 
              CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            PagerStyle-CssClass="pgr"  
            AllowPaging="true" AllowSorting="true" EmptyDataText="No Data Found !">
                     <Columns>
                  <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                        <HeaderTemplate>S.No</HeaderTemplate>
                        <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="VendorName" DataField="VendorName" SortExpression="VendorName" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                      <HeaderTemplate>Edit</HeaderTemplate> 
                      <ItemTemplate>
                        <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                      </ItemTemplate>
                  </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("VendorID") %>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
                          </ItemTemplate>
                             </asp:TemplateField>
                </Columns>
            </asp:GridView>	   
	   </asp:Panel>           
	    </td>
	</tr>
	</table>
	
</asp:Content>

