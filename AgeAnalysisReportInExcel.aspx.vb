Imports System.IO
Partial Class AgeAnalysisReportInExcel
    Inherits System.Web.UI.Page
    Public RType As String = ""
    Public AType As String = ""
    Public RValue As String = ""
    Public RMode As String = ""
    Dim sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim sqlcmd As New SqlCommand
    Dim drAsset As SqlDataReader
    Dim sqlad As New SqlDataAdapter
    Dim dt As DataTable
    Dim ds As DataSet
    Dim sql As String
    Dim orderdatefield As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AType = Request.QueryString("AType")
        RValue = Request.QueryString("RValue")
        RMode = Request.QueryString("RMode")
        If RValue IsNot Nothing AndAlso AType IsNot Nothing Then
            LoadGridView(AType, RValue, RMode)
        End If
        Dim attachment As String = "attachment; filename=Export.xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        ' Create a form to contain the grid
        Dim frm As New HtmlForm()
        grditems.Parent.Controls.Add(frm)
        frm.Attributes("runat") = "server"
        frm.Controls.Add(grditems)
        frm.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.End()
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
                    '   GridView1.Columns.Clear()
                    grditems.DataSource = dt
                    For j As Integer = 0 To dt.Columns.Count - 2
                        Dim s1 As New BoundField
                        s1.DataField = dt.Columns(j).ToString
                        s1.HeaderText = dt.Columns(j).ToString
                        grditems.Columns.Add(s1)
                    Next
                    grditems.DataBind()
                Else
                    grditems.EmptyDataText = "No data Found"
                    grditems.DataBind()
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
                grditems.DataSource = dt
                For j As Integer = 0 To dt.Columns.Count - 2
                    Dim s1 As New BoundField
                    s1.DataField = dt.Columns(j).ToString
                    s1.HeaderText = dt.Columns(j).ToString
                    grditems.Columns.Add(s1)
                Next
                grditems.DataBind()
            Else
                grditems.EmptyDataText = "No data Found"
                grditems.DataBind()
            End If
        End If
    End Function
End Class
