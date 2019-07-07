Imports System.Data
Imports System.Data.SqlClient
Partial Class ExpectReturnAssets
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim rdr1 As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim assettypeid As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("ATId")) And Request.QueryString("St") = Nothing Then
            BindStatusGroup()
        ElseIf Not String.IsNullOrEmpty(Request.QueryString("ATId")) And Not Request.QueryString("St") = Nothing Then

        End If
    End Sub
    Public Function BindStatusGroup()
        Try
            ' Dim RequiredFields As String = GetRequiredFields(assettypeid)
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand("Select  row_number() over (order by am.status) as no,am.status from View_assetmaster_status am where am.status in('With User','Repair(Outside)') group by am.status ", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                Dim strtable As New Table
                strtable.ID = "strtable1"
                strtable.Width = Unit.Percentage(100)
                While rdr.Read
                    Dim strrow As New TableRow
                    Dim strrow1 As New TableRow
                    Dim strCell_1 As New TableCell
                    Dim strCell_2 As New TableCell
                    Dim gridview1 As New GridView
                    Dim lbl As New Label
                    gridview1.ID = "grid1" & rdr("no")
                    lbl.ID = "lbl" & rdr("no")
                    strCell_2.CssClass = "trheaderbg"
                    lbl.Text = rdr("status")
                    strCell_2.Controls.Add(lbl)
                    strrow1.Cells.Add(strCell_2)
                    gridview1.CssClass = "mGrid"
                    gridview1.GridLines = GridLines.None
                    gridview1.AllowPaging = False
                    gridview1.AutoGenerateColumns = False
                    gridview1.Width = Unit.Percentage(100)
                    gridview1.AlternatingRowStyle.CssClass = "alt"
                    dtable = New DataTable
                    dtable = LoadDetailsView(rdr("status"), Request.QueryString("ATId"))
                    If dtable.Rows.Count > 0 Then
                        gridview1.Columns.Clear()
                        gridview1.DataSource = dtable
                        For i As Integer = 0 To dtable.Columns.Count - 1
                            Dim s As New BoundField
                            If i = 0 Then
                                s.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                            End If
                            s.DataField = dtable.Columns(i).ToString
                            s.HeaderText = dtable.Columns(i).ToString
                            gridview1.Columns.Add(s)
                        Next
                        gridview1.DataBind()
                        strCell_1.Controls.Add(gridview1)
                        strrow.Cells.Add(strCell_1)
                        strtable.Rows.Add(strrow1)
                        strtable.Rows.Add(strrow)
                    End If
                End While
                tddata.Controls.Add(strtable)
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function LoadDetailsView(ByVal assetstatus As String, ByVal assettypeid As String) As DataTable
        Dim sql As String = ""
        If con1.State = ConnectionState.Open Then
            con1.Close()
        End If
        con1.Open()
        sql = "select row_number() over (order by am.status) as [S.No],am.att1 as [Asset No],am.att2 as [Fin Asset No],emp.emp_name + ' ' + emp.Emp_Initial as UserName,convert(varchar,tr.date1,106) as DeployDate,convert(varchar,tr.date2,106) as ReturnDate,am.status,atm.AssetTypeCode as [Asset Type]"
        sql &= " from View_assetmaster_status am join tbl_Asset_Transactions tr on am.id = tr.assetid "
        sql &= " join  tbl_Asset_TypeMaster atm on am.AssetTypeid = atm.AssetTypeId join view_SIP_Employees emp "
        sql &= "  on am.userid = emp.emp_number where tr.transtype in (2,5) and tr.date2<>'' and convert(datetime,tr.date2) >= getdate() and am.status in ('" & assetstatus & "')"
        sql &= " and am.assettypeid = " & assettypeid
        sqladr = New SqlDataAdapter(sql, con1)
        dtable = New DataTable
        sqladr.Fill(dtable)
        Return dtable
        con1.Close()
    End Function
End Class
