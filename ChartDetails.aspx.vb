Imports System.Data
Imports System.Data.SqlClient

Partial Class ChartDetails
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim str As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Not Request.QueryString("Range").ToString Is Nothing And Not Session("GroupBy") Is Nothing And Not Session("LblCondition") Is Nothing And Not Session("AssetType") Is Nothing Then

                    If Session("ConditionText").ToString = String.Empty Then
                        divTitle.InnerText = "Asset Chart - " & Session("Asset") & ", " & Session("GroupBy") & " wise"
                    Else
                        divTitle.InnerText = "Asset Chart - " & Session("Asset") & ", " & Session("GroupBy") & " wise" & ", " & Session("ConditionText")
                    End If

                    Dim sb As New StringBuilder()
                    Dim dtable As New DataTable()

                    Dim GroupBy As String
                    GroupBy = UCase(Session("GroupBy").ToString)

                    Select Case GroupBy

                        Case UCase("Department")

                            str = "select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status  "
                            str = str & " from view_assetmaster_status v_asset left join view_sip_employees v_emp on v_asset.userid = v_emp.emp_Number where v_emp.emp_status = 'A' and v_emp.Dep_Name <> '' "
                            str = str & " and v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & " and v_asset.status <>'Sold' and v_emp.dep_Name = '" & Request.QueryString("Range").ToString & "' "

                        Case UCase("Building")

                            str = "select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status  "
                            str = str & " from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and v_asset.status <>'Sold' "
                            str = str & " and v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & "  and v_asset.buildingunit <> '' and v_asset.buildingunit = '" & Request.QueryString("Range").ToString & "' "

                        Case UCase("Age")

                            Dim AgeRange As String
                            AgeRange = Request.QueryString("Range").ToString

                            Select Case AgeRange
                                Case "1 Year"
                                    str = " select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status "
                                    str = str & " from view_assetmaster_status v_asset, tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid "
                                    str = str & " and  v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & "  "
                                    str = str & " and convert(datetime,v_asset.att5) >= DATEADD(year,-1,getdate()) and v_asset.status <>'Sold' "

                                Case "1-2 Years"

                                    str = str & " select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status "
                                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid "
                                    str = str & " and  v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & " "
                                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-2,getdate()) and DATEADD(year,-1,getdate()) and v_asset.status <>'Sold' "

                                Case "2-3 Years"
                                    str = str & " select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status "
                                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid "
                                    str = str & " and  v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & " "
                                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-2,getdate()) and DATEADD(year,-3,getdate()) and v_asset.status <>'Sold' "

                                Case "3-4 Years"
                                    str = str & " select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status "
                                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where  v_asset.userid = v_emp.emp_number and v_asset.catid = cat.catid "
                                    str = str & " and  v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & " "
                                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-4,getdate()) and DATEADD(year,-3,getdate()) and v_asset.status <>'Sold' "

                                Case "4-5 Years"
                                    str = str & " select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status "
                                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid "
                                    str = str & " and  v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & " "
                                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-5,getdate()) and DATEADD(year,-4,getdate()) and v_asset.status <>'Sold' "

                                Case "5-6 Years"
                                    str = str & " select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status "
                                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid "
                                    str = str & " and  v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & " "
                                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-6,getdate()) and DATEADD(year,-5,getdate()) and v_asset.status <>'Sold' "

                                Case "Above 6 Years"
                                    str = str & " select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status "
                                    str = str & " from view_assetmaster_status v_asset, tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid "
                                    str = str & " and  v_asset.assettypeid in (" & Session("AssetType") & ") and " & Session("LblCondition") & " "
                                    str = str & " and convert(datetime,v_asset.att5) <= DATEADD(year,-6,getdate()) and v_asset.status <>'Sold' "

                            End Select

                        Case UCase("Make")

                            str = "select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status   "
                            str = str & " from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and v_asset.status <> 'Sold' and v_asset.att9 <> '' "
                            str = str & " and v_asset.assettypeid in(" & Session("AssetType") & ") and " & Session("LblCondition") & " and v_asset.att9 = '" & Request.QueryString("Range").ToString & "' "

                        Case UCase("Status")

                            str = "select v_asset.assetid,v_asset.att1,v_asset.att2,v_asset.att3,v_asset.att4,v_asset.Userid,v_emp.Emp_Name,v_emp.Dep_Name,v_asset.status "
                            str = str & " from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and v_asset.status <> 'Sold' and v_asset.status <> '' "
                            str = str & " and v_asset.assettypeid in(" & Session("AssetType") & ") and " & Session("LblCondition") & " and v_asset.status='" & Request.QueryString("Range").ToString & "'"

                    End Select

                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    Dim oAdapter As New SqlDataAdapter(str, con)
                    oAdapter.Fill(dtable)
                    If dtable.Rows.Count > 0 Then
                        Dim SlNo As Integer = 0
                        With sb
                            .Append("<table class='mGrid' border='0' style='border-collapse:collapse;'>")
                            .Append("<tr>")
                            .Append("<th>Sl.No</th>")
                            .Append("<th>Att 1</th>")
                            .Append("<th>Att 2</th>")
                            .Append("<th>Att 3</th>")
                            .Append("<th>Att 4</th>")
                            .Append("<th>Employee ID</th>")
                            .Append("<th>Name</th>")
                            .Append("<th>Department</th>")
                            .Append("<th>Status</th>")
                            .Append("</tr>")
                            For Each dr As DataRow In dtable.Rows
                                SlNo = SlNo + 1
                                .Append("<tr>")
                                .Append("<td align='center'>").Append(SlNo).Append("</td>")
                                .Append("<td align='center'>").Append(dr.Item("att1")).Append("</td>")
                                .Append("<td align='center'>").Append(dr.Item("att2")).Append("</td>")
                                .Append("<td align='center'>").Append(dr.Item("att3")).Append("</td>")
                                .Append("<td nowrap align='center'>").Append(dr.Item("att4")).Append("</td>")

                                If dr.Item("Userid") = "0000" Then
                                    .Append("<td align='center'>-</td>")
                                    .Append("<td align='center'>-</td>")
                                Else
                                    .Append("<td align='center'>").Append(dr.Item("Userid")).Append("</td>")
                                    .Append("<td align='center'>").Append(dr.Item("Emp_Name")).Append("</td>")
                                End If

                                .Append("<td align='center'>").Append(dr.Item("Dep_Name")).Append("</td>")
                                .Append("<td nowrap align='center'>").Append(dr.Item("status")).Append("</td>")
                                .Append("</tr>")
                            Next
                            .Append("</table>")
                        End With
                    End If
                    divGrid.InnerHtml = sb.ToString()
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
End Class
