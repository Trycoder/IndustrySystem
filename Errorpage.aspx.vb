Imports System.Data.SqlClient
Imports System.Data

Partial Class Employee
    Inherits System.Web.UI.Page
    Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim sqldr As SqlDataReader
    Dim rowid As String
    Dim dtable As DataTable
    Shared sortExpression As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgbtn As ImageButton = Master.FindControl("imgsearch")
        If imgbtn IsNot Nothing Then
            imgbtn.Focus()
        End If
        rowid = Request.QueryString("Id")
        If Not IsPostBack Then
            FillCombos("", "")
            If Not String.IsNullOrEmpty(rowid) Then
                FnDisplay(rowid)
            End If
            ViewState("sortOrder") = ""
            BindGrid("", "")
        End If
        If Session("Usergroup") <> "1" Then
            btnSave.Enabled = False
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If UCase(btnSave.Text) = UCase("Update") Then
            If Not String.IsNullOrEmpty(rowid) Then
                Dim Sql As String
                Dim str As String
                cmd = New SqlCommand
                cmd.Connection = con
                con.Open()
                str = "update tbl_material set matcode ='" & Trim(TxtMaterialCode.Text.Replace("'", "''")) & "',matdesc ='" & Trim(TxtMaterialName.Text.Replace("'", "''")) & "',"
                str = str & "supplierid = " & Val(CboSupplier.SelectedValue) & ",unitid1 = " & Val(CboUnit1.SelectedValue) & ","
                str = str & "unitid2 = " & Val(CboUnit2.SelectedValue) & ",minqty = " & Val(TxtMinQty.Text) & ","
                str = str & "reorderqty = " & Val(TxtReOrderQty.Text) & ",price1 = " & Val(TxtPrice1.Text) & ","
                str = str & "price2 = " & Val(TxtPrice2.Text) & ",u1u2 = " & Val(TxtU1U2.Text)
                str = str & " where matid = " & rowid & """"
                cmd.CommandText = str
                cmd.Connection = con
                cmd.ExecuteScalar()
                con.Close()
                BindGrid("", "")
                TxtMaterialCode.Text = ""
                TxtMaterialName.Text = ""
                TxtMinQty.Text = ""
                TxtReOrderQty.Text = ""
                TxtPrice1.Text = ""
                TxtPrice2.Text = ""
                TxtU1U2.Text = ""
                FillCombos("", "")
                btnSave.Text = "Save"
            End If
        Else
            Dim Sql As String
            cmd = New SqlCommand
            con.Open()
            cmd.Connection = con
            Sql = "Select * from tbl_material where matcode like '" & Trim(TxtMaterialCode.Text.Replace("'", "''")) & "'  "
            cmd.CommandText = Sql
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                Dim myscript As String = "alert('Material Code already exists.. ');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                TxtMaterialCode.Focus()
                TxtMaterialCode.Text = ""
                Exit Sub
            End If
            con.Close()
            cmd.Dispose()
            Dim str As String
            con.Open()
            str = "insert into tbl_material(matcode,matdesc,supplierid,unitid1,unitid2,minqty,reorderqty,price1,price2,u1u2) values ('"
            str = str & TxtMaterialCode.Text.Replace("'", "''") & "','" & TxtMaterialName.Text.Replace("'", "''") & "',"
            str = str & Val(CboSupplier.SelectedValue) & "," & Val(CboUnit1.SelectedValue) & "," & Val(CboUnit2.SelectedValue) & ","
            str = str & Val(TxtMinQty.Text) & "," & Val(TxtReOrderQty.Text) & "," & Val(TxtPrice1.Text) & "," & Val(TxtPrice2.Text) & "," & Val(TxtU1U2.Text) & ")"
            cmd.CommandText = str
            cmd.Connection = con
            cmd.ExecuteScalar()
            con.Close()
            BindGrid("", "")
            TxtMaterialCode.Text = ""
            TxtMaterialName.Text = ""
            TxtMinQty.Text = ""
            TxtReOrderQty.Text = ""
            TxtPrice1.Text = ""
            TxtPrice2.Text = ""
            TxtU1U2.Text = ""
            FillCombos("", "")
        End If
    End Sub
    Private Function bindCategoryDetails(ByVal id As String)
        Try
            con.Open()
            cmd = New SqlCommand("select m.*,s.suppliercode,u.unitcode as unit1,(select unitcode from tbl_unitmaster where id = m.unitid2) as unit2 from tbl_material m,tbl_unitmaster u, tbl_supplier s where s.SupplierId = m.supplierid and u.id = m.unitid1 and m.matid = " & id & " order by m.matcode", con)
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read Then
                    TxtMaterialCode.Text = rdr("matcode")
                    TxtMaterialName.Text = rdr("matdesc")
                    CboSupplier.SelectedValue = rdr("supplierid")
                    CboUnit1.SelectedValue = rdr("unitid1")
                    CboUnit2.SelectedValue = rdr("unitid2")
                    TxtMinQty.Text = rdr("minqty")
                    TxtReOrderQty.Text = rdr("reorderqty")
                    TxtPrice1.Text = rdr("price1")
                    TxtPrice2.Text = rdr("price2")
                    TxtU1U2.Text = rdr("u1u2")
                    LblUnit1P.Text = " / " & rdr("unit1")
                    LblUnit1C.Text = rdr("unit1") & " X "
                    LblUnit2P.Text = " / " & rdr("unit2")
                    LblUnit2C.Text = " = " & rdr("unit2")

                End If
            End If
            con.Close()
            btnSave.Text = "Update"
            BindGrid("", "")
        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Function

    

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub
    
    Sub FillCombos(ByVal sortExpression As String, ByVal direction As String, Optional ByVal condition As String = "")
        Try
            Dim sql As String
            sql = "select * from tbl_unitmaster order by unitcode"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            CboUnit1.Items.Clear()
            CboUnit1.Items.Add(New ListItem("--Select--", ""))
            CboUnit2.Items.Clear()
            CboUnit2.Items.Add(New ListItem("--Select--", ""))
            If rdr.HasRows Then
                While rdr.Read
                    CboUnit1.Items.Add(New ListItem(rdr("unitcode"), rdr("id")))
                    CboUnit2.Items.Add(New ListItem(rdr("unitcode"), rdr("id")))
                End While
            End If
            con.Close()



            sql = "select * from tbl_supplier order by suppliercode"
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()
            cmd = New SqlCommand(sql, con)
            rdr = cmd.ExecuteReader
            CboSupplier.Items.Clear()
            CboSupplier.Items.Add(New ListItem("--Select--", ""))
            If rdr.HasRows Then
                While rdr.Read
                    CboSupplier.Items.Add(New ListItem(rdr("suppliercode"), rdr("supplierid")))
                End While
            End If
            con.Close()


        Catch ex As Exception
            Dim errorscript As String = "alert('Error - " & ex.Message.ToString().Replace("'", "") & "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", errorscript, True)
        End Try
    End Sub

    Protected Sub CboUnit1_Load(sender As Object, e As EventArgs) Handles CboUnit1.Load

    End Sub

    Protected Sub CboUnit1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboUnit1.SelectedIndexChanged
        LblUnit1P.Text = " / " & CboUnit1.SelectedItem.Text
        LblUnit1C.Text = CboUnit1.SelectedItem.Text & " X "
    End Sub

    Protected Sub CboUnit2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboUnit2.SelectedIndexChanged
        LblUnit2P.Text = " / " & CboUnit2.SelectedItem.Text
        LblUnit2C.Text = " = " & CboUnit2.SelectedItem.Text
    End Sub
End Class
