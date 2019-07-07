
Partial Class Testpage
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim sql As String = ""
    Dim cmd As SqlCommand
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim i As Integer
    Dim strbody As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        fnAjax()
    End Sub
    Public Function fnAjax() As String
        con.Open()
        Dim Datacount As Integer
        Datacount = 0
        ' Visible Popup ---------------------------------------------
        sql = "select c.complaintid,c.complaint,"
        sql = sql & " (select dbo.proper(e.emp_name) +' '+ e.emp_initial +'(' + e.emp_number + ')' + '-' + e.Dep_name from view_sip_employees e where e.emp_number = c.emp_number) as Emp_name,"
        sql = sql & " cat.catname,convert(varchar,cdate,106)  "
        sql = sql & " from tbl_hd_complaint c,tbl_hd_category cat,tbl_hd_consultant cons where"
        sql = sql & " cat.msgid = cons.gpid and c.closetag = 0 and c.catid = cat.catid and "
        sql = sql & " c.complaintid in (select v.complaintid from tbl_hd_consview v where v.consid = '" & Session("EmpNo") & "' and v.ctag = 0)  and cons.emp_number = '" & Session("EmpNo") & "' order by c.complaintid asc"

        strbody = ""
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        sqladr = New SqlDataAdapter(sql, con)
        dtable = New DataTable
        sqladr.Fill(dtable)
        Dim reccount As Integer = 0
        If dtable.Rows.Count > 0 Then
            Dim arrlist As New List(Of String)
            Dim arrnewlist As List(Of String)
            If Session("Complaints") IsNot Nothing Then
                arrnewlist = (DirectCast(Session("Complaints"), List(Of String)))
                arrlist = arrnewlist
                For j As Integer = 0 To dtable.Rows.Count - 1
                    If arrnewlist.Contains(dtable.Rows(j)("complaintid").ToString()) = False Then
                        arrlist.Add(dtable.Rows(j)("complaintid").ToString())
                    End If
                Next
            Else
                For j As Integer = 0 To dtable.Rows.Count - 1
                    arrlist.Insert(j, dtable.Rows(j)("complaintid").ToString())
                Next
                arrlist.Sort()
            End If
            Dim strdata As String = ""
            Dim count As Integer = dtable.Rows.Count
            For i As Integer = 0 To dtable.Rows.Count - 1
                If dtable.Rows(i)("complaintid").ToString() = arrlist(0) Then
                    reccount = 1
                    strbody = " <table>"
                    strbody &= "<tr><td id=emp  align='left'>"
                    strbody &= "" & dtable.Rows(i)("Emp_name") & "</a>"
                    strbody &= "</td></tr>"
                    strbody &= "<tr><td id=empp align='left'>"
                    strdata = dtable.Rows(i)("complaint").ToString()
                    If Len(strdata) > 200 Then
                        strdata = Left(strdata, 197) & "..."
                    End If
                    strbody &= "" & strdata & ""
                    strbody &= "</td><td id=cid align='left'> " & dtable.Rows(i)("complaintid").ToString() & " </td> </tr>"
                    strbody &= "</table> "
                    strbody = strbody & "<table><tr><td id='Datacount'>" & reccount & "</td></tr> </table>"
                End If
            Next
            arrlist.RemoveAt(0)
            Session("Complaints") = arrlist
        Else
            strbody = strbody & "<table><tr><td id='Datacount'>" & reccount & "</td></tr> </table>"
        End If
        Response.Write(strbody)
        Response.End()

        ' Visible Popup ---------------------------------------------
    End Function
End Class
