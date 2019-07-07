Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Partial Class AssetTransactions
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con2 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim rdr1 As SqlDataReader
    Dim cmd1 As New SqlCommand
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim transtag As String = ""
    Dim assetid As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            transtag = Request.QueryString("TransTag")
            assetid = Request.QueryString("AssetId")
            If Session("Usergroup") <> "1" Then
                btnsubmit.Enabled = False
            End If

            If Not IsPostBack Then
                tabReportinfo.ActiveTabIndex = 0
                txtdate.Attributes.Add("readonly", "readonly")
                bindcategory()
                BindVendor()
                BindUsers()
                ViewState("Emp_Number") = ""
                ViewState("VendorID") = ""
                If transtag = "2" Then
                    trcuruser.Visible = True
                    trcurlocation.Visible = True
                    trcursubloc.Visible = True
                    trtaruser.Visible = True
                    trtarloc.Visible = True
                    trtarsubloc.Visible = True
                    lbltransactions.Text = "Deployment"
                    ViewState("assetstatus") = "'S'"
                    If assetid <> "" Then
                        BindAssetDetails(assetid)
                    End If
                ElseIf transtag = "3" Then
                    cbouser.Enabled = False
                    trcuruser.Visible = True
                    trcurdept.Visible = True
                    trtaruser.Visible = True
                    trtarloc.Visible = True
                    trtarsubloc.Visible = True
                    lbltransactions.Text = "UnDeployment"
                    ViewState("assetstatus") = "'U','I'"
                    If assetid <> "" Then
                        BindAssetDetails(assetid)
                    End If
                    'Repair(Inhouse)
                ElseIf transtag = "4" Then
                    trcuruser.Visible = True
                    trcurdept.Visible = True
                    trtaruser.Visible = True
                    cbouser.Enabled = False
                    lbltransactions.Text = "Repair(Inhouse)"
                    ViewState("assetstatus") = "'S'"
                    If assetid <> "" Then
                        BindAssetDetails(assetid)
                    End If
                    'Repair(Outside)
                ElseIf transtag = "5" Then
                    ' trcurvendor.Visible = True
                    trtarvendor.Visible = True
                    lbltransactions.Text = "Repair(Outside)"
                    ViewState("assetstatus") = "'S'"
                    ' Return
                ElseIf transtag = "6" Then
                    cbouser.Enabled = False
                    trtaruser.Visible = True
                    trtarloc.Visible = True
                    trtarsubloc.Visible = True
                    lbltransactions.Text = "Return"
                    ViewState("assetstatus") = "'O','R'"
                    If assetid <> "" Then
                        BindAssetDetails(assetid)
                    End If
                ElseIf transtag = "7" Then
                    trtarvendor.Visible = False
                    cbouser.Enabled = False
                    trtaruser.Visible = True
                    trtarloc.Visible = True
                    trtarsubloc.Visible = True
                    lbltransactions.Text = "Retired"
                    BindUsers()
                    If cbouser.Items.FindByValue("0000") IsNot Nothing Then
                        cbouser.SelectedValue = "0000"
                    End If
                    FillLocation()
                    ViewState("assetstatus") = "'U','S','R'"
                    'Sales
                ElseIf transtag = "8" Then
                    trtarvendor.Visible = True
                    lbltransactions.Text = "Sales"
                    ViewState("assetstatus") = "'U','S','R','E'"
                    Dim s As String = ViewState("assetstatus")
                    ' lnkbulkupdate.Attributes.Add("OnClick", "javascript:window.open(BulkTransactions.aspx?status=" & ViewState("assetstatus") & ", 'popupwindow', 'width=420,height=500,left=300,top=300,scrollbars,resizable=1'); return false;")
                    'deploy(Idle)
                ElseIf transtag = "9" Then
                    trcuruser.Visible = True
                    trcurlocation.Visible = True
                    trcursubloc.Visible = True
                    cbouser.Enabled = False
                    trtaruser.Visible = True
                    trtardept.Visible = True
                    lbltransactions.Text = "deploy(Idle)"
                    ViewState("assetstatus") = "'M'"
                    ' undeploy(Idle)
                ElseIf transtag = "10" Then
                    cbouser.Enabled = False
                    trcuruser.Visible = True
                    trcurdept.Visible = True
                    trtaruser.Visible = True
                    trtarloc.Visible = True
                    trtarsubloc.Visible = True
                    lbltransactions.Text = "Undeploy(Idle)"
                    ViewState("assetstatus") = "'U'"
                    'Location Change
                ElseIf transtag = "11" Then
                    trtaruser.Visible = True
                    cbouser.Enabled = False
                    trcuruser.Visible = True
                    trcurlocation.Visible = True
                    trcursubloc.Visible = True
                    trtarloc.Visible = True
                    trtarsubloc.Visible = True
                    lbltransactions.Text = "Change Asset Location"
                    ViewState("assetstatus") = "'S','M','N','E','R'"
                    If assetid <> "" Then
                        BindAssetDetails(assetid)
                    End If
                End If
                If ViewState("assetstatus") = "" Then
                    ViewState("assetstatus") = "'S'"
                End If
                txtdate.Text = String.Format("{0:dd-MMM-yyyy}", Date.Today.Date)
                ' View Compliants 
            End If
            If transtag = "7" Then
                If cbouser.Items.FindByValue("0000") IsNot Nothing Then
                    cbouser.SelectedValue = "0000"
                End If
            End If
            lnkbulkupdate.OnClientClick = "javascript:window.open('BulkTransactions.aspx?CId=" & drpcategory.SelectedValue & "&AId=" & drpassettype.SelectedValue & "&Tid=" & transtag & "', 'popupwindow', 'width=600,height=700,left=300,top=200,scrollbars,resizable=1'); return false;"
            If Session("Assets") IsNot Nothing Then
                btnsubmit.Text = "Bulk Update"
            End If
            lblmessage.Text = ""
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindAssetDetails(ByVal assetid As String)
        Try
            Dim str As String = ""
            If con1.State = ConnectionState.Open Then
                con1.Close()
            End If
            con1.Open()
            Dim rdrasset As SqlDataReader
            str = "select acm.catid,atm.AssetTypeId,v_asset.assetid from View_assetmaster_status v_asset,tbl_Asset_TypeMaster atm,tbl_Asset_CategoryMaster acm where v_asset.AssetTypeId=atm.AssetTypeId and atm.catid = acm.catid and v_asset.assetid = " & assetid & ""
            cmd = New SqlCommand(str, con1)
            rdrasset = cmd.ExecuteReader
            If rdrasset.HasRows Then
                If rdrasset.Read Then
                    If drpcategory.Items.FindByValue(rdrasset("catid")) IsNot Nothing Then
                        drpcategory.SelectedValue = rdrasset("catid")
                        BindAssetType(drpcategory.SelectedValue)
                    End If
                    If Not String.IsNullOrEmpty(transtag) Then
                        BindMaintnance()
                    End If
                    If drpassettype.Items.FindByValue(rdrasset("AssetTypeId")) IsNot Nothing Then
                        drpassettype.SelectedValue = rdrasset("AssetTypeId")
                    End If
                    GetAssetDetails(ViewState("assetstatus"))
                    If cboassetno.Items.FindByValue(rdrasset("assetid")) IsNot Nothing Then
                        cboassetno.SelectedValue = rdrasset("assetid")
                        GetAssetStatus(cboassetno.SelectedValue)
                        BindGrid()
                        SortGridView("", "")
                        BindDeployments("", "")
                        BindRepairs("", "")
                        BindWarranty("", "")
                    End If
                End If
            End If
            rdrasset.Close()
            con1.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpcategory.SelectedIndexChanged
        Try
            If drpcategory.SelectedValue <> "" Then
                BindAssetType(drpcategory.SelectedValue)
                GetAssetDetails(ViewState("assetstatus"), True)
                If Not String.IsNullOrEmpty(transtag) Then
                    BindMaintnance()
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function BindAssetType(ByVal categoryid As String)
        Try
            Dim sql As String
            sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId=" & categoryid & " order by am.CatId"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            drpAssetType.Items.Clear()
            drpAssetType.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpassettype.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function bindcategory()
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpcategory.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select * from tbl_Asset_CategoryMaster where groupid = 1"
        rdr = cmd.ExecuteReader
        drpcategory.Items.Add(New ListItem("--Select--", ""))
        If rdr.HasRows Then
            While rdr.Read
                drpcategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function

    Protected Sub drpassettype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassettype.SelectedIndexChanged
        Try
            If drpcategory.SelectedValue <> "" Then
                If drpassettype.SelectedValue <> "" Then
                    GetAssetDetails(ViewState("assetstatus"))
                End If
            End If
            If transtag = "8" Or transtag = "7" Then
                lnkbulkupdate.Visible = True
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function GetAssetDetails(ByVal status As String, Optional ByVal AssetType As Boolean = False)
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
                    sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid in('1','2') and am.id = ast.Assetid and ast.status in(" & status & ")"
                Else
                    sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & drpassettype.SelectedValue & " and am.id = ast.Assetid and ast.status in(" & status & ")"
                End If
                sql = sql & " order by am.att1 asc"
                sqladr = New SqlDataAdapter(sql, con)
                dtable = New DataTable
                sqladr.Fill(dtable)
                If dtable.Rows.Count > 0 Then
                    cboassetno.Items.Clear()
                    cboassetno.Items.Add(New ListItem("--Select--", ""))
                    For Each dr As DataRow In dtable.Rows
                        cboassetno.Items.Add(New ListItem(dr(s).ToString(), dr("id").ToString()))
                    Next
                Else
                    cboassetno.Items.Clear()
                    cboassetno.Items.Add(New ListItem("--Select--", ""))
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function
    Private Sub PreventDoubleClick()
        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') { ")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("this.value = 'Please wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(Page.GetPostBackEventReference(btnsubmit))
        sb.Append(";")
        btnsubmit.Attributes.Add("onclick", sb.ToString())
    End Sub
    Protected Sub cboassetno_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboassetno.SelectedIndexChanged
        Try
            Dim transtype = Request.QueryString("TransTag")
            If cboassetno.SelectedValue <> "" Then
                GetAssetStatus(cboassetno.SelectedValue)
                Dim assettypeid As String = GetAssetTypeid(cboassetno.SelectedValue)
                If drpassettype.Items.FindByValue(assettypeid) IsNot Nothing Then
                    drpassettype.SelectedValue = assettypeid
                End If
                truserdetailsheader.Visible = True
                truserdetails.Visible = True
                BindGrid()
                If transtag = "6" Or transtag = "7" Then
                    FillLocation()
                End If
                'View Compliants
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand("select assettypeid from tbl_asset_master where id=" & cboassetno.SelectedValue, con)
                ViewState("AssetType") = Convert.ToString(cmd.ExecuteScalar())
                con.Close()
                BindGrid()
                ViewState("sortOrder") = ""
                SortGridView("", "")
                BindRepairs("", "")
                BindDeployments("", "")
                BindWarranty("", "")
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindMaintnance()
        con.Open()
        drpmaintanance.Items.Clear()
        cmd = New SqlCommand("select * from tbl_Asset_Maintainance where activitytype =" & transtag & "", con)
        rdr = cmd.ExecuteReader
        drpmaintanance.Items.Add(New ListItem("--Select--", ""))
        If rdr.HasRows Then
            While rdr.Read
                drpmaintanance.Items.Add(New ListItem(rdr("activity"), rdr("id")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function GetAssetStatus(ByVal assetid As String)
        Dim sqlstr As String = ""
        Dim boolassetadmin As Boolean = False
        '3 - Undeployment, 4 - Repair(Inhouse)
        ' 2 - Deployment
        If Request.QueryString("TransTag") = "2" Then
            sqlstr = "select top 1 at.id,status,isnull(ast.userid,'') as userid,isnull(masloc.loccatname + '-' + loc.locname,'') as locname,"
            sqlstr &= " isnull(subloc.sublocname,'') as sublocation,isnull(emp.Emp_Name,'') as Emp_Name,isnull(emp.Emp_Number,'') as Emp_Number,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(emp.seatno,'') as seatno,isnull(at.date1,'') as date1,isnull(at.remarks,'') as remarks,isnull(mn.activity,'') as  activity"
            sqlstr &= " from tbl_Asset_Status ast, tbl_Asset_Transactions at,tbl_Asset_location_master masloc,tbl_Asset_location loc,"
            sqlstr &= " tbl_Asset_sublocation subloc,view_SIP_Employees emp,tbl_Asset_Maintainance mn  where(loc.loccatid = masloc.id and at.locid = Loc.locid And at.sublocid = subloc.sublocid and at.transtype <> '20')"
            sqlstr &= " and ast.assetid = at.assetid and ast.userid = emp.Emp_Number and at.reasonid = mn.id and ast.assetid = " & assetid & "  and at.transtype in('3','6','0') and ast.status='S' order by at.id desc"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlstr, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    trcurvendor.Visible = False
                    trcurdept.Visible = False
                    trtardept.Visible = False
                    trtarvendor.Visible = False
                    lblcuruser.Text = rdr("Emp_Name") & " " & rdr("Emp_Initial") & " (" & rdr("Emp_Number") & ")"
                    lblcurphone.Text = rdr("Emp_Phone_Ext")
                    lblcuradminloc.Text = rdr("locname")
                    lblcuradminsubloc.Text = rdr("sublocation")
                    lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr("date1"))
                    lblcurtransreason.Text = rdr("activity")
                    lblcurlastremarks.Text = rdr("remarks")
                    ViewState("Emp_Number") = rdr("userid")
                End While
            Else
                lblcuruser.Text = ""
                lblcurphone.Text = ""
                lblcuradminloc.Text = ""
                lblcuradminsubloc.Text = ""
                lblcurtransdate.Text = ""
                lblcurtransreason.Text = ""
                lblcurlastremarks.Text = ""
            End If
            '   FillLocation()
            rdr.Close()
            con.Close()
        ElseIf Request.QueryString("TransTag") = "3" Or Request.QueryString("TransTag") = "4" Then
            ' If cbouser.SelectedItem.Value = "0000" Then
            Dim transtype As String = "'2'"
            sqlstr = "select top 1 isnull(at.locid,'') as locid,isnull(at.sublocid,'') as sublocid from tbl_Asset_Status ast, tbl_Asset_Transactions  at "
            sqlstr = sqlstr & " where  ast.assetid = at.assetid   and  ast.assetid = " & assetid & " "
            sqlstr = sqlstr & " and ast.status in(" & ViewState("assetstatus") & ") and at.transtype <> '20'  order by at.id desc "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlstr, con)
            rdr = cmd.ExecuteReader
            Dim locid As String
            Dim sublocid As String
            If rdr.HasRows Then
                While rdr.Read
                    sqlstr = ""
                    locid = rdr("locid")
                    sublocid = rdr("sublocid")
                    If locid <> "0" And sublocid <> "0" Then
                        sqlstr = " select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name, "
                        sqlstr = sqlstr & " isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,"
                        sqlstr = sqlstr & " isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 "
                        sqlstr = sqlstr & " from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlstr = sqlstr & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn,tbl_Asset_location loc, tbl_Asset_sublocation subloc"
                        sqlstr = sqlstr & " where at.locid = loc.locid and at.sublocid = subloc.sublocid and at.transtype <> '20' and"
                        sqlstr = sqlstr & " (ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlstr = sqlstr & " and at.reasonid = mn.id and ast.assetid = " & assetid & "  "
                        sqlstr = sqlstr & " and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc"
                    Else
                        sqlstr = "select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.BuildingUnit,'') as BuildingUnit ,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(emp.seatno,'') as seatno,isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlstr = sqlstr & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn "
                        sqlstr = sqlstr & " where(ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlstr = sqlstr & " and at.reasonid = mn.id and ast.assetid = " & assetid & "  and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc "
                    End If
                End While 
            End If
            rdr.Close()
            con.Close()
            If con1.State = ConnectionState.Open Then
                con1.Close()
            End If
            con1.Open()
            cmd = New SqlCommand(sqlstr, con1)
            Dim rdr2 As SqlDataReader
            rdr2 = cmd.ExecuteReader
            If rdr2.HasRows Then
                While rdr2.Read
                    trcurvendor.Visible = False
                    trtarvendor.Visible = False
                    lblcuruser.Text = rdr2("Emp_Name") & " " & rdr2("Emp_Initial") & " (" & rdr2("Emp_Number") & ")"
                    If UCase(rdr2("Emp_Name")) = UCase("Asset Admin") Then
                        trcuruser.Visible = True
                        trcurlocation.Visible = True
                        trcursubloc.Visible = True
                        trtarloc.Visible = True
                        trtarsubloc.Visible = True
                        trcurdept.Visible = False
                        GetAdminLocations(rdr2("id"), lblcuradminloc, lblcuradminsubloc)
                        lblcurphone.Text = rdr2("Emp_Phone_Ext")
                        lblcurdept.Text = rdr2("Dep_Name")
                        lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                        lblcurtransreason.Text = rdr2("activity")
                        lblcurlastremarks.Text = rdr2("remarks")
                        ViewState("Emp_Number") = rdr2("Emp_Number")
                    Else
                        trcuruser.Visible = True
                        trcurdept.Visible = True
                        trcurlocation.Visible = False
                        trcursubloc.Visible = False
                        lblcurphone.Text = rdr2("Emp_Phone_Ext")
                        lblcurdept.Text = rdr2("Dep_Name")
                        lblcurlocation.Text = rdr2("BuildingUnit") & "-" & rdr2("seatno")
                        lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                        lblcurtransreason.Text = rdr2("activity")
                        lblcurlastremarks.Text = rdr2("remarks")
                        ViewState("Emp_Number") = rdr2("Emp_Number")
                    End If
                End While
            Else
                lblcuruser.Text = ""
                lblcurphone.Text = ""
                lblcurdept.Text = ""
                lblcurlocation.Text = ""
                lblcurtransdate.Text = ""
                lblcurtransreason.Text = ""
                lblcurlastremarks.Text = ""
            End If
            cbouser.SelectedValue = "0000"
            trtarloc.Visible = True
            trtarsubloc.Visible = True
            FillLocation()
            rdr2.Close()
            con1.Close()
            ' 5 - Repair(outside)
        ElseIf Request.QueryString("TransTag") = "5" Then
            sqlstr = "select top 1 isnull(at.userid,'') as userid, isnull(at.transtype,'') as transtype,isnull(at.locid,'') as locid,isnull(at.sublocid,'') as sublocid from tbl_Asset_Transactions at where at.assetid = " & assetid & "  order by at.id desc"
            Dim userid As String = ""
            Dim locid As String = ""
            Dim sublocid As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlstr, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    userid = rdr("userid")
                    If userid = "0000" Then
                        sqlstr = " select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name, "
                        sqlstr = sqlstr & " isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,"
                        sqlstr = sqlstr & " isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 "
                        sqlstr = sqlstr & " from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlstr = sqlstr & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn,tbl_Asset_location loc, tbl_Asset_sublocation subloc"
                        sqlstr = sqlstr & " where at.locid = loc.locid and at.sublocid = subloc.sublocid and at.transtype <> '20' and"
                        sqlstr = sqlstr & " (ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlstr = sqlstr & " and at.reasonid = mn.id and ast.assetid = " & assetid & " and at.transtype in(" & rdr("transtype") & ") "
                        sqlstr = sqlstr & " and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc"
                    Else
                        sqlstr = "select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.BuildingUnit,'') as BuildingUnit ,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(emp.seatno,'') as seatno,isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlstr = sqlstr & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn "
                        sqlstr = sqlstr & " where(ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlstr = sqlstr & " and at.reasonid = mn.id and ast.assetid = " & assetid & " and at.transtype in(" & rdr("transtype") & ") and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc "
                    End If
                    'End If
                End While
                If con1.State = ConnectionState.Open Then
                    con1.Close()
                End If
                con1.Open()
                cmd = New SqlCommand(sqlstr, con1)
                Dim rdr2 As SqlDataReader
                rdr2 = cmd.ExecuteReader
                If rdr2.HasRows Then
                    While rdr2.Read
                        trtarvendor.Visible = False
                        lblcuruser.Text = rdr2("Emp_Name") & " " & rdr2("Emp_Initial") & " (" & rdr2("Emp_Number") & ")"
                        If UCase(rdr2("Emp_Name")) = UCase("Asset Admin") Then
                            trcuruser.Visible = True
                            trcurlocation.Visible = True
                            trcursubloc.Visible = True
                            trcurdept.Visible = False
                            GetAdminLocations(rdr2("id"), lblcuradminloc, lblcuradminsubloc)
                            lblcurphone.Text = rdr2("Emp_Phone_Ext")
                            lblcurdept.Text = rdr2("Dep_Name")
                            lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                            lblcurtransreason.Text = rdr2("activity")
                            lblcurlastremarks.Text = rdr2("remarks")
                            ViewState("Emp_Number") = rdr2("Emp_Number")
                            trtarvendor.Visible = True
                        Else
                            trtarvendor.Visible = True
                            trcuruser.Visible = True
                            trcurdept.Visible = True
                            trcurlocation.Visible = False
                            trcursubloc.Visible = False
                            lblcurphone.Text = rdr2("Emp_Phone_Ext")
                            lblcurdept.Text = rdr2("Dep_Name")
                            lblcurlocation.Text = rdr2("BuildingUnit") & "-" & rdr2("seatno")
                            lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                            lblcurtransreason.Text = rdr2("activity")
                            lblcurlastremarks.Text = rdr2("remarks")
                            ViewState("Emp_Number") = rdr2("Emp_Number")
                        End If
                    End While
                Else
                    lblcuruser.Text = ""
                    lblcurphone.Text = ""
                    lblcurdept.Text = ""
                    lblcurlocation.Text = ""
                    lblcurtransdate.Text = ""
                    lblcurtransreason.Text = ""
                    lblcurlastremarks.Text = ""
                End If
                rdr2.Close()
                con1.Close()
            End If
            FillLocation()
            rdr.Close()
            con.Close()
            ' Return
        ElseIf Request.QueryString("TransTag") = "6" Then
            sqlstr = "select  top 1 isnull(at.transtype,'') as transtype from tbl_Asset_Transactions at where at.assetid = " & assetid & "  and at.transtype in(4,5)  order by at.id desc"
            Dim vendorname As String = ""
            Dim locid As String = ""
            Dim sublocid As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlstr, con)
            rdr = cmd.ExecuteReader
            Dim m As Integer = 0
            If rdr.HasRows Then
                While rdr.Read
                    m = m + 1
                    If rdr("transtype") = "4" Then
                        sqlstr = ""
                        sqlstr = " select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name, "
                        sqlstr = sqlstr & " isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,"
                        sqlstr = sqlstr & " isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 "
                        sqlstr = sqlstr & " from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlstr = sqlstr & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn,tbl_Asset_location loc, tbl_Asset_sublocation subloc"
                        sqlstr = sqlstr & " where at.locid = loc.locid and at.sublocid = subloc.sublocid and at.transtype <> '20' and"
                        sqlstr = sqlstr & " (ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlstr = sqlstr & " and at.reasonid = mn.id and ast.assetid = " & assetid & " and at.transtype in(" & rdr("transtype") & ") "
                        sqlstr = sqlstr & " and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc"
                        If con1.State = ConnectionState.Open Then
                            con1.Close()
                        End If
                        con1.Open()
                        cmd1 = New SqlCommand(sqlstr, con1)
                        Dim rdrrepairI As SqlDataReader
                        rdrrepairI = cmd1.ExecuteReader
                        If rdrrepairI.HasRows Then
                            While rdrrepairI.Read
                                trcuruser.Visible = True
                                trcurlocation.Visible = True
                                trcursubloc.Visible = True
                                trcurdept.Visible = False
                                GetAdminLocations(rdrrepairI("id"), lblcuradminloc, lblcuradminsubloc)
                                lblcuruser.Text = rdrrepairI("Emp_Name") & " " & rdrrepairI("Emp_Initial") & " (" & rdrrepairI("Emp_Number") & ")"
                                lblcurphone.Text = rdrrepairI("Emp_Phone_Ext")
                                lblcurdept.Text = rdrrepairI("Dep_Name")
                                lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdrrepairI("date1"))
                                lblcurtransreason.Text = rdrrepairI("activity")
                                lblcurlastremarks.Text = rdrrepairI("remarks")
                                ViewState("Emp_Number") = rdrrepairI("Emp_Number")
                                ' trtarvendor.Visible = True
                            End While
                            con1.Close()
                            rdrrepairI.Close()
                        End If
                    ElseIf rdr("transtype") = "5" Then
                        sqlstr = " select top 1  at.id,ven.VendorID,isnull(ven.vendorname,'') as vendorname,isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 "
                        sqlstr = sqlstr & " from tbl_Asset_Status ast, tbl_Asset_Transactions at,tbl_Asset_Vendor ven,tbl_Asset_Maintainance mn where at.vendorid = ven.VendorID and at.reasonid = mn.id"
                        sqlstr = sqlstr & " and ast.assetid = " & assetid & " and at.transtype in(" & rdr("transtype") & ") and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc"
                        If con1.State = ConnectionState.Open Then
                            con1.Close()
                        End If
                        con1.Open()
                        cmd1 = New SqlCommand(sqlstr, con1)
                        Dim rdr2 As SqlDataReader
                        rdr2 = cmd1.ExecuteReader
                        If rdr2.HasRows Then
                            While rdr2.Read
                                trcurvendor.Visible = True
                                trcuruser.Visible = False
                                trcurlocation.Visible = False
                                trcurdept.Visible = False
                                trcursubloc.Visible = False
                                lblvendor.Text = rdr2("VendorName")
                                lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                                lblcurtransreason.Text = rdr2("activity")
                                lblcurlastremarks.Text = rdr2("remarks")
                                ViewState("VendorID") = rdr2("VendorID")
                            End While
                        End If
                    Else
                        lblvendor.Text = ""
                        lblcuruser.Text = ""
                        lblcurphone.Text = ""
                        lblcurdept.Text = ""
                        lblcurtransdate.Text = ""
                        lblcurtransreason.Text = ""
                        lblcurlastremarks.Text = ""
                        lblcuradminloc.Text = ""
                        lblcuradminsubloc.Text = ""
                    End If
                End While
                rdr.Close()
                con.Close()
            End If
            If cbouser.Items.FindByValue("0000") IsNot Nothing Then
                cbouser.SelectedValue = "0000"
                trtarloc.Visible = True
                trtarsubloc.Visible = True
            End If
            FillLocation()
            ' rdr.Close()
            ' con.Close()
            'Retired
        ElseIf Request.QueryString("TransTag") = "7" Then
            Dim sqlquery As String = ""
            Dim sqlquery1 As String = ""
            sqlquery = "select  top 1 isnull(at.transtype,'') as transtype,isnull(at.userid,'') as userid from tbl_Asset_Transactions at where at.assetid = " & assetid & "   order by at.id desc"
            Dim vendorname As String = ""
            Dim locid As String = ""
            Dim sublocid As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlquery, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    If rdr("userid") = "0000" Then
                        sqlquery1 = " select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name, "
                        sqlquery1 = sqlquery1 & " isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,"
                        sqlquery1 = sqlquery1 & " isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 "
                        sqlquery1 = sqlquery1 & " from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlquery1 = sqlquery1 & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn,tbl_Asset_location loc, tbl_Asset_sublocation subloc"
                        sqlquery1 = sqlquery1 & " where at.locid = loc.locid and at.sublocid = subloc.sublocid and at.transtype <> '20' and"
                        sqlquery1 = sqlquery1 & " (ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlquery1 = sqlquery1 & " and at.reasonid = mn.id and ast.assetid = " & assetid & " and at.transtype in(" & rdr("transtype") & ") "
                        sqlquery1 = sqlquery1 & " and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc"
                    Else
                        sqlquery1 = "select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.BuildingUnit,'') as BuildingUnit ,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(emp.seatno,'') as seatno,isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlquery1 = sqlquery1 & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn "
                        sqlquery1 = sqlquery1 & " where(ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlquery1 = sqlquery1 & " and at.reasonid = mn.id and ast.assetid = " & assetid & " and at.transtype in(" & rdr("transtype") & ") and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc "
                    End If
                    If con1.State = ConnectionState.Open Then
                        con1.Close()
                    End If
                    con1.Open()
                    Dim rdr2 As SqlDataReader
                    cmd1 = New SqlCommand(sqlquery1, con1)
                    rdr2 = cmd1.ExecuteReader
                    If rdr2.HasRows Then
                        While rdr2.Read
                            lblcuruser.Text = rdr2("Emp_Name") & " " & rdr2("Emp_Initial") & " (" & rdr2("Emp_Number") & ")"
                            If UCase(rdr2("Emp_Name")) = UCase("Asset Admin") Then
                                trcuruser.Visible = True
                                trcurlocation.Visible = True
                                trcursubloc.Visible = True
                                trcurdept.Visible = False
                                GetAdminLocations(rdr2("id"), lblcuradminloc, lblcuradminsubloc)
                                lblcurphone.Text = rdr2("Emp_Phone_Ext")
                                lblcurdept.Text = rdr2("Dep_Name")
                                lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                                lblcurtransreason.Text = rdr2("activity")
                                lblcurlastremarks.Text = rdr2("remarks")
                                ViewState("Emp_Number") = rdr2("Emp_Number")
                            Else
                                trcurlocation.Visible = False
                                trcursubloc.Visible = False
                                trcuruser.Visible = True
                                trcurdept.Visible = True
                                trtaruser.Visible = True
                                lblcurphone.Text = rdr2("Emp_Phone_Ext")
                                lblcurdept.Text = rdr2("Dep_Name")
                                lblcurlocation.Text = rdr2("BuildingUnit") & "-" & rdr2("seatno")
                                lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                                lblcurtransreason.Text = rdr2("activity")
                                lblcurlastremarks.Text = rdr2("remarks")
                                ViewState("Emp_Number") = rdr2("Emp_Number")
                            End If
                        End While
                    Else
                        lblcuruser.Text = ""
                        lblcurphone.Text = ""
                        lblcurdept.Text = ""
                        lblcurlocation.Text = ""
                        lblcurtransdate.Text = ""
                        lblcurtransreason.Text = ""
                        lblcurlastremarks.Text = ""
                    End If
                End While
            Else
                trcuruser.Visible = False
                trcurlocation.Visible = False
                trcursubloc.Visible = False
                trcurdept.Visible = False
                trcurvendor.Visible = False
                lblcurtransdate.Text = ""
                lblcurtransreason.Text = ""
                lblcurlastremarks.Text = ""
            End If
            If cbouser.Items.FindByValue("0000") IsNot Nothing Then
                cbouser.SelectedValue = "0000"
                trtarloc.Visible = True
                trtarsubloc.Visible = True
            End If
            cbouser.Enabled = False
            trtaruser.Visible = True
            trtarloc.Visible = True
            trtarsubloc.Visible = True
            rdr.Close()
            con.Close()
            'Sales
        ElseIf Request.QueryString("TransTag") = "8" Then
            Dim sqlquery As String = ""
            Dim sqlquery1 As String = ""
            sqlquery = "select  top 1 isnull(at.transtype,'') as transtype,isnull(at.userid,'') as userid from tbl_Asset_Transactions at where at.assetid = " & assetid & "   order by at.id desc"
            Dim vendorname As String = ""
            Dim locid As String = ""
            Dim sublocid As String = ""
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlquery, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    If rdr("userid") = "0000" Then
                        sqlquery1 = " select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name, "
                        sqlquery1 = sqlquery1 & " isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,"
                        sqlquery1 = sqlquery1 & " isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 "
                        sqlquery1 = sqlquery1 & " from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlquery1 = sqlquery1 & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn,tbl_Asset_location loc, tbl_Asset_sublocation subloc"
                        sqlquery1 = sqlquery1 & " where at.locid = loc.locid and at.sublocid = subloc.sublocid and at.transtype <> '20' and"
                        sqlquery1 = sqlquery1 & " (ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlquery1 = sqlquery1 & " and at.reasonid = mn.id and ast.assetid = " & assetid & " and at.transtype in(" & rdr("transtype") & ") "
                        sqlquery1 = sqlquery1 & " and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc"
                    Else
                        sqlquery1 = "select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.BuildingUnit,'') as BuildingUnit ,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(emp.seatno,'') as seatno,isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlquery1 = sqlquery1 & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn "
                        sqlquery1 = sqlquery1 & " where(ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlquery1 = sqlquery1 & " and at.reasonid = mn.id and ast.assetid = " & assetid & " and at.transtype in(" & rdr("transtype") & ") and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc "
                    End If
                    If con1.State = ConnectionState.Open Then
                        con1.Close()
                    End If
                    con1.Open()
                    Dim rdr2 As SqlDataReader
                    cmd1 = New SqlCommand(sqlquery1, con1)
                    rdr2 = cmd1.ExecuteReader
                    If rdr2.HasRows Then
                        While rdr2.Read
                            lblcuruser.Text = rdr2("Emp_Name") & " " & rdr2("Emp_Initial") & " (" & rdr2("Emp_Number") & ")"
                            If UCase(rdr2("Emp_Name")) = UCase("Asset Admin") Then
                                trcuruser.Visible = True
                                trcurlocation.Visible = True
                                trcursubloc.Visible = True
                                trcurdept.Visible = False
                                GetAdminLocations(rdr2("id"), lblcuradminloc, lblcuradminsubloc)
                                lblcurphone.Text = rdr2("Emp_Phone_Ext")
                                lblcurdept.Text = rdr2("Dep_Name")
                                lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                                lblcurtransreason.Text = rdr2("activity")
                                lblcurlastremarks.Text = rdr2("remarks")
                                ViewState("Emp_Number") = rdr2("Emp_Number")
                            Else
                                trcuruser.Visible = True
                                trcurdept.Visible = True
                                trcurlocation.Visible = False
                                trcursubloc.Visible = False
                                lblcurphone.Text = rdr2("Emp_Phone_Ext")
                                lblcurdept.Text = rdr2("Dep_Name")
                                lblcurlocation.Text = rdr2("BuildingUnit") & "-" & rdr2("seatno")
                                lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                                lblcurtransreason.Text = rdr2("activity")
                                lblcurlastremarks.Text = rdr2("remarks")
                                ViewState("Emp_Number") = rdr2("Emp_Number")
                            End If
                        End While
                    Else
                        lblcuruser.Text = ""
                        lblcurphone.Text = ""
                        lblcurdept.Text = ""
                        lblcurlocation.Text = ""
                        lblcurtransdate.Text = ""
                        lblcurtransreason.Text = ""
                        lblcurlastremarks.Text = ""
                    End If
                End While
            Else
                trcuruser.Visible = False
                trcurlocation.Visible = False
                trcursubloc.Visible = False
                trcurdept.Visible = False
                trcurvendor.Visible = False
                lblcurtransdate.Text = ""
                lblcurtransreason.Text = ""
                lblcurlastremarks.Text = ""
            End If
            'If cbouser.Items.FindByValue("0000") IsNot Nothing Then
            '    cbouser.SelectedValue = "0000"
            '    trtarloc.Visible = True
            '    trtarsubloc.Visible = True
            'End If
            trtarloc.Visible = False
            trtarsubloc.Visible = False
            trtarvendor.Visible = True
            rdr.Close()
            con.Close()
            ' 9 - Deployment(Idle)
        ElseIf Request.QueryString("TransTag") = "9" Then
            Dim transtype As String = "'10'"
            sqlstr = "select top 1 at.id,status,isnull(ast.userid,'') as userid,isnull(masloc.loccatname + '-' + loc.locname,'') as locname,"
            sqlstr &= " isnull(subloc.sublocname,'') as sublocation,isnull(emp.Emp_Name,'') as Emp_Name,isnull(emp.Emp_Number,'') as Emp_Number,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(emp.seatno,'') as seatno,isnull(at.date1,'') as date1,isnull(at.remarks,'') as remarks,isnull(mn.activity,'') as  activity"
            sqlstr &= " from tbl_Asset_Status ast, tbl_Asset_Transactions at,tbl_Asset_location_master masloc,tbl_Asset_location loc,"
            sqlstr &= " tbl_Asset_sublocation subloc,view_SIP_Employees emp,tbl_Asset_Maintainance mn  where(loc.loccatid = masloc.id and at.locid = Loc.locid And at.sublocid = subloc.sublocid and at.transtype <> '20')"
            sqlstr &= " and ast.assetid = at.assetid and ast.userid = emp.Emp_Number and at.reasonid = mn.id and ast.assetid = " & assetid & "  and at.transtype in(" & transtype & ") and ast.status='M' order by at.id desc"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlstr, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    trcurvendor.Visible = False
                    trtarvendor.Visible = False
                    lblcuruser.Text = rdr("Emp_Name") & " " & rdr("Emp_Initial") & " (" & rdr("Emp_Number") & ")"
                    lblcurphone.Text = rdr("Emp_Phone_Ext")
                    lblcuradminloc.Text = rdr("locname")
                    lblcuradminsubloc.Text = rdr("sublocation")
                    lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr("date1"))
                    lblcurtransreason.Text = rdr("activity")
                    lblcurlastremarks.Text = rdr("remarks")
                    ViewState("Emp_Number") = rdr("userid")
                    lbltrdept.Text = rdr("Dep_Name")
                    If cbouser.Items.FindByValue(rdr("userid")) IsNot Nothing Then
                        cbouser.SelectedValue = rdr("userid")
                    End If
                    lbltrlocation.Text = " Loc : " & rdr("seatno") & "         Ph : " & rdr("Emp_Phone_Ext") & ""
                End While
            Else
                lblcuruser.Text = ""
                lblcurphone.Text = ""
                lblcuradminloc.Text = ""
                lblcuradminsubloc.Text = ""
                lblcurtransdate.Text = ""
                lblcurtransreason.Text = ""
                lblcurlastremarks.Text = ""
            End If
            'sqlstr = ""
            'sqlstr = "select top 1 at.id,"
            'sqlstr &= " isnull(emp.Emp_Number,'') as Emp_Number,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(emp.seatno,'') as seatno "
            'sqlstr &= " from tbl_Asset_Status ast, tbl_Asset_Transactions at,"
            'sqlstr &= " view_SIP_Employees emp  where "
            'sqlstr &= " ast.assetid = at.assetid and at.userid = emp.Emp_Number and ast.assetid = " & assetid & "  and at.transtype in(" & transtype & ") and ast.status='M' order by at.id desc"
            'If con.State = ConnectionState.Open Then
            '    con.Close()
            'End If
            'con.Open()
            'cmd = New SqlCommand(sqlstr, con)
            'rdr = cmd.ExecuteReader
            'If rdr.HasRows Then
            '    While rdr.Read
            '        If cbouser.Items.FindByValue(ViewState("UserId")) IsNot Nothing Then
            '            cbouser.SelectedValue = ViewState("UserId")
            '        End If
            '        lbltrlocation.Text = " Loc : " & rdr("seatno") & "         Ph : " & rdr("Emp_Phone_Ext") & ""
            '    End While
            'Else
            '    lblcuruser.Text = ""
            '    lblcurphone.Text = ""
            '    lblcuradminloc.Text = ""
            '    lblcuradminsubloc.Text = ""
            '    lblcurtransdate.Text = ""
            '    lblcurtransreason.Text = ""
            '    lblcurlastremarks.Text = ""
            'End If
            '   FillLocation()
            rdr.Close()
            con.Close()
            '10 Undeploy(Idle)
        ElseIf Request.QueryString("TransTag") = "10" Then
            Dim transtype As String = "'2'"
            If cbouser.Items.FindByValue("0000") IsNot Nothing Then
                cbouser.SelectedValue = "0000"
                trtarloc.Visible = True
                trtarsubloc.Visible = True
            End If
            FillLocation()
            sqlstr = "select isnull(at.userid,'') as userid from tbl_Asset_Status ast, tbl_Asset_Transactions  at "
            sqlstr = sqlstr & " where  ast.assetid = at.assetid   and  ast.assetid = " & assetid & " and at.transtype in(" & transtype & ") "
            sqlstr = sqlstr & " and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlstr, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    sqlstr = ""
                    If rdr("userid") <> "" Then
                        sqlstr = "select top 1 at.id,status,emp.Emp_Number,emp.Emp_Name,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.BuildingUnit,'') as BuildingUnit, isnull(emp.seatno,'') as seatno ,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(mn.activity,'') as activity,isnull(at.remarks,'') as remarks,isnull(at.date1,'') as date1 from tbl_Asset_Status ast, tbl_Asset_Transactions "
                        sqlstr = sqlstr & " at,view_SIP_Employees emp,tbl_Asset_Maintainance mn "
                        sqlstr = sqlstr & " where(ast.userid = emp.emp_number And ast.assetid = at.assetid)  "
                        sqlstr = sqlstr & " and at.reasonid = mn.id and ast.assetid = " & assetid & " and at.transtype in(" & transtype & ") and ast.status in(" & ViewState("assetstatus") & ")  order by at.id desc "
                    End If
                End While
            End If
            rdr.Close()
            con.Close()
            If con1.State = ConnectionState.Open Then
                con1.Close()
            End If
            con1.Open()
            cmd = New SqlCommand(sqlstr, con1)
            Dim rdr2 As SqlDataReader
            rdr2 = cmd.ExecuteReader
            If rdr2.HasRows Then
                While rdr2.Read
                    lblcuruser.Text = rdr2("Emp_Name") & " " & rdr2("Emp_Initial") & " (" & rdr2("Emp_Number") & ")"
                    trcuruser.Visible = True
                    trcurdept.Visible = True
                    trcurlocation.Visible = False
                    trcursubloc.Visible = False
                    lblcurphone.Text = rdr2("Emp_Phone_Ext")
                    lblcurdept.Text = rdr2("Dep_Name")
                    lblcurlocation.Text = rdr2("BuildingUnit") & "-" & rdr2("seatno")
                    lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr2("date1"))
                    lblcurtransreason.Text = rdr2("activity")
                    lblcurlastremarks.Text = rdr2("remarks")
                    ViewState("Emp_Number") = rdr2("Emp_Number")
                End While
            Else
                lblcuruser.Text = ""
                lblcurphone.Text = ""
                lblcurdept.Text = ""
                lblcurlocation.Text = ""
                lblcurtransdate.Text = ""
                lblcurtransreason.Text = ""
                lblcurlastremarks.Text = ""
            End If
            rdr2.Close()
            con1.Close()
            '-----------------
            'Location Change
        ElseIf Request.QueryString("TransTag") = "11" Then
            sqlstr = "select top 1 at.id,status,isnull(ast.userid,'') as userid,isnull(masloc.loccatname + '-' + loc.locname,'') as locname,"
            sqlstr &= " isnull(subloc.sublocname,'') as sublocation,isnull(emp.Emp_Name,'') as Emp_Name,isnull(emp.Emp_Number,'') as Emp_Number,isnull(emp.Emp_Initial,'') as Emp_Initial,isnull(emp.Dep_Name,'') as Dep_Name,isnull(emp.Emp_Phone_Ext,'') as Emp_Phone_Ext,isnull(emp.seatno,'') as seatno,isnull(at.date1,'') as date1,isnull(at.remarks,'') as remarks,isnull(mn.activity,'') as  activity"
            sqlstr &= " from tbl_Asset_Status ast, tbl_Asset_Transactions at,tbl_Asset_location_master masloc,tbl_Asset_location loc,"
            sqlstr &= " tbl_Asset_sublocation subloc,view_SIP_Employees emp,tbl_Asset_Maintainance mn  where(loc.loccatid = masloc.id and at.locid = Loc.locid And at.sublocid = subloc.sublocid and at.transtype <> '20')"
            sqlstr &= " and ast.assetid = at.assetid and ast.userid = emp.Emp_Number and at.reasonid = mn.id and ast.assetid = " & assetid & "  and ast.status not in('U','X','O') order by at.id desc"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sqlstr, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    lblcuruser.Text = rdr("Emp_Name") & " " & rdr("Emp_Initial") & " (" & rdr("Emp_Number") & ")"
                    lblcurphone.Text = rdr("Emp_Phone_Ext")
                    lblcuradminloc.Text = rdr("locname")
                    lblcuradminsubloc.Text = rdr("sublocation")
                    lblcurtransdate.Text = String.Format("{0:dd-MMM-yyyy}", rdr("date1"))
                    lblcurtransreason.Text = rdr("activity")
                    lblcurlastremarks.Text = rdr("remarks")
                    ViewState("Emp_Number") = rdr("userid")
                End While
            Else
                lblcuruser.Text = ""
                lblcurphone.Text = ""
                lblcuradminloc.Text = ""
                lblcuradminsubloc.Text = ""
                lblcurtransdate.Text = ""
                lblcurtransreason.Text = ""
                lblcurlastremarks.Text = ""
            End If
            cbouser.SelectedValue = "0000"
            FillLocation()
            rdr.Close()
            con.Close()
        End If
    End Function
    Public Function BindVendor()
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        cbovendor.Items.Clear()
        cbovendor.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select * from tbl_Asset_Vendor", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                cbovendor.Items.Add(New ListItem(rdr("VendorName"), rdr("VendorID")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function BindUsers()
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        cbouser.Items.Clear()
        If transtag = "2" Then
            sql = "select * from view_SIP_Employees where emp_status='A'  order by Emp_Name"
        Else
            sql = "select * from view_SIP_Employees order by Emp_Name"
        End If
        cbouser.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                cbouser.Items.Add(New ListItem(rdr("Emp_Name") & " " & rdr("Emp_Initial") & "( " & rdr("Emp_Number") & " )", rdr("Emp_Number")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function GetAdminLocations(ByVal transid As String, ByVal lblloc As Label, ByVal lblsubloc As Label)
        Dim rdrloc As SqlDataReader
        If con2.State = ConnectionState.Open Then
            con2.Close()
        End If
        con2.Open()
        cmd = New SqlCommand("select mas.loccatname + '-' + loc.locname as locname,subloc.sublocname from tbl_Asset_Transactions at, tbl_Asset_location_master mas,tbl_Asset_location loc,tbl_Asset_sublocation subloc where at.locid=loc.locid and loc.loccatid=mas.id and at.sublocid = subloc.sublocid and at.id=" & transid & "", con2)
        rdrloc = cmd.ExecuteReader
        If rdrloc.HasRows Then
            While rdrloc.Read
                lblloc.Text = rdrloc("locname")
                lblsubloc.Text = rdrloc("sublocname")
            End While
        End If
        rdrloc.Close()
        con2.Close()
    End Function

    Protected Sub btnsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        Try
            Dim sql As String = ""
            Dim smessage As String = ""
            If drpcategory.SelectedValue = "" Then
                'Dim myscript As String = "alert('Please select the Category');"
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                drpcategory.Focus()
                Exit Sub
            End If
            If drpassettype.SelectedValue = "" Then
                'Dim myscript As String = "alert('Please select the Asset Type');"
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                drpassettype.Focus()
                Exit Sub
            End If
            If Session("Assets") Is Nothing Then
                If cboassetno.SelectedValue = "" Then
                    'Dim myscript As String = "alert('Please select the Asset Name');"
                    'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    cboassetno.Focus()
                    Exit Sub
                End If
            End If
            If transtag = "5" Or transtag = "8" Then
                If cbovendor.SelectedValue = "" Then
                    cbovendor.Focus()
                End If
            Else
                If cbouser.SelectedValue = "" Then
                    cbouser.Focus()
                    Exit Sub
                End If
            End If

            If cbouser.SelectedValue = "0000" Then
                'If drptargetloc.SelectedValue = "" Then
                '    Dim myscript As String = "alert('Please select the Asset Name');"
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                '    drptargetloc.Focus()
                '    Exit Sub
                'End If
                'If drptargetsubloc.SelectedValue = "" Then
                '    Dim myscript As String = "alert('Please select the Asset Name');"
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                '    drptargetsubloc.Focus()
                '    Exit Sub
                'End If
            End If

            If drpmaintanance.SelectedValue = "" Then
                'Dim myscript As String = "alert('Please select Reason');"
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                drpmaintanance.Focus()
                Exit Sub
            End If

            If txtreturndate.Text <> "" Then
                If (Convert.ToDateTime(txtreturndate.Text) < Convert.ToDateTime(txtdate.Text)) Then
                    Dim myscript As String = "alert('Expected Return Date is Greater Than Transaction Date!');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    Exit Sub
                End If
            End If
            Dim returndate As String = ""
            '1. Warranty/maintnance
            If transtag = "1" Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "insert into tbl_Asset_Transactions(assetid,transtype.date1,reasonid,remarks,transcreatedid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & DateTime.Now() & "') "
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                sql = ""
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "update tbl_Asset_Status set status='S' where Assetid =" & cboassetno.SelectedValue & ""
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                '2. Deployment
            ElseIf transtag = "2" Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                If txtreturndate.Text = "" Then
                    returndate = ""
                Else
                    returndate = txtreturndate.Text
                End If
                If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,date2,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & cbouser.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & returndate & "','" & DateTime.Now() & "') "
                Else
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,date2,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & cbouser.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & returndate & "','" & DateTime.Now() & "') "
                End If
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                sql = ""
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "update tbl_Asset_Status set status='U',userid='" & cbouser.SelectedValue & "' where Assetid =" & cboassetno.SelectedValue & ""
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                smessage = "Asset Deployed Successfully!"
                '3.Undeployment
            ElseIf transtag = "3" Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "' ) "
                Else
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & DateTime.Now() & "') "
                End If
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                sql = ""
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "update tbl_Asset_Status set status='S',userid='" & cbouser.SelectedValue & "' where Assetid =" & cboassetno.SelectedValue & ""
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                smessage = "Asset UnDeployed Successfully!"
                '4.Repair (Inhouse)
            ElseIf transtag = "4" Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()

                If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                    'If cbouser.SelectedValue = "" Then
                    'sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','0000','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ") "
                    '  Else
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                    'End If
                Else
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & DateTime.Now() & "') "
                End If
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                sql = ""
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "update tbl_Asset_Status set status='R',userid='" & cbouser.SelectedValue & "' where Assetid =" & cboassetno.SelectedValue & ""
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                smessage = "Asset Status Changed Successfully!"
                '5. Repair(Outside)
            ElseIf transtag = "5" Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,vendorid,reasonid,remarks,transcreatedid,date2,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & cbovendor.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & txtreturndate.Text & "','" & DateTime.Now() & "') "
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                sql = ""
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "update tbl_Asset_Status set status='O' where Assetid =" & cboassetno.SelectedValue & ""
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                smessage = "Asset Status Changed Successfully!"
                '6. Return
            ElseIf transtag = "6" Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                If ViewState("Emp_Number") <> Nothing Then
                    If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                        sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                    Else
                        sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & DateTime.Now() & "') "
                    End If
                ElseIf ViewState("VendorID") <> Nothing Then
                    If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                        sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,vendorid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("VendorID") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                    Else
                        sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,vendorid,reasonid,remarks,transcreatedid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("VendorID") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & DateTime.Now() & "') "
                    End If
                End If

                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                sql = ""
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "update tbl_Asset_Status set status='S',userid='0000' where Assetid =" & cboassetno.SelectedValue & ""
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                con.Close()
                cmd.Dispose()
                smessage = "Asset Status Changed Successfully!"
                '7. Retired
            ElseIf transtag = "7" Then
                If drptargetloc.SelectedValue = "" Then
                    Dim myscript As String = "alert('Please select Location');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    drptargetloc.Focus()
                    Exit Sub
                End If
                If drptargetsubloc.SelectedValue = "" Then
                    Dim myscript As String = "alert('Please select SubLocation');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                    drptargetsubloc.Focus()
                    Exit Sub
                End If
                If Session("Assets") IsNot Nothing Then
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    If con1.State = ConnectionState.Open Then
                        con1.Close()
                    End If
                    con1.Open()
                    Dim assets() As String = Convert.ToString(Session("Assets")).Split("|")
                    If assets.Length > 0 Then
                        For i As Integer = 0 To assets.Length - 1
                            sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & assets(i) & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                            cmd = New SqlCommand(sql, con)
                            cmd.ExecuteNonQuery()
                            sql = "update tbl_Asset_Status set status='E' where Assetid =" & assets(i) & ""
                            cmd1 = New SqlCommand(sql, con)
                            cmd1.ExecuteNonQuery()
                        Next
                    End If
                    con.Close()
                    cmd.Dispose()
                    con1.Close()
                    cmd.Dispose()
                Else
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                    cmd = New SqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    cmd.Dispose()
                    sql = ""
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    sql = "update tbl_Asset_Status set status='E',userid='0000' where Assetid =" & cboassetno.SelectedValue & ""
                    cmd = New SqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    cmd.Dispose()
                End If
                Session("Assets") = Nothing
                 smessage = "Asset Status Changed Successfully!"
                '8.Sales
                ElseIf transtag = "8" Then
                    If Session("Assets") IsNot Nothing Then
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                        con.Open()
                        If con1.State = ConnectionState.Open Then
                            con1.Close()
                        End If
                        con1.Open()
                        Dim assets() As String = Convert.ToString(Session("Assets")).Split("|")
                        If assets.Length > 0 Then
                            For i As Integer = 0 To assets.Length - 1
                            sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,vendorid,reasonid,remarks,transcreatedid,transdate)values(" & assets(i) & ",'" & transtag & "','" & txtdate.Text & "','" & cbovendor.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & DateTime.Now() & "') "
                                cmd = New SqlCommand(sql, con)
                                cmd.ExecuteNonQuery()
                                sql = "update tbl_Asset_Status set status='X' where Assetid =" & assets(i) & ""
                                cmd1 = New SqlCommand(sql, con)
                                cmd1.ExecuteNonQuery()
                            Next
                        End If
                        con.Close()
                        cmd.Dispose()
                        con1.Close()
                        cmd.Dispose()
                    Else
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                        con.Open()
                        If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                        sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,vendorid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & cbovendor.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                        Else
                        sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,vendorid,reasonid,remarks,transcreatedid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & cbovendor.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & DateTime.Now() & "') "
                        End If
                        cmd = New SqlCommand(sql, con)
                        cmd.ExecuteNonQuery()
                        con.Close()
                        cmd.Dispose()
                        sql = ""
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                        con.Open()
                        sql = "update tbl_Asset_Status set status='X' where Assetid =" & cboassetno.SelectedValue & ""
                        cmd = New SqlCommand(sql, con)
                        cmd.ExecuteNonQuery()
                        con.Close()
                        cmd.Dispose()
                    End If
                    smessage = "Asset Status Changed Successfully!"
                Session("Assets") = Nothing
                    '9.deploy(Idle)
                ElseIf transtag = "9" Then
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & ViewState("Emp_Number") & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                    Else
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,date2,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & cbouser.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & txtreturndate.Text & "','" & DateTime.Now() & "') "
                    End If
                    cmd = New SqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    cmd.Dispose()
                    sql = ""
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    sql = "update tbl_Asset_Status set status='U',userid='" & cbouser.SelectedValue & "' where Assetid =" & cboassetno.SelectedValue & ""
                    cmd = New SqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    cmd.Dispose()
                    smessage = "Asset Deployed(Idle) Successfully!"
                    '10.Undeploy(Idle)
                ElseIf transtag = "10" Then
                    If drptargetloc.SelectedValue = "" Then
                        Dim myscript As String = "alert('Please select Location');"
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                        drptargetloc.Focus()
                        Exit Sub
                    End If
                    If drptargetsubloc.SelectedValue = "" Then
                        Dim myscript As String = "alert('Please select SubLocation');"
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                        drptargetsubloc.Focus()
                        Exit Sub
                    End If
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & cbouser.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                Else
                    sql = "insert into tbl_Asset_Transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','" & cbouser.SelectedValue & "','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "','" & DateTime.Now() & "') "
                End If
                    cmd = New SqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    cmd.Dispose()
                    sql = ""
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    sql = "update tbl_Asset_Status set status='M',userid='" & ViewState("Emp_Number") & "' where Assetid =" & cboassetno.SelectedValue & ""
                    cmd = New SqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    cmd.Dispose()
                    smessage = "Asset UnDeployed(Idle) Successfully!"
                    'Location Change
                ElseIf transtag = "11" Then
                    If drptargetloc.SelectedValue = "" Then
                        Dim myscript As String = "alert('Please select Location');"
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                        drptargetloc.Focus()
                        Exit Sub
                    End If
                    If drptargetsubloc.SelectedValue = "" Then
                        Dim myscript As String = "alert('Please select SubLocation');"
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                        drptargetsubloc.Focus()
                        Exit Sub
                    End If
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    con.Open()
                    If drptargetloc.SelectedValue <> "" And drptargetsubloc.SelectedValue <> "" Then
                    sql = "insert into tbl_asset_transactions(assetid,transtype,date1,userid,reasonid,remarks,transcreatedid,locid,sublocid,transdate)values(" & cboassetno.SelectedValue & ",'" & transtag & "','" & txtdate.Text & "','0000','" & drpmaintanance.SelectedValue & "','" & txtremarks.Text.Replace("'", "''") & "','" & Session("EmpNo") & "'," & drptargetloc.SelectedValue & "," & drptargetsubloc.SelectedValue & ",'" & DateTime.Now() & "') "
                    End If
                    cmd = New SqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    con.Close()
                    cmd.Dispose()
                    sql = ""
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    smessage = "Asset Location Changed Successfully!"
                End If
                If smessage <> "" Then
                    trmessage.Visible = True
                    lblmessage.Text = smessage
                    cboassetno.Items.Clear()
                    If drpcategory.SelectedValue <> "" Then
                        If drpassettype.SelectedValue <> "" Then
                            GetAssetDetails(ViewState("assetstatus"))
                        End If
                    End If
                    lblcuradminloc.Text = ""
                    lblcuradminsubloc.Text = ""
                    lblcurdept.Text = ""
                    lblcurlastremarks.Text = ""
                    lblcurlocation.Text = ""
                    lblcurphone.Text = ""
                    lblcurtransdate.Text = ""
                    lblcurtransreason.Text = ""
                    lblcuruser.Text = ""
                    lbltrdept.Text = ""
                    lbltrlocation.Text = ""
                    cbovendor.SelectedIndex = 0
                    txtreturndate.Text = ""
                    txtdate.Text = String.Format("{0:dd-MMM-yyyy}", Date.Today.Date)
                ' cbouser.SelectedIndex = 0
                    drpmaintanance.SelectedIndex = 0
                    txtremarks.Text = ""
                    drptargetloc.SelectedIndex = 0
                    drptargetsubloc.SelectedIndex = 0
                End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindGrid()
        Try

            dtable = New DataTable
            dtable = LoadDetailsView()
            If dtable.Rows.Count > 0 Then
                grdassets.DataSource = dtable
                For j As Integer = 0 To dtable.Columns.Count - 2
                    Dim s As New BoundField
                    s.DataField = dtable.Columns(j).ToString
                    s.HeaderText = dtable.Columns(j).ToString
                Next
                grdassets.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
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
            cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where  aad.attid = aa.attid  and aad.AssetTypeId = " & drpassettype.SelectedValue & " order by aad.attid asc"
        Else
            cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where  aad.attid = aa.attid  and aad.AssetTypeId = " & drpassettype.SelectedValue & " and aa.Header <>'8' order by aad.attid asc"
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

        sql = sql & " from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid  and am.AssetTypeid =" & drpassettype.SelectedValue & " and am.id = " & cboassetno.SelectedValue
        sqladr = New SqlDataAdapter(sql, con)
        dtable = New DataTable
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Protected Sub cbouser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbouser.SelectedIndexChanged
        If cbouser.SelectedValue <> "" Then
            If cbouser.SelectedValue = "0000" Then
                trtardept.Visible = False
                trtarloc.Visible = True
                trtarsubloc.Visible = True
                FillLocation()
            Else
                FillLocation()
                trtardept.Visible = True
                trtarloc.Visible = False
                trtarsubloc.Visible = False
            End If
        End If

    End Sub
    'Protected Sub CboLoc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboLoc.SelectedIndexChanged
    '    fillsublocation(drptargetloc.SelectedValue)
    'End Sub
    Public Sub FillLocation()
        Dim sql As String
        If con1.State = ConnectionState.Open Then
            con1.Close()
        End If
        sql = "select isnull(right(emp_phone_ext,4),'') as ph,isnull(seatno,'') as seat,isnull(dep_name,'') as dep from view_SIP_Employees where emp_number ='" & cbouser.SelectedValue & "'"
        con1.Open()
        cmd1 = New SqlCommand(sql, con1)
        rdr1 = cmd1.ExecuteReader
        If rdr1.HasRows Then
            While rdr1.Read
                lbltrlocation.Text = " Loc : " & rdr1("seat") & "         Ph : " & rdr1("ph") & ""
                lbltrdept.Text = rdr1("dep")
            End While
        Else
            lbltrlocation.Text = ""
            lbltrdept.Text = ""
        End If
        rdr1.Close()
        con1.Close()
        sql = ""
        sql = "select mas.loccatname + '-' + loc.locname as locname,locid from tbl_Asset_location loc,tbl_Asset_location_master mas where loc.loccatid = mas.id order by loc.locname"
        If con1.State = ConnectionState.Open Then
            con1.Close()
        End If
        con1.Open()
        drptargetloc.Items.Clear()
        drptargetloc.Items.Add(New ListItem("--Select--", ""))
        cmd1 = New SqlCommand(sql, con1)
        rdr1 = cmd1.ExecuteReader
        If rdr1.HasRows Then
            While rdr1.Read
                'CboLoc.Items.Add(New ListItem(rdr1("locname"), rdr1("locid")))
                drptargetloc.Items.Add(New ListItem(rdr1("locname"), rdr1("locid")))
            End While
        End If
        rdr1.Close()
        con1.Close()

    End Sub
    Public Sub FillsubLocation(ByVal locid As String)
        Dim sql As String
        sql = "select sublocname,sublocid from tbl_Asset_sublocation where locid =" & Val(locid) & " order by sublocname"
        If con1.State = ConnectionState.Open Then
            con1.Close()
        End If
        con1.Open()
        drptargetsubloc.Items.Clear()
        drptargetsubloc.Items.Add(New ListItem("--Select--", ""))
        cmd1 = New SqlCommand(sql, con1)
        rdr1 = cmd1.ExecuteReader
        If rdr1.HasRows Then
            While rdr1.Read
                drptargetsubloc.Items.Add(New ListItem(rdr1("sublocname"), rdr1("sublocid")))
            End While
        End If
        rdr1.Close()
        con1.Close()
    End Sub
    Protected Sub drptargetloc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drptargetloc.SelectedIndexChanged
        If drptargetloc.SelectedValue <> "" Then
            FillsubLocation(drptargetloc.SelectedValue)
        End If
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("Default.aspx")
    End Sub
    Shared sortExpression As String
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
    Private Sub SortGridView(ByVal sortExpression As String, ByVal direction As String)
        Dim str As String
        str = "select hd.complaintid,hd.complaint,convert(varchar,hd.cdate,106) as cdate,hd.closetag,(select e.Emp_Name + ' '+ e.emp_initial + '(' + e.Emp_Number + ')'  from view_SIP_Employees e where e.emp_Number = hd.emp_Number) as Emp_Name,"
        str &= " (select e.Emp_Name + ' '+ e.emp_initial + '(' + e.Emp_Number + ')' from view_SIP_Employees e where e.Emp_Number = hd.cons_id) as ConsultantName"
        str &= " from tbl_hd_complaint hd where hd.nodeid =" & cboassetno.SelectedValue & "  order by hd.complaintid desc"
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
            sql = sql & " and trans.assetid = '" & cboassetno.SelectedValue & "' order by trans.id desc"
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
            sql = sql & " (trans.transtype in('2','3','9','10')) and  trans.assetid = am.id and trans.assetid = '" & cboassetno.SelectedValue & "' order by trans.id desc"

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
    Protected Sub grdmaintainance_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdmaintainance.Sorting
        BindRepairs(e.SortExpression, sortOrder)
    End Sub

    Protected Sub grddeployments_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grddeployments.Sorting
        BindDeployments(e.SortExpression, sortOrder)
    End Sub
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
            Dim myDataSet As New DataSet()
            If warrantystart <> "" Then
                sql = " select s.assetid,s.att1,s." & warrantystart & " as warrantystart,h.pono,convert(varchar,h.warrantyend,106) as warranty,h.contractno,(select v.vendorname from tbl_asset_vendor v where v.vendorid = h.vendorid) as vendor,"
                sql = sql & " (select em.emp_Name + ' ' + em.emp_Initial + '(' + emp_Number + ')' from view_SIP_Employees em where em.emp_number = h.transby) as ConsultantName"
                sql = sql & " from tbl_asset_transactionshistory h,view_assetmaster_status s where h.assetid = s.id and h.assetid = '" & cboassetno.SelectedValue & "' order by h.id desc"

                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                Dim oAdapter As New SqlDataAdapter(sql, con)
                oAdapter.Fill(myDataSet)
            End If
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

    Protected Sub grdwarranty_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdwarranty.Sorting
        BindWarranty(e.SortExpression, sortOrder)
    End Sub
End Class
