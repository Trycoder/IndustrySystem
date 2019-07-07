Imports System.Data.SqlClient
Imports System.Data
Partial Class ViewReport1
    Inherits System.Web.UI.Page
    Public reportname As String = ""
    Public reportcatid As String = ""
    Public assetcatid As String = ""
    Public assettypes As String = ""
    Public condition As String = ""
    Public usercondition As String = ""
    Public orderby As String = ""
    Public rights As String = ""
    Public reportheaders As String = ""
    Public reportheaderdesc As String = ""
    Public con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Public cmd As SqlCommand
    '--------ViewReport----------
    Public TempOrderBy As String = ""
    Public V_Emp_Bool As Boolean = False
    Public V_Trans_Bool As Boolean = False
    Public SqlStr As String = ""

    Public sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Public sqlcmd As New SqlCommand
    Public drAsset As SqlDataReader
    Public sqlad As New SqlDataAdapter
              
    Public cntrh As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim s() As String = Convert.ToString(Session("Values")).Split("*")
        If s.Length > 8 Then
            reportname = s(0)
            lblreportname.Text = s(0)
            reportcatid = s(1)
            assetcatid = s(2)
            assettypes = s(3)
            condition = s(4)
            usercondition = s(5)
            lblcondition.Text = s(5)
            orderby = s(6)
            rights = s(7)
            reportheaders = s(8)
            reportheaderdesc = s(9)
            ViewState("status") = s(10)

        Else
            Dim myscript As String = "alert('Selected Report Fields are Empty');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
            Exit Sub
        End If
    End Sub
    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            Dim sql As String
            If assettypes = "" Then
                Dim myscript1 As String = "alert('Asset Types are Empty! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                Exit Sub
            End If
            If reportheaders = "" Then
                Dim myscript1 As String = "alert('Report Headers are Empty! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                Exit Sub
            End If


            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()

            sql = "insert into tbl_Asset_reportformula(reportname,repcatid,assetcatid,assettypes,condition,usercondition,groupby,rights,createdby,createddate,reportheaders,reportheaderdesc,groupbydesc,status)"
            sql = sql & " values('" & reportname & "'," & reportcatid & "," & assetcatid & ",'" & assettypes.Replace("'", "''") & "','(" & usercondition.Replace("'", "''") & ")',"
            sql = sql & "'(" & condition.Replace("'", "''") & ") ','" & orderby & "'," & rights & ",'" & Convert.ToString(Session("EmpNo")) & "','" & DateTime.Now.Date & "','" & reportheaders & "', "
            sql = sql & "'" & reportheaderdesc & "','" & orderby & "','v_asset.status  <> ''Sold''')"
            cmd = New SqlCommand(sql, con)
            Try
                cmd.ExecuteNonQuery()
                Session("Message") = "Report Formula Inserted Successfully!"
            Catch ex As Exception
                Session("Message") = "Report Formula Not Inserted Successfully!"
            End Try
            cmd.Dispose()
            con.Close()
            sql.Trim()
            Response.Redirect("ReportFormula.aspx", True)
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("ReportFormula.aspx")
    End Sub
End Class
