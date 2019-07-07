<%@ Page Language="VB" MasterPageFile="~/Master.master" EnableEventValidation="true" AutoEventWireup="false" CodeFile="AddConsumables.aspx.vb" Inherits="AddConsumables" title="Add New Consumables" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="90%" align="center"  border="0" cellpadding="0" cellspacing="0">
<tr class="tdcolbg">
<td>
   <asp:UpdatePanel ID="upanel1" runat="server">
   <ContentTemplate>
    <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
    <tr  class="trheaderbg">
    <td colspan="2">
        <div>Add New Consumables</div>
    			<asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="assets" />
		   
            </td>
   </tr>
   <tr  class="whitebg" runat="server" id="trmessage" visible ="false">
     <td colspan="2">
       <asp:Label ID="lblMessage" runat="server"  text="test"  Visible="false" class="red_text" ></asp:Label>
     </td>
   </tr>
   <tr class="whitebg">
            <td style="width:50%" align="right" class="tdtext">
                Select Category :</td>          
            <td style="width:50%" class="tdtext">
            <asp:DropDownList ID="drpCategory"   CssClass="control" runat="server" Width="100" AutoPostBack="true" >
            </asp:DropDownList>
            </td>
        </tr>
        <tr class="whitebg">
            <td  align="right" class="tdtext">Select Consumables :</td>            
            <td>
        <asp:DropDownList ID="drpConsType"  CssClass="control" runat="server" Width="100" AutoPostBack="true" >
            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
        </asp:DropDownList>
            </td>
        </tr>
        <tr class="whitebg">
            <td  align="right" class="tdtext">
                Quantity :</td>
            <td class="tdtext">
                <asp:TextBox ID="txtQuantity" runat="server"  CssClass="control" Width="90px"></asp:TextBox>
                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"  TargetControlID="txtQuantity" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtQuantity" Display="None" ErrorMessage="Enter Quantity" 
                SetFocusOnError="True" ValidationGroup="assets"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="whitebg">
            <td style="width:50%" align="right" class="tdtext">
              Select Location :</td>          
            <td style="width:50%" class="tdtext">
            <asp:DropDownList ID="drpLocation"   CssClass="control" runat="server" Width="200" AutoPostBack="true" >
            </asp:DropDownList>
            </td>
        </tr>
        <tr class="whitebg">
            <td  align="right" class="tdtext">Select SubLocation :</td>            
            <td>
        <asp:DropDownList ID="drpSubLocation"  CssClass="control" runat="server" Width="200" AutoPostBack="true" >
            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
        </asp:DropDownList>
            </td>
        </tr>
        <tr class="whitebg"  runat="server" id="trlocation" align="right" visible ="false">
        <td class="tdtext" colspan="2">
            <asp:Label ID="lbllocation" runat="server"></asp:Label>
        </td>
        </tr>
    <tr class="whitebg">
    <td width="70%" colspan="2" runat="server" id="tddata" class="tdtext">
        
    </td>
       </tr>
    <tr class="whitebg">
   <td colspan="2">
     <table style="width: 100%">
        <tr>
            <td  class="tdtext" align="left" style="padding-left:500px;">
                <asp:Button ID="btnsave" runat="server" Text="Save" ValidationGroup="assets" Visible ="false" CssClass="lButton" Width ="80px" />
&nbsp;<asp:Button ID="btncancel" runat="server" Text="Cancel" Visible ="false" CssClass="lButton" Width ="80px"  />
            </td>
        </tr>
       
        </table>
   </td>
   </tr>
   </table>
   </ContentTemplate>
   </asp:UpdatePanel>
   
</td>
</tr>
     </table>
</asp:Content>

