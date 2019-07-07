Imports System.Data.SqlClient
Imports System.Data

Partial Class Supplier
    Inherits System.Web.UI.Page
    Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim rowid As String
    Dim dtable As DataTable
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        rowid = Request.QueryString("Id")
        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(rowid) Then
                bindCategoryDetails(rowid)
            End If
            ViewState("sortOrder") = ""
            BindGrid("", "")
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If UCase(btnSave.Text) = UCase("Update") Then
            If Not String.IsNullOrEmpty(rowid) Then
                Dim Sql As String
                Dim str As String
                con.Open()
                str = "update tbl_Supplier set suppliercode ='" & Trim(TxtSupplierCode.Text.Replace("'", "''")) & "',suppliername ='" & Trim(TxtSupplierName.Text.Replace("'", "''")) & "',"
                str = str & "SupplierAddress = '" & Trim(TxtSupplierAddress.Text.Replace("'", "''")) & "',SupplierContactPerson = '" & Trim(TxtContactPerson.Text.Replace("'", "''")) & "',"
                str = str & "SupplierPhoneNo= '" & Trim(TxtContactNo.Text.Replace("'", "''")) & "',SupplierGSTNo = '" & Trim(TxtGSTNo.Text.Replace("'", "''")) & "'"
                str = str & " where supplierid = " & rowid & """"
                cmd = New SqlCommand(str, con)
                cmd.ExecuteScalar()
                con.Close()
                BindGrid("", "")
                TxtSupplierCode.Text = ""
                TxtSupplierName.Text = ""
                TxtSupplierAddress.Text = ""
                TxtContactNo.Text = ""
                TxtContactPerson.Text = ""
                TxtGSTNo.Text = ""
                btnSave.Text = "Save"
            End If
        Else
            Dim Sql As String
            cmd = New SqlCommand
            con.Open()
            cmd.Connection = con
            Sql = "Select * from tbl_supplier where suppliercode like '" & Trim(TxtSupplierCode.Text.Replace("'", "''")) & "'  "
            cmd.CommandText = Sql
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                Dim myscript As String = "alert('Supplier Code already exists.. ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                TxtSupplierName.Focus()
                TxtSupplierCode.Text = ""
                Exit Sub
            End If
            con.Close()
            cmd.Dispose()
            Dim str As String
            con.Open()
            str = "insert into tbl_supplier(SupplierCode,SupplierName,SupplierAddress,SupplierContactPerson,SupplierPhoneNo,SupplierGSTNo) values ('" & TxtSupplierCode.Text.Replace("'", "''") & "','"
            str = str & TxtSupplierName.Text.Replace("'", "''") & "','" & TxtSupplierAddress.Text.Replace("'", "''") & "','" & TxtContactPerson.Text.Replace("'", "''") & "','" & TxtContactNo.Text.Replace("'", "''") & "','" & TxtGSTNo.Text.Replace("'", "''") & "')"
            cmd.CommandText = str
            cmd.Connection = con
            cmd.ExecuteScalar()
            con.Close()
            BindGrid("", "")
            TxtSupplierCode.Text = ""
            TxtSupplierName.Text = ""
            TxtSupplierAddress.Text = ""
            TxtContactNo.Text = ""
            TxtContactPerson.Text = ""
            TxtGSTNo.Text = ""
        End If
    End Sub
    Private Function bindCategoryDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_supplier where supplierid=" & id & " order by suppliercode", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    TxtSupplierCode.Text = rdr("suppliercode")
                    TxtSupplierName.Text = rdr("suppliername")
                    TxtSupplierAddress.Text = rdr("SupplierAddress")
                    TxtContactPerson.Text = rdr("SupplierContactPerson")
                    TxtContactNo.Text = rdr("SupplierPhoneNo")
                    TxtGSTNo.Text = rdr("SupplierGSTNo")
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
                sql = "select * from tbl_supplier order by suppliercode"
            Else
                sql = "select * from tbl_supplier where " & condition & " order by suppliercode"
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
                grdreportcategory.DataSource = myDataView
                grdreportcategory.DataBind()
            Else
                grdreportcategory.EmptyDataText = "No data Found"
                grdreportcategory.DataBind()
            End If
            TxtSupplierCode.Focus()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub grdreportcategory_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdreportcategory.PageIndexChanging
        grdreportcategory.PageIndex = e.NewPageIndex
        If drpreportsearch.SelectedValue <> "" And txtshsearch.Text <> "" Then
            condition = drpreportsearch.SelectedValue & " like '%" & txtshsearch.Text & "%'"
            BindGrid("", "", condition)
        Else
            BindGrid("", "")
        End If
    End Sub

    Protected Sub grdreportcategory_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdreportcategory.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim repcatid As String = e.CommandArgument
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand("Select count(supplierid) from tbl_Material where supplierid =" & repcatid, con)
                Dim str As String = cmd.ExecuteScalar
                con.Close()
                cmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('Supplier Mapped with Material');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    con.Open()
                    cmd = New SqlCommand("delete from tbl_supplier where supplierid=" & repcatid, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    BindGrid("", "")
                    Dim myscript As String = "alert('Supplier Deleted Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdreportcategory.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lnkedit As HyperLink = DirectCast(e.Row.Cells(3).FindControl("imgedit"), HyperLink)
            If lnkedit IsNot Nothing Then
                lnkedit.NavigateUrl = "Supplier.aspx?Id=" & grdreportcategory.DataKeys(e.Row.RowIndex).Value
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

    Protected Sub grdreportcategory_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles grdreportcategory.SelectedIndexChanging
      
    End Sub

    Protected Sub grdreportcategory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdreportcategory.Sorting
        If txtshsearch.Text.Trim = "" Then
            BindGrid(e.SortExpression, sortOrder)
        Else
            condition = drpreportsearch.SelectedValue & " like '%" & txtshsearch.Text.Trim & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        End If
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
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
End Class
