<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="AgeAnalysisReport.aspx.vb" Inherits="AgeAnalysisReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%" >
 <tr class="tdcolbg">
<td>
<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
    <tr  class="trheaderbg">
    <td >
        <asp:Label ID="lblmessage" runat="server"></asp:Label>
</td>
</tr>
    <tr class="whitebg">
<td class="blue_text">
  <asp:LinkButton ID="lnkback" runat="server" Text="Back"></asp:LinkButton>
</td>
</tr>
    <tr class="whitebg" runat="server" id="trreports" visible="false">
<td class="blue_text">
</td>
</tr>
	<tr class="whitebg">
	    <td  align="center" style="width:100%">
	        <asp:GridView ID="grdreports" runat="server"  DataKeyNames="id" 
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" Width="100%" >
            <Columns>
                        <asp:TemplateField>
                        <HeaderTemplate>S.No</HeaderTemplate>
                        <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    </asp:TemplateField> 
           </Columns>
            </asp:GridView>	   
            
           <asp:GridView ID="GridView1" runat="server"  
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" Width="100%" >
            <Columns>
            <asp:TemplateField>
                        <HeaderTemplate>S.No</HeaderTemplate>
                        <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
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

