Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports AjaxControlToolkit
Partial Class AddConsumables
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim dtableattributes As DataTable
    Dim dtableassetattributes As DataTable
    'Dim rowid As String
    'Dim GId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
            If imgbtn IsNot Nothing Then
                imgbtn.Focus()
            End If
            'rowid = Request.QueryString("Id")
            'GId = Request.QueryString("GId")
            If Not IsPostBack Then
                bindcategory()
                bindLocation()
            End If
            tddata.Visible = True
            trlocation.Visible = False
            lblMessage.Text = ""
            If drpCategory.SelectedValue <> "" Then
                If drpConsType.SelectedValue <> "" Then
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
                            Dim strCell_3 As New TableCell
                            strCell_1.Width = Unit.Percentage(50)
                            strCell_2.Width = Unit.Percentage(16)
                            strCell_3.Width = Unit.Percentage(34)
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
                            strCell_3.Controls.Add(lbltooltip)
                            strrow.Cells.Add(strCell_1)
                            If UCase(dtableattributes(i)("atttype")) = UCase("Text(Variable)") Then
                                Dim txtvar As New TextBox
                                txtvar.CssClass = "control"
                                txtvar.ID = "txtvar" & dtableattributes(i)("attid").ToString
                                txtvar.Width = Unit.Pixel(150)
                                txtvar.ValidationGroup = "assets"
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
                                txtfix.Width = Unit.Pixel(150)
                                txtfix.ValidationGroup = "assets"
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
                                txtseq.Width = Unit.Pixel(150)
                                txtseq.ValidationGroup = "assets"
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
                            ElseIf UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                                Dim drpsingle As New DropDownList
                                drpsingle.CssClass = "control"
                                drpsingle.ID = "drp" & dtableattributes(i)("attid").ToString
                                drpsingle.Width = Unit.Pixel(158)
                                BindDropdown(drpsingle, dtableattributes(i)("attid").ToString)
                                strCell_2.Controls.Add(drpsingle)
                                drpsingle.AutoPostBack = True
                                AddHandler drpsingle.SelectedIndexChanged, AddressOf drpmodel_SelectedIndexChanged
                                ViewState("Model") = drpsingle.SelectedValue
                            ElseIf UCase(dtableattributes(i)("atttype")) = UCase("MultiSelection") Then
                                Dim drplist As New ListBox
                                drplist.CssClass = "control"
                                drplist.ID = "list" & dtableattributes(i)("attid").ToString
                                drplist.Width = Unit.Pixel(158)
                                drplist.SelectionMode = ListSelectionMode.Multiple
                                BindListbox(drplist, dtableattributes(i)("attid").ToString)
                                strCell_2.Controls.Add(drplist)
                            ElseIf UCase(dtableattributes(i)("atttype")) = UCase("DATE") Then
                                Dim cal As New CalendarExtender
                                Dim txtdate As New TextBox
                                txtdate.Attributes.Add("readonly", "readonly")
                                txtdate.CssClass = "control"
                                txtdate.ID = "txtdate" & dtableattributes(i)("attid").ToString
                                txtdate.Width = Unit.Pixel(150)
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
                                strCell_2.Controls.Add(filupload)
                            ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Yes/No") Then
                                Dim radio As New RadioButtonList
                                radio.ID = "rdo" & dtableattributes(i)("attid").ToString
                                radio.Items.Add(New ListItem("Yes", "Yes"))
                                radio.Items.Add(New ListItem("No", "No"))
                                radio.RepeatDirection = RepeatDirection.Vertical
                                radio.SelectedIndex = 0
                                strCell_2.Controls.Add(radio)
                            ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Text(Number)") Then
                                Dim txtvar As New TextBox
                                txtvar.CssClass = "control"
                                txtvar.ID = "txtvar" & dtableattributes(i)("attid").ToString
                                txtvar.Width = Unit.Pixel(150)
                                txtvar.ValidationGroup = "assets"
                                strCell_2.Controls.Add(txtvar)
                                Dim filtertxt As New FilteredTextBoxExtender
                                filtertxt.ID = "filter" & dtableattributes(i)("attid").ToString
                                filtertxt.FilterType = FilterTypes.Numbers
                                filtertxt.FilterType = FilterTypes.Custom
                                filtertxt.ValidChars = "0123456789."
                                filtertxt.TargetControlID = "txtvar" & dtableattributes(i)("attid").ToString
                                strCell_2.Controls.Add(filtertxt)
                                If dtableattributes(i)("IsRequired") = 1 Then
                                    Dim reqvar As New RequiredFieldValidator
                                    reqvar.ID = "reqvar" & dtableattributes(i)("attid").ToString
                                    reqvar.ControlToValidate = "txtvar" & dtableattributes(i)("attid").ToString()
                                    reqvar.ErrorMessage = "Please Enter " & dtableattributes(i)("AttDesc").ToString()
                                    reqvar.ValidationGroup = "assets"
                                    reqvar.Display = ValidatorDisplay.None
                                    reqvar.SetFocusOnError = True
                                    strCell_2.Controls.Add(reqvar)
                                End If
                            End If
                            strrow.Cells.Add(strCell_2)
                            strrow.Cells.Add(strCell_3)
                            strtable.Rows.Add(strrow)
                        Next
                        tddata.Controls.Add(strtable)
                    End If
                End If
            End If
            If Session("Usergroup") <> "1" Then
                btnsave.Enabled = False
            End If

        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function GetAssetAttributes() As DataTable
        Dim sql As String
        Try
            'sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpConsType.SelectedValue & " order by aa.attid asc"
            sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpConsType.SelectedValue & " order by aa.attid asc"
            Dim dtable As New DataTable
            con.Open()
            sqladr = New SqlDataAdapter(sql, con)
            sqladr.Fill(dtable)
            Return dtable
        Catch ex As Exception
        Finally
            con.Close()
        End Try
    End Function

    Private Function bindcategory()
        Dim sql As String
        con.Open()
        drpCategory.Items.Clear()
        drpCategory.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select * from tbl_Asset_CategoryMaster where groupid = 3 order by catcode asc", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpCategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Private Function bindLocation()
        con.Open()
        drpLocation.Items.Clear()
        drpLocation.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select al.locid,ml.loccatname + '-' + al.locname as locname from tbl_Asset_location al,tbl_Asset_location_master ml where al.loccatid = ml.id order by al.locname asc", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpLocation.Items.Add(New ListItem(rdr("locname"), rdr("locid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Private Function BindAssetType(ByVal categoryid As String)
        Try
            Dim sql As String
            sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId=" & categoryid & " order by am.AssetTypeCode"
            'sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId='" & drpCategory.SelectedItem.Value & "' order by am.CatId"
            con.Open()
            drpConsType.Items.Clear()
            drpConsType.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpConsType.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub drpCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpCategory.SelectedIndexChanged
        Try
            If drpCategory.SelectedValue <> "" Then
                BindAssetType(drpCategory.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drpConsType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpConsType.SelectedIndexChanged
        Try
            btnsave.Visible = True
            btncancel.Visible = True
            txtQuantity.Focus()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            'Dim str As String
            Dim fieldname As String = ""
            Dim values As String = ""
            Dim query As String = ""
            Dim strmessage As String = ""
            Try
                If CInt(txtQuantity.Text) = 0 Then
                    Dim myscript As String = "alert('Please Enter Valid Quantity');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    txtQuantity.Focus()
                    Exit Sub
                End If
            Catch ex As Exception
                Dim myscript As String = "alert('Please Enter Valid Quantity');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                txtQuantity.Focus()
                Exit Sub
            End Try
            If drpLocation.SelectedValue = "" Then
                Dim myscript As String = "alert('Please Select Location');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                drpLocation.Focus()
                Exit Sub
            End If
            If drpSubLocation.SelectedValue = "" Then
                Dim myscript As String = "alert('Please Select SubLocation');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                drpSubLocation.Focus()
                Exit Sub
            End If
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
            'For m As Integer = 0 To CInt(txtassetCont.Text) - 1
            Dim dtConsumableatt1 As New DataTable
            dtConsumableatt1 = GetAssetAttributesFields()
            If CInt(txtQuantity.Text) >= 1 Then
                If dtableattributes.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable = CType(tddata.FindControl("strtable1"), Table)
                    Dim txtvar As TextBox
                    Dim txtfix As TextBox
                    Dim txtseq As TextBox
                    Dim txtdate As TextBox
                    Dim drpsingle As DropDownList
                    Dim drplist As ListBox
                    Dim radio As RadioButtonList
                    Dim fileupload As FileUpload
                    Dim confields As String = ""
                    fieldname = "insert into tbl_Asset_ConsumablesMaster("
                    For i As Integer = 0 To dtableattributes.Rows.Count - 1
                        If UCase(dtableattributes(i)("atttype")) = UCase("Text(Variable)") Then
                            txtvar = CType(strtable.FindControl("txtvar" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtvar IsNot Nothing Then
                                'If m = 0 Then
                                values = values & "'" & txtvar.Text & "',"
                                'Else
                                'values = values & "'',"
                                'End If
                            Else
                                values = values & "'',"
                            End If
                            If dtableattributes(i)("Header") = "1" Then
                                If txtvar IsNot Nothing Then
                                    PONumber = txtvar.Text
                                End If
                            End If

                            For k = 0 To dtConsumableatt1.Rows.Count - 1
                                If dtConsumableatt1.Rows(k)("Fieldorder") = dtableattributes(i)("Fieldorder") Then
                                    If dtableattributes(i)("Header") = "0" Or dtableattributes(i)("Header") = "7" Then
                                        confields = confields & "'" & txtvar.Text & "',"
                                    End If
                                End If
                            Next
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Text(Fixed)") Then
                            txtfix = CType(strtable.FindControl("txtfix" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtfix IsNot Nothing Then
                                values = values & "'" & txtfix.Text & "',"
                            Else
                                values = values & "'',"
                            End If
                            If dtableattributes(i)("Header") = "1" Then
                                If txtfix IsNot Nothing Then
                                    PONumber = txtfix.Text
                                End If
                            End If
                            For k = 0 To dtConsumableatt1.Rows.Count - 1
                                If dtConsumableatt1.Rows(k)("Fieldorder") = dtableattributes(i)("Fieldorder") Then
                                    If dtableattributes(i)("Header") = "0" Or dtableattributes(i)("Header") = "7" Then
                                        confields = confields & "'" & txtfix.Text & "',"
                                    End If
                                End If
                            Next
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Sequence") Then
                            txtseq = CType(strtable.FindControl("txtseq" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtseq IsNot Nothing Then
                                '  values = values & "'" & txtseq.Text & "',"
                                Dim str1() As String = GetStringandNumbers(txtseq.Text).Split("|")
                                If str1.Length > 0 Then
                                    seqvalue = str1(0).Replace("#", "-")
                                    seqno = str1(1)
                                    sqlnolength = str1(2)
                                End If
                                values = values & "'" & seqvalue & seqno1 & "',"
                            Else
                                values = values & "'',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                            drpsingle = CType(strtable.FindControl("drp" & dtableattributes(i)("attid").ToString), DropDownList)
                            If drpsingle IsNot Nothing Then
                                values = values & "'" & drpsingle.SelectedValue & "',"
                            Else
                                values = values & "'',"
                            End If
                            For k = 0 To dtConsumableatt1.Rows.Count - 1
                                If dtConsumableatt1.Rows(k)("Fieldorder") = dtableattributes(i)("Fieldorder") Then
                                    If dtableattributes(i)("Header") = "0" Or dtableattributes(i)("Header") = "7" Then
                                        confields = confields & "'" & drpsingle.SelectedValue & "',"
                                    End If
                                End If
                            Next
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("MultiSelection") Then
                            drplist = CType(strtable.FindControl("list" & dtableattributes(i)("attid").ToString), ListBox)
                            If drplist IsNot Nothing Then
                                Dim strstring As String = ""
                                For j As Integer = 0 To drplist.Items.Count - 1
                                    If drplist.Items(j).Selected Then
                                        strstring = strstring & drplist.Items(j).Text & "|"
                                    End If
                                Next
                                values = values & "'" & strstring & "',"
                            Else
                                values = values & "'',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Date") Then
                            txtdate = CType(strtable.FindControl("txtdate" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtdate IsNot Nothing Then
                                values = values & "'" & txtdate.Text & "',"
                            Else
                                values = values & "'',"
                            End If
                            If dtableattributes(i)("Header") = "2" Then
                                If txtdate IsNot Nothing Then
                                    PODate = txtdate.Text
                                End If
                            End If
                            If dtableattributes(i)("Header") = "3" Then
                                If txtdate IsNot Nothing Then
                                    WarrentyStart = txtdate.Text
                                End If
                            End If
                            If dtableattributes(i)("Header") = "4" Then
                                If txtdate IsNot Nothing Then
                                    WarrentyEnd = txtdate.Text
                                End If
                            End If
                            For k = 0 To dtConsumableatt1.Rows.Count - 1
                                If dtConsumableatt1.Rows(k)("Fieldorder") = dtableattributes(i)("Fieldorder") Then
                                    If dtableattributes(i)("Header") = "0" Or dtableattributes(i)("Header") = "7" Then
                                        confields = confields & "'" & txtdate.Text & "',"
                                    End If
                                End If
                            Next
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Yes/No") Then
                            radio = CType(strtable.FindControl("rdo" & dtableattributes(i)("attid").ToString), RadioButtonList)
                            If radio IsNot Nothing Then
                                values = values & "'" & radio.SelectedValue & "',"
                            Else
                                values = values & "'',"
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("FileUpload") Then
                            fileupload = CType(strtable.FindControl("fileupload" & dtableattributes(i)("attid").ToString), FileUpload)
                            If fileupload IsNot Nothing Then
                                If fileupload.HasFile Then
                                    values = values & "'" & Path.GetFileName(fileupload.FileName) & "',"
                                Else
                                    values = values & "'',"
                                End If
                            End If
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Text(Number)") Then
                            txtvar = CType(strtable.FindControl("txtvar" & dtableattributes(i)("attid").ToString), TextBox)
                            If txtvar IsNot Nothing Then
                                values = values & "'" & Convert.ToDecimal(txtvar.Text) & "',"
                            Else
                                values = values & "'',"
                            End If
                        Else
                            values = values & "'',"
                        End If
                        fieldname = fieldname & dtableattributes(i)("FieldOrder") & ","
                    Next
                    If confields.Contains(",") Then
                        confields = confields.Remove(confields.Length - 1, 1)
                    End If
                    values = values & "'" & drpConsType.SelectedValue & "',"
                    values &= Trim(txtQuantity.Text)
                    fieldname &= "ConsTypeid,Quantity)"
                    query = fieldname & "values(" & values & ")"
                    fieldname = ""
                    values = ""
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    cmd = New SqlCommand(query & "SELECT id FROM tbl_Asset_ConsumablesMaster WHERE id = @@IDENTITY", con)
                    Dim Asset_ConsumID As Integer = CInt(cmd.ExecuteScalar())
                    con.Close()
                    cmd.Dispose()

                    ' To get Vendor ID
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    cmd = New SqlCommand("SELECT VendorId FROM tbl_Asset_Vendor", con)
                    Dim Cons_VendorID As Integer = CInt(cmd.ExecuteScalar())
                    con.Close()
                    cmd.Dispose()
                    If Asset_ConsumID > 0 Then
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                        con.Open()
                        cmd = New SqlCommand("insert into tbl_Asset_Cons_Transactions(Transid,ConsTypeId,TransType,OrderNo,date1,date2,date3,att1,att2,UserId,VendorId,ReasonId,TransBy,Locid,Sublocid,transdate,quantity)values(" & Asset_ConsumID & ",'" & drpConsType.SelectedValue & "','N', '" & PONumber & "','" & PODate & "','" & WarrentyStart & "','" & WarrentyEnd & "','" & PONumber & "','" & PODate & "','" & Session("EmpNo") & "','" & Cons_VendorID & "','1','" & Session("EmpNo") & "','" & drpLocation.SelectedValue & "','" & drpSubLocation.SelectedValue & "','" & DateTime.Now.ToString() & "'," & Trim(txtQuantity.Text) & ")", con)
                        cmd.ExecuteNonQuery()
                        con.Close()
                        cmd.Dispose()
                    End If

                    ' To insert Consumable Stock table : tbl_Asset_Cons_Stock
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()

                    fieldname = "select * from tbl_Asset_Cons_Stock where ConsTypeId =" & drpConsType.SelectedValue & " and locid = " & drpLocation.SelectedValue & " and sublocid =" & drpSubLocation.SelectedValue & " and "
                    Dim updatestock As String = "update tbl_Asset_Cons_Stock set quantity = "
                    Dim dtConsumableatt As New DataTable
                    dtConsumableatt = GetAssetAttributesFields()
                    Dim fieldname1 As String = ""
                    Dim stockvalue() As String = confields.Split(",")
                    Dim fieldnameinsert As String = ""
                    For i As Integer = 0 To dtConsumableatt.Rows.Count - 1
                        fieldname1 &= dtConsumableatt(i)("FieldOrder") & "= " & stockvalue(i) & " and "
                        fieldnameinsert &= dtConsumableatt(i)("FieldOrder") & ","
                    Next
                    If fieldname1.Contains("and") Then
                        fieldname1 = fieldname1.Remove(fieldname1.Length - 4, 4)
                    End If
                    If fieldnameinsert.Contains(",") Then
                        fieldnameinsert = fieldnameinsert.Remove(fieldnameinsert.Length - 1, 1)
                    End If
                    fieldname = fieldname & fieldname1
                    If Right(fieldname, 4) = "and " Then
                        fieldname = fieldname.Remove(fieldname.Length - 4, 3)
                    End If
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    cmd = New SqlCommand(fieldname, con)
                    rdr = cmd.ExecuteReader
                    Dim quantity As Integer = 0
                    Dim stockid As Integer = 0
                    If rdr.HasRows Then
                        While rdr.Read
                            stockid = rdr("id")
                            quantity = rdr("Quantity")
                        End While
                        If con1.State = ConnectionState.Open Then
                            con1.Close()
                        End If
                        con1.Open()
                        cmd1 = New SqlCommand(updatestock & quantity + CInt(txtQuantity.Text.Trim) & " where id=" & stockid, con1)
                        cmd1.ExecuteNonQuery()
                        con1.Close()
                        cmd1.Dispose()
                        strmessage = "Record(s) Updated Successfully "
                        tddata.Visible = False
                    Else
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                        con.Open()
                        If fieldnameinsert <> "" Then
                            cmd = New SqlCommand("insert into tbl_Asset_Cons_Stock (ConsTypeId,Quantity,locid,sublocid," & fieldnameinsert & ") values(" & drpConsType.SelectedValue & "," & txtQuantity.Text.Trim() & "," & drpLocation.SelectedValue & ",'" & drpSubLocation.SelectedValue & "'," & confields & ")", con)
                        Else
                            cmd = New SqlCommand("insert into tbl_Asset_Cons_Stock (ConsTypeId,Quantity,locid,sublocid) values(" & drpConsType.SelectedValue & "," & txtQuantity.Text.Trim() & "," & drpLocation.SelectedValue & "," & drpSubLocation.SelectedValue & ")", con)
                        End If
                        cmd.ExecuteNonQuery()
                        con.Close()
                        cmd.Dispose()
                        strmessage = "Record(s) Inserted Successfully "
                    End If
                End If
            Else
                Dim myscript As String = "alert('Please Enter the Valid Quantity');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                drpSubLocation.Focus()
                Exit Sub
            End If

            ''****Diplay message and Reset or clear controls
            ' ResetFormControlValues(Me)
            '  drpCategory.SelectedIndex = 0
            ' drpConsType.SelectedIndex = 0
            '  drpLocation.SelectedIndex = 0
            '  drpSubLocation.SelectedIndex = 0
            lblMessage.Visible = True
            trmessage.Visible = True
            lblMessage.Text = strmessage
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function GetStringandNumbers(ByVal str As String) As String
        Dim number As String = String.Empty
        Dim strvalue As String = String.Empty
        Dim s As Integer = 0
        For i As Integer = 1 To str.Length
            Try
                If str.Contains("-") Then
                    str = str.Replace("-", "#")
                End If
                If IsNumeric(Right(str, i)) = True Then
                    number = Right(str, i)
                    s = i
                Else
                    strvalue = str.Substring(0, str.Length - s)
                End If
            Catch

            End Try
        Next
        Return strvalue & "|" & number & "|" & number.Length
    End Function
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
                    Case "System.Web.UI.WebControls.RadioButton"
                        CType(c, RadioButton).Checked = False
                End Select
            End If
        Next c
    End Sub

    Private Function BindDropdown(ByVal drpdown As DropDownList, ByVal attid As String)
        Try
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
            Dim cmd As New System.Data.SqlClient.SqlCommand
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim str As String = ""
            con.Open()
            drpdown.Items.Clear()
            str = "select * from tbl_asset_AttributeList where attid  =" & attid & " order by attdesc"
            cmd.CommandText = str
            cmd.Connection = con
            rdr = cmd.ExecuteReader
            While rdr.Read
                drpdown.Items.Add(New ListItem(rdr("attdesc"), rdr("attdesc")))
            End While
            drpdown.Items.Insert(0, "--Select--")
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
            con.Open()
            Lstbox.Items.Clear()
            str = "select * from tbl_asset_AttributeList where attid  =" & attid & " order by attdesc"
            cmd.CommandText = str
            cmd.Connection = con
            rdr = cmd.ExecuteReader
            While rdr.Read
                Lstbox.Items.Add(New ListItem(rdr("attdesc"), rdr("attdesc")))
            End While
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub drpLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpLocation.SelectedIndexChanged
        Try
            If drpLocation.SelectedValue <> "" Then
                BindSubLocation(drpLocation.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function BindSubLocation(ByVal LocationID As String)
        Try
            Dim sql As String
            sql = "SELECT * FROM tbl_Asset_sublocation WHERE locid = '" & LocationID & "' order by sublocname"
            con.Open()
            drpSubLocation.Items.Clear()
            drpSubLocation.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpSubLocation.Items.Add(New ListItem(rdr("sublocname"), rdr("sublocid")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function GetAssetAttributesFields() As DataTable
        Dim sql As String
        Try
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            'sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpConsType.SelectedValue & " order by aa.attid asc"
            sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header in('0','7') and aad.AssetTypeId = " & drpConsType.SelectedValue & " and aad.seq <> 0 order by aa.attid asc"
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
    Public Function bindlocation(ByVal constypeid As String, ByVal att1 As String)

    End Function
    Protected Sub drpmodel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim sql As String
            Dim dtConsumableatt1 As New DataTable
            dtConsumableatt1 = GetAssetAttributesFields()
            Dim strtable As New Table
            strtable = CType(tddata.FindControl("strtable1"), Table)
            Dim model As String = ""
            If dtableattributes.Rows.Count > 0 Then
                Dim drpsingle As DropDownList
                For i As Integer = 0 To dtableattributes.Rows.Count - 1
                    drpsingle = CType(strtable.FindControl("drp" & dtableattributes(i)("attid").ToString), DropDownList)
                    If drpsingle IsNot Nothing Then
                        model = model & "st." & dtableattributes(i)("FieldOrder") & "='" & drpsingle.SelectedValue & "' and "
                    End If
                Next
            End If
            model = model.Remove(model.Trim.Length - 3, 3)
            sql = "select att1,(select mas.loccatname + '-' + loc.locname from tbl_asset_location loc,tbl_Asset_location_master mas where loc.loccatid = mas.id and loc.locid = st.locid)  + '(' +"
            sql = sql & " (select sublocname from tbl_asset_sublocation sub where sub.sublocid = st.sublocid) + ')' as location,st.quantity"
            sql = sql & " from tbl_asset_cons_stock st where st.constypeid = " & drpConsType.SelectedValue & " "
            If model <> "" Then
                sql = sql & " and " & model
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            Dim stock As String = ""
            If rdr.HasRows Then
                stock = "<table><tr><td>"
                While rdr.Read
                    stock = stock & "<tr><td> <b>Location :</b> " & rdr("location") & "  <b>Quanity :</b> " & rdr("quantity") & "</td></tr>"
                End While
                stock = stock & "</td></tr></table>"
            End If
            rdr.Close()
            con.Close()
            If stock <> "" Then
                trlocation.Visible = True
                lbllocation.Text = stock
            Else
                trlocation.Visible = False
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
End Class
