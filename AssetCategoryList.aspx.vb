Imports System.Data
Imports System.Data.SqlClient
Partial Class AssetCategoryList
    Inherits System.Web.UI.Page
    Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim rowid As String
    Dim dtable As DataTable
    Dim GId As String
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rowid = Request.QueryString("Id")
        GId = Request.QueryString("GId")
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            drpgroupname.Items.Clear()
            If GId = "1" Then
                drpgroupname.Items.Add(New ListItem("Hardwares", "1"))
            ElseIf GId = "2" Then
                drpgroupname.Items.Add(New ListItem("Softwares", "2"))
            ElseIf GId = "3" Then
                drpgroupname.Items.Add(New ListItem("Consumables", "3"))
            End If
            If Not String.IsNullOrEmpty(rowid) Then
                bindCategoryDetails(rowid)
            End If
            ViewState("sortOrder") = ""
            BindGrid("", "")
            If Session("Usergroup") <> "1" Then
                btnSave.Enabled = False
            End If
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If UCase(btnSave.Text) = UCase("Update") Then
            If Not String.IsNullOrEmpty(rowid) Then
                Dim Sql As String
                Dim str As String
                con.Open()
                str = "update tbl_Asset_CategoryMaster set catcode ='" & Trim(txtcategorycode.Text.Replace("'", "''")) & "',catdesc='" & Trim(txtcategorydesc.Text.Replace("'", "''")) & "' where catid=" & rowid & ""
                cmd = New SqlCommand(str, con)
                cmd.ExecuteScalar()
                con.Close()
                'Dim myscript1 As String = "alert('Asset Category Updated Successfully!');"
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                BindGrid("catid", "Desc")
                txtcategorycode.Text = ""
                txtcategorydesc.Text = ""
                drpgroupname.SelectedIndex = 0
                btnSave.Text = "Save"
            End If
        Else
            Dim Sql As String
            cmd = New SqlCommand
            con.Open()
            cmd.Connection = con
            Sql = "Select * from tbl_Asset_CategoryMaster where catdesc like '" & Trim(txtcategorydesc.Text.Replace("'", "''")) & "' and groupid ='" & drpgroupname.SelectedValue & "'"
            cmd.CommandText = Sql
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                Dim myscript As String = "alert('Category already exists.. ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                txtcategorydesc.Focus()
                txtcategorydesc.Text = ""
                Exit Sub
            End If
            con.Close()
            cmd.Dispose()
            Dim str As String
            con.Open()
            str = "insert into tbl_Asset_CategoryMaster(catcode,catdesc,groupid) values ('" & txtcategorycode.Text.Replace("'", "''") & "','" & txtcategorydesc.Text.Replace("'", "''") & "','" & drpgroupname.SelectedValue & "')"
            cmd.CommandText = str
            cmd.Connection = con
            cmd.ExecuteScalar()
            con.Close()
            'Dim myscript1 As String = "alert('Asset Category Inserted Successfully! ');"
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            BindGrid("catid", "Desc")
            txtcategorycode.Text = ""
            txtcategorydesc.Text = ""
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim Catid As String = e.CommandArgument
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand("Select count(assettypeid) from tbl_Asset_typeMaster where catid=" & Catid, con)
                Dim str As String = cmd.ExecuteScalar
                con.Close()
                cmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('Category Mapped with Assets');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    con.Open()
                    cmd = New SqlCommand("delete from tbl_asset_categorymaster where catid=" & Catid, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    BindGrid("catid", "Desc")
                    Dim myscript As String = "alert('Category Deleted Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Text = "1" Then
                e.Row.Cells(3).Text = "Hardwares"
            ElseIf e.Row.Cells(3).Text = "2" Then
                e.Row.Cells(3).Text = "Softwares"
            ElseIf e.Row.Cells(3).Text = "3" Then
                e.Row.Cells(3).Text = "Consumables"
            End If
            Dim lnkedit As HyperLink = DirectCast(e.Row.Cells(4).FindControl("imgedit"), HyperLink)
            If lnkedit IsNot Nothing Then
                lnkedit.NavigateUrl = "AssetCategoryList.aspx?Id=" & GridView1.DataKeys(e.Row.DataItemIndex).Value & "&GId=" & GId
            End If
        End If

        If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
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

    Private Function bindCategoryDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_Asset_CategoryMaster where catid=" & id & " order by catcode asc", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    txtcategorycode.Text = rdr("catcode")
                    txtcategorydesc.Text = rdr("catdesc")
                    If drpgroupname.Items.FindByValue(rdr("groupid")) IsNot Nothing Then
                        drpgroupname.SelectedValue = rdr("groupid").ToString()
                    Else
                        drpgroupname.SelectedIndex = 0
                    End If
                End If
            End If
            con.Close()
            btnSave.Text = "Update"
            BindGrid("catcode", "ASC")
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
                sql = "select * from tbl_Asset_CategoryMaster where groupid = " & GId & " order by catcode asc "
            Else
                sql = "select * from tbl_Asset_CategoryMaster where groupid = " & GId & " and " & condition & "  order by catcode asc "
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
            GridView1.DataSource = myDataView
            GridView1.DataBind()
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

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        If drpcategorysearch.SelectedValue <> "" Then
            condition = drpcategorysearch.SelectedValue & " like '%" & txtshsearch.Text.Trim & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        Else
            BindGrid(e.SortExpression, sortOrder, condition)
        End If
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpcategorysearch.SelectedValue <> "" Then
                condition = drpcategorysearch.SelectedValue & " like '%" & txtshsearch.Text.Trim & "%'"
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

