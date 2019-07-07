Imports System.Data
Imports System.Data.SqlClient
Partial Class NewUser
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
                GetUserGroup()
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
    Public Function GetUserGroup()
        Dim sql As String = "select * from tbl_asset_usergroup order by groupname"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        drpusergroup.Items.Clear()
        drpusergroup.Items.Add(New ListItem("--Select--", ""))
        If dtable.Rows.Count > 0 Then
            For i As Integer = 0 To dtable.Rows.Count - 1
                drpusergroup.Items.Add(New ListItem(dtable.Rows(i)("groupname").ToString(), dtable.Rows(i)("id").ToString()))
            Next
        End If
        con.Close()
    End Function
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
                    str = "update tbl_Asset_users set userid ='" & Trim(txtuserid.Text) & "',username='" & Trim(txtusername.Text) & "',usergroup=" & drpusergroup.SelectedValue & ",status='" & drpstatus.SelectedValue & "',isstag=" & drprights.SelectedValue & "  where Id=" & rowid & ""
                    cmd = New SqlCommand(str, con)
                    cmd.ExecuteScalar()
                    con.Close()
                    BindGrid("", "")
                    drpusergroup.SelectedIndex = 0
                    drpstatus.SelectedIndex = 0
                    drprights.SelectedIndex = 0
                    txtuserid.Text = ""
                    txtusername.Text = ""
                    btnSave.Text = "Save"
                End If
            Else
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd.Connection = con
                Sql = "Select * from tbl_Asset_users where userid like '" & Trim(txtuserid.Text) & "'"
                cmd.CommandText = Sql
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    Dim myscript As String = "alert('EmployeeID already exists.. ');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    txtuserid.Text = ""
                    txtuserid.Focus()
                    Exit Sub
                End If
                con.Close()
                Dim str As String
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                str = "insert into tbl_Asset_users(userid,username,usergroup,status,isstag) values ('" & Trim(txtuserid.Text) & "','" & Trim(txtusername.Text) & "','" & drpusergroup.SelectedValue & "','" & drpstatus.SelectedValue & "'," & drprights.SelectedValue & ")"
                cmd.CommandText = str
                cmd.Connection = con
                cmd.ExecuteScalar()
                con.Close()
                ' Insert Setup Days

                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                str = "insert into tbl_asset_setupdays(userid,TransDays,WarrantyExpDays,DefaultSetting,AlertDays) values ('" & Trim(txtuserid.Text) & "',10,20,1,20)"
                cmd.CommandText = str
                cmd.Connection = con
                cmd.ExecuteScalar()
                con.Close()

                txtuserid.Text = ""
                txtusername.Text = ""
                drpusergroup.SelectedIndex = 0
                drpstatus.SelectedIndex = 0
                drprights.SelectedIndex = 0
                BindGrid("", "")
            End If
            ' Response.Redirect("Default.aspx", True)
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
                sql = "select * from tbl_Asset_users  order by username  asc"
            Else
                sql = "select * from tbl_Asset_users where " & condition & "  order by username ASC"
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
            txtuserid.Focus()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub grdusers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdusers.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Trim(e.Row.Cells(3).Text) = "1" Then
                e.Row.Cells(3).Text = "Super Admin"
            ElseIf Trim(e.Row.Cells(3).Text) = "2" Then
                e.Row.Cells(3).Text = "User"
            ElseIf Trim(e.Row.Cells(3).Text) = "3" Then
                e.Row.Cells(3).Text = "Admin-Helpdesk"
            ElseIf Trim(e.Row.Cells(3).Text) = "4" Then
                e.Row.Cells(3).Text = "Admin-Finance"
            ElseIf Trim(e.Row.Cells(3).Text) = "5" Then
                e.Row.Cells(3).Text = "Admin-ICT"
            End If
            If Trim(e.Row.Cells(4).Text) = "A" Then
                e.Row.Cells(4).Text = "Active"
            ElseIf Trim(e.Row.Cells(4).Text) = "I" Then
                e.Row.Cells(4).Text = "Inactive"
            End If
            If Trim(e.Row.Cells(5).Text) = "0" Then
                e.Row.Cells(5).Text = "No Access"
            ElseIf Trim(e.Row.Cells(5).Text) = "1" Then
                e.Row.Cells(5).Text = "Approve"
            ElseIf Trim(e.Row.Cells(5).Text) = "2" Then
                e.Row.Cells(5).Text = "Issue"
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
            Dim lnkedit As HyperLink = DirectCast(e.Row.Cells(3).FindControl("imgedit"), HyperLink)
            If lnkedit IsNot Nothing Then
                lnkedit.NavigateUrl = "NewUser.aspx?Id=" & grdusers.DataKeys(e.Row.DataItemIndex).Value & ""
            End If
        End If
    End Sub
    Private Function bindAssetDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_Asset_users where id=" & id & "", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    txtuserid.Text = rdr("userid")
                    txtusername.Text = Convert.ToString(rdr("username"))
                    If drpusergroup.Items.FindByValue(rdr("usergroup")) IsNot Nothing Then
                        drpusergroup.SelectedValue = rdr("usergroup").ToString()
                    Else
                        drpusergroup.SelectedIndex = 0
                    End If
                    If drpstatus.Items.FindByValue(rdr("status")) IsNot Nothing Then
                        drpstatus.SelectedValue = rdr("status").ToString()
                    Else
                        drpstatus.SelectedIndex = 0
                    End If
                    If drprights.Items.FindByValue(rdr("isstag")) IsNot Nothing Then
                        drprights.SelectedValue = rdr("isstag").ToString()
                    Else
                        drprights.SelectedIndex = 0
                    End If
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
        If drpemployeesearch.SelectedValue = "userid" Then
            condition = drpemployeesearch.SelectedValue & " like '%" & txtshempid.Text.Trim & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        ElseIf drpemployeesearch.SelectedValue = "username" Then
            condition = drpemployeesearch.SelectedValue & " like '%" & txtshempname.Text.Trim & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        ElseIf drpemployeesearch.SelectedValue = "usergroup" Then
            condition = drpemployeesearch.SelectedValue & " like '%" & drpshusergroup.SelectedValue & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        ElseIf drpemployeesearch.SelectedValue = "status" Then
            condition = drpemployeesearch.SelectedValue & " like '%" & drpshstatus.SelectedValue & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        ElseIf drpemployeesearch.SelectedValue = "isstag" Then
            condition = drpemployeesearch.SelectedValue & " like '%" & drpshrights.SelectedValue & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        Else
            BindGrid(e.SortExpression, sortOrder)
        End If
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpemployeesearch.SelectedValue = "userid" Then
                condition = drpemployeesearch.SelectedValue & " like '%" & txtshempid.Text.Trim & "%'"
                BindGrid("", "", condition)
            ElseIf drpemployeesearch.SelectedValue = "username" Then
                condition = drpemployeesearch.SelectedValue & " like '%" & txtshempname.Text.Trim & "%'"
                BindGrid("", "", condition)
            ElseIf drpemployeesearch.SelectedValue = "usergroup" Then
                condition = drpemployeesearch.SelectedValue & " like '%" & drpshusergroup.SelectedValue & "%'"
                BindGrid("", "", condition)
            ElseIf drpemployeesearch.SelectedValue = "status" Then
                condition = drpemployeesearch.SelectedValue & " like '%" & drpshstatus.SelectedValue & "%'"
                BindGrid("", "", condition)
            ElseIf drpemployeesearch.SelectedValue = "isstag" Then
                condition = drpemployeesearch.SelectedValue & " like '%" & drpshrights.SelectedValue & "%'"
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
        If drpemployeesearch.SelectedValue = "userid" Then
            txtshempid.Visible = True
            txtshempname.Visible = False
            drpshusergroup.Visible = False
            drpshstatus.Visible = False
            drpshrights.Visible = False
            txtshempid.Focus()
        ElseIf drpemployeesearch.SelectedValue = "username" Then
            txtshempid.Visible = False
            txtshempname.Visible = True
            drpshusergroup.Visible = False
            drpshstatus.Visible = False
            drpshrights.Visible = False
            txtshempname.Focus()
        ElseIf drpemployeesearch.SelectedValue = "usergroup" Then
            txtshempid.Visible = False
            txtshempname.Visible = False
            drpshusergroup.Visible = True
            drpshstatus.Visible = False
            drpshrights.Visible = False
            drpshusergroup.Focus()
        ElseIf drpemployeesearch.SelectedValue = "status" Then
            txtshempid.Visible = False
            txtshempname.Visible = False
            drpshusergroup.Visible = False
            drpshstatus.Visible = True
            drpshrights.Visible = False
            drpshstatus.Focus()
        ElseIf drpemployeesearch.SelectedValue = "isstag" Then
            txtshempid.Visible = False
            txtshempname.Visible = False
            drpshusergroup.Visible = False
            drpshstatus.Visible = False
            drpshrights.Visible = True
            drpshrights.Focus()
        Else
            BindGrid("", "")
        End If
    End Sub
End Class
