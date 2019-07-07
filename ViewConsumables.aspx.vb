Imports System.Data
Imports System.Data.SqlClient
Partial Class ViewConsumables
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ConsumableId As String = Request.QueryString(Convert.ToString("CId"))
        Try
            If Not IsPostBack Then
                If ConsumableId <> "" Then
                    ViewState("sortOrder") = ""
                    bindConsumables(ConsumableId, "", "")
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function bindConsumables(ByVal catid As String, ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String
            Dim firstcolumn As Boolean = False
            Dim ordercolumn As String = ""
            sql = "Select "
            con.Open()
            cmd = New SqlCommand
            cmd.Connection = con
            cmd.CommandType = Data.CommandType.Text
            cmd.CommandText = "select aad.*,aa.* from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & catid & " and Header in('0','7') order by aa.attid asc"
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    sql = sql & "acs." & rdr("FieldOrder") & " as [" & rdr("AttDesc") & "],"
                    If firstcolumn = False Then
                        ordercolumn = "acs." & rdr("FieldOrder")
                        firstcolumn = True
                    End If
                End While
            Else
                sql = sql & " "
            End If
            rdr.Close()
            con.Close()
            sql = sql & " mas.loccatname + '-' + TAL.LOCNAME as Location,TAS.sublocname as SubLocation ,acs.Quantity from tbl_Asset_Cons_Stock acs,tbl_Asset_TypeMaster atm , tbl_Asset_location TAL,tbl_Asset_sublocation TAS,tbl_Asset_location_master mas WHERE  tal.loccatid = mas.id and acs.ConsTypeId = atm.assettypeid and acs.ConsTypeId =" & catid & " AND ACS.LOCID=TAL.LOCID AND  acs.SUBLOCID=TAS.SUBLOCID  "

            If ordercolumn <> "" Then
                sql = sql & "order by " & ordercolumn & " asc "
            End If

            cmd.Dispose()
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            dtable = New DataTable
            sqladr = New SqlDataAdapter(sql, con)
            sqladr.Fill(dtable)
            If dtable.Rows.Count > 0 Then
                Dim myDataView1 As New DataView()
                myDataView1 = dtable.DefaultView
                If sortExpression <> String.Empty Then
                    myDataView1.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdconsumables.DataSource = myDataView1
                grdconsumables.Columns.Clear()
                For j As Integer = 0 To dtable.Columns.Count - 1
                    Dim s As New BoundField
                    s.DataField = dtable.Columns(j).ToString
                    s.HeaderText = dtable.Columns(j).ToString
                    s.SortExpression = dtable.Columns(j).ToString
                    grdconsumables.Columns.Add(s)
                Next
                grdconsumables.DataBind()
            Else
                grdconsumables.EmptyDataText = "No data Found"
                grdconsumables.DataBind()
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub grdconsumables_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdconsumables.Sorting
        bindConsumables(Request.QueryString(Convert.ToString("CId")), e.SortExpression, sortOrder)
    End Sub
    Public Property sortOrder() As String
        Get
            If ViewState("sortOrder").ToString() = "desc" Then
                ViewState("sortOrder") = "asc"
            Else
                ViewState("sortOrder") = "desc"
            End If
            Return ViewState("sortOrder").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortOrder") = value
        End Set
    End Property
End Class
