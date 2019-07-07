Partial Class Category
    Inherits System.Web.UI.Page
    Dim objDB As New DBFunctions
    Dim ds As New DataSet
    Dim sql As String
    Public Sub FillData()
        sql = "SELECT ROW_NUMBER() OVER (ORDER BY CatID ASC),CatCode,CatDesc FROM tbl_Asset_CategoryMaster WHERE GroupID = 1 ORDER BY CatCode"
        ds = objDB.FillDataset(sql)
        Dim altcolor As Integer = 1
        For i = 0 To ds.Tables(0).Rows.Count - 1
            If altcolor = 1 Then
                altcolor = 2
                Response.Write("<tr class='whitebg' onclick='ChangeColor(this);'>")
            Else
                altcolor = 1
                Response.Write("<tr class='whitebg' onclick='ChangeColor(this);' style='background-color:#E6E8FA;'>")
            End If
            Response.Write("<td align='center' class='tdtext'>")
            Response.Write(ds.Tables(0).Rows(i)(0))
            Response.Write("</td>")
            Response.Write("<td align='left' class='tdtext'>")
            Response.Write(ds.Tables(0).Rows(i)(1))
            Response.Write("</td>")
            Response.Write("<td align='left' class='tdtext'>")
            Response.Write(ds.Tables(0).Rows(i)(2))
            Response.Write("</td>")
            Response.Write("</tr>")
        Next
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strCatDesc As String = ""
        sql = "SELECT * FROM tbl_Asset_CategoryMaster WHERE GroupID = 1 AND CatCode = '" & Trim(txtCategoryCode.Text) & "'"
        ds = objDB.FillDataset(sql)

        If ds.Tables(0).Rows.Count > 0 Then
            sql = "UPDATE tbl_Asset_CategoryMaster SET CatDesc = '" & Trim(txtCategoryDesc.Text) & "' WHERE CatCode = '" & Trim(txtCategoryCode.Text) & "'"
            objDB.FillDataset(sql)
        Else
            sql = "INSERT INTO tbl_Asset_CategoryMaster (CatCode,CatDesc,GroupID) " & _
                    "VALUES('" & Trim(txtCategoryCode.Text) & "','" & Trim(txtCategoryDesc.Text) & "',1)"
            objDB.FillDataset(sql)
        End If
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Category.aspx")
    End Sub
End Class
