Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports AjaxControlToolkit
Partial Class DeploytoUsers
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim dtableattributes As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
            If imgbtn IsNot Nothing Then
                imgbtn.Focus()
            End If
            If Not IsPostBack Then
                bindcategory()
                BindUsers()
            End If
            txtdate.Text = Format(Now(), "dd-MMM-yyyy")
            If drpCategory.SelectedValue <> "" Then
                If drpConsType.SelectedValue <> "" Then
                    dtableattributes = New DataTable
                    dtableattributes = GetAssetAttributes()
                End If
            End If
            If drpConsType.SelectedItem.Text <> "Toner" And drpConsType.SelectedItem.Text <> "--Select--" And rdolocation.SelectedValue <> "" Then
                GetConsumableDetails()
                trconsumables.Visible = True
            End If
            lblMessage.Text = ""
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub WriteLogFile(ByVal fileName As String, ByVal methodName As String, ByVal message As String)
        Try
            If message <> "" Then
                Using file As New FileStream(Server.MapPath("~\LogFile\Log.txt"), FileMode.Append, FileAccess.Write)
                    Dim streamWriter As New StreamWriter(file)
                    streamWriter.WriteLine((((System.DateTime.Now & " - ") + fileName & " - ") + methodName & " - ") + message)
                    streamWriter.Close()
                End Using
            End If
        Catch ex As Exception
            WriteLogFile(Me.[GetType]().Name, "btnsave_Click()", ex.Message.ToString())
        End Try
    End Sub
    Public Sub BindUsers()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        lstuser.Items.Clear()
        cmd = New SqlCommand("select * from view_SIP_Employees order by Emp_Name", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                lstuser.Items.Add(New ListItem(rdr("Emp_Name") & " " & rdr("Emp_Initial") & "( " & rdr("Emp_Number") & " )", rdr("Emp_Number")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Sub
    Private Function GetAssetAttributes() As DataTable
        Dim sql As String
        Try
            sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aad.AssetTypeId = " & drpConsType.SelectedValue & " order by aa.attid asc"
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

    Private Sub bindcategory()
        con.Open()
        drpCategory.Items.Clear()
        drpCategory.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where ac.groupid='1' and am.AssetTypeCode <>'Printer'  order by am.CatId ", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpCategory.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Sub
    Private Function BindAssetType(ByVal categoryid As String)
        Try
            Dim sql As String
            sql = "select AM.AssetTypeId,AM.AssetTypeCode from tbl_Asset_TypeMaster am where am.assettypeid in (select acm.constypeid from tbl_Asset_cons_Mapping acm where acm.assettypeid = '" & categoryid & "')"
            con.Open()
            drpConsType.Items.Clear()
            drpConsType.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpConsType.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub drpCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpCategory.SelectedIndexChanged
        Try
            If drpCategory.SelectedValue <> "" Then
                lblQuantity.Text = ""
                BindAssetType(drpCategory.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub drpConsType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpConsType.SelectedIndexChanged
        'Try
        '    If drpConsType.SelectedIndex = 0 Then
        '        lblQuantity.Text = ""
        '        txtAssignQty.Text = ""
        '        txtAssignQty.ReadOnly = True
        '    End If
        '    If drpConsType.SelectedValue <> "" Then
        '        txtAssignQty.Text = ""
        '        BindQuantity(drpConsType.SelectedValue)
        '    End If
        'Catch ex As Exception
        '    WriteLogFile(Me.[GetType]().Name, "drpConsType_SelectedIndexChanged()", ex.Message.ToString())
        'End Try
        lblQuantity.Text = ""
        Try
            'To Display Location
            Dim sql As String
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            sql = ""
            sql = "select loc.locid as LocID,loc.locname AS LocationName,subloc.sublocid as SubLocID ,subloc.sublocname AS SubLocationName,st.ConsTypeId  from tbl_Asset_Cons_Stock st"
            sql = sql & " left join tbl_Asset_location loc on st.locid = loc.locid left join tbl_Asset_sublocation subloc on st.sublocid = subloc.sublocid where st.constypeid = '" & drpConsType.SelectedValue & "' "
            sqladr = New SqlDataAdapter(sql, con)
            dtable = New DataTable
            sqladr.Fill(dtable)
            If dtable.Rows.Count > 0 Then
                rdolocation.Items.Clear()
                For Each dr As DataRow In dtable.Rows
                    If rdolocation.Items.FindByValue(dr("SubLocID")) IsNot Nothing Then
                    Else
                        rdolocation.Items.Add(New ListItem(dr("LocationName") & "( " & dr("SubLocationName") & " )", dr("SubLocID")))
                    End If
                Next
            Else
                rdolocation.Items.Clear()
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Try
            'Validate the Controls
            ' First validate the user
            If lstuser.SelectedValue = "" Then
                Dim myscript As String = "alert('Select User Name');"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                lstuser.Focus()
                Exit Sub
            End If
            ' Validate Name of Category
            If drpCategory.SelectedValue = "" Then
                Dim myscript As String = "alert('Select Category Name');"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                drpCategory.Focus()
                Exit Sub
            End If
            If drpConsType.SelectedValue = "" Then
                Dim myscript As String = "alert('Select Consumable Name');"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "myscript", myscript, True)
                drpConsType.Focus()
                Exit Sub
            End If
            ' Update the Stock List
            If ViewState("Query") = "" Then
                Dim rtn As String = UpdateConsumableStock()
                If rtn = "Success" Then
                    'Insert into tbl_Asset_Cons_Transactions table
                    InsertTransactions()
                    ResetFormControlValues(Me)
                ElseIf rtn = "Failure" Then
                    lblMessage.Text = "Consumables Not Available In this Location"
                End If
            Else

            End If
            'Reset All controls
            GridView1.EmptyDataText = ""
            GridView1.DataBind()
            lblMessage.Visible = True
            lblQuantity.Text = "0"
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Sub InsertTransactions()
        Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim SQL As String
        Dim userID As Integer
        Dim AssignQtyCnt As Integer
        Dim AsgQtycnt As Integer = 1

        'To Get the User
        userID = Session("EmpNo")
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()

        'Insert "N" number of values into tbl_Asset_Cons_Transactions based on Assigned Quantity
        'Insert Always "1" till the "N" number of Assign Quantity.
        If lblQuantity.Text > 0 Then
            SQL = "insert into tbl_Asset_Cons_Transactions(ConsTypeId,TransType,Date1,Date2,UserId,Remarks,TransBy,quantity,status) values ('" & drpConsType.SelectedValue
            SQL = SQL & "','" & 3 & "','" & txtdate.Text & "','" & Now & "','" & lstuser.SelectedValue & "','" & txtremarks.Text & "','" & userID & "','" & AsgQtycnt & "','U')"
            cmd.Connection = con
            cmd.CommandText = SQL
            cmd.ExecuteNonQuery()
            con.Close()
        End If
    End Sub
    Private Sub ResetFormControlValues(ByVal parent As Control)
        For Each c As Control In parent.Controls
            If c.Controls.Count > 0 Then
                ResetFormControlValues(c)
            Else
                Select Case (c.GetType().ToString())
                    Case "System.Web.UI.WebControls.TextBox"
                        CType(c, TextBox).Text = ""
                    Case "System.Web.UI.WebControls.DropDownList"
                        CType(c, DropDownList).SelectedIndex = 0
                    Case "System.Web.UI.WebControls.GridView"
                        CType(c, GridView).EmptyDataText = ""
                        CType(c, GridView).DataBind()
                End Select
            End If
        Next c
    End Sub
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
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("Default.aspx")
    End Sub
    Public Function GetAssetAttributesFields() As DataTable
        Dim sql As String
        Try
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            sql = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header = '0' and aad.AssetTypeId = " & drpConsType.SelectedValue & " order by aa.attid asc"
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
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim sno As New Label
        Dim SrNo As Integer

        If e.Row.RowType = DataControlRowType.DataRow Then
            SrNo = (GridView1.PageIndex + 1) * 10 - 9
            sno = e.Row.FindControl("Sno")
            sno.Text = e.Row.RowIndex + SrNo
            sno.DataBind()
        End If
    End Sub

    Protected Sub lstuser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstuser.SelectedIndexChanged
        Try
            BindGrid()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim SQL As String
            dtable = New DataTable
            SQL = "SELECT view_SIP_Employees.Emp_Number AS EmpNumber, view_SIP_Employees.Emp_Name + ' ' + view_SIP_Employees.Emp_Initial  AS EmpName, "
            SQL = SQL & " tbl_Asset_TypeMaster.AssetTypeDesc AS ConsumableName,CONVERT(VARCHAR(15), tbl_Asset_Cons_Transactions.Date1, 106) AS DeploymentDate,"
            SQL = SQL & " SUM(tbl_Asset_Cons_Transactions.quantity) AS Quantity,tbl_Asset_Cons_Transactions.Remarks AS Reason"
            SQL = SQL & " FROM "
            SQL = SQL & " tbl_Asset_Cons_Transactions INNER JOIN view_SIP_Employees ON tbl_Asset_Cons_Transactions.UserId = view_SIP_Employees.Emp_Number INNER JOIN"
            SQL = SQL & " tbl_Asset_TypeMaster ON tbl_Asset_Cons_Transactions.ConsTypeId = tbl_Asset_TypeMaster.AssetTypeId"
            SQL = SQL & " where tbl_Asset_Cons_Transactions.userid='" & lstuser.SelectedValue & "' and tbl_Asset_Cons_Transactions.status='U'"
            SQL = SQL & " group by view_SIP_Employees.Emp_Number,view_SIP_Employees.Emp_Name,view_SIP_Employees.Emp_Initial,tbl_Asset_TypeMaster.AssetTypeDesc,tbl_Asset_Cons_Transactions.Date1,tbl_Asset_Cons_Transactions.Remarks"
            con.Open()
            sqladr = New SqlDataAdapter(SQL, con)
            sqladr.Fill(dtable)
            If dtable.Rows.Count > 0 Then
                GridView1.DataSource = dtable
                GridView1.DataBind()
            Else
                GridView1.EmptyDataText = "No Data Found"
                GridView1.DataBind()
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub rdolocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdolocation.SelectedIndexChanged
        Try
            If drpConsType.SelectedValue <> "" Then
                GetConsumableTotal(drpConsType.SelectedValue, rdolocation.SelectedValue)
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function GetConsumableTotal(ByVal ConsumableTypeId As String, ByVal SublocationId As String)
        Try

            Dim SQLQty As String
            SQLQty = "select atm.AssetTypeCode as ConsumableName,atm.AssetTypeId as Consumableid, sum(acs.quantity) as Quantity from tbl_Asset_CategoryMaster acm,tbl_Asset_TypeMaster atm,tbl_Asset_Cons_Stock acs where(acm.catid = atm.catid And atm.AssetTypeId = acs.ConsTypeId And acm.groupid = 3) and atm.AssetTypeId = '" & ConsumableTypeId & "' and acs.sublocid=" & SublocationId & " group by atm.AssetTypeCode,atm.AssetTypeId"
            con.Open()
            cmd = New SqlCommand(SQLQty, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    lblQuantity.Text = rdr("Quantity")
                End While
            Else
                lblQuantity.Text = "0"
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function GetConsumableDetails()
        Try
            If rdolocation.SelectedValue <> "" Then
                dtableattributes = New DataTable
                dtableattributes = GetAssetAttributes()
                If dtableattributes.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable.ID = "strtable1"
                    strtable.Width = Unit.Percentage(100)
                    For i As Integer = 0 To dtableattributes.Rows.Count - 1
                        Dim strrow As New TableRow
                        Dim strCell_1 As New TableCell
                        Dim strCell_2 As New TableCell
                        strCell_1.Width = Unit.Percentage(15)
                        strCell_2.Width = Unit.Percentage(85)
                        strtable.Style("align") = "center"
                        strCell_1.Style("text-align") = "right"
                        ' strCell_2.Style("text-align") = "center"
                        ' strCell_2.Style("font-weight") = "bold"
                        strCell_1.Style.Add("padding-right", "2px")
                        strCell_2.Style("text-align") = "left"
                        Dim lbl As New Label
                        'Dim lbl2 As New Label
                        'lbl2.ID = "lblsy" & dtableattributes(i)("attid").ToString
                        'lbl2.Text = ":"

                        '  strrow.Cells.Add(strCell_2)
                        Dim s As Boolean = False
                        If UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                            Dim rdosingle As New RadioButtonList
                            rdosingle.CssClass = "control"
                            rdosingle.RepeatDirection = WebControls.RepeatDirection.Horizontal
                            rdosingle.RepeatColumns = "5"
                            rdosingle.ID = "rdo" & dtableattributes(i)("attid").ToString
                            s = Bindradiobutton(rdosingle, drpConsType.SelectedValue, dtableattributes(i)("FieldOrder").ToString, rdolocation.SelectedValue)
                            lbl.ID = "lbl" & dtableattributes(i)("attid").ToString
                            lbl.Text = dtableattributes(i)("attdesc").ToString & " :"
                            strCell_1.Controls.Add(lbl)
                            strCell_2.Controls.Add(rdosingle)
                            strrow.Cells.Add(strCell_1)
                            strrow.Cells.Add(strCell_2)
                        End If
                        If s = True Then
                            strtable.Rows.Add(strrow)
                        End If
                    Next
                    tdconsumables.Controls.Add(strtable)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Private Function Bindradiobutton(ByVal rdobtn As RadioButtonList, ByVal attid As String, ByVal fieldname As String, ByVal sublocid As String) As Boolean
        Try
            Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
            Dim cmd As New System.Data.SqlClient.SqlCommand
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim str As String = ""
            con.Open()
            rdobtn.Items.Clear()
            str = "select distinct " & fieldname & " from tbl_Asset_Cons_Stock where ConsTypeId  =" & attid & " and sublocid=" & sublocid
            cmd.CommandText = str
            cmd.Connection = con
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    rdobtn.Items.Add(New ListItem(rdr("" & fieldname & ""), rdr("" & fieldname & "")))
                End While
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function UpdateConsumableStock() As String
        Try
            Dim returnvalue As String = ""
            Dim fields As String = "select quantity from tbl_Asset_Cons_Stock where "
            Dim upfields As String = " "
            Dim fields1 As String = ""
            Dim values As String = ""
            If rdolocation.SelectedValue <> "" Then
                dtableattributes = New DataTable
                dtableattributes = GetAssetAttributes()
                If dtableattributes.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable = CType(tdconsumables.FindControl("strtable1"), Table)
                    Dim rdo As RadioButtonList
                    For i As Integer = 0 To dtableattributes.Rows.Count - 1
                        If UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                            rdo = CType(strtable.FindControl("rdo" & dtableattributes(i)("attid").ToString), RadioButtonList)
                            If rdo IsNot Nothing Then
                                values = values & " " & dtableattributes(i)("FieldOrder") & "='" & rdo.SelectedItem.Text & "' and"
                            End If
                        End If
                    Next
                End If
            End If
            If values <> "" Then
                fields = fields & " " & values & "  ConsTypeId = " & drpConsType.SelectedValue & " And sublocid = " & rdolocation.SelectedValue & ""
                fields1 = values & "  ConsTypeId = " & drpConsType.SelectedValue & " And sublocid = " & rdolocation.SelectedValue & ""
            End If
            Dim sql As String = ""
            Dim result As Boolean = True
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(fields, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    If CInt(rdr("quantity")) <> "0" Then
                        upfields = "update tbl_Asset_Cons_Stock set quantity =" & CInt(rdr("quantity")) - 1 & " where " & fields1
                        If con1.State = ConnectionState.Open Then
                            con1.Close()
                        End If
                        con1.Open()
                        cmd1 = New SqlCommand(upfields, con1)
                        cmd1.ExecuteNonQuery()
                        result = True
                        con1.Close()
                        returnvalue = "Success"
                    Else
                        returnvalue = "Failure"
                    End If
                End While
            Else
                returnvalue = "Failure"
            End If
            rdr.Close()
            con.Close()
            If result = True Then
                lblMessage.Text = "Consumable Successfully Deployed to the User -" & lstuser.SelectedItem.Text
            Else
                lblMessage.Text = "Consumable Cannot be Deployed for the User.!"
            End If
            Return returnvalue
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn.Click
        Try
            Dim returnvalue As String = ""
            Dim fields As String = "select quantity from tbl_Asset_Cons_Stock where "
            Dim upfields As String = " "
            Dim fields1 As String = ""
            Dim values As String = ""
            If rdolocation.SelectedValue <> "" Then
                dtableattributes = New DataTable
                dtableattributes = GetAssetAttributes()
                If dtableattributes.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable = CType(tdconsumables.FindControl("strtable1"), Table)
                    Dim rdo As RadioButtonList
                    For i As Integer = 0 To dtableattributes.Rows.Count - 1
                        If UCase(dtableattributes(i)("atttype")) = UCase("SingleSelection") Then
                            rdo = CType(strtable.FindControl("rdo" & dtableattributes(i)("attid").ToString), RadioButtonList)
                            If rdo IsNot Nothing Then
                                values = values & " " & dtableattributes(i)("FieldOrder") & "='" & rdo.SelectedItem.Text & "' and"
                            End If
                        End If
                    Next
                End If
            End If
            If values <> "" Then
                fields = fields & " " & values & "  ConsTypeId = " & drpConsType.SelectedValue & " And sublocid = " & rdolocation.SelectedValue & ""
                fields1 = values & "  ConsTypeId = " & drpConsType.SelectedValue & " And sublocid = " & rdolocation.SelectedValue & ""
            End If
            Dim sql As String = ""
            Dim result As Boolean = True
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(fields, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    If CInt(rdr("quantity")) <> "0" Then
                        upfields = "update tbl_Asset_Cons_Stock set quantity =" & CInt(rdr("quantity")) - 1 & " where " & fields1
                    Else
                        returnvalue = "Consumable Not Available In this Location!"
                    End If
                End While
            End If
            rdr.Close()
            con.Close()
            Dim userID As Integer
            Dim AssignQtyCnt As Integer
            Dim AsgQtycnt As Integer = 1

            'To Get the User
            userID = Session("EmpNo")
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()

            'Insert "N" number of values into tbl_Asset_Cons_Transactions based on Assigned Quantity
            'Insert Always "1" till the "N" number of Assign Quantity.
            If lblQuantity.Text > 0 Then
                sql = "insert into tbl_Asset_Cons_Transactions(ConsTypeId,TransType,Date1,Date2,UserId,Remarks,TransBy,quantity,status) values ('" & drpConsType.SelectedValue
                sql = sql & "','" & 3 & "','" & txtdate.Text & "','" & Now & "','" & lstuser.SelectedValue & "','" & txtremarks.Text & "','" & userID & "','" & AsgQtycnt & "','U')"
            End If
            If returnvalue = "" Then
                ViewState("Query") = ViewState("Query") & upfields & "|" & sql & "*"
                lblMessage.Text = "Consumable Updated Successfully!"
            Else
                lblMessage.Text = returnvalue
            End If
            ResetFormControlValues(Me)
            trconsumables.Visible = False
            lblQuantity.Text = ""
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
End Class
