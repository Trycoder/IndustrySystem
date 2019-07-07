Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Partial Class AssetandTransactionwiseview
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim rdr1 As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim assettypeid As String = ""
    Dim deploytype As String = ""
    Dim DValue As String = ""
    Dim rejtag As String = ""
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("AssetTypeID") IsNot Nothing AndAlso Request.QueryString("TransTypeID") IsNot Nothing AndAlso Request.QueryString("Days") IsNot Nothing Then
            assettypeid = Request.QueryString("AssetTypeID")
            deploytype = Request.QueryString("TransTypeID")
            DValue = Request.QueryString("Days")
            rejtag = Request.QueryString("Rej")
            If deploytype <> "0" Then
                If deploytype = "1" Then
                    deploytype = "A"
                ElseIf deploytype = "2" Then
                    deploytype = "U"
                ElseIf deploytype = "3" Then
                    deploytype = "P"
                ElseIf deploytype = "4" Then
                    deploytype = "N"
                End If
                BindGrid("", "")
            Else
                BindGrid1("", "")
            End If
        End If
    End Sub
    Public Function BindGrid(ByVal sortExpression As String, ByVal direction As String)
        Try
            Dim sql As String = ""
            If rejtag = "1" Then
                rejtag = "rejdate"
            ElseIf rejtag = "0" Then
                rejtag = "issdate"
            End If
            If deploytype <> "N" Then
                sql = " select r.qty,t.assettypecode,r.att1,r.att2,r.att3, (select l.locname from tbl_Asset_location l where l.locid = r.locid) as location, "
                sql = sql & " (select sl.sublocname from tbl_Asset_sublocation sl where sl.sublocid = r.sublocid) as sublocation, "
                sql = sql & " case when r.deploytype = 'A' then 'To Asset' when r.deploytype = 'U' then 'To User'  when r.deploytype = 'P' then 'To Printer' end as deploytype, "
                sql = sql & " (select e.emp_name+' '+ e.emp_initial + '-' + e.emp_number from view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number and hd.complaintid = r.complaintid)  + '(' +"
                sql = sql & " (select  convert(varchar,hd.cdate,106)  from view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number  and hd.complaintid = r.complaintid) + ')' as complaintdetails,"
                sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.consid) + '(' +  convert(varchar,r.reqdate,106) + ')'  requestdetails  from tbl_asset_typemaster t,"
                sql = sql & " tbl_asset_consrequest r where r.constypeid = t.assettypeid and r.constypeid=" & assettypeid & " and r.deploytype='" & deploytype & "' and r." & rejtag & " >= DATEADD(day,-(" & DValue & "),getdate())"
                sqladr = New SqlDataAdapter(sql, con)
                Dim myDataSet As New DataSet()
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
            Else
                sql = " select am.assettypecode,convert(varchar,sum(tr.quantity)) as quantity,case when tr.transtype ='N' then 'Purchase'  end as transtype,"
                sql = sql & " convert(varchar,tr.transdate,106) as Transdate,(select e.emp_name + ' ' + e.emp_Initial from view_sip_employees e where e.emp_number = tr.transby) as Transby,"
                sql = sql & " (select acm.att1 from tbl_Asset_ConsumablesMaster acm where acm.id = tr.transid) as Att1 from tbl_asset_typemaster am,tbl_asset_cons_transactions tr  where am.assettypeid = tr.constypeid and tr.constypeid =" & assettypeid & " and tr.transtype='" & deploytype & "' and tr.transdate >= DATEADD(day,-(" & DValue & "),getdate())"
                sql = sql & " group by am.assettypecode,tr.transtype,tr.transdate,tr.transby,tr.transid"
                sqladr = New SqlDataAdapter(sql, con)
                Dim myDataSet As New DataSet()
                sqladr.Fill(myDataSet)
                con.Close()
                If myDataSet.Tables.Count > 0 Then
                    Dim myDataView As New DataView()
                    myDataView = myDataSet.Tables(0).DefaultView
                    If sortExpression <> String.Empty Then
                        myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
                    End If
                    GridView1.DataSource = myDataView
                    GridView1.DataBind()
                Else
                    GridView1.EmptyDataText = "No data Found"
                    GridView1.DataBind()
                End If
            End If
            
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub grdassets_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdassets.Sorting
        BindGrid(e.SortExpression, sortOrder)
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
    Public Function BindGrid1(ByVal sortExpression As String, ByVal direction As String)
        Try
            dtable = New DataTable
            dtable = LoadDetailsView()
            If dtable.Rows.Count > 0 Then
                GridView2.Columns.Clear()
                GridView2.DataSource = dtable
                For j As Integer = 0 To dtable.Columns.Count - 1
                    Dim s As New BoundField
                    s.DataField = dtable.Columns(j).ToString
                    s.HeaderText = dtable.Columns(j).ToString
                    GridView2.Columns.Add(s)
                Next
                GridView2.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function LoadDetailsView() As DataTable
        Dim sql As String
        sql = ""
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aa.IsRequired = 1 and aad.attid = aa.attid  and aad.AssetTypeId = " & assettypeid & " order by aad.attid asc"
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
        sql = "select ROW_NUMBER() OVER (ORDER BY am.id) AS SNo," & sql & "  from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid and convert(datetime,am.att" & rejtag & ")  between getdate() and DATEADD(day,(" & DValue & "),getdate()) and am.AssetTypeid =" & assettypeid & " order by am.id ASC "
        sqladr = New SqlDataAdapter(sql, con)
        dtable = New DataTable
        sqladr.Fill(dtable)
        Return dtable
    End Function

    Protected Sub GridView2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView2.Sorting
        BindGrid1(e.SortExpression, sortOrder)
    End Sub
End Class
