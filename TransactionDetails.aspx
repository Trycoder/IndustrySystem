<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="TransactionDetails.aspx.vb" Inherits="TransactionDetails" title="Transaction Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="100%">
        <%
        Dim i As Integer
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim sqldr As System.Data.SqlClient.SqlDataReader
            Dim sql As String = ""
        Dim strTransType As String
        strTransType = "Purchase,Maintenance,Deployment,UnDeploy,Repair(Inhouse),Repair(Outside),Return,Retired,Sales"
        Dim arr()
        arr = strTransType.Split(",")        
        %>
        <tr>
            <td align="right">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Default.aspx" Font-Size="Small" Font-Bold="true">Back</asp:HyperLink>
            </td>                        
        </tr>
        <tr class="tdcolbg">
            <td>
                <table align="center" cellpadding="4" cellspacing="1"  width="100%">
                    <tr class="trheaderbg">
                        <td align="center">S.No</td>
                        <td align="center">Asset No</td>
                        <td align="center">Transaction Date</td>
                        <%If Request("al") = 1 Then%>
                        <td align="center">Warranty Start Date</td>
                        <td align="center">Warranty End Date</td>
                        <%end if %>
                        <td align="center">Transaction Description</td>
                        <td align="center">Remark</td>                        
                        <td align="center">UserName</td>
                       
                    </tr>                    
                    <%  If Not Request("tt") = Nothing And Request("al") = 0 Then
                            'sql = "SELECT am.id,am.assettypeid,am.att1 as AssetNo,T.Date1,T.remarks,M.activity AS Act,emp.Emp_Name + ' ' + emp.Emp_Initial as Emp_Name FROM tbl_asset_transactions T,tbl_Asset_Maintainance M,view_SIP_Employees emp,view_assetmaster_status am " & _
                            '   " WHERE M.id = T.reasonid and am.userid = emp.Emp_Number and t.assetid = am.id AND T.transtype = '" & Request("tt") & "'" & _
                            '   " AND T.Date1 = '" & Format(Date.Now.Date.AddDays(-1), "yyyy-MM-dd") & "' order by am.att1"
                            sql = "SELECT am.id,am.assettypeid,am.att1 as AssetNo,T.Date1,T.remarks,M.activity AS Act,emp.Emp_Name + ' ' + emp.Emp_Initial as Emp_Name FROM tbl_asset_transactions T,tbl_Asset_Maintainance M,view_SIP_Employees emp,view_assetmaster_status am " & _
                               " WHERE M.id = T.reasonid and am.userid = emp.Emp_Number and t.assetid = am.id AND T.transtype = '" & Request("tt") & "'" & _
                               " AND T.Date1 = '" & Format(Date.Now.Date, "yyyy-MM-dd") & "' order by am.att1"
                             ElseIf Request("al") = 1 Then
                            sql = "SELECT am.id,am.assettypeid,am.att1 as AssetNo,emp.Emp_Name + ' ' + emp.Emp_Initial as Emp_Name,T.Date1,T.Date2,T.Date3,T.remarks,M.activity AS Act FROM tbl_asset_transactions T,tbl_Asset_Maintainance M,View_assetmaster_status am,view_SIP_Employees emp " & _
                                " WHERE am.userid = emp.Emp_Number and t.assetid = am.id and M.id = T.reasonid AND am.assettypeid = '" & Request("Aid") & "' and date3 <> '' and date3 =convert(varchar,'" & Request.QueryString("EDate") & "',103) order by am.att1"
                        End If
                        
                        'Response.Write(sql)
                        'Response.End()
                        con.Open()
                        cmd.Connection = con
                        cmd.CommandType = Data.CommandType.Text
                        cmd.CommandText = sql
                        sqldr = cmd.ExecuteReader
                        Dim m As Integer = 1
                        While sqldr.Read
                        %>
                        <tr class="whitebg">
                        <td class="tdtext" align="center"><%=m%></td>
                        <% m = m + 1%>
                            <td class="tdtext">
                         <a href="" onclick="window.open('ReportInfo.aspx?assetid=<%=sqldr("id")%>&AssetTypeId=<%=sqldr("assettypeid")%>','ReportInfo','width=750,height=750,location=no,scrollbars=yes,left=200,top=100,screenX=0,screenY=100,resizable=1');return false;">
                        <%=sqldr("AssetNo")%>
                         </a>    
                         </td>                
                         <td class="tdtext"><%=Format(sqldr("Date1"), "dd-MMM-yyyy")%></td>
                            <%If Request("al") = 1 Then%>
                            <td class="tdtext"><%=Format(sqldr("Date2"), "dd-MMM-yyyy")%></td>
                            <td class="tdtext"><%=Format(sqldr("Date3"), "dd-MMM-yyyy")%></td>
                            <%end if %>
                            <td class="tdtext"><%=sqldr("Act")%></td>
                            <td class="tdtext"><%=sqldr("remarks")%></td>
                            <td class="tdtext"><%=sqldr("Emp_Name")%></td>
                       </tr>
                   <%
                    End While
                    sqldr.Close()
                    con.Close()%>
                </table>
            </td>
        </tr>
        <tr><td></td></tr>
    </table>
</asp:Content>

