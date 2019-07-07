<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="NewUser.aspx.vb" Inherits="NewUser" title="Add New User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%" >
    <tr class="tdcolbg">
<td>
  <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">	
	<tr class="trheaderbg">
		<td colspan="2">Add New User
				    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="users" />
        </td>
	</tr>
	<tr class="whitebg">
		<td style="width:40%;" class="tdtext"  align="right">EmployeeID :</td>
		<td style="width:60%;" class="tdtext" >
                <asp:TextBox ID="txtuserid" runat="server" CssClass="control" ValidationGroup="users"></asp:TextBox>
                <asp:RequiredFieldValidator ID="req1" ControlToValidate="txtuserid" runat="server" ErrorMessage="Enter Employee ID" Text="*" ValidationGroup="users"></asp:RequiredFieldValidator>
        </td>
	</tr>
    <tr class="whitebg">
		<td style="width:40%;" class="tdtext"  align="right">Employee Name :</td>
		<td style="width:60%;" class="tdtext" >
               <asp:TextBox ID="txtusername" runat="server" CssClass="control" ValidationGroup="users"></asp:TextBox>
               <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtusername" ErrorMessage="Enter Employee Name" Text="*" ValidationGroup="users"></asp:RequiredFieldValidator>
        </td>
	</tr>
	<tr class="whitebg">
		<td  align="right" class="tdtext">User Group :</td>
		<td class="tdtext">
		    <asp:DropDownList ID="drpusergroup" runat="server" CssClass="control">
		    </asp:DropDownList>
        </td>
	</tr>
		<tr class="whitebg">
		<td  align="right" class="tdtext">Status :</td>
		<td class="tdtext">
		    <asp:DropDownList ID="drpstatus" runat="server" CssClass="control">
		    <asp:ListItem Value="A">Active</asp:ListItem>
		    <asp:ListItem Value="I">Inactive</asp:ListItem>
		    </asp:DropDownList>
        </td>
	</tr>
	 <tr class="whitebg">
		<td align="right" class="tdtext">User Rights :</td>
		<td class="tdtext">
 <asp:DropDownList ID="drprights" runat="server" CssClass="control">
 <asp:ListItem Value="0">No Access</asp:ListItem>
 <asp:ListItem Value="1">Approve</asp:ListItem>
 <asp:ListItem Value="2">Issue</asp:ListItem>
 </asp:DropDownList></td>
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
		    <asp:ListItem Value="userid">EmployeeId</asp:ListItem>
		    <asp:ListItem Value="username">EmployeeName</asp:ListItem>
		    <asp:ListItem Value="usergroup">UserGroup</asp:ListItem>
		    <asp:ListItem Value="status">Status</asp:ListItem>
		    <asp:ListItem Value="isstag">Rights</asp:ListItem>
		    </asp:DropDownList></td>
		    <td class="tdtext">
		        <asp:TextBox ID="txtshempid" runat="server" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		   <td class="tdtext">
		        <asp:TextBox ID="txtshempname" runat="server" Visible="false" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpshusergroup" runat="server" Visible="false" Width="150px" CssClass="control">
            <asp:ListItem Value="1">Super Admin</asp:ListItem>
		    <asp:ListItem Value="2">Admin</asp:ListItem>
		    <asp:ListItem Value="3">User</asp:ListItem>
		    </asp:DropDownList>
		    </td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpshstatus" runat="server" Visible="false" Width="150px" CssClass="control">
		    <asp:ListItem Value="A">Active</asp:ListItem>
		    <asp:ListItem Value="I">Inactive</asp:ListItem>
		    </asp:DropDownList>
		    </td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpshrights" runat="server" Visible="false" Width="150px" CssClass="control">
		    <asp:ListItem Value="0">No Access</asp:ListItem>
             <asp:ListItem Value="1">Approve</asp:ListItem>
             <asp:ListItem Value="2">Issue</asp:ListItem>
		    </asp:DropDownList>
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
                    <asp:BoundField HeaderText="Employee ID" DataField="userid" SortExpression="userid" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Employee Name" DataField="username" SortExpression="username" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="User Group" DataField="usergroup" SortExpression="usergroup" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Status" DataField="status" SortExpression="status" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Rights" DataField="isstag" SortExpression="isstag" ItemStyle-HorizontalAlign="Center" />
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>Edit</HeaderTemplate> 
                  <ItemTemplate>
                  <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                  </ItemTemplate>
                  </asp:TemplateField>  
                   </Columns>
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
            </asp:GridView>	   
	    </td>
	</tr>
	</table>
</asp:Content>

