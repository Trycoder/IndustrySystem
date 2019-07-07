<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="ConsumableRequest.aspx.vb" Inherits="ConsumableRequest" title="Consumable Request" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
<td>
   <asp:UpdatePanel ID="upanel1" runat="server">
   <ContentTemplate>
    <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
    <tr  class="trheaderbg">
        <td colspan="4">
        <div>Consumable Request</div>
    			<asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="assets" />
            </td>
   </tr>
   <tr  class="whitebg" runat="server" id="trmessage" visible="false">
     <td colspan="4">
       <asp:Label ID="lblMessage" runat="server"  class="red_text" ></asp:Label>
     </td>
   </tr>
   <tr class="whitebg">
                 <td align="right" class="tdtext" style="width:25%;" valign="top">Request Type :</td>
                 <td class="tdtext" valign="top" colspan="3">
                     <asp:RadioButtonList ID="rdorequest" runat="server"  AutoPostBack =true
                         RepeatDirection="Horizontal">
                         <asp:ListItem Value="Asset">Consumable To Asset</asp:ListItem>
                         <asp:ListItem Value="User">Consumable To User</asp:ListItem>
                     </asp:RadioButtonList>
                </td>
     </tr>
        <tr class="whitebg">
            <td align="right" class="tdtext" style="width:25%;" valign="top">
                Select Asset :</td>
            <td class="tdtext" style="width:15%;" valign="top">
                <asp:DropDownList ID="drpassettype" runat="server" AutoPostBack="true" 
                    CssClass="control">
                    <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="right" class="tdtext" style="width:15%;" valign="top">
                <asp:Label ID="lblassetno" runat="server" Text="Asset No :"></asp:Label>
            </td>
            <td class="tdtext" style="width:45%;">
                <asp:DropDownList ID="drpassetno" runat="server"
                    CssClass="control">
                    <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="cbouser"  CssClass="control" runat="server" Visible="false">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
               </asp:DropDownList>
            </td>
        </tr>
        <tr class="whitebg">
            <td align="right" class="tdtext" style="width:25%;" valign="top">
                Name of Consumable(s) :</td>
            <td class="tdtext" style="width:15%;" valign="top">
                <asp:DropDownList ID="drpConsName" runat="server" 
                    CssClass="control">
                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="right" class="tdtext" style="width:15%;" valign="top" >
                <asp:Label ID="lblconstitle" runat="server" Text="Select Consumable Model :" Visible="false"></asp:Label>
                </td>
            <td class="tdtext" style="width:45%;">
                <asp:DropDownList ID="drptonermodel" runat="server" CssClass="control" Visible="false">
                    <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="whitebg">
            <td  colspan="1" align="right" valign="top" class="tdtext">Request Message :</td>
            <td colspan="3" class="tdtext">		
                <asp:TextBox ID="txtremarks" runat="server" Height="150px" TextMode="MultiLine" 
                    Width="400px"></asp:TextBox>		
		    </td>
	    </tr>
<%--        <tr class="whitebg">
                <td width="70%" colspan="2" runat="server" id="tddata" class="tdtext">
                </td>
        </tr>--%>
    <tr class="whitebg">
                <td  colspan="4" class="tdtext" align="center" >
                <asp:Button ID="btnsave" runat="server" Text="Send Request" 
                        ValidationGroup="assets" Visible ="true" CssClass="lbutton" Width ="80px" />
                &nbsp;<asp:Button ID="btncancel" runat="server" Text="Cancel" Visible ="true" CssClass="lbutton" Width ="80px"  />
                 </td>
   </tr>
   </table>
   </ContentTemplate>
   </asp:UpdatePanel>
</td>
</tr>
     </table>
</asp:Content>
