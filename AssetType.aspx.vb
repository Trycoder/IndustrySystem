Partial Class AssetType
    Inherits System.Web.UI.Page
    Dim objDB As New DBFunctions
    Dim ds As New DataSet
    Dim sql As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            sql = "SELECT CatCode,CatID FROM tbl_Asset_CategoryMaster WHERE GroupID = 1"
            ds = objDB.FillDataset(sql)
            For i = 0 To ds.Tables(0).Rows.Count - 1
                cboCategoryName.Items.Add(New ListItem(ds.Tables(0).Rows(i)(0), ds.Tables(0).Rows(i)(1)))
            Next
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        sql = "SELECT * FROM tbl_Asset_TypeMaster WHERE AssetTypeCode = '" & Trim(txtAssetCode.Text) & "'"
        ds = objDB.FillDataset(sql)
        If ds.Tables(0).Rows.Count > 0 Then
            'Update
            sql = "UPDATE tbl_Asset_TypeMaster SET AssetTypeCode = '" & Trim(txtAssetCode.Text) & "',AssetTypeDesc = '" & Trim(txtAssetDesc.Text) & "' " & _
                    "WHERE AssetTypeID = " & ds.Tables(0).Rows(0)(0) & ""
            objDB.FillDataset(sql)
        Else
            sql = "INSERT INTO tbl_Asset_TypeMaster(AssetTypeCode,AssetTypeDesc,CatID) " & _
                    "VALUES('" & txtAssetCode.Text & "','" & txtAssetDesc.Text & "','" & cboCategoryName.SelectedValue & "')"
            objDB.FillDataset(sql)
        End If
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("AssetType.aspx")
    End Sub
    Protected Sub cboCategoryName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCategoryName.SelectedIndexChanged
        txtAssetCode.Text = ""
        txtAssetDesc.Text = ""
    End Sub
End Class
