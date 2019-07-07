<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="AssignAttributes.aspx.vb" Inherits="AssignAttributes" title="Assign Attributes List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
<table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
<tr class="tdcolbg">
<td>
    <table border="0" align="center" cellpadding="4" cellspacing="1"  width="100%" style="overflow:scroll;">	
	<tr class="trheaderbg">
		<td colspan="2">Assign Attributes To <asp:Label ID="lblgroup" runat="server" ></asp:Label>
			    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" ValidationGroup="assetname" />
		</td>
	</tr>
	<tr  class="whitebg">
		<td style="width:50%;" align="right" class="tdtext">Select Category :</td>
		<td style="width:50%;" class="tdtext">
                <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="true"  CssClass="control">
                </asp:DropDownList>
        </td>
	</tr>
	</table>
	
	<% If drpcategory.SelectedIndex > 0 Then%>
		<div style="overflow-x:scroll;" id="divtag">
            <script type="text/javascript">
                var w = screen.width;
                document.getElementById('divtag').style.width = w - 145;
            </script>
		<table cellpadding="4" cellspacing="1" style="width:100%;border-bottom:#cccccc 1px solid;border-right:#cccccc 1px solid;border-left:#cccccc 1px solid;">
			<tr class="trheaderbg">
			<td align="center" style="width:3%;">S.No</td>
		<td align="center" style="width:10%;">Attribute Name</td>
	    <td align="center" style="width:10%;">Attribute Type</td>
		 <%

		     Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd1 As New System.Data.SqlClient.SqlCommand
		     Dim rdr1 As System.Data.SqlClient.SqlDataReader
		     Dim str1 As String
		     'Dim I As Integer
		     con1.Open()
		     str1 = "select * from tbl_Asset_TypeMaster where catid=" & drpcategory.SelectedValue & " order by AssetTypeDesc"
		     cmd1.CommandText = str1
		     cmd1.Connection = con1
		     rdr1 = cmd1.ExecuteReader
		     While rdr1.Read
		                                 

                %>        
	
				<td style="width:10%;font-weight:bold;" align="center" >
		<% =rdr1("AssetTypeCode")%>
		</td>
	       <% 	        
	        End While
	        con1.Close()
	        rdr1.Close()
	        %>
		</tr>	
		 <%
		   
		     Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd As New System.Data.SqlClient.SqlCommand
		     Dim rdr As System.Data.SqlClient.SqlDataReader
		     Dim con2 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd2 As New System.Data.SqlClient.SqlCommand
		     Dim rdr2 As System.Data.SqlClient.SqlDataReader
		     Dim str, I As String
		     Dim n As Integer = 1
		     Dim altcolor As Integer = 1
		     con.Open()
		     str = "select * from tbl_Asset_Attributes where catid=" & drpcategory.SelectedValue
		     cmd.CommandText = str
		     cmd.Connection = con
		     rdr = cmd.ExecuteReader
		     If rdr.HasRows Then
		         While rdr.Read
		         
		        %>
		         <% If altcolor = 1 then  %>
		          <tr class="whitebg">
		          <%altcolor = 2 %>
		          <%else %>
		          <%altcolor = 1 %>
		          <tr class="whitebg" style="background-color:#F8F8FF;">
		          <%End If %>
		         <td style="width:3%;padding-left:3px;" class="tdtext" align="center"><% =n %></td>
		         <%n = n+ 1 %>
		    <td style="width:5%;padding-left:3px;" class="tdtext">
		    <% =rdr("AttDesc")%>
		    </td>
		    <td style="width:3%;padding-left:3px;" class="tdtext">
		    <% =rdr("AttType")%>
		    </td>
		     	<%	     con1.Open()
		     	    str1 = "select * from tbl_Asset_TypeMaster where catid=" & drpcategory.SelectedValue & " order by AssetTypeDesc"
		     cmd1.CommandText = str1
		     cmd1.Connection = con1
		     	    rdr1 = cmd1.ExecuteReader
		     	    If rdr1.HasRows Then
		     	        While rdr1.Read
		     	            con2.Open()
		     	            str1 = "select * from tbl_Asset_Attribute_Details where AssetTypeId =" & rdr1("AssetTypeId") & " and attid = " & rdr("AttId")
		     	            cmd2.CommandText = str1
		     	            cmd2.Connection = con2
		     	            rdr2 = cmd2.ExecuteReader
		     	            If Not rdr2.HasRows Then
                %>            
	
		                <td align="center" class="tdtext">
		                <input type="checkbox" id="chk<%=rdr1("AssetTypeId")& rdr("AttId") %>" name ="chk<%=rdr1("AssetTypeId")& rdr("AttId") %>" /> 
		                </td>
		                <%Else%>
		                <td align="center" class="tdtext">
		                <input type="checkbox" checked ="checked" id="Checkbox1" name ="chk<%=rdr1("AssetTypeId")& rdr("AttId") %>" /> 
		                </td>
		            <%	End if	
		                rdr2.Close()
		                con2.Close()
		            End While
		            Else%>
		        
		        <td colspan = "3" class="blue_textbold">
		                No Asset Type Found under this category
		        </td>
		        
	            <% 	            End If
	            rdr1.Close()
	            con1.Close()
		    %>
		</tr>
		    <%		        End While
		    End if
		
    rdr.Close()
    con.Close()
		    %>
		</table>
		
	<tr>
		<td colspan="3" align="center">&nbsp;
		</br>
		<asp:Button ID="btnSave" runat="server" Text="Save" 
                ValidationGroup="assetname" CssClass="lButton" Width ="80px" />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>	
	</div>
		
		</td>
	</tr>
	<% End If%>
	</table>
	
</asp:Content>
