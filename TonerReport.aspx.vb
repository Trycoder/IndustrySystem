Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class TonerReport
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim str As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowStatus", "javascript:winwidth();", True)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetConsumableDetails()
    End Sub
    Public Function GetAssetList() As DataTable
        Dim sql As String = "select distinct att11 + ' (' + convert(varchar,count(*)) + ')'  as PrinterModel,att11 from tbl_asset_master where assettypeid = 6 group by att11"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Public Function GetConsumableDetails()
        Try
            dtable = New DataTable
            Dim dtAssetlist As New DataTable
            dtAssetlist = GetAssetList()
            dtable = GenerateTransposedTable(dtAssetlist)
            Dim s As DataRow
            Dim dtablecons As New DataTable
            Dim dtableprinters As New DataTable
            dtablecons = GetConsumableList()
            Dim inc As Integer = 0
            For i As Integer = 0 To dtablecons.Rows.Count - 1
                s = dtable.NewRow
                s(0) = dtablecons.Rows(i)("conmodel") & " (" & GetConsumableTotal(dtablecons.Rows(i)("conmodel")) & ")"
                For j As Integer = 0 To dtAssetlist.Rows.Count - 1
                    inc = j + 1
                    cmd = New SqlCommand
                    con.Open()
                    str = "select * from tbl_Asset_Mapping where assetmodel='" & dtAssetlist.Rows(j)("att11") & "' and conmodel = '" & dtablecons.Rows(i)("conmodel") & "' "
                    cmd.CommandText = str
                    cmd.Connection = con
                    rdr = cmd.ExecuteReader
                    If Not rdr.HasRows Then
                        s(inc) = ""
                    Else
                        s(inc) = "X"
                    End If
                    rdr.Close()
                    con.Close()
                Next
                dtable.Rows.Add(s)
            Next
            Dim dtfinaldtable As New DataTable
            dtfinaldtable = dtable
            If dtfinaldtable.Rows.Count > 0 Then
                grdattributes.Columns.Clear()
                grdattributes.DataSource = dtfinaldtable
                For j As Integer = 0 To dtfinaldtable.Columns.Count - 1
                    Dim bfield As New BoundField
                    bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    bfield.DataField = dtable.Columns(j).ToString
                    bfield.HeaderText = dtable.Columns(j).ToString
                    grdattributes.Columns.Add(bfield)
                Next
                grdattributes.DataBind()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetConsumableList() As DataTable
        Dim sql As String = " select distinct att1 as conmodel from tbl_Asset_cons_stock where constypeid =24 order by att1"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Public Function GetConsumableTotal(ByVal consumablename As String) As String
        Dim sql As String = " select sum(quantity) as quantity from tbl_Asset_cons_stock where att1='" & consumablename & "'"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand(sql, con)
        Dim s As String = cmd.ExecuteScalar
        con.Close()
        Return s
    End Function
    Private Function GenerateTransposedTable(ByVal inputTable As DataTable) As DataTable
        Dim outputTable As New DataTable()
        ' Add columns by looping rows

        ' Header row's first column is same as in inputTable
        outputTable.Columns.Add(inputTable.Columns(0).ColumnName.ToString())

        ' Header row's second column onwards, 'inputTable's first column taken
        For Each inRow As DataRow In inputTable.Rows
            Dim newColName As String = inRow(0).ToString()
            outputTable.Columns.Add(newColName)
        Next

        ' Add rows by looping columns        
        For rCount As Integer = 2 To inputTable.Columns.Count - 1
            Dim newRow As DataRow = outputTable.NewRow()

            ' First column is inputTable's Header row's second column
            newRow(0) = inputTable.Columns(rCount).ColumnName.ToString()
            For cCount As Integer = 0 To inputTable.Rows.Count - 1
                Dim colValue As String = inputTable.Rows(cCount)(rCount).ToString()
                newRow(cCount + 1) = colValue
            Next
            outputTable.Rows.Add(newRow)
        Next
        Return outputTable
    End Function

    Protected Sub grdattributes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdattributes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).CssClass = "locked"
            For i As Integer = 1 To e.Row.Cells.Count - 1
                If e.Row.Cells(i).Text.Trim <> "&nbsp;" Then
                    e.Row.Cells(i).Attributes.Add("Style", "font-family: wingdings; font-size: 200%;")
                    e.Row.Cells(i).Text = ""
                    e.Row.Cells(i).Text = "&#252;"
                End If
            Next
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            ' grdattributes.HeaderRow.Cells(0).CssClass = "locked"
        End If

    End Sub

    Protected Sub lnkexport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkexport.Click
        'Response.Clear()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", "attachment;filename=g.xls")
        'Response.Charset = ""
        'Response.ContentType = "application/vnd.ms-excel"
        'Dim sw As New StringWriter()
        'Dim ht As New HtmlTextWriter(sw)
        'grdattributes.AllowPaging = False
        'grdattributes.DataBind()
        'grdattributes.RenderControl(ht)
        'Response.Output.Write(sw.ToString())
        'Response.Flush()
        'Response.[End]()
        Dim attachment As String = "attachment; filename=Export.xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        ' Create a form to contain the grid
        Dim frm As New HtmlForm()
        grdattributes.Parent.Controls.Add(frm)
        frm.Attributes("runat") = "server"
        frm.Controls.Add(grdattributes)
        frm.RenderControl(htw)
        'GridView1.RenderControl(htw);
        Response.Write(sw.ToString())
        Response.End()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Private Function j() As Integer
        Throw New NotImplementedException
    End Function

End Class
