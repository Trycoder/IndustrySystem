Imports System.Data
Imports System.Data.SqlClient
Partial Class Complaintview
    Inherits System.Web.UI.Page
    Public MC As Integer
    Public Rw As Integer
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim sqlstr As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            sqlstr = "update tbl_hd_consview set ctag = 1 where complaintid=" & Request.QueryString("CId") & " and consid='" & Session("EmpNo") & "'"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlstr, con)
            cmd.ExecuteNonQuery()
            con.Close()
        End If
    End Sub

    Protected Sub CmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSave.Click
        Dim i As Integer
        Dim x As Integer
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim sql As String

        'For i = 0 To Rw
        If Request("ChkSelect1") = "on" Then
            con.Open()
            cmd.Connection = con
            sql = "update tbl_hd_complaint set cons_id ='" & Session("EmpNo") & "' where complaintid =" & Request.QueryString("CId")
            cmd.CommandText = sql
            cmd.ExecuteScalar()
            con.Close()

            con.Open()
            cmd.Connection = con
            sql = "insert into tbl_hd_complaintdet(ctag,description,complaintid,ddate,emp_number) values(1,'Ticket Selected.'," & x & ",'" & Now() & "','" & Session("EmpNo") & "')"
            cmd.CommandText = sql
            cmd.ExecuteScalar()
            con.Close()

            con.Open()
            cmd.Connection = con
            sql = " delete from tbl_hd_consview where complaintid =" & Request.QueryString("Cid") & ""
            cmd.CommandText = sql
            cmd.ExecuteScalar()
            con.Close()

        End If
        ' Next
    End Sub
End Class
