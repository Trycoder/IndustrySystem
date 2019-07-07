Imports System.Data.SqlClient
Imports System.Data
Partial Class LocationMaster
    Inherits System.Web.UI.Page
    Dim sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim sqlcon1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim sqldr As SqlDataReader
    Dim sqlad As SqlDataAdapter
    Dim sqlcmd As SqlCommand
    Dim ds As New DataSet
    Dim sql As String
    Dim locationid As Integer
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        locationid = Request("locID")
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            If Not locationid = 0 Then
                btnSave.Text = "Update"
                bindlocationdetails(locationid)
                fillgrdLocation("", "")
            Else
                btnSave.Text = "Save"
            End If
            hdlink1.NavigateUrl = "LocationsubMaster.aspx"
            hdlink1.Attributes.Add("OnClick", "javascript:window.open (this.href, 'popupwindow', 'width=420,height=500,left=300,top=300,scrollbars,resizable=1'); return false;")
            ViewState("sortOrder") = ""
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
        lblmsg.Text = ""
    End Sub
    Sub fillgrdLocation(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")

        If condition = "" Then
            sql = "SELECT * FROM tbl_Asset_sublocation where locid='" & drploc2.SelectedValue & "' order by sublocname ASC"
        Else
            sql = "SELECT * FROM tbl_Asset_sublocation where  " & condition & " order by sublocname ASC"
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
        If Not txtloc3.Text = "" Then
            If btnSave.Text = "Save" Then
                sql = "SELECT * FROM tbl_Asset_sublocation WHERE locid = '" & drploc2.SelectedValue & "' and sublocname= '" & Trim(txtloc3.Text) & "'"
                sqlcon.Open()
                sqlad = New SqlDataAdapter(sql, sqlcon)
                sqlad.Fill(dsLoc)
                If dsLoc.Tables(0).Rows.Count > 0 Then
                    Dim myscript As String = "alert('Sublocation Name Already Exists.');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    txtloc3.Focus()
                    sqlcon.Close()
                    Exit Sub
                End If
                sqlcon.Close()
                dsLoc.Dispose()
                sql = "INSERT INTO tbl_Asset_sublocation(locid,sublocname) VALUES('" & drploc2.SelectedValue & "','" & Trim(txtloc3.Text) & "')"
                sqlcon.Open()
                sqlcmd = New SqlCommand(sql, sqlcon)
                sqlcmd.ExecuteNonQuery()
                sqlcon.Close()
                fillgrdLocation("", "")
                lblmsg.Text = "Sublocation Name Added Successfuly."
                txtloc3.Text = ""
                txtloc3.Focus()
            ElseIf btnSave.Text = "Update" Then
                sql = "UPDATE tbl_Asset_sublocation SET sublocname = '" & Trim(txtloc3.Text) & "',locid ='" & drploc2.SelectedValue & "' WHERE sublocid='" & locationid & "'"
                sqlcon.Open()
                sqlcmd = New SqlCommand(sql, sqlcon)
                sqlcmd.ExecuteNonQuery()
                sqlcon.Close()
                btnSave.Text = "Save"
                fillgrdLocation("", "")
                lblmsg.Text = "Sublocation Name Updated Successfuly."
                txtloc3.Text = ""
                txtloc3.Focus()
            End If
        Else
            Dim myscript As String = "alert('Sublocation Name can not be null.');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
            txtloc3.Focus()
            Exit Sub
        End If
    End Sub

    Protected Sub grdLocation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLocation.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim sublocid As String = e.CommandArgument
                If sqlcon.State = ConnectionState.Open Then
                    sqlcon.Close()
                End If
                sqlcon.Open()
                sqlcmd = New SqlCommand("Select count(sublocid) from tbl_asset_transactions where sublocid=" & sublocid, sqlcon)
                Dim str As String = sqlcmd.ExecuteScalar
                sqlcon.Close()
                sqlcmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('SubLocation Mapped with Transactions');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    sqlcon.Open()
                    sqlcmd = New SqlCommand("delete from tbl_Asset_sublocation where sublocid=" & sublocid, sqlcon)
                    sqlcmd.ExecuteNonQuery()
                    sqlcon.Close()
                    fillgrdLocation("", "")
                    Dim myscript As String = "alert('SubLocation Deleted Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub grdLocation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLocation.RowDataBound
        Dim intlocid As Integer
        sql = "SELECT * FROM tbl_Asset_sublocation WHERE sublocname = '" & e.Row.Cells(1).Text & "'"
        If sqlcon.State = ConnectionState.Open Then
            sqlcon.Close()
        End If
        sqlcon.Open()
        sqlcmd = New SqlCommand(sql, sqlcon)
        sqldr = sqlcmd.ExecuteReader
        While sqldr.Read
            intlocid = sqldr("sublocid")
        End While
        sqldr.Close()
        sqlcon.Close()

        Dim Hyper2 As New HyperLink
        If e.Row.RowType = DataControlRowType.DataRow Then
            Hyper2 = e.Row.FindControl("imgEdit")
            Hyper2.NavigateUrl = "LocationMaster.aspx?locID=" & intlocid
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
    Private Function bindlocationdetails(ByVal id As String)
        Try
            Dim sqldr1 As SqlDataReader
            If sqlcon1.State = ConnectionState.Open Then
                sqlcon1.Close()
            End If
            sqlcon1.Open()
            sqlcmd = New SqlCommand("select sub.sublocid,sub.sublocname,ad.locid,ad.loccatid from tbl_Asset_sublocation sub,tbl_Asset_location ad where sub.locid=ad.locid and sub.sublocid=" & id & "", sqlcon1)
            sqldr1 = sqlcmd.ExecuteReader
            If sqldr1.HasRows Then
                If sqldr1.Read Then
                    txtloc3.Text = sqldr1("sublocname")
                    If drploc1.Items.FindByValue(sqldr1("loccatid")) IsNot Nothing Then
                        drploc1.SelectedValue = sqldr1("loccatid").ToString()
                    Else
                        drploc1.SelectedIndex = 0
                    End If
                    FillLocation2()
                    If drploc2.Items.FindByValue(sqldr1("locid")) IsNot Nothing Then
                        drploc2.SelectedValue = sqldr1("locid").ToString()
                    Else
                        drploc2.SelectedIndex = 0
                    End If
                End If
            End If
            sqldr1.Close()
            sqlcon1.Close()
            btnSave.Text = "Update"
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub grdLocation_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdLocation.RowDeleting
        sql = "DELETE FROM tbl_Asset_location WHERE locname = '" & Trim(grdLocation.Rows(e.RowIndex).Cells(1).Text) & "'"
        sqlcon.Open()
        sqlcmd = New SqlCommand(sql, sqlcon)
        sqlcmd.ExecuteNonQuery()
        sqlcon.Close()
        fillgrdLocation("", "")
        lblmsg.Text = "Location Name Deleted Successfuly!"
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        btnSave.Text = "Save"
        txtloc3.Text = ""
        Response.Redirect("LocationMaster.aspx")
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
        If txtshlocation3.Text.Trim = "" Or drpshlocation2.SelectedValue = "" Then
            fillgrdLocation(e.SortExpression, sortOrder)
        Else
            If drpshlocations.SelectedValue = "Location2" Then
                condition = "locid =" & drpshlocation2.SelectedValue & ""
                fillgrdLocation(e.SortExpression, sortOrder, condition)
            ElseIf drpshlocations.SelectedValue = "Location3" Then
                condition = "sublocname" & " like '%" & txtshlocation3.Text.Trim & "%'"
                fillgrdLocation(e.SortExpression, sortOrder, condition)
            End If
        End If
    End Sub

    Protected Sub drploc1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drploc1.SelectedIndexChanged
        FillLocation2()
    End Sub
    Public Function FillLocation2()
        Try
            If drploc1.SelectedValue <> "" Then
                sql = "SELECT * FROM tbl_Asset_location WHERE loccatid = '" & drploc1.SelectedValue & "'"
                If sqlcon.State = ConnectionState.Open Then
                    sqlcon.Close()
                End If
                sqlcon.Open()
                sqlcmd = New SqlCommand(sql, sqlcon)
                sqldr = sqlcmd.ExecuteReader
                drploc2.Items.Clear()
                drpshlocation2.Items.Clear()
                drploc2.Items.Add(New ListItem("--Select--", ""))
                If sqldr.HasRows Then
                    While sqldr.Read
                        drploc2.Items.Add(New ListItem(sqldr("locname"), sqldr("locid")))
                        drpshlocation2.Items.Add(New ListItem(sqldr("locname"), sqldr("locid")))
                    End While
                End If
                '  sqldr.Close()
                sqlcon.Close()
            End If
            fillgrdLocation("", "")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub drploc2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drploc2.SelectedIndexChanged
        Try
            If drploc2.SelectedValue <> "" Then
                fillgrdLocation("", "")
                trsearch.Visible = True
            Else
                trsearch.Visible = False
            End If
            txtloc3.Focus()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Dim condition As String = ""
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpshlocations.SelectedValue = "Location2" Then
                condition = "locid =" & drpshlocation2.SelectedValue & ""
                fillgrdLocation("", "", condition)
            ElseIf drpshlocations.SelectedValue = "Location3" Then
                condition = "sublocname" & " like '%" & txtshlocation3.Text.Trim & "%'"
                fillgrdLocation("", "", condition)
            Else
                fillgrdLocation("", "")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drpshlocations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpshlocations.SelectedIndexChanged
        If drpshlocations.SelectedValue = "Location2" Then
            drpshlocation2.Visible = True
            txtshlocation3.Visible = False
        ElseIf drpshlocations.SelectedValue = "Location3" Then
            drpshlocation2.Visible = False
            txtshlocation3.Visible = True
        End If
    End Sub
End Class
