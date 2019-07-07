Imports System.Data
Imports System.Data.SqlClient
Partial Class ConsumableFieldorder
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As New SqlCommand
    Dim rdr As SqlDataReader
    Dim GId As String
    Dim sqladr As SqlDataAdapter

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
            btnsave.Enabled = False
        End If

    End Sub
    Public Function bindAssetype()
        Dim sql As String
        con.Open()
        cmd.Connection = con
        drpassets.Items.Clear()
        cmd.CommandType = Data.CommandType.Text
        cmd.CommandText = "select atm.assettypeid,atm.assettypecode from tbl_Asset_TypeMaster atm join tbl_Asset_CategoryMaster  acm on atm.catid = acm.catid where acm.groupid = '3' order by atm.assettypecode asc"
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
    Public Function LoadDetailsView(ByVal assettype As String) As DataTable
        Dim sql As String
        sql = "select * from tbl_asset_attributes aa,tbl_asset_attribute_details aad where aa.attid = aad.attid and aad.assettypeid = " & drpassets.SelectedValue & " order by aa.attdesc asc"
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
        con.Close()
    End Function
    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd As SqlCommand
        Dim str1 As String
        Dim dtable1 As New System.Data.DataTable
        Dim dtable2 As New System.Data.DataTable
        dtable1 = LoadDetailsView(drpassets.SelectedValue)
        If dtable1.Rows.Count > 0 Then
            For j As Integer = 0 To dtable1.Rows.Count - 1
                If Request("txt" & dtable1.Rows(j)("attid")) IsNot Nothing Then
                    If CInt(Request("txt" & dtable1.Rows(j)("attid"))) <= 3 Then
                        str1 = "update tbl_asset_attribute_details set seq= " & Request("txt" & dtable1.Rows(j)("attid")) & " where attid=" & dtable1.Rows(j)("attid") & " and assettypeid=" & drpassets.SelectedValue
                        con1.Open()
                        cmd = New SqlCommand(str1, con1)
                        cmd.ExecuteNonQuery()
                        con1.Close()
                    Else
                        Dim myscript2 As String = "alert('Only 3 Sequence Order Should be Allowed! ');"
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript2, True)
                    End If
                End If
            Next
            Dim myscript1 As String = "alert('Sequence Order Saved Successfully! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
        Else
            Dim myscript1 As String = "alert('Attributes are Empty! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
        End If
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("Default.aspx", True)
    End Sub
End Class
