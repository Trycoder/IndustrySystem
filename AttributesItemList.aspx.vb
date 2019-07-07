Imports System.Data
Imports System.Data.SqlClient
Partial Class AttributesItemList
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim attributeid As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("AId") <> Nothing Then
            attributeid = Request.QueryString("AId")
            lblassetname.Text = Request.QueryString("Attname")
        End If
        If Not IsPostBack Then
            txtitemname.Focus()
            GetAssetDetails(attributeid)
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim Sql As String
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd.Connection = con
            Sql = "Select * from tbl_asset_AttributeList where attdesc like '" & Trim(txtitemname.Text) & "' and attid =" & attributeid & ""
            cmd.CommandText = Sql
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                Dim myscript As String = "alert('Item already exists.. ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                txtitemname.Focus()
                Exit Sub
            End If
            con.Close()
            Dim str As String
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            str = "insert into tbl_asset_AttributeList(attid,attdesc) values (" & attributeid & ",'" & txtitemname.Text.Trim & "')"
            cmd.CommandText = str
            cmd.Connection = con
            cmd.ExecuteScalar()
            con.Close()
            txtitemname.Text = ""
            txtitemname.Focus()
            GetAssetDetails(attributeid)
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function GetAssetDetails(ByVal attributeid As String)
        Try
            Dim sql As String
            dtable = New DataTable
            sql = "select * from  tbl_asset_AttributeList atlist join tbl_Asset_Attributes aatt on atlist.attid = aatt.attid where atlist.attid = " & attributeid & " order by atlist.attdesc"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            sqladr = New SqlDataAdapter(sql, con)
            sqladr.Fill(dtable)
            If dtable.Rows.Count > 0 Then
                grdattributes.DataSource = dtable
                grdattributes.DataBind()
            Else
                grdattributes.EmptyDataText = "No data Found"
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub grdattributes_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdattributes.RowDeleting
        Try
            Dim ItemId As Integer = CInt(grdattributes.DataKeys(e.RowIndex).Value.ToString())
            If deleteitem(ItemId) = True Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "alert('Records Deleted Successfully!');", True)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "alert('Records Deleted not Successfully!');", True)
            End If
            GetAssetDetails(attributeid)
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub grdattributes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdattributes.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim db As LinkButton = CType(e.Row.Cells(2).Controls(0), LinkButton)
                db.Attributes.Add("onclick", "javascript:return confirm('Are you Sure want to Delete This Item?'); ")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function deleteitem(ByVal itemid As Integer) As Boolean
        Try
            Dim Sql As String
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd.Connection = con
            Sql = "delete from tbl_asset_AttributeList where id=" & itemid & ""
            cmd.CommandText = Sql
            cmd.ExecuteNonQuery()
            con.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click
        CloseWindow()
    End Sub
    Private Sub CloseWindow()
        Dim sb As New StringBuilder()
        ' sb.Append("window.opener.location.href = window.opener.location.href;")
        sb.Append("window.close();")
        'sb.Append("if (opener && !opener.closed) { ")
        'sb.Append("var x = opener.document.getElementById(" & Request.QueryString("AId") & ");")
        'sb.Append("if (x) {")
        'sb.Append("x.value = 'True';")
        'sb.Append("opener.document.forms[0].submit();")
        'sb.Append("}")
        'sb.Append("}")
        'sb.Append("window.close();")
        'sb.Append("}")
        ClientScript.RegisterClientScriptBlock(Me.[GetType](), "CloseWindowScript", sb.ToString(), True)
        ' Me.RegisterStartupScript("", "<script>window.close();if (window.opener && !window.opener.closed){window.opener.location.reload();}</script>")
    End Sub
End Class
