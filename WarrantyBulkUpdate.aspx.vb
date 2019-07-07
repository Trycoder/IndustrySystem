Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Partial Class WarrentyBulkUpdate
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim sql As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If chkselectby.SelectedValue = "P" Then
            trpo.Visible = True
            trasset.Visible = False
        ElseIf chkselectby.SelectedValue = "A" Then
            trasset.Visible = True
            trpo.Visible = False
        End If
        If Not IsPostBack Then
            BindPurchaseOrders()
            BindVendor()
            BindAssets()
        End If
        txtdate.Attributes.Add("readonly", "readonly")
        If drpcategory.SelectedValue <> "" Then
            BulkUpdateItems()
        ElseIf drpasset.SelectedValue <> "" Then
            BulkUpdateItems(drpasset.SelectedValue)
        End If
        lblmessage.Text = ""
    End Sub
    Private Function BindPurchaseOrders()
        con.Open()
        sql = "select a.assettypeid,assettypecode from tbl_asset_typemaster a, tbl_asset_categorymaster ac where a.catid = ac.catid and ac.groupid = 1"
        sqladr = New SqlDataAdapter(sql, con)
        dtable = New DataTable
        sqladr.Fill(dtable)
        con.Close()
        sql = ""
        Dim dtFieldorder As DataTable
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
                    cmd1 = New SqlCommand("select distinct " & fieldorder & " from tbl_asset_master where assettypeid = " & dtable.Rows(i)("assettypeid") & "", con)
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
    Private Function BindAssets()
        con.Open()
        sql = "select distinct id,att1 from tbl_asset_master order by att1"
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        drpasset.Items.Clear()
        drpasset.Items.Insert(0, New ListItem("--Select--", ""))
        If rdr.HasRows Then
            While rdr.Read
                drpasset.Items.Add(New ListItem(rdr("att1"), rdr("id")))
            End While
        End If
    End Function
    Public Function BulkUpdateItems(Optional ByVal assetid As String = "")
        Try
            tddata.Controls.Clear()
            Dim sql As String = ""
            con.Open()
            If assetid = "" Then
                sql = "select a.assettypeid,assettypecode from tbl_asset_typemaster a, tbl_asset_categorymaster ac where a.catid = ac.catid and ac.groupid = 1"
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
                        sql = "select isnull(aa.fieldorder,'') as fieldorder from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and ad.assettypeid = " & dtable.Rows(i)("assettypeid") & " and aa.header in (1,4,5,9,10) order by convert(int,aa.Header)"
                        cmd = New SqlCommand(sql, con)
                        rdr = cmd.ExecuteReader
                        Dim fieldorder As String = ""
                        Dim ContractNo As String = ""
                        Dim VendorName As String = ""
                        Dim POField As Boolean = False
                        Dim POField1 As String = ""
                        If rdr.HasRows Then
                            While rdr.Read
                                If POField = False Then
                                    POField = True
                                    POField1 = rdr("fieldorder")
                                Else
                                    fieldorder = fieldorder & rdr("fieldorder") & ","
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
                            Dim strfield As String = "select " & fieldorder & ",id from tbl_asset_master where assettypeid = " & dtable.Rows(i)("assettypeid") & " and " & POField1 & "='" & drpcategory.SelectedValue & "'"
                            sqladr = New SqlDataAdapter(strfield, con)
                            dtFieldData = New DataTable
                            sqladr.Fill(dtFieldData)
                            If dtFieldData.Rows.Count > 0 Then
                                Dim strtable As New Table
                                strtable.ID = "strtable" & dtable.Rows(i)("assettypeid")
                                strtable.Width = Unit.Percentage(100)
                                strtable.CssClass = "mGrid"
                                Dim strHeaderrow As New TableRow
                                Dim strHeaderCell As New TableCell
                                strHeaderCell.Width = Unit.Percentage(100)
                                strHeaderCell.Style("font-weight") = "Bold"
                                strHeaderCell.ColumnSpan = 5
                                strHeaderrow.CssClass = "mGridHeader"
                                strHeaderCell.CssClass = "whitebg"
                                'strHeaderrow.Style("color") = "red"
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
                                strHeaderCell_1.Width = Unit.Percentage(20)
                                strHeaderCell_2.Width = Unit.Percentage(20)
                                strHeaderCell_3.Width = Unit.Percentage(20)
                                strHeaderCell_4.Width = Unit.Percentage(20)
                                strHeaderCell_5.Width = Unit.Percentage(20)
                                strHeaderCell_1.Style("text-align") = "center"
                                strHeaderCell_2.Style("text-align") = "center"
                                strHeaderCell_3.Style("text-align") = "center"
                                strHeaderCell_4.Style("text-align") = "center"
                                strHeaderCell_5.Style("text-align") = "center"
                                strHeaderCell_1.Style("font-weight") = "Bold"
                                strHeaderCell_2.Style("font-weight") = "Bold"
                                strHeaderCell_3.Style("font-weight") = "Bold"
                                strHeaderCell_4.Style("font-weight") = "Bold"
                                strHeaderCell_5.Style("font-weight") = "Bold"
                                strHeaderCell_1.CssClass = "tdtext"
                                strHeaderCell_2.CssClass = "tdtext"
                                strHeaderCell_3.CssClass = "tdtext"
                                strHeaderCell_4.CssClass = "tdtext"
                                strHeaderCell_5.CssClass = "tdtext"
                                strHeaderCell_1.Text = "ICT Asset No"
                                strHeaderCell_2.Text = "Warrenty End Date"
                                strHeaderCell_3.Text = "Contract No"
                                strHeaderCell_4.Text = "Contract Vendor"
                                strHeaderCell_5.Text = "Select"
                                strHeadrow.Cells.Add(strHeaderCell_1)
                                strHeadrow.Cells.Add(strHeaderCell_2)
                                strHeadrow.Cells.Add(strHeaderCell_3)
                                strHeadrow.Cells.Add(strHeaderCell_4)
                                strHeadrow.Cells.Add(strHeaderCell_5)
                                strtable.Rows.Add(strHeadrow)
                                For j As Integer = 0 To dtFieldData.Rows.Count - 1
                                    Dim strrow As New TableRow
                                    Dim strCell_1 As New TableCell
                                    Dim strcell_2 As New TableCell
                                    Dim strcell_3 As New TableCell
                                    Dim strcell_4 As New TableCell
                                    Dim strcell_5 As New TableCell
                                    strCell_1.Width = Unit.Percentage(20)
                                    strcell_2.Width = Unit.Percentage(20)
                                    strcell_3.Width = Unit.Percentage(20)
                                    strcell_4.Width = Unit.Percentage(20)
                                    strcell_5.Width = Unit.Percentage(20)
                                    strCell_1.Style("text-align") = "center"
                                    strcell_2.Style("text-align") = "center"
                                    strcell_3.Style("text-align") = "center"
                                    strcell_4.Style("text-align") = "center"
                                    strcell_5.Style("text-align") = "center"
                                    strCell_1.CssClass = "tdtext"
                                    strcell_2.CssClass = "tdtext"
                                    strcell_3.CssClass = "tdtext"
                                    strcell_4.CssClass = "tdtext"
                                    strcell_5.CssClass = "tdtext"
                                    Dim lbl As New Label
                                    lbl.ID = "lbl" & dtFieldData.Rows(j)("id").ToString
                                    lbl.Text = dtFieldData.Rows(j)(1).ToString
                                    Dim lbl2 As New Label
                                    lbl2.ID = "lblwdate" & dtFieldData.Rows(j)("id").ToString
                                    lbl2.Text = dtFieldData.Rows(j)(0).ToString
                                    Dim lbl3 As New Label
                                    lbl3.ID = "lblcno" & dtFieldData.Rows(j)("id").ToString
                                    lbl3.Text = dtFieldData.Rows(j)(2).ToString
                                    Dim lbl4 As New Label
                                    lbl4.ID = "lblvname" & dtFieldData.Rows(j)("id").ToString
                                    lbl4.Text = dtFieldData.Rows(j)(3).ToString
                                    Dim chkdate As New CheckBox
                                    chkdate.ID = "chkdate" & dtFieldData.Rows(j)("id").ToString
                                    strCell_1.Controls.Add(lbl)
                                    strcell_2.Controls.Add(lbl2)
                                    strcell_3.Controls.Add(lbl3)
                                    strcell_4.Controls.Add(lbl4)
                                    strcell_5.Controls.Add(chkdate)
                                    strrow.Cells.Add(strCell_1)
                                    strrow.Cells.Add(strcell_2)
                                    strrow.Cells.Add(strcell_3)
                                    strrow.Cells.Add(strcell_4)
                                    strrow.Cells.Add(strcell_5)
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
            Else
                sql = "select am.assettypeid,at.assettypecode from  tbl_asset_master am,tbl_asset_typemaster at where am.assettypeid = at.assettypeid and am.id =" & assetid & ""
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
                        sql = "select isnull(aa.fieldorder,'') as fieldorder from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and ad.assettypeid = " & dtable.Rows(i)("assettypeid") & " and aa.header in (1,4,5,9,10) order by convert(int,aa.Header)"
                        cmd = New SqlCommand(sql, con)
                        rdr = cmd.ExecuteReader
                        Dim fieldorder As String = ""
                        Dim ContractNo As String = ""
                        Dim VendorName As String = ""
                        Dim POField As Boolean = False
                        Dim POField1 As String = ""
                        If rdr.HasRows Then
                            While rdr.Read
                                If POField = False Then
                                    POField = True
                                    POField1 = rdr("fieldorder")
                                Else
                                    fieldorder = fieldorder & rdr("fieldorder") & ","
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
                            Dim strfield As String = "select " & fieldorder & ",id from tbl_asset_master where assettypeid = " & dtable.Rows(i)("assettypeid") & " and id=" & assetid & ""
                            sqladr = New SqlDataAdapter(strfield, con)
                            dtFieldData = New DataTable
                            sqladr.Fill(dtFieldData)
                            If dtFieldData.Rows.Count > 0 Then
                                Dim strtable As New Table
                                strtable.ID = "strtable" & dtable.Rows(i)("assettypeid")
                                strtable.Width = Unit.Percentage(100)
                                strtable.CssClass = "mGrid"
                                Dim strHeaderrow As New TableRow
                                Dim strHeaderCell As New TableCell
                                strHeaderCell.Width = Unit.Percentage(100)
                                strHeaderCell.Style("font-weight") = "Bold"
                                strHeaderCell.ColumnSpan = 5
                                strHeaderrow.CssClass = "mGridHeader"
                                strHeaderCell.CssClass = "whitebg"
                                'strHeaderrow.Style("color") = "red"
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
                                strHeaderCell_1.Width = Unit.Percentage(20)
                                strHeaderCell_2.Width = Unit.Percentage(20)
                                strHeaderCell_3.Width = Unit.Percentage(20)
                                strHeaderCell_4.Width = Unit.Percentage(20)
                                strHeaderCell_5.Width = Unit.Percentage(20)
                                strHeaderCell_1.Style("text-align") = "center"
                                strHeaderCell_2.Style("text-align") = "center"
                                strHeaderCell_3.Style("text-align") = "center"
                                strHeaderCell_4.Style("text-align") = "center"
                                strHeaderCell_5.Style("text-align") = "center"
                                strHeaderCell_1.Style("font-weight") = "Bold"
                                strHeaderCell_2.Style("font-weight") = "Bold"
                                strHeaderCell_3.Style("font-weight") = "Bold"
                                strHeaderCell_4.Style("font-weight") = "Bold"
                                strHeaderCell_5.Style("font-weight") = "Bold"
                                strHeaderCell_1.CssClass = "tdtext"
                                strHeaderCell_2.CssClass = "tdtext"
                                strHeaderCell_3.CssClass = "tdtext"
                                strHeaderCell_4.CssClass = "tdtext"
                                strHeaderCell_5.CssClass = "tdtext"
                                strHeaderCell_1.Text = "ICT Asset No"
                                strHeaderCell_2.Text = "Warrenty End Date"
                                strHeaderCell_3.Text = "Contract No"
                                strHeaderCell_4.Text = "Contract Vendor"
                                strHeaderCell_5.Text = "Select"
                                strHeadrow.Cells.Add(strHeaderCell_1)
                                strHeadrow.Cells.Add(strHeaderCell_2)
                                strHeadrow.Cells.Add(strHeaderCell_3)
                                strHeadrow.Cells.Add(strHeaderCell_4)
                                strHeadrow.Cells.Add(strHeaderCell_5)
                                strtable.Rows.Add(strHeadrow)
                                For j As Integer = 0 To dtFieldData.Rows.Count - 1
                                    Dim strrow As New TableRow
                                    Dim strCell_1 As New TableCell
                                    Dim strcell_2 As New TableCell
                                    Dim strcell_3 As New TableCell
                                    Dim strcell_4 As New TableCell
                                    Dim strcell_5 As New TableCell
                                    strCell_1.Width = Unit.Percentage(20)
                                    strcell_2.Width = Unit.Percentage(20)
                                    strcell_3.Width = Unit.Percentage(20)
                                    strcell_4.Width = Unit.Percentage(20)
                                    strcell_5.Width = Unit.Percentage(20)
                                    strCell_1.Style("text-align") = "center"
                                    strcell_2.Style("text-align") = "center"
                                    strcell_3.Style("text-align") = "center"
                                    strcell_4.Style("text-align") = "center"
                                    strcell_5.Style("text-align") = "center"
                                    strCell_1.CssClass = "tdtext"
                                    strcell_2.CssClass = "tdtext"
                                    strcell_3.CssClass = "tdtext"
                                    strcell_4.CssClass = "tdtext"
                                    strcell_5.CssClass = "tdtext"
                                    Dim lbl As New Label
                                    lbl.ID = "lbl" & dtFieldData.Rows(j)("id").ToString
                                    lbl.Text = dtFieldData.Rows(j)(1).ToString
                                    Dim lbl2 As New Label
                                    lbl2.ID = "lblwdate" & dtFieldData.Rows(j)("id").ToString
                                    lbl2.Text = dtFieldData.Rows(j)(0).ToString
                                    Dim lbl3 As New Label
                                    lbl3.ID = "lblcno" & dtFieldData.Rows(j)("id").ToString
                                    lbl3.Text = dtFieldData.Rows(j)(2).ToString
                                    Dim lbl4 As New Label
                                    lbl4.ID = "lblvname" & dtFieldData.Rows(j)("id").ToString
                                    lbl4.Text = dtFieldData.Rows(j)(3).ToString
                                    Dim chkdate As New CheckBox
                                    chkdate.ID = "chkdate" & dtFieldData.Rows(j)("id").ToString
                                    strCell_1.Controls.Add(lbl)
                                    strcell_2.Controls.Add(lbl2)
                                    strcell_3.Controls.Add(lbl3)
                                    strcell_4.Controls.Add(lbl4)
                                    strcell_5.Controls.Add(chkdate)
                                    strrow.Cells.Add(strCell_1)
                                    strrow.Cells.Add(strcell_2)
                                    strrow.Cells.Add(strcell_3)
                                    strrow.Cells.Add(strcell_4)
                                    strrow.Cells.Add(strcell_5)
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
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            Dim sql As String = ""
            Dim updatequery As String = ""
            Dim transactions As String = ""
            con.Open()
            Dim chkassets As Boolean = False
            If drpasset.SelectedValue = "" Then
                sql = "select a.assettypeid,assettypecode from tbl_asset_typemaster a, tbl_asset_categorymaster ac where a.catid = ac.catid and ac.groupid = 1"
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
                        sql = "select isnull(aa.fieldorder,'') as fieldorder,aa.header from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and ad.assettypeid = " & dtable.Rows(i)("assettypeid") & " and aa.header in (1,4,5,9,10) order by convert(int,aa.Header)"
                        cmd = New SqlCommand(sql, con)
                        rdr = cmd.ExecuteReader
                        Dim WarrentyField As String = ""
                        Dim ContractNo As String = ""
                        Dim VendorName As String = ""
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
                            End While
                        End If
                        POField = False
                        con.Close()
                        cmd.Dispose()
                        If WarrentyField <> "" Then
                            con.Open()
                            Dim strfield As String = "select id from tbl_asset_master where assettypeid = " & dtable.Rows(i)("assettypeid") & " and " & POField1 & "='" & drpcategory.SelectedValue & "'"
                            sqladr = New SqlDataAdapter(strfield, con)
                            dtFieldData = New DataTable
                            sqladr.Fill(dtFieldData)

                            If dtFieldData.Rows.Count > 0 Then
                                Dim strtable As New Table
                                strtable = CType(tddata.FindControl("strtable" & dtable.Rows(i)("assettypeid")), Table)
                                For j As Integer = 0 To dtFieldData.Rows.Count - 1
                                    Dim chkdate As CheckBox
                                    Dim lbl1 As Label
                                    Dim lbl3 As Label
                                    Dim lbl4 As Label
                                    chkdate = CType(strtable.FindControl("chkdate" & dtFieldData.Rows(j)("id").ToString), CheckBox)
                                    lbl1 = CType(strtable.FindControl("lblwdate" & dtFieldData.Rows(j)("id").ToString), Label)
                                    lbl3 = CType(strtable.FindControl("lblcno" & dtFieldData.Rows(j)("id").ToString), Label)
                                    lbl4 = CType(strtable.FindControl("lblvname" & dtFieldData.Rows(j)("id").ToString), Label)
                                    If chkdate IsNot Nothing Then
                                        If chkdate.Checked = True Then
                                            chkassets = True
                                            'updatequery = updatequery & "update tbl_asset_master set " & WarrentyField & "='" & txtdate.Text.Trim() & "'," & ContractNo & "='" & txtcontractno.Text.Trim & "'," & VendorName & "='" & drpcontractvendor.SelectedValue & "' where id =" & dtFieldData.Rows(j)("id").ToString() & "" & "|"
                                            updatequery = updatequery & "update tbl_asset_master set " & WarrentyField & "='" & txtdate.Text.Trim() & "'"
                                            If txtcontractno.Text <> "" Then
                                                updatequery = updatequery & " ," & ContractNo & "='" & txtcontractno.Text.Trim & "' "
                                            End If
                                            If drpcontractvendor.SelectedValue <> "" Then
                                                updatequery = updatequery & " ," & VendorName & "='" & drpcontractvendor.SelectedValue & "' "
                                            End If
                                            updatequery = updatequery & " where id =" & dtFieldData.Rows(j)("id").ToString() & "" & "|"
                                            transactions = transactions & " insert into tbl_asset_transactionshistory(assetid,transtype,pono,warrantyend,remarks,contractno,vendorid,transby,transdate) values(" & dtFieldData.Rows(j)("id").ToString() & ",'20','" & drpcategory.SelectedValue & "','" & lbl1.Text.Trim() & "','" & txtremarks.Text.Trim() & "','" & lbl3.Text.Trim & "','" & lbl4.Text & "','" & Session("EmpNo") & "','" & DateTime.Now & "')" & "|"
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
                        lblmessage.Text = "Warrenty Date Updated Successfully!"
                        lblmessage.ForeColor = Drawing.Color.Blue
                        drpcategory.SelectedIndex = 0
                        txtdate.Text = ""
                        drpcontractvendor.SelectedIndex = 0
                        txtcontractno.Text = ""
                        txtremarks.Text = ""
                        tddata.Controls.Clear()
                    Else
                        lblmessage.ForeColor = Drawing.Color.Red
                        trmessage.Visible = True
                        lblmessage.Text = "Please Select Assets!"
                    End If
                Else
                    trbutton.Visible = False
                End If
            Else
                sql = "select am.assettypeid,at.assettypecode from  tbl_asset_master am,tbl_asset_typemaster at where am.assettypeid = at.assettypeid and am.id =" & drpasset.SelectedValue & ""
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
                        sql = "select isnull(aa.fieldorder,'') as fieldorder,aa.header from tbl_asset_attributes aa,tbl_asset_attribute_details ad where aa.attid = ad.attid and ad.assettypeid = " & dtable.Rows(i)("assettypeid") & " and aa.header in (1,4,5,9,10) order by convert(int,aa.Header)"
                        cmd = New SqlCommand(sql, con)
                        rdr = cmd.ExecuteReader
                        Dim WarrentyField As String = ""
                        Dim ContractNo As String = ""
                        Dim VendorName As String = ""
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
                            End While
                        End If
                        POField = False
                        con.Close()
                        cmd.Dispose()
                        If WarrentyField <> "" Then
                            con.Open()
                            Dim strfield As String = "select id from tbl_asset_master where assettypeid = " & dtable.Rows(i)("assettypeid") & " and  id=" & drpasset.SelectedValue & ""
                            sqladr = New SqlDataAdapter(strfield, con)
                            dtFieldData = New DataTable
                            sqladr.Fill(dtFieldData)

                            If dtFieldData.Rows.Count > 0 Then
                                Dim strtable As New Table
                                strtable = CType(tddata.FindControl("strtable" & dtable.Rows(i)("assettypeid")), Table)
                                For j As Integer = 0 To dtFieldData.Rows.Count - 1
                                    Dim chkdate As CheckBox
                                    Dim lbl1 As Label
                                    Dim lbl3 As Label
                                    Dim lbl4 As Label
                                    chkdate = CType(strtable.FindControl("chkdate" & dtFieldData.Rows(j)("id").ToString), CheckBox)
                                    lbl1 = CType(strtable.FindControl("lblwdate" & dtFieldData.Rows(j)("id").ToString), Label)
                                    lbl3 = CType(strtable.FindControl("lblcno" & dtFieldData.Rows(j)("id").ToString), Label)
                                    lbl4 = CType(strtable.FindControl("lblvname" & dtFieldData.Rows(j)("id").ToString), Label)
                                    If chkdate IsNot Nothing Then
                                        If chkdate.Checked = True Then
                                            chkassets = True
                                            'updatequery = updatequery & "update tbl_asset_master set " & WarrentyField & "='" & txtdate.Text.Trim() & "'," & ContractNo & "='" & txtcontractno.Text.Trim & "'," & VendorName & "='" & drpcontractvendor.SelectedValue & "' where id =" & dtFieldData.Rows(j)("id").ToString() & "" & "|"
                                            updatequery = updatequery & "update tbl_asset_master set " & WarrentyField & "='" & txtdate.Text.Trim() & "'"
                                            If txtcontractno.Text <> "" Then
                                                updatequery = updatequery & " ," & ContractNo & "='" & txtcontractno.Text.Trim & "' "
                                            End If
                                            If drpcontractvendor.SelectedValue <> "" Then
                                                updatequery = updatequery & " ," & VendorName & "='" & drpcontractvendor.SelectedValue & "' "
                                            End If
                                            updatequery = updatequery & " where id =" & dtFieldData.Rows(j)("id").ToString() & "" & "|"
                                            transactions = transactions & " insert into tbl_asset_transactionshistory(assetid,transtype,pono,warrantyend,remarks,contractno,vendorid,transby,transdate) values(" & dtFieldData.Rows(j)("id").ToString() & ",'20','" & drpcategory.SelectedValue & "','" & lbl1.Text.Trim() & "','" & txtremarks.Text.Trim() & "','" & lbl3.Text.Trim & "','" & lbl4.Text & "','" & Session("EmpNo") & "','" & DateTime.Now & "')" & "|"
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
                        lblmessage.Text = "Warrenty Date Updated Successfully!"
                        lblmessage.ForeColor = Drawing.Color.Blue
                        drpcategory.SelectedIndex = 0
                        txtdate.Text = ""
                        drpcontractvendor.SelectedIndex = 0
                        txtcontractno.Text = ""
                        txtremarks.Text = ""
                        tddata.Controls.Clear()
                    Else
                        lblmessage.ForeColor = Drawing.Color.Red
                        trmessage.Visible = True
                        lblmessage.Text = "Please Select Assets!"
                    End If
                Else
                    trbutton.Visible = False
                End If
            End If
            drpcategory.SelectedIndex = 0
            drpasset.SelectedIndex = 0
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
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

    Protected Sub chkselectby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkselectby.SelectedIndexChanged
        tddata.Controls.Clear()
        drpcategory.SelectedIndex = 0
        drpasset.SelectedIndex = 0
    End Sub
End Class
