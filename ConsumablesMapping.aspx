<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="~/ConsumablesMapping.aspx.vb" Inherits="ConsumablesMapping" title="Toner Mapping with Printers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
<td>
<%--<asp:UpdatePanel ID="upanel12" runat="server" >
<ContentTemplate>--%>
   <table border="0" align="center" cellpadding="4" cellspacing="1"  width="100%" style="overflow:scroll;">	
	<tr class="trheaderbg">
		<td colspan="2">Toner Mapping with Printers
		</td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" align="right" class="tdtext">Select Asset Type :</td>
		<td style="width:50%;" class="tdtext">
                <asp:DropDownList ID="drpassets" runat="server" CssClass="control" AutoPostBack="true">
                </asp:DropDownList>
        </td>
	</tr>
	<tr class="whitebg">
		<td style="width:50%;" align="right" class="tdtext">Select Consumable :</td>
		<td style="width:50%;" class="tdtext">
                <asp:DropDownList ID="drpconsumables" runat="server" AutoPostBack="true"  
                    CssClass="control">
                 <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
        </td>
	</tr>
	</table>
	 

	<% If drpassets.SelectedValue > 0 AndAlso drpconsumables.SelectedValue > 0 Then%>
		<div id="divtag" style="overflow:auto;height:500px;">
            <script type="text/javascript">
                var w = screen.width;
                document.getElementById('divtag').style.width = w - 145;
            </script>
		<table cellpadding="4" cellspacing="1"  style="border-bottom:#cccccc 1px solid;border-right:#cccccc 1px solid;border-left:#cccccc 1px solid;" >
			<tr class="trheaderbg">
		<td align="center" style="width:10%;font-weight:bold;">Printer Model</td>
	    <%

		     Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd1 As New System.Data.SqlClient.SqlCommand
		     Dim rdr1 As System.Data.SqlClient.SqlDataReader
	        Dim str1 As String
	        Dim dtable As New System.Data.DataTable
	        dtable = LoadDetailsView(drpconsumables.SelectedValue, "tbl_Asset_ConsumablesMaster", "ConsTypeId")
	        'Dim I As Integer
	        If dtable.Rows.Count > 0 Then
	            For j As Integer = 0 To dtable.Rows.Count - 1
	                 %>        
					<td style="width:5%;font-weight:bold;" align="center"> <% =dtable.Rows(j)(0)%> </td>
					 <%
	            Next
                 End If
		      %>
		      </tr>
		   <%
		   
		       Dim dtable1 As New System.Data.DataTable
		       Dim dtable2 As New System.Data.DataTable
		       dtable1 = LoadDetailsView(drpassets.SelectedValue, "tbl_Asset_Master", "AssetTypeid")
		       Dim altcolor As Integer = 1
		       If dtable1.Rows.Count > 0 Then
		           For j As Integer = 0 To dtable1.Rows.Count - 1
		                %>
		        <% If altcolor = 1 then %>
		         <tr class="whitebg" id="tr1<%=j%>">
		         <% altcolor = 2 %>
		         <%else %>
		         <% altcolor = 1 %>
		         <tr class="whitebg" id="tr1<%=j%>" style="background-color: #F8F8FF;">
		         <%end if %>
		    <td align="left" class="tdtext">
		    <% =dtable1.Rows(j)(0)%>
		    </td><%
		             dtable2 = LoadDetailsView(drpconsumables.SelectedValue, "tbl_Asset_ConsumablesMaster", "ConsTypeId")
		             If dtable2.Rows.Count > 0 Then
		                 For k As Integer = 0 To dtable2.Rows.Count - 1
		                     con1.Open()
		                     str1 = "select * from tbl_Asset_Mapping where AssetTypeId =" & dtable1.Rows(j)("AssetTypeid") & " and contypeid=" & dtable2.Rows(k)("constypeid") & "and assetmodel='" & dtable1.Rows(j)(0) & "' and conmodel = '" & dtable2.Rows(k)(0) & "'"
		                     cmd1.CommandText = str1
		                     cmd1.Connection = con1
		                     rdr1 = cmd1.ExecuteReader
		                     If Not rdr1.HasRows Then
		                     %>
		                <td align="center" class="tdtext">
		                <input type="checkbox" id="chk<%=dtable1.rows(j)("AssetTypeid")& dtable2.rows(k)("ConsTypeId") %>" name ="chk<%=j %><%=k %><%=dtable1.rows(j)("AssetTypeid")& dtable2.rows(k)("ConsTypeId") %>" /> 
		                </td>
		                <%Else%>
		                <td align="center" class="tdtext">
		                <input type="checkbox" id="Checkbox1" checked="checked" name ="chk<%=j %><%=k %><%=dtable1.rows(j)("AssetTypeid")& dtable2.rows(k)("ConsTypeId") %>" /> 
		                </td>
		                <%		                    		                    End If
		                    cmd1.Dispose()
		                    rdr1.Close()
		                    con1.Close()
		                    
		                    %>
		                
		                <%
		                 Next
		             End If
		         Next
             End If%>
		       </tr>
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
	<% End If%>
<%--</ContentTemplate>
</asp:UpdatePanel>--%>

	</table>
	
</asp:Content>
