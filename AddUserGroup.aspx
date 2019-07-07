<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="AddUserGroup.aspx.vb" Inherits="AddUserGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%" >
    <tr class="tdcolbg">
<td>
  <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">	
	<tr class="trheaderbg">
		<td colspan="2">Add New User Group
				    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="users" />
        </td>
	</tr>
	<tr class="whitebg">
		<td style="width:40%;" class="tdtext"  align="right">GroupName :</td>
		<td style="width:60%;" class="tdtext" >
                <asp:TextBox ID="txtgroupname" runat="server" CssClass="control" ValidationGroup="users"></asp:TextBox>
                <asp:RequiredFieldValidator ID="req1" ControlToValidate="txtgroupname" runat="server" ErrorMessage="Enter Group Name" Text="*" ValidationGroup="users"></asp:RequiredFieldValidator>
        </td>
	</tr>
	<tr class="whitebg">
		<td colspan="2" align="center" class="tdtext">&nbsp;
		<asp:Button ID="btnSave" runat="server" Text="Save" 
                ValidationGroup="users" CssClass="lButton" Width ="80px" />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>
    </table>
</td>
</tr>
<tr class="" >
		<td colspan="2" align="left">
		<table style="padding-top:5px;" >
		<tr class="">
		    <td class="tdtext">Search By :</td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpemployeesearch" runat="server" CssClass="control" AutoPostBack="true"> 
		    <asp:ListItem Value="groupname">groupname</asp:ListItem>
		     </asp:DropDownList></td>
		    <td class="tdtext">
		        <asp:TextBox ID="txtshgroupname" runat="server" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		    <td class="tdtext">
		    <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
</tr>
	        </table>
	        </td>
	        </tr>
	<tr class="whitebg">
	    <td colspan="2"  style="width:100%">
            <asp:GridView ID="grdusers" runat="server"  DataKeyNames="id" 
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AllowSorting="true"
            AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !" >
              <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>S.No</HeaderTemplate>
                  <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                     <asp:BoundField HeaderText="Group Name" DataField="groupname" SortExpression="groupname" ItemStyle-HorizontalAlign="Center" />
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>Edit</HeaderTemplate> 
                  <ItemTemplate>
                  <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                  </ItemTemplate>
                  </asp:TemplateField>  
                    <asp:BoundField HeaderText="ID" DataField="id" SortExpression="id" ItemStyle-HorizontalAlign="Center" Visible="false" />
                   </Columns>
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
            </asp:GridView>	   
	    </td>
	</tr>
	</table>
</asp:Content>


