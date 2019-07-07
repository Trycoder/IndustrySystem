<%@ Page Title="Add New Softwares" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" EnableViewState="true" CodeFile="AddSoftwares.aspx.vb" Inherits="AddSoftwares" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="90%" align="center"  border="0" cellpadding="0" cellspacing="0">
<tr class="tdcolbg">
<td>
<%--<asp:UpdatePanel ID="up1" runat="server">
<ContentTemplate>--%>
   <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
    <tr  class="trheaderbg">
    <td colspan="2">
        <div>Add New Softwares</div>
    			<asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="softwares" />
                   <asp:Label ID="lblmessage" runat="server"></asp:Label>
		   </td>
   </tr>
      <tr  class="whitebg" runat="server" id="trmessage" visible ="false">
     <td colspan="2">
       <asp:Label ID="lblsuccess" runat="server"  text="test"  Visible="false" class="red_text" ></asp:Label>
     </td>
   </tr>
   <tr class="whitebg">
            <td style="width:50%" align="right" class="tdtext">
                Select Category :</td>          
            <td style="width:50%" class="tdtext">
        <asp:DropDownList ID="drpCategory"   CssClass="control" runat="server" Width="200px" AutoPostBack="true" >
                 </asp:DropDownList>
            </td>
        </tr>
        <tr class="whitebg">
            <td  align="right" class="tdtext">Select Softwares :</td>            
            <td>
        <asp:DropDownList ID="drpAssetType"  CssClass="control" runat="server" Width="300px" AutoPostBack="true" >
            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
        </asp:DropDownList>
            </td>
        </tr>
        <tr class="whitebg">
            <td  align="right" class="tdtext">
                No Of Licenses :</td>
            <td class="tdtext">
                <asp:TextBox ID="txtnooflicenses" runat="server"  CssClass="control" Width="90px"></asp:TextBox>
                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"  TargetControlID="txtnooflicenses" FilterType="Numbers"></asp:FilteredTextBoxExtender>
            </td>
        </tr>
                <tr class="whitebg">
            <td  align="right" class="tdtext">
               Location :</td>
            <td class="tdtext">
            <asp:DropDownList ID="drploc" runat="server" CssClass="control" AutoPostBack="true" Width="300px">
                                <asp:ListItem Value="">--Select--</asp:ListItem></asp:DropDownList>
                                <asp:ListSearchExtender ID="ListSearchExtender12" runat="server"  PromptCssClass="ListExtend" TargetControlID="drploc" ></asp:ListSearchExtender >          
            </td>
        </tr>
                        <tr class="whitebg">
            <td  align="right" class="tdtext">
             Sub Location :</td>
            <td class="tdtext">
          <asp:DropDownList ID="drpsubloc" runat="server" CssClass="control"  Width="300px">
                                <asp:ListItem Value="">--Select--</asp:ListItem></asp:DropDownList>
                                <asp:ListSearchExtender ID="ListSearchExtender13" runat="server"  PromptCssClass="ListExtend" TargetControlID="drpsubloc" ></asp:ListSearchExtender >
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
            <td  class="tdtext" align="left" style="padding-left:450px;">
                <asp:Button ID="btnsave" runat="server" Text="Save" ValidationGroup="assets" Visible ="false" CssClass="lButton" Width ="80px" />
&nbsp;<asp:Button ID="btncancel" runat="server" Text="Cancel" Visible ="false" CssClass="lButton" Width ="80px"  />
            </td>
        </tr>
        </table>
   </td>
   </tr>
   </table>
<%--</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="drpAssetType" />
</Triggers>
</asp:UpdatePanel>--%>
</td>
</tr>
     </table>
</asp:Content>

