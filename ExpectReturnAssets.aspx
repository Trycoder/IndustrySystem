<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="ExpectReturnAssets.aspx.vb" Inherits="ExpectReturnAssets"  %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Expected Return Assets</title>
 <link rel="stylesheet" type="text/css" href="css/ApplyCSS1.css"/> 
  <link rel="stylesheet" type="text/css" href="Css/grid.css"/> 
 <link rel="Stylesheet" type="text/css" href="css/tabs.css" />
  <script language="javascript" type="text/javascript">
        function openReportInfo()
        {
            var mywin = window.open("ReportInfo.aspx", "mywin", "location=1,status=1,scrollbars=1, width=100,height=100,resizable=1");
        }
    </script>
  </head>
<body style="margin-top:0; margin-left:0;">
  <form id="form1" runat="server">
  <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="" align="center"></td>
        </tr>
          <% if  Request.QueryString("ATId") <> "" and Request.QueryString("St") <> "" Then  %>
                <tr  class="whitebg">
                <td  align="center">
               <div id="Div1" style="height:auto; overflow-x: hidden">
                <table class="mgrid" width="100%">
                <%    Dim count As Integer = 1%>
                    <tr  class="trheaderbg"> 
                       <td align="center" style="width:2%;color:White;font-weight:bold;">S.No</td>
                       <td  align="center" style="width:10%;color:White;font-weight:bold;">Asset No</td>
                        <td  align="center" style="width:5%;color:White;font-weight:bold;">Fin Asset No</td>
                        <% If Request.QueryString("St") = "With User" Then%>
                        <td  align="center" style="width:20%;color:White;font-weight:bold;">User Name</td>
                        <%ElseIf Request.QueryString("St") = "Repair(Outside)" Then%>
                        <td  align="center" style="color:White;font-weight:bold;">Vendor Name</td>
                        <% End If%>
                        <td  align="center" style="color:White;font-weight:bold;">Deploy Date</td>
                        <td  align="center" style="color:White;font-weight:bold;">Return Date</td>
                    </tr>
                   
                      <%  Dim qry As String, assetno, qry1, qry4 As String, qry2 As String, qry3 As String
                            Dim n As Integer = 0
                          Dim j As Integer
                          Dim cmd As New System.Data.SqlClient.SqlCommand
                            Dim rdrReport As System.Data.SqlClient.SqlDataReader
                          Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
                            Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
                            Dim con2 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
                          Dim con3 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
                          Dim con4 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
                          Dim rdrReport4 As System.Data.SqlClient.SqlDataReader
                          Dim cmd3 As New System.Data.SqlClient.SqlCommand
                          Dim cmd4 As New System.Data.SqlClient.SqlCommand
                     %>   
                     
                    <% If Request.QueryString("St") = "With User" Then%>
                            <%
                                qry = "select am.att1,am.att2,emp.emp_name + ' ' + emp.Emp_Initial as UserName,convert(varchar,tr.date1,106) as DeployDate,convert(varchar,tr.date2,106) as ReturnDate, am.assettypeid,am.status,atm.AssetTypeCode "
                                    qry = qry & " from View_assetmaster_status am join tbl_Asset_Transactions tr on am.id = tr.assetid "
                                    qry = qry & " join  tbl_Asset_TypeMaster atm on am.AssetTypeid = atm.AssetTypeId join view_SIP_Employees emp "
                                qry = qry & " on am.userid = emp.emp_number where tr.transtype in (2) and date2<>'' and convert(datetime,date2) >= getdate() and am.status in ('" & Request.QueryString("St") & "') "
                                    qry = qry & " and am.assettypeid = " & Request.QueryString("ATId") & ""
                           %>

                    <% ElseIf Request.QueryString("St") = "Repair(Outside)" Then%>
                                <%
                                    qry = "select am.att1,am.att2,v.vendorname as Username,convert(varchar,tr.date1,106) as DeployDate,convert(varchar,tr.date2,106) as ReturnDate, am.assettypeid,am.status,atm.AssetTypeCode from View_assetmaster_status am join "
                                    qry = qry & " tbl_Asset_Transactions tr on am.id = tr.assetid join  tbl_Asset_TypeMaster atm on am.AssetTypeid = atm.AssetTypeId join  tbl_Asset_Vendor v "
                                    qry = qry & " on tr.vendorid = v.vendorid where tr.transtype in (5) and date2<>'' and convert(datetime,date2) >= getdate() and am.status in ('" & Request.QueryString("St") & "')  "
                                    qry = qry & " and am.assettypeid = " & Request.QueryString("ATId") & ""
                                    %>

                    <% End If%>
                    <%
                        Dim strtest As String = ""
                       con.Open()
                        con4.Open()
                        cmd.Connection = con
                        cmd.CommandText = qry
                        rdrReport = cmd.ExecuteReader
                        While rdrReport.Read
                   %>
                   <%
                   
                       qry4 = " select id,assettypeid from tbl_Asset_Master where att1='" & rdrReport("att1") & "' "
                       cmd4 = New System.Data.SqlClient.SqlCommand(qry4, con4)
                       rdrReport4 = cmd4.ExecuteReader()
                       
                       If (rdrReport4.Read()) Then
                           
                      
                    %>
                   
                    <tr>
                    <td style="width:5%" align="center" class="tdtext"> <% =count%> </td>
                       <td style="width:2%;border-collapse:collapse;" align="center">
                       
                       
                       <a href="" onclick="window.open('ReportInfo.aspx?assetid=<%=rdrReport4("id")%>&assettypeid=<%=rdrReport4("assettypeid")%>','ReportInfo','width=950,height=650,location=no,scrollbars=yes,left=150,top=100,screenX=0,screenY=100,resizable=1');return false;"><%=rdrReport("att1")%></a></td>                                
                      <% End If%>
                      <%
                          If (rdrReport4.IsClosed = False) Then
                              rdrReport4.Close()
                          End If
                      
                       %>
                        <td style="width:5%;" align="center"><%=rdrReport("att2")%></td>
                        <td style="width:15%;" align="left"><%=rdrReport("UserName")%></td>
                        <td style="width:5%;" align="center"><%=rdrReport("DeployDate")%> </td>
                        <td style="width:5%;" align="center"><%=rdrReport("ReturnDate")%></td>
                        <% count = count + 1%>
                     </tr>
                   <%      
                   End While
                   rdrReport4.Close()
                   rdrReport.Close()
                   con.Close()
                   %>
                   </table>
                 </div>
                 </td>
                </tr>
                 <% Else %>   
             <tr  class="whitebg">
                <td  align="center" class="tdtextbold" >
                    <div id="Div2" style="height:auto; overflow-x: hidden">
                    <table width="100%" border="0" style="border-collapse:collapse">
                        <tr>
                            <td runat="server" id="tddata" class="tdtextbold">
                
                </td>
                </tr>
              </table> 
                </div>               
            </td>
             </tr>
                 
                 <% End If %>
                 

             </table>
               
        </form>
</body>
</html> 


<%--
                     <% ElseIf Request.QueryString("ATId") <> "" Then%>
                               <%
                                    qry = "select am.att1,am.att2,v.vendorname as Username,tr.date1,tr.date2, am.assettypeid,am.status,atm.AssetTypeCode from View_assetmaster_status am join "
                                    qry = qry & " tbl_Asset_Transactions tr on am.id = tr.assetid join  tbl_Asset_TypeMaster atm on am.AssetTypeid = atm.AssetTypeId join  tbl_Asset_Vendor v "
                                    qry = qry & " on tr.vendorid = v.vendorid where tr.transtype in (2,5) and date2<>'' and am.status in ('With User','Repair(Outside)')  "
                                    qry = qry & " and am.assettypeid = " & Request.QueryString("ATId") & ""
                                    %>--%>