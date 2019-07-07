
Partial Class TransactionDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack = True Then
            Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
            If imgbtn IsNot Nothing Then
                imgbtn.Focus()
            End If
        End If
    End Sub
End Class
