Imports System.Data
Imports System.Data.SqlClient
Partial Class UserRoles
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim GId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblmessage.Text = ""
        If Session("Msg") IsNot Nothing Then
            trmsg.Visible = True
            lblmessage.Text = Session("Msg")
            Session("Msg") = Nothing
        Else
            trmsg.Visible = False
        End If
        If Not IsPostBack Then
            GetUserGroup()
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub
    Public Function GetUserGroup()
        Dim sql As String = "select * from tbl_asset_usergroup order by groupname"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        drpusergroup.Items.Clear()
        drpusergroup.Items.Add(New ListItem("--Select--", ""))
        If dtable.Rows.Count > 0 Then
            For i As Integer = 0 To dtable.Rows.Count - 1
                drpusergroup.Items.Add(New ListItem(dtable.Rows(i)("groupname").ToString(), dtable.Rows(i)("id").ToString()))
            Next
        End If
    End Function
    Public Function GetMenuMasters() As DataTable
        Dim sql As String = "select * from tbl_asset_menuitems mi where  submenuid = 0 "
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Public Function GetMenuMastersmain() As DataTable
        Dim sql As String = "select mi.id,mi.mainmenuid,mi.menudesc,mi.mtag from tbl_asset_menuitems mi where mi.submenuid = 0  group by mi.mainmenuid,mi.menudesc,mi.id,mi.mtag"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Public Function GetSubMenuList(ByVal menumasterid As String) As DataTable
        Dim sql As String = "select * from tbl_asset_menuitems mi where  mi.mainmenuid = " & menumasterid & ""
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Public Function GetSubMenuListExtn(ByVal menumasterid As String) As DataTable
        Dim sql As String = "select * from tbl_asset_menugroup where menuid = " & menumasterid & " and groupid = " & drpusergroup.SelectedValue
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Public Function GetSubMenuListmain(ByVal menumasterid As String) As DataTable
        Dim sql As String = "select mi.id,mi.mainmenuid,mi.menudesc,mi.mtag from tbl_asset_menuitems mi where mi.submenuid <> 0 and mi.mainmenuid = " & menumasterid
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim con2 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd As SqlCommand
        Dim str1 As String
        Dim dtable As New DataTable
        Dim dtable1 As New DataTable
        Dim asset As Integer = 0
        Dim user As Integer = 0

        If drpusergroup.SelectedValue = "" Then
            Dim errorscript As String = "alert('Please Select the Group');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
            drpusergroup.Focus()
            Exit Sub
        End If
        dtable = GetMenuMastersmain()
        If dtable.Rows.Count > 0 Then
            str1 = " delete from tbl_asset_menugroup where  groupid = " & drpusergroup.SelectedValue & ""
            cmd = New SqlCommand(str1, con)
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            For i As Integer = 0 To dtable.Rows.Count - 1
                If Request("chkfull" & dtable.Rows(i)("id")) = "on" Then
                    str1 = " insert into tbl_asset_menugroup values(" & dtable.Rows(i)("id") & "," & drpusergroup.SelectedValue & ",1)"
                    cmd = New SqlCommand(str1, con)
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                ElseIf Request("chkread" & dtable.Rows(i)("id")) = "on" Then
                    str1 = " insert into tbl_asset_menugroup values(" & dtable.Rows(i)("id") & "," & drpusergroup.SelectedValue & ",2)"
                    cmd = New SqlCommand(str1, con)
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                Else
                    str1 = " insert into tbl_asset_menugroup values(" & dtable.Rows(i)("id") & "," & drpusergroup.SelectedValue & ",0)"
                    cmd = New SqlCommand(str1, con)
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                End If
                dtable1 = GetSubMenuListmain(dtable.Rows(i)("mainmenuid").ToString())
                If dtable1.Rows.Count > 0 Then
                    'str1 = "delete from tbl_asset_menugroup where menuid in (select p.id from tbl_asset_menuitems p join tbl_asset_menugroup a on p.id = a.menuid where a.groupid = " & drpusergroup.SelectedValue & " and p.submenuid <> 0 and p.mainmenuid = " & dtable.Rows(i)("mainmenuid").ToString() & ") and groupid = " & drpusergroup.SelectedValue
                    'cmd = New SqlCommand(str1, con)
                    'cmd.ExecuteNonQuery()
                    'cmd.Dispose()
                    For j As Integer = 0 To dtable1.Rows.Count - 1
                        If Request("chkfull" & dtable1.Rows(j)("id")) = "on" Then
                            str1 = "insert into tbl_asset_menugroup values(" & dtable1.Rows(j)("id") & "," & drpusergroup.SelectedValue & ",1)"
                            cmd = New SqlCommand(str1, con)
                            cmd.ExecuteNonQuery()
                            cmd.Dispose()
                            str1 = "update tbl_asset_menugroup set rights = 1 where menuid = " & dtable.Rows(i)("id").ToString() & " "
                            cmd = New SqlCommand(str1, con)
                            cmd.ExecuteNonQuery()
                            cmd.Dispose()
                        ElseIf Request("chkread" & dtable1.Rows(j)("id")) = "on" Then
                            str1 = " insert into tbl_asset_menugroup values(" & dtable1.Rows(j)("id") & "," & drpusergroup.SelectedValue & ",2)"
                            cmd = New SqlCommand(str1, con)
                            cmd.ExecuteNonQuery()
                            cmd.Dispose()
                            str1 = "update tbl_asset_menugroup set rights = 2 where menuid = " & dtable.Rows(i)("id").ToString() & " "
                            cmd = New SqlCommand(str1, con)
                            cmd.ExecuteNonQuery()
                            cmd.Dispose()
                        Else
                            str1 = " insert into tbl_asset_menugroup values(" & dtable1.Rows(j)("id") & "," & drpusergroup.SelectedValue & ",0)"
                            cmd = New SqlCommand(str1, con)
                            cmd.ExecuteNonQuery()
                            cmd.Dispose()
                        End If
                    Next
                End If
            Next
        End If
        con.Close()
        trmsg.Visible = True
        lblmessage.ForeColor = Drawing.Color.Blue
        lblmessage.Text = "User Rights Set Successfully!"

        'Response.Redirect("userroles.aspx", True)
    End Sub
End Class
