Imports System.Data.SqlClient
Imports System.Data

Partial Class Reports
    Inherits System.Web.UI.Page
    Dim sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim sqlcmd As New SqlCommand
    Dim drAsset As SqlDataReader
    Dim sqlad As New SqlDataAdapter
    Dim dt As DataTable
    Dim sql As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            cboAge.SelectedIndex = 0
            cboAsset.Focus()
            fillAssetcbo()
        End If
    End Sub
    Sub fillAssetcbo()
        cboAsset.Items.Clear()
        cboAssetCount.Items.Clear()
        sql = "SELECT * FROM tbl_Asset_TypeMaster"
        sqlcon.Open()
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandType = Data.CommandType.Text
        sqlcmd.CommandText = sql
        drAsset = sqlcmd.ExecuteReader
        cboAsset.Items.Add(New ListItem("--Select--", ""))
        cboAssetCount.Items.Add(New ListItem("--Select--", ""))
        While drAsset.Read
            cboAsset.Items.Add(New ListItem(drAsset("AssetTypeDesc"), drAsset("AssetTypeID")))
            cboAssetCount.Items.Add(New ListItem(drAsset("AssetTypeDesc"), drAsset("AssetTypeID")))
        End While
        drAsset.Close()
        sqlcon.Close()
    End Sub
    Protected Sub imgasset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgasset.Click
        If cboAsset.SelectedValue = "" Then
            Dim myscript1 As String = "alert('Please select Asset type! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            cboAsset.Focus()
            Exit Sub
        End If
        If txtAge.Text.Trim = "" Then
            Dim myscript1 As String = "alert('Please enter Valid Integer! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            txtAge.Focus()
            Exit Sub
        End If
        Response.Redirect("AgeAnalysisReport.aspx?RType=Age&AType=" & cboAsset.SelectedValue & "&RValue=" & txtAge.Text.Trim & "&RMode=" & cboAge.SelectedValue & "")
    End Sub

    Protected Sub imageemployee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imageemployee.Click
        If txtCount.Text.Trim = "" Then
            Dim myscript1 As String = "alert('Please enter Valid Integer! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            txtCount.Focus()
            Exit Sub
        End If
        If cboAssetCount.SelectedValue = "" Then
            Dim myscript1 As String = "alert('Please select Asset type! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            cboAssetCount.Focus()
            Exit Sub
        End If
        Session("Rpt") = Request.QueryString("Rpt")
        Response.Redirect("AgeAnalysisReport.aspx?RType=MAssets&AType=" & cboAssetCount.SelectedValue & "&RValue=" & txtCount.Text.Trim & "")
    End Sub

    Protected Sub imgxlsasset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgxlsasset.Click
        If cboAsset.SelectedValue = "" Then
            Dim myscript1 As String = "alert('Please select Asset type! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            cboAsset.Focus()
            Exit Sub
        End If
        If txtAge.Text.Trim = "" Then
            Dim myscript1 As String = "alert('Please enter Valid Integer! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            txtAge.Focus()
            Exit Sub
        End If
        Session("Rpt") = Request.QueryString("Rpt")
        Response.Redirect("AgeAnalysisReportInExcel.aspx?rpttype=xls&report=stdreport&RType=Age&AType=" & cboAsset.SelectedValue & "&RValue=" & txtAge.Text.Trim & "&RMode=" & cboAge.SelectedValue & "")
    End Sub

    Protected Sub imgxlsemployee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgxlsemployee.Click
        If txtCount.Text.Trim = "" Then
            Dim myscript1 As String = "alert('Please enter Valid Integer! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            txtCount.Focus()
            Exit Sub
        End If
        If cboAssetCount.SelectedValue = "" Then
            Dim myscript1 As String = "alert('Please select Asset type! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
            cboAssetCount.Focus()
            Exit Sub
        End If
        Session("Rpt") = Request.QueryString("Rpt")
        Response.Redirect("AgeAnalysisReportInExcel.aspx?rpttype=xls&report=employee&RType=MAssets&AType=" & cboAssetCount.SelectedValue & "&RValue=" & txtCount.Text.Trim & "")
    End Sub

End Class
