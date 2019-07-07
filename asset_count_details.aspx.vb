Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Partial Class asset_count_details
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim rdr1 As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim assetstatus As String = ""
    Dim assettypeid As String = ""
    Dim assettypecode As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        assetstatus = getassetstatusdescription(Request.QueryString("status"))
        assettypeid = Request.QueryString("assettypeid")
        assettypecode = Request.QueryString("assettypecode")
        If Request.QueryString("TType") IsNot Nothing Then
            If String.IsNullOrEmpty(assetstatus) And Not String.IsNullOrEmpty(assettypeid) Then
                BindStatusGroup()
            End If
        Else
            If String.IsNullOrEmpty(assetstatus) Then
                lblmsg.Text = assettypecode & "  Details"
            Else
                lblmsg.Text = assettypecode & " (" & assetstatus & ") Details"
            End If
            If Not String.IsNullOrEmpty(assetstatus) And Not String.IsNullOrEmpty(assettypeid) Then
                BindGrid()
            ElseIf String.IsNullOrEmpty(assetstatus) And Not String.IsNullOrEmpty(assettypeid) Then
                BindStatusGroup()
            End If
        End If
    End Sub
    Public Function BindGrid()
        Try
            dtable = New DataTable
            dtable = LoadDetailsView(GetRequiredFields(assettypeid), assetstatus, assettypeid)
            If dtable.Rows.Count > 0 Then
                grdassets.Columns.Clear()
                grdassets.DataSource = dtable
                For j As Integer = 0 To dtable.Columns.Count - 1
                    Dim s As New BoundField
                    If j = 0 Then
                        s.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    End If
                    s.DataField = dtable.Columns(j).ToString
                    s.HeaderText = dtable.Columns(j).ToString
                    grdassets.Columns.Add(s)
                Next
                grdassets.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function LoadDetailsView(ByVal sql As String, ByVal assetstatus As String, ByVal assettypeid As String) As DataTable
        If con1.State = ConnectionState.Open Then
            con1.Close()
        End If
        con1.Open()
        If sql <> "" Then
            sql = sql & ", (select emp_name+' '+emp_initial + '(' + emp_Number + ')'  from view_SIP_Employees where emp_number = v_asset.userid) as UserName "
            sql = sql & ", (select Dep_name  from view_SIP_Employees where emp_number = v_asset.userid) as Department "
        End If
        sql = sql & "  from View_assetmaster_status v_asset where v_asset.status='" & assetstatus & "' and v_asset.assettypeid='" & assettypeid & "' order by v_asset.id ASC "
        sqladr = New SqlDataAdapter(sql, con1)
        dtable = New DataTable
        sqladr.Fill(dtable)
        Return dtable
        con1.Close()
    End Function
    Public Function GetRequiredFields(ByVal assettypeid As String) As String
        Try
            Dim sql As String
            sql = "Select  row_number() over (order by v_asset.id) as [S.No],"
            If con1.State = ConnectionState.Open Then
                con1.Close()
            End If
            con1.Open()
            cmd1 = New SqlCommand("select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aa.IsRequired = 1 and aad.attid = aa.attid  and aad.AssetTypeId = " & assettypeid & " order by aad.attid asc", con1)
            rdr1 = cmd1.ExecuteReader
            If rdr1.HasRows Then
                While rdr1.Read
                    sql = sql & "v_asset." & rdr1("FieldOrder") & " as [" & rdr1("AttDesc") & "],"
                End While
            Else
                sql = sql & "* "
            End If
            cmd1.Dispose()
            rdr1.Close()
            con1.Close()
            sql = Left(sql, Len(sql) - 1)
            Return sql
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function BindStatusGroup()
        Try
            ' Dim RequiredFields As String = GetRequiredFields(assettypeid)
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand("Select  row_number() over (order by v_asset.status) as no,v_asset.status from View_assetmaster_status v_asset where v_asset.assettypeid = '" & assettypeid & "' and v_asset.status <>'' group by v_asset.status ", con)
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
                    dtable = LoadDetailsView(GetRequiredFields(assettypeid), rdr("status"), assettypeid)
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
    Public Function getassetstatusdescription(ByVal assetstatus As String) As String
        Try
            If assetstatus = "N" Then
                Return "New"
            ElseIf assetstatus = "S" Then
                Return "Spare"
            ElseIf assetstatus = "U" Then
                Return "With User"
            ElseIf assetstatus = "M" Then
                Return "With User(Idle)"
            ElseIf assetstatus = "R" Then
                Return "Repair(Inhouse)"
            ElseIf assetstatus = "E" Then
                Return "Expired"
            ElseIf assetstatus = "X" Then
                Return "Sold"
            ElseIf assetstatus = "O" Then
                Return "Repair(Outside)"
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Private Function LinkButton() As Object
        Throw New NotImplementedException
    End Function

End Class
