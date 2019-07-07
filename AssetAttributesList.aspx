<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="AssetAttributesList.aspx.vb" Inherits="AssetAttributesList" title="Asset/Consumable/Software Attributes List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%" >
    <tr class="tdcolbg">
<td>
  <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">	
	<tr class="trheaderbg">
		<td colspan="2"><asp:label ID="lblmessage" Text="" runat="server"></asp:label>
				    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="assetname" />
		</td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" class="tdtext"  align="right">Select Category :</td>
		<td style="width:50%;" class="tdtext" >
                <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="true"  CssClass="control" Width="130">
                </asp:DropDownList>
        </td>
	</tr>
	<tr class="whitebg">
		<td  align="right" class="tdtext">Attribute Description :</td>
		<td class="tdtext">
            <asp:TextBox ID="txtattributedesc" runat="server" Width="300px" 
                ValidationGroup="assetname" CssClass="control" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtattributedesc" Display="None" ErrorMessage="Enter Attribute Description" 
                SetFocusOnError="True" ValidationGroup="assetname"></asp:RequiredFieldValidator>
                              </td>
	</tr>
		<tr class="whitebg">
		<td  align="right" class="tdtext">Attribute ToolTip Text :</td>
		<td class="tdtext">
            <asp:TextBox ID="txttooltip" runat="server" Width="300px" 
                ValidationGroup="assetname" CssClass="control" ></asp:TextBox>
       </td>
	</tr>
	<tr class="whitebg">
		<td align="right" class="tdtext">Attribute Type :</td>
		<td class="tdtext">
            <asp:DropDownList ID="drpattributetype" runat="server" CssClass="control" Width="120">
                <asp:ListItem>Text(Variable)</asp:ListItem>
                <asp:ListItem>Text(Fixed)</asp:ListItem>
                <asp:ListItem>Sequence</asp:ListItem>
                 <asp:ListItem>Random</asp:ListItem>
                <asp:ListItem>Date</asp:ListItem>
                <asp:ListItem>Yes/No</asp:ListItem>
                <asp:ListItem>SingleSelection</asp:ListItem>
                <asp:ListItem>MultiSelection</asp:ListItem>
                <asp:ListItem>FileUpload</asp:ListItem>
                </asp:DropDownList>
                              </td>
	</tr>
	<tr class="whitebg">
		<td align="right" class="tdtext">Occurance :</td>
		<td class="tdtext">
            <asp:DropDownList ID="drpoccurance" runat="server" CssClass="control" Width="120">
                <asp:ListItem Value="0">Non-Mandatory</asp:ListItem>
                <asp:ListItem Value="1">Mandatory</asp:ListItem>
            </asp:DropDownList>
                              </td>
	</tr>
	<tr class="whitebg">
		<td align="right" class="tdtext">Header :</td>
		<td class="tdtext">
            <asp:DropDownList ID="drpheader" runat="server" Width="120" CssClass="control">
                <asp:ListItem Value="0">--Select--</asp:ListItem>
                <asp:ListItem Value="1">PO Number</asp:ListItem>
                <asp:ListItem Value="2">PO Date</asp:ListItem>
                <asp:ListItem Value="3">Warranty Start Date</asp:ListItem>
                <asp:ListItem Value="4">Warranty End Date</asp:ListItem>
                <asp:ListItem Value="5">Primary</asp:ListItem>
                <asp:ListItem Value="8">Lock No</asp:ListItem>
                <asp:ListItem Value="9">Contract No</asp:ListItem>
                <asp:ListItem Value="10">Contract Vendor</asp:ListItem>
            </asp:DropDownList>
            
                              </td>
	</tr>
	<tr class="whitebg">
		<td colspan="2" align="center" class="tdtext">&nbsp;
		<asp:Button ID="btnSave" runat="server" Text="Save" 
                ValidationGroup="assetname" CssClass="lButton" Width ="80px" />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>

    </table>
