Imports System.Data.SqlClient
Imports System.Data


Partial Class ReportinExcel
    Inherits System.Web.UI.Page
    Public sqlcon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Public sqlcmd As New SqlCommand
    Public drAsset As SqlDataReader
    Public sqlad As New SqlDataAdapter
    Public RType As String = ""
    Public AType As String = ""

    Public CatId As String = ""
    Public FormulaId As String = ""
    Public TempOrderBy As String = ""
    Public V_Emp_Bool As Boolean = False
    Public V_Trans_Bool As Boolean = False
    Public SqlStr As String = ""
    Public condition As String = ""
    Public assettypes As String = ""
    Public orderby As String = ""
    Public ReportHeaders As String = ""
    Public ReportHeaderDesc As String = ""
    Public usercondition As String = ""
    Public reportname As String = ""
    Public cntrh As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FormulaId = Request.QueryString("Fid")
        If FormulaId IsNot Nothing Then
            If sqlcon.State = Data.ConnectionState.Open Then
                sqlcon.Close()
            End If
            sqlcon.Open()
            sqlcmd = New SqlCommand
            sqlcmd.Connection = sqlcon
            sqlcmd.CommandType = Data.CommandType.Text
            sqlcmd.CommandText = "select * from tbl_Asset_reportformula where id =" & FormulaId & ""
            drAsset = sqlcmd.ExecuteReader
            If drAsset.HasRows Then
                While drAsset.Read
                    ReportHeaders = Convert.ToString(drAsset("ReportHeaders"))
                    condition = Convert.ToString(drAsset("usercondition"))
                    assettypes = Convert.ToString(drAsset("assettypes"))
                    orderby = Convert.ToString(drAsset("groupby"))
                    ReportHeaderDesc = Convert.ToString(drAsset("reportheaderdesc"))
                    reportname = Convert.ToString(drAsset("reportname"))
                    usercondition = Convert.ToString(drAsset("condition"))
                    lblcondition.Text = Convert.ToString(drAsset("condition"))
                    lblreportname.Text = Convert.ToString(drAsset("reportname"))
                    ViewState("status") = Convert.ToString(drAsset("status"))
                End While
            End If
            sqlcmd.Dispose()
            drAsset.Close()
            sqlcon.Close()
        End If
        Dim attachment As String = "attachment; filename=RptExcel.xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
    End Sub
End Class
