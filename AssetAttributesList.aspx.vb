Imports System.Data
Imports System.Data.SqlClient
Partial Class AssetAttributesList
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
            GId = Request.QueryString("GId")
            If Not IsPostBack Then
                BindCategory()
                If GId = "1" Then
                    lblmessage.Text = "Add New Asset Attributes"
                End If
                If GId = "3" Then
                    Dim item As New ListItem
                    item.Text = "Text(Number)"
                    item.Value = "Text(Number)"
                    drpattributetype.Items.Insert(2, item)
                    Dim item2 As New ListItem("Unit Price", "6")
                    If drpheader.Items.FindByText(item2.Text) Is Nothing Then
                        drpheader.Items.Add(item2)
                    End If
                    lblmessage.Text = "Add New Consumable Attributes"
                End If
                If GId = "2" Then
                    Dim item2 As New ListItem("Software Type", "11")
                    Dim item3 As New ListItem("Version", "12")
                    If drpheader.Items.FindByText(item2.Text) Is Nothing Then
                        drpheader.Items.Add(item2)
                    End If
                    If drpheader.Items.FindByText(item3.Text) Is Nothing Then
                        drpheader.Items.Add(item3)
                    End If
                    lblmessage.Text = "Add New Software Attributes"
                End If
                If Not String.IsNullOrEmpty(rowid) Then
                    bindAssetDetails(rowid)
                    BindGrid("", "")
                End If
                ViewState("sortOrder") = ""
            End If
            Dim item1 As New ListItem("Model", "7")
            If drpcategory.Items.FindByText("Printer") IsNot Nothing Then
                If drpheader.Items.FindByText(item1.Text) Is Nothing Then
                    drpheader.Items.Add(item1)
                End If
            Else
                drpheader.Items.Remove(item1)
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
                    str = "update tbl_Asset_Attributes set AttDesc ='" & txtattributedesc.Text & "',AttType='" & drpattributetype.SelectedValue & "',CatId=" & drpcategory.SelectedValue & ",IsRequired ='" & drpoccurance.SelectedValue & "',Header='" & drpheader.SelectedValue & "',atttooltipdesc='" & txttooltip.Text.Trim & "' where AttId=" & rowid & ""
                    cmd = New SqlCommand(str, con)
                    cmd.ExecuteScalar()
                    con.Close()
                    'Dim myscript1 As String = "alert('Asset Type Updated Successfully!');"
                    'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                    BindGrid("", "")
                    drpcategory.SelectedIndex = 0
                    txtattributedesc.Text = ""
                    txttooltip.Text = ""
                    drpattributetype.SelectedIndex = 0
                    drpoccurance.SelectedIndex = 0
                    drpheader.SelectedIndex = 0
                    btnSave.Text = "Save"
                End If
            Else
                con.Open()
                cmd.Connection = con
                Sql = "Select * from tbl_Asset_Attributes where AttDesc like '" & Trim(txtattributedesc.Text) & "' and CatId =" & drpcategory.SelectedValue & ""
                cmd.CommandText = Sql
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    Dim myscript As String = "alert('Fieldname already exists.. ');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    txtattributedesc.Focus()
                    Exit Sub
                End If
                con.Close()
                Dim str As String
                Dim i As Integer = GetFieldOrder(drpcategory.SelectedValue) + 1
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                str = "insert into tbl_Asset_Attributes(AttDesc,AttType,CatId,IsRequired,FieldOrder,Header,atttooltipdesc) values ('" & txtattributedesc.Text & "','" & drpattributetype.SelectedValue & "','" & drpcategory.SelectedValue & "'," & drpoccurance.SelectedValue & ",'Att" & i & "','" & drpheader.SelectedValue & "','" & txttooltip.Text.Trim() & "')"
                cmd.CommandText = str
                cmd.Connection = con
                cmd.ExecuteScalar()
                con.Close()
                'Dim myscript1 As String = "alert('Asset Attributes Inserted Successfully! ');"
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                txtattributedesc.Text = ""
                txttooltip.Text = ""
                drpattributetype.SelectedIndex = 0
                drpheader.SelectedIndex = 0
                BindGrid("", "")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
    Public Function BindCategory()
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpcategory.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        drpcategory.Items.Add(New ListItem("--Select--", ""))
        cmd.CommandText = "select * from tbl_Asset_CategoryMaster where groupid = " & GId & " order by catcode asc "
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpcategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function BindGrid(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String
            dtable = New DataTable
            If condition = "" Then
                sql = "select * from tbl_Asset_Attributes aatt join  tbl_Asset_CategoryMaster ac on ac.catid = aatt.catid where ac.catid =" & drpcategory.SelectedValue & " order by aatt.attdesc  asc"
            Else
                sql = "select * from tbl_Asset_Attributes aatt join  tbl_Asset_CategoryMaster ac on ac.catid = aatt.catid where ac.catid =" & drpcategory.SelectedValue & " and " & condition & " order by aatt.attdesc  asc"
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
                grdattributes.DataSource = myDataView
                grdattributes.DataBind()
            Else
                grdattributes.EmptyDataText = "No data Found"
                grdattributes.DataBind()
            End If
            txtattributedesc.Focus()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpcategory.SelectedIndexChanged
        Try
            If drpcategory.SelectedValue <> "" Then
                BindGrid("", "")
                ' trsearch.Visiable = True
                txtshdesc.Text = ""
                txtshtooltip.Text = ""
            Else
                '      trsearch.Visiable = False
                grdattributes.Dispose()
                grdattributes.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub grdattributes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdattributes.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim attid As String = e.CommandArgument
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand("Select count(assettypeid) from tbl_asset_attribute_details where attid = " & attid, con)
                Dim str As String = cmd.ExecuteScalar
                con.Close()
                cmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('Attribute Mapped with Assets');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    con.Open()
                    cmd = New SqlCommand("delete from tbl_asset_attributes where attid=" & attid, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    BindGrid("", "")
                    Dim myscript As String = "alert('Attribute Deleted Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub grdattributes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdattributes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Trim(e.Row.Cells(3).Text) <> "Text" And Trim(e.Row.Cells(3).Text) <> "Text(Variable)" And Trim(e.Row.Cells(3).Text) <> "Text(Fixed)" And Trim(e.Row.Cells(3).Text) <> "Sequence" And Trim(e.Row.Cells(3).Text) <> "Date" And Trim(e.Row.Cells(3).Text) <> "FileUpload" And Trim(e.Row.Cells(3).Text) <> "Yes/No" And Trim(e.Row.Cells(3).Text) <> "Text(Number)" And Trim(e.Row.Cells(3).Text) <> "Random" Then
                Dim lnk1 As New HyperLink
                e.Row.Cells(6).Style.Add("color", "fff")
                lnk1.Text = "Update List"
                'lnk1.Target = "_blank"
                If Session("Usergroup") = "1" Then
                    lnk1.NavigateUrl = "AttributesItemList.aspx?AId=" & grdattributes.DataKeys(e.Row.RowIndex).Value & "&Attname=" & e.Row.Cells(1).Text
                    lnk1.Attributes.Add("OnClick", "javascript:window.open (this.href, 'popupwindow', 'width=420,height=500,left=300,top=300,scrollbars,resizable=1'); return false;")
                End If
                e.Row.Cells(6).Controls.Add(lnk1)
            End If
            If Trim(e.Row.Cells(4).Text) = "0" Then
                e.Row.Cells(4).Text = "Non-Mandatory"
            ElseIf Trim(e.Row.Cells(4).Text) = "1" Then
                e.Row.Cells(4).Text = "Mandatory"
            End If
            If Trim(e.Row.Cells(5).Text) = "0" Then
                e.Row.Cells(5).Text = ""
            ElseIf Trim(e.Row.Cells(5).Text) = "1" Then
                e.Row.Cells(5).Text = "PO Number"
            ElseIf Trim(e.Row.Cells(5).Text) = "2" Then
                e.Row.Cells(5).Text = "PO Date"
            ElseIf Trim(e.Row.Cells(5).Text) = "3" Then
                e.Row.Cells(5).Text = "Warranty Start Date"
            ElseIf Trim(e.Row.Cells(5).Text) = "4" Then
                e.Row.Cells(5).Text = "Warranty End Date"
            ElseIf Trim(e.Row.Cells(5).Text) = "5" Then
                e.Row.Cells(5).Text = "Primary"
            ElseIf Trim(e.Row.Cells(5).Text) = "6" Then
                e.Row.Cells(5).Text = "Unit Price"
            ElseIf Trim(e.Row.Cells(5).Text) = "7" Then
                e.Row.Cells(5).Text = "Model"
            ElseIf Trim(e.Row.Cells(5).Text) = "8" Then
                e.Row.Cells(5).Text = "Lock No"
            ElseIf Trim(e.Row.Cells(5).Text) = "9" Then
                e.Row.Cells(5).Text = "Contract No"
            ElseIf Trim(e.Row.Cells(5).Text) = "10" Then
                e.Row.Cells(5).Text = "Contract Vendor"
            ElseIf Trim(e.Row.Cells(5).Text) = "11" Then
                e.Row.Cells(5).Text = "Software Type"
            ElseIf Trim(e.Row.Cells(5).Text) = "12" Then
                e.Row.Cells(5).Text = "Version"
            End If
            Dim lnkedit As HyperLink = DirectCast(e.Row.Cells(3).FindControl("imgedit"), HyperLink)
            If lnkedit IsNot Nothing Then
                lnkedit.NavigateUrl = "AssetAttributesList.aspx?Id=" & grdattributes.DataKeys(e.Row.DataItemIndex).Value & "&GId=" & GId
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
        End If
    End Sub
    Public Function GetFieldOrder(ByVal catid As String) As Integer
        Try
            Dim Sql As String
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd.Connection = con
            Sql = "select max(convert(float,(substring(FieldOrder,4,len(FieldOrder)-1)))) as field from tbl_Asset_Attributes where CatId=" & catid & ""
            cmd.CommandText = Sql
            If Not cmd.ExecuteScalar().Equals(DBNull.Value) Then
                Return Int32.Parse(cmd.ExecuteScalar().ToString())
            Else
                Return 0
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Private Function bindAssetDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_Asset_Attributes where AttId=" & id & "", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    txtattributedesc.Text = rdr("AttDesc")
                    txttooltip.Text = Convert.ToString(rdr("atttooltipdesc"))
                    If drpattributetype.Items.FindByValue(rdr("AttType")) IsNot Nothing Then
                        drpattributetype.SelectedValue = rdr("AttType").ToString()
                    Else
                        drpattributetype.SelectedIndex = 0
                    End If
                    If drpcategory.Items.FindByValue(rdr("CatId")) IsNot Nothing Then
                        drpcategory.SelectedValue = rdr("CatId").ToString()
                    Else
                        drpcategory.SelectedIndex = 0
                    End If
                    If drpoccurance.Items.FindByValue(rdr("IsRequired")) IsNot Nothing Then
                        drpoccurance.SelectedValue = rdr("IsRequired").ToString()
                    Else
                        drpoccurance.SelectedIndex = 0
                    End If
                    If drpheader.Items.FindByValue(rdr("Header")) IsNot Nothing Then
                        drpheader.SelectedValue = rdr("Header").ToString()
                    Else
                        drpheader.SelectedIndex = 0
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

    Protected Sub grdattributes_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdattributes.Sorting
        If drpshattributes.SelectedValue = "attdesc" Then
            condition = drpshattributes.SelectedValue & " like '%" & txtshdesc.Text.Trim & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        ElseIf drpshattributes.SelectedValue = "atttooltipdesc" Then
            condition = drpshattributes.SelectedValue & " like '%" & txtshtooltip.Text.Trim & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        ElseIf drpshattributes.SelectedValue = "atttype" Then
            condition = drpshattributes.SelectedValue & " like '%" & drpshatttype.SelectedValue & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        ElseIf drpshattributes.SelectedValue = "isrequired" Then
            condition = drpshattributes.SelectedValue & " like '%" & drpshoccurance.SelectedValue & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        ElseIf drpshattributes.SelectedValue = "header" Then
            condition = drpshattributes.SelectedValue & " like '%" & drpshheader.SelectedValue & "%'"
            BindGrid(e.SortExpression, sortOrder, condition)
        Else
            BindGrid(e.SortExpression, sortOrder)
        End If
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpshattributes.SelectedValue = "attdesc" Then
                condition = drpshattributes.SelectedValue & " like '%" & txtshdesc.Text.Trim & "%'"
                BindGrid("", "", condition)
            ElseIf drpshattributes.SelectedValue = "atttooltipdesc" Then
                condition = drpshattributes.SelectedValue & " like '%" & txtshtooltip.Text.Trim & "%'"
                BindGrid("", "", condition)
            ElseIf drpshattributes.SelectedValue = "atttype" Then
                condition = drpshattributes.SelectedValue & " like '%" & drpshatttype.SelectedValue & "%'"
                BindGrid("", "", condition)
            ElseIf drpshattributes.SelectedValue = "isrequired" Then
                condition = drpshattributes.SelectedValue & " like '%" & drpshoccurance.SelectedValue & "%'"
                BindGrid("", "", condition)
            ElseIf drpshattributes.SelectedValue = "header" Then
                condition = drpshattributes.SelectedValue & " like '%" & drpshheader.SelectedValue & "%'"
                BindGrid("", "", condition)
            Else
                BindGrid("", "")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "").Replace("'", "''") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drpshattributes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpshattributes.SelectedIndexChanged
        If drpshattributes.SelectedValue = "attdesc" Then
            txtshdesc.Visible = True
            txtshtooltip.Visible = False
            drpshatttype.Visible = False
            drpshoccurance.Visible = False
            drpshheader.Visible = False
            txtshdesc.Focus()
            txtshdesc.Text = ""
        ElseIf drpshattributes.SelectedValue = "atttooltipdesc" Then
            txtshdesc.Visible = False
            txtshtooltip.Visible = True
            drpshatttype.Visible = False
            drpshoccurance.Visible = False
            drpshheader.Visible = False
            txtshtooltip.Focus()
            txtshtooltip.Text = ""
        ElseIf drpshattributes.SelectedValue = "atttype" Then
            txtshdesc.Visible = False
            txtshtooltip.Visible = False
            drpshatttype.Visible = True
            drpshoccurance.Visible = False
            drpshheader.Visible = False
            drpshatttype.SelectedIndex = 0
            drpshatttype.Focus()
        ElseIf drpshattributes.SelectedValue = "isrequired" Then
            txtshdesc.Visible = False
            txtshtooltip.Visible = False
            drpshatttype.Visible = False
            drpshoccurance.Visible = True
            drpshheader.Visible = False
            drpshoccurance.SelectedIndex = 0
            drpshoccurance.Focus()
        ElseIf drpshattributes.SelectedValue = "header" Then
            txtshdesc.Visible = False
            txtshtooltip.Visible = False
            drpshatttype.Visible = False
            drpshoccurance.Visible = False
            drpshheader.Visible = True
            drpshheader.SelectedIndex = 0
            drpshheader.Focus()
        End If
    End Sub
End Class
