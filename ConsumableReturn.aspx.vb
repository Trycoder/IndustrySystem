Imports System.Data
Imports System.Data.SqlClient

Partial Class ConsumableReturn
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim cmd1 As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim sql As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ViewState("sortOrder") = ""
            BindUsers()
        End If
        trmessage.Visible = False
        lblmessage.Text = ""
    End Sub
    Public Function BindUsers()
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        drpuser.Items.Clear()

        sql = "select * from view_SIP_Employees order by Emp_Name"
        drpuser.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpuser.Items.Add(New ListItem(rdr("Emp_Name") & " " & rdr("Emp_Initial") & "( " & rdr("Emp_Number") & " )", rdr("Emp_Number")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function bindAssetype(Optional ByVal assettype As String = "")
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpassettype.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        If assettype = "ToPrinter" Then
            cmd.CommandText = "select atm.assettypeid,atm.assettypecode from tbl_Asset_TypeMaster atm join tbl_Asset_CategoryMaster acm on atm.catid = acm.catid where acm.groupid = '1' and upper(atm.assettypecode)=upper('Printer')"
        ElseIf assettype = "" Then
            cmd.CommandText = "select atm.assettypeid,atm.assettypecode from tbl_Asset_TypeMaster atm join tbl_Asset_CategoryMaster acm on atm.catid = acm.catid where acm.groupid = '1' and upper(atm.assettypecode) <> upper('Printer')"
        End If
        rdr = cmd.ExecuteReader
        drpassettype.Items.Add(New ListItem("--Select--", 0))
        If rdr.HasRows Then
            While rdr.Read
                drpassettype.Items.Add(New ListItem(rdr("assettypecode"), rdr("assettypeid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function

    Protected Sub rdoundeploytype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoundeploytype.SelectedIndexChanged
        Try
            If rdoundeploytype.SelectedValue = "U" Then
                drpassets.Visible = False
                drpassettype.Visible = False
                drpuser.Visible = True
            ElseIf rdoundeploytype.SelectedValue = "A" Then
                drpassets.Visible = True
                drpassettype.Visible = True
                drpuser.Visible = False
                drpassets.Items.Clear()
                drpassets.Items.Add(New ListItem("--Select--", ""))
                bindAssetype()
            ElseIf rdoundeploytype.SelectedValue = "P" Then
                drpassets.Visible = True
                drpassettype.Visible = True
                drpuser.Visible = False
                drpassets.Items.Clear()
                drpassets.Items.Add(New ListItem("--Select--", ""))
                bindAssetype("ToPrinter")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub FillLocation(ByVal drplist As DropDownList)
        Dim sql As String
        con.Close()
        sql = ""
        sql = "select mas.loccatname + '-' + loc.locname as locname,locid from tbl_Asset_location loc,tbl_Asset_location_master mas where loc.loccatid = mas.id order by loc.locname"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        drplist.Items.Clear()
        drplist.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                'CboLoc.Items.Add(New ListItem(rdr("locname"), rdr("locid")))
                drplist.Items.Add(New ListItem(rdr("locname"), rdr("locid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Sub

    Protected Sub grdassets_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdassets.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drpitems As DropDownList = DirectCast(e.Row.Cells(7).FindControl("drplocation"), DropDownList)
            If drpitems IsNot Nothing Then
                FillLocation(drpitems)
            End If
        End If
    End Sub

    Protected Sub drpassettype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassettype.SelectedIndexChanged
        Try
            If drpassettype.SelectedValue <> "" Then
                GetAssetDetails(drpassettype.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function GetAssetDetails(ByVal AssetType As String)
        Dim suggestions As List(Of String) = New List(Of String)()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Try
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            sql = ""
            sql = " select v.id,v.att1 from view_assetmaster_status v where v.assettypeid = " & AssetType & " order by v.att1 asc"
            sqladr = New SqlDataAdapter(sql, con)
            dtable = New DataTable
            sqladr.Fill(dtable)
            If dtable.Rows.Count > 0 Then
                drpassets.Items.Clear()
                drpassets.Items.Add(New ListItem("--Select--", ""))
                For Each dr As DataRow In dtable.Rows
                    drpassets.Items.Add(New ListItem(dr("att1").ToString(), dr("id").ToString()))
                Next
            Else
                drpassets.Items.Clear()
                drpassets.Items.Add(New ListItem("--Select--", ""))
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function

    Protected Sub drpassets_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassets.SelectedIndexChanged
        Try
            If drpassettype.SelectedValue <> "" Then
                bindconsumables()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function bindconsumables()
        Try
            Dim fields As String = ""
            If rdoundeploytype.SelectedValue = "U" Then
                fields = " and r.emp_Number='" & drpuser.SelectedValue & "'"
            Else
                fields = " and r.nodeid='" & drpassets.SelectedValue & "'"
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            sql = ""
            sql = " select r.id,r.constypeid,m.assettypecode,r.att1,r.att2,r.att3,r.qty, case r.deploytype when 'U' then "
            sql = sql & " (select emp_name + ' ' + emp_Initial from view_sip_employees e where e.Emp_Number = r.emp_number)"
            sql = sql & " when 'A' then (select a.att1 from tbl_asset_master a where a.id = r.nodeid)"
            sql = sql & " when 'P' then (select a.att1 from tbl_asset_master a where a.id = r.nodeid)"
            sql = sql & " end as Asset,"
            sql = sql & " (select emp_name + ' ' + emp_Initial from view_sip_employees e where e.Emp_Number = r.emp_number) as EmpName,"
            sql = sql & " convert(varchar,r.reqdate,106) as compdate"
            sql = sql & " from tbl_asset_consrequest r,tbl_asset_typemaster m where r.constypeid = m.assettypeid and r.status = 'I' " & fields
            sqladr = New SqlDataAdapter(sql, con)
            dtable = New DataTable
            sqladr.Fill(dtable)
            If dtable.Rows.Count > 0 Then
                grdassets.DataSource = dtable
                grdassets.DataBind()
            Else
                grdassets.EmptyDataText = "No Data Found!"
                grdassets.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            Dim boolchkerror As Boolean = False
            Dim message As String = ""
            Dim query As String = ""
            Dim fieldname As String = ""
            Dim confields As String = ""
            Dim changestatus As String = ""
            For Each row In grdassets.Rows
                Dim rowid As String = grdassets.DataKeys(row.DataItemIndex).Value
                Dim lblconsid As Label = row.FindControl("lblconsid")
                Dim chk As CheckBox = row.FindControl("chkapproved")
                Dim drplocation As DropDownList = row.FindControl("drplocation")
                Dim drpsublocation As DropDownList = row.FindControl("drpsublocation")
                Dim drpundeploytype As DropDownList = row.FindControl("drpundeploytype")
                Dim txtqty As TextBox = row.FindControl("txtqty")
                Dim lblqty As Label = row.FindControl("lblqty")
                If chk IsNot Nothing And chk.Checked = True Then
                    If drpundeploytype IsNot Nothing And drpundeploytype.SelectedValue = "" Then
                        message = "Select Deploy Type"
                        drpundeploytype.Focus()
                        boolchkerror = True
                    End If
                    If drplocation IsNot Nothing And drplocation.SelectedValue = "" Then
                        message = "Select Location"
                        boolchkerror = True
                        drplocation.Focus()
                    End If
                    If drpsublocation IsNot Nothing And drpsublocation.SelectedValue = "" Then
                        message = "Select Sub Location"
                        boolchkerror = True
                        drpsublocation.Focus()
                    End If
                    If CInt(lblqty.Text) < CInt(txtqty.Text) = True Then
                        message = "Please Enter Correct Quantity!"
                        boolchkerror = True
                        txtqty.Focus()
                    End If
                    Dim updatestock As String = "update tbl_Asset_Cons_Stock set quantity = "
                    fieldname = "select * from tbl_Asset_Cons_Stock where ConsTypeId =" & lblconsid.Text & " and "
                    Dim dtConsumableatt As New DataTable
                    dtConsumableatt = GetAssetAttributesFields(lblconsid.Text)
                    Dim fieldname1 As String = ""
                    Dim fieldnameinsert As String = ""
                    Dim j As Integer = 1
                    For i As Integer = 0 To dtConsumableatt.Rows.Count - 1
                        confields = confields & "'" & CType(row.FindControl("lblcond" & j), Label).Text & "',"
                        fieldname1 &= dtConsumableatt(i)("FieldOrder") & "='" & CType(row.FindControl("lblcond" & j), Label).Text & "' and "
                        fieldnameinsert &= dtConsumableatt(i)("FieldOrder") & ","
                        j = j + 1
                    Next
                    If confields.Contains(",") Then
                        confields = confields.Remove(confields.Length - 1, 1)
                    End If
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
                        query = query & updatestock & quantity + CInt(txtqty.Text.Trim) & " where id=" & stockid & " *" & "update tbl_asset_consrequest set status='" & drpundeploytype.SelectedValue & "' where id=" & rowid & "" & "|"
                    Else
                        If fieldnameinsert <> "" Then
                            query = query & " insert into tbl_Asset_Cons_Stock (ConsTypeId,Quantity,locid,sublocid," & fieldnameinsert & ") values(" & lblconsid.Text & "," & txtqty.Text.Trim() & "," & drplocation.SelectedValue & ",'" & drpsublocation.SelectedValue & "'," & confields & ")" & "*" & " update tbl_asset_consrequest set status='" & drpundeploytype.SelectedValue & "' where id=" & rowid & "" & "|"
                        Else
                            query = query & "insert into tbl_Asset_Cons_Stock (ConsTypeId,Quantity,locid,sublocid) values(" & lblconsid.Text & "," & txtqty.Text.Trim() & "," & drplocation.SelectedValue & "," & drpsublocation.SelectedValue & ")" & "*" & "update tbl_asset_consrequest set status='" & drpundeploytype.SelectedValue & "' where id=" & rowid & "" & "|"
                        End If
                    End If
                    con.Close()
                End If
                If boolchkerror = False Then
                    Dim qry1() As String = query.Split("|")
                    If qry1.Length > 0 Then
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                        con.Open()
                        For i As Integer = 0 To qry1.Length - 1
                            Dim qry2() As String = qry1(i).Split("*")
                            If qry2.Length > 1 Then
                                For j As Integer = 0 To qry2.Length - 1
                                    cmd = New SqlCommand(qry2(j), con)
                                    cmd.ExecuteNonQuery()
                                Next
                            End If
                        Next
                        cmd.Dispose()
                        con.Close()
                    End If
                Else
                    trmessage.Visible = True
                    lblmessage.Text = message
                End If
            Next
            bindconsumables()
            Dim myscript1 As String = "alert('Consumable(s) Returned Successfully! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drplocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim ddl As DropDownList = DirectCast(sender, DropDownList)
            Dim row As GridViewRow = TryCast(ddl.NamingContainer, GridViewRow)
            If row IsNot Nothing Then
                If CType(row.FindControl("drplocation"), DropDownList).SelectedValue <> "" Then
                    Dim drpsublocation As DropDownList = CType(row.FindControl("drpsublocation"), DropDownList)
                    If drpsublocation IsNot Nothing Then
                        FillsubLocation(CType(row.FindControl("drplocation"), DropDownList).SelectedValue, drpsublocation)
                    End If
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub FillsubLocation(ByVal locid As String, ByVal drp As DropDownList)
        Dim sql As String
        sql = "select sublocname,sublocid from tbl_Asset_sublocation where locid =" & Val(locid) & " order by sublocname"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        drp.Items.Clear()
        drp.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drp.Items.Add(New ListItem(rdr("sublocname"), rdr("sublocid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Sub
    Public Function GetAssetAttributesFields(ByVal constypeid As String) As DataTable
        Dim sql As String
        Try
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            'sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpConsType.SelectedValue & " order by aa.attid asc"
            sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header in('0','7') and aad.AssetTypeId = " & constypeid & " and aad.seq <> 0 order by aa.attid asc"
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

    Protected Sub drpuser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpuser.SelectedIndexChanged
        If drpuser.SelectedValue <> "" Then
            bindconsumables()
        End If
    End Sub
End Class
