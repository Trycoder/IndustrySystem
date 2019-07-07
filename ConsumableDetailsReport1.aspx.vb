Imports System.Data
Imports System.Data.SqlClient
Partial Class ConsumableDetailsReport1
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ViewState("sortOrder") = ""
            BindEmployees()
        End If
    End Sub
    Public Function BindEmployees()
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        drpemployee.Items.Clear()
        sql = "select * from view_SIP_Employees where emp_status='A'  order by Emp_Name"
        drpemployee.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpemployee.Items.Add(New ListItem(rdr("Emp_Name") & " " & rdr("Emp_Initial") & "( " & rdr("Emp_Number") & " )", rdr("Emp_Number")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function bindAssets(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String = ""
            'Bind Assets
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim myDataSet As New DataSet()
            sql = "select v.assettypecode,v.att1 as AssetNo,v.att2 as FinAssetNo,v.att3 as OrderNo,v.att7 as Warrantystart,v.att8 as warrantyend"
            sql = sql & " from view_assetmaster_status v where v.userid = '" & drpemployee.SelectedValue & "'"
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
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function bindConsumables(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String = ""
            ' Bind Consumables
            sql = " select r.qty,t.assettypecode,r.att1,r.att2,r.att3, "
            sql = sql & " case when r.deploytype = 'A' then 'To Asset' when r.deploytype = 'U' then 'To User'  when r.deploytype = 'P' then 'To Printer' end as deploytype, convert(varchar,r.reqdate,106) as reqdate, "
            sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.appby) + '(' +  convert(varchar,r.appdate,106) + ')' as ApprovedBy,"
            sql = sql & " (select emp_name+' '+emp_initial + '-' + emp_Number from view_SIP_Employees  where emp_number = r.issby) + '(' +  convert(varchar,r.issdate,106) + ')' as IssuedBy,r.qty from tbl_asset_typemaster t,"
            sql = sql & " tbl_asset_consrequest r where r.constypeid = t.assettypeid and r.isstag=1  and r.emp_Number = '" & drpemployee.SelectedValue & "' "
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
        bindAssets(e.SortExpression, sortOrder)
    End Sub
    Protected Sub drpemployee_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpemployee.SelectedIndexChanged
        Try
            If drpemployee.SelectedValue <> "" Then
                bindAssets("", "")
                bindConsumables("", "")
                trAssets.Visible = True
                trconsumables.Visible = True
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub grdconsumables_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdconsumables.Sorting
        bindConsumables(e.SortExpression, sortOrder)
    End Sub
End Class
