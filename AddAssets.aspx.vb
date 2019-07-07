Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports AjaxControlToolkit
Partial Class AddAssets
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim dtableattributes As DataTable
    Dim dtableassetattributes As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
            If imgbtn IsNot Nothing Then
                imgbtn.Focus()
            End If
            If Not IsPostBack Then
                bindcategory()
                FillLocation()
            End If
            lblmessage.Text = ""
            AddAssets()
            If Session("Usergroup") <> "1" Then
                btnsave.Enabled = False
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function AddAssets()
        If drpCategory.SelectedValue <> "" Then
            If drpAssetType.SelectedValue <> "" Then
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
                        Dim strCell_4 As New TableCell
                        strCell_1.Width = Unit.Percentage(52)
                        strCell_2.Width = Unit.Percentage(10)
                        strCell_3.Width = Unit.Percentage(4)
                        strCell_4.Width = Unit.Percentage(33)
                        strCell_2.VerticalAlign = VerticalAlign.Bottom
                        strtable.Style("align") = "center"
                        strCell_1.Style("text-align") = "right"
                        strCell_1.Style.Add("padding-right", "2px")
                        strCell_2.Style("text-align") = "left"
                        strCell_3.Style("text-align") = "center"
                        strCell_3.Style("color") = "blue"
                        strCell_4.Style("text-align") = "left"
                        strCell_4.Style("color") = "blue"
                        Dim lbl As New Label
                        lbl.ID = "lbl" & dtableattributes(i)("attid").ToString
                        lbl.Text = dtableattributes(i)("attdesc").ToString & " :"
                        Dim lbltooltip As New Label
                        lbltooltip.ID = "lbltooltip" & dtableattributes(i)("attid").ToString
                        If dtableattributes(i)("atttooltipdesc").ToString <> "" Then
                            lbltooltip.Text = "  (" & dtableattributes(i)("atttooltipdesc").ToString & ")  "
                        End If
                        strCell_1.Controls.Add(lbl)
                        strrow.Cells.Add(strCell_1)
                        strCell_4.Controls.Add(lbltooltip)
                        If UCase(dtableattributes(i)("atttype")) = UCase("Text(Variable)") Then
                            Dim txtvar As New TextBox
                            txtvar.CssClass = "control"
                            txtvar.Width = Unit.Pixel(150)
                            txtvar.ID = "txtvar" & dtableattributes(i)("attid").ToString
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
                            txtfix.ValidationGroup = "assets"
                            txtfix.Width = Unit.Pixel(150)
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
                            txtseq.ValidationGroup = "assets"
                            txtseq.Width = Unit.Pixel(150)
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
                            Dim hdlink As New HyperLink
                            Dim u1 As New UpdatePanel()
                            Dim b As New LinkButton()
                            Dim strtable1 As New Table
                            Dim strchildrow As New TableRow
                            Dim strchCell_1 As New TableCell
                            Dim strchCell_2 As New TableCell
                            strtable1.ID = "tbl1" & dtableattributes(i)("attid").ToString
                            b.Text = "Refresh"
                            b.ID = "btn" & dtableattributes(i)("attid").ToString
                            AddHandler b.Click, AddressOf OnClick
                            drpsingle.CssClass = "control"
                            drpsingle.ID = "drp" & dtableattributes(i)("attid").ToString
                            hdlink.ID = "hdlink" & dtableattributes(i)("attid").ToString
                            hdlink.Text = "Add"
                            drpsingle.Width = Unit.Pixel(158)
                            BindDropdown(drpsingle, dtableattributes(i)("attid").ToString)
                            hdlink.NavigateUrl = "AttributesItemList.aspx?AId=" & dtableattributes(i)("attid").ToString & "&Attname=" & dtableattributes(i)("AttDesc").ToString
                            hdlink.Attributes.Add("OnClick", "javascript:window.open (this.href, 'popupwindow', 'width=420,height=500,left=300,top=300,scrollbars,resizable=1'); return false;")
                            strchCell_1.Controls.Add(drpsingle)
                            strchCell_2.Controls.Add(b)
                            strchildrow.Cells.Add(strchCell_1)
                            strchildrow.Cells.Add(strchCell_2)
                            strtable1.Rows.Add(strchildrow)
                            u1.ContentTemplateContainer.Controls.Add(strtable1)
                            strCell_2.Controls.Add(u1)
                            strCell_3.Controls.Add(hdlink)
                        ElseIf UCase(dtableattributes(i)("atttype")) = UCase("MultiSelection") Then
                            Dim drplist As New ListBox
                            Dim hdlink As New HyperLink
                            Dim u1 As New UpdatePanel()
                            Dim strtable1 As New Table
                            Dim strchildrow As New TableRow
                            Dim strchCell_1 As New TableCell
                            Dim strchCell_2 As New TableCell
                            strtable1.ID = "tbl1" & dtableattributes(i)("attid").ToString
                            drplist.CssClass = "control"
                            drplist.ID = "list" & dtableattributes(i)("attid").ToString
                            hdlink.ID = "hdlink" & dtableattributes(i)("attid").ToString
                            hdlink.Text = "Add"
                            Dim b As New LinkButton()
                            b.Text = "Refresh"
                            b.ID = "btn" & dtableattributes(i)("attid").ToString
                            AddHandler b.Click, AddressOf OnClick
                            drplist.Width = Unit.Pixel(150)
                            drplist.SelectionMode = ListSelectionMode.Multiple
                            BindListbox(drplist, dtableattributes(i)("attid").ToString)
                            strchCell_1.Controls.Add(drplist)
                            strchCell_2.Controls.Add(b)
                            strchildrow.Cells.Add(strchCell_1)
                            strchildrow.Cells.Add(strchCell_2)
                            strtable1.Rows.Add(strchildrow)
                            u1.ContentTemplateContainer.Controls.Add(strtable1)
                            hdlink.NavigateUrl = "AttributesItemList.aspx?AId=" & dtableattributes(i)("attid").ToString & "&Attname=" & dtableattributes(i)("AttDesc").ToString
                            hdlink.Attributes.Add("OnClick", "javascript:window.open (this.href, 'popupwindow', 'width=420,height=500,left=300,top=300,scrollbars,resizable=1'); return false;")
                            strCell_2.Controls.Add(u1)
                            strCell_3.Controls.Add(hdlink)
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
                        End If
                        strrow.Cells.Add(strCell_2)
                        strrow.Cells.Add(strCell_3)
                        strrow.Cells.Add(strCell_4)
                        strtable.Rows.Add(strrow)
                    Next
                    tddata.Controls.Add(strtable)
                End If
            End If
        End If
    End Function
    Public Function GetAssetAttributes(Optional ByVal primaryfield As Boolean = False) As DataTable
        Dim sql As String
        Try
            If primaryfield = True Then
                sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpAssetType.SelectedValue & " and aa.Header='5' order by aa.attid asc"
            Else
                sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpAssetType.SelectedValue & " order by aa.attid asc"
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
    Private Function bindcategory()
        Dim sql As String
        con.Open()
        drpCategory.Items.Clear()
        drpCategory.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select * from tbl_Asset_CategoryMaster where groupid = 1  order by catcode asc", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpCategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Private Function BindAssetType(ByVal categoryid As String)
        Try
            Dim sql As String
            sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId=" & categoryid & " order by am.AssetTypeCode asc"
            con.Open()
            drpAssetType.Items.Clear()
            drpAssetType.Items.Add(New ListItem("--Select--", ""))
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

    Protected Sub drpAssetType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAssetType.SelectedIndexChanged
        Try
            btncancel.Visible = True
            If Session("Usergroup") <> "1" Then
                btnsave.Visible = False
            Else
                btnsave.Visible = True
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            Dim str As String
            Dim fieldname As String = ""
            Dim values As String = ""
            Dim query As String = ""
            Try
                If CInt(txtassetCont.Text) = 0 Then
                    Dim myscript As String = "alert('Please Enter Valid Count');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    txtassetCont.Focus()
                    Exit Sub
                End If
            Catch ex As Exception
                Dim myscript As String = "alert('Please Enter Valid Count');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                txtassetCont.Focus()
                Exit Sub
            End Try
            If drploc.SelectedValue = "" Then
                Dim myscript As String = "alert('Please Select Location');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                drploc.Focus()
                Exit Sub
            End If
            If drpsubloc.SelectedValue = "" Then
                Dim myscript As String = "alert('Please Select Sub Location');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                drpsubloc.Focus()
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
            Dim chkassetno As Boolean = False
            Dim assetno As String = ""
            Dim primaryfield As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim dtable1 As New DataTable
            dtable1 = GetAssetAttributes(True)
            For i As Integer = 0 To dtable1.Rows.Count - 1
                Dim strtable As New Table
                strtable = CType(tddata.FindControl("strtable1"), Table)
                Dim txtseq As TextBox
                Dim qry As String = ""
                primaryfield = dtable1(i)("FieldOrder").ToString
                For m As Integer = 0 To CInt(txtassetCont.Text) - 1
                    If UCase(dtable1(i)("atttype")) = UCase("Sequence") Then
                        txtseq = CType(strtable.FindControl("txtseq" & dtable1(i)("attid").ToString), TextBox)
                        If txtseq IsNot Nothing Then
                            Dim str1() As String = GetStringandNumbers(txtseq.Text.Trim()).Split("|")
                            If str1.Length > 0 Then
                                seqvalue = str1(0).Replace("#", "-")
                                seqno = str1(1)
                                sqlnolength = str1(2)
                            End If
                            If sqlnolength = 1 Then
                                seqno1 = Format(seqno + m, "0")
                            ElseIf sqlnolength = 2 Then
                                seqno1 = Format(seqno + m, "00")
                            ElseIf sqlnolength = 3 Then
                                seqno1 = Format(seqno + m, "000")
                            ElseIf sqlnolength = 4 Then
                                seqno1 = Format(seqno + m, "0000")
                            ElseIf sqlnolength = 5 Then
                                seqno1 = Format(seqno + m, "00000")
                            ElseIf sqlnolength = 6 Then
                                seqno1 = Format(seqno + m, "000000")
                            ElseIf sqlnolength = 7 Then
                                seqno1 = Format(seqno + m, "0000000")
                            ElseIf sqlnolength = 8 Then
                                seqno1 = Format(seqno + m, "00000000")
                            ElseIf sqlnolength = 9 Then
                                seqno1 = Format(seqno + m, "000000000")
                            ElseIf sqlnolength = 10 Then
                                seqno1 = Format(seqno + m, "0000000000")
                            Else
                                seqno1 = seqno + m
                            End If
                            qry = "select " & primaryfield & " from tbl_Asset_Master where " & primaryfield & "='" & seqvalue & seqno1 & "' "
                            cmd = New SqlCommand(qry, con)
                            rdr = cmd.ExecuteReader
                            If rdr.HasRows Then
                                If rdr.Read Then
                                    assetno = assetno & seqvalue & seqno1 & ","
                                    chkassetno = True
                                End If
                            End If
                            rdr.Close()
                        Else
                            values = values & "'',"
                        End If
                    End If
                Next
            Next
            con.Close()
            ' Random No Generation
            Dim my_num As Integer
            Dim arrlist As New ArrayList()
            For j As Integer = 1000 To 9999 - 1
                my_num = Int((9999 - j + 1) * Rnd() + 1000)
                Dim arr() As String = {1111, 2222, 3333, 4444, 5555, 6666, 7777, 8888, 9999}
                If arr.Contains(my_num) <> True And Convert.ToString(my_num).Length = 4 Then
                    arrlist.Add(my_num)
                End If
            Next

            If chkassetno = False Then
                For m As Integer = 0 To CInt(txtassetCont.Text) - 1
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
                        fieldname = "insert into tbl_Asset_Master("

                        For i As Integer = 0 To dtableattributes.Rows.Count - 1
                            If UCase(dtableattributes(i)("atttype")) = UCase("Text(Variable)") Then
                                txtvar = CType(strtable.FindControl("txtvar" & dtableattributes(i)("attid").ToString), TextBox)
                                If txtvar IsNot Nothing Then
                                    If m = 0 Then
                                        values = values & "'" & txtvar.Text & "',"
                                    Else
                                        values = values & "'',"
                                    End If
                                Else
                                    values = values & "'',"
                                End If
                                If dtableattributes(i)("Header") = "1" Then
                                    If txtvar IsNot Nothing Then
                                        PONumber = txtvar.Text
                                    End If
                                End If
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
                                    If sqlnolength = 1 Then
                                        seqno1 = Format(seqno + m, "0")
                                    ElseIf sqlnolength = 2 Then
                                        seqno1 = Format(seqno + m, "00")
                                    ElseIf sqlnolength = 3 Then
                                        seqno1 = Format(seqno + m, "000")
                                    ElseIf sqlnolength = 4 Then
                                        seqno1 = Format(seqno + m, "0000")
                                    ElseIf sqlnolength = 5 Then
                                        seqno1 = Format(seqno + m, "00000")
                                    ElseIf sqlnolength = 6 Then
                                        seqno1 = Format(seqno + m, "000000")
                                    ElseIf sqlnolength = 7 Then
                                        seqno1 = Format(seqno + m, "0000000")
                                    ElseIf sqlnolength = 8 Then
                                        seqno1 = Format(seqno + m, "00000000")
                                    ElseIf sqlnolength = 9 Then
                                        seqno1 = Format(seqno + m, "000000000")
                                    ElseIf sqlnolength = 10 Then
                                        seqno1 = Format(seqno + m, "0000000000")
                                    Else
                                        seqno1 = seqno + m
                                    End If
                                    values = values & "'" & seqvalue & seqno1 & "',"
                                Else
                                    values = values & "'',"
                                End If
                            ElseIf UCase(dtableattributes(i)("atttype")) = UCase("Random") Then
                                lblrnd = CType(strtable.FindControl("lblrnd" & dtableattributes(i)("attid").ToString), Label)
                                If lblrnd IsNot Nothing Then
                                    For n As Integer = 0 To arrlist.Count - 1
                                        Dim CheckRandamNo As String = GetRadanQuery(drpAssetType.SelectedValue, arrlist(n))
                                        If CheckRandamNo = "0" Then
                                            If CInt(seqno1) <> CInt(arrlist(n)) Then
                                                values = values & "'" & arrlist(n) & "',"
                                                Exit For
                                            End If
                                        End If
                                    Next
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
                                        If m = 0 Then
                                            fileupload.SaveAs(Server.MapPath("~/POFiles/") & fileupload.FileName)
                                        End If
                                        values = values & "'" & Path.GetFileName(fileupload.FileName) & "',"
                                    Else
                                        values = values & "'',"
                                    End If
                                End If
                            Else
                                values = values & "'',"
                            End If
                            fieldname = fieldname & dtableattributes(i)("FieldOrder") & ","
                            'fieldno = fieldno + 1
                        Next
                        values &= drpAssetType.SelectedValue
                        fieldname &= "AssetTypeid)"
                        query = fieldname & "values(" & values & ")"
                        fieldname = ""
                        ' fieldno = 1
                        values = ""
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                        con.Open()
                        cmd = New SqlCommand(query & "SELECT id FROM tbl_Asset_Master WHERE id = @@IDENTITY", con)
                        Dim AssetID As Integer = CInt(cmd.ExecuteScalar())

                        If m = 0 Then
                            startassetid = AssetID
                        End If
                        If m = CInt(txtassetCont.Text - 1) Then
                            endassetid = AssetID
                        End If
                        con.Close()
                        cmd.Dispose()
                        If AssetID > 0 Then
                            If con.State = ConnectionState.Open Then
                                con.Close()
                            End If
                            con.Open()
                            cmd = New SqlCommand("insert into tbl_Asset_Transactions(assetid,TransType,PONo,date1,date2,date3,transcreatedid,reasonid,locid,sublocid,remarks,userid,transdate)values(" & AssetID & ",'0','" & PONumber & "','" & PODate & "','" & WarrentyStart & "','" & WarrentyEnd & "','" & Session("EmpNo") & "','1'," & drploc.SelectedValue & "," & drpsubloc.SelectedValue & ",'New Purchase','0000','" & DateTime.Now() & "')", con)
                            cmd.ExecuteNonQuery()
                            con.Close()
                            cmd.Dispose()
                            If con.State = ConnectionState.Open Then
                                con.Close()
                            End If
                            con.Open()
                            cmd = New SqlCommand("insert into tbl_Asset_Status(Assetid,status,userid)values(" & AssetID & ",'N','0000')", con)
                            cmd.ExecuteNonQuery()
                            con.Close()
                        End If
                    End If
                Next
                Session("Assets") = drpCategory.SelectedValue & "|" & drpAssetType.SelectedValue & "|" & txtassetCont.Text & "|" & startassetid & "|" & endassetid
                Response.Redirect("EditAssets.aspx", True)
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
    Private Function BindDropdown(ByVal drpdown As DropDownList, ByVal attid As String)
        Try
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
            Dim cmd As New System.Data.SqlClient.SqlCommand
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim str As String = ""
            con.Open()
            drpdown.Items.Clear()
            str = "select * from tbl_asset_AttributeList where attid  =" & attid
            cmd.CommandText = str
            cmd.Connection = con
            rdr = cmd.ExecuteReader
            While rdr.Read
                drpdown.Items.Add(New ListItem(rdr("attdesc"), rdr("attdesc")))
            End While
            drpdown.Items.Insert(0, New ListItem("--Select--", ""))
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
            str = "select * from tbl_asset_AttributeList where attid  =" & attid
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
    Public Sub FillLocation()
        Try
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            drploc.Items.Clear()
            drploc.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand("select al.locid,ml.loccatname + '-' + al.locname as locname from tbl_Asset_location al,tbl_Asset_location_master ml where al.loccatid = ml.id order by ml.loccatname asc", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drploc.Items.Add(New ListItem(rdr("locname"), rdr("locid")))
                End While
            End If
            rdr.Close()
            con.Close()

        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub FillSubLocation(ByVal locid As String)
        Try
            Dim sql As String
            sql = "select sublocname,sublocid from tbl_Asset_sublocation where locid =" & Val(locid) & " order by sublocname"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            drpsubloc.Items.Clear()
            drpsubloc.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpsubloc.Items.Add(New ListItem(rdr("sublocname"), rdr("sublocid")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drploc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drploc.SelectedIndexChanged
        Try
            If drploc.SelectedValue <> "" Then
                FillSubLocation(drploc.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("Default.aspx")
    End Sub
    Public Function GetRadanQuery(ByVal AssetTypeId As String, ByVal Value As String) As String
        Dim sql As String
        Dim FieldOrder As String = ""
        If AssetTypeId <> "" Then
            sql = "Select "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand
            cmd.Connection = con
            cmd.CommandType = Data.CommandType.Text
            '  cmd.CommandText = "select * from tbl_Asset_Attributes where CatId=" & catid & " order by attid"
            cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId in(" & AssetTypeId & ") and aa.Header ='8' order by aad.attid asc"
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    If sql.Contains(rdr("FieldOrder")) = False Then
                        sql = sql & "count(isnull(" & rdr("FieldOrder") & ",''))" & " as [" & rdr("AttDesc") & "],"
                        FieldOrder = rdr("FieldOrder")
                    End If
                End While
            Else
                sql = sql & "* "
            End If
            rdr.Close()
            sql = Left(sql, Len(sql) - 1)
            sql = sql & " from tbl_Asset_Master where AssetTypeid in(" & AssetTypeId & ") and " & FieldOrder & " = '" & Value & "'"
            cmd = New SqlCommand(sql, con)
            Dim s As String = Convert.ToString(cmd.ExecuteScalar)
            cmd.Dispose()
            con.Close()
            Return s
        Else
            Return ""
        End If
    End Function
    Public Function GetDataTable(ByVal AssetTypeId As String) As DataTable
        Dim sql As String
        If AssetTypeId <> "" Then
            sql = "Select  "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand
            cmd.Connection = con
            cmd.CommandType = Data.CommandType.Text
            '  cmd.CommandText = "select * from tbl_Asset_Attributes where CatId=" & catid & " order by attid"
            cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId in(" & AssetTypeId & ") and aa.Header in ('5','8') order by aad.attid asc"
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    If sql.Contains(rdr("FieldOrder")) = False Then
                        sql = sql & rdr("FieldOrder") & " as [" & rdr("AttDesc") & "],"
                    End If
                End While
            Else
                sql = sql & "* "
            End If
            sql = Left(sql, Len(sql) - 1)
            con1.Open()
            sql = sql & " ,id from tbl_Asset_Master where AssetTypeid in(" & AssetTypeId & ")"
            sqladr = New SqlDataAdapter(sql, con1)
            dtable = New DataTable
            sqladr.Fill(dtable)
            con1.Close()
            rdr.Close()
            con.Close()
            Return dtable
        End If
    End Function
    Protected Sub OnClick(ByVal sender As [Object], ByVal e As EventArgs)

    End Sub
End Class
