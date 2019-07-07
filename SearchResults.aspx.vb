Imports System.Data
Imports System.Data.SqlClient
Partial Class SearchResults
    Inherits System.Web.UI.Page
    Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim SearchName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SearchName = Session("SearchString")
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If SearchName = "" Then
            lblSearchString.Text = SearchName
            Dim txtsearch As TextBox = Master.FindControl("txtsearch")
            If txtsearch IsNot Nothing Then
                SearchName = txtsearch.Text
            End If
        End If
        If Not IsPostBack Then
            lblSearchString.Text = SearchName
            ViewState("sortOrder") = ""
            SortGridView("", "")
        End If
    End Sub
    Protected Sub grdassets_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdassets.RowCommand
        Dim AssetId As String = ""
        If e.CommandName = "Deploy" Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            If row IsNot Nothing Then
                AssetId = grdassets.DataKeys(row.RowIndex).Value
            End If
            Response.Redirect("AssetTransactions.aspx?TransTag=2&AssetId=" & AssetId & "")
        ElseIf e.CommandName = "UnDeploy" Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            If row IsNot Nothing Then
                AssetId = grdassets.DataKeys(row.RowIndex).Value
            End If
            Response.Redirect("AssetTransactions.aspx?TransTag=3&AssetId=" & AssetId & "")
        ElseIf UCase(e.CommandName) = UCase("Repair(Inhouse)") Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            If row IsNot Nothing Then
                AssetId = grdassets.DataKeys(row.RowIndex).Value
            End If
            Response.Redirect("AssetTransactions.aspx?TransTag=4&AssetId=" & AssetId & "")
        ElseIf UCase(e.CommandName) = UCase("Return") Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            If row IsNot Nothing Then
                AssetId = grdassets.DataKeys(row.RowIndex).Value
            End If
            Response.Redirect("AssetTransactions.aspx?TransTag=6&AssetId=" & AssetId & "")
        ElseIf UCase(e.CommandName) = UCase("ChangeLocation") Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            If row IsNot Nothing Then
                AssetId = grdassets.DataKeys(row.RowIndex).Value
            End If
            Response.Redirect("AssetTransactions.aspx?TransTag=11&AssetId=" & AssetId & "")
        End If
    End Sub

    Protected Sub grdassets_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdassets.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgbtn As ImageButton = CType(e.Row.FindControl("imgdeploy"), ImageButton)
            Dim imgrepair As ImageButton = CType(e.Row.FindControl("imgrepair"), ImageButton)
            Dim imgreturn As ImageButton = CType(e.Row.FindControl("imgreturn"), ImageButton)
            Dim imglocation As ImageButton = CType(e.Row.FindControl("imgchlocation"), ImageButton)
            If Trim(e.Row.Cells(7).Text) = "With User" Then
                If imgbtn IsNot Nothing Then
                    imgbtn.AlternateText = "UnDeploy Asset"
                    imgbtn.ImageUrl = "~/Images/Deploy.png"
                    imgbtn.CommandName = "UnDeploy"
                End If
            ElseIf Trim(e.Row.Cells(7).Text) = "Spare" Then
                If imgbtn IsNot Nothing Then
                    imgbtn.AlternateText = "Deploy Asset"
                    imgbtn.ImageUrl = "~/Images/Undeploy.png"
                    imgbtn.CommandName = "Deploy"
                End If
            Else
                If imgbtn IsNot Nothing Then
                    imgbtn.Visible = False
                End If
            End If
            If Trim(e.Row.Cells(7).Text) = "Spare" Then
                If imgrepair IsNot Nothing Then
                    imgrepair.CommandName = "Repair(Inhouse)"
                End If
            Else
                If imgrepair IsNot Nothing Then
                    imgrepair.Visible = False
                End If
            End If
            If Trim(e.Row.Cells(7).Text) = "Repair(Inhouse)" Then
                If imgreturn IsNot Nothing Then
                    imgreturn.CommandName = "Return"
                End If
            Else
                If imgreturn IsNot Nothing Then
                    imgreturn.Visible = False
                End If
            End If
            If Trim(e.Row.Cells(7).Text) = "Spare" Or Trim(e.Row.Cells(7).Text) = "Repair(Inhouse)" Or Trim(e.Row.Cells(7).Text) = "New" Or Trim(e.Row.Cells(7).Text) = "Expired" Or Trim(e.Row.Cells(7).Text) = "Deploy(Idle)" Or Trim(e.Row.Cells(7).Text) = "Undeploy(Idle)" Then
                If imglocation IsNot Nothing Then
                    imglocation.CommandName = "ChangeLocation"
                End If
            Else
                If imglocation IsNot Nothing Then
                    imglocation.Visible = False
                End If
            End If


        End If
    End Sub
    Shared sortExpression As String
    Private Sub SortGridView(ByVal sortExpression As String, ByVal direction As String)
        Dim str As String
        If Not String.IsNullOrEmpty(SearchName) Then
            str = "select stat.att1,stat.assetid,stat.AssetTypeId,emp.Emp_Number,emp.Emp_Name + ' ' + isnull(emp.Emp_Initial,'') as EmpName,emp.Dep_Name,right(emp.Emp_Phone_Ext,4) as Emp_Phone_Ext,"
            str = str & " case when stat.status ='With User(Idle)' then"
            str = str & " (select top 1 alm.loccatname + '-' + al.locname + '-' + subloc.sublocname  from tbl_asset_transactions tr,  "
            str = str & " tbl_Asset_location_master alm,tbl_Asset_location al,tbl_Asset_sublocation subloc  where  tr.assetid = stat.id "
            str = str & " and alm.id = al.loccatid and tr.locid = al.locid and tr.sublocid = subloc.sublocid and tr.transtype <> '20' order by tr.id DESC)"
            str = str & " Else"
            str = str & " case when stat.userid = '0000' then"
            str = str & " (select top 1 alm.loccatname + '-' + al.locname + '-' + subloc.sublocname  from tbl_asset_transactions tr,  "
            str = str & " tbl_Asset_location_master alm,tbl_Asset_location al,tbl_Asset_sublocation subloc  where  tr.assetid = stat.id "
            str = str & " and alm.id = al.loccatid and tr.locid = al.locid and tr.sublocid = subloc.sublocid and tr.transtype <> '20' order by tr.id DESC)"
            str = str & " Else"
            str = str & " (emp.BuildingUnit + '-' + isnull(emp.seatno,''))	"
            str = str & " End"
            str = str & " End"
            str = str & " as BuildingUnit,"
            str = str & " stat.status from View_assetmaster_status stat,view_SIP_Employees emp where stat.userid = emp.Emp_Number and "
            str = str & " (stat.att1 like '%" & SearchName & "%' or stat.att2 like '%" & SearchName & "%' "
            str = str & " or stat.status like '%" & SearchName & "%' or emp.Emp_Number like '%" & SearchName & "%' or emp.Emp_Name like '%" & SearchName & "%' or right(emp.Emp_Phone_Ext,4) like '%" & SearchName & "%')  order by stat.att1"
            ' str = "select stat.att1,stat.assetid,stat.AssetTypeId,emp.Emp_Number,emp.Emp_Name + ' ' + isnull(emp.Emp_Initial,'') as EmpName,emp.Dep_Name,right(emp.Emp_Phone_Ext,4) as Emp_Phone_Ext, case when stat.userid = '0000' then (select top 1 al.locname + '-' + subloc.sublocname  from tbl_asset_transactions tr,  tbl_Asset_location al,tbl_Asset_sublocation subloc  where tr.assetid = stat.id and tr.locid = al.locid and tr.sublocid = subloc.sublocid order by tr.id DESC)  else (emp.BuildingUnit + '-' + isnull(emp.seatno,'')) end as  BuildingUnit,stat.status from View_assetmaster_status stat,view_SIP_Employees emp where stat.userid = emp.Emp_Number and  (emp.Emp_Number like '%9707') "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim oAdapter As New SqlDataAdapter(str, con)
            'oAdapter.SelectCommand.CommandType = CommandType.Text
            Dim myDataSet As New DataSet()
            oAdapter.Fill(myDataSet)
            Dim myDataView As New DataView()
            myDataView = myDataSet.Tables(0).DefaultView
            If sortExpression <> String.Empty Then
                myDataView.Sort = String.Format("{0} {1}", sortExpression, direction)
            End If
            grdassets.DataSource = myDataView
            grdassets.DataBind()
        End If
    End Sub

    Protected Sub grdassets_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdassets.Sorting
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
End Class
