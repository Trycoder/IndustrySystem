Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic

Partial Class Employee
    Inherits System.Web.UI.Page
    Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim rowid As String
    Dim dtable As DataTable
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BtnLogo.Attributes.Add("onclick", "document.getElementById('" + ImgUpload.ClientID + "').click();return false")
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        rowid = Request.QueryString("Id")
        '  rowid = "1"
        If Not IsPostBack Then
            FillCombos("", "")
            If Not String.IsNullOrEmpty(rowid) Then
                FnDisplay(rowid)
            End If
            ViewState("sortOrder") = ""
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If

    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If UCase(btnSave.Text) = UCase("Update") Then
            If Not String.IsNullOrEmpty(rowid) Then
                Dim str As String
                con.Open()
                str = "update tbl_employee set empcode ='" & Trim(TxtEmpCode.Text.Replace("'", "''")) & "',empname ='" & Trim(TxtEmpName.Text.Replace("'", "''")) & "',"
                str = str & "empinitial = '" & Trim(TxtEmpInitial.Text.Replace("'", "''")) & "',empfathername = '" & Trim(TxtFatherName.Text.Replace("'", "''")) & "',"
                str = str & "empaddress1 = '" & Trim(TxtAddress1.Text.Replace("'", "''")) & "',empaddress2 = '" & Trim(TxtAddress2.Text.Replace("'", "''")) & "',"
                str = str & "empaddress3 = '" & Trim(TxtAddress3.Text.Replace("'", "''")) & "',empaddress4 = '" & Trim(TxtAddress4.Text.Replace("'", "''")) & "',"
                str = str & "empaddress1P = '" & Trim(TxtAddress1P.Text.Replace("'", "''")) & "',empaddress2P = '" & Trim(TxtAddress2P.Text.Replace("'", "''")) & "',"
                str = str & "empaddress3P = '" & Trim(TxtAddress3P.Text.Replace("'", "''")) & "',empaddress4P = '" & Trim(TxtAddress4P.Text.Replace("'", "''")) & "',"
                str = str & "empcity = '" & Trim(TxtCity.Text.Replace("'", "''")) & "',empstate = '" & Trim(TxtState.Text.Replace("'", "''")) & "',emppincode = '" & Trim(TxtPinCode.Text.Replace("'", "''")) & "',"
                str = str & "empmobile = '" & Trim(TxtMobileNo.Text.Replace("'", "''")) & "',empemergcont = '" & Trim(TxtEmCo.Text.Replace("'", "''")) & "',"
                str = str & "empemergno = '" & Trim(TxtEmNo.Text.Replace("'", "''")) & "',empaadhar = '" & Trim(TxtAadharNo.Text.Replace("'", "''")) & "',"
                str = str & "emppan = '" & Trim(TxtPanNo.Text.Replace("'", "''")) & "',empdl = '" & Trim(TxtLicenseNo.Text.Replace("'", "''")) & "',"
                str = str & "deptid = " & Val(CboDept.SelectedValue) & ",desigid = " & Val(CboDesig.SelectedValue) & ",empcatid = " & Val(CboEmpCat.SelectedValue) & ","
                str = str & "basicsalary  = " & Val(TxtBasicSalary.Text) & ",email ='" & Trim(TxtEmail.Text.Replace("'", "''")) & "'"
                If ChkLunch.Checked = True Then
                    str = str & ",lunchtag = 1"
                Else
                    str = str & ",lunchtag = 1"
                End If
                If chkinactive.Checked = True Then
                    str = str & ",empstatus = 0"
                Else
                    str = str & ",empstatus= 1"
                End If
                str = str & ",empdob = '" & TxtDOB.Text & "'"
                If TxtJDate.Text <> "" And IsDate(TxtJDate.Text) = True Then
                    str = str & ",doj = '" & TxtJDate.Text & "'"
                Else
                    str = str & ",doj = NULL"
                End If
                If TxtCDate.Text <> "" And IsDate(TxtCDate.Text) = True Then
                    str = str & ",doc = '" & TxtCDate.Text & "'"
                Else
                    str = str & ",doc = NULL,"
                End If
                If chkinactive.Checked = True And TxtRDate.Text <> "" And IsDate(TxtRDate.Text) = True Then
                    str = str & ",dor = '" & TxtRDate.Text & "'"
                Else
                    str = str & ",dor = NULL"
                End If
                str = str & " where empid = " & rowid
                cmd = New SqlCommand(str, con)
                cmd.ExecuteScalar()
                con.Close()
                FnClear(0)
                FillCombos("", "")
            End If
            Else
                Dim str As String
                cmd = New SqlCommand
                con.Open()
            str = "insert into tbl_employee(empcode,empname,empinitial,empfathername,empdob,empaddress1,empaddress2,empaddress3,empaddress4,empaddress1P,empaddress2P,empaddress3P,empaddress4P,"
            str = str & "empcity,empstate,emppincode,empmobile,empemergcont,empemergno,empaadhar,emppan,empdl,deptid,desigid,empcatid,basicsalary,email,lunchtag,empstatus,doj,doc,dor) values('"
            str = str & TxtEmpCode.Text.Replace("'", "''") & "','" & TxtEmpName.Text.Replace("'", "''") & "','" & TxtEmpInitial.Text.Replace("'", "''") & "','"
            str = str & TxtFatherName.Text.Replace("'", "''") & "','" & TxtDOB.Text & "','"
            str = str & TxtAddress1.Text.Replace("'", "''") & "','" & TxtAddress2.Text.Replace("'", "''") & "','" & TxtAddress3.Text.Replace("'", "''") & "','" & TxtAddress4.Text.Replace("'", "''") & "','"
            str = str & TxtAddress1P.Text.Replace("'", "''") & "','" & TxtAddress2P.Text.Replace("'", "''") & "','" & TxtAddress3P.Text.Replace("'", "''") & "','" & TxtAddress4P.Text.Replace("'", "''") & "','"
            str = str & TxtCity.Text.Replace("'", "''") & "','" & TxtState.Text.Replace("'", "''") & "','" & TxtPinCode.Text.Replace("'", "''") & "','" & TxtMobileNo.Text & "','" & TxtEmCo.Text.Replace("'", "''") & "','"
            str = str & TxtEmNo.Text.Replace("'", "''") & "','" & TxtAadharNo.Text.Replace("'", "''") & "','" & TxtPanNo.Text.Replace("'", "''") & "','" & TxtLicenseNo.Text.Replace("'", "''") & "',"
            str = str & Val(CboDept.SelectedValue) & "," & Val(CboDesig.SelectedValue) & "," & Val(CboEmpCat.SelectedValue) & ","
            str = str & Val(TxtBasicSalary.Text) & ",'" & TxtEmail.Text.Replace("'", "''") & "',"
            If ChkLunch.Checked = True Then
                str = str & "lunchtg = 1,"
            Else
                str = str & "lunchtg = 0,"
            End If
            If chkinactive.Checked = True Then
                str = str & "lempstatus = 0,"
            Else
                str = str & "empstatus= 1,"
            End If
            If TxtJDate.Text <> "" Then
                str = str & "'" & TxtJDate.Text & "',"
            Else
                str = str & "null,"
            End If

                If TxtCDate.Text <> "" Then
                    str = str & "'" & TxtCDate.Text & "',"
                Else
                    str = str & "null,"
                End If
                If TxtRDate.Text <> "" Then
                    str = str & "'" & TxtRDate.Text & "')"
                Else
                    str = str & "null)"
                End If
                cmd.CommandText = str
                cmd.Connection = con
                cmd.ExecuteScalar()
                con.Close()
                FnClear(0)
                FillCombos("", "")
            End If
    End Sub

    Private Function FnClear(ByVal id As String)
        TxtEmpCode.Text = ""
        TxtEmpName.Text = ""
        TxtEmpInitial.Text = ""
        TxtFatherName.Text = ""
        TxtDOB.Text = ""
        TxtAddress1.Text = ""
        TxtAddress2.Text = ""
        TxtAddress3.Text = ""
        TxtAddress4.Text = ""
        TxtAddress1P.Text = ""
        TxtAddress2P.Text = ""
        TxtAddress3P.Text = ""
        TxtAddress4P.Text = ""
        TxtCity.Text = ""
        TxtState.Text = ""
        TxtPinCode.Text = ""
        TxtMobileNo.Text = ""
        TxtEmCo.Text = ""
        TxtEmNo.Text = ""
        TxtAadharNo.Text = ""
        TxtPanNo.Text = ""
        TxtLicenseNo.Text = ""
        TxtCDate.Text = ""
        TxtJDate.Text = ""
        TxtRDate.Text = ""
        TxtEmail.Text = ""
        TxtBasicSalary.Text = ""
        chkinactive.Checked = False
        ChkLunch.Checked = False
        btnSave.Text = "Save"
    End Function
    Private Function FnDisplay(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select e.*,isnull(e.empdob,'') as bdate,isnull(e.doj,'') as jdate,isnull(e.doc,'') as cdate,isnull(e.dor,'') as rdate,d.deptcode,de.desigcode,c.empcatcode from tbl_employee e,tbl_employeecategory c,tbl_department d,tbl_designation de where e.deptid = d.deptid and e.desigid = de.desigid and e.empcatid = c.empcatid and  e.empid =" & id & " order by e.empcode", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    TxtEmpCode.Text = rdr("empcode")
                    TxtEmpName.Text = rdr("empname")
                    TxtEmpInitial.Text = rdr("empinitial")
                    TxtFatherName.Text = rdr("empfathername")
                    TxtAddress1.Text = rdr("empaddress1")
                    TxtAddress2.Text = rdr("empaddress2")
                    TxtAddress3.Text = rdr("empaddress3")
                    TxtAddress4.Text = rdr("empaddress4")
                    TxtAddress1P.Text = rdr("empaddress1P")
                    TxtAddress2P.Text = rdr("empaddress2P")
                    TxtAddress3P.Text = rdr("empaddress3P")
                    TxtAddress4P.Text = rdr("empaddress4P")
                    TxtCity.Text = rdr("empcity")
                    TxtState.Text = rdr("empstate")
                    TxtPinCode.Text = rdr("emppincode")
                    TxtMobileNo.Text = rdr("empmobile")
                    TxtEmCo.Text = rdr("empemergcont")
                    TxtEmNo.Text = rdr("empemergno")
                    TxtAadharNo.Text = rdr("empaadhar")
                    TxtPanNo.Text = rdr("emppan")
                    TxtLicenseNo.Text = rdr("empdl")
                    TxtEmail.Text = rdr("email")
                    TxtBasicSalary.Text = rdr("basicsalary")
                    If rdr("empstatus") = 0 Then chkinactive.Checked = True
                    If rdr("lunchtag") = 1 Then ChkLunch.Checked = True
                    CboDept.SelectedValue = rdr("deptid")
                    CboDesig.SelectedValue = rdr("desigid")
                    CboEmpCat.SelectedValue = rdr("empcatid")
                    TxtDOB.Text = rdr("bdate")
                    TxtJDate.Text = rdr("jdate")
                    TxtCDate.Text = rdr("cdate")
                    TxtRDate.Text = rdr("rdate")
                    If rdr("rdate") = "1/1/1900" Then TxtRDate.Text = ""
                    'ImgEmp.ImageUrl = "~/Files/6759.JPG"
                End If
            End If
            con.Close()
            btnSave.Text = "Update"
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
    'Protected Sub UploadFile(sender As Object, e As EventArgs)
    '    Dim folderPath As String = Server.MapPath("~/Files/")

    '    'Check whether Directory (Folder) exists.
    '    If Not Directory.Exists(folderPath) Then
    '        'If Directory (Folder) does not exists Create it.
    '        Directory.CreateDirectory(folderPath)
    '    End If

    '    'Save the File to the Directory (Folder).
    '    FileUpload1.SaveAs(folderPath & "logo123.png")

    '    'Display the Picture in Image control.
    '    Image1.ImageUrl = "~/Files/logo123.png"
    'End Sub

    'Protected Sub BtnLogo_Click(sender As Object, e As ImageClickEventArgs) Handles BtnLogo.Click
    '    Dim folderPath As String = Server.MapPath("~/Files/")

    '    'Check whether Directory (Folder) exists.
    '    If Not Directory.Exists(folderPath) Then
    '        'If Directory (Folder) does not exists Create it.
    '        Directory.CreateDirectory(folderPath)
    '    End If

    '    'Save the File to the Directory (Folder).
    '    ' FileUpload1.SaveAs(folderPath & "logo123.png")

    '    'Display the Picture in Image control.
    '    Image1.ImageUrl = "~/Files/logo123.png"
    'End Sub

    Sub FillCombos(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String
            sql = "select * from tbl_department order by deptcode"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            CboDept.Items.Clear()
            CboDept.Items.Add(New ListItem("--Select--", ""))
            If rdr.HasRows Then
                While rdr.Read
                    CboDept.Items.Add(New ListItem(rdr("deptcode"), rdr("deptid")))
                End While
            End If
            con.Close()

            sql = "select * from tbl_designation order by desigcode"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            CboDesig.Items.Clear()
            CboDesig.Items.Add(New ListItem("--Select--", ""))
            If rdr.HasRows Then
                While rdr.Read
                    CboDesig.Items.Add(New ListItem(rdr("desigcode"), rdr("desigid")))
                End While
            End If
            con.Close()

            sql = "select * from tbl_EmployeeCategory order by empcatcode"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            CboEmpCat.Items.Clear()
            CboEmpCat.Items.Add(New ListItem("--Select--", ""))
            If rdr.HasRows Then
                While rdr.Read
                    CboEmpCat.Items.Add(New ListItem(rdr("empcatcode"), rdr("empcatid")))
                End While
            End If
            con.Close()



        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub imgsrch_Click(sender As Object, e As ImageClickEventArgs) Handles imgsrch.Click
        Dim query As String
        If Not String.IsNullOrEmpty(TxtEmpCode.Text) Then
            query = "Select e.empid,e.empcode,e.empname,d.deptname,de.designame from tbl_employee e,tbl_department d,tbl_designation de where e.deptid = d.deptid and e.desigid = de.desigid and e.empcode like '%" & TxtEmpCode.Text.Trim & "%' order by empcode asc"
        Else
            query = "Select e.empid,e.empcode,e.empname,d.deptname,de.designame from tbl_employee e,tbl_department d,tbl_designation de where e.deptid = d.deptid and e.desigid = de.desigid order by empcode asc"
        End If
        ucModal.LaunchPopup(query, "Employee")
        ucModal.Visible = True
    End Sub


    Protected Sub TxtEmpCode_TextChanged(sender As Object, e As EventArgs) Handles TxtEmpCode.TextChanged
        Dim sql As String
        sql = "select * from tbl_employee where empcode = '" & TxtEmpCode.Text & "'"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand(sql, con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            TxtEmpName.Text = rdr("empname")
        End If
        con.Close()
    End Sub

    Protected Sub BtnPreview_Click(sender As Object, e As ImageClickEventArgs) Handles BtnPreview.Click
        Dim imgtype As String = ""

        Dim str As String
        If (ImgUpload.HasFile) Then
            Dim length = ImgUpload.PostedFile.ContentLength
            Dim img(length) As Byte
            ImgUpload.PostedFile.InputStream.Read(img, 0, length)
            imgtype = ImgUpload.PostedFile.ContentType
            If (Not String.IsNullOrEmpty(TxtEmpCode.Text)) Then
                con.Open()
                str = " insert into temp_emp_pic (empcode,img,imgtype) values('" & TxtEmpCode.Text & "',@Img,'" & ImgUpload.PostedFile.ContentType & "')"
                cmd = New SqlCommand(str, con)
                Dim sqlparam As SqlParameter = cmd.Parameters.AddWithValue("@Img", img)
                sqlparam.SqlDbType = SqlDbType.VarBinary
                cmd.ExecuteScalar()
                con.Close()
                con.Open()
                cmd = New SqlCommand("select * from temp_emp_pic where empcode='" & TxtEmpCode.Text & "'", con)
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    If rdr.Read Then
                        Dim imgdb = rdr("Img")

                        Dim imgtyp = rdr("ImgType")
                        If (Not IsDBNull(imgdb)) Then
                            '    Image1.ImageUrl = "~/Files/Logo123.png"
                            'Else
                            ImgEmp.ImageUrl = "data:" + imgtyp + ";base64," + Convert.ToBase64String(imgdb, 0, imgdb.Length)
                        End If


                    End If
                End If
            End If
          
        End If
        con.Close()
    End Sub
End Class
