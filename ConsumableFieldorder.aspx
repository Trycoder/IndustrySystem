<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="ConsumableFieldorder.aspx.vb" Inherits="ConsumableFieldorder" title="Consumable Field Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
<td>
<%--<asp:UpdatePanel ID="upanel12" runat="server" >
<ContentTemplate>--%>
   <table border="0" align="center" cellpadding="4" cellspacing="1"  width="100%" style="overflow:scroll;">	
	<tr class="trheaderbg">
		<td colspan="2">Consumable Attributes Order
			   
		</td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" align="right" class="tdtext">Select Asset Type :</td>
		<td style="width:50%;" class="tdtext">
                <asp:DropDownList ID="drpassets" runat="server" CssClass="control" AutoPostBack="true">
                 
                </asp:DropDownList>
        </td>
	</tr>
	</table>    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="100%">
<tr class="tdcolbg">
<td>
<%--<asp:UpdatePanel ID="upanel12" runat="server" >
<ContentTemplate>--%>
	<% If drpassets.SelectedValue > 0 Then%>
		<div style="width:100%; overflow-y:auto;"  height = "auto" >
			<table cellpadding="4" cellspacing="1" style="width:100%;border-bottom:#cccccc 1px solid;border-right:#cccccc 1px solid;border-left:#cccccc 1px solid;">
			<tr class="trheaderbg">
		<td align="center" style="width:50%;font-weight:bold;">Attribute Name</td>
		<td align="center" style="width:50%;font-weight:bold;">Sequence Order</td>
		</tr>
	    <%

		     Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd1 As New System.Data.SqlClient.SqlCommand
	        Dim dtable As New System.Data.DataTable
	        dtable = LoadDetailsView(drpassets.SelectedValue)
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
					<td style="padding-left:3px;width:30%"  class="tdtext"> <% =dtable.Rows(j)("attdesc")%> </td>
					<td style="width:10%;font-weight:bold;" align="left" > 
					<% If dtable.Rows(j)("seq") = "0" Then%>
					<input type="text"  value="0" class="control" id="txt<%=dtable.rows(j)("attid")%>" name="txt<%=dtable.rows(j)("attid")%>" />
					<% ElseIf dtable.Rows(j)("seq") = "1" Then%>
					<input type="text" value="<%=dtable.Rows(j)("seq")%>" class="control" id="txt<%=dtable.rows(j)("attid")%>" name="txt<%=dtable.rows(j)("attid")%>" />
				<% ElseIf dtable.Rows(j)("seq") = "2" Then%>
					<input type="text" value="<%=dtable.Rows(j)("seq")%>" class="control" id="txt<%=dtable.rows(j)("attid")%>" name="txt<%=dtable.rows(j)("attid")%>" />
				<% ElseIf dtable.Rows(j)("seq") = "3" Then%>
					<input type="text" value="<%=dtable.Rows(j)("seq")%>" class="control" id="txt<%=dtable.rows(j)("attid")%>" name="txt<%=dtable.rows(j)("attid")%>" />
					<%Else%>
					<input type="text"  value="<%=dtable.Rows(j)("seq")%>" class="control" id="txt<%=dtable.rows(j)("attid")%>" name="txt<%=dtable.rows(j)("attid")%>" />
					<%End If%>
				</td>
					 <%
	            Next
                 End If
		      %>
		  </table>
		</div>
	
		</td>
	</tr>
	<tr  class="whitebg">
		<td colspan="2" align="center">
		      <asp:Button ID="btnsave" runat="server" Text="Save" 
                ValidationGroup="assetname" CssClass="lButton" Width ="80px" />&nbsp;
		<asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>	
	<% End If%>
<%--</ContentTemplate>
</asp:UpdatePanel>--%>

	</table>
	</td>
	</tr>
	</table>
</asp:Content>

