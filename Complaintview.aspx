<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Complaintview.aspx.vb" Inherits="Complaintview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" href="css/grid.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%"  class="mGrid">
	<tr>
		<th align="center" style='white-space:nowrap;'>Ticket#</th>
		<th align="center">User</th>
		<th align="center">Department</th>
		<th align="center">Location</th>
		<th align="center">Phone</th>
		<th align="center">Time</th>
		<th align="center">Category</th>
		<th align="center">Type</th>
		<th align="center">Complaint Description</th>
		<th align="center">Select</th>
	</tr>
	 <%
	     
	     Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd1 As New System.Data.SqlClient.SqlCommand
        Dim rdrApp1 As System.Data.SqlClient.SqlDataReader
	    Dim sql1 As String
	        con1.Open()
	     
	        Rw = 0
	     sql1 = "select c.subcatid,c.catid,c.complaintid,c.emp_number,e.emp_name+' '+e.emp_initial+'('+e.emp_number+')' as ename,c.cdate as cdate1,isnull(c.filename,'-') as filename,"
	     sql1 = sql1 & "e.dep_name,right(e.Emp_phone_ext,4) as emp_phone_ext,convert(varchar,c.cdate,100) as cdate,ct.catname,s.subcatname,"
	     sql1 = sql1 & "(select seatno from idpeapp.dbo.sipfloorplan where emp_number = c.emp_number) as seatno, "
	     sql1 = sql1 & "c.complaint"
	     sql1 = sql1 & " from tbl_hd_complaint c,idpeapp.dbo.View_SIP_ContractEmployee e,tbl_hd_category ct,tbl_hd_subcategory s"
	     sql1 = sql1 & " where e.emp_number = c.emp_number"
	     sql1 = sql1 & " and c.catid = ct.catid and s.subcatid = c.subcatid and c.cons_id = '0' and c.complaintid =" & Request.QueryString("CId") & " "
	     sql1 = sql1 & " order by c.cdate1 desc"
	     
	     cmd1.CommandText = sql1
	     cmd1.Connection = con1
	     'Response.Write(sql1)
	     'Response.End()
	    rdrApp1 = cmd1.ExecuteReader
	        While rdrApp1.Read
	         Rw = Rw + 1
       %>
       <tr>
	    
   		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><% End If %><%=rdrApp1("complaintid")%>
   		
   		</td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("ename")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"> <%Else%> <td align="center" style="font-family: Trebuchet MS; font-size:8pt"><%end if %><%=rdrApp1("dep_name")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("seatno")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("emp_phone_ext")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("cdate")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("catname")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("subcatname")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="left" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="left" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %>

		<%If rdrApp1("filename") <> "-" Then%>
		<a target="_new" href ="\\sipsv0033\ict-software$\helpdesk\<%=rdrApp1("filename")%>">  
		<img border = "0" src="images/att.jpg" /></a> 
		       
   		&nbsp;
   		<%end if %> 
   		<%=rdrApp1("complaint")%> 
		</td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %>
		
		<input type ="checkbox" name="ChkSelect1" />
		
		<input type ="hidden" name="TxtId<%=Rw%>" value=<%=rdrapp1("complaintid")%> />
		</td>
	</tr>
	     <%	     	     End While
	    rdrApp1.Close()
	    con1.Close()
	    %>
	    
	    <%
	        con1.Open()
	        sql1 = "select c.subcatid,c.catid,c.complaintid,c.complaint,c.emp_number,c.cdate as cdate1,"
	        sql1 = sql1 & "convert(varchar,c.cdate,100) as cdate,ct.catname,s.subcatname"
	        sql1 = sql1 & " from tbl_hd_complaint c,tbl_hd_category ct,tbl_hd_subcategory s"
	        sql1 = sql1 & " where c.catid = ct.catid and s.subcatid = c.subcatid and c.cons_id = '0' and c.complaintid =" & Request.QueryString("CId") & ""
	        sql1 = sql1 & " and c.emptag =1"
	        sql1 = sql1 & " order by c.cdate1 desc"
	        cmd1.CommandText = sql1
	        cmd1.Connection = con1
	        rdrApp1 = cmd1.ExecuteReader
	        While rdrApp1.Read
	            Rw = Rw + 1
       %>
       <tr>
	    
   		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><% End If %><%=rdrApp1("complaintid")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("emp_number")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %>&nbsp;</td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %>&nbsp;</td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %>&nbsp;</td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("cdate")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("catname")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("subcatname")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %><%=rdrApp1("complaint")%></td>
		<%If Rw Mod 2 = 0 Then%> <td align="center" bgcolor="#F9FCFF" style="font-family: Trebuchet MS; font-size: 8pt"><%Else%> <td align="center" style="font-family: Trebuchet MS; font-size: 8pt"><%end if %>
		
		<input type ="checkbox" name="ChkSelect<%=Rw%>" />
		
		<input type ="hidden" name="TxtId<%=Rw%>" value=<%=rdrapp1("complaintid")%> />
		</td>
	</tr>
	     <%
	     End While
	    rdrApp1.Close()
	    con1.Close()
	     con1 = Nothing
	    %>
	    
    </table>
        <table width ="100%">
    <tr>
    <td align ="right"> 
    <asp:Button ID="CmdSave" runat="server" Text="Select" style=" font-size: 8pt; background-color:#E6F2FF; color: #000080;border-style:groove;"   />&nbsp;
    <asp:Button ID="cmdcancel" runat="server" Text="Close" OnClientClick="window.close();" style=" font-size: 8pt; background-color:#E6F2FF; color: #000080;border-style:groove;"  />
    </td>
    </tr>
    </div>
    </form>
</body>
</html>
