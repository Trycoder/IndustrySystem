Imports System.Data
Imports System.Data.SqlClient
Partial Class EditAssetData
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim dtableattributes As DataTable
    Dim primaryfieldname As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            bindcategory()
            txtexistvalue.Attributes.Add("readonly", "readonly")
        End If
        lblmessage.Text = ""
        If Session("Usergroup") <> "1" Then
            btnsubmit.Enabled = False
        End If
    End Sub
    Private Function bindcategory()
        Dim sql As String
        con.Open()
        drpcategory.Items.Clear()
        drpcategory.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select * from tbl_Asset_CategoryMaster where groupid = 1", con)
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
                Session("Assets") = ""
            End If
        End If
    End Function

    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpcategory.SelectedIndexChanged
        Try
            If drpcategory.SelectedValue <> "" Then
                BindAssetType(drpcategory.SelectedValue)
                GetAssetDetails(True)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function GetAssetDetails(Optional ByVal AssetType As Boolean = False)
        Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim sql As String
        Dim suggestions As List(Of String) = New List(Of String)()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Try
            sql = "select aa.Fieldorder from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid and  aa.Header = 5 and aa.catid =" & drpcategory.SelectedValue
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
                If AssetType = True Then
                    sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid in('1','2') and am.id = ast.Assetid "
                Else
                    sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & drpAssetType.SelectedValue & " and am.id = ast.Assetid "
                End If
                sql = sql & " order by am.id"
                sqladr = New SqlDataAdapter(sql, con)
                dtable = New DataTable
                sqladr.Fill(dtable)
                If dtable.Rows.Count > 0 Then
                    drpassets.Items.Clear()
                    drpassets.Items.Add(New ListItem("--Select--", ""))
                    For Each dr As DataRow In dtable.Rows
                        drpassets.Items.Add(New ListItem(dr(s).ToString(), dr("id").ToString()))
                    Next
                Else
                    drpassets.Items.Clear()
                    drpassets.Items.Add(New ListItem("--Select--", ""))
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
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
    Public Function BindAssetNo(ByVal assettypeid As String, ByVal catid As String)
        Dim sql As String
        dtable = New DataTable
        assetno.Visible = True
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
                sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & assettypeid & " and am.id = ast.Assetid order by am.att1 asc"
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                drpassets.Items.Clear()
                drpassets.Items.Add(New ListItem("--Select--", ""))
                While rdr.Read
                    drpassets.Items.Add(New ListItem(rdr(s), rdr("id")))
                End While
            End If
        Catch ex As Exception
        Finally
            con.Close()
        End Try
    End Function
    Public Function GetAssetAttributes(ByVal assetcatid As String, ByVal assettypeid As String)
        Try
            Dim sql As String = ""
            If Session("Admingroup") = "1" Then
                sql = "select aa.Fieldorder,aa.AttDesc from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid and aa.catid =" & assetcatid & " and  aad.assettypeid = " & assettypeid
            Else
                sql = "select aa.Fieldorder,aa.AttDesc from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid and aa.catid =" & assetcatid & " and aa.Header <>'8' and aad.assettypeid = " & assettypeid
            End If
            'sql = "select top 1 " & fieldname & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & assettypeid & " and am.id = ast.Assetid and ast.status ='N' "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            drpattributes.Items.Clear()
            drpattributes.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(Sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpattributes.Items.Add(New ListItem(rdr("AttDesc"), rdr("Fieldorder")))
                End While
            End If
            cmd.Dispose()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub drpassets_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassets.SelectedIndexChanged
        Try
            If drpcategory.SelectedValue = "" Then
                Dim myscript1 As String = "alert('Please select Category! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                drpcategory.Focus()
                Exit Sub
            End If
            Try
                If drpassets.SelectedValue <> "" Then
                    Dim assettypeid As String = GetAssetTypeid(drpassets.SelectedValue)
                    If drpAssetType.Items.FindByValue(assettypeid) IsNot Nothing Then
                        drpAssetType.SelectedValue = assettypeid
                        GetAssetAttributes(drpcategory.SelectedValue, drpAssetType.SelectedValue)
                    End If
                End If
            Catch ex As Exception
                Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
            End Try
            txtexistvalue.Text = ""
            txtnewvalue.Text = ""
            drpattributes.SelectedIndex = 0
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function GetAssetTypeid(ByVal assetid As String) As String
        Try
            Dim sql As String
            If con1.State = ConnectionState.Open Then
                con1.Close()
            End If
            sql = "select AssetTypeid from tbl_Asset_Master where id =" & assetid
            con1.Open()
            cmd1 = New SqlCommand(sql, con1)
            Dim assettypeid As String = Convert.ToString(cmd1.ExecuteScalar())
            Return assettypeid
            con1.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function GetAssetValue(ByVal fieldname As String, ByVal assettypeid As String) As String
        Dim sql As String
        Try
            sql = "select top 1 " & fieldname & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & assettypeid & " and am.id = ast.Assetid and ast.status ='N' "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            Dim s As String = Convert.ToString(cmd.ExecuteScalar())
            Return s
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function
    Public Function GetAssetAttributes(ByVal assettypeid As String) As DataTable
        Dim sql As String
        Try
            sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & assettypeid & " order by aad.attid asc"
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

    Protected Sub drpAssetType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAssetType.SelectedIndexChanged
        Try
            If drpAssetType.SelectedValue <> "" Then
                BindAssetNo(drpAssetType.SelectedValue, drpcategory.SelectedValue)
                GetAssetAttributes(drpcategory.SelectedValue, drpAssetType.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drpattributes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpattributes.SelectedIndexChanged
        Try
            If drpassets.SelectedValue = "" Then
                Dim myscript1 As String = "alert('Please Select Asset Type! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                drpassets.Focus()
                Exit Sub
            End If
            trupdate.Visible = True
            trassetvalue.Visible = True
            If drpattributes.SelectedValue <> "" Then
                Try
                    Dim sql As String = ""
                    sql = "select isnull(" & drpattributes.SelectedValue & ",'') from tbl_Asset_Master where id=" & drpassets.SelectedValue
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    cmd = New SqlCommand(sql, con)
                    Dim s As String = cmd.ExecuteScalar
                    txtexistvalue.Text = Convert.ToString(s)
                    cmd.Dispose()
                    con.Close()
                    If UCase(drpattributes.SelectedItem.Text) = UCase("Lock No") Then
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
                        For n As Integer = 0 To arrlist.Count - 1
                            Dim CheckRandamNo As String = GetRadanQuery(drpAssetType.SelectedValue, arrlist(n))
                            If CheckRandamNo = "0" Then
                                If CInt(CheckRandamNo) <> CInt(arrlist(n)) Then
                                    txtnewvalue.Text = arrlist(n)
                                    Exit For
                                End If
                            End If
                        Next
                    Else
                        txtnewvalue.Text = ""
                    End If
                Catch ex As Exception
                    Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
                End Try
            End If
            txtnewvalue.Focus()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnupdate.Click
        Try
            If UCase(drpattributes.SelectedItem.Text) = UCase("Lock No") Then
                Dim CheckRandamNo As String = GetRadanQuery(drpAssetType.SelectedValue, Trim(txtnewvalue.Text))
                If CheckRandamNo = "0" Then
                    txtremarks.Text = txtremarks.Text & vbCrLf & drpassets.SelectedItem.Text & "(" & drpattributes.SelectedItem.Text & ") is changed " & txtexistvalue.Text & " to " & txtnewvalue.Text
                    ViewState("UpdateFields") = ViewState("UpdateFields") & " update tbl_Asset_Master set " & drpattributes.SelectedValue & "='" & txtnewvalue.Text & "' where id =" & drpassets.SelectedValue & "|"
                Else
                    trmessage.Visible = True
                    lblmessage.Text = "The Lock No is Already Exists!"
                    txtnewvalue.Focus()
                    Exit Sub
                End If
            Else
                txtremarks.Text = txtremarks.Text & vbCrLf & drpassets.SelectedItem.Text & "(" & drpattributes.SelectedItem.Text & ") is changed " & txtexistvalue.Text & " to " & txtnewvalue.Text
                ViewState("UpdateFields") = ViewState("UpdateFields") & " update tbl_Asset_Master set " & drpattributes.SelectedValue & "='" & txtnewvalue.Text & "' where id =" & drpassets.SelectedValue & "|"
            End If
            txtnewvalue.Text = ""
            txtnewvalue.Focus()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        Try

            Dim smessage As String = ""
            If Not String.IsNullOrEmpty(ViewState("UpdateFields")) Then
                Dim query() As String = Convert.ToString(ViewState("UpdateFields")).Split("|")
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                For i As Integer = 0 To query.Length - 2
                    cmd = New SqlCommand(query(i), con)
                    cmd.ExecuteNonQuery()
                Next
                cmd = New SqlCommand("insert into tbl_Asset_Transactions(assetid,transtype,date1,remarks,transcreatedid) values(" & drpassets.SelectedValue & ",20,'" & Today.Date.ToShortDateString & "','" & txtremarks.Text.Trim().Replace("'", "''") & "','" & Session("EmpNo") & "') ", con)
                cmd.ExecuteNonQuery()
                smessage = "Asset Modified Successfully!"
                If smessage <> "" Then
                    trmessage.Visible = True
                    lblmessage.Text = smessage
                    drpassets.SelectedIndex = 0
                    drpattributes.SelectedIndex = 0
                    txtexistvalue.Text = ""
                    txtnewvalue.Text = ""
                    txtremarks.Text = ""
                End If
                ViewState("UpdateFields") = ""
            Else
                txtexistvalue.Focus()
                trmessage.Visible = True
                lblmessage.Text = "Asset Condition is Empty!"
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
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
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
End Class
