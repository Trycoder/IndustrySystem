Imports System.Data
Imports System.Data.SqlClient
Partial Class ConsumablesMappingWithAssets
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim GId As String
    Dim sqladr As SqlDataAdapter
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowStatus", "javascript:winwidth();", True)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            '  bindAssetCategory()
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If

    End Sub
    Public Function GetAssetList() As DataTable
        Dim sql As String = "select atm.AssetTypeId,atm.AssetTypeCode,atm.Touser,atm.Toasset from tbl_Asset_TypeMaster atm,tbl_Asset_CategoryMaster acm where atm.catid = acm.catid and acm.groupid ='1'"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Public Function GetConsumableList() As DataTable
        Dim sql As String = "select AssetTypeId,AssetTypeCode from tbl_Asset_TypeMaster atm, tbl_Asset_CategoryMaster acm where atm.catid = acm.catid  and  acm.groupid = '3' "
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim con2 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd As SqlCommand
        Dim str1 As String
        Dim dtable1 As New DataTable
        Dim dtable2 As New DataTable

        dtable2 = GetConsumableList()
        dtable1 = GetAssetList()
        If dtable2.Rows.Count > 0 Then
            For j As Integer = 0 To dtable2.Rows.Count - 1
                str1 = "delete from tbl_Asset_cons_Mapping where ConsTypeid =" & dtable2(j)("AssetTypeId").ToString()
                con1.Open()
                cmd = New SqlCommand(str1, con1)
                cmd.ExecuteNonQuery()
                cmd.Dispose()
                con1.Close()
                If dtable1.Rows.Count > 0 Then
                    For k As Integer = 0 To dtable1.Rows.Count - 1
                        If Request("chk" & dtable2.Rows(j)("AssetTypeid") & dtable1.Rows(k)("AssetTypeId")) = "on" Then
                            str1 = "insert into tbl_Asset_cons_Mapping(AssetTypeId,ConsTypeid) values('" & dtable1(k)("AssetTypeId").ToString & "','" & dtable2(j)("AssetTypeId").ToString & "')"
                            con1.Open()
                            cmd = New SqlCommand(str1, con1)
                            cmd.ExecuteNonQuery()
                            con1.Close()
                        End If
                    Next
                End If
            Next
            Dim myscript1 As String = "alert('Consumables Mapped Successfully! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
End Class
