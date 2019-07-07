Imports System.Data.SqlClient
Imports System.Data
Partial Class ReportInfo
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim sqlad As SqlDataAdapter
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim ds As New DataSet
    Dim dtable As DataTable
    Dim assettypeid As String = ""
    Public Function LoadDetailsView() As DataTable
        Dim sql As String
        sql = "Select  "
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandType = Data.CommandType.Text

        If Session("Admingroup") = "1" Then
            cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where  aad.attid = aa.attid  and aad.AssetTypeId = " & ViewState("AssetType") & " order by aad.attid asc"
        Else
            cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where  aad.attid = aa.attid  and aad.AssetTypeId = " & ViewState("AssetType") & " and aa.Header <>'8' order by aad.attid asc"
        End If
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                sql = sql & "am." & rdr("FieldOrder") & " as [" & rdr("AttDesc") & "],"
            End While
        Else
            sql = sql & "* "
        End If
        cmd.Dispose()
        rdr.Close()
        con.Close()
        sql = Left(sql, Len(sql) - 1)
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        sql = sql & " from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid  and am.AssetTypeid =" & ViewState("AssetType") & " and am.id = " & Request("assetid")
        sqladr = New SqlDataAdapter(sql, con)
        dtable = New DataTable
        sqladr.Fill(dtable)
        con.Close()
        sqladr.Dispose()
        Return dtable
    End Function
    Public Sub BindGrid()
        Try
            dtable = New DataTable
            dtable = LoadDetailsView()
            If dtable.Rows.Count > 0 Then
                grdassets.DataSource = dtable
                'For j As Integer = 0 To dtable.Columns.Count - 5
                '    Dim s As New BoundField
                '    s.DataField = dtable.Columns(j).ToString
                '    s.HeaderText = dtable.Columns(j).ToString
                'Next
                grdassets.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            tabReportinfo.ActiveTabIndex = 0
            con.Open()
            cmd = New SqlCommand("select assettypeid from tbl_asset_master where id=" & Request.QueryString("assetid"), con)
            ViewState("AssetType") = Convert.ToString(cmd.ExecuteScalar())
            con.Close()
            BindGrid()
            ViewState("sortOrder") = ""
            SortGridView("", "")
            BindRepairs("", "")
            BindDeployments("", "")
            BindWarranty("", "")
        End If
    End Sub
    Shared sortExpression As String
    Private Sub SortGridView(ByVal sortExpression As String, ByVal direction As String)
        Dim str As String
        str = "select hd.complaintid,hd.complaint,convert(varchar,hd.cdate,106) as cdate,hd.closetag,(select e.Emp_Name + ' '+ e.emp_initial + '(' + e.Emp_Number + ')'  from view_SIP_Employees e where e.emp_Number = hd.emp_Number) as Emp_Name,"
        str &= " (select e.Emp_Name + ' '+ e.emp_initial + '(' + e.Emp_Number + ')' from view_SIP_Employees e where e.Emp_Number = hd.cons_id) as ConsultantName"
        str &= " from tbl_hd_complaint hd where hd.nodeid =" & Request.QueryString("assetid") & "  order by hd.complaintid desc"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim oAdapter As New SqlDataAdapter(str, con)
        oAdapter.SelectCommand.CommandType = CommandType.Text
        Dim myDataSet As New DataSet()
        oAdapter.Fill(myDataSet)
        Dim myDataView As New DataView()
        myDataView = myDataSet.Tables(0).DefaultView
        If sortExpression <> String.Empty Then
            myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
        End If
        grdcompliants.DataSource = myDataView
        grdcompliants.DataBind()
    End Sub

    Protected Sub grdcompliants_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdcompliants.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Trim(e.Row.Cells(4).Text) = "1" Then
                e.Row.Cells(4).Text = "Closed"
            ElseIf Trim(e.Row.Cells(4).Text) = "0" Then
                e.Row.Cells(4).Text = "Pending"
            End If
            'If Trim(e.Row.Cells(3).Text) <> "" Then
            '    e.Row.Cells(3).Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(e.Row.Cells(3).Text))
            'End If
        End If
    End Sub

    Protected Sub grdcompliants_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdcompliants.Sorting
        SortGridView(e.SortExpression, sortOrder)
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
    Public Function BindRepairs(ByVal sortExpression As String, ByVal direction As String)
        Try
            Dim sql As String
            dtable = New DataTable
            sql = "  select  case trans.transtype when '4' then (select em.emp_name + ' '  + em.Emp_Initial from view_SIP_Employees em "
            sql = sql & " where em.emp_Number = trans.userid) when '5' then (select v.vendorname from tbl_asset_vendor v where v.VendorID = trans.vendorid) when '6' then"
            sql = sql & " (select case when trans.vendorid is null then (select em.Emp_Name from view_SIP_Employees em where em.Emp_Number=trans.userid)"
            sql = sql & " else (select v.vendorname from tbl_asset_vendor v where v.vendorid = trans.vendorid) end)"
            sql = sql & " end as  [Username], case trans.transtype when '4' then"
            sql = sql & " 'Repair(Inhouse)'  when '5' then 'Repair(Outside)' when '6' then 'Return' else '' end as TransType, convert(varchar,trans.date1,106) as RepairDate,(select em.emp_name + ' '  + em.Emp_Initial + '(' + em.Emp_Number + ')' from view_SIP_Employees em where  em.emp_Number = trans.transcreatedid) as ConsultantName,"
            sql = sql & " trans.remarks from tbl_asset_transactions trans,view_SIP_Employees emp, view_assetmaster_status am where "
            sql = sql & " trans.transcreatedid = emp.Emp_Number and   (trans.transtype in('4','5','6')) and  trans.assetid = am.id  "
            sql = sql & " and trans.assetid = '" & Request.QueryString("assetid") & "' order by trans.id desc"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim oAdapter As New SqlDataAdapter(sql, con)
            Dim myDataSet As New DataSet()
            oAdapter.Fill(myDataSet)
            If myDataSet.Tables.Count > 0 Then
                Dim myDataView As New DataView()
                myDataView = myDataSet.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdmaintainance.DataSource = myDataView
                grdmaintainance.DataBind()
            Else
                grdmaintainance.EmptyDataText = "No data Found"
                grdmaintainance.DataBind()
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function BindDeployments(ByVal sortExpression As String, ByVal direction As String)
        Try
            Dim sql As String
            dtable = New DataTable
            sql = " select (select em.emp_Name + ' ' + em.emp_Initial + '(' + emp_Number + ')' from view_SIP_Employees em where em.emp_number = trans.userid) as Emp_Name,"
            sql = sql & " case trans.transtype when '2' then 'Deployment' when '3' then 'Undeployment' when '9' then 'Deployment(Idle)'"
            sql = sql & " when '10' then 'UnDeployment(Idle)' else '' end as TransType,"
            sql = sql & " convert(varchar,trans.date1,106) as Deploydate,"
            sql = sql & " (select em.emp_Name + ' ' + em.emp_Initial + '(' + emp_Number + ')' from view_SIP_Employees em where em.emp_number = trans.transcreatedid)"
            sql = sql & "  as ConsultantName,trans.remarks from tbl_asset_transactions trans,"
            sql = sql & " view_SIP_Employees emp,view_assetmaster_status am where trans.transcreatedid = emp.Emp_Number and  "
            sql = sql & " (trans.transtype in('2','3','9','10')) and  trans.assetid = am.id and trans.assetid = '" & Request.QueryString("assetid") & "' order by trans.id desc"

            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim oAdapter As New SqlDataAdapter(sql, con)
            Dim myDataSet As New DataSet()
            oAdapter.Fill(myDataSet)
            If myDataSet.Tables.Count > 0 Then
                Dim myDataView As New DataView()
                myDataView = myDataSet.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grddeployments.DataSource = myDataView
                grddeployments.DataBind()
            Else
                grddeployments.EmptyDataText = "No data Found"
                grddeployments.DataBind()
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function BindWarranty(ByVal sortExpression As String, ByVal direction As String)
        Try
            Dim sql As String
            Dim warrantystart As String = ""
            dtable = New DataTable
            con.Open()
            cmd = New SqlCommand(" select aa.fieldorder from tbl_asset_attribute_details ad,tbl_asset_attributes aa where ad.attid = aa.attid and aa.header = '3' and ad.assettypeid = (select am.assettypeid from tbl_asset_master am where am.id = '" & Request.QueryString("assetid") & "')", con)
            warrantystart = cmd.ExecuteScalar
            cmd.Dispose()
            con.Close()
            If warrantystart <> "" Then
                sql = " select s.assetid,s.att1,s." & warrantystart & " as warrantystart,h.pono,convert(varchar,h.warrantyend,106) as warranty,h.contractno,(select v.vendorname from tbl_asset_vendor v where v.vendorid = h.vendorid) as vendor,"
                sql = sql & " (select em.emp_Name + ' ' + em.emp_Initial + '(' + emp_Number + ')' from view_SIP_Employees em where em.emp_number = h.transby) as ConsultantName"
                sql = sql & " from tbl_asset_transactionshistory h,view_assetmaster_status s where h.assetid = s.id and h.assetid = '" & Request.QueryString("assetid") & "' order by h.id desc"
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim oAdapter As New SqlDataAdapter(sql, con)
            Dim myDataSet As New DataSet()
            oAdapter.Fill(myDataSet)
            If myDataSet.Tables.Count > 0 Then
                Dim myDataView As New DataView()
                myDataView = myDataSet.Tables(0).DefaultView
                If sortExpression <> String.Empty Then
                    myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
                End If
                grdwarranty.DataSource = myDataView
                grdwarranty.DataBind()
            Else
                grdwarranty.EmptyDataText = "No warranty details in this Asset"
                grdwarranty.DataBind()
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub grdmaintainance_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdmaintainance.Sorting
        BindRepairs(e.SortExpression, sortOrder)
    End Sub

    Protected Sub grddeployments_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grddeployments.Sorting
        BindDeployments(e.SortExpression, sortOrder)
    End Sub

    Protected Sub grdwarranty_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdwarranty.Sorting
        BindWarranty(e.SortExpression, sortOrder)
    End Sub
End Class