</td>
</tr>
<tr class="" id="trsearch" runat="server"  >
		<td colspan="2" align="left">
		<table style="padding-top:5px;" >
		<tr class="">
		    <td class="tdtext">Search By :</td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpshattributes" runat="server" CssClass="control" AutoPostBack="true"> 
		    <asp:ListItem Value="attdesc">Attribute Description</asp:ListItem>
		    <asp:ListItem Value="atttooltipdesc">Tooltip Text</asp:ListItem>
		    <asp:ListItem Value="atttype">Attribute Type</asp:ListItem>
		    <asp:ListItem Value="isrequired">Occurance</asp:ListItem>
		    <asp:ListItem Value="header">Header</asp:ListItem>
		    </asp:DropDownList></td>
		    <td class="tdtext">
		        <asp:TextBox ID="txtshdesc" runat="server" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		   <td class="tdtext">
		        <asp:TextBox ID="txtshtooltip" runat="server" Visible="false" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		    <td class="tdtext">
            <asp:DropDownList ID="drpshatttype" Visible="false" runat="server" CssClass="control" Width="120">
                <asp:ListItem>Text(Variable)</asp:ListItem>
                <asp:ListItem>Text(Fixed)</asp:ListItem>
                <asp:ListItem>Sequence</asp:ListItem>
                 <asp:ListItem>Random</asp:ListItem>
                <asp:ListItem>Date</asp:ListItem>
                <asp:ListItem>Yes/No</asp:ListItem>
                <asp:ListItem>SingleSelection</asp:ListItem>
                <asp:ListItem>MultiSelection</asp:ListItem>
                <asp:ListItem>FileUpload</asp:ListItem>
                </asp:DropDownList>
		    </td>
		    <td class="tdtext">
            <asp:DropDownList ID="drpshoccurance" Visible="false" runat="server" CssClass="control" Width="120">
                <asp:ListItem Value="0">Non-Mandatory</asp:ListItem>
                <asp:ListItem Value="1">Mandatory</asp:ListItem>
            </asp:DropDownList>
		    </td>
		    <td class="tdtext">
            <asp:DropDownList ID="drpshheader" Visible="false" runat="server" Width="120" CssClass="control">
                <asp:ListItem Value="1">PO Number</asp:ListItem>
                <asp:ListItem Value="2">PO Date</asp:ListItem>
                <asp:ListItem Value="3">Warranty Start Date</asp:ListItem>
                <asp:ListItem Value="4">Warranty End Date</asp:ListItem>
                <asp:ListItem Value="5">Primary</asp:ListItem>
                <asp:ListItem Value="8">Lock No</asp:ListItem>
                <asp:ListItem Value="9">Contract No</asp:ListItem>
                <asp:ListItem Value="10">Contract Vendor</asp:ListItem>
            </asp:DropDownList>
		    </td>
		    <td class="tdtext">
		    <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
</tr>
	        </table>
	        </td>
	        </tr>
	<tr class="whitebg">
	    <td colspan="2" align="center" style="width:100%">
            <asp:GridView ID="grdattributes" runat="server"  DataKeyNames="AttId" 
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AllowSorting="true"
            AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !" >
              <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                  <HeaderTemplate>S.No</HeaderTemplate>
                  <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-HorizontalAlign="Left" HeaderText="Attribute Description" DataField="AttDesc" SortExpression="AttDesc" HeaderStyle-HorizontalAlign="Center" />
                     <asp:BoundField ItemStyle-HorizontalAlign="Left" HeaderText="ToolTip Text" DataField="atttooltipdesc" SortExpression="atttooltipdesc" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Attribute Type" DataField="AttType" SortExpression="AttType" />
                    <asp:BoundField HeaderText="Occurance" DataField="IsRequired" ItemStyle-HorizontalAlign="Center"  SortExpression="IsRequired" />
                    <asp:BoundField HeaderText="Header" DataField="Header" ItemStyle-HorizontalAlign="Center" SortExpression="Header" />
                    <asp:BoundField HeaderText="Items" ItemStyle-HorizontalAlign="Center" />
                   <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" >
                  <HeaderTemplate>Edit</HeaderTemplate> 
                  <ItemTemplate>
                  <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                  </ItemTemplate>
                     </asp:TemplateField>
                                       <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("AttId") %>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');" ></asp:ImageButton>
                          </ItemTemplate>
                             </asp:TemplateField>  
                   </Columns>

<AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

            </asp:GridView>	   
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"  ConnectionString="<%$ ConnectionStrings:IMS %>" > 
            </asp:SqlDataSource>
	    </td>
	</tr>
	</table>
</asp:Content>

