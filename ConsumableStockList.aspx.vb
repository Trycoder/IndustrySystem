Imports System.Data
Imports System.Data.SqlClient
Partial Class ConsumableStockList
    Inherits System.Web.UI.Page
    Dim con As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New System.Data.SqlClient.SqlCommand
    Dim rdr As System.Data.SqlClient.SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim query As String = ""
    Dim drader As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not Page.IsPostBack Then
            ViewState("sortOrder") = ""
            bindConsumables("", "")
        End If
    End Sub
    Public Function bindConsumables(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim strquery As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim myDataSet As New DataSet()
            strquery = "select atm.AssetTypeCode as ConsumableName,atm.AssetTypeId as Consumableid, sum(acs.quantity) as Quantity  "
            strquery &= " from tbl_Asset_CategoryMaster acm,tbl_Asset_TypeMaster atm,tbl_Asset_Cons_Stock acs"
            strquery &= " where(acm.catid = atm.catid And atm.AssetTypeId = acs.ConsTypeId And acm.groupid = 3)"
            strquery &= " group by atm.AssetTypeCode,atm.AssetTypeId order by atm.assettypecode"
            sqladr = New SqlDataAdapter(strquery, con)
            sqladr.Fill(myDataSet)
            con.Close()
            If myDataSet.Tables.Count > 0 Then
                Dim myDataView As New DataView()
                myDataView = myDataSet.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdconsumables.DataSource = myDataView
                grdconsumables.DataBind()
            Else
                grdconsumables.EmptyDataText = "No data Found"
                grdconsumables.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub grdconsumables_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdconsumables.Sorting
        bindConsumables(e.SortExpression, SortOrder)
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
