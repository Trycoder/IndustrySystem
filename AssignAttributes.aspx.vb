Imports System.Data
Imports System.Data.SqlClient
Partial Class AssignAttributes
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con2 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim cmd1 As New SqlCommand
    Dim cmd2 As SqlCommand
    Dim rdr As SqlDataReader
    Dim rdr1 As SqlDataReader
    Dim GId As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        GId = Request.QueryString("GId")
        If Not IsPostBack Then
            bindcategory()
        End If
        If GId = "1" Then
            lblgroup.Text = "Asset(s)"
        ElseIf GId = "2" Then
            lblgroup.Text = "Software(s)"
        ElseIf GId = "3" Then
            lblgroup.Text = "Consumable(s)"
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub

    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpcategory.SelectedIndexChanged
        Try
          
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function bindcategory()
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpcategory.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select * from tbl_Asset_CategoryMaster where groupid=" & GId & " order by catcode"
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

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim str, str1, str2 As String
        Try
            con1.Open()
            str1 = "select * from tbl_Asset_TypeMaster where catid=" & drpcategory.SelectedValue
            cmd1.CommandText = str1
            cmd1.Connection = con1
            rdr1 = cmd1.ExecuteReader
            While rdr1.Read
                str2 = "delete from tbl_Asset_Attribute_Details where AssetTypeId =" & rdr1("AssetTypeId")
                con2.Open()
                cmd2 = New SqlCommand(str2, con2)
                cmd2.ExecuteNonQuery()
                cmd2.Dispose()
                con2.Close()
                con.Open()
                str = "select * from tbl_Asset_Attributes where catid=" & drpcategory.SelectedValue
                cmd.CommandText = str
                cmd.Connection = con
                rdr = cmd.ExecuteReader
                While rdr.Read
                    If Request("chk" & rdr1("AssetTypeId") & rdr("AttId")) = "on" Then
                        str2 = "insert into tbl_Asset_Attribute_Details(AssetTypeId,attid,seq) values(" & rdr1("AssetTypeId") & "," & rdr("AttId") & ",0)"
                        con2.Open()
                        cmd2 = New SqlCommand(str2, con2)
                        cmd2.ExecuteNonQuery()
                        con2.Close()
                    End If
                End While
                rdr.Close()
                con.Close()
            End While
            Dim myscript1 As String = "alert('Attributes Assigned Successfully! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            rdr1.Close()
            con1.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try  
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
End Class
