<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="LocationMaster.aspx.vb" Inherits="LocationMaster" title="New Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="90%" align="center" border="0" cellpadding="0" cellspacing="0">
<tr class="tdcolbg">
<td>
  <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
        <tr class="trheaderbg">
            <td colspan="3">&nbsp;&nbsp;&nbsp;Add New Location
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="location" />
            </td>
        </tr>
        <tr class="whitebg">
            <td class="tdtext" width="50%" align="right"> Location 1:</td>
            <td class="tdtext" width="30%">
                <asp:DropDownList ID="drploc1" Width="250" class="control" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="">--Select--</asp:ListItem>
                    <asp:ListItem Value="1">SIP-YG</asp:ListItem>
                    <asp:ListItem Value="2">SIP-CG</asp:ListItem>
                    <asp:ListItem Value="3">SIP-IH</asp:ListItem>
                    <asp:ListItem Value="4">NDPC</asp:ListItem>
                    <asp:ListItem Value="5">SIP-PP</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="tdtext" width="20%"></td>
        </tr>
        <tr class="whitebg">
            <td class="tdtext"  align="right">Location 2:</td>
            <td class="tdtext" >
                <asp:DropDownList ID="drploc2" Width="250" class="control" runat="server" 
                    AutoPostBack="true">
                    <asp:ListItem Text="--Select--"></asp:ListItem>
                </asp:DropDownList>
            </td>
             <td class="tdtext">
             <asp:HyperLink ID="hdlink1" runat="server">New Location</asp:HyperLink>
           </td>
        </tr>
        <tr class="whitebg">
            <td class="tdtext"  align="right">Location 3:</td>
            <td class="tdtext" >
            <asp:TextBox ID="txtloc3" runat="server" class="control" Width="240"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtloc3" Display="None" ErrorMessage="Enter Sublocation" 
                SetFocusOnError="True" ValidationGroup="location"></asp:RequiredFieldValidator>
            </td>
             <td></td>
        </tr>
        <tr class="whitebg">
            <td class="tdtext" align="center" colspan="3">
                <asp:Button ID="btnSave" runat="server" Text="Save" class="lButton" Width="80" ValidationGroup="location"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="lButton" Width="80"/>
            </td>
        </tr>
    </table>
</td>
</tr>
<tr class="" >
		<td colspan="2" align="left">
		<table style="padding-top:5px;" >
		<tr class="" runat="server" id="trsearch" visible ="false">
		        <td class="tdtext">Search By :</td>
		         <td class="tdtext">
                <asp:DropDownList ID="drpshlocations" Width="250" class="control" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Location2">Location2</asp:ListItem>
                    <asp:ListItem Value="Location3">Location3</asp:ListItem>
                </asp:DropDownList>
		        </td>
		        <td class="tdtext">
		            <asp:DropDownList ID="drpshlocation2" Width="250" class="control" runat="server">
                    </asp:DropDownList>
		        </td>
		        <td class="tdtext">
		        <asp:TextBox ID="txtshlocation3" runat="server" Width="150px" CssClass="control" Visible="false"></asp:TextBox>
		        </td>
		        <td class="tdtext">
		        <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
        </tr>
	    </table>
	        </td>
	        </tr>
<tr class="whitebg">
    <td align="center">
        <asp:Label ID="lblmsg" runat="server" class="red_text"></asp:Label>        
    </td>
</tr>
<tr class="">
    <td class="tdtext">
    <table align="center" cellpadding="0" cellspacing="0" width="100%" style="background-color:white;">
        <tr class="">
            <td>
                <asp:GridView ID="grdLocation" runat="server" 
                CssClass="mGrid"
                GridLines="None" DataKeyNames="sublocid"
                AlternatingRowStyle-CssClass="alt"
                AllowSorting="true"
                AutoGenerateColumns="false" Width="100%">
                    <Columns>
                      <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                      <HeaderTemplate>S.No</HeaderTemplate>
                      <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                      </asp:TemplateField>
                      <asp:BoundField HeaderText="SubLocation Name" HeaderStyle-HorizontalAlign="Center" DataField="sublocname" SortExpression="sublocname" />
                      <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                         <ItemTemplate>
                             <asp:HyperLink ID="imgEdit" runat="server" ImageUrl="~/Images/edit.png"></asp:HyperLink>
                         </ItemTemplate>
                      </asp:TemplateField>                                 
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" >
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" CommandName="Deleterow" CommandArgument='<%# Eval("sublocid") %>' runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="javascript:return confirm('Are you Sure want to Delete This Item?');"  ></asp:ImageButton>
                          </ItemTemplate>
                             </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="HiddenField1" runat="server" />
            </td>
        </tr>
    </table>
    </td>
</tr>
</table>
</asp:Content>