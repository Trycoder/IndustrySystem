Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.DataVisualization.Charting

Partial Class Charts
    Inherits System.Web.UI.Page
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Dim str As String = ""
    Dim assettypes As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                BindAssetType()
            Else
                divChartTitle.InnerText = ""
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
    '    Try
    '        Dim dtable As New DataTable()
    '        'If drpassets.SelectedValue = "" Then
    '        '    Dim scrip1 As String = "alert('Please Select Asset Type');"
    '        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", scrip1, True)
    '        'End If
    '        If lstassets.SelectedValue = "" Then
    '            Dim scrip1 As String = "alert('Please Select Asset Type');"
    '            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", scrip1, True)
    '            Exit Sub
    '        End If
    '        If drpgroupby.SelectedValue = "" Then
    '            Dim scrip1 As String = "alert('Please Select group by');"
    '            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", scrip1, True)
    '            drpgroupby.Focus()
    '            Exit Sub
    '        End If
    '        Dim assettypes As String = ""
    '        For Each lst1 As ListItem In lstassets.Items
    '            If lst1.Selected = True Then
    '                assettypes &= "" & lst1.Value & ","
    '            End If
    '        Next

    '        If LblInvConText.Text = "" Then
    '            LblInvConText.Text = "1=1"
    '        End If

    '        assettypes = Left(assettypes, Len(assettypes) - 1)
    '        If assettypes.Length > 0 Then
    '            If UCase(drpgroupby.SelectedValue) = UCase("Department") Then
    '                Chart1.Titles("title1").Text = "Department wise Asset Count"
    '                str = "select count(v_asset.assetid) as yvalue,v_emp.Dep_Name + ' (' + convert(varchar,count(v_asset.assetid)) + ')' as xvalue from view_assetmaster_status v_asset "
    '                str = str & " left join view_sip_employees v_emp on v_asset.userid = v_emp.emp_Number where v_emp.emp_status = 'A' and v_emp.Dep_Name <> '' and "
    '                str = str & " v_asset.assettypeid in(" & assettypes & ") and " & LblInvConText.Text & " and v_asset.status <>'Sold'  group by v_emp.dep_Name order by v_emp.dep_Name"
    '            ElseIf UCase(drpgroupby.SelectedValue) = UCase("Building") Then
    '                Chart1.Titles("title1").Text = "Building wise Asset Count"
    '                str = "select count(v_asset.assettypeid) as yvalue,v_asset.buildingunit + ' (' + convert(varchar,count(v_asset.assettypeid)) + ')' as xvalue from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and v_asset.assettypeid in(" & assettypes & ") and " & LblInvConText.Text & "  and v_asset.buildingunit <> ''"
    '                str = str & "  and v_asset <>'Sold' group by v_asset.buildingunit order by v_asset.buildingunit asc"
    '            ElseIf UCase(drpgroupby.SelectedValue) = UCase("age") Then
    '                Chart1.Titles("title1").Text = "Age wise Asset Count"
    '                str = " select count(cat.groupid) as yvalue,'1 Year' + ' (' + convert(varchar,count(cat.groupid)) + ')' as xvalue "
    '                str = str & " from view_assetmaster_status v_asset, tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & "  "
    '                str = str & " and convert(datetime,v_asset.att5) >= DATEADD(year,-1,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
    '                str = str & " union all"
    '                str = str & " select count(cat.groupid) as yvalue,'1-2 Years' + ' (' + convert(varchar,count(cat.groupid)) + ')' as xvalue"
    '                str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
    '                str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-2,getdate()) and DATEADD(year,-1,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
    '                str = str & " union all"
    '                str = str & " select count(cat.groupid) as yvalue,'2-3 Years' + ' (' + convert(varchar,count(cat.groupid)) + ')' as xvalue"
    '                str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
    '                str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-2,getdate()) and DATEADD(year,-3,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
    '                str = str & " union all"
    '                str = str & " select count(cat.groupid) as yvalue,'3-4 Years' + ' (' + convert(varchar,count(cat.groupid)) + ')' as xvalue"
    '                str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where  v_asset.userid = v_emp.emp_number and v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
    '                str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-4,getdate()) and DATEADD(year,-3,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
    '                str = str & " union all"
    '                str = str & " select count(cat.groupid) as yvalue,'4-5 Years' + ' (' + convert(varchar,count(cat.groupid)) + ')' as xvalue"
    '                str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
    '                str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-5,getdate()) and DATEADD(year,-4,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
    '                str = str & " union all"
    '                str = str & " select count(cat.groupid) as yvalue,'5-6 Years' + ' (' + convert(varchar,count(cat.groupid)) + ')' as xvalue"
    '                str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
    '                str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-6,getdate()) and DATEADD(year,-5,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
    '                str = str & " union all"
    '                str = str & " select count(cat.groupid) as yvalue,'Above 6 Years' + ' (' + convert(varchar,count(cat.groupid)) + ')' as xvalue"
    '                str = str & " from view_assetmaster_status v_asset, tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
    '                str = str & " and convert(datetime,v_asset.att5) <= DATEADD(year,-6,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
    '            ElseIf UCase(drpgroupby.SelectedValue) = UCase("Make") Then
    '                Chart1.Titles("title1").Text = "Make wise Asset Count"
    '                str = "select count(v_asset.assettypeid) as yvalue,v_asset.att9 + ' (' + convert(varchar,count(v_asset.assettypeid)) + ')' as xvalue from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and  v_asset.assettypeid in(" & assettypes & ") and " & LblInvConText.Text & "  and v_asset.att9 <> ''"
    '                str = str & " and v_asset.status <> 'Sold' group by v_asset.att9 order by v_asset.att9 asc"
    '            ElseIf UCase(drpgroupby.SelectedValue) = UCase("Status") Then
    '                Chart1.Titles("title1").Text = "Status wise Asset Count"
    '                str = "select count(v_asset.assettypeid) as yvalue,v_asset.status + ' (' + convert(varchar,count(v_asset.assettypeid)) + ')' as xvalue from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and  v_asset.assettypeid in(" & assettypes & ") and " & LblInvConText.Text & "  and v_asset.status <> ''"
    '                str = str & " and v_asset.status <> 'Sold'  group by v_asset.status order by v_asset.status asc"
    '            End If
    '            If con.State = ConnectionState.Open Then
    '                con.Close()
    '            End If
    '            con.Open()
    '            Dim oAdapter As New SqlDataAdapter(str, con)
    '            oAdapter.Fill(dtable)
    '        End If
    '        If dtable.Rows.Count > 0 Then
    '            Dim yvalue As New List(Of Integer)()
    '            Dim xvalue As New List(Of String)()
    '            For i As Integer = 0 To dtable.Rows.Count - 1
    '                yvalue.Add(dtable.Rows(i)("yvalue"))
    '                xvalue.Add(dtable.Rows(i)("xvalue"))
    '            Next


    '            'pt.ToolTip = c.FirstName + " " + c.LastName + ": #VALY"
    '            'pt.LegendText = "#VALX: #VALY"
    '            'pt.LegendUrl = "/Contact/Details/" + dbaId.ToString()
    '            'pt.LegendToolTip = "Click to view " + c.FirstName + "'s contact information..."

    '            Dim yValues As Integer() = yvalue.ToArray
    '            Dim xvalues As String() = xvalue.ToArray
    '            Chart1.Series("piecharts").Points.DataBindXY(xvalues, yValues)
    '            Dim pt As DataPoint = Chart1.Series("piecharts").Points(0)
    '            pt.Url = "~/Default.aspx"
    '            pt.AxisLabel = "test"
    '            Chart1.Visible = True
    '            Chart1.DataSource = dtable
    '            Chart1.ChartAreas(0).Area3DStyle.Enable3D = True
    '            Chart1.Series("piecharts")("PieLabelStyle") = "Outside"
    '            Chart1.Series("piecharts").IsValueShownAsLabel = True
    '            'Chart1.Series("piecharts").XValueMember = dtable.Columns(1).ToString()
    '            'Chart1.Series("piecharts").YValueMembers = dtable.Columns(0).ToString()
    '            Chart1.Series("piecharts").XValueMember = dtable.Columns("xvalue").ToString()
    '            'Chart1.Series("piecharts")("PointWidth") = "5.5"
    '            ' Chart1.ChartAreas("ChartArea1").Area3DStyle.Inclination = 0
    '            'Chart1.Series("piecharts").Label = "#PERCENT"
    '            Chart1.Series("piecharts").YValueMembers = Val(dtable.Columns("yvalue").ToString())
    '            Chart1.Legends.Add(New Legend("xvalue"))


    '            Select Case UCase(drpgroupby.SelectedValue)
    '                Case "DEPARTMENT"
    '                    Chart1.Legends("xvalue").Title = "Departments"
    '                Case "BUILDING"
    '                    Chart1.Legends("xvalue").Title = "Buildings"
    '                Case "AGE"
    '                    Chart1.Legends("xvalue").Title = "Age"
    '                Case "MAKE"
    '                    Chart1.Legends("xvalue").Title = "Make"
    '                Case "STATUS"
    '                    Chart1.Legends("xvalue").Title = "Status"
    '            End Select

    '            'If UCase(drpgroupby.SelectedValue) = UCase("Department") Then
    '            '    Chart1.Legends("xvalue").Title = "Departments"
    '            'ElseIf UCase(drpgroupby.SelectedValue) = UCase("Building") Then
    '            '    Chart1.Legends("xvalue").Title = "Buildings"
    '            'ElseIf UCase(drpgroupby.SelectedValue) = UCase("age") Then
    '            '    Chart1.Legends("xvalue").Title = "Age"
    '            'ElseIf UCase(drpgroupby.SelectedValue) = UCase("Make") Then
    '            '    Chart1.Legends("xvalue").Title = "Make"
    '            'ElseIf UCase(drpgroupby.SelectedValue) = UCase("Status") Then
    '            '    Chart1.Legends("xvalue").Title = "Status"
    '            'End If


    '            Chart1.Series("piecharts").Legend = "xvalue"
    '            Chart1.Legends("xvalue").Docking = Docking.Right
    '            Chart1.Legends("xvalue").LegendStyle = LegendStyle.Column
    '            Chart1.AlignDataPointsByAxisLabel("piecharts", PointSortOrder.Ascending)
    '            'Chart1.Series("piecharts")("PieDrawingStyle") = "Concave"

    '            'Chart1.Series("piecharts").Points(8)("Exploded") = "true"
    '            ' Chart1.DataBind()
    '        End If
    '        ' LblInvConText.Text = ""
    '    Catch ex As Exception
    '        Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
    '    End Try
    'End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim sb As New StringBuilder
            Dim dtable As New DataTable()
            If lstassets.SelectedValue = "" Then
                Dim scrip1 As String = "alert('Please Select Asset Type');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", scrip1, True)
                Exit Sub
            End If
            If drpgroupby.SelectedValue = "" Then
                Dim scrip1 As String = "alert('Please Select group by');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", scrip1, True)
                drpgroupby.Focus()
                Exit Sub
            End If
            Dim assettypes As String = ""
            For Each lst1 As ListItem In lstassets.Items
                If lst1.Selected = True Then
                    assettypes &= "" & lst1.Value & ","
                End If
            Next

            If LblInvConText.Text = "" Then
                LblInvConText.Text = "1=1"
            End If

            assettypes = Left(assettypes, Len(assettypes) - 1)
            If assettypes.Length > 0 Then

                Session("Asset") = lstassets.SelectedItem.Text
                Session("GroupBy") = drpgroupby.SelectedValue
                Session("LblCondition") = LblInvConText.Text
                Session("AssetType") = assettypes
                Session("ConditionText") = LblCondText.Text

                If UCase(drpgroupby.SelectedValue) = UCase("Department") Then
                    divChartTitle.InnerText = "Department wise Asset Count"
                    str = "select count(v_asset.assetid) as yvalue,v_emp.Dep_Name as xvalue from view_assetmaster_status v_asset "
                    str = str & " left join view_sip_employees v_emp on v_asset.userid = v_emp.emp_Number where v_emp.emp_status = 'A' and v_emp.Dep_Name <> '' and "
                    str = str & " v_asset.assettypeid in(" & assettypes & ") and " & LblInvConText.Text & " and v_asset.status <>'Sold'  group by v_emp.dep_Name order by v_emp.dep_Name"
                ElseIf UCase(drpgroupby.SelectedValue) = UCase("Building") Then
                    divChartTitle.InnerText = "Building wise Asset Count"
                    str = "select count(v_asset.assettypeid) as yvalue,v_asset.buildingunit as xvalue from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and v_asset.assettypeid in(" & assettypes & ") and " & LblInvConText.Text & "  and v_asset.buildingunit <> ''"
                    str = str & "  and v_asset.status <>'Sold' group by v_asset.buildingunit order by v_asset.buildingunit asc"
                ElseIf UCase(drpgroupby.SelectedValue) = UCase("Age") Then
                    divChartTitle.InnerText = "Age wise Asset Count"
                    str = " select count(cat.groupid) as yvalue,'1 Year' as xvalue "
                    str = str & " from view_assetmaster_status v_asset, tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & "  "
                    str = str & " and convert(datetime,v_asset.att5) >= DATEADD(year,-1,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
                    str = str & " union all"
                    str = str & " select count(cat.groupid) as yvalue,'1-2 Years' as xvalue"
                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-2,getdate()) and DATEADD(year,-1,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
                    str = str & " union all"
                    str = str & " select count(cat.groupid) as yvalue,'2-3 Years' as xvalue"
                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-2,getdate()) and DATEADD(year,-3,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
                    str = str & " union all"
                    str = str & " select count(cat.groupid) as yvalue,'3-4 Years' as xvalue"
                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where  v_asset.userid = v_emp.emp_number and v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-4,getdate()) and DATEADD(year,-3,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
                    str = str & " union all"
                    str = str & " select count(cat.groupid) as yvalue,'4-5 Years' as xvalue"
                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-5,getdate()) and DATEADD(year,-4,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
                    str = str & " union all"
                    str = str & " select count(cat.groupid) as yvalue,'5-6 Years' as xvalue"
                    str = str & " from view_assetmaster_status v_asset , tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
                    str = str & " and convert(datetime,v_asset.att5) between DATEADD(year,-6,getdate()) and DATEADD(year,-5,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
                    str = str & " union all"
                    str = str & " select count(cat.groupid) as yvalue,'Above 6 Years' as xvalue"
                    str = str & " from view_assetmaster_status v_asset, tbl_asset_categorymaster cat,view_sip_employees v_emp where v_asset.userid = v_emp.emp_number and  v_asset.catid = cat.catid and  v_asset.assettypeid in (" & assettypes & ") and " & LblInvConText.Text & " "
                    str = str & " and convert(datetime,v_asset.att5) <= DATEADD(year,-6,getdate()) and v_asset.status <>'Sold' group by  cat.groupid"
                ElseIf UCase(drpgroupby.SelectedValue) = UCase("Make") Then
                    divChartTitle.InnerText = "Make wise Asset Count"
                    str = "select count(v_asset.assettypeid) as yvalue,v_asset.att9  as xvalue from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and  v_asset.assettypeid in(" & assettypes & ") and " & LblInvConText.Text & "  and v_asset.att9 <> ''"
                    str = str & " and v_asset.status <> 'Sold' group by v_asset.att9 order by v_asset.att9 asc"
                ElseIf UCase(drpgroupby.SelectedValue) = UCase("Status") Then
                    divChartTitle.InnerText = "Status wise Asset Count"
                    str = "select count(v_asset.assettypeid) as yvalue,v_asset.status as xvalue from view_assetmaster_status v_asset,view_sip_employees v_emp  where v_asset.userid = v_emp.emp_number and  v_asset.assettypeid in(" & assettypes & ") and " & LblInvConText.Text & "  and v_asset.status <> ''"
                    str = str & " and v_asset.status <> 'Sold'  group by v_asset.status order by v_asset.status asc"
                End If
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Open()
                Dim oAdapter As New SqlDataAdapter(str, con)
                oAdapter.Fill(dtable)
            Else
                divChartTitle.InnerText = ""
            End If

            If dtable.Rows.Count > 0 Then
                For Each dr As DataRow In dtable.Rows
                    Dim xvalue As String = dr.Item("xvalue")
                    Dim yvalue As Integer = Convert.ToInt32(dr.Item("yvalue"))
                    sb.Append("arrRange.push(new GroupRange('" & xvalue.ToString() & "', " & yvalue.ToString() & "));")
                Next
            End If

            Dim lt As New LiteralControl("<script type='text/javascript'>" & sb.ToString() & "drawChart();</script>")
            Me.Controls.Add(lt)

        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Private Function BindAssetType()
        Try
            Dim sql As String
            sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid and am.catid in(1,2,4)  order by am.AssetTypeCode asc"
            con.Open()
            ' drpassets.Items.Clear()
            ' drpassets.Items.Add(New ListItem("--Select--", ""))
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    ' drpassets.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                    lstassets.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Public Function BindCondition()
        Try
            'lblcondition.Text = drpgroupby.SelectedValue
            'drpconsvalue.SelectedIndex = 0
            'LblCondText.Text = ""
            'LblInvConText.Text = ""
            If UCase(drpCondition.SelectedValue) = UCase("Department") Then
                drpconsvalue.Width = Unit.Pixel(200)
                str = "select distinct dep_name as value1 from view_sip_employees where dep_name <>'' order by dep_name asc"
            ElseIf UCase(drpCondition.SelectedValue) = UCase("Building") Then
                str = "select distinct buildingunit as value1 from view_sip_employees where buildingunit <>'' order by buildingunit asc"
            ElseIf UCase(drpCondition.SelectedValue) = UCase("Age") Then
                drpconsvalue.Items.Clear()
                drpconsvalue.Items.Add(New ListItem("All", "All"))
                drpconsvalue.Items.Add(New ListItem("1Year", "1"))
                drpconsvalue.Items.Add(New ListItem("1year-2years", "1-2"))
                drpconsvalue.Items.Add(New ListItem("2years-3years", "2-3"))
                drpconsvalue.Items.Add(New ListItem("3years-4years", "3-4"))
                drpconsvalue.Items.Add(New ListItem("4years-5years", "4-5"))
                drpconsvalue.Items.Add(New ListItem("5years-6years", "5-6"))
                drpconsvalue.Items.Add(New ListItem("6years and above", "6"))
            ElseIf UCase(drpCondition.SelectedValue) = UCase("Make") Then
                For Each lst1 As ListItem In lstassets.Items
                    If lst1.Selected = True Then
                        assettypes &= "" & lst1.Value & ","
                    End If
                Next
                assettypes = Left(assettypes, Len(assettypes) - 1)
                str = "select distinct att9 as value1 from tbl_asset_master  where assettypeid in (" & assettypes & ") and att9 <> '' order by att9 asc"
            ElseIf UCase(drpCondition.SelectedValue) = UCase("Status") Then
                str = "select distinct status as value1 from view_assetmaster_status where status <> '' order by status asc"
            End If
            If str <> "" Then
                con.Open()
                drpconsvalue.Items.Clear()
                drpconsvalue.Items.Add(New ListItem("All", "All"))
                cmd = New SqlCommand(str, con)
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    While rdr.Read
                        drpconsvalue.Items.Add(New ListItem(rdr("value1"), rdr("value1")))
                    End While
                End If
                rdr.Close()
                con.Close()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Public Function GetAge(ByVal agevalue As String) As String
        Try
            Dim condition As String = ""
            If agevalue = "All" Then
                condition = ""
            ElseIf agevalue = "1" Then
                condition = "convert(datetime,v_asset.att5) >= DATEADD(year,-1,getdate())"
            ElseIf agevalue = "1-2" Then
                condition = "convert(datetime,v_asset.att5) between DATEADD(year,-2,getdate()) and DATEADD(year,-1,getdate())"
            ElseIf agevalue = "2-3" Then
                condition = "convert(datetime,v_asset.att5) between DATEADD(year,-3,getdate()) and DATEADD(year,-2,getdate())"
            ElseIf agevalue = "3-4" Then
                condition = "convert(datetime,v_asset.att5) between DATEADD(year,-4,getdate()) and DATEADD(year,-3,getdate())"
            ElseIf agevalue = "4-5" Then
                condition = "convert(datetime,v_asset.att5) between DATEADD(year,-5,getdate()) and DATEADD(year,-4,getdate())"
            ElseIf agevalue = "5-6" Then
                condition = "convert(datetime,v_asset.att5) between DATEADD(year,-6,getdate()) and DATEADD(year,-5,getdate())"
            ElseIf agevalue = "6" Then
                condition = "convert(datetime,v_asset.att5) <= DATEADD(year,-6,getdate())"
            End If
            Return condition
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub drpgroupby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpgroupby.SelectedIndexChanged
        If lstassets.SelectedValue = "" Then
            Dim errorscript1 As String = "alert('Please Select Asset Type');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript1, True)
        End If
        drpCondition.SelectedIndex = 0
        DrpANDOR.SelectedIndex = 0
        drpCondition.SelectedIndex = 0
        LblCondText.Text = ""
        LblInvConText.Text = ""
    End Sub

    Protected Sub DrpANDOR_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DrpANDOR.SelectedIndexChanged
        Try
            Dim condition As String = ""
            Dim usercondition As String = ""
            Dim conditionend As Boolean
            If conditionend = False Then
                LblInvConText.Text = ""
                If UCase(drpCondition.SelectedValue) = UCase("Age") Then
                    condition = condition & drpCondition.SelectedItem.Text & "='" & drpconsvalue.SelectedItem.Text & "' "
                    If drpconsvalue.SelectedValue = "All" Then
                        usercondition = "1=1"
                    Else
                        usercondition = GetAge(drpconsvalue.SelectedValue)
                    End If
                ElseIf UCase(drpCondition.SelectedValue) = UCase("Department") Then
                    condition = condition & drpCondition.SelectedItem.Text & "='" & drpconsvalue.SelectedItem.Text & "' "
                    If drpconsvalue.SelectedValue = "All" Then
                        usercondition = "1=1"
                    Else

                        usercondition = "v_emp.dep_name='" & drpconsvalue.SelectedValue & "'"
                    End If
                ElseIf UCase(drpCondition.SelectedValue) = UCase("Make") Then
                    condition = condition & drpCondition.SelectedItem.Text & "='" & drpconsvalue.SelectedItem.Text & "' "
                    If drpconsvalue.SelectedValue = "All" Then
                        usercondition = "1=1"
                    Else

                        usercondition = "v_asset.att9='" & drpconsvalue.SelectedValue & "'"
                    End If
                ElseIf UCase(drpCondition.SelectedValue) = UCase("Building") Then
                    condition = condition & drpCondition.SelectedItem.Text & "='" & drpconsvalue.SelectedItem.Text & "' "
                    If drpconsvalue.SelectedValue = "All" Then
                        usercondition = "1=1"
                    Else

                        usercondition = "v_asset.buildingunit='" & drpconsvalue.SelectedValue & "'"
                    End If
                ElseIf UCase(drpCondition.SelectedValue) = UCase("Status") Then
                    condition = condition & drpCondition.SelectedItem.Text & "='" & drpconsvalue.SelectedItem.Text & "' "
                    If drpconsvalue.SelectedValue = "All" Then
                        usercondition = "1=1"
                    Else

                        usercondition = "v_asset.status='" & drpconsvalue.SelectedValue & "'"
                    End If
                End If
            End If
            If DrpANDOR.SelectedValue = "0" Then
            ElseIf DrpANDOR.SelectedValue = "1" Then
                conditionend = True
                If condition <> "" Then
                    If Session("OR") = "True" Then
                        Session("OR") = "False"
                        condition = condition & ")"
                        usercondition = usercondition & ")"
                    End If
                End If
                LblCondText.Text = LblCondText.Text & condition
                LblInvConText.Text = LblInvConText.Text & usercondition
                If Trim(Right(LblCondText.Text, 4)) = "and" Then
                    LblCondText.Text = Left(LblCondText.Text, Len(LblCondText.Text) - 4)
                    LblInvConText.Text = Left(LblInvConText.Text, Len(LblInvConText.Text) - 4)
                End If
            ElseIf DrpANDOR.SelectedValue = "2" Then
                If Session("OR") = "True" Then
                    Session("OR") = "False"
                    condition = condition & ")"
                    usercondition = usercondition & ")"
                End If
                If condition <> "" Then
                    condition = condition & " and "
                    usercondition = usercondition & " and "
                End If

                LblCondText.Text = LblCondText.Text & condition
                LblInvConText.Text = LblInvConText.Text & usercondition

            ElseIf DrpANDOR.SelectedValue = "3" Then
                If Session("OR") <> "True" Then
                    Session("OR") = "True"
                    condition = "(" & condition
                    usercondition = "(" & usercondition
                End If
                If condition <> "" Then
                    condition = condition & " or "
                    usercondition = usercondition & " or "
                End If
                LblCondText.Text = LblCondText.Text & condition
                LblInvConText.Text = LblInvConText.Text & usercondition
            End If
            DrpANDOR.SelectedIndex = 0
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnclearcondtion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclearcondtion.Click
        drpCondition.SelectedIndex = 0
        DrpANDOR.SelectedIndex = 0
        drpconsvalue.SelectedIndex = 0
        LblCondText.Text = ""
        LblInvConText.Text = ""
    End Sub

    Protected Sub drpCondition_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpCondition.SelectedIndexChanged
        If lstassets.SelectedValue <> "" AndAlso drpCondition.SelectedValue <> "" Then
            LblCondText.Text = ""
            LblInvConText.Text = ""
            BindCondition()
        End If
    End Sub

    Protected Sub drpconsvalue_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpconsvalue.SelectedIndexChanged
        LblCondText.Text = ""
        LblInvConText.Text = ""
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Charts.aspx")
    End Sub

End Class
