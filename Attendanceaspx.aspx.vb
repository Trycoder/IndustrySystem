Imports System.Data
Imports System.Data.SqlClient
Imports System.cl
Partial Class Attendanceaspx
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim empDict As New Dictionary(Of Integer, Decimal)
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Dim dtt As DateTime = New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            Calendar1.SelectedDate = dtt.AddDays(-1).ToString()

            LoadGridByDate(Calendar1.SelectedDate)
            LoadWorkHours()
        End If
    End Sub
    Public Sub LoadWorkHours()
        Dim str As String = "Select e.empId,c.whrs from tbl_employee e left join tbl_EmployeeCategory c on e.empcatid=c.EmpCatId"
        con.Open()
        cmd = New SqlCommand(str, con)
        rdr = cmd.ExecuteReader

        If rdr.HasRows Then
            While (rdr.Read)
                Dim employeeid = Integer.Parse(rdr("empId"))
                Dim whrs As Decimal
                If (Not IsDBNull(rdr("whrs"))) Then
                    whrs = Decimal.Parse(rdr("whrs"))
                End If

                empDict.Add(employeeid, whrs)
            End While

        End If
        con.Close()
    End Sub
    Public Sub LoadDefaultGrid(ByRef dt As DateTime)
        Dim str As String = "SELECT e.empid,e.empcode, e.empname,Time1,Time2,Time3,Time4,Islunch,whrs,othrs FROM tbl_employee e  left join tbl_att_temp a on e.empid=a.empid "

        Dim oAdapter As New SqlDataAdapter(str, con)

        Dim myDataSet As New DataSet()
        oAdapter.Fill(myDataSet)
        GrdDetails.DataSource = myDataSet
        GrdDetails.DataBind()
        oAdapter.Dispose()
        con.Close()
        Calendar1.SelectedDate = dt
    End Sub
    Protected Sub GrdDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdDetails.PageIndexChanging
        GrdDetails.PageIndex = e.NewPageIndex
        LoadDefaultGrid(Calendar1.SelectedDate.Value)
    End Sub
    Public Sub LoadGridByDate(ByVal dateCondition As DateTime)
        Dim str As String = "SELECT e.empid,e.empcode, e.empname,Time1,Time2,Time3,Time4,Islunch,whrs,othrs FROM tbl_employee e  left join tbl_attendance a on e.empid=a.empid where a.attdate= @dt"
        If (IsDataAvailable(dateCondition)) Then
            Dim oAdapter As New SqlDataAdapter(str, con)
            oAdapter.SelectCommand.Parameters.AddWithValue("@dt", dateCondition)
            Dim myDataSet As New DataSet()
            oAdapter.Fill(myDataSet)
            If myDataSet.Tables.Count > 0 Then
                Dim myDataView As New DataView()
                myDataView = myDataSet.Tables(0).DefaultView
                If (myDataView.Table.Rows.Count > 0) Then
                    GrdDetails.DataSource = Nothing
                    GrdDetails.DataBind()
                    GrdDetails.DataSource = myDataView
                    GrdDetails.DataBind()
                    oAdapter.Dispose()
                    con.Close()
                    Calendar1.SelectedDate = dateCondition
                Else
                    LoadDefaultGrid(dateCondition)

                End If

            Else
                LoadDefaultGrid(dateCondition)
            End If
        Else
            LoadDefaultGrid(dateCondition)
        End If

    End Sub

    Protected Sub GrdDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdDetails.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim empcodelbl As Label = DirectCast(e.Row.FindControl("txtEmpCode"), Label)
                Dim empNamelbl As Label = DirectCast(e.Row.FindControl("txtEmpName"), Label)
                Dim T1txtbx As TextBox = DirectCast(e.Row.FindControl("txtInTime"), TextBox)
                Dim T2txtbx As TextBox = DirectCast(e.Row.FindControl("txtLo"), TextBox)
                Dim T3txtbx As TextBox = DirectCast(e.Row.FindControl("txtLi"), TextBox)
                Dim T4txtbx As TextBox = DirectCast(e.Row.FindControl("txtoutTime"), TextBox)
                Dim chkbxlunch As CheckBox = DirectCast(e.Row.FindControl("chkbxlunch"), CheckBox)
                Dim lbllunch As Label = DirectCast(e.Row.FindControl("lbllunch"), Label)
                Dim whrslbl As Label = DirectCast(e.Row.FindControl("lblwhrs"), Label)
                Dim OTlbl As Label = DirectCast(e.Row.FindControl("lblOt"), Label)
                If (empcodelbl IsNot Nothing) Then
                    T1txtbx.Text = If(T1txtbx.Text.Length > 5, T1txtbx.Text.Substring(0, 5), T1txtbx.Text)
                    T2txtbx.Text = If(T2txtbx.Text.Length > 5, T2txtbx.Text.Substring(0, 5), T2txtbx.Text)
                    T3txtbx.Text = If(T3txtbx.Text.Length > 5, T3txtbx.Text.Substring(0, 5), T3txtbx.Text)
                    T4txtbx.Text = If(T4txtbx.Text.Length > 5, T4txtbx.Text.Substring(0, 5), T4txtbx.Text)
                    If (Not String.IsNullOrEmpty(lbllunch.Text)) Then
                        chkbxlunch.Checked = If(lbllunch.Text = "True", True, False)
                    Else
                        chkbxlunch.Checked = False
                    End If
                    whrslbl.Text = If(whrslbl.Text.Length > 5, whrslbl.Text.Substring(0, 5), whrslbl.Text)
                    OTlbl.Text = If(OTlbl.Text.Length > 5, OTlbl.Text.Substring(0, 5), OTlbl.Text)
                End If


            End If

        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Protected Sub imgsrch_Click(sender As Object, e As ImageClickEventArgs) Handles imgsrch.Click

        Dim selectedDate = DateTime.Parse(txtattDt.Text)
        LoadGridByDate(selectedDate)


    End Sub

    Public Function IsDataAvailable(ByVal DateCondition As DateTime) As Boolean
        Dim str As String = "SELECT e.empid ,e.empcode, e.empname,Time1,Time2,Time3,Time4,lunchtag,whrs,othrs FROM tbl_employee e  left join tbl_attendance a on e.empid=a.empid where a.attdate= @dt"

        con.Open()
        cmd = New SqlCommand(str, con)
        cmd.Parameters.AddWithValue("@dt", DateCondition)
        rdr = cmd.ExecuteReader
        Dim val As Boolean
        If rdr.HasRows Then
            val = True
        Else
            val = False
        End If
        con.Close()
        Return val
    End Function


    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnCalculate_Click(sender, e)
        If (Not String.IsNullOrEmpty(txtattDt.Text)) Then
            Dim selectedDate = DateTime.Parse(txtattDt.Text)
            Dim atdate = selectedDate
            Dim IsAvailable = IsDataAvailable(selectedDate)

            For Each item As GridViewRow In GrdDetails.Rows


                Dim OT As TimeSpan
                Dim empid As Integer = Convert.ToInt32(GrdDetails.DataKeys(item.RowIndex).Values(0))
                Dim empcodelbl As Label = item.Cells(0).FindControl("txtEmpCode")
                Dim empNamelbl As Label = item.Cells(1).FindControl("txtEmpName")
                Dim T1txtbx As TextBox = item.Cells(2).FindControl("txtInTime")
                Dim T2txtbx As TextBox = item.Cells(3).FindControl("txtLo")
                Dim T3txtbx As TextBox = item.Cells(4).FindControl("txtLi")
                Dim T4txtbx As TextBox = item.Cells(5).FindControl("txtoutTime")
                'Dim lunchtg As Label = item.Cells(6).FindControl("lbllunch")
                Dim lunchchxbx As CheckBox = item.Cells(6).FindControl("chkbxlunch")
                Dim whrslbl As Label = item.Cells(7).FindControl("lblwhrs")
                Dim OTlbl As Label = item.Cells(8).FindControl("lblOt")

                Dim empcode = empcodelbl.Text
                Dim empName = empNamelbl.Text
                Dim lunchval As Integer = 0

                Dim T1 As TimeSpan = Nothing
                Dim T2 As TimeSpan = Nothing
                Dim T3 As TimeSpan = Nothing
                Dim T4 As TimeSpan = Nothing
                Dim whrs As TimeSpan = Nothing
                Dim othrs As TimeSpan = Nothing

                If Not String.IsNullOrEmpty(T1txtbx.Text) Then
                    T1 = TimeSpan.Parse(T1txtbx.Text)
                End If
                If Not String.IsNullOrEmpty(T2txtbx.Text) Then
                    T2 = TimeSpan.Parse(T2txtbx.Text)
                End If
                If Not String.IsNullOrEmpty(T3txtbx.Text) Then
                    T3 = TimeSpan.Parse(T3txtbx.Text)
                End If
                If Not String.IsNullOrEmpty(T4txtbx.Text) Then
                    T4 = TimeSpan.Parse(T4txtbx.Text)
                End If
                If Not String.IsNullOrEmpty(whrslbl.Text) Then
                    whrs = TimeSpan.Parse(whrslbl.Text)
                End If
                If Not String.IsNullOrEmpty(OTlbl.Text) Then
                    othrs = TimeSpan.Parse(OTlbl.Text)
                End If

                If (lunchchxbx.Checked = True) Then
                    lunchval = 1
                End If
                Dim str As String
                If (Not IsAvailable) Then
                    str = "insert into tbl_attendance (attdate,empId,Time1,Time2,Time3,Time4,whrs,othrs,IsLunch) values(@DtTime," & empid & ",@T1,@T2,@T3,@T4,@whrs,@othr," & lunchval & ")"
                Else
                    str = "update  tbl_attendance set Time1=@T1,Time2=@T2,Time3=@T3,Time4=@T4,whrs=@whrs,othrs=@othr,IsLunch=" & lunchval & " where empId=@empId"
                End If

                con.Open()
                cmd = New SqlCommand(str, con)
                If (IsAvailable) Then
                    cmd.Parameters.AddWithValue("@empId", empid)
                End If
                cmd.Parameters.AddWithValue("@DtTime", atdate)
                cmd.Parameters.AddWithValue("@T1", T1)
                cmd.Parameters.AddWithValue("@T2", T2)
                cmd.Parameters.AddWithValue("@T3", T3)
                cmd.Parameters.AddWithValue("@T4", T4)
                cmd.Parameters.AddWithValue("@whrs", whrs)
                cmd.Parameters.AddWithValue("@othr", othrs)
                cmd.ExecuteScalar()
                con.Close()
            Next
            LoadGridByDate(atdate)
        End If

    End Sub


    Protected Sub btnCalculate_Click(sender As Object, e As EventArgs) Handles btnCalculate.Click
        LoadWorkHours()

        Dim selectedDate = DateTime.Parse(txtattDt.Text)

        con.Open()
        Dim atdate = selectedDate
        For Each item As GridViewRow In GrdDetails.Rows


            Dim OT As TimeSpan

            Dim empcodelbl As Label = item.Cells(0).FindControl("txtEmpCode")
            Dim empNamelbl As Label = item.Cells(1).FindControl("txtEmpName")
            Dim T1txtbx As TextBox = item.Cells(2).FindControl("txtInTime")
            Dim T2txtbx As TextBox = item.Cells(3).FindControl("txtLo")
            Dim T3txtbx As TextBox = item.Cells(4).FindControl("txtLi")
            Dim T4txtbx As TextBox = item.Cells(5).FindControl("txtoutTime")
            Dim whrslbl As Label = item.Cells(6).FindControl("lblwhrs")
            Dim OTlbl As Label = item.Cells(7).FindControl("lblOt")
            Dim chkbxlun As CheckBox = item.Cells(6).FindControl("chkbxlunch")

            Dim empcode = empcodelbl.Text
            If (chkbxlun.Checked) Then
                Dim tim1 = TimeSpan.Parse(T1txtbx.Text)
                Dim tim4 = TimeSpan.Parse(T4txtbx.Text)
                If (tim4 > tim1) Then
                    Dim wrkHrs = (tim4 - tim1)
                    Dim whrstxt = wrkHrs.Hours.ToString + ":" + wrkHrs.Minutes.ToString()
                    whrslbl.Text = whrstxt
                    Dim empid As Integer = Convert.ToInt32(GrdDetails.DataKeys(item.RowIndex).Values(0))
                    If (Not empDict Is Nothing) Then
                        If empDict.ContainsKey(empid) Then
                            If empDict(empid) > 0 Then

                                Dim arr = empDict(empid).ToString().Split(".")
                                Dim whdb = arr(0) + ":" + arr(1)
                                Dim whidl = TimeSpan.Parse(whdb)
                                If (wrkHrs > whidl) Then
                                    OT = wrkHrs - whidl
                                    Dim OThrstxt = OT.Hours.ToString + ":" + OT.Minutes.ToString()
                                    OTlbl.Text = OThrstxt
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                If (Not String.IsNullOrEmpty(T1txtbx.Text) And Not String.IsNullOrEmpty(T2txtbx.Text) And Not String.IsNullOrEmpty(T3txtbx.Text) And Not String.IsNullOrEmpty(T4txtbx.Text)) Then

                    Dim time1 = TimeSpan.Parse(T1txtbx.Text)
                    Dim time2 = TimeSpan.Parse(T2txtbx.Text)
                    Dim time3 = TimeSpan.Parse(T3txtbx.Text)
                    Dim time4 = TimeSpan.Parse(T4txtbx.Text)
                    If (time2 > time1 And time3 > time2 And time4 > time3) Then
                        Dim wrkHrs = (time2 - time1) + (time4 - time3)
                        Dim whrstxt = wrkHrs.Hours.ToString + ":" + wrkHrs.Minutes.ToString()
                        whrslbl.Text = whrstxt
                        Dim empid As Integer = Convert.ToInt32(GrdDetails.DataKeys(item.RowIndex).Values(0))
                        If (Not empDict Is Nothing) Then
                            If empDict.ContainsKey(empid) Then
                                If empDict(empid) > 0 Then

                                    Dim arr = empDict(empid).ToString().Split(".")
                                    Dim whdb = arr(0) + ":" + arr(1)
                                    Dim whidl = TimeSpan.Parse(whdb)
                                    If (wrkHrs > whidl) Then
                                        OT = wrkHrs - whidl
                                        Dim OThrstxt = OT.Hours.ToString + ":" + OT.Minutes.ToString()
                                        OTlbl.Text = OThrstxt
                                    Else
                                        OTlbl.Text = "00:00"
                                    End If
                                End If
                            End If
                        End If
                    Else
                        whrslbl.Text = "00:00"
                        OTlbl.Text = "00:00"
                    End If
                End If
            End If


            'Dim str As String = "insert into tbl_attendance attdate,empId,Time1,Time2,Time3,Time4,IsLunch,whrs,othrs values(" & selectedDate & ",'" & 
        Next


        con.Close()
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub

    'Private Sub calculate()
    '    If (Not String.IsNullOrEmpty(T1txtbx.Text) And Not String.IsNullOrEmpty(T2txtbx.Text) And Not String.IsNullOrEmpty(T3txtbx.Text) And Not String.IsNullOrEmpty(T4txtbx.Text)) Then

    '        Dim time1 = TimeSpan.Parse(T1txtbx.Text)
    '        Dim time2 = TimeSpan.Parse(T2txtbx.Text)
    '        Dim time3 = TimeSpan.Parse(T3txtbx.Text)
    '        Dim time4 = TimeSpan.Parse(T4txtbx.Text)
    '        If (time4 > time1) Then
    '            Dim wrkHrs = (time2 - time1) + (time4 - time3)
    '            Dim whrstxt = wrkHrs.Hours.ToString + ":" + wrkHrs.Minutes.ToString()
    '            whrslbl.Text = whrstxt
    '            Dim empid As Integer = Convert.ToInt32(GrdDetails.DataKeys(item.RowIndex).Values(0))
    '            If (Not empDict Is Nothing) Then
    '                If empDict.ContainsKey(empid) Then
    '                    If empDict(empid) > 0 Then

    '                        Dim arr = empDict(empid).ToString().Split(".")
    '                        Dim whdb = arr(0) + ":" + arr(1)
    '                        Dim whidl = TimeSpan.Parse(whdb)
    '                        If (wrkHrs > whidl) Then
    '                            OT = wrkHrs - whidl
    '                            Dim OThrstxt = OT.Hours.ToString + ":" + OT.Minutes.ToString()
    '                            OTlbl.Text = OThrstxt
    '                        Else
    '                            OTlbl.Text = "00:00"
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Else
    '            whrslbl.Text = "00:00"
    '            OTlbl.Text = "00:00"
    '        End If

    '    ElseIf (Not String.IsNullOrEmpty(T1txtbx.Text) And String.IsNullOrEmpty(T2txtbx.Text) And String.IsNullOrEmpty(T3txtbx.Text) And Not String.IsNullOrEmpty(T4txtbx.Text))
    '        Dim tim1 = TimeSpan.Parse(T1txtbx.Text)
    '        Dim tim4 = TimeSpan.Parse(T4txtbx.Text)
    '        If (tim4 > tim1) Then
    '            Dim wrkHrs = (tim4 - tim1)
    '            Dim whrstxt = wrkHrs.Hours.ToString + ":" + wrkHrs.Minutes.ToString()
    '            whrslbl.Text = whrstxt
    '            Dim empid As Integer = Convert.ToInt32(GrdDetails.DataKeys(item.RowIndex).Values(0))
    '            If (Not empDict Is Nothing) Then
    '                If empDict.ContainsKey(empid) Then
    '                    If empDict(empid) > 0 Then

    '                        Dim arr = empDict(empid).ToString().Split(".")
    '                        Dim whdb = arr(0) + ":" + arr(1)
    '                        Dim whidl = TimeSpan.Parse(whdb)
    '                        If (wrkHrs > whidl) Then
    '                            OT = wrkHrs - whidl
    '                            Dim OThrstxt = OT.Hours.ToString + ":" + OT.Minutes.ToString()
    '                            OTlbl.Text = OThrstxt
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If

    '    End If

    'End Sub


End Class
