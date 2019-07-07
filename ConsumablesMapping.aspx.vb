Imports System.Data
Imports System.Data.SqlClient
Partial Class ConsumablesMapping
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
            bindAssetype()
            '  bindconsumables()
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub
    Public Function bindAssetype()
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpassets.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select atm.assettypeid,atm.assettypecode from tbl_Asset_TypeMaster atm join tbl_Asset_CategoryMaster acm on atm.catid = acm.catid where acm.groupid = '1' and upper(atm.assettypecode)=upper('Printer')"
        rdr = cmd.ExecuteReader
        drpassets.Items.Add(New ListItem("--Select--", 0))
        If rdr.HasRows Then
            While rdr.Read
                drpassets.Items.Add(New ListItem(rdr("assettypecode"), rdr("assettypeid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Public Function bindconsumables()
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpconsumables.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select AM.AssetTypeId,AM.AssetTypeCode from tbl_Asset_TypeMaster am where am.assettypeid in (select acm.constypeid from tbl_Asset_cons_Mapping acm where acm.assettypeid = '" & drpassets.SelectedValue & "') order by am.assettypecode asc"
        rdr = cmd.ExecuteReader
        drpconsumables.Items.Add(New ListItem("--Select--", 0))
        If rdr.HasRows Then
            While rdr.Read
                drpconsumables.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function
    Dim sqladr As SqlDataAdapter
    Public Function LoadDetailsView(ByVal assettype As String, ByVal tablename As String, ByVal tblFieldname As String) As DataTable
        Dim sql As String
        sql = "Select  distinct "
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aad.attid = aa.attid and aa.Header='7'  and aad.AssetTypeId = " & assettype & " order by aad.attid asc"
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                sql = sql & rdr("FieldOrder") & " as [" & rdr("AttDesc") & "],"
            End While
        Else
            sql = sql & "* "
        End If
        rdr.Close()
        con.Close()
        sql = Left(sql, Len(sql) - 1)
        sql = sql & ", " & tblFieldname & " from " & tablename & " where " & tblFieldname & " =" & assettype
        cmd.Dispose()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim dtable As New DataTable
        Dim vertical As New DataTable
        sqladr = New SqlDataAdapter(sql, con)
        sqladr.Fill(dtable)
        Return dtable
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim con2 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd As SqlCommand
        Dim str1 As String
        Dim dtable1 As New System.Data.DataTable
        Dim dtable2 As New System.Data.DataTable
        dtable1 = LoadDetailsView(drpassets.SelectedValue, "tbl_Asset_Master", "AssetTypeid")
        If dtable1.Rows.Count > 0 Then
            str1 = "delete from tbl_Asset_Mapping where AssetTypeId =" & drpassets.SelectedValue & " and contypeid=" & drpconsumables.SelectedValue
            con1.Open()
            cmd = New SqlCommand(str1, con1)
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            con1.Close()
            For j As Integer = 0 To dtable1.Rows.Count - 1
                dtable2 = LoadDetailsView(drpconsumables.SelectedValue, "tbl_Asset_ConsumablesMaster", "ConsTypeId")
                If dtable2.Rows.Count > 0 Then
                    For k As Integer = 0 To dtable2.Rows.Count - 1
                        If Request("chk" & j & k & dtable1.Rows(j)("AssetTypeid") & dtable2.Rows(k)("ConsTypeId")) = "on" Then
                            str1 = "insert into tbl_Asset_Mapping(assettypeid,contypeid,assetmodel,conmodel) values(" & drpassets.SelectedValue & "," & drpconsumables.SelectedValue & ", '" & dtable1(j)(0).ToString & "','" & dtable2(k)(0).ToString & "')"
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

    Protected Sub drpassets_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpassets.SelectedIndexChanged
        If drpassets.SelectedValue <> "" Then
            bindconsumables()
        End If
    End Sub
End Class
