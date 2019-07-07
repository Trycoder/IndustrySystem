Imports System.Data.SqlClient
Imports System.Data
Partial Class LocationsubMaster
    Inherits System.Web.UI.Page
    Dim sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim sqldr As SqlDataReader
    Dim sqlad As SqlDataAdapter
    Dim sqlcmd As New SqlCommand
    Dim ds As New DataSet
    Dim sql As String
    Dim intSublocid As Integer
    Dim locname As String
    Shared sortExpression As String
    Dim dtable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtloc2.Focus()
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dsLoc As New DataSet
        If btnSave.Text = "Save" Then
            sql = "SELECT * FROM tbl_Asset_location WHERE loccatid = '" & drploc1.SelectedValue & "' and locname = '" & Trim(txtloc2.Text) & "'"
            sqlcon.Open()
            sqlad = New SqlDataAdapter(sql, sqlcon)
            sqlad.Fill(dsLoc)
            If dsLoc.Tables(0).Rows.Count > 0 Then
                Dim myscript As String = "alert('Location Name Already Exists for this Location.');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                txtloc2.Focus()
                sqlcon.Close()
                Exit Sub
            End If
            sqlcon.Close()
            dsLoc.Dispose()
            sql = "INSERT INTO tbl_Asset_location(loccatid,locname) VALUES(" & drploc1.SelectedValue & ",'" & Trim(txtloc2.Text) & "')"
            sqlcon.Open()
            sqlcmd.Connection = sqlcon
            sqlcmd.CommandType = CommandType.Text
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()
            sqlcon.Close()
            Dim myscript1 As String = "alert('Location Name Added Successfuly.');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            txtloc2.Text = ""
            drploc1.SelectedIndex = 0
            If drploc1.SelectedValue <> "" Then
                GetLocations(drploc1.SelectedValue)
            End If
        Else
            Dim myscript As String = "alert('Location Name or Sub Location Name can not be null.');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
            Exit Sub
        End If
    End Sub
    Private Function GetLocations(ByVal loccatid As String)
        Try
            Dim sql As String
            dtable = New DataTable
            sql = "select * from tbl_Asset_location where loccatid='" & loccatid & "'"
            If sqlcon.State = ConnectionState.Open Then
                sqlcon.Close()
            End If
            sqlcon.Open()
            sqlad = New SqlDataAdapter(sql, sqlcon)
            sqlad.Fill(dtable)
            If dtable.Rows.Count > 0 Then
                grdlocations.DataSource = dtable
                grdlocations.DataBind()
            Else
                grdlocations.EmptyDataText = "No data Found"
            End If
            sqlcon.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub drploc1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drploc1.SelectedIndexChanged
        If drploc1.SelectedValue <> "" Then
            GetLocations(drploc1.SelectedValue)
        End If
    End Sub

    Protected Sub grdlocations_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdlocations.RowCommand
        Try
            If e.CommandName = "Deleterow" Then
                Dim Locid As String = e.CommandArgument
                If sqlcon.State = ConnectionState.Open Then
                    sqlcon.Close()
                End If
                sqlcon.Open()
                sqlcmd = New SqlCommand("Select count(locid) from tbl_asset_transactions where locid=" & Locid, sqlcon)
                Dim str As String = sqlcmd.ExecuteScalar
                sqlcon.Close()
                sqlcmd.Dispose()
                If str <> "0" Then
                    Dim myscript As String = "alert('Location Mapped with Transactions');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                Else
                    sqlcon.Open()
                    sqlcmd = New SqlCommand("delete from tbl_Asset_location where locid=" & Locid, sqlcon)
                    sqlcmd.ExecuteNonQuery()
                    sqlcon.Close()
                    If drploc1.SelectedValue <> "" Then
                        GetLocations(drploc1.SelectedValue)
                    End If
                    Dim myscript As String = "alert('Location Deleted Successfully!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                End If
            End If

        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub grdlocations_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdlocations.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("Usergroup") <> "1" Then
                Dim imgbtn As ImageButton = CType(e.Row.FindControl("imgdelete"), ImageButton)
                If imgbtn IsNot Nothing Then
                    imgbtn.Enabled = False
                    imgbtn.ImageUrl = "images/dash.jpg"
                End If
            End If
        End If
    End Sub
End Class
