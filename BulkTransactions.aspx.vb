Imports System.Data
Imports System.Data.SqlClient
Partial Class BulkTransactions
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim catid As String = ""
    Dim assettypeid As String = ""
    Dim transtag As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        catid = Request.QueryString("CId")
        assettypeid = Request.QueryString("AId")
        transtag = Request.QueryString("Tid")
        If Not IsPostBack Then
            If catid IsNot Nothing AndAlso assettypeid IsNot Nothing Then
                If transtag = "7" Then
                    GetAssetDetails("'U','S','R'")
                ElseIf transtag = "8" Then
                    GetAssetDetails("'U','S','R','E'")
                End If
            End If
        End If
    End Sub
    Private Function GetAssetDetails(ByVal status As String, Optional ByVal AssetType As Boolean = False)
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Try
            sql = "select aa.Fieldorder from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid and  aa.Header = 5 and aa.catid =" & catid
            cmd = New SqlCommand(sql, con)
            Dim s As String = cmd.ExecuteScalar()
            cmd.Dispose()
            con.Close()
            If Not String.IsNullOrEmpty(s) Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                sql = ""
                If AssetType = True Then
                    sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid in('1','2') and am.id = ast.Assetid and ast.status in(" & status & ") "
                Else
                    sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & assettypeid & " and am.id = ast.Assetid and ast.status in(" & status & ") "
                End If
                sql = sql & " order by am.att1 asc"
                sqladr = New SqlDataAdapter(sql, con)
                dtable = New DataTable
                sqladr.Fill(dtable)
                If dtable.Rows.Count > 0 Then
                    chkassets.Items.Clear()
                    For Each dr As DataRow In dtable.Rows
                        chkassets.Items.Add(New ListItem(dr(s).ToString(), dr("id").ToString()))
                    Next
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Session("Assets") = Nothing
            Dim chkitem As Boolean = False
            For Each item As ListItem In chkassets.Items
                If item.Selected Then
                    chkitem = True
                    Session("Assets") = Session("Assets") & item.Value & "|"
                End If
            Next
            If chkitem = True Then
                Session("Assets") = Left(Session("Assets"), Len(Session("Assets")) - 1)
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "javascript:window.close();", True)
            Else
                Dim myscript As String = "alert('Plese Select Assets');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                chkassets.Focus()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
End Class
