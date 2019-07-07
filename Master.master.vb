Imports System.IO
Imports System.Data.SqlClient
Imports System.Data



Partial Class Master
    Inherits System.Web.UI.MasterPage
    Dim WUser As String = ""
    Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim sqladr1 As SqlDataAdapter
    Dim dtable As DataTable
    Dim dtable1 As DataTable
    Dim str As String = ""
    Dim strval As String = ""
    Public qry_local As String
    Dim strstring As String = ""
    Dim att As String
    Public qry As String
    Public lbtn As HyperLink
    Public lnkBtn As LinkButton

    Public iframsrc As String

    Dim gvNewPageIndex As Integer
    Dim gvEditIndex As Integer
    Dim gvUniqueID As String
    Dim gvSortExpr, statuss As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If Group Id Expires
        If Session("EmpNo") = Nothing Or Session("EmpNo") = "" Then
            WUser = (Request.ServerVariables("AUTH_USER"))
            '  WUser = Mid(WUser, InStr(WUser, "\") + 1, (Len(WUser) - InStr(WUser, "\")))
            WUser = "Check\Check"
            Dim user() As String = WUser.Split("\")
            If user.Length > 0 Then
                ''Session("EmpNo") = Right(user(1), 4)
                Session("EmpNo") = "1001"
                If Not String.IsNullOrEmpty(user(1)) Then
                    ''strstring = "select * from view_SIP_Employees where Emp_Number='" & Right(user(1), 4) & "'"
                    strstring = "select * from tbl_asset_users where userid='" & Session("EmpNo") & "'"
                    con.Open()
                    cmd = New SqlCommand
                    cmd.Connection = con
                    cmd.CommandType = Data.CommandType.Text
                    cmd.CommandText = strstring
                    rdr = cmd.ExecuteReader
                    If rdr.HasRows = True Then
                        While rdr.Read
                            'Session("EmpName") = rdr("Emp_Name") & " " & rdr("Emp_Initial")
                            Session("EmpName") = rdr("username")
                        End While
                    End If
                    rdr.Close()
                    con.Close()
                End If
            End If
        End If


        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand("select * from tbl_Asset_users where userid='" & Session("EmpNo") & "' and status='A'", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows = False Then
            Response.Redirect("Errorpage.aspx")
        End If
        rdr.Close()
        con.Close()

        Dim name As String = Path.GetFileName(Request.ServerVariables("SCRIPT_NAME"))
        Dim qrystring As String = Request.ServerVariables("QUERY_STRING")
        Dim fullname As String
        If qrystring <> "" Then
            fullname = name & "?" & qrystring
        Else
            fullname = name
        End If
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        Dim str As String = ""
        str = "select mi.id,mg.groupid,mg.rights,mi.mainmenuid,mi.submenuid,mi.menudesc,mi.pagename"
        str = str & " from tbl_asset_menugroup mg,tbl_asset_users usr,tbl_asset_menuitems mi where mg.menuid = mi.id and mg.groupid = usr.usergroup and usr.userid = '" & Session("EmpNo") & "'"
        str = str & " and mi.pagename = '" & fullname & "'"
        cmd = New SqlCommand(str, con)
        rdr = cmd.ExecuteReader
        While rdr.Read
            Session("Usergroup") = rdr("rights")
            Session("Admingroup") = rdr("groupid")
        End While
        If Session("Usergroup") = "0" And Session("mtag") <> "1" Then
            Response.Redirect("Default.aspx")
        End If

        rdr.Close()
        con.Close()

        'Dim AllowUsers() As String = {"6759", "6579", "6899", "5009", "7510", "6694", "6604", "7444", "7534", "7512", "6174", "7104", "7538", "7446", "6338", "0638", "9433", "9432", "0770", "7369", "7508", "7510", "9658", "7157", "1193", "6518", "9633", "7303", "7713"}
        'If AllowUsers.Contains(Session("EmpNo")) = False Then
        '    Response.Redirect("Errorpage.aspx")
        'End If
        'Group Id Expires
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim name As String = Path.GetFileName(Request.ServerVariables("SCRIPT_NAME"))
        Dim qrystring As String = Request.ServerVariables("QUERY_STRING")
        Dim fullname As String
        If qrystring <> "" Then
            fullname = name & "?" & qrystring
        Else
            fullname = name
        End If
        
    End Sub

    Private Function ChildDataSource1(ByVal TransType As String) As SqlDataSource
        Dim strQRY As String = ""
        Dim dsTemp As New SqlDataSource
        dsTemp.ConnectionString = ConfigurationManager.ConnectionStrings("ims").ConnectionString.ToString

        dsTemp.SelectCommand = "select atm.assettypeid,tas.status as stat,ATM.assettypecode as asset, '(' + convert(varchar,count(tas.assetid)) + ')' " & _
                            "as CountOfAsset from tbl_Asset_Master AM inner join  tbl_Asset_TypeMaster ATM on " & _
                            "AM.assettypeid=ATM.assettypeid inner join tbl_Asset_Status TAS on AM.id=TAS.assetid " & _
                            " where tas.status = '" & TransType & "' group by tas.status,ATM.assettypecode,atm.assettypeid "
        'Response.Write(dsTemp.SelectCommand)
        'Response.End()
        Return dsTemp
    End Function
    
    Private Function ChildDataSource3(ByVal TransType As String) As SqlDataSource
        Dim strQRY As String = ""
        Dim dsTemp As New SqlDataSource
        dsTemp.ConnectionString = ConfigurationManager.ConnectionStrings("ims").ConnectionString.ToString

        dsTemp.SelectCommand = "select am.assettypeid,am.status,atm.AssetTypeCode, '(' + convert(varchar,count(am.id)) + ')' as Reccount from View_assetmaster_status am join " & _
                                " tbl_Asset_Transactions tr on am.id = tr.assetid " & _
                                " join  tbl_Asset_TypeMaster atm on am.AssetTypeid = atm.AssetTypeId" & _
                                " where tr.transtype in (2,5) and tr.date2<>'' and convert(datetime,tr.date2) >= getdate() and am.status in ('With User','Repair(Outside)')  and am.assettypeid = " & TransType & "" & _
                                " group by am.status,atm.AssetTypeCode,am.assettypeid "
        'Response.Write(dsTemp.SelectCommand)
        'Response.End()
        Return dsTemp
    End Function


End Class

