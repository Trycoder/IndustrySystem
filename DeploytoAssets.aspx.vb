Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports AjaxControlToolkit
Imports System.Net.Mail
Partial Class DeploytoAssets
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
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
                'BindConsumables()
                BindAssetType()
            End If
            txtdate.Text = Format(Now(), "dd-MMM-yyyy")
            If UCase(drpConsName.SelectedItem.Text) = UCase("Toner") Then
                If drptonermodel.SelectedValue = "" Then
                    If drpassetno.SelectedValue <> "" AndAlso drpConsName.SelectedValue <> "" Then
                        BindTonerModel()
                    End If
                End If
            End If
            If drpConsName.SelectedItem.Text <> "Toner" And rdoSubLocation.SelectedValue <> "" Then
                GetConsumableDetails()
                trconsumables.Visible = True
            End If
            lblMessage.Text = ""
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function Bindradiobutton(ByVal rdobtn As RadioButtonList, ByVal attid As String, ByVal fieldname As String, ByVal sublocid As String) As Boolean
        Try
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
            Dim cmd As New System.Data.SqlClient.SqlCommand
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim str As String = ""
            con.Open()
            rdobtn.Items.Clear()
            str = "select distinct " & fieldname & " from tbl_Asset_Cons_Stock where ConsTypeId  =" & attid & " and sublocid=" & sublocid
            cmd.CommandText = str
            cmd.Connection = con
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    rdobtn.Items.Add(New ListItem(rdr("" & fieldname & ""), rdr("" & fieldname & "")))
                End While
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    'Private Sub BindConsumableType(ByVal consid As String)
    '    Try
    '        Dim sql As String = ""
    '        sql = "select distinct conmodel from tbl_Asset_Mapping where contypeid=" & consid
    '        con.Open()
    '        drptonermodel.Items.Clear()
    '        drptonermodel.Items.Add(New ListItem("--Select--", ""))
    '        cmd = New SqlCommand(sql, con)
    '        rdr = cmd.ExecuteReader
    '        If rdr.HasRows Then
    '            While rdr.Read
    '                drptonermodel.Items.Add(New ListItem(rdr("conmodel"), rdr("conmodel")))
    '            End While
    '        End If
    '        rdr.Close()
    '        con.Close()
    '    Catch ex As Exception
    '        Errorlog.WriteLogFile(Me.[GetType]().Name, "BindConsumableType()", ex.Message.ToString())
    '    End Try
    'End Sub
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

    Private Sub InsertTransactions()
        Dim SQL As String
        Dim userID As Integer
        'To Get the User
        userID = Session("EmpNo")
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        'Transaction Type - 2 = Consumable Deployed to Asset
        SQL = "insert into tbl_Asset_Cons_Transactions(ConsTypeId,TransType,Date1,AssetId,Remarks,TransBy,status) values ('" & drpConsName.SelectedValue
        SQL = SQL & "','" & 2 & "','" & Today.Date & "','" & drpassetno.SelectedValue & "','" & Trim(txtremarks.Text.Replace("'", "''")) & "','" & userID & "','R')"
        cmd = New SqlCommand(SQL, con)
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub
    Private Sub ResetFormControlValues(ByVal parent As Control)
        For Each c As Control In parent.Controls
            If c.Controls.Count > 0 Then
                ResetFormControlValues(c)
            Else
                Select Case (c.GetType().ToString())
                    Case "System.Web.UI.WebControls.TextBox"
                        CType(c, TextBox).Text = ""
                    Case "System.Web.UI.WebControls.CheckBox"
                        CType(c, CheckBox).Checked = False
                    Case "System.Web.UI.WebControls.DropDownList"
                        CType(c, DropDownList).SelectedIndex = 0
                    Case "System.Web.UI.WebControls.RadioButtonList"
                        CType(c, RadioButtonList).Visible = False
                    Case "System.Web.UI.WebControls.RadioButton"
                        CType(c, RadioButton).Checked = False
                End Select
            End If
        Next c
    End Sub
    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("Default.aspx")
    End Sub
    Protected Sub drpConsName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpConsName.SelectedIndexChanged
        Try
            'To Display Location
            Dim sql As String
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            sql = ""
            sql = "select loc.locid as LocID,loc.locname AS LocationName,subloc.sublocid as SubLocID ,subloc.sublocname AS SubLocationName,st.ConsTypeId  from tbl_Asset_Cons_Stock st"
            sql = sql & " left join tbl_Asset_location loc on st.locid = loc.locid left join tbl_Asset_sublocation subloc on st.sublocid = subloc.sublocid where st.constypeid = '" & drpConsName.SelectedValue & "' "
            sqladr = New SqlDataAdapter(sql, con)
            dtable = New DataTable
            sqladr.Fill(dtable)
            If dtable.Rows.Count > 0 Then
                rdoSubLocation.Items.Clear()
                For Each dr As DataRow In dtable.Rows
                    If rdoSubLocation.Items.FindByValue(dr("SubLocID")) IsNot Nothing Then
                    Else
                        rdoSubLocation.Items.Add(New ListItem(dr("LocationName") & "( " & dr("SubLocationName") & " )", dr("SubLocID")))
                    End If
                Next
            Else
                rdoSubLocation.Items.Clear()
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            ' Validate the controls
            If drpConsName.SelectedValue = "" Then
                Dim myscript As String = "alert('Select Consumable Name');"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                drpConsName.Focus()
                Exit Sub
            End If
            If drptonermodel.SelectedItem.Text = "" Then
                Dim myscript As String = "alert('Select Asset Type');"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                drptonermodel.Focus()
                Exit Sub
            End If
            If drpassetno.SelectedValue = "" Then
                Dim myscript As String = "alert('Select AssetNo');"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                drpassetno.Focus()
                Exit Sub
            End If
            If drpConsName.SelectedValue <> "" Then
                If rdoSubLocation.SelectedValue = "" Then
                    Dim myscript As String = "alert('There is No SubLocation for this Consumable');"
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                    drpConsName.Focus()
                    Exit Sub
                End If
            Else
                If rdoSubLocation.SelectedValue = "" Then
                    Dim myscript As String = "alert('Select Sub Location Name');"
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                    rdoSubLocation.Focus()
                    Exit Sub
                End If
            End If
            If lblQuantity.Text = "0" Then
                Dim myscript As String = "alert('" & drpConsName.SelectedItem.Text & " Not Available In the Stock');"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                Exit Sub
            End If
            ' Update the Stock List
            If UCase(drpConsName.SelectedItem.Text) = UCase("Toner") Then
                UpdateConsumableTonerStock()
            Else
                UpdateConsumableStock()
            End If
            'Insert Asset Transactions
            InsertTransactions()
            'Reset All controls
            ResetFormControlValues(Me)
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function GetConsumableTotal(ByVal ConsumableTypeId As String, ByVal SublocationId As String)
        Try
            If UCase(drpConsName.SelectedItem.Text) = UCase("Toner") Then
                'To Display Quantity
                Dim sql As String = ""
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = Data.CommandType.Text
                Dim fieldorder As String = ""
                cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header ='7'  and aad.AssetTypeId = " & drpConsName.SelectedValue & " order by aad.attid asc"
                rdr = cmd.ExecuteReader
                Dim fieldname As String = ""
                If rdr.HasRows Then
                    While rdr.Read
                        fieldname = rdr("FieldOrder")
                    End While
                End If
                con.Close()
                rdr.Close()
                Dim SQLQty As String
                If drptonermodel.SelectedItem.Text = "" Then
                    SQLQty = "select atm.AssetTypeCode as ConsumableName,atm.AssetTypeId as Consumableid, sum(acs.quantity) as Quantity from tbl_Asset_CategoryMaster acm,tbl_Asset_TypeMaster atm,tbl_Asset_Cons_Stock acs where(acm.catid = atm.catid And atm.AssetTypeId = acs.ConsTypeId And acm.groupid = 3) and atm.AssetTypeId = '" & ConsumableTypeId & "' and acs.sublocid=" & SublocationId & " group by atm.AssetTypeCode,atm.AssetTypeId"
                Else
                    SQLQty = "select atm.AssetTypeCode as ConsumableName,atm.AssetTypeId as Consumableid, sum(acs.quantity) as Quantity from tbl_Asset_CategoryMaster acm,tbl_Asset_TypeMaster atm,tbl_Asset_Cons_Stock acs where(acm.catid = atm.catid And atm.AssetTypeId = acs.ConsTypeId And acm.groupid = 3) and atm.AssetTypeId = '" & ConsumableTypeId & "' and acs.sublocid=" & SublocationId & " and " & fieldname & "='" & drptonermodel.SelectedItem.Text & "' group by atm.AssetTypeCode,atm.AssetTypeId"
                End If
                con.Open()
                cmd = New SqlCommand(SQLQty, con)
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    While rdr.Read
                        lblQuantity.Text = rdr("Quantity")
                    End While
                Else
                    lblQuantity.Text = "0"
                End If
                rdr.Close()
                con.Close()
            Else
                Dim SQLQty As String
                SQLQty = "select atm.AssetTypeCode as ConsumableName,atm.AssetTypeId as Consumableid, sum(acs.quantity) as Quantity from tbl_Asset_CategoryMaster acm,tbl_Asset_TypeMaster atm,tbl_Asset_Cons_Stock acs where(acm.catid = atm.catid And atm.AssetTypeId = acs.ConsTypeId And acm.groupid = 3) and atm.AssetTypeId = '" & ConsumableTypeId & "' and acs.sublocid=" & SublocationId & " group by atm.AssetTypeCode,atm.AssetTypeId"
                con.Open()
                cmd = New SqlCommand(SQLQty, con)
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    While rdr.Read
                        lblQuantity.Text = rdr("Quantity")
                    End While
                Else
                    lblQuantity.Text = "0"
                End If
                rdr.Close()
                con.Close()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function BindAssetNo(ByVal assettypeid As String)
        Try
            Dim Sql As String = ""
            Sql = ""
            Sql = "select id,"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim fieldorder As String = ""
            cmd = New SqlCommand("select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header ='5'  and aad.AssetTypeId = " & assettypeid & " order by aad.attid asc", con)
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
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function GetAssetAttributes() As DataTable
        Dim sql As String
        Try
            sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpConsName.SelectedValue & " order by aa.attid asc"
            Dim dtable As New DataTable
            con.Open()
            sqladr = New SqlDataAdapter(sql, con)
            sqladr.Fill(dtable)
            Return dtable
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function

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
    Protected Sub drpassettype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassettype.SelectedIndexChanged
        Try
            If drpassettype.SelectedValue <> "" Then
                BindConsumables()
                BindAssetNo(drpassettype.SelectedValue)
                If UCase(drpassettype.SelectedItem.Text) = UCase("Printer") Then
                    trtoner.Visible = True
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindTonerModel()
        Try
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
                    drptonermodel.Items.Add(New ListItem(rdr("conmodel"), rdr("conmodel")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub rdoSubLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoSubLocation.SelectedIndexChanged
        Try
            If drpConsName.SelectedValue <> "" Then
                GetConsumableTotal(drpConsName.SelectedValue, rdoSubLocation.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function UpdateConsumableTonerStock()
        Try
            Dim sql As String = ""
            sql = "select "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand
            cmd.Connection = con
            cmd.CommandType = Data.CommandType.Text
            Dim FieldName As String = ""
            cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header ='7'  and aad.AssetTypeId = " & drpConsName.SelectedValue & " order by aad.attid asc"
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    FieldName = rdr("FieldOrder")
                End While
            Else
                FieldName = ""
            End If
            rdr.Close()
            con.Close()
            con.Open()
            sql = "update tbl_Asset_Cons_Stock set quantity =" & CInt(lblQuantity.Text) - 1 & " where " & FieldName & "='" & drptonermodel.SelectedItem.Text & "' and ConsTypeId=" & drpConsName.SelectedValue & " and  sublocid=" & rdoSubLocation.SelectedValue & ""
            cmd = New SqlCommand(sql, con)
            cmd.ExecuteNonQuery()
            lblMessage.Text = "Consumable Successfully Deployed to the Asset -" & drpassetno.SelectedItem.Text
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function UpdateConsumableStock()
        Try
            Dim fields As String = "select * from tbl_Asset_Cons_Stock where "
            Dim upfields As String = "update tbl_Asset_Cons_Stock set quantity =" & CInt(lblQuantity.Text) - 1 & " where "
            Dim values As String = ""
            If rdoSubLocation.SelectedValue <> "" Then
                dtableattributes = New DataTable
                dtableattributes = GetAssetAttributes()
                If dtableattributes.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable = CType(tdconsumables.FindControl("strtable1"), Table)
                    Dim rdo As RadioButtonList
                    For i As Integer = 0 To dtableattributes.Rows.Count - 1
                        If UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                            rdo = CType(strtable.FindControl("rdo" & dtableattributes(i)("attid").ToString), RadioButtonList)
                            If rdo IsNot Nothing Then
                                values = values & " " & dtableattributes(i)("FieldOrder") & "='" & rdo.SelectedItem.Text & "' and"
                            End If
                        End If
                    Next
                End If
            End If
            If values <> "" Then
                fields = fields & " " & values & "  ConsTypeId = " & drpConsName.SelectedValue & " And sublocid = " & rdoSubLocation.SelectedValue & ""
                upfields = upfields & " " & values & "  ConsTypeId = " & drpConsName.SelectedValue & " And sublocid = " & rdoSubLocation.SelectedValue & ""
            End If
            Dim sql As String = ""
            Dim result As Boolean = True
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(fields, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If lblQuantity.Text <> "0" Then
                    con1.Open()
                    cmd1 = New SqlCommand(upfields, con1)
                    cmd1.ExecuteNonQuery()
                    result = True
                    con1.Close()
                Else
                    Dim myscript As String = "alert('Consumable Not Available In this SubLocation!');"
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                    Exit Function
                End If
            Else
                Dim myscript As String = "alert('Consumable Not Available In this SubLocation!');"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                Exit Function
            End If
            rdr.Close()
            con.Close()
            If result = True Then
                lblMessage.Text = "Consumable Successfully Deployed to the Asset -" & drpassetno.SelectedItem.Text
            Else
                lblMessage.Text = "Consumable Cannot be Deployed for the Asset.!"
            End If

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
            message.Subject = "Consumable(" & drpConsName.SelectedItem.Text & ") Deployed to the Asset -" & drpassetno.SelectedItem.Text
            message.Body = "The Consumable(" & drpConsName.SelectedItem.Text & ") Has been Deployed to the Asset -" & drpassetno.SelectedItem.Text
            Dim client As New SmtpClient()
            client.Send(message)
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
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
    Public Function GetConsumableDetails()
        Try
            If rdoSubLocation.SelectedValue <> "" Then
                dtableattributes = New DataTable
                dtableattributes = GetAssetAttributes()
                If dtableattributes.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable.ID = "strtable1"
                    strtable.Width = Unit.Percentage(100)
                    For i As Integer = 0 To dtableattributes.Rows.Count - 1
                        Dim strrow As New TableRow
                        Dim strCell_1 As New TableCell
                        Dim strCell_2 As New TableCell
                        strCell_1.Width = Unit.Percentage(20)
                        strCell_2.Width = Unit.Percentage(80)
                        strtable.Style("align") = "center"
                        strCell_1.Style("text-align") = "right"
                        ' strCell_2.Style("text-align") = "center"
                        ' strCell_2.Style("font-weight") = "bold"
                        strCell_1.Style.Add("padding-right", "2px")
                        strCell_2.Style("text-align") = "left"
                        Dim lbl As New Label
                        'Dim lbl2 As New Label
                        'lbl2.ID = "lblsy" & dtableattributes(i)("attid").ToString
                        'lbl2.Text = ":"
                        strCell_1.Controls.Add(lbl)
                        ' strCell_2.Controls.Add(lbl2)
                        strrow.Cells.Add(strCell_1)
                        '  strrow.Cells.Add(strCell_2)
                        Dim s As Boolean = False
                        If UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                            Dim rdosingle As New RadioButtonList
                            rdosingle.CssClass = "control"
                            rdosingle.RepeatDirection = WebControls.RepeatDirection.Horizontal
                            rdosingle.RepeatColumns = "5"
                            rdosingle.ID = "rdo" & dtableattributes(i)("attid").ToString
                            s = Bindradiobutton(rdosingle, drpConsName.SelectedValue, dtableattributes(i)("FieldOrder").ToString, rdoSubLocation.SelectedValue)
                            lbl.ID = "lbl" & dtableattributes(i)("attid").ToString
                            lbl.Text = dtableattributes(i)("attdesc").ToString & " :"
                            strCell_2.Controls.Add(rdosingle)
                            'rdosingle.SelectedIndex = 0
                        End If
                        strrow.Cells.Add(strCell_2)
                        If s = True Then
                            strtable.Rows.Add(strrow)
                        End If
                    Next
                    tdconsumables.Controls.Add(strtable)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
End Class
