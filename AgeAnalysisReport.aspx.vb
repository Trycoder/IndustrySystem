
Partial Class AgeAnalysisReport
    Inherits System.Web.UI.Page
    Dim sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim sqlcmd As New SqlCommand
    Dim drAsset As SqlDataReader
    Dim sqlad As New SqlDataAdapter
    Dim dt As DataTable
    Dim ds As DataSet
    Dim sql As String
    Public RType As String = ""
    Public AType As String = ""
    Dim RValue As String = ""
    Dim RMode As String = ""
    Public CatId As String = ""
    Public FormulaId As String = ""
    Dim gvNewPageIndex As Integer
    Dim gvEditIndex As Integer
    Dim gvUniqueID As String
    Dim gvSortExpr, statuss As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        RType = Request.QueryString("RType")
        If UCase(RType) = UCase("Age") Then
            AType = Request.QueryString("AType")
            RValue = Request.QueryString("RValue")
            RMode = Request.QueryString("Rmode")
            LoadGridView(AType, RValue, RMode)
            lblmessage.Text = "Age Analysis Report"
        ElseIf UCase(RType) = UCase("MAssets") Then
            AType = Request.QueryString("AType")
            RValue = Request.QueryString("RValue")
            LoadGridView(AType, RValue, "")
            lblmessage.Text = "Employees Having More Asset"
        End If
    End Sub
    Public Function LoadGridView(ByVal AType As String, ByVal RValue As String, ByVal RMode As String)
        Dim s As String = ""
        Dim a As String = ""
        If RMode <> "" Then
            dt = New DataTable
            dt.Clear()
            sql = "Select  "
            If sqlcon.State = Data.ConnectionState.Open Then
                sqlcon.Close()
            End If
            sqlcon.Open()
            sqlcmd = New SqlCommand
            sqlcmd.Connection = sqlcon
            sqlcmd.CommandType = Data.CommandType.Text
            sqlcmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where  aad.attid = aa.attid  and " & _
                                "aad.AssetTypeId = '" & AType & "' order by aad.attid asc"
            drAsset = sqlcmd.ExecuteReader
            If drAsset.HasRows Then
                While drAsset.Read
                    If drAsset("Header") = "2" Then
                        s = "am." & drAsset("FieldOrder")
                        sql = sql & "am." & drAsset("FieldOrder") & " as [" & drAsset("AttDesc") & "],"
                    Else
                        sql = sql & "am." & drAsset("FieldOrder") & " as [" & drAsset("AttDesc") & "],"
                    End If
                End While
                If RMode = "Day(s)" Then
                    a = "convert(datetime," & s & ") < dateadd(day,-" & RValue & ",getdate())"
                ElseIf RMode = "Month(s)" Then
                    a = "convert(datetime," & s & ") < dateadd(month,-" & RValue & ",getdate())"
                ElseIf RMode = "Year(s)" Then
                    a = "convert(datetime," & s & ") < dateadd(year,-" & RValue & ",getdate())"
                Else
                End If
            End If

            sqlcmd.Dispose()
            drAsset.Close()
            sqlcon.Close()
            If sql.Contains(",") Then
                sql = Left(sql, Len(sql) - 1)
            End If


            If sql <> "" Then
                If sqlcon.State = Data.ConnectionState.Open Then
                    sqlcon.Close()
                End If
                sqlcon.Open()
                If s <> "" Then
                    sql = sql & " ,am.id from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid  " & _
                            "and " & a & " and am.AssetTypeid =" & AType & " order by " & s & " asc "
                Else
                    sql = sql & " am.id from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid  " & _
                                              " and am.AssetTypeid =" & AType & " order by am.id asc"
                End If
                sqlad = New SqlDataAdapter(sql, sqlcon)
                dt = New DataTable
                sqlad.Fill(dt)
                If dt.Rows.Count > 0 Then
                    '  grdreports.Columns.Clear()
                    grdreports.DataSource = dt
                    For j As Integer = 0 To dt.Columns.Count - 18
                        Dim s1 As New BoundField
                        s1.DataField = dt.Columns(j).ToString
                        s1.HeaderText = dt.Columns(j).ToString
                        grdreports.Columns.Add(s1)
                    Next
                    grdreports.DataBind()
                Else
                    grdreports.EmptyDataText = "No data Found"
                    grdreports.DataBind()
                End If
            End If
        Else
            dt = New DataTable
            dt.Clear()

            sql = "SELECT usr.Emp_Number as Number,usr.Emp_Name as Name,usr.Dep_Name as Department,COUNT(distinct aas.assetid) as AssetCount,am.AssetTypeid FROM tbl_Asset_Master am, tbl_Asset_Transactions aat,tbl_Asset_Status aas, " & _
                    " View_SIP_Employees usr WHERE aas.assetid = aat.assetid AND am.id = aas.assetid AND am.assettypeid = " & AType & " AND aat.userid = usr.Emp_Number AND" & _
                    " aas.status = 'U' AND aat.transtype = 2 AND aat.id IN (SELECT max(id) FROM tbl_Asset_Transactions GROUP BY assetid)" & _
                    " GROUP BY am.AssetTypeid,usr.Emp_Number,usr.Dep_Name,usr.Emp_Name HAVING COUNT(DISTINCT aas.assetid) > " & RValue & ""
            'Response.Write(sql)
            sqlad = New SqlDataAdapter(sql, sqlcon)
            dt = New DataTable
            sqlad.Fill(dt)
            If dt.Rows.Count > 0 Then
                '   GridView1.Columns.Clear()
                GridView1.DataSource = dt
                For j As Integer = 0 To dt.Columns.Count - 2
                    Dim s1 As New BoundField
                    s1.DataField = dt.Columns(j).ToString
                    s1.HeaderText = dt.Columns(j).ToString
                    GridView1.Columns.Add(s1)
                Next
                GridView1.DataBind()
            Else
                GridView1.EmptyDataText = "No data Found"
                GridView1.DataBind()
            End If
        End If
    End Function
    Protected Sub lnkback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkback.Click
        Response.Redirect("Reports.aspx?Rpt=A")
    End Sub
End Class

