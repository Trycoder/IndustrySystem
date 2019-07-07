Imports System.Data
Imports System.Data.SqlClient
Partial Class Vendor
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As New SqlCommand
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
        If Page.IsPostBack = False Then
            'grdvendor.PageSize = ConfigurationManager.AppSettings("PageSize")
            ViewState("sortOrder") = ""
            BindGrid("", "")
            If Not String.IsNullOrEmpty(rowid) Then
                bindVendorDetails(rowid)
            End If
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If

    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If UCase(btnSave.Text) = UCase("Update") Then
                If Not String.IsNullOrEmpty(rowid) Then
                    Dim str As String
                    con.Open()
                    str = "update tbl_Asset_Vendor set VendorName ='" & txtvendorname.Text & "',status='" & drpstatus.SelectedValue & "' where VendorID=" & rowid & ""
                    cmd = New SqlCommand(str, con)
                    cmd.ExecuteScalar()
                    con.Close()
                    Dim myscript1 As String = "alert('Vendor Updated Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                    BindGrid("", "")
                    txtvendorname.Text = ""
                    drpstatus.SelectedIndex = 0
                    btnSave.Text = "Save"
                End If
            Else
                If checkvendor(txtvendorname.Text.Trim) = True Then
                    txtvendorname.Focus()
                    Exit Sub
                End If
                Dim str As String
                If con.State = Data.ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                str = "insert into tbl_Asset_Vendor(VendorName,status) values ('" & txtvendorname.Text & "','" & drpstatus.Text & "')"
                cmd.CommandText = str
                cmd.Connection = con
                cmd.ExecuteScalar()
                con.Close()
                'Dim myscript As String = "alert('Vendor Inserted Successfully');"
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                txtvendorname.Text = ""
                drpstatus.SelectedIndex = 0
                BindGrid("", "")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Public Function checkvendor(ByVal vendorname As String) As Boolean
        Dim Sql As String
        If con.State = Data.ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        Sql = "Select * from tbl_Asset_Vendor where VendorName like '%" & vendorname & "%' order by vendorname asc"
        cmd.CommandText = Sql
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            Dim myscript1 As String = "alert('Vendor already exists.. ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            Return True
        Else
            Return False
        End If
        con.Close()
    End Function

    Private Function bindVendorDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_Asset_Vendor where VendorID=" & id & "", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    txtvendorname.Text = Convert.ToString(rdr("VendorName"))
                    If drpstatus.Items.FindByValue(rdr("status")) IsNot Nothing Then
                        drpstatus.SelectedValue = rdr("status").ToString()
                    Else
                        drpstatus.SelectedIndex = 0
                    End If
                End If
            End If
            con.Close()
            btnSave.Text = "Update"
            BindGrid("", "")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function BindGrid(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String
            dtable = New DataTable
            If condition = "" Then
                sql = "select * from tbl_Asset_Vendor order by VendorName ASC"
            Else
                sql = "select * from tbl_Asset_Vendor where " & condition & "  order by VendorName ASC"
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
                grdvendor.DataSource = myDataView
                grdvendor.DataBind()
            Else
                grdvendor.EmptyDataText = "No data Found"
                grdvendor.DataBind()
            End If
            oAdapter.Dispose()
            con.Close()
            txtvendorname.Focus()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx")
    End Sub

    Protected Sub grdvendor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvendor.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim Vendorid As String = e.CommandArgument
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand("select count(vendorid) from tbl_asset_transactions where vendorid=" & Vendorid, con)
                Dim str As String = cmd.ExecuteScalar
                con.Close()
                cmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('Vendor Mapped with Tranction');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    con.Open()
                    cmd = New SqlCommand("delete from tbl_Asset_Vendor where vendorid=" & Vendorid, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    BindGrid("", "")
                    Dim myscript As String = "alert('Vendor Deleted Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvendor.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'Dim status As Label = CType(e.Row.FindControl("lblstatus"), Label)
                'If status.Text.Trim = "A" Then
                '    status.Text = "Active"
                'ElseIf status.Text.Trim = "I" Then
                '    status.Text = "InActive"
                'End If
                If e.Row.Cells(2).Text = "A" Then
                    e.Row.Cells(2).Text = "Active"
                ElseIf e.Row.Cells(2).Text = "I" Then
                    e.Row.Cells(2).Text = "Inactive"
                End If
                Dim imgbtn As HyperLink = DirectCast(e.Row.Cells(2).FindControl("imgedit"), HyperLink)
                If imgbtn IsNot Nothing Then
                    imgbtn.NavigateUrl = "Vendor.aspx?Id=" & grdvendor.DataKeys(e.Row.RowIndex).Value
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
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            Dim condition As String = ""
            If drpsearchvendor.SelectedValue = "VendorName" Then
                condition = drpsearchvendor.SelectedValue & " like '%" & txtvendorsearch.Text & "%'"
                BindGrid("", "", condition)
            ElseIf drpsearchvendor.SelectedValue = "status" Then
                condition = drpsearchvendor.SelectedValue & " like '%" & drpstatus1.SelectedValue & "%'"
                BindGrid("", "", condition)
            Else
                BindGrid("", "")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Dim condition As String = ""
    Protected Sub grdvendor_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvendor.PageIndexChanging
        grdvendor.PageIndex = e.NewPageIndex
        If drpsearchvendor.SelectedValue = "VendorName" And txtvendorsearch.Text <> "" Then
            condition = drpsearchvendor.SelectedValue & " like '%" & txtvendorsearch.Text & "%'"
            BindGrid("", "", condition)
        ElseIf drpsearchvendor.SelectedValue = "status" Then
            condition = drpsearchvendor.SelectedValue & " like '%" & drpstatus1.SelectedValue & "%'"
            BindGrid("", "", condition)
        Else
            BindGrid("", "")
        End If
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
    Protected Sub grdvendor_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvendor.Sorting
        If txtvendorsearch.Text.Trim = "" Then
            BindGrid(e.SortExpression, sortOrder)
        Else
            condition = drpsearchvendor.SelectedValue & " like '%" & txtvendorsearch.Text & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        End If
    End Sub

    Protected Sub drpsearchvendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpsearchvendor.SelectedIndexChanged
        If drpsearchvendor.SelectedValue = "VendorName" Then
            txtvendorsearch.Visible = True
            drpstatus1.Visible = False
            txtvendorsearch.Focus()
        ElseIf drpsearchvendor.SelectedValue = "status" Then
            txtvendorsearch.Visible = False
            drpstatus1.Visible = True
            drpstatus1.Focus()
            txtvendorsearch.Text = ""
        End If
    End Sub


    Protected Sub imgsrch_Click(sender As Object, e As ImageClickEventArgs) Handles imgsrch.Click
        Dim query As String
        If Not String.IsNullOrEmpty(txtvendorname.Text) Then
            query = "Select * from tbl_Asset_Vendor where VendorName like '%" & txtvendorname.Text.Trim & "%' order by vendorname asc"
        Else
            query = "select * from tbl_Asset_Vendor order by VendorName ASC"
        End If
        ucModal.LaunchPopup(query, "Vendor")
        ucModal.Visible = True
    End Sub
End Class
