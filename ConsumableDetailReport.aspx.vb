Imports System.Data
Imports System.Data.SqlClient
Partial Class ConsumableDetailReport
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ViewState("sortOrder") = ""
            BindEmployees()
            bindAssetype()
            If Request.QueryString("RType") = "1" Then
                lblmessage.Text = "Consumable Report"
            ElseIf Request.QueryString("RType") = "2" Then
                lblmessage.Text = "User Assets/Consumables Report"
            ElseIf Request.QueryString("RType") = "3" Then
                If Request.QueryString("AType") IsNot Nothing Then
                    BindConsumableCount("", "")
                End If
                lblmessage.Text = "Consumable Transactions Report"
            End If
        End If
    End Sub
    Public Function BindEmployees()
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        drpemployee1.Items.Clear()
        drpemployee2.Items.Clear()
        sql = "select * from view_SIP_Employees where emp_status='A'  order by Emp_Name"
        drpemployee1.Items.Add(New ListItem("--Select--", ""))
        drpemployee2.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpemployee1.Items.Add(New ListItem(rdr("Emp_Name") & " " & rdr("Emp_Initial") & "( " & rdr("Emp_Number") & " )", rdr("Emp_Number")))
                drpemployee2.Items.Add(New ListItem(rdr("Emp_Name") & " " & rdr("Emp_Initial") & "( " & rdr("Emp_Number") & " )", rdr("Emp_Number")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function bindAssetype()
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpconsumable1.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select atm.assettypeid,atm.assettypecode from tbl_Asset_TypeMaster atm join tbl_Asset_CategoryMaster  acm on atm.catid = acm.catid where acm.groupid = '3' order by atm.assettypecode asc"
        rdr = cmd.ExecuteReader
        drpconsumable1.Items.Add(New ListItem("--Select--", ""))
        If rdr.HasRows Then
            While rdr.Read
                drpconsumable1.Items.Add(New ListItem(rdr("assettypecode"), rdr("assettypeid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function bindAssetandConsumables(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String = ""
            'Bind Assets
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim myDataSet As New DataSet()
            sql = "select v.assettypecode,v.att1 as AssetNo,v.att2 as FinAssetNo,v.att3 as OrderNo,v.att7 as Warrantystart,v.att8 as warrantyend"
            sql = sql & " from view_assetmaster_status v where v.userid = '" & drpemployee2.SelectedValue & "'"
            sqladr = New SqlDataAdapter(sql, con)
            sqladr.Fill(myDataSet)
            con.Close()
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
            ' Bind Consumables
            sql = " select t.assettypecode,r.att1,r.att2,r.att3, "
            sql = sql & " case when r.deploytype = 'A' then 'To Asset' when r.deploytype = 'U' then 'To User'  when r.deploytype = 'P' then 'To Printer' end as deploytype, convert(varchar,r.reqdate,106) as reqdate, "
            sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.appby) + '(' +  convert(varchar,r.appdate,106) + ')' as ApprovedBy,"
            sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.issby) + '(' +  convert(varchar,r.issdate,106) + ')' as IssuedBy,r.qty from tbl_asset_typemaster t,"
            sql = sql & " tbl_asset_consrequest r,tbl_hd_complaint hd where r.constypeid = t.assettypeid and r.complaintid = hd.complaintid and hd.emp_Number = '" & drpemployee2.SelectedValue & "' and r.status = 'I'"
            sqladr = New SqlDataAdapter(sql, con)
            Dim myDataSet1 As New DataSet()
            sqladr.Fill(myDataSet1)
            con.Close()
            If myDataSet1.Tables.Count > 0 Then
                Dim myDataView1 As New DataView()
                myDataView1 = myDataSet1.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView1.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdconsumables.DataSource = myDataView1
                grdconsumables.DataBind()
            Else
                grdconsumables.EmptyDataText = "No data Found"
                grdconsumables.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
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

    Protected Sub grdassets_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdassets.Sorting
        bindAssetandConsumables(e.SortExpression, sortOrder)
    End Sub

    Protected Sub drpemployee2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpemployee2.SelectedIndexChanged
        Try
            If drpemployee2.SelectedValue <> "" Then
                bindAssetandConsumables("", "")
                trAssets.Visible = True
                trconsumables.Visible = True
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindConsumableCount(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            ' Bind Consumables
            sql = " select r.att1,r.att2,r.att3, "
            sql = sql & " case when r.deploytype = 'A' then 'To Asset' when r.deploytype = 'U' then 'To User'  when r.deploytype = 'P' then 'To Printer' end as deploytype, convert(varchar,r.reqdate,106) as reqdate, "
            sql = sql & " (select e.emp_name+' '+ e.emp_initial + '(' + e.emp_Number + ')' from view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number and hd.complaintid = r.complaintid)"
            sql = sql & " as compby,"
            sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.appby) + '(' +  convert(varchar,r.appdate,106) + ')' as ApprovedBy,"
            sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.issby) + '(' +  convert(varchar,r.issdate,106) + ')' as IssuedBy,r.qty from tbl_asset_typemaster t,"
            sql = sql & " tbl_asset_consrequest r where r.constypeid = t.assettypeid and r.status = 'I' and deploytype='U' and r.constypeid = '" & Request.QueryString("AType") & "' "
            sqladr = New SqlDataAdapter(sql, con)
            Dim myDataSet1 As New DataSet()
            sqladr.Fill(myDataSet1)
            con.Close()
            If myDataSet1.Tables.Count > 0 Then
                Dim myDataView1 As New DataView()
                myDataView1 = myDataSet1.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView1.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdconsumables1.DataSource = myDataView1
                grdconsumables1.DataBind()
            Else
                grdconsumables1.EmptyDataText = "No data Found"
                grdconsumables1.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            BindConsumables("", "")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindConsumablesorAssetsByUser(ByVal constypeid As String) As DataTable
        Try
            Dim sql As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            ' Bind Consumables
            sql = " select t.assettypecode,r.att1,r.att2,r.att3, "
            sql = sql & " case when r.deploytype = 'A' then 'To Asset' when r.deploytype = 'U' then 'To User'  when r.deploytype = 'P' then 'To Printer' end as deploytype, convert(varchar,r.reqdate,106) as reqdate, "
            sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.appby) + '(' +  convert(varchar,r.appdate,106) + ')' as ApprovedBy,"
            sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.issby) + '(' +  convert(varchar,r.issdate,106) + ')' as IssuedBy,r.qty from tbl_asset_typemaster t,"
            sql = sql & " tbl_asset_consrequest r where r.constypeid = t.assettypeid "
            If drporderby.SelectedValue = "U" Then
                sql = sql & " and r.emp_Number = " & constypeid
            ElseIf drporderby.SelectedValue = "C" Then
                sql = sql & " and r.constypeid = " & constypeid
            End If
            If drpemployee1.SelectedValue <> "" Then
                sql = sql & " and r.emp_number='" & drpemployee1.SelectedValue & "'"
            End If
            If txtfromdate.Text.Trim <> "" And txttodate.Text.Trim <> "" Then
                sql = sql & " and r.reqdate between convert(varchar,'" & txtfromdate.Text & "',106) and convert(varchar,'" & txttodate.Text & "',106)"
            End If
            If drpconsumable1.SelectedValue <> "" Then
                sql = sql & " and r.constypeid=" & drpconsumable1.SelectedValue
            End If

            sqladr = New SqlDataAdapter(sql, con)
            Dim dtable As New DataTable()
            sqladr.Fill(dtable)
            Return dtable
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function BindConsumables(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            ' Bind Consumables
            If drporderby.SelectedValue = "U" Then
                sql = "select distinct r.emp_number as EmpNumber,(select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.emp_Number) "
                sql = sql & " as EmpName from tbl_asset_consrequest r where r.emp_Number is not null and r.emp_Number <> ''"
            Else
                sql = "select distinct r.constypeid as  EmpNumber,(select assettypecode from tbl_asset_typemaster where assettypeid = r.constypeid) "
                sql = sql & " as EmpName from tbl_asset_consrequest r where r.constypeid is not null and r.constypeid <> ''"
            End If
            If drpemployee1.SelectedValue <> "" Then
                sql = sql & " and r.emp_number ='" & drpemployee1.SelectedValue & "'"
            End If
            If drpconsumable1.SelectedValue <> "" Then
                sql = sql & " and  r.constypeid =" & drpconsumable1.SelectedValue
            End If
            sqladr = New SqlDataAdapter(sql, con)
            Dim myDataSet1 As New DataSet()
            sqladr.Fill(myDataSet1)
            con.Close()
            If myDataSet1.Tables.Count > 0 Then
                Dim myDataView1 As New DataView()
                myDataView1 = myDataSet1.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView1.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdmainassets.DataSource = myDataView1
                grdmainassets.DataBind()
            Else
                grdmainassets.EmptyDataText = "No data Found"
                grdmainassets.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function


    Protected Sub grdconsumables_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdconsumables.Sorting
        bindAssetandConsumables(e.SortExpression, sortOrder)
    End Sub

    Protected Sub grdconsumables1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdconsumables1.Sorting
        BindConsumableCount(e.SortExpression, sortOrder)
    End Sub

    Protected Sub grdmainassets_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdmainassets.Sorting
        BindConsumables(e.SortExpression, sortOrder)
    End Sub
End Class
