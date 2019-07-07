<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="AssetList.aspx.vb" Inherits="AssetList" title="Asset List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
<td>
    <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">	
	<tr class="trheaderbg">
		<td colspan="2">Add New <asp:Label ID="lblgroup" runat="server" ></asp:Label>		
		    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="assetname" />
		</td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" class="tdtext" align="right">Category Name :</td>		
		<td style="width:50%;" class="tdtext">
                <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="True" CssClass="control">
                 <asp:ListItem value="0">--Select--</asp:ListItem>
                </asp:DropDownList>
        </td>
	</tr>
	<tr class="whitebg">
		<td align="right" class="tdtext">Asset Code :</td>		
		<td class="tdtext">
            <asp:TextBox ID="txtAssetTypeCode" runat="server" Width="300px" 
                ValidationGroup="assetname" CssClass="control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtAssetTypeCode" Display="None" ErrorMessage="Enter Asset Code" 
                SetFocusOnError="True" ValidationGroup="assetname"></asp:RequiredFieldValidator>
                              </td>
	</tr>
	<tr class="whitebg">
		<td style="vertical-align:top;" align="right" class="tdtext">Asset Description :</td>		
		<td class="tdtext"><asp:TextBox ID="txtAssetTypeDesc" runat="server" Width="300px" CssClass="control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtAssetTypeDesc" Display="None" ErrorMessage="Enter Asset Description" 
                SetFocusOnError="True" ValidationGroup="assetname"></asp:RequiredFieldValidator>
                              </td>
	</tr>
	<tr class="whitebg">
		<td colspan="2" align="center">&nbsp;
		<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="lButton" 
                ValidationGroup="assetname" Width ="80px" />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>
	</table>
    </td>
    </tr>
    	<tr>
		<td colspan="2" align="left">
		<table style="padding-top:5px;" >
		<tr class="" id="trsearch" runat="server" visible = "false">
		    <td class="tdtext">Search By :</td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpassetlistsearch" runat="server" CssClass="control"> 
		         <asp:ListItem Value="AssetTypeCode">Asset Code</asp:ListItem>
                 <asp:ListItem Value="AssetTypeDesc">Asset Description</asp:ListItem>
            </asp:DropDownList></td>
		    <td class="tdtext">
		        <asp:TextBox ID="txtsearch" runat="server" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		    <td class="tdtext">
		    <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
</tr>
	        </table>
	        </td>
</tr>
	<tr class="whitebg" >
	    <td colspan="2" style="width:100%">
	    <table align="center" cellpadding="0" cellspacing="0" width="100%" style="background-color:White;">
	        <tr class="">
	            <td>
            <asp:GridView ID="GridView1" runat="server"  DataKeyNames="AssetTypeId"
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AllowSorting="true"
            AutoGenerateColumns="false" width="100%" EmptyDataText="No Data Found !" >
             <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                   <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Asset Code" SortExpression="AssetTypeCode" HeaderStyle-HorizontalAlign="Center" DataField="AssetTypeCode" ReadOnly="true" />
                    <asp:BoundField HeaderText="Asset Description" SortExpression="AssetTypeDesc" DataField="AssetTypeDesc" HeaderStyle-HorizontalAlign="Center" />
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                  <HeaderTemplate>Edit</HeaderTemplate> 
                  <ItemTemplate>
                  <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                  </ItemTemplate>
                     </asp:TemplateField>  
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("AssetTypeId") %>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
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

