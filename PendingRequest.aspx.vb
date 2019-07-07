Imports System.Data
Imports System.Data.SqlClient
Partial Class PendingRequest
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con2 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con3 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con4 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim cmd1 As SqlCommand
    Dim cmd2 As SqlCommand
    Dim rdr As SqlDataReader
    Dim rdr1 As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim rowid As String
    Dim GId As String
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                ViewState("sortOrder") = ""
                If Request.QueryString("Reqid") IsNot Nothing Then
                    BindGrid("", "", Request.QueryString("Reqid"))
                Else
                    BindGrid("", "")
                End If
            End If
            If Session("Usergroup") <> "1" Then
                btnUpdate.Enabled = False
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindGrid(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim myDataSet As New DataSet()
            If Session("Isstag") <> "0" Then
                sql = " select r.complaintid,r.id,t.assettypecode,r.att1,r.att2,r.att3,"
                sql = sql & " (select l.locname from tbl_Asset_location l where l.locid = r.locid) as location,"
                sql = sql & " (select sl.sublocname from tbl_Asset_sublocation sl where sl.sublocid = r.sublocid) as sublocation,"
                sql = sql & " case when r.deploytype = 'A' then 'To Asset' when r.deploytype = 'U' then 'To User' "
                sql = sql & " when r.deploytype = 'P' then 'To Printer' end as deploytype,"
                sql = sql & " (select emp_name+' '+emp_initial + '(' + emp_Number + ')' from view_SIP_Employees where emp_number = r.consid) as requestedby,"
                sql = sql & " convert(varchar,r.reqdate,106) as reqdate,"
                sql = sql & " (select e.emp_name+' '+ e.emp_initial + '(' + e.emp_Number + ')' from view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number and hd.complaintid = r.complaintid)"
                sql = sql & " as compby, (select  convert(varchar,hd.cdate,106)  from view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number and hd.complaintid = r.complaintid)"
                sql = sql & " as compdate,r.qty,t.assettypeid,r.locid,r.sublocid,r.apptag,r.isstag from tbl_asset_typemaster t,tbl_asset_consrequest r where r.constypeid = t.assettypeid   "
                If Session("Isstag") = "1" Then
                    sql = sql & " and r.apptag='0' and r.isstag ='0' and r.rejtag <> '1' "
                ElseIf Session("Isstag") = "2" Then
                    sql = sql & " and r.apptag ='1' and r.isstag='0' and r.rejtag <> '1' "
                End If
                If condition <> "" Then
                    sql = sql & " and r.constypeid = " & condition
                End If
                sqladr = New SqlDataAdapter(sql, con)
                sqladr.Fill(myDataSet)
                con.Close()
            End If
            If myDataSet.Tables.Count > 0 Then
                Dim myDataView As New DataView()
                myDataView = myDataSet.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdassets.DataSource = myDataView
                grdassets.DataBind()
            Else
                grdassets.EmptyDataText = "No data Found"
                grdassets.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
          
            Dim row As GridViewRow
            Dim issued As Boolean = False
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            If con1.State = ConnectionState.Open Then
                con1.Close()
            End If
            con1.Open()
            If con2.State = ConnectionState.Open Then
                con2.Close()
            End If
            con2.Open()
            If con3.State = ConnectionState.Open Then
                con3.Close()
            End If
            con3.Open()
            Dim chkcons As Boolean = False
            For Each row In grdassets.Rows
                Dim str As String = ""
                Dim fieldorder As String = ""
                Dim chkapproved As CheckBox = row.FindControl("chkapproved")
                Dim chkissued As CheckBox = row.FindControl("chkissued")
                Dim chkreject As CheckBox = row.FindControl("chkreject")
                Dim lblconstypeid As Label = row.FindControl("lblconstypeid")
                Dim lbllocid As Label = row.FindControl("lbllocid")
                Dim lblsublocid As Label = row.FindControl("lblsublocid")
                Dim lblapprove As Label = row.FindControl("lblapp")
                Dim lblissue As Label = row.FindControl("lblissue")
                Dim lblcid As Label = row.FindControl("lblcid")
                Dim lblassettype As Label = row.FindControl("lblassettype")
                Dim constypeid As String = ""
                Dim cid As String = ""
                Dim msg As String = ""
                If lblconstypeid IsNot Nothing Then
                    constypeid = lblconstypeid.Text
                End If
                If lblconstypeid IsNot Nothing Then
                    cid = lblcid.Text
                End If
                Dim assetid As String = grdassets.DataKeys(row.RowIndex).Value
                If chkapproved.Checked = True AndAlso chkissued.Checked = False Then
                    chkcons = True
                    Str = "update tbl_asset_consrequest set apptag='1',appdate='" & Now & "',appby=" & Session("EmpNo") & " where id=" & assetid & ""
                    cmd = New SqlCommand(Str, con)
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                    Str = ""
                    msg = lblassettype.Text & " Request Approved"
                    Str = "insert into tbl_hd_complaintdet(complaintid,ctag,ddate,emp_number,description) values(" & cid & ",1,'" & Now() & "'," & Session("EmpNo") & ",'" & msg & "')"
                    cmd = New SqlCommand(Str, con3)
                    cmd.ExecuteNonQuery()
                End If
                Dim assetid1 As String = grdassets.DataKeys(row.RowIndex).Value
                If chkissued.Checked = True AndAlso chkreject.Checked = False Then
                    chkcons = True
                    If lblconstypeid IsNot Nothing Then
                        str = "select * from tbl_asset_attribute_details ca,tbl_asset_attributes aa where ca.assettypeid =" & lblconstypeid.Text & "  and ca.attid = aa.attid and ca.seq <> 0 order by aa.attid asc"
                        cmd = New SqlCommand(str, con)
                        rdr = cmd.ExecuteReader
                        If rdr.HasRows Then
                            While rdr.Read
                                fieldorder &= rdr("FieldOrder") & "|"
                            End While
                        End If
                        rdr.Close()
                        fieldorder = Left(fieldorder, Len(fieldorder) - 1)
                        Dim s() As String = Convert.ToString(fieldorder).Split("|")
                        Dim j As Integer = 2
                        Dim fields As String = ""
                        If s.Length > 0 Then
                            For i As Integer = 0 To s.Length - 1
                                fields = fields & s(i) & "='" & row.Cells(j).Text & "' and "
                                j = j + 1
                            Next
                        End If
                        If lbllocid IsNot Nothing Then
                            fields = fields & " locid=" & lbllocid.Text & " and "
                        End If
                        If lblsublocid IsNot Nothing Then
                            fields = fields & " sublocid=" & lblsublocid.Text & " and constypeid=" & lblconstypeid.Text & " "
                        End If
                        str = ""
                        str = "select quantity from  tbl_Asset_Cons_Stock where " & fields
                        con4.Open()
                        cmd = New SqlCommand(str, con4)
                        Dim qty1 As String = cmd.ExecuteScalar()
                        con4.Close()
                        str = ""
                        str = "update tbl_Asset_Cons_Stock set quantity = " & CInt(qty1) - CInt(row.Cells(13).Text) & " where " & fields & "  "
                        cmd1 = New SqlCommand(str, con1)
                        cmd1.ExecuteNonQuery()
                        str = ""
                        cmd.Dispose()
                        str = "update tbl_asset_consrequest set isstag='1',issdate='" & Now & "',issby=" & Session("EmpNo") & ",status='I' where id=" & assetid & ""
                        cmd2 = New SqlCommand(str, con2)
                        cmd2.ExecuteNonQuery()
                        cmd.Dispose()
                        str = ""
                        msg = lblassettype.Text & "  Request Isseed"
                        str = "insert into tbl_hd_complaintdet(complaintid,ctag,ddate,emp_number,description) values(" & cid & ",1,'" & Now() & "'," & Session("EmpNo") & ",'" & msg & "')"
                        cmd = New SqlCommand(str, con3)
                        cmd.ExecuteNonQuery()
                    End If
                End If
                If chkreject.Checked = True Then
                    chkcons = True
                    str = "update tbl_asset_consrequest set rejtag='1',rejby=" & Session("EmpNo") & ",rejdate='" & Date.Today() & "',status='R' where id=" & assetid & ""
                    cmd = New SqlCommand(str, con)
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                    str = ""
                    msg = lblassettype.Text & " Request Rejected"
                    str = "insert into tbl_hd_complaintdet(complaintid,ctag,ddate,emp_number,description) values(" & cid & ",1,'" & Now() & "'," & Session("EmpNo") & ",'" & msg & "')"
                    cmd = New SqlCommand(str, con3)
                    cmd.ExecuteNonQuery()
                ElseIf chkreject.Checked = True And chkapproved.Checked = True Then
                    chkcons = True
                    str = "update tbl_asset_consrequest set rejtag='1',rejby=" & Session("EmpNo") & ",rejdate='" & Date.Today() & "',status='R' where id=" & assetid & ""
                    cmd = New SqlCommand(str, con)
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                    str = ""
                    msg = lblassettype.Text & " Request Rejected"
                    str = "insert into tbl_hd_complaintdet(complaintid,ctag,ddate,emp_number,description) values(" & cid & ",1,'" & Now() & "'," & Session("EmpNo") & ",'" & msg & "')"
                    cmd = New SqlCommand(str, con3)
                    cmd.ExecuteNonQuery()
                End If
            Next
            If chkcons = True Then
                Dim myscript1 As String = "alert('Request Saved Successfully! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            Else
                Dim myscript1 As String = "alert('Please Select the Request! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                Session("Update") = ""
            End If
            con.Close()
            con1.Close()
            con2.Close()
            con3.Close()
            If Request.QueryString("Reqid") IsNot Nothing Then
                BindGrid("", "", Request.QueryString("Reqid"))
            Else
                BindGrid("", "")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub grdassets_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdassets.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
            If Session("Isstag") = "1" Then
                e.Row.Cells(14).Visible = True
                e.Row.Cells(15).Visible = False
                e.Row.Cells(16).Visible = True
            ElseIf Session("Isstag") = "2" Then
                e.Row.Cells(14).Visible = False
                e.Row.Cells(15).Visible = True
                e.Row.Cells(16).Visible = True
            End If
        End If
    End Sub

    Protected Sub grdassets_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdassets.RowDataBound
        Try

            Dim str As String = ""
            Dim fieldorder As String = ""
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lblconstypeid As Label = e.Row.FindControl("lblconstypeid")
                Dim lbllocid As Label = e.Row.FindControl("lbllocid")
                Dim lblsublocid As Label = e.Row.FindControl("lblsublocid")
                Dim lblapprove As Label = e.Row.FindControl("lblapp")
                Dim lblissue As Label = e.Row.FindControl("lblissue")
                Dim chkapproved As CheckBox = e.Row.FindControl("chkapproved")
                Dim chkissued As CheckBox = e.Row.FindControl("chkissued")
                If lblapprove.Text = "0" And lblissue.Text = "1" Then
                    chkissued.Enabled = False
                End If
                If lblapprove.Text = "1" Then
                    chkapproved.Enabled = False
                End If
                Dim id As String = grdassets.DataKeys(e.Row.RowIndex).Value
                If lblconstypeid IsNot Nothing Then
                    str = "select * from tbl_asset_attribute_details ca,tbl_asset_attributes aa where ca.assettypeid =" & lblconstypeid.Text & "  and ca.attid = aa.attid and ca.seq <> 0 order by aa.attid asc"
                    con.Open()
                    cmd = New SqlCommand(str, con)
                    rdr = cmd.ExecuteReader
                    If rdr.HasRows Then
                        While rdr.Read
                            fieldorder &= rdr("FieldOrder") & "|"
                        End While
                    End If
                    fieldorder = Left(fieldorder, Len(fieldorder) - 1)
                    con.Close()
                    Dim s() As String = Convert.ToString(fieldorder).Split("|")
                    Dim j As Integer = 2
                    Dim fields As String = ""
                    If s.Length > 0 Then
                        For i As Integer = 0 To s.Length - 1
                            fields = fields & s(i) & "='" & e.Row.Cells(j).Text & "' and "
                            j = j + 1
                        Next
                    End If
                    If lbllocid IsNot Nothing Then
                        fields = fields & " locid=" & lbllocid.Text & " and "
                    End If
                    If lblsublocid IsNot Nothing Then
                        fields = fields & " sublocid=" & lblsublocid.Text & " and constypeid=" & lblconstypeid.Text
                    End If
                    str = ""
                    str = "select quantity from  tbl_Asset_Cons_Stock where " & fields
                    con.Open()
                    cmd = New SqlCommand(str, con)
                    Dim qty As String = cmd.ExecuteScalar()
                    con.Close()
                    str = ""
                    str = "select isnull(sum(qty),'0') as Quantity from  tbl_asset_consrequest where status='N' and constypeid=" & lblconstypeid.Text
                    con.Open()
                    cmd = New SqlCommand(str, con)
                    Dim qty1 As String = cmd.ExecuteScalar()
                    If qty1 = "" Then
                        qty1 = "0"
                    End If
                    con.Close()

                    If CInt(qty) >= CInt(qty1) Then
                        e.Row.Cells(12).Text = CInt(qty) - CInt(qty1)
                    Else
                        e.Row.Cells(12).Text = "0"
                    End If

                    'If ViewState("qty") IsNot Nothing Then
                    '    ViewState("qty") = CInt(ViewState("qty")) - CInt(qty1)
                    '    e.Row.Cells(12).Text = ViewState("qty")
                    '    ' Session("qty") = CInt(Session("qty")) + CInt(qty1)
                    'Else
                    '    Dim totqty As String = CInt(qty) - CInt(qty1)
                    '    e.Row.Cells(12).Text = totqty
                    '    ViewState("qty") = CInt(totqty)
                    'End If
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub grdassets_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdassets.Sorting
        BindGrid(e.SortExpression, SortOrder)
    End Sub
    Public Property sortOrder() As String
        Get
            If ViewState("sortOrder").ToString() = "desc" Then
                ViewState("sortOrder") = "asc"
            Else
                ViewState("sortOrder") = "desc"
            End If
            Return ViewState("sortOrder").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortOrder") = value
        End Set
    End Property

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
End Class
