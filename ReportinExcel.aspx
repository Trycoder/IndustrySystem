<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="ReportinExcel.aspx.vb" Inherits="ReportinExcel" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Report Info</title>
</head>
<body>
    <form id="form1" runat="server">
     <table border="0" align="center" cellpadding="0" cellspacing="0"  width="100%" >
 <tr>
<td>
<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
        <tr>
<td">
<table>
<tr>
<td align="left"><b>Report Name :</b></td>
<td colspan="3"><asp:Label ID="lblreportname" runat="server" ></asp:Label></td>
</tr>
<tr>
<td align="left"><b>Condition :</b></td>
<td  colspan="3"><asp:Label ID="lblcondition" runat="server"></asp:Label></td>
</tr>
</table>
</td>
</tr>
	<tr>
	    <td  align="center" style="width:100%">
	      <table cellpadding="0" cellspacing="0" style="border-collapse:collapse;border:#ccccff 1px solid;" border="1" width = "100%"  >
            <tr>
            <td>
           
            <%  RType = Request.QueryString("RType")
                If UCase(RType) = UCase("User") Then
                    Dim reportheaderarr() As String = ReportHeaderDesc.Split("|")
                    %>
                    <tr class="trheaderbg">
                    <td style="font-weight:bold;">S.No</td>
                    <%
                        For j As Integer = 0 To reportheaderarr.Length - 1
                            cntrh = j + 1
                    %>
                            <td style="font-weight:bold;"><%=reportheaderarr(j)%></td>         
                    <%                   
                    Next
                    %>
                              </tr>
                    <%
                        If ReportHeaders.Contains("v_emp") = True Or condition.Contains("v_emp") = True Or orderby.Contains("v_emp") = True Or ReportHeaders.Contains("BuildingUnit") = True Or ReportHeaders.Contains("Location") = True Or ReportHeaders.Contains("seatno") = True Then
                            V_Emp_Bool = True
                        End If
                        If ReportHeaders.Contains("v_trans") = True Or condition.Contains("v_trans") = True Or orderby.Contains("v_trans") = True Then
                            V_Trans_Bool = True
                        End If
                    
                    If V_Emp_Bool = True And V_Trans_Bool = True Then
                        SqlStr = "select "
                        If Len(Trim(orderby)) > 0 Then
                            SqlStr = SqlStr & orderby & ","
                        End If
                            SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset, view_SIP_Employees v_emp, view_asset_transactions v_trans"
                            SqlStr = SqlStr & " where v_asset.userid = v_emp.emp_number and v_asset.assetid  = V_trans.assetid  "
                    ElseIf V_Emp_Bool = True And V_Trans_Bool = False Then
                        SqlStr = "select "
                        If Len(Trim(orderby)) > 0 Then
                            SqlStr = SqlStr & orderby & ","
                        End If
                            SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset left join view_SIP_Employees v_emp on v_asset.userid = v_emp.emp_number  "
                            SqlStr = SqlStr & "    "
                    ElseIf V_Emp_Bool = False And V_Trans_Bool = True Then
                        SqlStr = "select "
                        If Len(Trim(orderby)) > 0 Then
                            SqlStr = SqlStr & orderby & ","
                        End If
                            SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset, view_asset_transactions v_trans, "
                            SqlStr = SqlStr & " where v_asset.assetid  = V_trans.assetid  "
                    Else
                        SqlStr = "select "
                        If Len(Trim(orderby)) > 0 Then
                            SqlStr = SqlStr & orderby & ","
                            End If
                            If ReportHeaders = "" Then
                                SqlStr = SqlStr.Remove(SqlStr.Length - 1, 1)
                                SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset "
                            Else
                                SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset "
                            End If
                            SqlStr = SqlStr & " where 1 = 1  "
                        End If
                        If SqlStr.Contains("v_emp.Emp_Name") Then
                            SqlStr = SqlStr.Replace("v_emp.Emp_Name", "v_emp.Emp_Name + ' ' + V_emp.Emp_Initial")
                        End If
                        If SqlStr.Contains("v_emp.BuildingUnit") Then
                            SqlStr = SqlStr.Replace("v_emp.BuildingUnit", "v_emp.BuildingUnit + '-' + v_emp.seatno")
                        End If
                        If Len(Trim(condition)) > 0 Then
                            If SqlStr.Contains("where") Then
                                SqlStr = SqlStr & " and " & condition & " "
                            Else
                                SqlStr = SqlStr & " where " & condition & " "
                            End If
                        End If
                        If Len(Trim(assettypes)) > 0 Then
                            SqlStr = SqlStr & " and v_asset.assettypeid " & assettypes & " "
                        End If
                        If ViewState("status") IsNot Nothing Then
                            SqlStr = SqlStr & " and v_asset.assettypeid " & assettypes & " and  " & ViewState("status")
                        End If
                        If Len(Trim(orderby)) > 0 Then
                            If orderby.Contains("assettypecode") = True then
                                 SqlStr = SqlStr & " order by v_asset.assettypecode "    
                            Else
                                SqlStr = SqlStr & " order by isnull(" & orderby & ",'') "     
                            End If
                        End If
                         sqlcon.Open()
                        sqlcmd = New SqlCommand
                    sqlcmd.Connection = sqlcon
                    sqlcmd.CommandType = Data.CommandType.Text
                    sqlcmd.CommandText = SqlStr
                        Try
                            drAsset = sqlcmd.ExecuteReader
                        Catch ex As Exception
                            Dim myscript As String = "alert('Error - " & ex.Message.ToString() & "');"
                            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                            Exit Sub
                        End Try
                        Dim m As Integer = 1
                        If drAsset.HasRows Then
                           While drAsset.Read
                            %>
                              <tr>
                             <%
                             If orderby <> "" then
                             
                                If TempOrderBy = "" Or TempOrderBy <> Convert.ToString(drAsset(0)) Then
                            %>
                             
                                <td class = "blue_textbold" style="font-weight:bold;font-size:11px;padding-left:2px;" align="left" colspan ="<%=cntrh + 1%>"  >
                                <%=drAsset(0)%>
                                </td>
                                </tr>
                                <tr>
                                <td class = "blue_text"><%=m%></td>
                            <%
                                TempOrderBy = drAsset(0)
                            Else
                                %> 
                                   <td class = "blue_text"><%=m%></td> 
                                <%
                            End If
                            Else
                             %> 
                                   <td class = "blue_text"><%=m%></td> 
                                <%
                            End if
                            m = m + 1
                            If orderby.Length > 0 then
                              For i = 1  To cntrh 
                                %>
                                <td class = "blue_text" align = "left">
                                <%=drAsset(i)%>
                                </td>
                            <%
                            Next
                            Else
                              For i = 0  To cntrh -1
                             %>
                                <td class = "blue_text" align = "left">
                                <%=drAsset(i)%>
                                </td>
                            <%
                            Next
                          End if
                            %>
                              </tr>
                            <%
                        End While
                            
                    End If
                    sqlcmd.Dispose()
                        drAsset.Close()
                    End If
             %>
            </td>
            </tr>
            </table>
	    </td>
	</tr>
	</table>
	</td>
	</tr>
	</table>
    </form>
  </body>
  </html>  
   
