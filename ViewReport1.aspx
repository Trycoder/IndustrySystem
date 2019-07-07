<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" Inherits="ViewReport1" title="View Reports" CodeFile="ViewReport1.aspx.vb" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <table border="0" align="center" cellpadding="0" cellspacing="0"  width="95%" >
 <tr class="tdcolbg">
<td>
<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
    <tr  class="trheaderbg">
    <td>
  View Report 
</td>
</tr>
    <tr class="whitebg">
  <td class="tdtext">
  <asp:Button ID="btnsave" runat="server" Text="Save Report" CssClass="lButton" Width="100px" />&nbsp;&nbsp;&nbsp;
  <asp:Button ID="btncancel" runat="server" Text="Cancel Report" CssClass="lButton" Width="100px" />
  </td>
</tr>
<tr class="whitebg">
<td class="tdtext">
        <table>
        <tr>
        <td class="tdtextbold" align="right">Report Name :</td>
        <td class="tdtext"><asp:Label ID="lblreportname" runat="server" ></asp:Label></td>
        </tr>
        <tr>
        <td class = "tdtextbold" align="right">Condition :</td>
        <td class = "tdtext"><asp:Label ID="lblcondition" runat="server"></asp:Label></td>
        </tr>
        </table>
</td>
</tr>

	<tr class="whitebg">
	    <td  align="center" style="width:100%">
	        <table cellpadding="0" cellspacing="0" style="border-collapse:collapse;border:1px solid #ccccff" border="1" width = "100%"  >
            <tr>
            <td>
           
            <%
                If String.IsNullOrEmpty(Session("Values")) = False Then
                    
                    If sqlcon.State = Data.ConnectionState.Open Then
                        sqlcon.Close()
                    End If
                    
                    Dim reportheaderarr() As String = ReportHeaderDesc.Split("|")
                    %>
                    <tr class="trheaderbg">
                    <td>S.No</td>
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
                            SqlStr = SqlStr & reportheaders & " from view_assetmaster_status v_asset, view_asset_transactions v_trans "
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
                        Dim altcolor As Integer = 1
                        If drAsset.HasRows Then
                           While drAsset.Read
                            %>
                                 <% If altcolor = 1 %>
		                        <tr class="whitebg"> 
		                        <% altcolor = 2 %>          
		                    <% else %>
		                        <tr class="whitebg" style="background-color:#F8F8FF;">
		                        <% altcolor = 1 %>          
		                    <%End If %>
		                    <%
                             If orderby <> "" then
                             
                                If TempOrderBy = "" Or TempOrderBy <> Convert.ToString(drAsset(0)) Then
                            %>
                             
                                <td class = "tdtextbold"  style="font-weight:bold;font-size:11px;padding-left:2px;" align="left" colspan ="<%=cntrh + 1%>"  >
                                <%=drAsset(0)%>
                                </td>
                                </tr>
                                 <% If altcolor = 1 %>
		                        <tr class="whitebg"> 
		                        <% altcolor = 2 %>          
		                    <% else %>
		                        <tr class="whitebg" style="background-color:#F8F8FF;">
		                        <% altcolor = 1 %>          
		                    <%End If %>
                                <td class = "tdtext"><%=m%></td>
                            <%
                                TempOrderBy = drAsset(0)
                            Else
                                %> 
                                   <td class = "tdtext"><%=m%></td> 
                                <%
                            End If
                            Else
                             %> 
                                   <td class = "tdtext"><%=m%></td> 
                                <%
                            End if
                            m = m + 1
                            If orderby.Length > 0 then
                              For i = 1  To cntrh 
                                %>
                                <td class = "tdtext" align = "left">
                                <%=drAsset(i)%>
                                </td>
                            <%
                            Next
                            Else
                              For i = 0  To cntrh -1
                             %>
                                <td class = "tdtext" align = "left">
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
             '----------------------------------- 
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
	</asp:Content>
