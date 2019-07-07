Imports System.Data
Imports System.Data.SqlClient
Partial Class ChangeAssetStatus
    Inherits System.Web.UI.Page
    Private WithEvents CheckboxColumn As CheckBoxTemplate
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        If Not IsPostBack Then
            bindcategory()
            'chkall.Attributes.Add("onclick", "return ClientCheck();")
        End If
        If drpAssetType.SelectedValue <> "" Then
            If drpassetfrom.SelectedValue <> "" And drpassetto.SelectedValue <> "" Then
                tddata1.Visible = True
                tddata.Visible = True
                BindGrid()
            Else
                tddata1.Visible = False
                tddata.Visible = False
            End If
        End If
        If Session("Usergroup") <> "1" Then
            btnupdate.Enabled = False
        End If

    End Sub
    Private Function bindcategory()
        Dim sql As String
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        drpcategory.Items.Clear()
        drpcategory.Items.Add(New ListItem("--Select--", ""))
        cmd = New SqlCommand("select * from tbl_Asset_CategoryMaster where groupid = 1 order by catcode asc", con)
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                drpcategory.Items.Add(New ListItem(rdr("catcode"), rdr("catid")))
            End While
        End If
        rdr.Close()
        con.Close()
    End Function

    Protected Sub drpcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpcategory.SelectedIndexChanged
        Try
            If drpcategory.SelectedValue <> "" Then
                BindAssetType(drpcategory.SelectedValue)
            End If
            tddata.Visible = False
            trupdate.Visible = False
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Private Function BindAssetType(ByVal categoryid As String)
        Try
            Dim sql As String
            drpAssetType.Items.Clear()
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            drpAssetType.Items.Add(New ListItem("--Select--", ""))
            sql = "select am.AssetTypeId,am.AssetTypeCode,am.AssetTypeDesc,am.CatId,ac.catdesc from tbl_Asset_TypeMaster am join tbl_Asset_CategoryMaster ac on am.CatId = ac.catid where am.CatId=" & categoryid & " order by am.CatId"
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    drpAssetType.Items.Add(New ListItem(rdr("AssetTypeCode"), rdr("AssetTypeId")))
                End While
            End If
            rdr.Close()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    Protected Sub drpAssetType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAssetType.SelectedIndexChanged
        If drpcategory.SelectedValue <> "" Then
            If drpAssetType.SelectedValue <> "" Then
                BindAssetNo(drpAssetType.SelectedValue, drpcategory.SelectedValue)
            End If
        End If
        tddata1.Visible = False
        tddata.Visible = False
        trupdate.Visible = False
    End Sub
    Public Function BindAssetNo(ByVal assettypeid As String, ByVal catid As String)
        Dim sql As String
        dtable = New DataTable
        assetno.Visible = True
        Try
            sql = "select aa.Fieldorder from tbl_Asset_Attributes aa,tbl_Asset_Attribute_Details aad where aa.AttId = aad.attid and  aa.Header = 5 and aa.catid =" & catid
            'sql = "select top 1 " & fieldname & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & assettypeid & " and am.id = ast.Assetid and ast.status ='N' "
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
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
                sql = "select am.id,am." & s & " from  tbl_Asset_Master am,tbl_Asset_Status ast where am.AssetTypeid =" & assettypeid & " and am.id = ast.Assetid and ast.status ='N' order by am.id asc"
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader
                drpassetfrom.Items.Clear()
                drpassetto.Items.Clear()
                drpassetfrom.Items.Add(New ListItem("--Select--", ""))
                drpassetto.Items.Add(New ListItem("--Select--", ""))
                While rdr.Read
                    drpassetfrom.Items.Add(New ListItem(rdr(s), rdr("id")))
                    drpassetto.Items.Add(New ListItem(rdr(s), rdr("id")))
                End While
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            If drpcategory.SelectedValue <> "" And drpAssetType.SelectedValue <> "" Then
                If drpassetfrom.SelectedValue <> "" And drpassetto.SelectedValue <> "" Then
                    tddata.Visible = True
                    tddata1.Visible = True
                    BindGrid()
                    trupdate.Visible = True
                Else
                    tddata1.Visible = False
                    tddata.Visible = False
                    trupdate.Visible = False
                End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Sub chkall_indexchanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkall.CheckedChanged
        BindGrid()
        If chkall.Checked = True Then
            For Each row As GridViewRow In grdassets.Rows
                Dim Chk As CheckBox = row.FindControl("Chk")
                If Chk IsNot Nothing Then
                    If Chk.Enabled = True Then
                        Chk.Checked = True
                    End If
                End If
            Next
        Else
            For Each row As GridViewRow In grdassets.Rows
                Dim Chk As CheckBox = row.FindControl("Chk")
                If Chk IsNot Nothing Then
                    Chk.Checked = False
                End If
            Next
        End If

        'BindGrid()
    End Sub
    Public Function BindGrid()
        Try
            dtable = New DataTable
            dtable = LoadDetailsView()
            If dtable.Rows.Count > 0 Then
                grdassets.Columns.Clear()
                grdassets.DataSource = dtable
                For j As Integer = 0 To dtable.Columns.Count - 2
                    Dim s As New BoundField
                    s.DataField = dtable.Columns(j).ToString
                    s.HeaderText = dtable.Columns(j).ToString
                    grdassets.Columns.Add(s)
                Next
                'Dim chkboxfield As New CheckBoxField
                'chkboxfield.HeaderText = "Select"
                'chkboxfield.DataField = "id"
                'grdassets.Columns.Add(chkboxfield)

                Dim TemplatedColumn As New TemplateField()
                CheckboxColumn = New CheckBoxTemplate("id")
                TemplatedColumn.ItemTemplate = CheckboxColumn
                TemplatedColumn.HeaderText = "Select"
                grdassets.Columns.Add(TemplatedColumn)

                'For Each col As DataColumn In dtable.Columns
                '    'Declare the bound field and allocate memory for the bound field.
                '    Dim bfield As New BoundField()
                '    'Initalize the DataField value.
                '    bfield.DataField = col.ColumnName
                '    'Initialize the HeaderText field value.
                '    bfield.HeaderText = col.ColumnName
                '    'Add the newly created bound field to the GridView.
                '    grdassets.Columns.Add(bfield)
                'Next
                'Initialize the DataSource

                'Bind the datatable with the GridView.
                grdassets.DataBind()
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function
    Public Function LoadDetailsView() As DataTable
        Dim sql As String
        sql = "Select  "
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandType = Data.CommandType.Text
        '  cmd.CommandText = "select * from tbl_Asset_Attributes where CatId=" & catid & " order by attid"
        cmd.CommandText = "select * from tbl_Asset_Attribute_Details aad, tbl_Asset_Attributes aa where aa.IsRequired = 1 and aad.attid = aa.attid  and aad.AssetTypeId = " & drpAssetType.SelectedValue & " order by aad.attid asc"
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                'If rdr("AttributeName") = "WARRANTY START DATE" Then
                '    sql = sql & " Convert(VARCHAR,Field" & rdr("AttributePos") & ",103) as [" & rdr("AttributeName") & "],"
                'ElseIf rdr("AttributeName") = "WARRANTY END DATE" Then
                '    sql = sql & " Convert(VARCHAR,Field" & rdr("AttributePos") & ",103) as [" & rdr("AttributeName") & "],"
                'Else
                sql = sql & "am." & rdr("FieldOrder") & " as [" & rdr("AttDesc") & "],"
                ' End If
            End While
        Else
            sql = sql & "* "
        End If
        cmd.Dispose()
        rdr.Close()
        con.Close()
        sql = Left(sql, Len(sql) - 1)
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        If drpassetfrom.SelectedValue <> "" And drpassetto.SelectedValue <> "" Then
            sql = sql & " , am.id from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid and ast.status ='N' and am.AssetTypeid =" & drpAssetType.SelectedValue & " and am.id between " & drpassetfrom.SelectedValue & " and " & drpassetto.SelectedValue & " order by am.id ASC "
        Else
            sql = sql & " , am.id from tbl_Asset_Master am, tbl_Asset_Status ast where am.id=ast.assetid and ast.status ='N' and am.AssetTypeid =" & drpAssetType.SelectedValue & " order by am.id ASC "
        End If
        sqladr = New SqlDataAdapter(sql, con)
        dtable = New DataTable
        sqladr.Fill(dtable)
        Return dtable

    End Function
    Protected Sub grdassets_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdassets.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'Response.Write(e.Row.Cells.Count)
                Dim boolchk As Boolean = True
                For i As Integer = 0 To e.Row.Cells.Count - 2
                    If Trim(e.Row.Cells(i).Text.ToString()) = "&nbsp;" Or Trim(e.Row.Cells(i).Text.ToString()) = "" Then
                        boolchk = False
                    End If
                Next
                If boolchk = False Then
                    Dim Chk As CheckBox = e.Row.FindControl("Chk")
                    If Chk IsNot Nothing Then
                        Chk.Enabled = False
                    End If
                End If

                ' e.Row.Cells(grdassets.Columns.Count - 1).FindControl("Chk" & grdassets.DataKeys(e.Row.ID).Value)
                '   Chk.Enabled = False
                ''  If Chk.Checked = True Then
                'Chk.Visible = True
                'Else
                ' Chk.Visible = False
                ' End If
                'Response.Write(e.Row.Cells.Count)

                'If Trim(e.Row.Cells(3).Text) = "0" Then
                '    e.Row.Cells(3).Text = "Non-Mandatory"
                'ElseIf Trim(e.Row.Cells(3).Text) = "1" Then
                '    e.Row.Cells(3).Text = "Mandatory"
                'End If
                'If Trim(e.Row.Cells(4).Text) = "0" Then
                '    e.Row.Cells(4).Text = ""
                'ElseIf Trim(e.Row.Cells(4).Text) = "1" Then
                '    e.Row.Cells(4).Text = "PO Number"
                'ElseIf Trim(e.Row.Cells(4).Text) = "2" Then
                '    e.Row.Cells(4).Text = "PO Date"
                'ElseIf Trim(e.Row.Cells(4).Text) = "3" Then
                '    e.Row.Cells(4).Text = "Warranty Start Date"
                'ElseIf Trim(e.Row.Cells(4).Text) = "4" Then
                '    e.Row.Cells(4).Text = "Warranty End Date"
                'ElseIf Trim(e.Row.Cells(4).Text) = "5" Then
                '    e.Row.Cells(4).Text = "Primary"
                'End If
            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnupdate.Click
        Try
            Dim chkcount As Integer = 0
            If drpassetfrom.SelectedValue <> "" And drpassetto.SelectedValue <> "" Then
                For Each row As GridViewRow In grdassets.Rows
                    Dim Chk As CheckBox = row.FindControl("Chk")
                    If Chk IsNot Nothing Then
                        If Chk.Checked = True Then
                            Session("Update") = "Update"
                            UpdateAssetStatus(grdassets.DataKeys(row.DataItemIndex).Value)
                        Else
                            chkcount = chkcount + 1
                        End If
                    End If
                Next
                If chkcount = grdassets.Rows.Count Then
                    Dim myscript1 As String = "alert('Please Check any of the Assets! ');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                    BindGrid()
                Else
                    If drpAssetType.SelectedValue <> "" Then
                        BindAssetNo(drpAssetType.SelectedValue, drpcategory.SelectedValue)
                    End If
                    tddata1.Visible = False
                    tddata.Visible = False
                    trupdate.Visible = False
                End If
                If Session("Update") = "Update" Then
                    Dim myscript1 As String = "alert('Asset Status Changed Successfully! ');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript1, True)
                    Session("Update") = ""
                End If
                chkall.Checked = False

            End If
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub
    Public Function UpdateAssetStatus(ByVal assetid As String)
        Dim sql As String
        dtable = New DataTable
        assetno.Visible = True
        Try
            sql = "update tbl_Asset_Status set status='S',userid='0000'  where Assetid =" & assetid
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            con.Close()
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        Finally
            con.Close()
        End Try
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
End Class
Public Class CheckBoxTemplate
    Implements ITemplate
    Private m_ColumnName As String
    Event CheckBoxCheckedChanged(ByVal sender As CheckBox, ByVal e As EventArgs)
    Public Property ColumnName() As String
        Get
            Return m_ColumnName
        End Get
        Set(ByVal value As String)
            m_ColumnName = value
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Sub New(ByVal ColumnName As String)
        Me.ColumnName = ColumnName
    End Sub
    Private Sub InstantiateIn(ByVal ThisColumn As System.Web.UI.Control) _
            Implements ITemplate.InstantiateIn
        Dim CheckBoxItem As New CheckBox()
        CheckBoxItem.ID = "Chk" '& ColumnName
        'CheckBoxItem.AutoPostBack = True
        AddHandler CheckBoxItem.DataBinding, AddressOf CheckBoxItem_DataBinding
        AddHandler CheckBoxItem.CheckedChanged, AddressOf CheckBoxItem_CheckedChanged
        ThisColumn.Controls.Add(CheckBoxItem)
    End Sub
    Private Sub CheckBoxItem_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent CheckBoxCheckedChanged(sender, e)
    End Sub
    Private Sub CheckBoxItem_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim CheckBoxItem As CheckBox = DirectCast(sender, CheckBox)
        Dim CurrentRow As GridViewRow = DirectCast(CheckBoxItem.NamingContainer, GridViewRow)
        Dim CurrentDataItem As Object = DataBinder.Eval(CurrentRow.DataItem, ColumnName)
        If Not CurrentDataItem Is DBNull.Value Then
            ' CheckBoxItem.Checked = Convert.ToBoolean(CurrentDataItem)
        End If
    End Sub
End Class
