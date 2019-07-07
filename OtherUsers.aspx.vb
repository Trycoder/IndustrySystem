
Imports System.Data.SqlClient
Imports System.Data
Partial Class OtherUsers
    Inherits System.Web.UI.Page
    Dim sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim sqldr As SqlDataReader
    Dim sqlad As SqlDataAdapter
    Dim sqlcmd As New SqlCommand
    Dim ds As New DataSet
    Dim sql As String
    Dim empno As String
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        empno = Request("UserId")
        If Not IsPostBack Then
            If Not empno = "" Then
                sql = "SELECT * FROM tbl_Asset_tempemployees where Emp_Number = '" & empno & "' order by emp_Name asc"
                sqlcon.Open()
                sqlcmd.Connection = sqlcon
                sqlcmd.CommandText = sql
                sqldr = sqlcmd.ExecuteReader
                While sqldr.Read
                    ViewState("Emp_Number") = sqldr("Emp_Number")
                    txtusername.Text = sqldr("Emp_Name")
                    txtdepartment.Text = sqldr("Dep_Name")
                    txtlocation.Text = sqldr("BuildingUnit")
                    If drpstatus.Items.FindByValue(sqldr("Emp_Status")) IsNot Nothing Then
                        drpstatus.SelectedValue = sqldr("Emp_Status")
                    Else
                        drpstatus.SelectedIndex = 0
                    End If
                End While
                txtusername.Attributes.Add("readonly", "readonly")
                btnSave.Text = "Update"
                sqldr.Close()
                sqlcon.Close()
            Else
                txtusername.Attributes.Remove("readonly")
                btnSave.Text = "Save"
            End If
            ViewState("sortOrder") = ""
            bindgrid("", "")
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub
    Sub bindgrid(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        If condition = "" Then
            sql = "SELECT * FROM tbl_Asset_tempemployees order by emp_Name asc "
        Else
            sql = "SELECT * FROM tbl_Asset_tempemployees where " & condition & " order by emp_Name asc "
        End If
        If sqlcon.State = ConnectionState.Open Then
            sqlcon.Close()
        End If
        sqlcon.Open()
        Dim oAdapter As New SqlDataAdapter(sql, sqlcon)
        Dim myDataSet As New DataSet()
        oAdapter.Fill(myDataSet)
        If myDataSet.Tables.Count > 0 Then
            Dim myDataView As New DataView()
            myDataView = myDataSet.Tables(0).DefaultView
            If sortExpression <> String.Empty Then
                myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
            End If
            grdLocation.DataSource = myDataView
            grdLocation.DataBind()
        Else
            grdLocation.EmptyDataText = "No data Found"
            grdLocation.DataBind()
        End If
        sqlcon.Close()
        ds.Dispose()
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dsLoc As New DataSet
        If Not txtusername.Text = "" Then
            If btnSave.Text = "Save" Then

                sql = "SELECT max(right(emp_number,4)) + 1 as No FROM tbl_Asset_tempemployees"
                sqlcon.Open()
                sqlcmd = New SqlCommand(sql, sqlcon)
                Dim empno As String = sqlcmd.ExecuteScalar()
                Dim employeeno As String = ""
                If empno.Length = 1 Then
                    employeeno = "000" & empno
                ElseIf empno.Length = 2 Then
                    employeeno = "00" & empno
                ElseIf empno.Length = 3 Then
                    employeeno = "0" & empno
                ElseIf empno.Length = 4 Then
                    employeeno = empno
                Else
                    employeeno = empno
                End If
                sqlcon.Close()
                dsLoc.Dispose()

                sql = "INSERT INTO tbl_Asset_tempemployees(Emp_Number,Emp_Name,Emp_Initial,Dep_Name,BuildingUnit,Emp_JoinDate,Emp_Status) values('E" & employeeno & "','" & Trim(txtusername.Text) & "','','" & Trim(txtdepartment.Text) & "','" & Trim(txtlocation.Text) & "','" & DateTime.Now.Today() & "','" & drpstatus.SelectedValue & "')"
                sqlcon.Open()
                sqlcmd.Connection = sqlcon
                sqlcmd.CommandType = CommandType.Text
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()
                sqlcon.Close()
                bindgrid("", "")
                txtusername.Text = ""
                txtdepartment.Text = ""
                txtlocation.Text = ""
            ElseIf btnSave.Text = "Update" Then
                sql = "UPDATE tbl_Asset_tempemployees SET Emp_Name = '" & Trim(txtusername.Text) & "',Dep_Name = '" & Trim(txtdepartment.Text) & "',BuildingUnit = '" & Trim(txtlocation.Text) & "',Emp_Status ='" & drpstatus.SelectedValue & "'  WHERE Emp_Number='" & ViewState("Emp_Number") & "'"
                sqlcon.Open()
                sqlcmd.Connection = sqlcon
                sqlcmd.CommandType = CommandType.Text
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()
                sqlcon.Close()
                btnSave.Text = "Save"
                bindgrid("", "")
                txtusername.Text = ""
                txtdepartment.Text = ""
                txtlocation.Text = ""
            End If
        Else
            Dim myscript As String = "alert('User Name can not be null.');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
            txtusername.Focus()
            Exit Sub
        End If
    End Sub

    Protected Sub grdLocation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLocation.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim userid As String = e.CommandArgument
                If sqlcon.State = ConnectionState.Open Then
                    sqlcon.Close()
                End If
                sqlcon.Open()
                sqlcmd = New SqlCommand("Select count(userid) from tbl_Asset_Transactions where userid='" & userid & "'", sqlcon)
                Dim str As String = sqlcmd.ExecuteScalar
                sqlcon.Close()
                sqlcmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('Ext.User Mapped with Asset Transaction');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    sqlcon.Open()
                    sqlcmd = New SqlCommand("delete from tbl_asset_tempemployees where Emp_Number='" & userid & "'", sqlcon)
                    sqlcmd.ExecuteNonQuery()
                    sqlcon.Close()
                    bindgrid("", "")
                    Dim myscript As String = "alert('Ext.User Deleted Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub grdLocation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLocation.RowDataBound
        Dim intlocid As String = ""
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Trim(e.Row.Cells(5).Text) = "A" Then
                e.Row.Cells(5).Text = "Active"
            ElseIf Trim(e.Row.Cells(5).Text) = "I" Then
                e.Row.Cells(5).Text = "Inactive"
            End If
            sql = "SELECT * FROM tbl_Asset_tempemployees WHERE Emp_Number = '" & e.Row.Cells(1).Text & "'"
            If sqlcon.State = ConnectionState.Open Then
                sqlcon.Close()
            End If
            sqlcon.Open()
            sqlcmd.Connection = sqlcon
            sqlcmd.CommandType = CommandType.Text
            sqlcmd.CommandText = sql
            sqldr = sqlcmd.ExecuteReader
            While sqldr.Read
                intlocid = sqldr("Emp_Number")
            End While
            sqldr.Close()
            sqlcon.Close()
            Dim Hyper2 As New HyperLink
            Hyper2 = e.Row.FindControl("imgEdit")
            Hyper2.NavigateUrl = "OtherUsers.aspx?UserID=" & intlocid
            Hyper2.DataBind()
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
    'Protected Sub grdLocation_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdLocation.RowDeleting
    '    sql = "DELETE FROM tbl_Asset_tempemployees WHERE Emp_Number = '" & Trim(grdLocation.Rows(e.RowIndex).Cells(1).Text) & "'"
    '    sqlcon.Open()
    '    sqlcmd.Connection = sqlcon
    '    sqlcmd.CommandText = sql
    '    sqlcmd.ExecuteNonQuery()
    '    sqlcon.Close()
    '    bindgrid("", "")
    '    lblmsg.Text = "User Name Deleted Successfuly!"
    'End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        btnSave.Text = "Save"
        txtusername.Text = ""
        txtdepartment.Text = ""
        txtlocation.Text = ""
        Response.Redirect("OtherUsers.aspx")
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
    Protected Sub grdLocation_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdLocation.Sorting
        If drpotherusersearch.SelectedValue = "Emp_Number" Or drpotherusersearch.SelectedValue = "Emp_Name" Or drpotherusersearch.SelectedValue = "Dep_Name" Or drpotherusersearch.SelectedValue = "BuildingUnit" Then
            txtshusers.Visible = True
            drpshstatus.Visible = False
            condition = drpotherusersearch.SelectedValue & " like '%" & txtshusers.Text.Trim & "%'"
            bindgrid(e.SortExpression, sortOrder, condition)
        ElseIf drpotherusersearch.SelectedValue = "Emp_Status" Then
            txtshusers.Visible = False
            drpshstatus.Visible = True
            condition = drpotherusersearch.SelectedValue & " like '%" & drpshstatus.SelectedValue & "%'"
            bindgrid(e.SortExpression, sortOrder, condition)
        Else
            bindgrid("", "", condition)
        End If
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpotherusersearch.SelectedValue = "Emp_Number" Or drpotherusersearch.SelectedValue = "Emp_Name" Or drpotherusersearch.SelectedValue = "Dep_Name" Or drpotherusersearch.SelectedValue = "BuildingUnit" Then
                txtshusers.Visible = True
                drpshstatus.Visible = False
                condition = drpotherusersearch.SelectedValue & " like '%" & txtshusers.Text.Trim & "%'"
                bindgrid("", "", condition)
            ElseIf drpotherusersearch.SelectedValue = "Emp_Status" Then
                txtshusers.Visible = False
                drpshstatus.Visible = True
                condition = drpotherusersearch.SelectedValue & " like '%" & drpshstatus.SelectedValue & "%'"
                bindgrid("", "", condition)
            Else
                bindgrid("", "", condition)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drpotherusersearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpotherusersearch.SelectedIndexChanged
        If drpotherusersearch.SelectedValue = "Emp_Status" Then
            drpshstatus.Visible = True
            txtshusers.Visible = False
        Else
            txtshusers.Visible = True
            drpshstatus.Visible = False
        End If
    End Sub
End Class
