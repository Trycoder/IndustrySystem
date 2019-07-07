Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Partial Class SoftwareWarranty
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindPurchaseOrders()
            BindVendor()
            GetVersion()
            GetSoftwareType()
        End If
        If drpcategory.SelectedValue <> "" Then
            BulkUpdateItems()
        End If
        lblmessage.Text = ""
    End Sub
    Private Function BindPurchaseOrders()
        Dim sql As String = ""
        con.Open()
        sql = "select a.assettypeid,assettypecode from tbl_asset_typemaster a, tbl_asset_categorymaster ac where a.catid = ac.catid and ac.groupid = 2"
        sqladr = New SqlDataAdapter(sql, con)
        dtable = New DataTable
        sqladr.Fill(dtable)
        con.Close()
        sql = ""
        Dim arrlist As New ArrayList()
        If dtable.Rows.Count > 0 Then
            For i As Integer = 0 To dtable.Rows.Count - 1
                con.Open()
                sql = "select isnull(aa.fieldorder,'') as fieldorder from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and ad.assettypeid = " & dtable.Rows(i)("assettypeid") & " and aa.header = 1"
                cmd = New SqlCommand(sql, con)
                Dim fieldorder As String = cmd.ExecuteScalar
                con.Close()
                cmd.Dispose()
                If fieldorder <> "" Then
                    con.Open()
                    cmd1 = New SqlCommand("select distinct " & fieldorder & " from tbl_asset_soft_master where softwareid = " & dtable.Rows(i)("assettypeid") & "", con)
                    rdr = cmd1.ExecuteReader
                    If rdr.HasRows Then
                        While rdr.Read
                            If arrlist.Contains(rdr(fieldorder)) <> True And rdr(fieldorder) <> "" Then
                                arrlist.Add(rdr(fieldorder))
                            End If
                        End While
                    End If
                    cmd1.Dispose()
                    rdr.Close()
                    con.Close()
                End If
            Next
            If arrlist.Count > 0 Then
                arrlist.Sort()
                drpcategory.DataSource = arrlist
                drpcategory.DataBind()
                drpcategory.Items.Insert(0, New ListItem("--Select--", ""))
            End If
        End If
    End Function
    Public Function GetAssetAttributes(Optional ByVal primaryfield As Boolean = False) As DataTable
        Dim sql As String
        Try
            con.Open()
            sql = "select isnull(aa.fieldorder,'') as fieldorder from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and ad.assettypeid = " & drpcategory.SelectedValue & " and aa.header = 1"
            cmd = New SqlCommand(sql, con)
            Dim fieldorder As String = cmd.ExecuteScalar
            con.Close()
            cmd.Dispose()
            If primaryfield = True Then
                sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpcategory.SelectedValue & " and aa.Header='5' order by aa.attid asc"
            Else
                sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpcategory.SelectedValue & " order by aa.attid asc"
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
    Public Function BulkUpdateItems()
        Try
            Dim sql As String = ""
            con.Open()
            sql = "select a.assettypeid,assettypecode from tbl_asset_typemaster a, tbl_asset_categorymaster ac where a.catid = ac.catid and ac.groupid = 2"
            sqladr = New SqlDataAdapter(sql, con)
            dtable = New DataTable
            sqladr.Fill(dtable)
            sqladr.Dispose()
            con.Close()
            sql = ""
            Dim primaryfield As String = ""
            Dim dtFieldData As DataTable
            Dim arrlist As New ArrayList()
            If dtable.Rows.Count > 0 Then
                For i As Integer = 0 To dtable.Rows.Count - 1
                    con.Open()
                    sql = "select isnull(aa.fieldorder,'') as fieldorder,aa.header from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and ad.assettypeid = " & dtable.Rows(i)("assettypeid") & " and aa.header in (1,4,5,9,10,11,12) order by convert(int,aa.Header)"
                    cmd = New SqlCommand(sql, con)
                    rdr = cmd.ExecuteReader
                    Dim fieldorder As String = ""
                    Dim ContractNo As String = ""
                    Dim VendorName As String = ""
                    Dim POField As Boolean = False
                    Dim POField1 As String = ""
                    If rdr.HasRows Then
                        While rdr.Read
                            If rdr("header") = "1" Then
                                POField1 = rdr("fieldorder")
                            Else
                                fieldorder = fieldorder & rdr("fieldorder") & ","
                            End If
                            If rdr("header") = "5" Then
                                primaryfield = rdr("fieldorder")
                            End If
                        End While
                    End If
                    If fieldorder.Length > 0 Then
                        fieldorder = Left(fieldorder, Len(fieldorder) - 1)
                    End If
                    POField = False
                    con.Close()
                    cmd.Dispose()
                    If fieldorder <> "" Then
                        con.Open()
                        Dim strfield As String = "select " & fieldorder & "," & primaryfield & ",licid from tbl_asset_soft_master where softwareid = " & dtable.Rows(i)("assettypeid") & " and " & POField1 & "='" & drpcategory.SelectedValue & "'"
                        sqladr = New SqlDataAdapter(strfield, con)
                        dtFieldData = New DataTable
                        sqladr.Fill(dtFieldData)
                        If dtFieldData.Rows.Count > 0 Then
                            Dim strtable As New Table
                            strtable.ID = "strtable" & dtable.Rows(i)("assettypeid")
                            strtable.CssClass = "mGrid"
                            strtable.Width = Unit.Percentage(100)
                            Dim strHeaderrow As New TableRow
                            Dim strHeaderCell As New TableCell
                            strHeaderCell.Width = Unit.Percentage(100)
                            strHeaderCell.Style("font-weight") = "Bold"
                            strHeaderCell.ColumnSpan = 7
                            strHeaderrow.CssClass = "mGridHeader"
                            strHeaderCell.CssClass = "whitebg"
                            Dim lblHeader As New Label
                            lblHeader.ID = "lblheader" & dtable.Rows(i)("assettypeid").ToString
                            lblHeader.Text = dtable.Rows(i)("assettypecode").ToString
                            strHeaderCell.Controls.Add(lblHeader)
                            strHeaderrow.Cells.Add(strHeaderCell)
                            strtable.Rows.Add(strHeaderrow)
                            Dim strHeadrow As New TableRow
                            Dim strHeaderCell_1 As New TableCell
                            Dim strHeaderCell_2 As New TableCell
                            Dim strHeaderCell_3 As New TableCell
                            Dim strHeaderCell_4 As New TableCell
                            Dim strHeaderCell_5 As New TableCell
                            Dim strHeaderCell_6 As New TableCell
                            Dim strHeaderCell_7 As New TableCell
                            strHeaderCell_1.Width = Unit.Percentage(20)
                            strHeaderCell_2.Width = Unit.Percentage(20)
                            strHeaderCell_3.Width = Unit.Percentage(15)
                            strHeaderCell_4.Width = Unit.Percentage(15)
                            strHeaderCell_5.Width = Unit.Percentage(15)
                            strHeaderCell_6.Width = Unit.Percentage(10)
                            strHeaderCell_6.Width = Unit.Percentage(5)
                            strHeaderCell_1.Style("text-align") = "center"
                            strHeaderCell_2.Style("text-align") = "center"
                            strHeaderCell_3.Style("text-align") = "center"
                            strHeaderCell_4.Style("text-align") = "center"
                            strHeaderCell_5.Style("text-align") = "center"
                            strHeaderCell_6.Style("text-align") = "center"
                            strHeaderCell_7.Style("text-align") = "center"
                            strHeaderCell_1.Style("font-weight") = "Bold"
                            strHeaderCell_2.Style("font-weight") = "Bold"
                            strHeaderCell_3.Style("font-weight") = "Bold"
                            strHeaderCell_4.Style("font-weight") = "Bold"
                            strHeaderCell_5.Style("font-weight") = "Bold"
                            strHeaderCell_6.Style("font-weight") = "Bold"
                            strHeaderCell_7.Style("font-weight") = "Bold"
                            strHeaderCell_1.CssClass = "tdtext"
                            strHeaderCell_2.CssClass = "tdtext"
                            strHeaderCell_3.CssClass = "tdtext"
                            strHeaderCell_4.CssClass = "tdtext"
                            strHeaderCell_5.CssClass = "tdtext"
                            strHeaderCell_6.CssClass = "tdtext"
                            strHeaderCell_7.CssClass = "tdtext"
                            strHeaderCell_1.Text = "Software Name"
                            strHeaderCell_2.Text = "Warrenty End Date"
                            strHeaderCell_3.Text = "Contract No"
                            strHeaderCell_4.Text = "Contract Vendor"
                            strHeaderCell_5.Text = "Software Type"
                            strHeaderCell_6.Text = "Version"
                            strHeaderCell_7.Text = "Select"
                            strHeadrow.Cells.Add(strHeaderCell_1)
                            strHeadrow.Cells.Add(strHeaderCell_2)
                            strHeadrow.Cells.Add(strHeaderCell_3)
                            strHeadrow.Cells.Add(strHeaderCell_4)
                            strHeadrow.Cells.Add(strHeaderCell_5)
                            strHeadrow.Cells.Add(strHeaderCell_6)
                            strHeadrow.Cells.Add(strHeaderCell_7)
                            strtable.Rows.Add(strHeadrow)
                            For j As Integer = 0 To dtFieldData.Rows.Count - 1
                                Dim strrow As New TableRow
                                Dim strCell_1 As New TableCell
                                Dim strcell_2 As New TableCell
                                Dim strcell_3 As New TableCell
                                Dim strcell_4 As New TableCell
                                Dim strcell_5 As New TableCell
                                Dim strcell_6 As New TableCell
                                Dim strcell_7 As New TableCell
                                strCell_1.Style("text-align") = "center"
                                strcell_2.Style("text-align") = "center"
                                strcell_3.Style("text-align") = "center"
                                strcell_4.Style("text-align") = "center"
                                strcell_5.Style("text-align") = "center"
                                strcell_6.Style("text-align") = "center"
                                strcell_7.Style("text-align") = "center"
                                strCell_1.CssClass = "tdtext"
                                strcell_2.CssClass = "tdtext"
                                strcell_3.CssClass = "tdtext"
                                strcell_4.CssClass = "tdtext"
                                strcell_5.CssClass = "tdtext"
                                strcell_6.CssClass = "tdtext"
                                strcell_7.CssClass = "tdtext"
                                Dim lbl As New Label
                                lbl.ID = "lbl" & dtFieldData.Rows(j)("licid").ToString
                                lbl.Text = dtFieldData.Rows(j)(primaryfield).ToString
                                Dim lbl2 As New Label
                                lbl2.ID = "lblwdate" & dtFieldData.Rows(j)("licid").ToString
                                lbl2.Text = dtFieldData.Rows(j)(0).ToString
                                Dim lbl3 As New Label
                                lbl3.ID = "lblcno" & dtFieldData.Rows(j)("licid").ToString
                                lbl3.Text = dtFieldData.Rows(j)(2).ToString
                                Dim lbl4 As New Label
                                lbl4.ID = "lblvname" & dtFieldData.Rows(j)("licid").ToString
                                lbl4.Text = dtFieldData.Rows(j)(3).ToString
                                Dim lbl5 As New Label
                                lbl5.ID = "lblsoft" & dtFieldData.Rows(j)("licid").ToString
                                lbl5.Text = dtFieldData.Rows(j)(4).ToString
                                Dim lbl6 As New Label
                                lbl6.ID = "lblver" & dtFieldData.Rows(j)("licid").ToString
                                lbl6.Text = dtFieldData.Rows(j)(5).ToString
                                Dim chkdate As New CheckBox
                                chkdate.ID = "chkdate" & dtFieldData.Rows(j)("licid").ToString
                                strCell_1.Controls.Add(lbl)
                                strcell_2.Controls.Add(lbl2)
                                strcell_3.Controls.Add(lbl3)
                                strcell_4.Controls.Add(lbl4)
                                strcell_5.Controls.Add(lbl5)
                                strcell_6.Controls.Add(lbl6)
                                strcell_7.Controls.Add(chkdate)
                                strrow.Cells.Add(strCell_1)
                                strrow.Cells.Add(strcell_2)
                                strrow.Cells.Add(strcell_3)
                                strrow.Cells.Add(strcell_4)
                                strrow.Cells.Add(strcell_5)
                                strrow.Cells.Add(strcell_6)
                                strrow.Cells.Add(strcell_7)
                                strtable.Rows.Add(strrow)
                            Next
                            tddata.Controls.Add(strtable)
                        End If
                        rdr.Close()
                        con.Close()
                    End If
                Next
                trbutton.Visible = True
            Else
                trbutton.Visible = False
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function BindVendor()
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        drpcontractvendor.Items.Clear()
        drpcontractvendor.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select distinct * from tbl_Asset_Vendor where status = 'A'", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpcontractvendor.Items.Add(New ListItem(rdr("VendorName"), rdr("VendorName")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function GetVersion()
        Dim sql As String = ""
        Dim arrlist As New ArrayList()
        con.Open()
        sql = "select aa.attid from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and aa.header = 12"
        cmd = New SqlCommand(sql, con)
        Dim fieldorder As String = cmd.ExecuteScalar
        con.Close()
        cmd.Dispose()
        If fieldorder <> "" Then
            con.Open()
            cmd1 = New SqlCommand("select id,attid,attdesc from tbl_asset_AttributeList where attid = " & fieldorder & "", con)
            rdr = cmd1.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    If arrlist.Contains(rdr("attdesc")) <> True And rdr("attdesc") <> "" Then
                        arrlist.Add(rdr("attdesc"))
                    End If
                End While
            End If
            cmd1.Dispose()
            rdr.Close()
            con.Close()
        End If
        If arrlist.Count > 0 Then
            drpversion.DataSource = arrlist
            drpversion.DataBind()
            drpversion.Items.Insert(0, New ListItem("--Select--", ""))
        End If
    End Function
    Public Function GetSoftwareType()
        Dim sql As String = ""
        Dim arrlist As New ArrayList()
        con.Open()
        sql = "select aa.attid from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and aa.header = 11"
                cmd = New SqlCommand(sql, con)
                Dim fieldorder As String = cmd.ExecuteScalar
                con.Close()
                cmd.Dispose()
                If fieldorder <> "" Then
                    con.Open()
                    cmd1 = New SqlCommand("select id,attid,attdesc from tbl_asset_AttributeList where attid = " & fieldorder & "", con)
                    rdr = cmd1.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    If arrlist.Contains(rdr("attdesc")) <> True And rdr("attdesc") <> "" Then
                        arrlist.Add(rdr("attdesc"))
                    End If
                End While
            End If
                    cmd1.Dispose()
                    rdr.Close()
                    con.Close()
                End If
        If arrlist.Count > 0 Then
            drpsoftwaretype.DataSource = arrlist
            drpsoftwaretype.DataBind()
            drpsoftwaretype.Items.Insert(0, New ListItem("--Select--", ""))
        End If
    End Function

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            Dim sql As String = ""
            Dim updatequery As String = ""
            Dim transactions As String = ""
            con.Open()
            Dim chkassets As Boolean = False
            sql = "select a.assettypeid,assettypecode from tbl_asset_typemaster a, tbl_asset_categorymaster ac where a.catid = ac.catid and ac.groupid = 2"
            sqladr = New SqlDataAdapter(sql, con)
            dtable = New DataTable
            sqladr.Fill(dtable)
            sqladr.Dispose()
            con.Close()
            sql = ""
            Dim dtFieldData As DataTable
            Dim arrlist As New ArrayList()
            If dtable.Rows.Count > 0 Then
                For i As Integer = 0 To dtable.Rows.Count - 1
                    con.Open()
                    sql = "select isnull(aa.fieldorder,'') as fieldorder,aa.header from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and ad.assettypeid = " & dtable.Rows(i)("assettypeid") & " and aa.header in (1,4,5,9,10,11,12) order by convert(int,aa.Header)"
                    cmd = New SqlCommand(sql, con)
                    rdr = cmd.ExecuteReader
                    Dim WarrentyField As String = ""
                    Dim ContractNo As String = ""
                    Dim VendorName As String = ""
                    Dim version As String = ""
                    Dim softtype As String = ""
                    Dim POField As Boolean = False
                    Dim POField1 As String = ""
                    If rdr.HasRows Then
                        While rdr.Read
                            If POField = False Then
                                POField = True
                                POField1 = rdr("fieldorder")
                            End If
                            If rdr("header") = "4" Then
                                WarrentyField = rdr("fieldorder")
                            End If
                            If rdr("header") = "9" Then
                                ContractNo = rdr("fieldorder")
                            End If
                            If rdr("header") = "10" Then
                                VendorName = rdr("fieldorder")
                            End If
                            If rdr("header") = "11" Then
                                softtype = rdr("fieldorder")
                            End If
                            If rdr("header") = "12" Then
                                version = rdr("fieldorder")
                            End If
                        End While
                    End If
                    POField = False
                    con.Close()
                    cmd.Dispose()
                    If WarrentyField <> "" Then
                        con.Open()
                        Dim strfield As String = "select licid from tbl_asset_soft_master where softwareid = " & dtable.Rows(i)("assettypeid") & " and " & POField1 & "='" & drpcategory.SelectedValue & "'"
                        sqladr = New SqlDataAdapter(strfield, con)
                        dtFieldData = New DataTable
                        sqladr.Fill(dtFieldData)
                        If dtFieldData.Rows.Count > 0 Then
                            Dim strtable As New Table
                            strtable = CType(tddata.FindControl("strtable" & dtable.Rows(i)("assettypeid")), Table)
                            For j As Integer = 0 To dtFieldData.Rows.Count - 1
                                Dim chkdate As CheckBox
                                Dim lbl2 As Label
                                Dim lbl4 As Label
                                Dim lbl3 As Label
                                Dim lbl5 As Label
                                Dim lbl6 As Label
                                chkdate = CType(strtable.FindControl("chkdate" & dtFieldData.Rows(j)("licid").ToString), CheckBox)
                                lbl2 = CType(strtable.FindControl("lblwdate" & dtFieldData.Rows(j)("licid").ToString), Label)
                                lbl4 = CType(strtable.FindControl("lblvname" & dtFieldData.Rows(j)("licid").ToString), Label)
                                lbl3 = CType(strtable.FindControl("lblcno" & dtFieldData.Rows(j)("licid").ToString), Label)
                                lbl5 = CType(strtable.FindControl("lblsoft" & dtFieldData.Rows(j)("licid").ToString), Label)
                                lbl6 = CType(strtable.FindControl("lblver" & dtFieldData.Rows(j)("licid").ToString), Label)
                                If chkdate IsNot Nothing Then
                                    If chkdate.Checked = True Then
                                        chkassets = True
                                        updatequery = updatequery & "update tbl_asset_soft_master set " & WarrentyField & "='" & txtdate.Text.Trim() & "'"
                                        If txtcontractno.Text <> "" Then
                                            updatequery = updatequery & " ," & ContractNo & "='" & txtcontractno.Text.Trim & "' "
                                        End If
                                        If drpcontractvendor.SelectedValue <> "" Then
                                            updatequery = updatequery & " ," & VendorName & "='" & drpcontractvendor.SelectedValue & "' "
                                        End If
                                        If softtype <> "" Then
                                            updatequery = updatequery & " ," & softtype & "='" & drpsoftwaretype.SelectedValue & "' "
                                        End If
                                        If version <> "" Then
                                            updatequery = updatequery & " ," & version & "='" & drpversion.SelectedValue & "' "
                                        End If
                                        updatequery = updatequery & " where licid =" & dtFieldData.Rows(j)("licid").ToString() & "" & "|"
                                        transactions = transactions & " insert into tbl_asset_soft_transactions(licenseid,transtype,PoNo,date1,remarks,contractno,vendorid,transcreatedid,transdate,softwaretype,version) values(" & dtFieldData.Rows(j)("licid").ToString() & ",20," & drpcategory.SelectedValue & ",'" & lbl2.Text.Trim() & "','" & txtremarks.Text.Trim() & "','" & lbl3.Text.Trim() & "','" & lbl4.Text.Trim() & "','" & Session("EmpNo") & "','" & DateTime.Now & "','" & lbl5.Text & "','" & lbl6.Text & "')" & "|"
                                    End If
                                End If
                            Next
                        End If
                        rdr.Close()
                        con.Close()
                    End If
                Next
                trbutton.Visible = True
                If chkassets = True Then
                    Dim query1() As String = transactions.Split("|")
                    If query1.Length > 0 Then
                        For m As Integer = 0 To (query1.Length - 2)
                            con1.Open()
                            cmd1 = New SqlCommand(query1(m), con1)
                            cmd1.ExecuteNonQuery()
                            con1.Close()
                        Next
                    End If

                    Dim query() As String = updatequery.Split("|")
                    If query.Length > 0 Then
                        For m As Integer = 0 To (query.Length - 2)
                            con1.Open()
                            cmd1 = New SqlCommand(query(m), con1)
                            cmd1.ExecuteNonQuery()
                            con1.Close()
                        Next
                    End If
                    trmessage.Visible = True
                    lblmessage.Text = "Software Renewal Updated Successfully!"
                    lblmessage.ForeColor = Drawing.Color.Blue
                    drpcategory.SelectedIndex = 0
                    txtdate.Text = ""
                    drpcontractvendor.SelectedIndex = 0
                    drpversion.SelectedIndex = 0
                    drpsoftwaretype.SelectedIndex = 0
                    txtremarks.Text = ""
                    txtcontractno.Text = ""
                    tddata.Controls.Clear()
                Else
                    lblmessage.ForeColor = Drawing.Color.Red
                    trmessage.Visible = True
                    lblmessage.Text = "Please Select Assets!"
                End If
            Else
                trbutton.Visible = False
            End If

        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

End Class
