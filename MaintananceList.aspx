<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="MaintananceList.aspx.vb" Inherits="MaintananceList" title="Maintanance List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
<td>
    <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
	<tr class="trheaderbg">		
		<td colspan="2">Maintainance List		
	    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
            ShowMessageBox="True" ShowSummary="False" ValidationGroup="assetname" />
		</td>
	</tr>
	<tr class="whitebg">
		<td align="right" class="tdtext" style="width:50%;">Activity Type :</td>		
		<td class="tdtext" style="width:50%;">
          <asp:DropDownList ID="drptype" runat="server" Width="250" CssClass="control" AutoPostBack="true">
                 <asp:ListItem Value="">--Select--</asp:ListItem>
                 <asp:ListItem Value="0">Purchase</asp:ListItem>
                 <asp:ListItem Value="1">Warranty</asp:ListItem>
                 <asp:ListItem Value="2">Deployment</asp:ListItem>
                 <asp:ListItem Value="3">UnDeployment</asp:ListItem>
                 <asp:ListItem Value="4">Repair(Inhouse)</asp:ListItem>
                 <asp:ListItem Value="5">Repair(Outside)</asp:ListItem>
                 <asp:ListItem Value="6">Return</asp:ListItem>
                 <asp:ListItem Value="7">Retired</asp:ListItem>
                 <asp:ListItem Value="8">Sales</asp:ListItem>
                 <asp:ListItem Value="9">Deployment(Idle)</asp:ListItem>
                 <asp:ListItem Value="10">UnDeployment(Idle)</asp:ListItem>
                 <asp:ListItem Value="11">Location Change</asp:ListItem>
                 </asp:DropDownList>
        </td>
	</tr>
	<tr class="whitebg">
		<td style="vertical-align:top;" class="tdtext" align="right">Activity Name :</td>		
		<td><asp:TextBox ID="txtactivityname" runat="server" Width="240px" CssClass="control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtactivityname" Display="None" ErrorMessage="Enter Asset Description" 
                SetFocusOnError="True" ValidationGroup="assetname"></asp:RequiredFieldValidator>
                              </td>
	</tr>
	<tr class="whitebg">
		<td colspan="3" class="tdtext" align="center">&nbsp;
		<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="lButton" 
                ValidationGroup="assetname" Width ="80px" />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>	
	    </table>
	</td>
    </tr>
<tr class="" id="trsearch" runat="server" visible = "false" >
		<td colspan="2" align="left">
		<table style="padding-top:5px;" >
		<tr class="">
		    <td class="tdtext">Search By :</td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpactivitysearch" runat="server" CssClass="control"> 
		  <asp:ListItem Value="0">Purchase</asp:ListItem>
                 <asp:ListItem Value="1">Warranty</asp:ListItem>
                 <asp:ListItem Value="2">Deployment</asp:ListItem>
                 <asp:ListItem Value="3">UnDeployment</asp:ListItem>
                 <asp:ListItem Value="4">Repair(Inhouse)</asp:ListItem>
                 <asp:ListItem Value="5">Repair(Outside)</asp:ListItem>
                 <asp:ListItem Value="6">Return</asp:ListItem>
                 <asp:ListItem Value="7">Retired</asp:ListItem>
                 <asp:ListItem Value="8">Sales</asp:ListItem>
                 <asp:ListItem Value="9">Deployment(Idle)</asp:ListItem>
                 <asp:ListItem Value="10">UnDeployment(Idle)</asp:ListItem>
                 <asp:ListItem Value="11">Location Change</asp:ListItem>
		    </asp:DropDownList></td>
		    <td class="tdtext">
		        <asp:TextBox ID="txtshactivity" runat="server" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		    <td class="tdtext">
		    <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
</tr>
	        </table>
	        </td>
</tr>
    
	<tr class="whitebg">
		<td colspan="3">
	<table border="0" align="center" cellpadding="0" cellspacing="0" width="100%" style="background-color:White;">
   	<tr class="">
	    <td colspan="3" style="width:70%">
            <asp:GridView ID="grdmaintanance" runat="server"  DataKeyNames="id"  
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowSorting="true"
            AutoGenerateColumns="false" width="100%" EmptyDataText="No Data Found !" >
             <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                  <HeaderTemplate>S.No</HeaderTemplate>
                  <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                  </asp:TemplateField>
                  <asp:BoundField HeaderText="Activity Name"  HeaderStyle-HorizontalAlign="Center" DataField="activity" SortExpression="activity"  />
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                  <HeaderTemplate>Edit</HeaderTemplate> 
                  <ItemTemplate>
                  <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                  </ItemTemplate>
                     </asp:TemplateField>
                   <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("id") %>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
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

