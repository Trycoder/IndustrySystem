Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Partial Class ReportFormula
    Inherits System.Web.UI.Page
    Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim rowid As String
    Dim dtable As DataTable
    Shared sortExpression As String
    Public Function bindcategory()
        Dim sql As String
        con.Open()
        drpassetcategory.Items.Clear()
        cmd = New SqlCommand("select * from tbl_Asset_CategoryMaster where groupid = 1 order by catcode ASC", con)
        rdr = cmd.ExecuteReader
        drpassetcategory.Items.Add(New ListItem("--Select--", 0))
        If rdr.HasRows Then
            While rdr.Read
                drpassetcategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        rowid = Request.QueryString("Id")
        If Not IsPostBack Then
            bindcategory()
            bindReportcategory()
            Session("OR") = "False"
            ViewState("sortOrder") = ""
            BindGrid("", "")
        End If
        lblmessage.Text = ""
        If Not String.IsNullOrEmpty(Session("Message")) Then
            lblmessage.Text = Session("Message")
        End If
        If drpattributetype.SelectedValue <> "" Then
            If DrpOperator.SelectedValue <> "0" Then
                If drpattributetype.SelectedValue = "v_emp.Emp_Number" Or drpattributetype.SelectedValue = "v_emp.Emp_Name" Or drpattributetype.SelectedValue = "v_emp.Emp_Phone_Ext" Then
                    Dim txt As New TextBox
                    Dim drpassetno As New DropDownList
                    txt.CssClass = "control"
                    drpassetno.CssClass = "control"
                    txt.Width = Unit.Pixel(75)
                    txt.ID = "txt123"
                    drpassetno.ID = "drpasset123"
                    Dim lbl As New Label
                    lbl.ID = "lbl1"
                    lbl.Text = " "
                    tdcondition.Controls.Add(txt)
                    tdcondition.Controls.Add(lbl)
                    tdcondition.Controls.Add(drpassetno)
                    bindassetno(drpassetno, chkassettype, drpattributetype.SelectedValue)
                ElseIf drpattributetype.SelectedValue = "v_asset.status" Then
                    tdcondition.Controls.Clear()
                    '  Dim txt As New TextBox
                    Dim drpassetno As New DropDownList
                    '  txt.CssClass = "control"
                    drpassetno.CssClass = "control"
                    '   txt.Width = Unit.Pixel(75)
                    '  txt.ID = "txt456"
                    drpassetno.ID = "drpasset456"
                    Dim lbl As New Label
                    lbl.ID = "lbl1"
                    lbl.Text = " "
                    '   tdcondition.Controls.Add(txt)
                    tdcondition.Controls.Add(lbl)
                    drpassetno.Items.Clear()
                    drpassetno.Items.Add(New ListItem("--Select--", ""))
                    drpassetno.Items.Add(New ListItem("Spare", "S"))
                    drpassetno.Items.Add(New ListItem("With User", "U"))
                    drpassetno.Items.Add(New ListItem("Repair(Inside)", "R"))
                    drpassetno.Items.Add(New ListItem("Repair(Outside)", "O"))
                    drpassetno.Items.Add(New ListItem("Expired", "E"))
                    drpassetno.Items.Add(New ListItem("Sold", "X"))
                    drpassetno.Items.Add(New ListItem("With User(Idle)", "M"))
                    tdcondition.Controls.Add(drpassetno)
                ElseIf drpattributetype.SelectedValue = "v_trans.transtype" Then
                    tdcondition.Controls.Clear()
                    '  Dim txt As New TextBox
                    Dim drpassetno As New DropDownList
                    ' txt.CssClass = "control"
                    drpassetno.CssClass = "control"
                    ' txt.Width = Unit.Pixel(75)
                    ' txt.ID = "txt1"
                    drpassetno.ID = "drp1"
                    Dim lbl As New Label
                    lbl.ID = "lbl1"
                    lbl.Text = " "
                    'tdcondition.Controls.Add(txt)
                    tdcondition.Controls.Add(lbl)
                    drpassetno.Items.Clear()
                    drpassetno.Items.Add(New ListItem("--Select--", ""))
                    drpassetno.Items.Add(New ListItem("Purchase", "0"))
                    drpassetno.Items.Add(New ListItem("Maintnance", "1"))
                    drpassetno.Items.Add(New ListItem("Deployment", "2"))
                    drpassetno.Items.Add(New ListItem("UnDeploy", "3"))
                    drpassetno.Items.Add(New ListItem("Repair(Inhouse)", "4"))
                    drpassetno.Items.Add(New ListItem("Repair(Outside)", "5"))
                    drpassetno.Items.Add(New ListItem("Return", "6"))
                    drpassetno.Items.Add(New ListItem("Retired", "7"))
                    drpassetno.Items.Add(New ListItem("Sales", "8"))
                    drpassetno.Items.Add(New ListItem("Deploy(Idle)", "9"))
                    drpassetno.Items.Add(New ListItem("Undeploy(Idle)", "10"))
                    drpassetno.Items.Add(New ListItem("LocationChange", "11"))
                    tdcondition.Controls.Add(drpassetno)
                ElseIf drpattributetype.SelectedValue.Contains("v_emp.BuildingUnit") = True Then
                    '  Dim txt As New TextBox
                    Dim drpassetno As New DropDownList
                    'txt.CssClass = "control"
                    drpassetno.CssClass = "control"
                    ' txt.Width = Unit.Pixel(75)
                    '  txt.ID = "txt456"
                    drpassetno.ID = "drpasset456"
                    Dim lbl As New Label
                    lbl.ID = "lbl1"
                    lbl.Text = " "
                    ' tdcondition.Controls.Add(txt)
                    tdcondition.Controls.Add(lbl)
                    tdcondition.Controls.Add(drpassetno)
                    bindassetno(drpassetno, chkassettype, drpattributetype.SelectedValue, drpassetcategory.SelectedValue)
                    'Transaction Date
                ElseIf drpattributetype.SelectedValue = "v_trans.date1" Then
                    If DrpOperator.SelectedValue = "7" Then
                        tdcondition.Controls.Clear()
                        Dim cal1 As New CalendarExtender
                        Dim cal2 As New CalendarExtender
                        Dim txtdate1 As New TextBox
                        Dim txtdate2 As New TextBox
                        txtdate1.Attributes.Add("readonly", "readonly")
                        txtdate1.CssClass = "control"
                        txtdate1.ID = "txtdate1"
                        txtdate2.Attributes.Add("readonly", "readonly")
                        txtdate2.CssClass = "control"
                        txtdate2.ID = "txtdate2"
                        cal1.ID = "calex1"
                        cal1.TargetControlID = "txtdate1"
                        cal1.Format = "dd-MMM-yyyy"
                        cal2.ID = "calex2"
                        cal2.TargetControlID = "txtdate2"
                        cal2.Format = "dd-MMM-yyyy"
                        Dim lbl As New Label
                        lbl.ID = "lbl1"
                        lbl.Text = " "
                        tdcondition.Controls.Add(txtdate1)
                        tdcondition.Controls.Add(cal1)
                        tdcondition.Controls.Add(lbl)
                        tdcondition.Controls.Add(txtdate2)
                        tdcondition.Controls.Add(cal2)
                    Else
                        tdcondition.Controls.Clear()
                        Dim cal12 As New CalendarExtender
                        Dim txtdate1 As New TextBox
                        txtdate1.Attributes.Add("readonly", "readonly")
                        txtdate1.CssClass = "control"
                        txtdate1.ID = "txtdate1"
                        cal12.ID = "calex12"
                        cal12.TargetControlID = "txtdate1"
                        cal12.Format = "dd-MMM-yyyy"
                        tdcondition.Controls.Add(txtdate1)
                        tdcondition.Controls.Add(cal12)
                    End If
                ElseIf drpattributetype.SelectedValue = "v_trans.activity" Then
                    '  Dim txt As New TextBox
                    Dim drpassetno As New DropDownList
                    'txt.CssClass = "control"
                    drpassetno.CssClass = "control"
                    ' txt.Width = Unit.Pixel(75)
                    '  txt.ID = "txt456"
                    drpassetno.ID = "drpasset456"
                    Dim lbl As New Label
                    lbl.ID = "lbl1"
                    lbl.Text = " "
                    ' tdcondition.Controls.Add(txt)
                    tdcondition.Controls.Add(lbl)
                    tdcondition.Controls.Add(drpassetno)
                    bindassetno(drpassetno, chkassettype, drpattributetype.SelectedValue, drpassetcategory.SelectedValue)
                ElseIf drpattributetype.SelectedValue = "v_trans.transcreatedid" Then
                    Dim txt As New TextBox
                    Dim drpassetno As New DropDownList
                    txt.CssClass = "control"
                    drpassetno.CssClass = "control"
                    txt.Width = Unit.Pixel(75)
                    txt.ID = "txt123"
                    drpassetno.ID = "drpasset123"
                    Dim lbl As New Label
                    lbl.ID = "lbl1"
                    lbl.Text = " "
                    tdcondition.Controls.Add(txt)
                    tdcondition.Controls.Add(lbl)
                    tdcondition.Controls.Add(drpassetno)
                    bindassetno(drpassetno, chkassettype, drpattributetype.SelectedValue)
                ElseIf drpattributetype.SelectedValue = "v_trans.remarks" Then
                    Dim txt As New TextBox
                    Dim drpassetno As New DropDownList
                    txt.CssClass = "control"
                    drpassetno.CssClass = "control"
                    txt.Width = Unit.Pixel(75)
                    txt.ID = "txt123"
                    drpassetno.ID = "drpasset123"
                    Dim lbl As New Label
                    lbl.ID = "lbl1"
                    lbl.Text = " "
                    tdcondition.Controls.Add(txt)
                    tdcondition.Controls.Add(lbl)
                    tdcondition.Controls.Add(drpassetno)
                    bindassetno(drpassetno, chkassettype, drpattributetype.SelectedValue)
                    'Building Unit
                ElseIf drpattributetype.SelectedValue = "v_asset.BuildingUnit" Or drpattributetype.SelectedValue = "v_asset.Location" Or drpattributetype.SelectedValue = "v_asset.seatno" Then
                    Dim txt As New TextBox
                    Dim drpassetno As New DropDownList
                    txt.CssClass = "control"
                    drpassetno.CssClass = "control"
                    txt.Width = Unit.Pixel(75)
                    txt.ID = "txt123"
                    drpassetno.ID = "drpasset123"
                    Dim lbl As New Label
                    lbl.ID = "lbl1"
                    lbl.Text = " "
                    tdcondition.Controls.Add(txt)
                    tdcondition.Controls.Add(lbl)
                    tdcondition.Controls.Add(drpassetno)
                    bindassetno(drpassetno, chkassettype, drpattributetype.SelectedValue)
                Else
                    dtable = New DataTable
                    dtable = GetFieldtype(drpattributetype.SelectedValue)
                    If dtable.Rows.Count > 0 Then
                        'Dim strtable As New Table
                        'strtable.ID = "strtable1"
                        'strtable.Width = Unit.Percentage(100)
                        ' Dim strrow As New TableRow
                        '  Dim strCell_1 As New TableCell
                        If UCase(dtable.Rows(0)("AttType")) = UCase("Sequence") Or UCase(dtable.Rows(0)("AttType")) = UCase("Text(Variable)") Or UCase(dtable.Rows(0)("AttType")) = UCase("Text(Fixed)") Then
                            Dim txt As New TextBox
                            Dim drpassetno As New DropDownList
                            txt.CssClass = "control"
                            drpassetno.CssClass = "control"
                            txt.Width = Unit.Pixel(75)
                            txt.ID = "txt" & dtable.Rows(0)("attid").ToString
                            drpassetno.ID = "drp" & dtable.Rows(0)("attid").ToString
                            Dim lbl As New Label
                            lbl.ID = "lbl1"
                            lbl.Text = " "
                            tdcondition.Controls.Add(txt)
                            tdcondition.Controls.Add(lbl)
                            tdcondition.Controls.Add(drpassetno)
                            bindassetno(drpassetno, chkassettype, drpattributetype.SelectedValue)
                        ElseIf UCase(dtable.Rows(0)("AttType")) = UCase("Date") Then
                            If DrpOperator.SelectedValue = "7" Then
                                tdcondition.Controls.Clear()
                                Dim cal1 As New CalendarExtender
                                Dim cal2 As New CalendarExtender
                                Dim txtdate1 As New TextBox
                                Dim txtdate2 As New TextBox
                                txtdate1.Attributes.Add("readonly", "readonly")
                                txtdate1.CssClass = "control"
                                txtdate1.ID = "txtdate1" & dtable.Rows(0)("attid")
                                txtdate2.Attributes.Add("readonly", "readonly")
                                txtdate2.CssClass = "control"
                                txtdate2.ID = "txtdate2" & dtable.Rows(0)("attid")
                                cal1.ID = "calex1" & dtable.Rows(0)("attid").ToString
                                cal1.TargetControlID = "txtdate1" & dtable.Rows(0)("attid").ToString
                                cal1.Format = "dd-MMM-yyyy"
                                cal2.ID = "calex2" & dtable.Rows(0)("attid").ToString
                                cal2.TargetControlID = "txtdate2" & dtable.Rows(0)("attid").ToString
                                cal2.Format = "dd-MMM-yyyy"
                                Dim lbl As New Label
                                lbl.ID = "lbl1"
                                lbl.Text = " "
                                tdcondition.Controls.Add(txtdate1)
                                tdcondition.Controls.Add(cal1)
                                tdcondition.Controls.Add(lbl)
                                tdcondition.Controls.Add(txtdate2)
                                tdcondition.Controls.Add(cal2)
                            Else
                                tdcondition.Controls.Clear()
                                Dim cal12 As New CalendarExtender
                                Dim txtdate1 As New TextBox
                                txtdate1.Attributes.Add("readonly", "readonly")
                                txtdate1.CssClass = "control"
                                txtdate1.ID = "txtdate1" & dtable.Rows(0)("attid")
                                cal12.ID = "calex12" & dtable.Rows(0)("attid").ToString
                                cal12.TargetControlID = "txtdate1" & dtable.Rows(0)("attid").ToString
                                cal12.Format = "dd-MMM-yyyy"
                                tdcondition.Controls.Add(txtdate1)
                                tdcondition.Controls.Add(cal12)
                            End If
                        ElseIf UCase(dtable.Rows(0)("AttType")) = UCase("SingleSelection") Then
                            Dim drp As New DropDownList
                            drp.ID = "drp" & dtable.Rows(0)("attid")
                            drp.CssClass = "control"
                            BindDropdown(drp, dtable(0)("attid").ToString)
                            tdcondition.Controls.Add(drp)
                        ElseIf UCase(dtable.Rows(0)("atttype")) = UCase("Yes/No") Then
                            Dim rdo As New RadioButtonList
                            rdo.ID = "rdo" & dtable.Rows(0)("attid")
                            rdo.Items.Add(New ListItem("Yes", "Yes"))
                            rdo.Items.Add(New ListItem("No", "No"))
                            rdo.RepeatDirection = WebControls.RepeatDirection.Horizontal
                            rdo.CssClass = "control"
                            tdcondition.Controls.Add(rdo)
                        End If
                        'strrow.Cells.Add(strCell_1)
                        'strtable.Rows.Add(strrow)
                        'tdcondition.Controls.Add(strtable)
                    End If
                End If
            End If
        End If
        lblmessage.Text = ""
        Session("Values") = Nothing
        If Not String.IsNullOrEmpty(Session("Message")) Then
            trmessage.Visible = True
            lblmessage.Text = Session("Message")
            drpreportcategory.SelectedIndex = 0
            drpassetcategory.SelectedIndex = 0
            txtreportname.Text = ""
            chkrights.Checked = False
            chkassettype.Items.Clear()
            chkreportheader.Items.Clear()
            LblCondText.Text = ""
            Session("Message") = ""
        End If
    End Sub
    Public Function bindassetno(ByVal drp As DropDownList, ByVal chkboxlist As CheckBoxList, ByVal attributeid As String, Optional ByVal catid As String = "", Optional ByVal fieldname As String = "")
        Try
            Dim sql As String = ""
            drp.Items.Clear()
            drp.Items.Add(New ListItem("--Select--", ""))
            If attributeid = "v_emp.Emp_Number" Or attributeid = "v_emp.Emp_Name" Or attributeid = "v_emp.Dep_Name" Or attributeid = "v_emp.BuildingUnit" Or attributeid = "v_emp.Emp_Phone_Ext" Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = ""
                sql = "select distinct " & attributeid & " from view_SIP_Employees v_emp order by " & attributeid & ""
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                'drp.Items.Add(New ListItem("Asset Admin", "0000"))
                'drp.Items.Add(New ListItem("Other User", "0001"))
                While rdr.Read
                    If Not String.IsNullOrEmpty(rdr(0).ToString()) Then
                        If drp.Items.Contains(drp.Items.FindByValue(rdr(0).ToString())) = False Then
                            drp.Items.Add(New ListItem(rdr(0), rdr(0)))
                        End If
                    End If
                End While
            ElseIf attributeid = "v_emp.BuildingUnit" Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = ""
                sql = "select distinct " & attributeid & " from view_SIP_Employees v_emp order by v_emp.BuildingUnit "
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                'drp.Items.Add(New ListItem("Asset Admin", "0000"))
                'drp.Items.Add(New ListItem("Other User", "0001"))
                While rdr.Read
                    If Not String.IsNullOrEmpty(rdr(0).ToString()) Then
                        If drp.Items.Contains(drp.Items.FindByValue(rdr(0).ToString())) = False Then
                            drp.Items.Add(New ListItem(rdr(0), rdr(0)))
                        End If
                    End If
                End While
            ElseIf UCase(attributeid) = UCase("v_trans.activity") Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = ""
                sql = "select id,activity from tbl_Asset_Maintainance"
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                While rdr.Read
                    If Not String.IsNullOrEmpty(rdr(0).ToString()) Then
                        If drp.Items.Contains(drp.Items.FindByValue(rdr(0).ToString())) = False Then
                            drp.Items.Add(New ListItem(rdr(1), rdr(0)))
                        End If
                    End If
                End While
            ElseIf UCase(attributeid) = UCase("v_trans.transcreatedid") Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = ""
                sql = "select emp.Emp_number,emp.Emp_Name from view_SIP_Employees emp where emp.Emp_Number in (select distinct(transcreatedid) from tbl_Asset_Transactions) order by v_emp.Emp_Name ASC"
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                While rdr.Read
                    If Not String.IsNullOrEmpty(rdr(0).ToString()) Then
                        If drp.Items.Contains(drp.Items.FindByValue(rdr(0).ToString())) = False Then
                            drp.Items.Add(New ListItem(rdr(1), rdr(0)))
                        End If
                    End If
                End While
                'Building Unit
            ElseIf UCase(attributeid) = UCase("v_asset.BuildingUnit") Or UCase(attributeid) = UCase("v_asset.Location") Or UCase(attributeid) = UCase("v_asset.seatno") Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = ""
                sql = "select distinct " & attributeid & " from view_SIP_Locations v_asset "
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                While rdr.Read
                    If Not String.IsNullOrEmpty(rdr(0).ToString()) Then
                        If drp.Items.Contains(drp.Items.FindByValue(rdr(0).ToString())) = False Then
                            drp.Items.Add(New ListItem(rdr(0), rdr(0)))
                        End If
                    End If
                End While
            Else
                For Each lst As ListItem In chkboxlist.Items
                    If lst.Selected Then
                        dtable = New DataTable
                        Try
                            Dim field() As String = attributeid.Split(".")
                            If field.Length > 0 Then
                                If drpassetcategory.SelectedValue = "0" Then
                                    sql = "select top 1 aa.Fieldorder from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid  and aa.fieldorder='" & field(1) & "' "
                                Else
                                    sql = "select aa.Fieldorder from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid  and aa.fieldorder='" & field(1) & "' and aa.catid=" & drpassetcategory.SelectedValue & ""
                                End If
                            End If
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
                                If attributeid = "v_emp.Emp_Number" Or attributeid = "v_emp.Emp_Name" Or attributeid = "v_emp.Dep_Name" Or attributeid = "v_emp.BuildingUnit" Or attributeid = "v_trans.transcreatedid" Then
                                    sql = "select distinct " & attributeid & " from view_SIP_Employees v_emp order by " & attributeid & ""
                                Else
                                    sql = "select distinct isnull(am." & s & ",''),am.id from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & lst.Value & " and am.id = ast.Assetid order by  isnull(am." & s & ",'') asc "
                                End If
                                cmd = New SqlCommand(sql, con)
                                rdr = cmd.ExecuteReader
                                While rdr.Read
                                    If Not String.IsNullOrEmpty(rdr(0).ToString()) Then
                                        If drp.Items.Contains(drp.Items.FindByValue(rdr(0).ToString())) = False Then
                                            drp.Items.Add(New ListItem(rdr(0), rdr(0)))
                                        End If
                                    End If
                                End While
                            End If
                            rdr.Close()
                            cmd.Dispose()
                        Catch ex As Exception
                            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
                            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
                        Finally
                            con.Close()
                        End Try
                    End If
                Next
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassetcategory.SelectedIndexChanged
        Try
            If drpassetcategory.SelectedValue <> "" Then
                BindAssetType(drpassetcategory.SelectedValue)
                BindAssetAttributes(drpassetcategory.SelectedValue)
            End If
            chkall.Checked = False
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function BindAssetType(ByVal categoryid As String, Optional ByVal chkall As Boolean = False)
        Try
            Dim sql As String
            If chkall = False Then
                sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId=" & categoryid & " order by am.AssetTypeCode"
            Else
                sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where ac.groupid ='1' order by am.AssetTypeCode"
            End If
            con.Open()
            chkassettype.Items.Clear()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    chkassettype.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function bindReportcategory()
        Dim sql As String
        con.Open()
        cmd = New SqlCommand("select * from tbl_Asset_reportcategory order by catcode", con)
        drpreportcategory.Items.Clear()
        rdr = cmd.ExecuteReader
        drpreportcategory.Items.Add(New ListItem("--Select--", 0))
        If rdr.HasRows Then
            While rdr.Read
                drpreportcategory.Items.Add(New ListItem(rdr("catcode"), rdr("id")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function BindAssetAttributes(ByVal categoryid As String, Optional ByVal chkall As Boolean = False, Optional ByVal AllAsset As String = "")
        Try
            Dim sql As String
            dtable = New DataTable
            If chkall = False Then
                If Session("Admingroup") = "1" Then
                    sql = "select * from tbl_Asset_Attribute_Details aad join tbl_Asset_Attributes aatt on aad.attid=aatt.attid right outer join  tbl_Asset_CategoryMaster ac on ac.catid = aatt.catid where ac.catid =" & categoryid & " order by aatt.AttId  asc"
                Else
                    sql = "select * from tbl_Asset_Attribute_Details aad join tbl_Asset_Attributes aatt on aad.attid=aatt.attid right outer join  tbl_Asset_CategoryMaster ac on ac.catid = aatt.catid where ac.catid =" & categoryid & " and aatt.Header <>'8' order by aatt.AttId  asc"
                End If
            Else
                If Session("Admingroup") = "1" Then
                    sql = "select distinct aa.attdesc,aa.attid,aa.Fieldorder from  tbl_Asset_Attributes aa,tbl_Asset_CategoryMaster acm, tbl_Asset_Attribute_Details aad where acm.groupid = '1' and aad.assettypeid in(" & AllAsset & ") and aa.attid = aad.attid  order by aa.attid"
                Else
                    sql = "select distinct aa.attdesc,aa.attid,aa.Fieldorder from  tbl_Asset_Attributes aa,tbl_Asset_CategoryMaster acm, tbl_Asset_Attribute_Details aad where acm.groupid = '1' and aad.assettypeid in(" & AllAsset & ") and aa.attid = aad.attid and aa.Header <> '8'  order by aa.attid"
                End If
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            chkreportheader.Items.Clear()
            drpattributetype.Items.Clear()
            drpgroupby.Items.Clear()
            drpgroupby.Items.Add(New ListItem("--Select--", ""))
            drpattributetype.Items.Add(New ListItem("--Select--", ""))
            rdr = cmd.ExecuteReader
            Dim arrlist As New ArrayList()
            If rdr.HasRows Then
                While rdr.Read
                    If chkall = False Then
                        If arrlist.Contains(rdr("AttDesc")) = False Then
                            chkreportheader.Items.Add(New ListItem(rdr("AttDesc"), "v_asset." & rdr("FieldOrder")))
                            drpattributetype.Items.Add(New ListItem(rdr("AttDesc"), "v_asset." & rdr("FieldOrder")))
                            drpgroupby.Items.Add(New ListItem(rdr("AttDesc"), "v_asset." & rdr("FieldOrder")))
                            arrlist.Add(rdr("AttDesc"))
                        End If
                    Else
                        If arrlist.Contains(rdr("AttDesc")) = False Then
                            chkreportheader.Items.Add(New ListItem(rdr("AttDesc"), "v_asset." & rdr("FieldOrder")))
                            drpattributetype.Items.Add(New ListItem(rdr("AttDesc"), "v_asset." & rdr("FieldOrder")))
                            drpgroupby.Items.Add(New ListItem(rdr("AttDesc"), "v_asset." & rdr("FieldOrder")))
                            arrlist.Add(rdr("AttDesc"))
                        End If
                    End If
                End While

                chkreportheader.Items.Add(New ListItem("EmployeeNumber", "v_emp.Emp_Number"))
                chkreportheader.Items.Add(New ListItem("EmployeeName", "v_emp.Emp_Name"))
                chkreportheader.Items.Add(New ListItem("EmpDepartment", "v_emp.Dep_Name"))
                chkreportheader.Items.Add(New ListItem("EmpLocation", "v_emp.BuildingUnit"))
                chkreportheader.Items.Add(New ListItem("Asset Status", "v_asset.status"))
                chkreportheader.Items.Add(New ListItem("EmployeePhone", "v_emp.Emp_Phone_Ext"))

                chkreportheader.Items.Add(New ListItem("Reason", "v_trans.activity"))
                chkreportheader.Items.Add(New ListItem("TransType", "v_trans.transtype"))
                chkreportheader.Items.Add(New ListItem("TransDate", "convert(varchar,v_trans.date1,106)"))
                chkreportheader.Items.Add(New ListItem("TransactionBy", "v_trans.transcreatedid"))
                chkreportheader.Items.Add(New ListItem("TransRemarks", "v_trans.remarks"))

                chkreportheader.Items.Add(New ListItem("BuildingUnit", "v_asset.BuildingUnit"))
                chkreportheader.Items.Add(New ListItem("Location", "v_asset.Location"))
                chkreportheader.Items.Add(New ListItem("SeatNo", "v_asset.seatno"))

                drpgroupby.Items.Add(New ListItem("EmployeeNumber", "v_emp.Emp_Number"))
                drpgroupby.Items.Add(New ListItem("EmployeeName", "v_emp.Emp_Name"))
                drpgroupby.Items.Add(New ListItem("EmpDepartment", "v_emp.Dep_Name"))
                drpgroupby.Items.Add(New ListItem("EmpLocation", "v_emp.BuildingUnit"))
                drpgroupby.Items.Add(New ListItem("EmployeePhone", "v_emp.Emp_Phone_Ext"))

                drpgroupby.Items.Add(New ListItem("Asset Status", "v_asset.status"))

                drpgroupby.Items.Add(New ListItem("Reason", "v_trans.activity"))
                drpgroupby.Items.Add(New ListItem("TransType", "v_trans.transtype"))
                drpgroupby.Items.Add(New ListItem("TransDate", "v_trans.date1"))
                drpgroupby.Items.Add(New ListItem("TransactionBy", "v_trans.transcreatedid"))
                drpgroupby.Items.Add(New ListItem("BuildingUnit", "v_asset.BuildingUnit"))
                drpgroupby.Items.Add(New ListItem("Location", "v_asset.Location"))
                drpgroupby.Items.Add(New ListItem("SeatNo", "v_asset.seatno"))

                drpattributetype.Items.Add(New ListItem("EmployeeNumber", "v_emp.Emp_Number"))
                drpattributetype.Items.Add(New ListItem("EmployeeName", "v_emp.Emp_Name"))
                drpattributetype.Items.Add(New ListItem("EmpDepartment", "v_emp.Dep_Name"))
                drpattributetype.Items.Add(New ListItem("EmpLocation", "v_emp.BuildingUnit"))
                drpattributetype.Items.Add(New ListItem("EmployeePhone", "v_emp.Emp_Phone_Ext"))

                drpattributetype.Items.Add(New ListItem("Asset Status", "v_asset.status"))

                drpattributetype.Items.Add(New ListItem("Reason", "v_trans.activity"))
                drpattributetype.Items.Add(New ListItem("TransType", "v_trans.transtype"))
                drpattributetype.Items.Add(New ListItem("TransDate", "v_trans.date1"))
                drpattributetype.Items.Add(New ListItem("TransactionBy", "v_trans.transcreatedid"))
                drpattributetype.Items.Add(New ListItem("BuildingUnit", "v_asset.BuildingUnit"))
                drpattributetype.Items.Add(New ListItem("Location", "v_asset.Location"))
                drpattributetype.Items.Add(New ListItem("SeatNo", "v_asset.seatno"))
            End If
            rdr.Close()
            con.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub DrpOperator_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DrpOperator.SelectedIndexChanged

    End Sub
    Public Function GetFieldtype(ByVal attributeid As String) As DataTable
        Try
            Dim sql As String
            dtable = New DataTable
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            'If attributeid = "EmployeeName" Or attributeid = "EmpDepartment" Or attributeid = "Location" Then
            '    Return dtable
            '    Exit Function
            'End If
            Dim field() As String = attributeid.Split(".")
            If field.Length > 0 Then
                If drpassetcategory.SelectedValue = "0" Then
                    sql = "select top 1 * from tbl_Asset_Attributes where fieldorder='" & field(1) & "'"
                Else
                    sql = "select * from tbl_Asset_Attributes where fieldorder='" & field(1) & "' and catid=" & drpassetcategory.SelectedValue & ""
                End If
            End If
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
    Public Function GetFieldname(ByVal attributeid As String) As String
        Try
            Dim sql As String
            Dim s As String = ""
            dtable = New DataTable
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            'If attributeid = "EmployeeName" Or attributeid = "EmpDepartment" Or attributeid = "Location" Then
            '    Return s
            '    Exit Function
            'End If
            Dim field() As String = attributeid.Split(".")
            If field.Length > 0 Then
                If drpassetcategory.SelectedValue = "0" Then
                    sql = "select FieldOrder from tbl_Asset_Attributes where fieldorder='" & field(1) & "'"
                Else
                    sql = "select FieldOrder from tbl_Asset_Attributes where fieldorder='" & field(1) & "' and catid=" & drpassetcategory.SelectedValue & ""
                End If
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            s = Convert.ToString(cmd.ExecuteScalar())
            Return s
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function
    Private Function BindDropdown(ByVal drpdown As DropDownList, ByVal attid As String)
        Try
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
            Dim cmd As New System.Data.SqlClient.SqlCommand
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim str As String = ""
            con.Open()
            drpdown.Items.Clear()
            str = "select * from tbl_asset_AttributeList where attid  =" & attid
            cmd.CommandText = str
            cmd.Connection = con
            rdr = cmd.ExecuteReader
            While rdr.Read
                drpdown.Items.Add(New ListItem(rdr("attdesc"), rdr("attdesc")))
            End While
            drpdown.Items.Insert(0, "--Select--")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub DrpANDOR_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DrpANDOR.SelectedIndexChanged
        Try
            Dim condition As String = ""
            Dim usercondition As String = ""
            Dim conditionend As Boolean
            If conditionend = False Then
                If drpattributetype.SelectedValue <> "" Then
                    If drpattributetype.SelectedValue = "v_emp.Emp_Number" Or drpattributetype.SelectedValue = "v_emp.Emp_Name" Or drpattributetype.SelectedValue = "v_emp.Dep_Name" Or drpattributetype.SelectedValue.Contains("v_emp.BuildingUnit") = True Or drpattributetype.SelectedValue = "v_emp.Emp_Phone_Ext" Then
                        Dim txt As New TextBox
                        Dim drpasset As New DropDownList
                        Dim drpbuilding As New DropDownList
                        txt = CType(tdcondition.FindControl("txt123"), TextBox)
                        drpasset = CType(tdcondition.FindControl("drpasset123"), DropDownList)
                        drpbuilding = CType(tdcondition.FindControl("drpasset456"), DropDownList)
                        If drpasset IsNot Nothing Then
                            If drpasset.SelectedValue <> "" Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                usercondition = usercondition & " " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                txt.Text = ""
                            Else
                                If txt IsNot Nothing Then
                                    condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                    usercondition = usercondition & " " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                    txt.Text = ""
                                End If
                            End If
                        Else
                            If txt IsNot Nothing Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                usercondition = usercondition & " " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                txt.Text = ""
                            End If
                        End If
                        If drpbuilding IsNot Nothing Then
                            If drpbuilding.SelectedValue <> "" Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drpbuilding.SelectedItem.Text & "' "
                                usercondition = usercondition & " " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & drpbuilding.SelectedItem.Text & "' "
                            End If
                        End If
                    ElseIf drpattributetype.SelectedValue = "v_asset.status" Then
                        '  Dim txt As New TextBox
                        Dim drpasset As New DropDownList
                        '   txt = CType(tdcondition.FindControl("txt456"), TextBox)
                        drpasset = CType(tdcondition.FindControl("drpasset456"), DropDownList)
                        If drpasset IsNot Nothing Then
                            If drpasset.SelectedValue <> "" Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                            End If
                        End If
                    ElseIf drpattributetype.SelectedValue = "v_trans.transtype" Then
                        'Dim txt As New TextBox
                        Dim drpasset As New DropDownList
                        ' txt = CType(tdcondition.FindControl("txt1"), TextBox)
                        drpasset = CType(tdcondition.FindControl("drp1"), DropDownList)
                        If drpasset IsNot Nothing Then
                            If drpasset.SelectedValue <> "" Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                            End If
                        End If
                        ' Transaction date
                    ElseIf drpattributetype.SelectedValue = "v_trans.date1" Then
                        Dim txtdate1 As New TextBox
                        Dim txtdate2 As New TextBox
                        txtdate1 = CType(tdcondition.FindControl("txtdate1"), TextBox)
                        txtdate2 = CType(tdcondition.FindControl("txtdate2"), TextBox)
                        If DrpOperator.SelectedValue = "7" Then
                            If txtdate1 IsNot Nothing And txtdate2 IsNot Nothing Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txtdate1.Text & "' and '" & txtdate2.Text & "' "
                                usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & txtdate1.Text & "' and '" & txtdate2.Text & "' "
                                txtdate1.Text = ""
                                txtdate2.Text = ""
                            End If
                        Else
                            If txtdate1 IsNot Nothing Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txtdate1.Text & "' "
                                usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & txtdate1.Text & "' "
                                txtdate1.Text = ""
                            End If
                        End If
                    ElseIf drpattributetype.SelectedValue = "v_trans.activity" Then
                        '   Dim txt As New TextBox
                        Dim drpasset As New DropDownList
                        '  txt = CType(tdcondition.FindControl("txt456"), TextBox)
                        drpasset = CType(tdcondition.FindControl("drpasset456"), DropDownList)
                        If drpasset IsNot Nothing Then
                            If drpasset.SelectedValue <> "" Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedValue & "' "
                            End If
                        End If
                    ElseIf drpattributetype.SelectedValue = "v_trans.transcreatedid" Then
                        Dim txt As New TextBox
                        Dim drpasset As New DropDownList
                        txt = CType(tdcondition.FindControl("txt123"), TextBox)
                        drpasset = CType(tdcondition.FindControl("drpasset123"), DropDownList)
                        If drpasset IsNot Nothing Then
                            If drpasset.SelectedValue <> "" Then
                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedValue & "' "
                            Else
                                If txt IsNot Nothing Then
                                    condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                    usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                    txt.Text = ""
                                End If
                            End If
                        End If
                        'BuildingUnit
                    ElseIf drpattributetype.SelectedValue = "v_asset.BuildingUnit" Or drpattributetype.SelectedValue = "v_asset.Location" Or drpattributetype.SelectedValue = "v_asset.seatno" Then
                        Dim txt As New TextBox
                        Dim drpasset As New DropDownList
                        txt = CType(tdcondition.FindControl("txt123"), TextBox)
                        drpasset = CType(tdcondition.FindControl("drpasset123"), DropDownList)
                        If drpasset IsNot Nothing Then
                            If drpasset.SelectedValue <> "" Then
                                condition = condition & " " & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedValue & "' "
                            Else
                                If txt IsNot Nothing Then
                                    condition = condition & " " & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "'"
                                    usercondition = usercondition & "  " & drpattributetype.SelectedValue & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                    txt.Text = ""
                                End If
                            End If
                        End If
                    Else
                        dtable = New DataTable
                        dtable = GetFieldtype(drpattributetype.SelectedValue)
                        If dtable.Rows.Count > 0 Then
                            Dim txt As New TextBox
                            Dim txtdate1 As New TextBox
                            Dim txtdate2 As New TextBox
                            Dim drp As New DropDownList
                            Dim drpasset As New DropDownList
                            Dim rdo As New RadioButtonList
                            If DrpOperator.SelectedValue <> "0" Then
                                If UCase(dtable.Rows(0)("AttType")) = UCase("Date") Then
                                    txtdate1 = CType(tdcondition.FindControl("txtdate1" & dtable.Rows(0)("attid").ToString), TextBox)
                                    txtdate2 = CType(tdcondition.FindControl("txtdate2" & dtable.Rows(0)("attid").ToString), TextBox)
                                    If DrpOperator.SelectedValue = "7" Then
                                        If txtdate1 IsNot Nothing And txtdate2 IsNot Nothing Then
                                            condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txtdate1.Text & "' and '" & txtdate2.Text & "' "
                                            usercondition = usercondition & " convert(datetime,v_asset." & GetFieldname(drpattributetype.SelectedValue) & ",103) " & DrpOperator.SelectedItem.Text & " '" & txtdate1.Text & "' and '" & txtdate2.Text & "' "
                                            txtdate1.Text = ""
                                            txtdate2.Text = ""
                                        End If
                                    Else
                                        If txtdate1 IsNot Nothing Then
                                            condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txtdate1.Text & "' "
                                            usercondition = usercondition & " convert(datetime,v_asset." & GetFieldname(drpattributetype.SelectedValue) & ",103) " & DrpOperator.SelectedItem.Text & " '" & txtdate1.Text & "' "
                                            txtdate1.Text = ""
                                        End If
                                    End If
                                ElseIf UCase(dtable.Rows(0)("AttType")) = UCase("SingleSelection") Then
                                    drp = CType(tdcondition.FindControl("drp" & dtable.Rows(0)("attid").ToString), DropDownList)
                                    If drp IsNot Nothing Then
                                        condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drp.SelectedItem.Text & "' "
                                        usercondition = usercondition & " v_asset." & GetFieldname(drpattributetype.SelectedValue) & " " & DrpOperator.SelectedItem.Text & " '" & drp.SelectedItem.Text & "' "
                                        drp.SelectedIndex = 0
                                    End If
                                ElseIf UCase(dtable.Rows(0)("atttype")) = UCase("Yes/No") Then
                                    rdo = CType(tdcondition.FindControl("rdo" & dtable.Rows(0)("attid").ToString), RadioButtonList)
                                    If rdo IsNot Nothing Then
                                        condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & rdo.SelectedItem.Text & "' "
                                        usercondition = usercondition & " v_asset." & GetFieldname(drpattributetype.SelectedValue) & " " & DrpOperator.SelectedItem.Text & " '" & rdo.SelectedItem.Text & "' "
                                        rdo.SelectedIndex = 0
                                    End If
                                Else
                                    txt = CType(tdcondition.FindControl("txt" & dtable.Rows(0)("attid").ToString), TextBox)
                                    drpasset = CType(tdcondition.FindControl("drp" & dtable.Rows(0)("attid").ToString), DropDownList)
                                    If drpasset IsNot Nothing Then
                                        If drpasset.SelectedValue <> "" Then
                                            condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                            usercondition = usercondition & " v_asset." & GetFieldname(drpattributetype.SelectedValue) & " " & DrpOperator.SelectedItem.Text & " '" & drpasset.SelectedItem.Text & "' "
                                            txt.Text = ""
                                        Else
                                            If txt IsNot Nothing Then
                                                condition = condition & drpattributetype.SelectedItem.Text & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                                usercondition = usercondition & " v_asset." & GetFieldname(drpattributetype.SelectedValue) & " " & DrpOperator.SelectedItem.Text & " '" & txt.Text & "' "
                                                txt.Text = ""
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If
            End If
            If DrpANDOR.SelectedValue = "0" Then
            ElseIf DrpANDOR.SelectedValue = "1" Then
                conditionend = True
                If condition <> "" Then
                    If Session("OR") = "True" Then
                        Session("OR") = "False"
                        condition = condition & ")"
                        usercondition = usercondition & ")"
                    End If
                End If
                LblCondText.Text = LblCondText.Text & condition
                LblInvConText.Text = LblInvConText.Text & usercondition
                If Trim(Right(LblCondText.Text, 4)) = "and" Then
                    LblCondText.Text = Left(LblCondText.Text, Len(LblCondText.Text) - 4)
                    LblInvConText.Text = Left(LblInvConText.Text, Len(LblInvConText.Text) - 4)
                End If
                If assettype = False Then
                    Dim myscript1 As String = "alert('Please select Asset type! ');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                    chkassettype.Focus()
                    Exit Sub
                End If
                If reporthdr = False Then
                    Dim myscript1 As String = "alert('Please select Report Headers! ');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                    chkreportheader.Focus()
                    Exit Sub
                End If

            ElseIf DrpANDOR.SelectedValue = "2" Then
                If Session("OR") = "True" Then
                    Session("OR") = "False"
                    condition = condition & ")"
                    usercondition = usercondition & ")"
                End If
                If condition <> "" Then
                    condition = condition & " and "
                    usercondition = usercondition & " and "
                End If

                LblCondText.Text = LblCondText.Text & condition
                LblInvConText.Text = LblInvConText.Text & usercondition

            ElseIf DrpANDOR.SelectedValue = "3" Then
                If Session("OR") <> "True" Then
                    Session("OR") = "True"
                    condition = "(" & condition
                    usercondition = "(" & usercondition
                End If
                If condition <> "" Then
                    condition = condition & " or "
                    usercondition = usercondition & " or "
                End If
                LblCondText.Text = LblCondText.Text & condition
                LblInvConText.Text = LblInvConText.Text & usercondition
            End If
            drpattributetype.SelectedIndex = 0
            DrpANDOR.SelectedIndex = 0
            DrpOperator.SelectedIndex = 0
            tdcondition.Controls.Clear()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub


    Protected Sub drpattributetype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpattributetype.SelectedIndexChanged
        DrpOperator.SelectedIndex = 0
    End Sub
    Dim assettype As Boolean = False
    Dim reporthdr As Boolean = False
    Dim assettypes As String = ""
    Dim accessrights As Integer = 0
    Dim reportheaders As String = ""
    Dim reportheaderdesc As String = ""
    Dim smessage As String = ""
    Dim groupbyvalue As String = ""
    Public Function GetReportCondition()
        For Each lst As ListItem In chkassettype.Items
            If lst.Selected = True Then
                assettype = True
                assettypes &= "'" & lst.Value & "',"
            End If
        Next

        For Each lst1 As ListItem In chkreportheader.Items
            If lst1.Selected = True Then
                reporthdr = True
                reportheaders &= "isnull(" & lst1.Value & ",''),"
                reportheaderdesc &= "" & lst1.Text & "|"
            End If
        Next

        reportheaders = reportheaders.Replace("'", "''")

        If reportheaders <> "" Then
            reportheaders = Left(reportheaders, Len(reportheaders) - 1)
            reportheaderdesc = Left(reportheaderdesc, Len(reportheaderdesc) - 1)
        End If

        If assettypes <> "" Then
            assettypes = Left(assettypes, Len(assettypes) - 1)
            assettypes = "in(" & assettypes & ")"
        End If
        If chkrights.Checked = True Then
            accessrights = 1
        End If

        If String.IsNullOrEmpty(LblInvConText.Text) Then
            LblInvConText.Text = "0=0"
        End If
        If chkall.Checked = True Then
            LblInvConText.Text &= " and (v_asset.assettypeid " & assettypes & " )"
        End If
        If String.IsNullOrEmpty(LblCondText.Text) Then
            LblCondText.Text = "0=0"
        End If

        If drpgroupby.SelectedValue <> "" Then
            groupbyvalue = "isnull(" & drpgroupby.SelectedValue & ",'''')"
        Else
            If chkall.Checked = True Then
                groupbyvalue = "v_asset.assettypecode"
            End If
        End If
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim sql As String
            GetReportCondition()
            If assettype = False Then
                Dim myscript1 As String = "alert('Please select Asset type! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                chkassettype.Focus()
                Exit Sub
            End If
            If reporthdr = False Then
                Dim myscript1 As String = "alert('Please select Report Headers! ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                chkreportheader.Focus()
                Exit Sub
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()


            sql = "insert into tbl_Asset_reportformula(reportname,repcatid,assetcatid,assettypes,condition,usercondition,groupby,rights,createdby,createddate,reportheaders,reportheaderdesc,groupbydesc,status)"
            sql = sql & " values('" & txtreportname.Text.Trim & "'," & drpreportcategory.SelectedValue & "," & drpassetcategory.SelectedValue & ",'" & assettypes.Replace("'", "''") & "','(" & LblCondText.Text.Replace("'", "''") & ")',"
            sql = sql & "'(" & LblInvConText.Text.Replace("'", "''") & ") ','" & groupbyvalue & "'," & accessrights & ",'" & Convert.ToString(Session("EmpNo")) & "','" & DateTime.Now.Date & "','" & reportheaders & "', "
            sql = sql & "'" & reportheaderdesc & "','" & groupbyvalue & "','v_asset.status  <> ''Sold''')"
            cmd = New SqlCommand(sql, con)
            'cmd = New SqlCommand(sql & " SELECT id FROM tbl_Asset_reportformula WHERE id = @@IDENTITY", con)
            Try
                cmd.ExecuteNonQuery()
                Session("Message") = "Report Formula Inserted Successfully!"
            Catch ex As Exception
                Session("Message") = "Report Formula Not Inserted Successfully!"
                lblmessage.ForeColor = Drawing.Color.Red
            End Try
            cmd.Dispose()
            con.Close()
            sql.Trim()
            Response.Redirect("ReportFormula.aspx", True)
            '  BindGrid()


        Catch ex As Exception
            lblmessage.ForeColor = Drawing.Color.Red
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub drpreportcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpreportcategory.SelectedIndexChanged
        Try
            If drpreportcategory.SelectedValue <> "" Then
                BindGrid("", "")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function BindGrid(ByVal sortExpression As String, ByVal direction As String)
        Try

            Dim sql As String
            dtable = New DataTable
            sql = "select * from tbl_Asset_reportformula where repcatid='" & drpreportcategory.SelectedValue & "' order by id DESC"
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
                grdformula.DataSource = myDataView
                grdformula.DataBind()
            Else
                grdformula.EmptyDataText = "No data Found"
                grdformula.DataBind()
            End If
            oAdapter.Dispose()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub grdformula_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdformula.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Text = "0" Then
                e.Row.Cells(3).Text = "Restricted"
                Dim imgbtn As ImageButton = CType(e.Row.Cells(4).FindControl("imgdelete"), ImageButton)
                Dim lbluser As Label = CType(e.Row.Cells(5).FindControl("lblUser"), Label)
                If imgbtn IsNot Nothing Then
                    If lbluser IsNot Nothing And lbluser.Text = Session("EmpNo") Then
                        imgbtn.Visible = True
                    Else
                        imgbtn.Visible = False
                    End If
                End If
            ElseIf e.Row.Cells(3).Text = "1" Then
                e.Row.Cells(3).Text = "Full"
            End If

            Dim button As ImageButton = DirectCast(e.Row.Cells(4).FindControl("imgdelete"), ImageButton)
            If button IsNot Nothing AndAlso button.CommandName = "Deleting" Then
                button.OnClientClick = "if (!confirm('Are you sure want to delete this record?')) return false;"
            End If
        End If

        If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
            If Session("Usergroup") <> "1" Then
                Dim imgbtn As ImageButton = CType(e.Row.FindControl("imgdelete"), ImageButton)
                If imgbtn IsNot Nothing Then
                    imgbtn.Enabled = False
                    imgbtn.ImageUrl = "images/dash.jpg"
                End If
            End If
        End If
    End Sub

    Protected Sub grdformula_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdformula.RowCommand
        Dim rowid As Integer = 0
        Dim sql As String = ""
        Dim returnvalue As String = ""
        Try
            If e.CommandName = "Deleting" Then
                rowid = Convert.ToInt32(e.CommandArgument)
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = "delete from tbl_Asset_reportformula where id=" & rowid
                cmd = New SqlCommand(sql, con)
                returnvalue = cmd.ExecuteNonQuery()
                cmd.Dispose()
                con.Close()
            End If
            BindGrid("", "")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("OR") = "False"
    End Sub
    Protected Sub btnclearcondtion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclearcondtion.Click
        drpattributetype.SelectedIndex = 0
        DrpOperator.SelectedIndex = 0
        DrpANDOR.SelectedIndex = 0
        LblCondText.Text = ""
        LblInvConText.Text = ""
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
    Protected Sub grdformula_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdformula.Sorting
        BindGrid(e.SortExpression, sortOrder)
    End Sub

    Protected Sub chkall_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkall.CheckedChanged
        Try
            If drpassetcategory.SelectedValue <> "" Then
                BindAssetType(drpassetcategory.SelectedValue, True)
            End If
            If chkall.Checked = True Then
                drpassetcategory.SelectedIndex = 0
                drpgroupby.Enabled = False
            Else
                drpgroupby.Enabled = True
                chkassettype.Items.Clear()
                chkreportheader.Items.Clear()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub chkassettype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkassettype.SelectedIndexChanged
        Try
            Dim items As String = ""
            If chkall.Checked = True Then
                For Each lst1 As ListItem In chkassettype.Items
                    If lst1.Selected Then
                        items &= "'" & lst1.Value & "',"
                    End If
                Next
                If items.Length > 0 Then
                    items = Left(items, Len(items) - 1)
                    BindAssetAttributes(drpassetcategory.SelectedValue, True, items)
                Else
                    chkreportheader.Items.Clear()
                    drpattributetype.Items.Clear()
                    drpgroupby.Items.Clear()
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub btnviewreport_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnviewreport.Click
        '----------View Report----------------------
        If txtreportname.Text.Trim() = "" Then
            Dim myscript1 As String = "alert('Please Enter Report Name! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            txtreportname.Focus()
            Exit Sub
        End If
        GetReportCondition()
        If assettype = False Then
            Dim myscript1 As String = "alert('Please select Asset type! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            chkassettype.Focus()
            Exit Sub
        End If
        If reporthdr = False Then
            Dim myscript1 As String = "alert('Please select Report Headers! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            chkreportheader.Focus()
            Exit Sub
        End If
        Session("Values") &= txtreportname.Text.Trim & "*"
        Session("Values") &= drpreportcategory.SelectedValue & "*"
        Session("Values") &= drpassetcategory.SelectedValue & "*"
        Session("Values") &= assettypes & "*"
        Session("Values") &= LblInvConText.Text.Trim & "*"
        Session("Values") &= LblCondText.Text.Trim & "*"
        Session("Values") &= groupbyvalue & "*"
        Session("Values") &= accessrights & "*"
        Session("Values") &= reportheaders & "*"
        Session("Values") &= reportheaderdesc & "*"
        Session("Values") &= "v_asset.status <> 'Sold'*"
        Response.Redirect("ViewReport1.aspx")
        '-------------------------------------------
    End Sub
End Class
