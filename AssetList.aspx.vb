Imports System.Data
Imports System.Data.SqlClient
Partial Class AssetList
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim rowid As String
    Dim GId As String
    Shared sortExpression As String
    Dim queryname As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rowid = Request.QueryString("Id")
        GId = Request.QueryString("GId")
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            bindcategory()
            If Not String.IsNullOrEmpty(rowid) Then
                bindAssetDetails(rowid)
            End If
            ViewState("sortOrder") = ""
            BindGrid("", "")
        End If
        If GId = "1" Then
            lblgroup.Text = "Asset(s)"
        ElseIf GId = "2" Then
            lblgroup.Text = "Software(s)"
        ElseIf GId = "3" Then
            lblgroup.Text = "Consumable(s)"
        End If
        'If GId = "1" Then
        '    queryname = "Select count(assettypeid) from tbl_asset_master where assettypeid="
        'ElseIf GId = "3" Then
        '    queryname = "Select count(constypeid) from tbl_Asset_Cons_Stock where constypeid="
        'End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim Sql As String
            If UCase(btnSave.Text) = UCase("Update") Then
                If Not String.IsNullOrEmpty(rowid) Then
                    Dim str As String
                    con.Open()
                    str = "update tbl_Asset_TypeMaster set AssetTypeDesc ='" & Trim(txtAssetTypeDesc.Text.Replace("'", "''")) & "',AssetTypeCode='" & Trim(txtAssetTypeCode.Text.Replace("'", "''")) & "',CatId=" & drpcategory.SelectedValue & " where AssetTypeId=" & rowid & ""
                    cmd = New SqlCommand(str, con)
                    cmd.ExecuteScalar()
                    con.Close()
                    txtAssetTypeCode.Text = ""
                    txtAssetTypeDesc.Text = ""

                    btnSave.Text = "Save"
                    BindGrid("", "")
                End If
            Else
                con.Open()
                cmd.Connection = con
                Sql = "Select * from tbl_Asset_TypeMaster where AssetTypeDesc like '" & Trim(txtAssetTypeCode.Text.Replace("'", "''")) & "' and CatId ='" & drpcategory.SelectedValue & "'"
                cmd.CommandText = Sql
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    Dim myscript As String = "alert('Asset/Software/Consumable Type exists.. ');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    txtAssetTypeCode.Focus()
                    Exit Sub
                End If
                con.Close()
                Dim str As String
                con.Open()
                str = "insert into tbl_Asset_TypeMaster(AssetTypeCode,AssetTypeDesc,CatId) values ('" & txtAssetTypeCode.Text.Replace("'", "''") & "','" & txtAssetTypeDesc.Text.Replace("'", "''") & "','" & drpcategory.SelectedValue & "')"
                cmd.CommandText = str
                cmd.Connection = con
                cmd.ExecuteScalar()
                con.Close()
                'Dim myscript1 As String = "alert('Asset Inserted Successfully! ');"
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                BindGrid("", "")
            End If
            txtAssetTypeCode.Text = ""
            txtAssetTypeDesc.Text = ""
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
        'Response.Redirect("HardwareList.aspx")
    End Sub
    Private Function bindAssetDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_Asset_TypeMaster where AssetTypeId=" & id & " order by assettypecode asc", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    txtAssetTypeCode.Text = rdr("AssetTypeCode")
                    txtAssetTypeDesc.Text = rdr("AssetTypeDesc")
                    If drpcategory.Items.FindByValue(rdr("CatId")) IsNot Nothing Then
                        drpcategory.SelectedValue = rdr("CatId").ToString()
                    Else
                        drpcategory.SelectedIndex = 0
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
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
    Public Function bindcategory()
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpcategory.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select * from tbl_Asset_CategoryMaster where groupid =" & GId & " order by catcode asc "
        rdr = cmd.ExecuteReader
        drpcategory.Items.Add(New ListItem("--Select--", 0))
        If rdr.HasRows Then
            While rdr.Read
                drpcategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim assettypeid As String = e.CommandArgument
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand("Select count(assettypeid) from tbl_Asset_Master where assettypeid = " & assettypeid, con)
                Dim str As String = cmd.ExecuteScalar
                con.Close()
                cmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('Asset/Software/Consumable Type Mapped with Assets');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    con.Open()
                    cmd = New SqlCommand("delete from tbl_asset_typemaster where assettypeid=" & assettypeid, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    BindGrid("", "")
                    Dim myscript As String = "alert('Asset/Software/Consumable Type Deleted Successfully!');"
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
            'Dim linkbtn As LinkButton = DirectCast(e.Row.FindControl("lnkproperties"), LinkButton)
            'linkbtn.PostBackUrl = "AddAssetProperties.aspx?AssetId=" & GridView1.DataKeys(e.Row.RowIndex).Value
            Dim lnkedit As HyperLink = DirectCast(e.Row.Cells(3).FindControl("imgedit"), HyperLink)
            If lnkedit IsNot Nothing Then
                lnkedit.NavigateUrl = "AssetList.aspx?Id=" & GridView1.DataKeys(e.Row.DataItemIndex).Value & "&GId=" & GId
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

    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpcategory.SelectedIndexChanged
        Try
            txtAssetTypeCode.Text = ""
            txtAssetTypeDesc.Text = ""
            BindGrid("", "")
            If drpcategory.SelectedValue = "" Then
                trsearch.Visible = False
            Else
                txtsearch.Text = ""
                trsearch.Visible = True
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
                sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId='" & drpcategory.SelectedItem.Value & "' order by am.AssetTypeCode"
            Else
                sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId='" & drpcategory.SelectedItem.Value & "' and " & condition & " order by am.AssetTypeCode"
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
            txtAssetTypeCode.Focus()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        If drpassetlistsearch.SelectedValue <> "" Then
            condition = drpassetlistsearch.SelectedValue & " like '%" & txtsearch.Text.Trim & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        Else
            BindGrid(e.SortExpression, sortOrder)
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
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpassetlistsearch.SelectedValue <> "" Then
                condition = drpassetlistsearch.SelectedValue & " like '%" & txtsearch.Text.Trim & "%'"
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
