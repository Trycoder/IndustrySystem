Imports System.Data
Imports System.Data.SqlClient
Partial Class VendorListUserControl
    Inherits System.Web.UI.UserControl
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable

    Public Sub LaunchPopup(ByVal CommandQuery As String, ByVal Page As String)
        'Dim sql As String
        'sql = "select * from tbl_Asset_Vendor order by VendorName ASC"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim oAdapter As New SqlDataAdapter(CommandQuery, con)
        Dim myDataSet As New DataSet()
        HiddenQuery.Value = CommandQuery
        HiddenPageName.Value = Page
        oAdapter.Fill(myDataSet)
        'GrdDetails.DataSource = myDataSet
        'GrdDetails.DataBind()
        If myDataSet.Tables.Count > 0 Then
            Dim myDataView As New DataView()
            myDataView = myDataSet.Tables(0).DefaultView
            GrdDetails.DataSource = Nothing
            GrdDetails.DataBind()
            GrdDetails.DataSource = myDataView
            GrdDetails.DataBind()
        Else
            GrdDetails.EmptyDataText = "No data Found"
            GrdDetails.DataBind()
        End If
        oAdapter.Dispose()
        con.Close()

        mpePopup.Show()

    End Sub

    Protected Sub imgdelete_Click(sender As Object, e As ImageClickEventArgs) Handles imgdelete.Click
        mpePopup.Hide()
    End Sub
    Protected Sub GrdDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdDetails.PageIndexChanging
        GrdDetails.PageIndex = e.NewPageIndex
        LaunchPopup(HiddenQuery.Value, HiddenPageName.Value)
    End Sub
    Protected Sub GrdDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdDetails.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim imgbtn As HyperLink = DirectCast(e.Row.FindControl("imgedit"), HyperLink)
                If imgbtn IsNot Nothing Then
                    imgbtn.NavigateUrl = HiddenPageName.Value + ".aspx?Id=" & GrdDetails.DataKeys(e.Row.RowIndex).Value
                End If
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Session("Usergroup") <> "1" Then
                    Dim hyper As HyperLink = CType(e.Row.FindControl("imgedit"), HyperLink)
                    If hyper IsNot Nothing Then
                        hyper.Enabled = False
                        hyper.ImageUrl = "images/dash.jpg"
                    End If
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(sender As Object, e As EventArgs) Handles btnsearch.Click
        Try
            If drpreportsearch.SelectedValue <> "" Then
                condition = drpreportsearch.SelectedValue & " like '%" & txtshsearch.Text.Trim & "%'"
                BindGrid("", "", condition)
            Else
                BindGrid("", "", condition)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindGrid(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String
            dtable = New DataTable
            If condition = "" Then
                sql = " Select e.empid,e.empcode,e.empname,d.deptname,de.designame from tbl_employee e,tbl_department d,tbl_designation de where e.deptid = d.deptid and e.desigid = de.desigid order by empcode asc"
            Else
                sql = " Select e.empid,e.empcode,e.empname,d.deptname,de.designame from tbl_employee e,tbl_department d,tbl_designation de where e.deptid = d.deptid and e.desigid = de.desigid and " & condition & " order by e.empcode"
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim oAdapter As New SqlDataAdapter(sql, con)
            Dim myDataSet As New DataSet()
            oAdapter.Fill(myDataSet)
            If myDataSet.Tables.Count > 0 Then
                Dim myDataView As New DataView()
                myDataView = myDataSet.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                GrdDetails.DataSource = myDataView
                GrdDetails.DataBind()
            Else
                GrdDetails.EmptyDataText = "No data Found"
                GrdDetails.DataBind()
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function



End Class
