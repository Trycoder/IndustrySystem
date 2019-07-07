Imports System.Data
Imports System.Data.SqlClient

Partial Class PrinterTonerMapping
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim ds As New DataSet
    Dim str As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            GetPrinterType()
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub

    Public Sub GetPrinterType()
        con.Open()
        cmd.Connection = con
        ddlPrinter.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select distinct(att11) from tbl_asset_master where assettypeid = 6"
        rdr = cmd.ExecuteReader
        ddlPrinter.Items.Add(New ListItem("--Select--", 0))
        If rdr.HasRows Then
            While rdr.Read
                ddlPrinter.Items.Add(New ListItem(rdr("att11"), rdr("att11")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Sub

    Public Function GetAllTonners() As DataTable
        Dim dtable As New DataTable
        str = "select distinct(att1) from tbl_Asset_ConsumablesMaster where constypeid = 24"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim oAdapter As New SqlDataAdapter(str, con)
        oAdapter.Fill(dtable)
        Return dtable
    End Function

    Public Sub GetTonerType()
        Dim sb As New StringBuilder()
        Dim dtHeader As DataTable = GetAllTonners()

        If dtHeader.Rows.Count > 0 Then
            With sb
                .Append("<table cellpadding='0' cellspacing='0' width='100%' border='0' style='border-collapse:collapse;'>")
                Dim i As Integer = 1
                For Each dr As DataRow In dtHeader.Rows

                    If (i = 1) Then
                        .Append("<tr>")
                    End If

                    .Append("<td>")
                    .Append("<table class='tdcolbg' width='100%' cellpadding='1' cellspacing='1' border='0' style='border-collapse:collapse;'><tr class='trheaderbg'><th>")
                    .Append(dr.Item("att1")).Append("</th></tr>")

                    str = "select distinct(cm.att1),am.assetmodel,am.conmodel from tbl_Asset_Mapping am join tbl_Asset_ConsumablesMaster cm  "
                    str = str & " on  cm.constypeid = am.contypeid and am.conmodel = '" & dr.Item("att1") & "' "
                    str = str & " where am.contypeid = 24 and am.assetmodel = '" & ddlPrinter.SelectedValue & "'"

                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    cmd.CommandText = str
                    cmd.Connection = con
                    rdr = cmd.ExecuteReader
                    .Append("<tr class='whitebg'><td align='center'>")
                    If rdr.HasRows Then
                        .Append("<input type='checkbox' checked ='checked' name='chk" & dr.Item("att1") & "'/>")
                    Else
                        .Append("<input type='checkbox' name='chk" & dr.Item("att1") & "'/>")
                    End If
                    .Append("</td></tr></table></td>")

                    i = i + 1

                    If (i > 6) Then
                        .Append("</tr><tr><td>&nbsp;</td></tr>")
                        i = 1
                    End If

                Next
                .Append("</table>")
            End With
        End If

        divTonerGrid.InnerHtml = sb.ToString()

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dtable As DataTable = GetAllTonners()
        Dim str1 As String
        If dtable.Rows.Count > 0 Then
            str1 = "delete from tbl_Asset_Mapping where contypeid = 24 And assettypeid = 6  and assetmodel = '" & ddlPrinter.SelectedValue & "'"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(str1, con)
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            con.Close()


            For Each dr As DataRow In dtable.Rows
                If Request("chk" & dr("att1")) = "on" Then
                    str1 = "insert into tbl_Asset_Mapping(assettypeid,contypeid,assetmodel,conmodel) values(6,24, '" & ddlPrinter.SelectedValue & "','" & dr("att1") & "')"
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    cmd = New SqlCommand(str1, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    GetTonerType()
                End If
            Next

        End If
        Dim myscript1 As String = "alert('Tonners Mapped Successfully! ');"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
    End Sub

    Protected Sub ddlPrinter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPrinter.SelectedIndexChanged
        If ddlPrinter.SelectedValue <> "" Then
            GetTonerType()
            btnSave.Visible = True
            btnCancel.Visible = True
        End If
    End Sub

End Class
