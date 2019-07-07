Imports System.Data
Imports System.Data.SqlClient
Partial Class ConsumableMappingwithuserandassets
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim GId As String

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
    Dim sqladr As SqlDataAdapter
    Public Function GetAssetTypeList() As DataTable
        Dim sql As String = "select * from tbl_asset_typemaster am,tbl_asset_categorymaster ac where am.catid = ac.catid and ac.groupid = 3 order by am.assettypecode asc"
        '"select * from tbl_asset_typemaster at, tbl_asset_ order by assettypecode"
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    'Public Function GetConsumableList() As DataTable
    '    Dim sql As String = "select AssetTypeId,AssetTypeCode from tbl_Asset_TypeMaster atm, tbl_Asset_CategoryMaster acm where atm.catid = acm.catid  and  acm.groupid = '3' "
    '    If con.State = ConnectionState.Open Then
    '        con.Close()
    '    End If
    '    con.Open()
    '    Dim dtable As New DataTable
    '    sqladr = New SqlDataAdapter(sql, con)
    '    sqladr.Fill(dtable)
    '    Return dtable
    'End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim con2 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd As SqlCommand
        Dim str1 As String
        Dim dtable1 As New System.Data.DataTable
        Dim asset As Integer = 0
        Dim user As Integer = 0
        dtable1 = GetAssetTypeList()
        If dtable1.Rows.Count > 0 Then
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            If dtable1.Rows.Count > 0 Then
                For j As Integer = 0 To dtable1.Rows.Count - 1
                    If Request("chkasset" & dtable1.Rows(j)("AssetTypeid")) = "on" And Request("chkuser" & dtable1.Rows(j)("AssetTypeid")) = "on" And Request("chkprinter" & dtable1.Rows(j)("AssetTypeid")) = "on" Then
                        str1 = "update tbl_asset_typemaster set toasset=1,touser=1,toprinter=1 where assettypeid = " & dtable1(j)("assettypeid").ToString()
                        cmd = New SqlCommand(str1, con)
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    ElseIf Request("chkasset" & dtable1.Rows(j)("AssetTypeid")) = "on" And Request("chkuser" & dtable1.Rows(j)("AssetTypeid")) <> "on" And Request("chkprinter" & dtable1.Rows(j)("AssetTypeid")) <> "on" Then
                        str1 = "update tbl_asset_typemaster set toasset=1,touser=0,toprinter=0 where assettypeid = " & dtable1(j)("assettypeid").ToString()
                        cmd = New SqlCommand(str1, con)
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    ElseIf Request("chkasset" & dtable1.Rows(j)("AssetTypeid")) <> "on" And Request("chkuser" & dtable1.Rows(j)("AssetTypeid")) = "on" And Request("chkprinter" & dtable1.Rows(j)("AssetTypeid")) <> "on" Then
                        str1 = "update tbl_asset_typemaster set toasset=0,touser=1,toprinter=0 where assettypeid = " & dtable1(j)("assettypeid").ToString()
                        cmd = New SqlCommand(str1, con)
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    ElseIf Request("chkasset" & dtable1.Rows(j)("AssetTypeid")) <> "on" And Request("chkuser" & dtable1.Rows(j)("AssetTypeid")) <> "on" And Request("chkprinter" & dtable1.Rows(j)("AssetTypeid")) = "on" Then
                        str1 = "update tbl_asset_typemaster set toasset=0,touser=0,toprinter=1 where assettypeid = " & dtable1(j)("assettypeid").ToString()
                        cmd = New SqlCommand(str1, con)
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    ElseIf Request("chkasset" & dtable1.Rows(j)("AssetTypeid")) = "on" And Request("chkuser" & dtable1.Rows(j)("AssetTypeid")) = "on" And Request("chkprinter" & dtable1.Rows(j)("AssetTypeid")) <> "on" Then
                        str1 = "update tbl_asset_typemaster set toasset=1,touser=1,toprinter=0 where assettypeid = " & dtable1(j)("assettypeid").ToString()
                        cmd = New SqlCommand(str1, con)
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    ElseIf Request("chkasset" & dtable1.Rows(j)("AssetTypeid")) <> "on" And Request("chkuser" & dtable1.Rows(j)("AssetTypeid")) = "on" And Request("chkprinter" & dtable1.Rows(j)("AssetTypeid")) = "on" Then
                        str1 = "update tbl_asset_typemaster set toasset=0,touser=1,toprinter=1 where assettypeid = " & dtable1(j)("assettypeid").ToString()
                        cmd = New SqlCommand(str1, con)
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    ElseIf Request("chkasset" & dtable1.Rows(j)("AssetTypeid")) = "on" And Request("chkuser" & dtable1.Rows(j)("AssetTypeid")) <> "on" And Request("chkprinter" & dtable1.Rows(j)("AssetTypeid")) = "on" Then
                        str1 = "update tbl_asset_typemaster set toasset=1,touser=0,toprinter=1 where assettypeid = " & dtable1(j)("assettypeid").ToString()
                        cmd = New SqlCommand(str1, con)
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    Else
                        str1 = "update tbl_asset_typemaster set toasset=0,touser=0,toprinter=0 where assettypeid = " & dtable1(j)("assettypeid").ToString()
                        cmd = New SqlCommand(str1, con)
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    End If
                Next
            End If
            con.Close()
            Dim myscript1 As String = "alert('Consumables with Assets and Users Mapped Successfully! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
        End If
    End Sub
End Class
