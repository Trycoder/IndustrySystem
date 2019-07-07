Imports System.Data
Imports System.Data.SqlClient
Partial Class MaintananceList
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim rowid As String
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        rowid = Request.QueryString("Id")
        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(rowid) Then
                bindMaintananceDetails(rowid)
            End If
            ViewState("sortOrder") = ""
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub
   
    Private Sub bindMaintananceDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_Asset_Maintainance where id=" & id & " order by activity", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    If drptype.Items.FindByValue(rdr("activitytype")) IsNot Nothing Then
                        drptype.SelectedValue = rdr("activitytype").ToString()
                    Else
                        drptype.SelectedIndex = 0
                    End If
                    txtactivityname.Text = rdr("activity")
                End If
            End If
            con.Close()
            btnSave.Text = "Update"
            BindGrid(drptype.SelectedValue, "", "")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drptype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drptype.SelectedIndexChanged
        Try
            txtactivityname.Text = ""
            If drptype.SelectedValue <> "" Then
                BindGrid(drptype.SelectedValue, "", "")
                trsearch.Visible = True
            Else
                trsearch.Visible = False
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub BindGrid(ByVal activitytypeid As String, ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String
            dtable = New DataTable
            If condition = "" Then
                sql = "select * from tbl_Asset_Maintainance where activitytype ='" & activitytypeid & "' order by activity"
            Else
                sql = "select * from tbl_Asset_Maintainance where " & condition & " order by activity"
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim oAdapter As New SqlDataAdapter(sql, con)
            Dim myDataSet As New DataSet()
            oAdapter.Fill(myDataSet)
            Dim myDataView As New DataView()
            myDataView = myDataSet.Tables(0).DefaultView
            If sortExpression <> String.Empty Then
                myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
            End If
            grdmaintanance.DataSource = myDataView
            grdmaintanance.DataBind()
            txtactivityname.Focus()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If drptype.SelectedIndex = 0 Then
            Dim myscript As String = "alert('Select Type..');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
            txtactivityname.Focus()
            Exit Sub
        End If
        If UCase(btnSave.Text) = UCase("Update") Then
            If Not String.IsNullOrEmpty(rowid) Then
                Dim Sql As String
                Dim str As String
                con.Open()
                str = "update tbl_Asset_Maintainance set activitytype='" & drptype.SelectedValue & "',activity ='" & Trim(txtactivityname.Text.Replace("'", "''")) & "' where id=" & rowid & ""
                cmd = New SqlCommand(str, con)
                cmd.ExecuteScalar()
                con.Close()
                BindGrid(drptype.SelectedValue, "", "")
                btnSave.Text = "Save"
            End If
        Else
            Dim Sql As String
            con.Open()

            Sql = "Select * from tbl_Asset_Maintainance where activity = '" & Trim(txtactivityname.Text) & "' and activitytype='" & drptype.SelectedValue & "'"
            cmd = New SqlCommand(Sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                Dim myscript As String = "alert('Activity already exists..');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                txtactivityname.Focus()
                Exit Sub
            End If
            con.Close()
            Dim str As String
            con.Open()
            str = "insert into tbl_Asset_Maintainance(activitytype,activity) values ('" & drptype.SelectedValue & "','" & Trim(txtactivityname.Text.Replace("'", "''")) & "')"
            cmd.CommandText = str
            cmd.Connection = con
            cmd.ExecuteNonQuery()
            con.Close()
            BindGrid(drptype.SelectedValue, "", "")
        End If
        txtactivityname.Text = ""
    End Sub

    Protected Sub grdmaintanance_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdmaintanance.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim reasonid As String = e.CommandArgument
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand("Select count(reasonid) from tbl_Asset_Transactions where reasonid=" & reasonid, con)
                Dim str As String = cmd.ExecuteScalar
                con.Close()
                cmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('Reason Mapped with Asset Transaction');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    con.Open()
                    cmd = New SqlCommand("delete from tbl_Asset_Maintainance where id=" & reasonid, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    BindGrid(drptype.SelectedValue, "", "")
                    Dim myscript As String = "alert('Reason Deleted Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub


    Protected Sub grdmaintanance_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdmaintanance.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lbl As Label = DirectCast(e.Row.Cells(1).FindControl("lbltype"), Label)
            If lbl IsNot Nothing Then
                If lbl.Text = "0" Then
                    lbl.Text = "Purchase"
                ElseIf lbl.Text = "1" Then
                    lbl.Text = "Warranty"
                ElseIf lbl.Text = "2" Then
                    lbl.Text = "Deployment"
                ElseIf lbl.Text = "3" Then
                    lbl.Text = "UnDeployment"
                ElseIf lbl.Text = "4" Then
                    lbl.Text = "Repair(Inhouse)"
                ElseIf lbl.Text = "5" Then
                    lbl.Text = "Repair(Outside)"
                ElseIf lbl.Text = "6" Then
                    lbl.Text = "Return"
                ElseIf lbl.Text = "7" Then
                    lbl.Text = "Retired"
                ElseIf lbl.Text = "8" Then
                    lbl.Text = "Sales"
                End If
            End If
            Dim imgbtn As HyperLink = DirectCast(e.Row.Cells(2).FindControl("imgedit"), HyperLink)
                If imgbtn IsNot Nothing Then
                    '  Session("Edit") = "Edit"
                    imgbtn.NavigateUrl = "MaintananceList.aspx?Id=" & grdmaintanance.DataKeys(e.Row.DataItemIndex).Value
                End If
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("Usergroup") <> "1" Then
                Dim hyper As HyperLink = CType(e.Row.FindControl("imgedit"), HyperLink)
                If hyper IsNot Nothing Then
                   hyper.Enabled = False
                        hyper.ImageUrl = "images/dash.jpg"
                End If
                Dim imgbtn As ImageButton = CType(e.Row.FindControl("imgdelete"), ImageButton)
                If imgbtn IsNot Nothing Then
                                           imgbtn.Enabled = False
                        imgbtn.ImageUrl = "images/dash.jpg"
                End If
            End If
        End If
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
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

    Protected Sub grdmaintanance_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdmaintanance.Sorting
        If drpactivitysearch.SelectedValue <> "" Then
            condition = " activitytype = " & drpactivitysearch.SelectedValue & " and activity  like '%" & txtshactivity.Text.Trim & "%'"
            BindGrid("", e.SortExpression, sortOrder, condition)
        Else
            BindGrid("", e.SortExpression, sortOrder, condition)
        End If
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpactivitysearch.SelectedValue <> "" Then
                condition = " activitytype = " & drpactivitysearch.SelectedValue & " and activity  like '%" & txtshactivity.Text.Trim & "%'"
                BindGrid("", "", "", condition)
            Else
                BindGrid("", "", "", condition)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
End Class
