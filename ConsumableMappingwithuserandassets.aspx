<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="ConsumableMappingwithuserandassets.aspx.vb" Inherits="ConsumableMappingwithuserandassets" title="Consumable Mapping with User and Assets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
<td>
<%--<asp:UpdatePanel ID="upanel12" runat="server" >
<ContentTemplate>--%>
   <table border="0" align="center" cellpadding="4" cellspacing="1"  width="100%" style="overflow:scroll;">	
	<tr class="trheaderbg">
		<td colspan="2">Consumables Mapping with Asset Types
			   
		</td>
	</tr>
	</table>
		<div style="width:100%;overflow-x:scroll; height = "auto"  >
		<table cellpadding="4" cellspacing="1" style="width:100%;border-bottom:#cccccc 1px solid;border-right:#cccccc 1px solid;border-left:#cccccc 1px solid;">
			<tr class="trheaderbg">
		<td align="center" style="width:30%;font-weight:bold;">Asset Type</td>
		<td align="center" style="width:10%;font-weight:bold;">To Asset</td>
		<td align="center" style="width:10%;font-weight:bold;">To User</td>
		<td align="center" style="width:10%;font-weight:bold;">To Printer</td>
		</tr>
	    <%

		     Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd1 As New System.Data.SqlClient.SqlCommand
	        Dim dtable As New System.Data.DataTable
	        dtable = GetAssetTypeList()
	        'Dim I As Integer
	         Dim altcolor As Integer = 1
	        If dtable.Rows.Count > 0 Then
	            For j As Integer = 0 To dtable.Rows.Count - 1
	                 %>        
	                <% If altcolor = 1 %>
		                        <tr class="whitebg"> 
		                        <% altcolor = 2 %>          
		                    <% else %>
		                        <tr class="whitebg" style="background-color:#F8F8FF;">
		                        <% altcolor = 1 %>          
		                    <%End If %>
					<td style="padding-left:3px;width:30%"  class="tdtext"> <% =dtable.Rows(j)("AssetTypeCode")%> </td>
					<td style="width:10%;font-weight:bold;" align="center"> 
					<%   If dtable.Rows(j)("toasset").ToString() = "1" Then%>
					 <input type="checkbox" checked="checked" id="chkasset<%=dtable.rows(j)("AssetTypeid")%>" name="chkasset<%=dtable.rows(j)("AssetTypeid")%>" />
					<% else %>
					<input type="checkbox" id="chkasset<%=dtable.rows(j)("AssetTypeid")%>" name="chkasset<%=dtable.rows(j)("AssetTypeid")%>" />
					 <% End if %>
					</td>
					<td style="width:10%;font-weight:bold;" align="center">
					<%   If dtable.Rows(j)("touser").ToString() = "1" Then%>
					<input type="checkbox" checked="checked" id="chkuser<%=dtable.rows(j)("AssetTypeid") %>" name="chkuser<%=dtable.rows(j)("AssetTypeid")%>" />  
					<% else %>
					<input type="checkbox" id="chkuser<%=dtable.rows(j)("AssetTypeid")%>" name="chkuser<%=dtable.rows(j)("AssetTypeid")%>" />  
					 <% End if %>
					</td>
                    <td style="width:10%;font-weight:bold;" align="center">
					<%   If dtable.Rows(j)("toprinter").ToString() = "1" Then%>
					<input type="checkbox" checked="checked" id="chkprinter<%=dtable.rows(j)("AssetTypeid") %>"" name="chkprinter<%=dtable.rows(j)("AssetTypeid")%>" />  
					<% else %>
					<input type="checkbox" id="chkprinter<%=dtable.rows(j)("AssetTypeid") %>" name="chkprinter<%=dtable.rows(j)("AssetTypeid")%>" />  
					 <% End if %>
					</td>
					</tr>
					 <%
	            Next
                 End If
		      %>
		  </table>
		</div>
	
		</td>
	</tr>
	<tr>
		<td colspan="2" align="center">
		      <asp:Button ID="btnSave" runat="server" Text="Save" 
                ValidationGroup="assetname" CssClass="lButton" Width ="80px" />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>	
<%--</ContentTemplate>
</asp:UpdatePanel>--%>

	</table>
</asp:Content>

