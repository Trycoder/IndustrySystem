Imports System.Data
Imports System.Data.SqlClient
Partial Class EditSoftwareItems
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
                        lbl.ID = "lbl" & dtableassets(i)("licid").ToString
                        lbl.Text = dtableassets(i)(primaryfieldname).ToString
                        Dim txt As New TextBox
                        txt.ID = "txt" & dtableassets(i)("licid").ToString
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
            If Session("Usergroup") <> "1" Then
                btnSave.Enabled = False
            End If

        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function GetAssetValues(ByVal fromid As String, ByVal toid As String, ByVal fieldname As String, ByVal primaryfieldname As String) As DataTable
        Dim sql As String
        Try
            sql = "select licid," & primaryfieldname & "," & fieldname & "  from  tbl_asset_soft_master  where licid between " & fromid & " and " & toid & " and softwareid=" & AssetType & "  and  status ='S'  order by licid ASC"
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
            sql = "select licid," & primaryfieldname & "  from  tbl_asset_soft_master where licid =" & assettypeid & " and status='S'"
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
                        txt = CType(strtable.FindControl("txt" & dtableassets(i)("licid").ToString), TextBox)
                        If txt IsNot Nothing Then
                            UpdateAssetItems(dtableassets(i)("licid").ToString, Fieldname, txt.Text)
                        Else

                        End If
                    Next
                End If
            End If
            Dim myscript1 As String = "alert('Software Updated Successfully! ');"
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
                sql = "update tbl_asset_soft_master set " & fieldname & " =" & " '" & value & "' where licid=" & assetid
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
