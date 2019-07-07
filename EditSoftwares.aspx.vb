Imports System.Data
Imports System.Data.SqlClient
Partial Class EditSoftwares
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim dtableattributes As DataTable
    Dim primaryfieldname As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            bindcategory()
        End If
    End Sub
    Private Function bindcategory()
        Dim sql As String
        con.Open()
        drpcategory.Items.Clear()
        drpcategory.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select * from tbl_Asset_CategoryMaster where groupid = 2", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpcategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
        If Session("Softwares") IsNot Nothing Then
            Dim s() As String = Session("Softwares").ToString.Split("|")
            If s.Length > 4 Then
                assetno.Visible = True
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
                If drpassetfrom.Items.FindByValue(s(3)) IsNot Nothing Then
                    drpassetfrom.SelectedValue = s(3)
                Else
                    drpassetfrom.SelectedIndex = 0
                End If
                If drpassetto.Items.FindByValue(s(4)) IsNot Nothing Then
                    drpassetto.SelectedValue = s(4)
                Else
                    drpassetto.SelectedIndex = 0
                End If
                Session("Softwares") = ""
            End If
        End If
    End Function
    Public Function GetAssetAttributes(ByVal assettypeid As String) As DataTable
        Dim sql As String
        Try
            If Session("Admingroup") = "1" Then
                sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & assettypeid & " order by aad.attid asc"
            Else
                sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & assettypeid & " and aa.Header <> '8' order by aad.attid asc"
            End If
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
    Public Function GetAssetValue(ByVal fieldname As String, ByVal assettypeid As String, ByVal fromassetno As String) As String
        Dim sql As String
        Dim s As String = ""
        Try
            If fieldname <> "" Then
                sql = "select " & fieldname & " from  tbl_asset_soft_master where softwareid =" & assettypeid & " and status ='S' and licid=" & fromassetno
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand(sql, con)
                s = Convert.ToString(cmd.ExecuteScalar())
            End If
            Return s
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function
    Public Function BindAssetNo(ByVal softwareid As String, ByVal catid As String)
        Dim sql As String
        dtable = New DataTable
        assetno.Visible = True
        Try
            sql = "select aa.Fieldorder from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid and  aa.Header = 5 and aa.catid =" & catid
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
                sql = "select licid," & s & " from  tbl_asset_soft_master where softwareid =" & softwareid & " and status ='S' order by licid asc"
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                drpassetfrom.Items.Clear()
                drpassetto.Items.Clear()
                drpassetfrom.Items.Add(New ListItem("--Select--", ""))
                drpassetto.Items.Add(New ListItem("--Select--", ""))
                While rdr.Read
                    drpassetfrom.Items.Add(New ListItem(rdr(s), rdr("licid")))
                    drpassetto.Items.Add(New ListItem(rdr(s), rdr("licid")))
                End While
            End If
        Catch ex As Exception
        Finally
            con.Close()
        End Try
    End Function
    Private Function BindAssetType(ByVal categoryid As String)
        Try
            Dim sql As String
            drpAssetType.Items.Clear()
            drpAssetType.Items.Add(New ListItem("--Select--", ""))
            sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId=" & categoryid & " order by am.CatId"
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

    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpcategory.SelectedIndexChanged
        Try
            If drpcategory.SelectedValue <> "" Then
                BindAssetType(drpcategory.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub LoadDetailsView(ByVal catid As String)
        Dim sql As String
        sql = "Select top 1 "
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandType = Data.CommandType.Text
        '  cmd.CommandText = "select * from tbl_Asset_Attributes where CatId=" & catid & " order by attid"
        cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpAssetType.SelectedValue & " order by aad.attid asc"
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                'If rdr("AttributeName") = "WARRANTY START DATE" Then
                '    sql = sql & " Convert(VARCHAR,Field" & rdr("AttributePos") & ",103) as [" & rdr("AttributeName") & "],"
                'ElseIf rdr("AttributeName") = "WARRANTY END DATE" Then
                '    sql = sql & " Convert(VARCHAR,Field" & rdr("AttributePos") & ",103) as [" & rdr("AttributeName") & "],"
                'Else
                sql = sql & rdr("FieldOrder") & " as [" & rdr("AttDesc") & "],"
                ' End If
            End While
        Else
            sql = sql & "* "
        End If


        rdr.Close()
        con.Close()
        sql = Left(sql, Len(sql) - 1)
        sql = sql & " from tbl_asset_soft_master where softwareid =" & drpAssetType.SelectedValue
        cmd.Dispose()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        Dim vertical As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        'If dtable.Rows.Count > 0 Then
        '    vertical = LoadVerticalData(dtable)
        'End If
        'Response.Write(sql)
        'Response.End()
    End Sub

    Protected Sub drpAssetType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAssetType.SelectedIndexChanged
        If drpcategory.SelectedValue <> "" Then
            If drpAssetType.SelectedValue <> "" Then
                BindAssetNo(drpAssetType.SelectedValue, drpcategory.SelectedValue)
            End If
        End If
    End Sub
    Private Function LoadVerticalData(ByVal dtable1 As DataTable) As DataTable
        'first lets create a new table which will have the new vertical structure
        Dim VerticalTable As New DataTable()
        'get number of rows to make new columns
        For i As Integer = 0 To dtable1.Rows.Count - 1
            VerticalTable.Columns.Add()
        Next
        Dim row As DataRow
        'New rows for the columns
        For j As Integer = 0 To dtable1.Columns.Count - 1
            row = VerticalTable.NewRow()
            row(0) = dtable1.Columns(j).ToString()
            ' set first field of the row to be coulmn fro the table
            For k As Integer = 1 To dtable1.Rows.Count - 1
                'noticed the -1 on the rows count
                row(k) = dtable1.Rows(k - 1)(j)
            Next
            VerticalTable.Rows.Add(row)
        Next
        VerticalTable.AcceptChanges()
        Return VerticalTable
    End Function
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpcategory.SelectedValue = "" Then
                Dim myscript1 As String = "alert('Please select Category! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                drpcategory.Focus()
                Exit Sub
            End If
            If drpAssetType.SelectedValue = "" Then
                Dim myscript1 As String = "alert('Please select Asset Type! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                drpAssetType.Focus()
                Exit Sub
            End If

            If drpassetfrom.SelectedValue <> "" Then
                If drpassetto.SelectedValue <> "" Then
                    dtableattributes = New DataTable
                    dtableattributes = GetAssetAttributes(drpAssetType.SelectedValue)
                    Dim dview As New DataView(dtableattributes)
                    dview.RowFilter = "Header='5'"
                    primaryfieldname = dview(0)("FieldOrder")
                    If dtableattributes.Rows.Count > 0 Then
                        Dim strtable As New Table
                        strtable.ID = "strtable1"
                        strtable.Width = Unit.Percentage(100)
                        For i As Integer = 0 To dtableattributes.Rows.Count - 1
                            Dim strrow As New TableRow
                            Dim strCell_1 As New TableCell
                            ' Dim strCell_2 As New TableCell
                            Dim strCell_3 As New TableCell
                            strCell_1.Width = Unit.Percentage(50)
                            '  strCell_2.Width = Unit.Percentage(1)
                            strCell_3.Width = Unit.Percentage(50)
                            strtable.Style("align") = "center"
                            strCell_1.Style("text-align") = "right"
                            ' strCell_2.Style("text-align") = "center"
                            ' strCell_2.Style("font-weight") = "bold"
                            strCell_3.Style("text-align") = "left"
                            strCell_1.CssClass = "tdtext"
                            'strCell_2.CssClass = "tdtext"
                            strCell_3.CssClass = "tdtext"
                            If dtableattributes(i)("AttType").ToString <> "Text(Variable)" Then
                                Dim lblfield As New Label
                                lblfield.ID = "lblfield" & dtableattributes(i)("attid").ToString
                                lblfield.Text = dtableattributes(i)("attdesc").ToString & " :"
                                strCell_1.Controls.Add(lblfield)
                            Else
                                Dim lnk As New LinkButton
                                lnk.ID = "lnk" & dtableattributes(i)("attid").ToString
                                '  lnk.OnClientClick = "javascript:window.showModalDialog ('EditAssetItems.aspx?AType=" & dtableattributes(i)("AttDesc").ToString & "&Fid=" & drpassetfrom.SelectedValue & "&Tid=" & drpassetto.SelectedValue & "&Fname=" & dtableattributes(i)("FieldOrder").ToString & "&Fcount=" & ViewState("Recordcount") & "&PField=" & primaryfieldname & "', 'popupwindow', 'dialogwidth:500px;dialogheight:500px;dialogleft:300px;dialogtop:300px;scrollbars:yes;resizable:no;status:no;'); return false;"
                                lnk.OnClientClick = "javascript:window.open ('EditSoftwareItems.aspx?AType=" & dtableattributes(i)("AttDesc").ToString & "&Fid=" & drpassetfrom.SelectedValue & "&Tid=" & drpassetto.SelectedValue & "&Fname=" & dtableattributes(i)("FieldOrder").ToString & "&Fcount=" & ViewState("Recordcount") & "&PField=" & primaryfieldname & "&AId=" & drpAssetType.SelectedValue & "', 'popupwindow', 'width=600,height=700,left=300,top=200,scrollbars,resizable=1'); return false;"
                                lnk.Text = dtableattributes(i)("attdesc").ToString & " :"
                                strCell_1.Controls.Add(lnk)
                            End If
                            strrow.Cells.Add(strCell_1)
                            Dim lblvalue As New Label
                            lblvalue.ID = "lblvalue" & dtableattributes(i)("attid").ToString
                            lblvalue.Text = GetAssetValue(dtableattributes(i)("FieldOrder").ToString, drpAssetType.SelectedValue, drpassetfrom.SelectedValue)
                            strCell_3.Controls.Add(lblvalue)
                            strrow.Cells.Add(strCell_3)
                            strtable.Rows.Add(strrow)
                        Next
                        tddata.Controls.Add(strtable)
                    Else

                    End If
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
End Class
