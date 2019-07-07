Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Imports System.IO
Partial Class EditAssetAttributes
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim dtableattributes As DataTable
    Dim primaryfieldname As String = ""
    Protected Sub drpassetno_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassetno.SelectedIndexChanged
        btnsave.Visible = True
        btncancel.Visible = True
        EditAssetDetails()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            bindcategory()
        End If
        EditAssetDetails()
        If Session("Usergroup") <> "1" Then
            btnsave.Enabled = False
        End If

    End Sub
    Private Function bindcategory()
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        drpcategory.Items.Clear()
        drpcategory.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select * from tbl_Asset_CategoryMaster where groupid = 1 order by catcode asc", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpcategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
        If Session("Assets") IsNot Nothing Then
            Dim s() As String = Session("Assets").ToString.Split("|")
            If s.Length > 4 Then
                If drpcategory.Items.FindByValue(s(0)) IsNot Nothing Then
                    drpcategory.SelectedValue = s(0)
                    BindAssetType(drpcategory.SelectedValue)
                Else
                    drpcategory.SelectedIndex = 0
                End If
                If drpAssetType.Items.FindByValue(s(1)) IsNot Nothing Then
                    drpAssetType.SelectedValue = s(1)
                    BindAssetNo(drpAssetType.SelectedValue, drpcategory.SelectedValue)
                Else
                    drpAssetType.SelectedIndex = 0
                End If
                ViewState("Recordcount") = s(2)
                Session("Assets") = ""
            End If
        End If
    End Function
    Private Function BindAssetType(ByVal categoryid As String)
        Try
            Dim sql As String
            drpAssetType.Items.Clear()
            drpAssetType.Items.Add(New ListItem("--Select--", ""))
            sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId=" & categoryid & " order by am.assettypecode"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpAssetType.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub drpAssetType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAssetType.SelectedIndexChanged
        If drpcategory.SelectedValue <> "" Then
            If drpAssetType.SelectedValue <> "" Then
                BindAssetNo(drpAssetType.SelectedValue, drpcategory.SelectedValue)
                drpassetno.SelectedIndex = 0
                tddata.Controls.Clear()
            End If
        End If
    End Sub
    Public Function BindAssetNo(ByVal assettypeid As String, ByVal catid As String)
        Dim sql As String
        dtable = New DataTable
        Try
            sql = "select aa.Fieldorder from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid and  aa.Header = 5 and aa.catid =" & catid
            'sql = "select top 1 " & fieldname & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & assettypeid & " and am.id = ast.Assetid and ast.status ='N' "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            Dim s As String = cmd.ExecuteScalar()
            cmd.Dispose()
            con.Close()
            If Not String.IsNullOrEmpty(s) Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = ""
                sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & assettypeid & " and am.id = ast.Assetid  order by am.att1 asc"
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                drpassetno.Items.Clear()
                drpassetno.Items.Add(New ListItem("--Select--", ""))
                While rdr.Read
                    drpassetno.Items.Add(New ListItem(rdr(s), rdr("id")))
                End While
            End If
        Catch ex As Exception
        Finally
            con.Close()
        End Try
    End Function
    Public Function GetAssetAttributes(Optional ByVal primaryfield As Boolean = False) As DataTable
        Dim sql As String
        Try
            If primaryfield = True Then
                sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpAssetType.SelectedValue & " and aa.Header='5' and aa.Header <>'8' order by aa.attid asc"
            Else
                sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpAssetType.SelectedValue & " and aa.Header <>'8' order by aa.attid asc"
            End If
            Dim dtable As New DataTable
            con1.Open()
            sqladr = New SqlDataAdapter(sql, con1)
            sqladr.Fill(dtable)
            Return dtable
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con1.Close()
        End Try
    End Function
    Private Function BindDropdown(ByVal drpdown As DropDownList, ByVal attid As String)
        Try
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
            Dim cmd As New System.Data.SqlClient.SqlCommand
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim str As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            drpdown.Items.Clear()
            str = "select * from tbl_asset_AttributeList where attid  =" & attid
            cmd.CommandText = str
            cmd.Connection = con
            rdr = cmd.ExecuteReader
            While rdr.Read
                drpdown.Items.Add(New ListItem(rdr("attdesc"), rdr("attdesc")))
            End While
            drpdown.Items.Insert(0, "--Select--")
            con.Close()
            rdr.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Private Function BindListbox(ByVal Lstbox As ListBox, ByVal attid As String)
        Try
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
            Dim cmd As New System.Data.SqlClient.SqlCommand
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim str As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Lstbox.Items.Clear()
            str = "select * from tbl_asset_AttributeList where attid  =" & attid
            cmd.CommandText = str
            cmd.Connection = con
            rdr = cmd.ExecuteReader
            While rdr.Read
                Lstbox.Items.Add(New ListItem(rdr("attdesc"), rdr("attdesc")))
            End While
            con.Close()
            rdr.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpcategory.SelectedIndexChanged
        If drpcategory.SelectedValue <> "" Then
            BindAssetType(drpcategory.SelectedValue)
            drpAssetType.SelectedIndex = 0
            drpassetno.SelectedIndex = 0
            tddata.Controls.Clear()
        End If
    End Sub
    Public Function GetAssetData(ByVal asssettypeid As String, ByVal assetid As String) As DataTable
        Dim sql As String
        sql = "Select top 1 "
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & asssettypeid & " and aa.Header <>'8'  order by aad.attid asc"
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
        sql = sql & " from tbl_Asset_Master where id =" & assetid
        cmd.Dispose()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        con.Close()
        Return dtable
    End Function

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            Dim str As String
            Dim fieldname As String = ""
            Dim values As String = ""
            Dim query As String = ""
            Dim fieldno As Integer = 1
            Dim filecheck As Boolean = False
            dtableattributes = New DataTable
            dtableattributes = GetAssetAttributes()
            Dim seqno As Integer = 0
            Dim seqvalue As String = ""
            Dim PONumber As String = ""
            Dim PODate As String = ""
            Dim WarrentyStart As String = ""
            Dim WarrentyEnd As String = ""
            Dim boolseq As Boolean = False
            Dim txtvariable As Boolean = False
            Dim startassetid As Integer = 0
            Dim endassetid As Integer = 0
            Dim sqlnolength As Integer = 0
            Dim seqno1 As String = ""
            Dim chkassetno As Boolean = False
            Dim assetno As String = ""
            Dim primaryfield As String = ""

            If chkassetno = False Then
                If dtableattributes.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable = CType(tddata.FindControl("strtable1"), Table)
                    Dim txtvar As TextBox
                    Dim txtfix As TextBox
                    Dim txtseq As TextBox
                    Dim txtdate As TextBox
                    Dim lblrnd As Label
                    Dim drpsingle As DropDownList
                    Dim drplist As ListBox
                    Dim radio As RadioButtonList
                    Dim fileupload As FileUpload
                    fieldname = "update tbl_Asset_Master set  "

                    For i As Integer = 0 To dtableattributes.Rows.Count - 1
                        If UCase(dtableattributes(i)("atttype")) = UCase("Text(Variable)") Then
                            txtvar = CType(strtable.FindControl("txtvar" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtvar IsNot Nothing Then
                                values = values & dtableattributes(i)("FieldOrder") & "='" & txtvar.Text & "',"
                            Else
                                values = values & dtableattributes(i)("FieldOrder") & "='',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Text(Fixed)") Then
                            txtfix = CType(strtable.FindControl("txtfix" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtfix IsNot Nothing Then
                                values = values & dtableattributes(i)("FieldOrder") & "='" & txtfix.Text & "',"
                            Else
                                values = values & dtableattributes(i)("FieldOrder") & "='',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Sequence") Then
                            txtseq = CType(strtable.FindControl("txtseq" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtseq IsNot Nothing Then
                                values = values & dtableattributes(i)("FieldOrder") & "='" & txtseq.Text & "',"
                            Else
                                values = values & dtableattributes(i)("FieldOrder") & "='',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Random") Then
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                            drpsingle = CType(strtable.FindControl("drp" & dtableattributes(i)("attid").ToString), DropDownList)
                            If drpsingle IsNot Nothing Then
                                values = values & dtableattributes(i)("FieldOrder") & "='" & drpsingle.SelectedValue & "',"
                            Else
                                values = values & dtableattributes(i)("FieldOrder") & "='',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("MultiSelection") Then
                            drplist = CType(strtable.FindControl("list" & dtableattributes(i)("attid").ToString), ListBox)
                            If drplist IsNot Nothing Then
                                Dim strstring As String = ""
                                For j As Integer = 0 To drplist.Items.Count - 1
                                    If drplist.Items(j).Selected Then
                                        strstring = strstring & drplist.Items(j).Text & "|"
                                    End If
                                Next
                                values = values & dtableattributes(i)("FieldOrder") & "='" & strstring & "',"
                            Else
                                values = values & dtableattributes(i)("FieldOrder") & "='',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Date") Then
                            txtdate = CType(strtable.FindControl("txtdate" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtdate IsNot Nothing Then
                                values = values & dtableattributes(i)("FieldOrder") & "='" & txtdate.Text & "',"
                            Else
                                values = values & dtableattributes(i)("FieldOrder") & "='',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Yes/No") Then
                            radio = CType(strtable.FindControl("rdo" & dtableattributes(i)("attid").ToString), RadioButtonList)
                            If radio IsNot Nothing Then
                                values = values & dtableattributes(i)("FieldOrder") & "='" & radio.SelectedValue & "',"
                            Else
                                values = values & dtableattributes(i)("FieldOrder") & "='',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("FileUpload") Then
                            fileupload = CType(strtable.FindControl("fileupload" & dtableattributes(i)("attid").ToString), FileUpload)
                            If fileupload IsNot Nothing Then
                                If fileupload.HasFile Then
                                    fileupload.SaveAs(Server.MapPath("~/POFiles/") & fileupload.FileName)
                                    values = values & dtableattributes(i)("FieldOrder") & "='" & Path.GetFileName(fileupload.FileName) & "',"
                                Else
                                    values = values & dtableattributes(i)("FieldOrder") & "='',"
                                End If
                            End If
                        Else
                            values = values & dtableattributes(i)("FieldOrder") & "='',"
                        End If
                    Next
                    values = values.Remove(values.Length - 1, 1)
                    query = fieldname & values & " where id=" & drpassetno.SelectedValue
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    cmd = New SqlCommand(query, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    cmd = New SqlCommand("insert into tbl_Asset_Transactions(assetid,TransType,date1,transcreatedid,reasonid,remarks,userid)values(" & drpassetno.SelectedValue & ",'20','" & Today.Date.ToShortDateString & "','" & Session("EmpNo") & "','1','Edit Assets','0000')", con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    cmd.Dispose()
                    lblmessage.ForeColor = Drawing.Color.Peru
                    If assetno.Length > 0 Then
                        assetno = assetno.Remove(assetno.Length - 1, 1)
                    End If
                    lblmessage.Text = "Asset Updated Successfully!"
                    drpassetno.SelectedIndex = 0
                    tddata.Visible = False
                    btnsave.Visible = False
                    btncancel.Visible = False
                End If
            Else
                lblmessage.ForeColor = Drawing.Color.Peru
                If assetno.Length > 0 Then
                    assetno = assetno.Remove(assetno.Length - 1, 1)
                End If
                lblmessage.Text = "This Asset Number(s) is Already Exists. " & vbCrLf & assetno
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function EditAssetDetails()
        Try
            If drpassetno.SelectedValue <> "" Then
                tddata.Visible = True
                tddata.Controls.Clear()
                dtableattributes = New DataTable
                dtableattributes = GetAssetAttributes()
                Dim assetdata As New DataTable
                assetdata = GetAssetData(drpAssetType.SelectedValue, drpassetno.SelectedValue)
                If dtableattributes.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable.ID = "strtable1"
                    strtable.Width = Unit.Percentage(100)
                    For i As Integer = 0 To dtableattributes.Rows.Count - 1
                        Dim strrow As New TableRow
                        Dim strCell_1 As New TableCell
                        Dim strCell_2 As New TableCell
                        Dim strCell_3 As New TableCell
                        strCell_1.Width = Unit.Percentage(50)
                        strCell_2.Width = Unit.Percentage(15)
                        strCell_3.Width = Unit.Percentage(35)
                        strtable.Style("align") = "center"
                        strCell_1.Style("text-align") = "right"
                        strCell_1.Style.Add("padding-right", "2px")
                        strCell_2.Style("text-align") = "left"
                        strCell_3.Style("text-align") = "left"
                        strCell_3.Style("color") = "blue"
                        Dim lbl As New Label
                        lbl.ID = "lbl" & dtableattributes(i)("attid").ToString
                        lbl.Text = dtableattributes(i)("attdesc").ToString & " :"
                        Dim lbltooltip As New Label
                        lbltooltip.ID = "lbltooltip" & dtableattributes(i)("attid").ToString
                        lbltooltip.Text = dtableattributes(i)("atttooltipdesc").ToString
                        strCell_1.Controls.Add(lbl)
                        strrow.Cells.Add(strCell_1)
                        strCell_3.Controls.Add(lbltooltip)
                        If UCase(dtableattributes(i)("atttype")) = UCase("Text(Variable)") Then
                            Dim txtvar As New TextBox
                            txtvar.CssClass = "control"
                            txtvar.Width = Unit.Pixel(150)
                            txtvar.ID = "txtvar" & dtableattributes(i)("attid").ToString
                            If dtableattributes(i)("Header") = "5" Then
                                txtvar.Attributes.Add("readonly", "readonly")
                            End If
                            txtvar.ValidationGroup = "assets"
                            txtvar.Text = ""
                            txtvar.Text = assetdata(0)(i).ToString()
                            strCell_2.Controls.Add(txtvar)
                            If dtableattributes(i)("IsRequired") = 1 And UCase(dtableattributes(i)("AttDesc").ToString()) <> UCase("FINANCE ASSET NO") Then
                                Dim reqvar As New RequiredFieldValidator
                                reqvar.ID = "reqvar" & dtableattributes(i)("attid").ToString
                                reqvar.ControlToValidate = "txtvar" & dtableattributes(i)("attid").ToString()
                                reqvar.ErrorMessage = "Please Enter " & dtableattributes(i)("AttDesc").ToString()
                                reqvar.ValidationGroup = "assets"
                                reqvar.Display = ValidatorDisplay.None
                                reqvar.SetFocusOnError = True
                                strCell_2.Controls.Add(reqvar)
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Text(Fixed)") Then
                            Dim txtfix As New TextBox
                            txtfix.CssClass = "control"
                            txtfix.ID = "txtfix" & dtableattributes(i)("attid").ToString
                            If dtableattributes(i)("Header") = "5" Then
                                txtfix.Attributes.Add("readonly", "readonly")
                            End If
                            txtfix.ValidationGroup = "assets"
                            txtfix.Width = Unit.Pixel(150)
                            txtfix.Text = assetdata(0)(i).ToString()
                            strCell_2.Controls.Add(txtfix)
                            If dtableattributes(i)("IsRequired") = 1 And UCase(dtableattributes(i)("AttDesc").ToString()) <> UCase("FINANCE ASSET NO") Then
                                Dim reqfix As New RequiredFieldValidator
                                reqfix.ID = "reqfix" & dtableattributes(i)("attid").ToString
                                reqfix.ControlToValidate = "txtfix" & dtableattributes(i)("attid").ToString()
                                reqfix.ErrorMessage = "Please Enter " & dtableattributes(i)("AttDesc").ToString()
                                reqfix.ValidationGroup = "assets"
                                reqfix.Display = ValidatorDisplay.None
                                reqfix.SetFocusOnError = True
                                strCell_2.Controls.Add(reqfix)
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Sequence") Then
                            Dim txtseq As New TextBox
                            txtseq.CssClass = "control"
                            txtseq.ID = "txtseq" & dtableattributes(i)("attid").ToString
                            If dtableattributes(i)("Header") = "5" Then
                                txtseq.Attributes.Add("readonly", "readonly")
                            End If
                            txtseq.ValidationGroup = "assets"
                            txtseq.Width = Unit.Pixel(150)
                            txtseq.Text = ""
                            txtseq.Text = assetdata(0)(i).ToString()
                            strCell_2.Controls.Add(txtseq)
                            If dtableattributes(i)("IsRequired") = 1 And UCase(dtableattributes(i)("AttDesc").ToString()) <> UCase("FINANCE ASSET NO") Then
                                Dim reqseq As New RequiredFieldValidator
                                reqseq.ID = "reqseq" & dtableattributes(i)("attid").ToString
                                reqseq.ControlToValidate = "txtseq" & dtableattributes(i)("attid").ToString()
                                reqseq.ErrorMessage = "Please Enter " & dtableattributes(i)("AttDesc").ToString()
                                reqseq.ValidationGroup = "assets"
                                reqseq.Display = ValidatorDisplay.None
                                reqseq.SetFocusOnError = True
                                strCell_2.Controls.Add(reqseq)
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Random") Then
                            Dim lblrnd As New Label
                            lblrnd.CssClass = "control"
                            lblrnd.ID = "lblrnd" & dtableattributes(i)("attid").ToString
                            lblrnd.Width = Unit.Pixel(150)
                            strCell_2.Controls.Add(lblrnd)
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                            Dim drpsingle As New DropDownList
                            drpsingle.CssClass = "control"
                            drpsingle.ID = "drp" & dtableattributes(i)("attid").ToString
                            drpsingle.Width = Unit.Pixel(158)
                            BindDropdown(drpsingle, dtableattributes(i)("attid").ToString)
                            If drpsingle.Items.FindByValue(assetdata(0)(i).ToString()) IsNot Nothing Then
                                drpsingle.SelectedValue = assetdata(0)(i).ToString()
                            Else
                                drpsingle.SelectedIndex = 0
                            End If
                            strCell_2.Controls.Add(drpsingle)
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("MultiSelection") Then
                            Dim drplist As New ListBox
                            drplist.CssClass = "control"
                            drplist.ID = "list" & dtableattributes(i)("attid").ToString
                            drplist.Width = Unit.Pixel(150)
                            drplist.SelectionMode = ListSelectionMode.Multiple
                            BindListbox(drplist, dtableattributes(i)("attid").ToString)
                            Dim s() As String = assetdata(0)(i).ToString().Split("|")
                            If s.Length > 0 Then
                                For Val As Integer = 0 To s.Length - 1
                                    If drplist.Items.FindByValue(s(Val)) IsNot Nothing Then
                                        drplist.Items.FindByValue(s(Val)).Selected = True
                                    End If
                                Next
                            End If
                            strCell_2.Controls.Add(drplist)
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("DATE") Then
                            Dim cal As New CalendarExtender
                            Dim txtdate As New TextBox
                            txtdate.Attributes.Add("readonly", "readonly")
                            txtdate.CssClass = "control"
                            txtdate.ID = "txtdate" & dtableattributes(i)("attid").ToString
                            txtdate.Width = Unit.Pixel(150)
                            txtdate.Text = assetdata(0)(i).ToString()
                            cal.ID = "calex" & dtableattributes(i)("attid").ToString
                            cal.TargetControlID = "txtdate" & dtableattributes(i)("attid").ToString
                            cal.Format = "dd-MMM-yyyy"
                            strCell_2.Controls.Add(txtdate)
                            strCell_2.Controls.Add(cal)
                            If dtableattributes(i)("IsRequired") = 1 Then
                                Dim req As New RequiredFieldValidator
                                req.ID = "reqdate" & dtableattributes(i)("attid").ToString
                                req.ControlToValidate = "txtdate" & dtableattributes(i)("attid").ToString()
                                req.ErrorMessage = "Please Enter " & dtableattributes(i)("AttDesc").ToString()
                                req.ValidationGroup = "assets"
                                req.Display = ValidatorDisplay.None
                                req.SetFocusOnError = True
                                strCell_2.Controls.Add(req)
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("FileUpload") Then
                            Dim filupload As New FileUpload
                            filupload.CssClass = "control"
                            filupload.ID = "fileupload" & dtableattributes(i)("attid").ToString
                            filupload.Width = Unit.Pixel(158)
                            ' filupload = assetdata(0)(i).ToString()
                            strCell_2.Controls.Add(filupload)
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Yes/No") Then
                            Dim radio As New RadioButtonList
                            radio.ID = "rdo" & dtableattributes(i)("attid").ToString
                            radio.Items.Add(New ListItem("Yes", "Yes"))
                            radio.Items.Add(New ListItem("No", "No"))
                            radio.RepeatDirection = RepeatDirection.Vertical
                            If radio.Items.FindByValue(assetdata(0)(i).ToString()) IsNot Nothing Then
                                radio.SelectedValue = assetdata(0)(i).ToString()
                            Else
                                radio.SelectedIndex = 0
                            End If
                            strCell_2.Controls.Add(radio)
                        End If
                        strrow.Cells.Add(strCell_2)
                        strrow.Cells.Add(strCell_3)
                        strtable.Rows.Add(strrow)
                    Next
                    tddata.Controls.Add(strtable)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("Default.aspx")
    End Sub
End Class
