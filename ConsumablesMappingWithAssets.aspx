<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="ConsumablesMappingWithAssets.aspx.vb"
    Inherits="ConsumablesMappingWithAssets" Title="Consumables Mapping with Asset Types" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <table border="0" align="center" cellpadding="0" cellspacing="0" width="90%">
        <tr class="tdcolbg">
            <td>
                 <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
                    <tr class="trheaderbg">
                        <td colspan="2">
                            Consumables Mapping with Asset Types
                        </td>
                    </tr>
                </table>
                 <div id="divmain"  style="overflow:scroll;height:600px">
                             <script type="text/javascript">
                                 var w = screen.width;
                                 document.getElementById('divmain').style.width = w - 145;
                              </script>
                    <table width="100%"  id="table1" cellpadding="1" cellspacing="1">
                        <tr class="trheaderbg">
                            <th align="center" style="width: 3%; font-weight: bold;">
                                Cons Name
                            </th>
                            <%

                                Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
                                Dim cmd1 As New System.Data.SqlClient.SqlCommand
                                Dim rdr1 As System.Data.SqlClient.SqlDataReader
                                Dim str1 As String
                                Dim dtable As New System.Data.DataTable
                                dtable = GetAssetList()
                                'Dim I As Integer
                                If dtable.Rows.Count > 0 Then
                                    For j As Integer = 0 To dtable.Rows.Count - 1
                            %>
                            <th style="width: 5%; font-weight: bold;" align="center">
                                <% =dtable.Rows(j)("AssetTypeCode")%>
                            </th>
                            <%
                            Next
                        End If
                            %>
                        </tr>
                        <%
		   
                            Dim dtable1 As New System.Data.DataTable
                            Dim dtable2 As New System.Data.DataTable
                            dtable1 = GetConsumableList()
                            Dim altcolor As Integer = 1
                            If dtable1.Rows.Count > 0 Then
                                For j As Integer = 0 To dtable1.Rows.Count - 1
                        %>
                        <% If altcolor = 1 Then%>
                        <tr class="whitebg">
                            <% altcolor = 2%>
                            <% Else%>
                            <tr class="whitebg">
                                <% altcolor = 1%>
                                <%End If%>
                                <td class="tdtext">
                                    <% =dtable1.Rows(j)(1)%>
                                </td>
                                <%
                                    If dtable.Rows.Count > 0 Then
                                        For k As Integer = 0 To dtable.Rows.Count - 1
                                            con1.Open()
                                            str1 = "select * from tbl_Asset_cons_Mapping where  ConsTypeid =" & dtable1.Rows(j)("AssetTypeid") & " and AssetTypeId=" & dtable.Rows(k)("AssetTypeId")
                                            cmd1.CommandText = str1
                                            cmd1.Connection = con1
                                            rdr1 = cmd1.ExecuteReader
                                            If Not rdr1.HasRows Then
                                %>
                                <td align="center" class="tdtext">
                                    <input type="checkbox" id="chk<%=dtable1.rows(j)("AssetTypeid")& dtable.rows(k)("AssetTypeId") %>"
                                        name="chk<%=dtable1.rows(j)("AssetTypeid")& dtable.rows(k)("AssetTypeId") %>" />
                                </td>
                                <%Else%>
                                <td align="center" class="tdtext">
                                    <input type="checkbox" id="Checkbox1" checked="checked" name="chk<%=dtable1.rows(j)("AssetTypeid")& dtable.rows(k)("AssetTypeId") %>" />
                                </td>
                                <%  End If
                                    cmd1.Dispose()
                                    rdr1.Close()
                                    con1.Close()
		                    
                                %>
                                <%
                                Next
                            End If
                        Next
                    End If%>
                            </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="assetname" CssClass="lButton"
                    Width="80px" />&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width="80px" />
            </td>
        </tr>
    </table>
</asp:Content>
