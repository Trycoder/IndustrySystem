Partial Class Location
    Inherits System.Web.UI.Page
    Dim objDB As New DBFunctions
    Dim ds As New DataSet
    Dim sql As String
    Protected Sub cboLocation1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLocation1.SelectedIndexChanged
        sql = "SELECT * FROM tbl_Asset_location WHERE LoccatID = '" & cboLocation1.SelectedValue & "' ORDER BY LocName"
        ds = objDB.FillDataset(sql)
        For i = 0 To ds.Tables(0).Rows.Count - 1
            cboLocation2.Items.Add(New ListItem(ds.Tables(0).Rows(i)(1), ds.Tables(0).Rows(i)(0)))
        Next
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

    End Sub
End Class
