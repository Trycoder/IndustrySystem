Imports System.Data
Imports System.Data.SqlClient
Partial Class ConsumableReports
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ViewState("sortOrder") = ""
            BindConsumablesWithUser("", "")
            lnkconsreport1.OnClientClick = "javascript:window.open('ConsumableDetailReport.aspx?RType=1', 'popupwindow', 'width=1100px,height=750px,left=100,top=100,scrollbars,resizable=1'); return false;"
            lnkconsreport3.OnClientClick = "javascript:window.open('ConsumableDetailReport.aspx?RType=2', 'popupwindow', 'width=1100px,height=750px,left=100,top=100,scrollbars,resizable=1'); return false;"
        End If
    End Sub
    Public Function BindConsumablesWithUser(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim myDataSet As New DataSet()
            sql = " select r.constypeid,am.assettypecode,count(r.constypeid) as Quantity from tbl_asset_consrequest r, tbl_asset_typemaster am where r.constypeid = am.assettypeid"
            sql = sql & " and r.status = 'I' and deploytype='U' group by am.assettypecode,r.constypeid"
            sqladr = New SqlDataAdapter(sql, con)
            sqladr.Fill(myDataSet)
            con.Close()
            If myDataSet.Tables.Count > 0 Then
                Dim myDataView As New DataView()
                myDataView = myDataSet.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdconssummary.DataSource = myDataView
                grdconssummary.DataBind()
            Else
                grdconssummary.EmptyDataText = "No data Found"
                grdconssummary.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
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

    Protected Sub grdconssummary_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdconssummary.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lnkbtn As LinkButton = DirectCast(e.Row.Cells(1).FindControl("lnkassetcode"), LinkButton)
                If lnkbtn IsNot Nothing Then
                    lnkbtn.OnClientClick = "javascript:window.open('ConsumableDetailReport.aspx?RType=3&AType=" & grdconssummary.DataKeys(e.Row.DataItemIndex).Value & "', 'popupwindow', 'width=1100px,height=750px,left=100,top=100,scrollbars,resizable=1'); return false;"
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub grdconssummary_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdconssummary.Sorting
        BindConsumablesWithUser(e.SortExpression, sortOrder)
    End Sub
    
End Class
