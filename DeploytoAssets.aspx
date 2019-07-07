<%@ Page Language="VB" MasterPageFile="~/Master.master" EnableEventValidation="true" AutoEventWireup="false" CodeFile="DeploytoAssets.aspx.vb" Inherits="DeploytoAssets" title="Deploy to Assets" %>
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
        <div>Deploy to Assets</div>
        </td>
   </tr>
   <tr  class="whitebg" runat="server">
     <td colspan="4">
       <asp:Label ID="lblMessage" runat="server"   class="red_text" ></asp:Label>
     </td>
   </tr>
   <tr class="whitebg">
                 <td align="right" class="tdtext" style="width:25%;" valign="top">Select Asset</td>
                 <td class="tdtext" style="width:15%;" valign="top">
                     <asp:DropDownList ID="drpassettype" runat="server" AutoPostBack="true" 
                         CssClass="control">
                         <asp:ListItem Value="">--Select--</asp:ListItem>
                     </asp:DropDownList>
                 </td>
                 <td align="right" class="tdtext" style="width:15%;" valign="top">
                     Asset No: </td>
                 <td class="tdtext" style="width:45%;">
                       <asp:DropDownList ID="drpassetno" runat="server" AutoPostBack="true" 
                           CssClass="control">
                           <asp:ListItem Value="">--Select--</asp:ListItem>
                       </asp:DropDownList>
                 </td>
     </tr>
        <tr class="whitebg">
            <td align="right" class="tdtext" style="width:25%;" valign="top">
                Name of Consumable(s):</td>
            <td class="tdtext" style="width:15%;" valign="top">
                <asp:DropDownList ID="drpConsName" runat="server" AutoPostBack="true" 
                    CssClass="control">
                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="right" class="tdtext" style="width:15%;" valign="top">
                Name of SubLocation :</td>
            <td class="tdtext" style="width:45%;">
                <asp:RadioButtonList ID="rdoSubLocation" runat="server"  AutoPostBack="true"
                    CssClass="control" RepeatColumns="2" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
   <tr class="whitebg" runat="server" id="trtoner" visible = "false">
                 <td align="right" class="tdtext" style="width:25%;">Select Consumable Model :</td>
                  <td class="tdtext" style="width:15%;">
                     <asp:DropDownList ID="drptonermodel" runat="server"  
                          CssClass="control" >
                         <asp:ListItem Value="">--Select--</asp:ListItem>
                     </asp:DropDownList>
                 </td>
        		<td colspan="2"></td>
	</tr>
	<tr class="whitebg" runat="server" id="trconsumables" visible = "false">
                 <td align="right" class="tdtext" >ConsumableDetails</td>
                  <td colspan="3" class="tdtext" id="tdconsumables">
                    
                 </td>
        	
	</tr>
     <tr class="whitebg">
                <td align="right" class="tdtext" style="width:25%;">Available Quantity :</td>
                <td class="tdtext" style="width:15%;">
                    <asp:Label ID="lblQuantity" runat="server" CssClass="control" Width="90px" BorderStyle="None"></asp:Label>
                </td>
                <td align="right" class="tdtext" style="width:15%;">Deployment Date :</td>
		        <td class="tdtext">
		            <asp:TextBox ID="txtdate" runat="server"  CssClass="control" ></asp:TextBox>
                       <asp:CalendarExtender ID="txtDeploydate_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtdate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                </td>
        </tr>
        <tr class="whitebg">
            <td  colspan="1" align="right" valign="top" class="tdtext">Remarks :</td>
            <td colspan="3" class="tdtext">		
                <asp:TextBox ID="txtremarks" runat="server" Height="50px" TextMode="MultiLine" 
                    Width="400px"></asp:TextBox>		
		    </td>
	    </tr>
<%--        <tr class="whitebg">
                <td width="70%" colspan="2" runat="server" id="tddata" class="tdtext">
                </td>
        </tr>--%>
    <tr class="whitebg">
                <td  colspan="4" class="tdtext" align="center" >
                <asp:Button ID="btnsave" runat="server" Text="Save" ValidationGroup="assets" Visible ="true" CssClass="lbutton" Width ="80px" />
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

