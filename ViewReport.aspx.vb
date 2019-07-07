Imports System.Data.SqlClient
Imports System.Data
Partial Class ViewReport
    Inherits System.Web.UI.Page
    Public sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Public sqlcmd As New SqlCommand
    Public drAsset As SqlDataReader
    Public sqlad As New SqlDataAdapter
    Public dt As DataTable
    Public ds As DataSet
    Public sql As String
    Public RType As String = ""
    Public AType As String = ""
    Public RValue As String = ""
    Public RMode As String = ""
    Public CatId As String = ""
    Public FormulaId As String = ""
    Public gvNewPageIndex As Integer
    Public gvEditIndex As Integer
    Public gvUniqueID As String
    Public gvSortExpr, statuss As String
    Public TempOrderBy As String = ""
    Public V_Emp_Bool As Boolean = False
    Public V_Trans_Bool As Boolean = False
    Public SqlStr As String = ""
    Public condition As String = ""
    Public assettypes As String = ""
    Public orderby As String = ""
    Public ReportHeaders As String = ""
    Public ReportHeaderDesc As String = ""
    Public usercondition As String = ""
    Public reportname As String = ""
    Public cntrh As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        FormulaId = Request.QueryString("Fid")
        If Page.IsPostBack = False Then
            If sqlcon.State = Data.ConnectionState.Open Then
                sqlcon.Close()
            End If
            sqlcon.Open()
            sqlcmd = New SqlCommand
            sqlcmd.Connection = sqlcon
            sqlcmd.CommandType = Data.CommandType.Text
            sqlcmd.CommandText = "select * from tbl_Asset_reportformula where id =" & FormulaId & ""
            drAsset = sqlcmd.ExecuteReader
            If drAsset.HasRows Then
                While drAsset.Read
                    ReportHeaders = Convert.ToString(drAsset("ReportHeaders"))
                    condition = Convert.ToString(drAsset("usercondition"))
                    assettypes = Convert.ToString(drAsset("assettypes"))
                    orderby = Convert.ToString(drAsset("groupby"))
                    ReportHeaderDesc = Convert.ToString(drAsset("reportheaderdesc"))
                    reportname = Convert.ToString(drAsset("reportname"))
                    usercondition = Convert.ToString(drAsset("condition"))
                    lblreportname.Text = Convert.ToString(drAsset("reportname"))
                    lblcondition.Text = Convert.ToString(drAsset("condition"))
                    'If ReportHeaders.Contains("v_emp.BuildingUnit") Then
                    '    ReportHeaders = ReportHeaders.Replace("v_emp.BuildingUnit", "v_emp.BuildingUnit + '-' + v_emp.seatno")
                    'End If
                    'If ReportHeaders.Contains("v_emp.Emp_Name") Then
                    '    ReportHeaders = ReportHeaders.Replace("v_emp.Emp_Name", "v_emp.Emp_Name + ' ' + v_emp.Emp_Initial")
                    'End If
                    ViewState("status") = Convert.ToString(drAsset("status"))
                End While
            End If
            sqlcmd.Dispose()
            drAsset.Close()
            sqlcon.Close()
        End If
    End Sub
    'Public Function LoadGridView(ByVal AType As String, ByVal RValue As String, ByVal RMode As String)
    '    Dim s As String
    '    If RMode <> "" Then
    '        dt = New DataTable
    '        dt.Clear()
    '        sql = "Select  "
    '        If sqlcon.State = Data.ConnectionState.Open Then
    '            sqlcon.Close()
    '        End If
    '        sqlcon.Open()
    '        sqlcmd = New SqlCommand
    '        sqlcmd.Connection = sqlcon
    '        sqlcmd.CommandType = Data.CommandType.Text

    '        '  sqlcmd.CommandText = "select * from tbl_Asset_Attributes where CatId=" & catid & " order by attid"

    '        sqlcmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where  aad.attid = aa.attid  and " & _
    '                            "aad.AssetTypeId = '" & AType & "' order by aad.attid asc"
    '        drAsset = sqlcmd.ExecuteReader
    '        If drAsset.HasRows Then
    '            While drAsset.Read
    '                If drAsset("Header") = "2" Then
    '                    s = "am." & drAsset("FieldOrder")
    '                    sql = sql & "am." & drAsset("FieldOrder") & " as [" & drAsset("AttDesc") & "],"
    '                Else
    '                    sql = sql & "am." & drAsset("FieldOrder") & " as [" & drAsset("AttDesc") & "],"
    '                End If
    '            End While
    '            If RMode = "Day(s)" Then
    '                s = "DATEDIFF(day,convert(datetime," & s & "),getdate())"
    '            ElseIf RMode = "Month(s)" Then
    '                s = "DATEDIFF(month,convert(datetime," & s & "),getdate())"
    '            ElseIf RMode = "Year(s)" Then
    '                s = "DATEDIFF(year,convert(datetime," & s & "),getdate())"
    '            Else
    '            End If
    '        End If

    '        sqlcmd.Dispose()
    '        drAsset.Close()
    '        sqlcon.Close()
    '        If sql.Contains(",") Then
    '            sql = Left(sql, Len(sql) - 1)
    '        End If


    '        If sql <> "" Then
    '            If sqlcon.State = Data.ConnectionState.Open Then
    '                sqlcon.Close()
    '            End If
    '            sqlcon.Open()
    '            If s <> "" Then
    '                sql = sql & " ," & s & " as Age, am.id from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid and ast.status ='S' " & _
    '                        "and " & s & ">=" & RValue & " and am.AssetTypeid =" & AType
    '            Else
    '                sql = sql & " am.id from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid and ast.status ='S' " & _
    '                                          " and am.AssetTypeid =" & AType
    '            End If
    '            sqlad = New SqlDataAdapter(sql, sqlcon)
    '            dt = New DataTable
    '            'ds = New DataSet()
    '            sqlad.Fill(dt)

    '            If dt.Rows.Count > 0 Then
    '                '  grdreports.Columns.Clear()
    '                grdreports.DataSource = dt
    '                'Dim serialno As New BoundField
    '                'serialno.HeaderText = "S. No"
    '                'grdreports.Columns.Add(serialno)
    '                For j As Integer = 0 To dt.Columns.Count - 18
    '                    Dim s1 As New BoundField
    '                    s1.DataField = dt.Columns(j).ToString
    '                    s1.HeaderText = dt.Columns(j).ToString
    '                    grdreports.Columns.Add(s1)
    '                Next
    '                grdreports.DataBind()
    '            Else
    '                grdreports.EmptyDataText = "No data Found"
    '                grdreports.DataBind()
    '            End If
    '        End If
    '    Else
    '        dt = New DataTable
    '        dt.Clear()

    '        sql = "SELECT usr.Emp_Number as Number,usr.Emp_Name as Name,usr.Dep_Name as Department,COUNT(distinct aas.assetid) as AssetCount,am.AssetTypeid FROM tbl_Asset_Master am, tbl_Asset_Transactions aat,tbl_Asset_Status aas, " & _
    '                " IDPEAPP.dbo.View_SIP_ContractAllEmployees usr WHERE aas.assetid = aat.assetid AND am.id = aas.assetid AND am.assettypeid = " & AType & " AND aat.userid = usr.Emp_Number AND" & _
    '                " aas.status = 'U' AND aat.transtype = 2 AND aat.id IN (SELECT max(id) FROM tbl_Asset_Transactions GROUP BY assetid)" & _
    '                " GROUP BY am.AssetTypeid,usr.Emp_Number,usr.Dep_Name,usr.Emp_Name HAVING COUNT(DISTINCT aas.assetid) > " & RValue & ""
    '        'Response.Write(sql)
    '        sqlad = New SqlDataAdapter(sql, sqlcon)
    '        dt = New DataTable
    '        sqlad.Fill(dt)
    '        If dt.Rows.Count > 0 Then
    '            '   GridView1.Columns.Clear()
    '            GridView1.DataSource = dt
    '            'Dim serialno As New BoundField
    '            'serialno.HeaderText = "S. No"
    '            'GridView1.Columns.Add(serialno)
    '            For j As Integer = 0 To dt.Columns.Count - 2
    '                Dim s1 As New BoundField
    '                s1.DataField = dt.Columns(j).ToString
    '                s1.HeaderText = dt.Columns(j).ToString
    '                GridView1.Columns.Add(s1)
    '            Next
    '            GridView1.DataBind()
    '        Else
    '            GridView1.EmptyDataText = "No data Found"
    '            GridView1.DataBind()
    '        End If
    '    End If
    'End Function

    'Public Function LoadGridView1(ByVal Catid As String, ByVal Formulaid As String, ByVal RMode As String)
    '    Dim s As String
    '    dt = New DataTable
    '    dt.Clear()
    '    sql = "Select  "
    '    If sqlcon.State = Data.ConnectionState.Open Then
    '        sqlcon.Close()
    '    End If
    '    sqlcon.Open()
    '    sqlcmd = New SqlCommand
    '    sqlcmd.Connection = sqlcon
    '    sqlcmd.CommandType = Data.CommandType.Text

    '    sqlcmd.CommandText = "select * from tbl_Asset_reportheaders rh,  , aa where  rh.attid = aa.attid  and " & _
    '                        "aa.catid = '" & Catid & "' and rh.rptformulaid =" & Formulaid & " order by aa.attid asc"
    '    drAsset = sqlcmd.ExecuteReader
    '    If drAsset.HasRows Then
    '        While drAsset.Read
    '            sql = sql & "am." & drAsset("FieldOrder") & " as [" & drAsset("AttDesc") & "],"
    '        End While
    '    End If
    '    'Response.Write(sql)
    '    'Response.End()
    '    sqlcmd.Dispose()
    '    drAsset.Close()
    '    sqlcon.Close()
    '    'If sql.Contains(",") Then
    '    '    sql = Left(sql, Len(sql) - 1)
    '    'End If



    '    If sqlcon.State = Data.ConnectionState.Open Then
    '        sqlcon.Close()
    '    End If
    '    sqlcon.Open()
    '    sqlcmd = New SqlCommand
    '    sqlcmd.Connection = sqlcon
    '    sqlcmd.CommandType = Data.CommandType.Text
    '    Dim condition As String = ""
    '    Dim assettypes As String = ""
    '    Dim orderby As String = ""
    '    sqlcmd.CommandText = "select * from tbl_Asset_reportformula  where  id =" & Formulaid & ""
    '    drAsset = sqlcmd.ExecuteReader
    '    If drAsset.HasRows Then
    '        While drAsset.Read
    '            condition = Convert.ToString(drAsset("usercondition"))
    '            assettypes = Convert.ToString(drAsset("assettypes"))
    '            orderby = Convert.ToString(drAsset("groupby"))
    '        End While
    '    End If
    '    sqlcmd.Dispose()
    '    drAsset.Close()

    '    If sql <> "" Then
    '        If sqlcon.State = Data.ConnectionState.Open Then
    '            sqlcon.Close()
    '        End If
    '        sqlcon.Open()
    '        If Not String.IsNullOrEmpty(condition) Then
    '            If condition.Contains("Emp_Name") = True Or condition.Contains("BuildingUnit") = True Or condition.Contains("Emp_Dept") = True Or condition.Contains("status") Then
    '                '        sql = sql & " am.id from tbl_Asset_Master am,view_SIP_Employees where am.AssetTypeid " & assettypes & " and " & condition & "  " & orderby
    '                sql = sql & " am.id from tbl_Asset_Master am,tbl_Asset_Status where am.AssetTypeid " & assettypes & " and " & condition & "  " & orderby
    '            ElseIf condition.Contains("status") = True Then
    '                sql = sql & " am.id from tbl_Asset_Master am where am.AssetTypeid " & assettypes & " and " & condition & "  " & orderby
    '            Else
    '                sql = sql & " am.id from tbl_Asset_Master am where am.AssetTypeid " & assettypes & " and " & condition & "  " & orderby
    '            End If
    '        Else
    '            sql = sql & " am.id from tbl_Asset_Master am where am.AssetTypeid " & assettypes & " "
    '        End If
    '        sqlad = New SqlDataAdapter(sql, sqlcon)
    '        dt = New DataTable


    '        sqlad.Fill(dt)
    '        If dt.Rows.Count > 0 Then
    '            'grdreports.Columns.Clear()
    '            grdreports.DataSource = dt
    '            For j As Integer = 0 To dt.Columns.Count - 2
    '                Dim s1 As New BoundField
    '                s1.DataField = dt.Columns(j).ToString
    '                s1.HeaderText = dt.Columns(j).ToString
    '                grdreports.Columns.Add(s1)
    '            Next
    '            grdreports.DataBind()
    '        Else
    '            grdreports.EmptyDataText = "No data Found"
    '            grdreports.DataBind()
    '        End If
    '    End If
    'End Function
    Protected Sub lnkback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkback.Click
        Response.Redirect("Reports.aspx?Rpt=" & Request.QueryString("Rpt"))
    End Sub

    Protected Sub grdreports_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdreports.RowDataBound
        'Dim row As GridViewRow = e.Row
        'Dim strSort As String = String.Empty
        'If row.RowType = DataControlRowType.DataRow Then
        '    If row.DataItem Is Nothing Then
        '        Return
        '    End If
        '    'Find Child GridView control
        '    Dim gv As New GridView()
        '    gv = DirectCast(row.FindControl("grdreports1"), GridView)

        '    'Check if any additional conditions (Paging, Sorting, Editing, etc) to be applied on child GridView
        '    If gv.UniqueID = gvUniqueID Then
        '        gv.PageIndex = gvNewPageIndex
        '        gv.EditIndex = gvEditIndex
        '        'Check if Sorting used

        '        'Expand the Child grid
        '        'Page.ClientScript.RegisterStartupScript([GetType](), "collapse", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + DirectCast(e.Row.DataItem, Data.DataRowView)("Company").ToString() & "','one');</script>")
        '    End If

        '    'Prepare the query for Child GridView by passing the Company of the parent row
        '    gv.DataSource = ChildDataSource3(DirectCast(e.Row.DataItem, Data.DataRowView)("AssetTypeid").ToString())
        '    gv.DataBind()
        'End If
    End Sub
    Private Function ChildDataSource3(ByVal ASSET As String) As SqlDataSource
        Dim strQRY As String = ""
        Dim dsTemp As New SqlDataSource
        dsTemp.ConnectionString = ConfigurationManager.ConnectionStrings("ims").ConnectionString.ToString

        dsTemp.SelectCommand = "select count(s.assetid) as COUNTOFASSETATT,s.status,case when status = 'N' then 'New'" & _
                                " when status = 'S' then 'Spare'" & _
                                " when status = 'M' then 'With User(Idle)'" & _
                                "when status = 'R' then 'Repair(Inhouse)'  when status = 'O' then 'Repair(Outside)' " & _
                               " when status = 'U' then 'With User'  when status = 'D' then 'UnDeployed' " & _
                               " when status = 'X' then 'Sold'  when status = 'E' then 'Expired' end as statusdes, " & _
                               " atm.assettypecode as asset from tbl_asset_status s,tbl_asset_master am,tbl_Asset_TypeMaster atm " & _
                               " where am.assettypeid = '" & ASSET & "' and s.assetid = am.id and atm.assettypeid = am.assettypeid group by status,atm.assettypecode"
        Return dsTemp
    End Function
    Private Sub BuildTemplateColumnsDynamically()
        Dim fname As New BoundField()
        fname.DataField = "au_fname"
        fname.HeaderText = "Author FirstName"
        Dim lname As New TemplateField()
        lname.HeaderText = "Author LastName"
        lname.ItemTemplate = New AddTemplateToGridView(ListItemType.Item, "au_lname")
        GridView1.Columns.Add(lname)
        GridView1.Columns.Add(fname)
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        'Dim row As GridViewRow = e.Row
        '' Make sure we aren't in header/footer rows
        'If row.DataItem Is Nothing Then
        '    Return
        'End If

        ''Find Child GridView control
        'Dim gv As New GridView()
        'gv = DirectCast(row.FindControl("GridView2"), GridView)

        ''Check if any additional conditions (Paging, Sorting, Editing, etc) to be applied on child GridView
        'If gv.UniqueID = gvUniqueID Then
        '    gv.PageIndex = gvNewPageIndex
        '    gv.EditIndex = gvEditIndex
        '    'Check if Sorting used

        '    'Expand the Child grid
        '    ClientScript.RegisterStartupScript([GetType](), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + DirectCast(e.Row.DataItem, Data.DataRowView)("name").ToString() & "','one');</script>")
        'End If

        ''Prepare the query for Child GridView by passing the Company of the parent row
        'gv.DataSource = ChildDataSource3(DirectCast(e.Row.DataItem, Data.DataRowView)("assettypeid").ToString())
        'gv.DataBind()
    End Sub
End Class
Public Class AddTemplateToGridView
    Implements ITemplate
    Private _type As ListItemType
    Private _colName As String

    Public Sub New(ByVal type As ListItemType, ByVal colname As String)
        _type = type
        _colName = colname
    End Sub
    Private Sub ITemplate_InstantiateIn(ByVal container As System.Web.UI.Control) Implements ITemplate.InstantiateIn

        Select Case _type
            Case ListItemType.Item
                Dim ht As New HyperLink()
                Dim text1 As New TextBox
                ht.Target = "_blank"
                'ht.DataBinding += New EventHandler(AddressOf ht_DataBinding)
                ht.Attributes.Add("click", "ht_DataBinding()")
                container.Controls.Add(ht)
                Exit Select
        End Select

    End Sub

    Private Sub ht_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As HyperLink = DirectCast(sender, HyperLink)
        Dim container As GridViewRow = DirectCast(lnk.NamingContainer, GridViewRow)
        Dim dataValue As Object = DataBinder.Eval(container.DataItem, _colName)
        If dataValue <> "" Then
            lnk.Text = dataValue.ToString()
            '   lnk.NavigateUrl = "http://www.booktitles.com"
        End If
    End Sub
End Class
