Imports System.Data
Imports System.Data.SqlClient
Partial Class AddUserGroup
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim rowid As String
    Dim GId As String
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
            If imgbtn IsNot Nothing Then
                imgbtn.Focus()
            End If
            rowid = Request.QueryString("Id")
            If Not IsPostBack Then
                If Not String.IsNullOrEmpty(rowid) Then
                    bindAssetDetails(rowid)
                    BindGrid("", "")
                End If
                ViewState("sortOrder") = ""
                BindGrid("", "")
            End If
            If Session("Usergroup") <> "1" Then
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim Sql As String
            If UCase(btnSave.Text) = UCase("Update") Then
                If Not String.IsNullOrEmpty(rowid) Then
                    Dim str As String
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    str = "update tbl_asset_usergroup set groupname ='" & Trim(txtgroupname.Text) & "'  where Id=" & rowid & ""
                    cmd = New SqlCommand(str, con)
                    cmd.ExecuteScalar()
                    con.Close()
                    BindGrid("", "")
                    txtgroupname.Text = ""
                    btnSave.Text = "Save"
                End If
            Else
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd.Connection = con
                Sql = "Select * from tbl_asset_usergroup where groupname like '" & Trim(txtgroupname.Text) & "'"
                cmd.CommandText = Sql
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    Dim myscript As String = "alert('Group Name already exists.. ');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    txtgroupname.Text = ""
                    txtgroupname.Focus()
                    Exit Sub
                End If
                con.Close()
                Dim str As String
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                str = "insert into tbl_asset_usergroup(groupname) values ('" & Trim(txtgroupname.Text) & "')"
                cmd.CommandText = str
                cmd.Connection = con
                cmd.ExecuteScalar()
                con.Close()
                txtgroupname.Text = ""
                BindGrid("", "")
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
                sql = "select * from tbl_asset_usergroup  order by groupname  asc"
            Else
                sql = "select * from tbl_asset_usergroup where " & condition & "  order by groupname ASC"
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
                grdusers.DataSource = myDataView
                grdusers.DataBind()
            Else
                grdusers.EmptyDataText = "No data Found"
                grdusers.DataBind()
            End If
            txtgroupname.Focus()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub grdusers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdusers.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lnkedit As HyperLink = DirectCast(e.Row.Cells(3).FindControl("imgedit"), HyperLink)
            If lnkedit IsNot Nothing Then
                lnkedit.NavigateUrl = "AddUserGroup.aspx?Id=" & grdusers.DataKeys(e.Row.DataItemIndex).Value & ""
            End If
        End If
        If Session("Usergroup") <> "1" Then
            Dim hyper As HyperLink = CType(e.Row.FindControl("imgedit"), HyperLink)
            If hyper IsNot Nothing Then
                hyper.Enabled = False
                hyper.ImageUrl = "images/dash.jpg"
            End If
        End If
    End Sub
    Private Function bindAssetDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_asset_usergroup where id=" & id & "", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    txtgroupname.Text = rdr("groupname")
                End If
            End If
            con.Close()
            btnSave.Text = "Update"
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

    Protected Sub grdusers_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdusers.Sorting
        If drpemployeesearch.SelectedValue = "groupname" Then
            condition = drpemployeesearch.SelectedValue & " like '%" & txtgroupname.Text.Trim & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        Else
            BindGrid(e.SortExpression, sortOrder)
        End If
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpemployeesearch.SelectedValue = "groupname" Then
                condition = drpemployeesearch.SelectedValue & " like '%" & txtshgroupname.Text.Trim & "%'"
                BindGrid("", "", condition)
            Else
                BindGrid("", "")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drpemployeesearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpemployeesearch.SelectedIndexChanged
        If drpemployeesearch.SelectedValue = "groupname" Then
            txtgroupname.Visible = True
            txtgroupname.Focus()
        Else
            BindGrid("", "")
        End If
    End Sub
End Class
