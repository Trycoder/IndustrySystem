Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Partial Class AssetTypeDetails
    Inherits System.Web.UI.Page
    Dim ds As New DataSet
    Dim objDB As New DBFunctions
    Dim sql As String
    Dim TransDays As Integer
    Dim WarrantyExp As Integer
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim cmd1 As SqlCommand
    Dim rdr As SqlDataReader
    Dim rdr1 As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '-----------------------------------------------------------------------------------------------------------------------
        Dim WUser As String
        WUser = (Request.ServerVariables("AUTH_USER"))
        WUser = Mid(WUser, InStr(WUser, "\") + 1, (Len(WUser) - InStr(WUser, "\")))
        WUser = Right(WUser, Len(WUser) - 5)
        sql = "SELECT TransDays,WarrantyExpDays FROM tbl_Asset_SetUpDays WHERE UserID = '" & Trim(WUser) & "'"
        ds = objDB.FillDataset(sql)
        If ds.Tables(0).Rows.Count > 0 Then
            TransDays = ds.Tables(0).Rows(0)(0)
            WarrantyExp = ds.Tables(0).Rows(0)(1)
            ds.Clear()
        End If
        '-----------------------------------------------------------------------------------------------------------------------
    End Sub
    Public Sub AssetTransaction()
        Try
            Dim strFields As String = ""
            Dim dsHeader As New DataSet
            sql = "SELECT * FROM tbl_Asset_Attributes AA,tbl_Asset_Attribute_Details AD " & _
                    "WHERE AA.AttID = AD.AttID AND AA.IsRequired = 1  AND CatID = " & Request("CatID") & " AND AD.AssetTypeID = " & Request("AssetTypeID")
            ds = objDB.FillDataset(sql)
            dsHeader = objDB.FillDataset(sql)
            For i = 0 To ds.Tables(0).Rows.Count - 1
                strFields = strFields & "," & ds.Tables(0).Rows(i)("FieldOrder") & " AS [" & ds.Tables(0).Rows(i)("AttDesc") & "]"
            Next
            strFields = Right(strFields, Len(strFields) - 1)

            ds.Clear()

            Response.Write("<table class='mGrid'>")
            Response.Write("<tr>")
            Response.Write("<th>")
            Response.Write("S.No</th>")
            For i = 0 To dsHeader.Tables(0).Rows.Count - 1
                Response.Write("<th>")
                'If i = 0 Then
                '    Response.Write("SN.")
                'Else
                Response.Write(dsHeader.Tables(0).Rows(i)("AttDesc").ToString)
                ' End If
                Response.Write("</th>")
            Next
            Response.Write("</tr>")

            sql = "SELECT ROW_NUMBER() OVER (ORDER BY tbl_Asset_Master.id) AS SNo," & strFields & "FROM tbl_Asset_Master,tbl_Asset_Transactions " & _
                    "WHERE tbl_Asset_Master.AssetTypeID = " & Request("AssetTypeID") & " " & _
                    "AND tbl_Asset_Master.ID = tbl_Asset_Transactions.AssetID " & _
                    "AND tbl_Asset_Transactions.TransType = " & Request("TransTypeID") & " AND tbl_Asset_Transactions.date1 >= DATEADD(day,-(" & Trim(TransDays) & "),getdate()) "
            ds = objDB.FillDataset(sql)
            Dim s As Integer = 0
            For i = 0 To ds.Tables(0).Rows.Count - 1
                If s = 0 Then
                    Response.Write("<tr>")
                    s = 1
                Else
                    Response.Write("<tr class='alt'>")
                    s = 0
                End If
                For j = 0 To ds.Tables(0).Columns.Count - 1
                    Response.Write("<td>")
                    Response.Write(ds.Tables(0).Rows(i)(j))
                    Response.Write("</td>")
                Next
                Response.Write("</tr>")
            Next
            Response.Write("</table>")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub ConsumableTransaction()
        Try
            Dim strFields As String = ""
            Dim dsHeader As New DataSet

            sql = "SELECT * FROM tbl_Asset_Attributes AA,tbl_Asset_Attribute_Details AD " & _
                    "WHERE AA.AttID = AD.AttID AND AA.IsRequired = 1  AND CatID = " & Request("CatID") & " AND AD.AssetTypeID = " & Request("AssetTypeID")
            ds = objDB.FillDataset(sql)
            dsHeader = objDB.FillDataset(sql)
            For i = 0 To ds.Tables(0).Rows.Count - 1
                strFields = strFields & ",ACS." & ds.Tables(0).Rows(i)("FieldOrder") & " AS [" & ds.Tables(0).Rows(i)("AttDesc") & "]"
            Next
            strFields = Right(strFields, Len(strFields) - 1)
            ds.Clear()
            Response.Write("<table cellpadding='4' cellspacing='1' border='0' width='100%' align='center' id='myTable'>")
            Response.Write("<tr class='trheaderbg'>")
            Response.Write("<td  align='center' class='trheaderbg' style='font-family:verdana;font-size:12px;border: 1px solid #3C8FD1;'>")
            Response.Write("Brand Name")
            Response.Write("</td>")
            Response.Write("<td  align='center' class='trheaderbg' style='font-family:verdana;font-size:12px;border: 1px solid #3C8FD1;'>")
            Response.Write("Location")
            Response.Write("</td>")
            Response.Write("<td  align='center' class='trheaderbg' style='font-family:verdana;font-size:12px;border: 1px solid #3C8FD1;'>")
            Response.Write("Sub-Location")
            Response.Write("</td>")
            Response.Write("<td  align='center' class='trheaderbg' style='font-family:verdana;font-size:12px;border: 1px solid #3C8FD1;'>")
            Response.Write("Quantity")
            Response.Write("</td>")
            Response.Write("</tr>")

            sql = "SELECT " & strFields & ",ACS.Quantity" & _
                    " FROM tbl_Asset_Cons_Stock ACS,tbl_Asset_TypeMaster ATM,tbl_Asset_location AL,tbl_Asset_sublocation ASL," & _
                    " tbl_Asset_location_master ALM,tbl_Asset_Cons_Transactions ACT" & _
                    " WHERE AL.loccatid = ALM.id and ACS.ConsTypeId = ATM.assettypeid and ACS.ConsTypeId = '" & Request("AssetTypeID") & "'" & _
                    " AND ACS.LOCID = AL.LOCID AND ACS.SUBLOCID = ASL.SUBLOCID AND ACT.ConsTypeID = ATM.AssetTypeID AND ACT.TransType = " & Request("TransTypeID") & "" & _
                    " AND ACT.date1 >= DATEADD(day,-(" & Trim(TransDays) & "),getdate())"
            ds = objDB.FillDataset(sql)

            For i = 0 To ds.Tables(0).Rows.Count - 1
                Response.Write("<tr height='20' onmouseover='ChangeColor(this);'>")
                For j = 0 To ds.Tables(0).Columns.Count - 1
                    Response.Write("<td align='center' style='font-family:verdana;font-size:11px;border: 1px solid #3C8FD1;'>")
                    Response.Write(ds.Tables(0).Rows(i)(j))
                    Response.Write("</td>")
                Next
                Response.Write("</tr>")
            Next
            Response.Write("</table>")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub WarrantyExpiry()
        Try
            'Request("TransTypeID") is intensionally passed as 0 for WarrantyExpiry.
            Dim strFields As String = ""
            Dim dsHeader As New DataSet
            sql = "SELECT * FROM tbl_Asset_Attributes AA,tbl_Asset_Attribute_Details AD " & _
                    "WHERE AA.AttID = AD.AttID AND AA.IsRequired = 1  AND CatID = " & Request("CatID") & " AND AD.AssetTypeID = " & Request("AssetTypeID")
            ds = objDB.FillDataset(sql)
            dsHeader = objDB.FillDataset(sql)
            For i = 0 To ds.Tables(0).Rows.Count - 1
                strFields = strFields & "," & ds.Tables(0).Rows(i)("FieldOrder") & " AS [" & ds.Tables(0).Rows(i)("AttDesc") & "]"
            Next
            strFields = Right(strFields, Len(strFields) - 1)

            ds.Clear()


            Response.Write("<table cellpadding='4' cellspacing='1' border='0' width='100%' align='center'>")
            Response.Write("<tr class='trheaderbg'>")
            For j = 0 To dsHeader.Tables(0).Rows.Count - 1
                Response.Write("<td  align='center' class='trheaderbg' style='font-family:verdana;font-size:12px;border: 1px solid #3C8FD1;'>")
                Response.Write(dsHeader.Tables(0).Rows(j)("AttDesc").ToString)
                Response.Write("</td>")
            Next
            Response.Write("</tr>")
            sql = "SELECT " & strFields & " FROM tbl_Asset_Master,tbl_Asset_Transactions" & _
                    " WHERE tbl_Asset_Master.AssetTypeID = " & Request("AssetTypeID") & " And tbl_Asset_Master.ID = tbl_Asset_Transactions.AssetID" & _
                    " AND tbl_Asset_Transactions.Date3 <= DateAdd(Day, " & WarrantyExp & ", getdate()) And tbl_Asset_Transactions.Date3 >= getdate()"
            ds = objDB.FillDataset(sql)

            For i = 0 To ds.Tables(0).Rows.Count - 1
                Response.Write("<tr>")
                For j = 0 To ds.Tables(0).Columns.Count - 1
                    Response.Write("<td align='center' style='font-family:verdana;font-size:11px;border: 1px solid #3C8FD1;'>")
                    Response.Write(ds.Tables(0).Rows(i)(j))
                    Response.Write("</td>")
                Next
                Response.Write("</tr>")
            Next
            Response.Write("</table>")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub RequestCons()
        Try
            Dim strFields As String = ""
            Dim arr() As String
            'strFields = "Asset Type,Brand Name,Size,Color,Location,Sub Location,Deploye Type,Quantity,Requested By,Request Date,Complaint By,Complaint Date,NodeID/UserID,Approved By,Approved Date"
            strFields = "Asset Type,Brand Name,Size,Color,Location,Sub Location,Deploye Type,Quantity,Complaint Details,Requested Details"
            arr = strFields.Split(",")
            Dim dsHeader As New DataSet
            Response.Write("<table class='mGrid'>")
            Response.Write("<tr>")
            For i = 0 To UBound(arr)
                Response.Write("<th align='center' style='white-space:nowrap;'>")
                Response.Write(arr(i))
                Response.Write("</th>")
            Next
            Response.Write("</tr>")
            sql = "SELECT T.AssetTypeCode,R.Att1,R.Att2,R.Att3,(SELECT L.LocName FROM tbl_Asset_Location L where L.LocID = R.LocID) AS Location, "
            sql = sql & " (SELECT sl.sublocname FROM tbl_Asset_sublocation sl where sl.sublocid = R.sublocid) AS sublocation,CASE WHEN R.deploytype = 'A' "
            sql = sql & " THEN 'To Asset' WHEN R.deploytype = 'U' THEN 'To User' WHEN R.deploytype = 'P' THEN 'To Printer' END AS deploytype,R.Qty, "
            sql = sql & " (SELECT e.emp_name+' '+ e.emp_initial + '-' + e.emp_Number  FROM view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number and hd.complaintid = R.complaintid) + '(' + "
            sql = sql & " (SELECT  convert(varchar,hd.cdate,106) FROM view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number and hd.complaintid = R.complaintid) + ')' AS CompliantDetails ,"
            sql = sql & " (SELECT emp_name+' '+emp_initial + '(' + emp_Number + ')' FROM view_SIP_Employees where emp_number = R.consid) + '(' +"
            sql = sql & " convert(varchar,R.reqdate,106) + ')' AS RequestDetails"
            sql = sql & " FROM tbl_asset_typemaster T,tbl_asset_consrequest R where(R.constypeid = t.assettypeid and R.apptag='0' and r.isstag = '0' and r.rejtag='0' "
            sql = sql & " AND T.AssetTypeID = '" & Request("AssetTypeID") & "') AND R.ConsID = '" & Request("TransTypeID") & "'"
            ds = objDB.FillDataset(sql)
            For i = 0 To ds.Tables(0).Rows.Count - 1
                Response.Write("<tr height='20'>")
                For j = 0 To ds.Tables(0).Columns.Count - 1
                    Response.Write("<td align='center' sstyle='white-space:nowrap;'>")
                    Response.Write(ds.Tables(0).Rows(i)(j))
                    Response.Write("</td>")
                Next
                Response.Write("</tr>")
            Next
            Response.Write("</table>")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub ApproveCons()
        Try
            Dim strFields As String = ""
            Dim arr() As String
            strFields = "Asset Type,Brand Name,Size,Color,Location,Sub Location,Deploye Type,Quantity,Complaint Details,Requested Details,Approved Details"
            arr = strFields.Split(",")
            Dim dsHeader As New DataSet
            Response.Write("<table width='100%' align='center' class='mGrid'>")
            Response.Write("<tr>")
            For i = 0 To UBound(arr)
                Response.Write("<th>")
                Response.Write(arr(i))
                Response.Write("</th>")
            Next
            Response.Write("</tr>")
            sql = " SELECT T.AssetTypeCode,R.Att1,R.Att2,R.Att3,(SELECT L.LocName FROM tbl_Asset_Location L where L.LocID = R.LocID) AS Location,"
            sql = sql & " (SELECT sl.sublocname FROM tbl_Asset_sublocation sl where sl.sublocid = R.sublocid) AS sublocation,"
            sql = sql & " CASE WHEN R.deploytype = 'A' THEN 'To Asset' WHEN R.deploytype = 'U' THEN 'To User' WHEN R.deploytype = 'P' THEN 'To Printer' END AS deploytype, R.Qty, "
            sql = sql & " (SELECT e.emp_name+' '+ e.emp_initial + '-' + e.emp_Number  FROM view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number and hd.complaintid = R.complaintid) + '(' + "
            sql = sql & " (SELECT  convert(varchar,hd.cdate,106) FROM view_SIP_Employees e,tbl_hd_complaint hd where e.emp_number = hd.emp_number and hd.complaintid = R.complaintid) + ')' AS CompliantDetails,  "
            sql = sql & " (SELECT emp_name+' '+ emp_initial + '-' + emp_Number  FROM view_SIP_Employees where emp_number = R.consid) + '(' + convert(varchar,R.reqdate,106) + ')' AS RequestedDetails,"
            sql = sql & " (select e.emp_name+' '+ e.emp_initial + '-' + e.emp_Number FROM view_SIP_Employees e where e.Emp_Number = r.appby) + '(' +"
            sql = sql & " convert(varchar,R.appdate,106) + ')' AS ApprovedDetails "
            sql = sql & " FROM tbl_asset_typemaster T,tbl_asset_consrequest R where(R.constypeid = t.assettypeid and R.apptag='1' and r.isstag = '0' and r.rejtag='0'"
            sql = sql & " AND T.AssetTypeID = '" & Request("AssetTypeID") & "') AND R.ConsID = '" & Request("TransTypeID") & "'"

            ds = objDB.FillDataset(sql)
            ',CASE WHEN R.Emp_Number = '' THEN (R.NodeID) WHEN R.NodeID = '' THEN (R.Emp_Number) END AS 'NodeID/UserID'
            For i = 0 To ds.Tables(0).Rows.Count - 1
                Response.Write("<tr height='20'>")
                For j = 0 To ds.Tables(0).Columns.Count - 1
                    Response.Write("<td align='center'>")
                    Response.Write(ds.Tables(0).Rows(i)(j))
                    Response.Write("</td>")
                Next
                Response.Write("</tr>")
            Next
            Response.Write("</table>")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function BindOtherAssets()
        Try
            ' Dim RequiredFields As String = GetRequiredFields(assettypeid)
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            Dim sql As String = ""
            sql = " SELECT AM.AssetTypeID AS AssetTypeID,ATM.CatID AS CatID, "
            sql = sql & " CASE AT.transtype  WHEN '0' THEN 'Purchase' WHEN '1' THEN 'Warranty' WHEN '2' THEN "
            sql = sql & " 'Deployment' WHEN '3' THEN 'Undeployment'  WHEN '4' THEN 'Repair(Inhouse)' WHEN '5' THEN "
            sql = sql & " 'Repair(Outside)' WHEN '6' THEN 'Return' WHEN '7' THEN 'Retired' WHEN '8' THEN 'Sale' WHEN '9' "
            sql = sql & " THEN 'Deployment(Idle)' WHEN '10' THEN 'UnDeployment(Idle)' WHEN '11' THEN 'Location Change' "
            sql = sql & " WHEN '20' THEN 'Edit Asset' ELSE '' END AS TransType, COUNT(AT.TransType) AS CNT,ATM.AssetTypeCode AS AssetTypeCode,"
            sql = sql & " AT.transtype AS TType  FROM tbl_Asset_Transactions AT, tbl_asset_master AM,tbl_asset_typemaster ATM  "
            sql = sql & " WHERE atm.assettypecode not in('Desktop','Laptop','Monitor','Workstation') and AM.AssetTypeID = " & Request("AssetTypeID") & " AND AM.ID = AT.AssetID "
            sql = sql & " AND Date1 >= DATEADD(day,-(" & TransDays & "),getdate()) "
            sql = sql & " AND ATM.AssetTypeID =  AM.AssetTypeID  GROUP BY AT.TransType,AT.TransType,ATM.AssetTypeCode,AM.AssetTypeID,ATM.CatID"
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                Dim strtable As New Table
                strtable.ID = "strtable1"
                strtable.Width = Unit.Percentage(100)
                While rdr.Read
                    Dim strrow As New TableRow
                    Dim strrow1 As New TableRow
                    Dim strCell_1 As New TableCell
                    Dim strCell_2 As New TableCell
                    Dim gridview1 As New GridView
                    Dim lbl As New Label
                    gridview1.ID = "grid1" & rdr("AssetTypeID")
                    lbl.ID = "lbl" & rdr("AssetTypeID")
                    strCell_2.CssClass = "trheaderbg"
                    lbl.Text = rdr("AssetTypeCode") & "-" & rdr("TransType")
                    strCell_2.HorizontalAlign = HorizontalAlign.Left
                    strCell_2.Controls.Add(lbl)
                    strrow1.Cells.Add(strCell_2)
                    gridview1.CssClass = "mGrid"
                    gridview1.GridLines = GridLines.None
                    gridview1.AllowPaging = False
                    gridview1.AutoGenerateColumns = False
                    gridview1.Width = Unit.Percentage(100)
                    gridview1.AlternatingRowStyle.CssClass = "alt"
                    dtable = New DataTable
                    dtable = LoadDetailsView(GetRequiredFields(Request("AssetTypeID")), rdr("TType"), Request("AssetTypeID"))
                    If dtable.Rows.Count > 0 Then
                        gridview1.Columns.Clear()
                        gridview1.DataSource = dtable
                        For i As Integer = 0 To dtable.Columns.Count - 1
                            Dim s As New BoundField
                            If i = 0 Then
                                s.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                            End If
                            s.DataField = dtable.Columns(i).ToString
                            s.HeaderText = dtable.Columns(i).ToString
                            gridview1.Columns.Add(s)
                        Next
                        gridview1.DataBind()
                        strCell_1.Controls.Add(gridview1)
                        strrow.Cells.Add(strCell_1)
                        strtable.Rows.Add(strrow1)
                        strtable.Rows.Add(strrow)
                    End If
                End While
                tddata.Controls.Add(strtable)
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function LoadDetailsView(ByVal sql As String, ByVal transtype As String, ByVal assettypeid As String) As DataTable
        If con1.State = ConnectionState.Open Then
            con1.Close()
        End If
        con1.Open()
        sql = sql & "  FROM tbl_Asset_Master am, tbl_Asset_Transactions tr WHERE am.AssetTypeID = " & assettypeid & " And am.ID = tr.AssetID And tr.TransType =" & transtype & " "
        sql = sql & "  AND tr.date1 >= DATEADD(day,-(" & TransDays & "),getdate()) "
        sqladr = New SqlDataAdapter(sql, con1)
        dtable = New DataTable
        sqladr.Fill(dtable)
        Return dtable
        con1.Close()
    End Function
    Public Function GetRequiredFields(ByVal assettypeid As String) As String
        Try
            Dim sql As String
            sql = "Select  row_number() over (order by am.id) as [S.No],"
            If con1.State = ConnectionState.Open Then
                con1.Close()
            End If
            con1.Open()
            cmd1 = New SqlCommand("select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aa.IsRequired = 1 and aad.attid = aa.attid  and aad.AssetTypeId = " & assettypeid & " order by aad.attid asc", con1)
            rdr1 = cmd1.ExecuteReader
            If rdr1.HasRows Then
                While rdr1.Read
                    sql = sql & "am." & rdr1("FieldOrder") & " as [" & rdr1("AttDesc") & "],"
                End While
            Else
                sql = sql & "* "
            End If
            cmd1.Dispose()
            rdr1.Close()
            con1.Close()
            sql = Left(sql, Len(sql) - 1)
            Return sql
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
End Class
