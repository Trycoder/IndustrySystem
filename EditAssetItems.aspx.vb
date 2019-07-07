Imports System.Data
Imports System.Data.SqlClient
Partial Class EditAssetItems
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim Fromid As String = ""
    Dim Toid As String = ""
    Dim Fieldname As String = ""
    Dim Fieldcount As String = ""
    Dim primaryfieldname As String = ""
    Dim AssetType As String = ""
    Dim dtableassets As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Fromid = Request.QueryString("Fid")
            Toid = Request.QueryString("Tid")
            Fieldname = Request.QueryString("Fname")
            Fieldcount = Request.QueryString("Fcount")
            lblassetname.Text = Request.QueryString("AType")
            primaryfieldname = Request.QueryString("PField")
            AssetType = Request.QueryString("AId")
            lblfrom.Text = GetAssettypevalue(Request.QueryString("Fid"), primaryfieldname)
            lblto.Text = GetAssettypevalue(Request.QueryString("Tid"), primaryfieldname)
            dtableassets = New DataTable
            If Not String.IsNullOrEmpty(Fromid) And Not String.IsNullOrEmpty(Toid) And Not String.IsNullOrEmpty(Fieldname) And Not String.IsNullOrEmpty(primaryfieldname) Then
                dtableassets = GetAssetValues(Fromid, Toid, Fieldname, primaryfieldname)
                If dtableassets.Rows.Count > 0 Then
                    Dim strtable As New Table
                    strtable.ID = "strtable1"
                    strtable.Width = Unit.Percentage(100)
                    For i As Integer = 0 To dtableassets.Rows.Count - 1
                        Dim strrow As New TableRow
                        Dim strCell_1 As New TableCell
                        Dim strcell_2 As New TableCell
                        strCell_1.Width = Unit.Percentage(50)
                        strcell_2.Width = Unit.Percentage(50)
                        'strtable.Style("align") = ""
                        strCell_1.Style("text-align") = "center"
                        strcell_2.Style("text-align") = "left"
                        strCell_1.CssClass = "tdtext"
                        strcell_2.CssClass = "tdtext"
                        Dim lbl As New Label
                        lbl.ID = "lbl" & dtableassets(i)("id").ToString
                        lbl.Text = dtableassets(i)(primaryfieldname).ToString
                        Dim txt As New TextBox
                        txt.ID = "txt" & dtableassets(i)("id").ToString
                        txt.Text = dtableassets(i)(Fieldname).ToString
                        strCell_1.Controls.Add(lbl)
                        strcell_2.Controls.Add(txt)
                        strrow.Cells.Add(strCell_1)
                        strrow.Cells.Add(strcell_2)
                        strtable.Rows.Add(strrow)
                    Next
                    tddata.Controls.Add(strtable)
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function GetAssetValues(ByVal fromid As String, ByVal toid As String, ByVal fieldname As String, ByVal primaryfieldname As String) As DataTable
        Dim sql As String
        Try
            sql = "select am.id,am." & primaryfieldname & ",am." & fieldname & "  from  tbl_Asset_Master am join tbl_Asset_Status st on am.id=st.Assetid  where am.id between " & fromid & " and " & toid & " and am.AssetTypeid=" & AssetType & "  and  st.status ='N'  order by am.id ASC"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            sqladr = New SqlDataAdapter(sql, con)
            dtable = New DataTable
            sqladr.Fill(dtable)
            Return dtable
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function
    Public Function GetAssettypevalue(ByVal assettypeid As String, ByVal primaryfieldname As String) As String
        Dim sql As String
        Try
            sql = "select am.id,am." & primaryfieldname & "  from  tbl_Asset_Master am join  tbl_Asset_Status st on am.id=st.assetid where am.id =" & assettypeid & " and st.status='N'"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.Read Then
                Return rdr(primaryfieldname)
            Else
                Return ""
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
            If Not String.IsNullOrEmpty(Fromid) And Not String.IsNullOrEmpty(Toid) And Not String.IsNullOrEmpty(Fieldname) And Not String.IsNullOrEmpty(primaryfieldname) Then
                dtableassets = GetAssetValues(Fromid, Toid, Fieldname, primaryfieldname)
                If dtableassets.Rows.Count > 0 Then
                    Dim txt As New TextBox
                    Dim strtable As New Table
                    strtable = CType(tddata.FindControl("strtable1"), Table)
                    strtable.Width = Unit.Percentage(100)
                    For i As Integer = 0 To dtableassets.Rows.Count - 1
                        txt = CType(strtable.FindControl("txt" & dtableassets(i)("id").ToString), TextBox)
                        If txt IsNot Nothing Then
                            UpdateAssetItems(dtableassets(i)("id").ToString, Fieldname, txt.Text)
                        Else

                        End If
                    Next
                End If
            End If
            Dim myscript1 As String = "alert('Asset Items Updated Successfully! ');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function UpdateAssetItems(ByVal assetid As String, ByVal fieldname As String, ByVal value As String)
        Dim sql As String
        Try
            If fieldname <> "" And assetid <> "" Then
                sql = "update tbl_Asset_Master set " & fieldname & " =" & " '" & value & "' where id=" & assetid
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                cmd = New SqlCommand(sql, con)
                cmd.ExecuteNonQuery()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function
End Class
