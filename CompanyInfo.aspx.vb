Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Partial Class CompanyInfo
    Inherits System.Web.UI.Page
    Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim rowid As String
    Dim dtable As DataTable
    Shared sortExpression As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        '   If Not IsPostBack Then
        Try
            con.Open()
            cmd = New SqlCommand("select * from tbl_companyinfo", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    TxtCompanyCode.Text = rdr("compcode")
                    TxtCompanyName.Text = rdr("compname")
                    TxtDisplayName.Text = rdr("dispname")
                    TxtAddress1.Text = rdr("address1")
                    TxtAddress2.Text = rdr("address2")
                    TxtAddress3.Text = rdr("address3")
                    TxtAddress4.Text = rdr("address4")
                    TxtPhone1.Text = rdr("phone1")
                    TxtPhone2.Text = rdr("phone2")
                    Txtfax.Text = rdr("fax")
                    TxtEmail.Text = rdr("email")
                    TxtWebSite.Text = rdr("website")
                    TxtGSTNo.Text = rdr("gstno")
                    TxtTaxNo1.Text = rdr("taxno1")
                    TxtTaxNo2.Text = rdr("taxno2")

                    Dim img = rdr("Img")

                    Dim imgtype = rdr("ImgType")
                    If (Not IsDBNull(img)) Then
                        Image1.ImageUrl = "data:" + imgtype + ";base64," + Convert.ToBase64String(img, 0, img.Length)
                    End If


                End If
            End If
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
        BtnLogo.Attributes.Add("onclick", "document.getElementById('" + ImgUpload.ClientID + "').click();return false")

        '  End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Sql As String
        Dim str As String
       

                  


        con.Open()
        str = " update tbl_companyinfo set CompCode = '" & TxtCompanyCode.Text & "',"
        str = str & "CompName = '" & TxtCompanyName.Text & "',"
        str = str & "DispName = '" & TxtDisplayName.Text & "',"
        str = str & "Address1 = '" & TxtAddress1.Text & "',"
        str = str & "Address2 = '" & TxtAddress2.Text & "',"
        str = str & "Address3 = '" & TxtAddress3.Text & "',"
        str = str & "Address4 = '" & TxtAddress4.Text & "',"
        str = str & "phone1 = '" & TxtPhone1.Text & "',"
        str = str & "phone2 = '" & TxtPhone2.Text & "',"
        str = str & "fax = '" & Txtfax.Text & "',"
        str = str & "email = '" & TxtEmail.Text & "',"
        str = str & "website = '" & TxtWebSite.Text & "',"
        str = str & "GSTNo = '" & TxtGSTNo.Text & "',"
        str = str & "TaxNo1 = '" & TxtTaxNo1.Text & "',"
        str = str & "TaxNo2 = '" & TxtTaxNo2.Text & "'"
      
     
        cmd = New SqlCommand(str, con)
    

        cmd.ExecuteScalar()

        con.Close()
    End Sub
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

    Protected Sub BtnPreview_Click(sender As Object, e As ImageClickEventArgs) Handles BtnPreview.Click
        Dim imgtype As String = ""

        Dim str As String
        If (ImgUpload.HasFile) Then
            Dim length = ImgUpload.PostedFile.ContentLength
            Dim img(length) As Byte
            ImgUpload.PostedFile.InputStream.Read(img, 0, length)
            imgtype = ImgUpload.PostedFile.ContentType
            con.Open()
            str = " update tbl_companyinfo set Img = @Img,ImgType='" & ImgUpload.PostedFile.ContentType & "'"
            cmd = New SqlCommand(str, con)
            Dim sqlparam As SqlParameter = cmd.Parameters.AddWithValue("@Img", img)
            sqlparam.SqlDbType = SqlDbType.VarBinary
            cmd.ExecuteScalar()
            con.Close()
            con.Open()
            cmd = New SqlCommand("select * from tbl_companyinfo", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    Dim imgdb = rdr("Img")

                    Dim imgtyp = rdr("ImgType")
                    If (Not IsDBNull(imgdb)) Then
                        '    Image1.ImageUrl = "~/Files/Logo123.png"
                        'Else
                        Image1.ImageUrl = "data:" + imgtyp + ";base64," + Convert.ToBase64String(imgdb, 0, imgdb.Length)
                    End If


                End If
            End If
        End If
        con.Close()
        'Dim path = Server.MapPath("images/") + ImgUpload.FileName

        'ImgUpload.SaveAs(path)

        'Image1.ImageUrl = "images/" + ImgUpload.FileName

    End Sub
End Class
