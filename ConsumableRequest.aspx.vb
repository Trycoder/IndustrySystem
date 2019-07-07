Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Net.Mail
Partial Class ConsumableRequest
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim dtableattributes As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
            If imgbtn IsNot Nothing Then
                imgbtn.Focus()
            End If
            If Not IsPostBack Then
                BindAssetType()
                BindUsers()
                rdorequest.SelectedIndex = 0
            End If
            If drptonermodel.SelectedValue = "" Then
                If drpassetno.SelectedValue <> "" AndAlso drpConsName.SelectedValue <> "" Then
                    BindTonerModel()
                End If
            End If
            lblMessage.Text = ""
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Sub BindConsumables()
        Try
            Dim sql As String
            sql = "select AM.AssetTypeId,AM.AssetTypeCode from tbl_Asset_TypeMaster am where am.assettypeid in (select acm.constypeid from tbl_Asset_cons_Mapping acm where acm.assettypeid = '" & drpassettype.SelectedValue & "')"
            con.Open()
            drpConsName.Items.Clear()
            drpConsName.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpConsName.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function BindAssetType()
        Try
            Dim sql As String
            sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where ac.groupid=1 order by am.CatId"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            drpassettype.Items.Clear()
            drpassettype.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpassettype.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function BindTonerModel()
        Dim sql As String = ""
        sql = "select "
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandType = Data.CommandType.Text
        Dim fieldorder As String = ""
        cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header ='7'  and aad.AssetTypeId = " & drpassettype.SelectedValue & " order by aad.attid asc"
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                sql = sql & rdr("FieldOrder") & " as [" & rdr("AttDesc") & "],"
            End While
        Else
            sql = sql & "* "
        End If
        rdr.Close()
        con.Close()
        sql = Left(sql, Len(sql) - 1)
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        sql = sql & " from tbl_Asset_Master where id =" & drpassetno.SelectedValue & ""
        cmd = New SqlCommand(sql, con)
        Dim pmodel As String = Convert.ToString(cmd.ExecuteScalar)
        con.Close()

        drptonermodel.Items.Clear()
        sql = "select distinct conmodel,contypeid from tbl_Asset_Mapping where assetmodel='" & pmodel & "'"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        drptonermodel.Items.Add(New ListItem("--Select--", ""))
        If rdr.HasRows Then
            While rdr.Read
                drptonermodel.Items.Add(New ListItem(rdr("conmodel"), rdr("contypeid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Protected Sub drpassettype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassettype.SelectedIndexChanged
        If drpassettype.SelectedValue <> "" Then
            BindAssetNo(drpassettype.SelectedValue)
            BindConsumables()
        End If
    End Sub
    Public Function BindAssetNo(ByVal assettypeid As String)
        Dim Sql As String = ""
        Sql = ""
        Sql = "select id,"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandType = Data.CommandType.Text
        Dim fieldorder As String = ""
        cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header ='5'  and aad.AssetTypeId = " & assettypeid & " order by aad.attid asc"
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                Sql = Sql & rdr("FieldOrder") & " as [" & rdr("AttDesc") & "],"
            End While
        Else
            Sql = Sql & "* "
        End If
        rdr.Close()
        con.Close()
        Sql = Left(Sql, Len(Sql) - 1)
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Sql = Sql & " from tbl_Asset_Master where AssetTypeid =" & assettypeid & ""
        cmd = New SqlCommand(Sql, con)
        rdr = cmd.ExecuteReader
        drpassetno.Items.Clear()
        drpassetno.Items.Add(New ListItem("--Select--", ""))
        If rdr.HasRows Then
            While rdr.Read
                drpassetno.Items.Add(New ListItem(rdr(1), rdr("id")))
            End While
        Else
            Sql = Sql & "* "
        End If
        rdr.Close()
        con.Close()
    End Function

    Protected Sub drpassetno_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassetno.SelectedIndexChanged
        If drpassetno.SelectedValue <> "" AndAlso drpConsName.SelectedValue <> "" Then
            BindTonerModel()
        End If
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            If drpassettype.SelectedValue = "" Then
                trmessage.Visible = True
                lblMessage.Text = "please Select Asset Type!"
                drpassettype.Focus()
                Exit Sub
            End If
            If drpassetno.SelectedValue = "" Then
                trmessage.Visible = True
                lblMessage.Text = "please Select Asset No!"
                drpassetno.Focus()
                Exit Sub
            End If
            If drpConsName.SelectedValue = "" Then
                trmessage.Visible = True
                lblMessage.Text = "please Select Consumable!"
                drpConsName.Focus()
                Exit Sub
            End If
            If drptonermodel.SelectedValue = "" Then
                trmessage.Visible = True
                lblMessage.Text = "please Select Consumable Model!"
                drptonermodel.Focus()
                Exit Sub
            End If

            'Store Transactions 
            InsertTransactions()
            'Send Request Mail
            SendEmailToAdmin()
            'Insert the Helpdesk compliants. 
            Dim myscript As String = "alert('Your Request Has been sent to Admin...!');"
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
            drptonermodel.SelectedIndex = 0
            drpassettype.SelectedIndex = 0
            drpConsName.SelectedIndex = 0
            drpassetno.SelectedIndex = 0
            txtremarks.Text = ""
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function GetFromEmail(ByVal EmpNo As String) As String
        Try
            Dim returnvalue As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand("select Emp_EMail from view_SIP_Employees where Emp_Number='" & EmpNo & "'", con)
            returnvalue = Convert.ToString(cmd.ExecuteScalar())
            con.Close()
            Return returnvalue
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function SendEmailToAdmin()
        Try
            Dim message As New MailMessage()
            If Session("EmpNo") <> "" Then
                message.From = (New MailAddress(GetFromEmail(Session("EmpNo"))))
            Else
                message.From = (New MailAddress(""))
            End If
            If ConfigurationManager.AppSettings("ToAddress") <> "" Then
                message.To.Add(New MailAddress(ConfigurationManager.AppSettings("ToAddress")))
            End If
            If ConfigurationManager.AppSettings("CCAddress1") <> "" Then
                message.CC.Add(New MailAddress(ConfigurationManager.AppSettings("CCAddress1")))
            End If
            If ConfigurationManager.AppSettings("CCAddress2") <> "" Then
                message.CC.Add(New MailAddress(ConfigurationManager.AppSettings("CCAddress2")))
            End If
            message.Subject = drpassetno.SelectedItem.Text & "- wants to the " & drpConsName.SelectedItem.Text & "-" & drptonermodel.SelectedItem.Text
            message.Body = Server.HtmlEncode(txtremarks.Text.Trim())
            Dim client As New SmtpClient()
            client.Send(message)
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub drpConsName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpConsName.SelectedIndexChanged
        If drpConsName.SelectedItem.Text = "Toner" Then
            lblconstitle.Visible = True
            drptonermodel.Visible = True
        Else
            lblconstitle.Visible = False
            drptonermodel.Visible = False
        End If
    End Sub
    Private Sub InsertTransactions()
        Try
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
            Dim cmd As New System.Data.SqlClient.SqlCommand
            Dim SQL As String
            Dim userID As Integer
            Dim AsgQtycnt As Integer = 1

            'To Get the User
            userID = Session("EmpNo")
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            'Transaction Type - 4 = Request Consumables to Admin
            If rdorequest.SelectedValue = "Asset" Then
                SQL = "insert into tbl_Asset_Cons_Transactions(ConsTypeId,TransType,Date1,AssetId,Remarks,TransBy,status) values ('" & drpConsName.SelectedValue
                SQL = SQL & "','" & 4 & "','" & Today.Date & "','" & drpassetno.SelectedValue & "','" & Trim(txtremarks.Text.Replace("'", "''")) & "','" & userID & "','R')"
            ElseIf rdorequest.SelectedValue = "User" Then
                SQL = "insert into tbl_Asset_Cons_Transactions(ConsTypeId,TransType,Date1,UserId,Remarks,TransBy,status) values ('" & drpConsName.SelectedValue
                SQL = SQL & "','" & 4 & "','" & Today.Date & "','" & cbouser.SelectedValue & "','" & Trim(txtremarks.Text.Replace("'", "''")) & "','" & userID & "','R')"
            End If
            cmd.Connection = con
            cmd.CommandText = SQL
            cmd.ExecuteNonQuery()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub rdorequest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdorequest.SelectedIndexChanged
        If rdorequest.SelectedValue = "Asset" Then
            lblassetno.Text = "Asset No :"
            drpassetno.Visible = True
            cbouser.Visible = False
        ElseIf rdorequest.SelectedValue = "User" Then
            lblassetno.Text = "User Name :"
            drpassetno.Visible = False
            cbouser.Visible = True
        End If
    End Sub
    Public Sub BindUsers()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        cbouser.Items.Clear()
        cbouser.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select * from view_SIP_Employees order by Emp_Name", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                cbouser.Items.Add(New ListItem(rdr("Emp_Name") & " " & rdr("Emp_Initial") & "( " & rdr("Emp_Number") & " )", rdr("Emp_Number")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Sub
End Class
