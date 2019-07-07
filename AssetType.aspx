<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="AssetType.aspx.vb" Inherits="AssetType" title="Asset Type Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <script language="javascript" type="text/javascript">
        function ChangeColor(tablerw)
        {
            var i;
            var table = document.getElementById('myTable');
            var rows = table.getElementsByTagName("tr");
            for (i=2; i<rows.length; i++)
            {            
                rows[i].style.backgroundColor = '#FFFFFF';
            }    
            tablerw.style.backgroundColor = '#E6E8FA';
            document.getElementById(getControlID('txtAssetCode')).value=(tablerw.cells[1].childNodes[0].data);
            document.getElementById(getControlID('txtAssetDesc')).value=(tablerw.cells[2].childNodes[0].data);
        }
    </script>
    
    <asp:HiddenField ID="HiddenField1" runat="server" />    
    <table cellpadding='0' cellspacing='0' border='0' width="90%" align="center">
        <tr class="tdcolbg">
            <td>
                <table cellpadding='4' cellspacing='1' border='0'  width='100%' align='center'>
                    <tr class="trheaderbg">
                        <td align="left" colspan="2">Add New/Update Asset Type</td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext" style="width:50%;">
                            Category Name :
                         </td>
                         <td  style="width:50%;">
                            <asp:DropDownList ID="cboCategoryName" class="control" Width="120" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="--Select--"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext">
                            Asset Code :
                         </td>
                         <td>
                            <asp:TextBox ID="txtAssetCode" class="control" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext">
                            Asset Description :
                         </td>
                         <td>
                            <asp:TextBox ID="txtAssetDesc" class="control" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="padding-top:10px;">
            <td align="center">
                <asp:Button ID="btnSave" runat="server" Text="Save" class="lButton" Width="100"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="lButton" Width="100"/>
            </td>
        </tr>
    </table>
    <br />
    <%If cboCategoryName.SelectedIndex > 0 Then%>
    <table cellpadding='0' cellspacing='0' border='0' width="90%" align="center" id="myTable">
        <tr class="tdcolbg">
            <td>
                <table cellpadding='4' cellspacing='1' border='0' width='100%' align='center'>
                    <tr class="trheaderbg">
                        <td align="center">S.No</td>
                        <td align="center">Asset Code</td>
                        <td align="center">Asset Description</td>
                    </tr>
                    <%  Dim objDB As New DBFunctions
                    Dim ds As New DataSet
                    Dim sql As String
                        sql = "SELECT ROW_NUMBER() OVER (ORDER BY CatID ASC) AS SN,AssetTypeCode,AssetTypeDesc " & _
                                "FROM tbl_Asset_TypeMaster WHERE CatID= '" & cboCategoryName.SelectedValue & "'"
                    ds = objDB.FillDataset(sql)
                    Dim altcolor As Integer = 1
                    If ds.Tables(0).Rows.Count > 0 Then
                        For i = 0 To ds.Tables(0).Rows.Count - 1%>
                         <% If altcolor = 1 then  %>
                       <tr class="whitebg" onclick="ChangeColor(this);">
                             <%altcolor = 2 %>
		                  <%else %>
		                      <%altcolor = 1 %>
		                      <tr class="whitebg" onclick="ChangeColor(this);" style="background-color:#E6E8FA;">
		                 <%End If %>
                       <%For j = 0 To ds.Tables(0).Columns.Count - 1%>
                            <td align="center" class="tdtext"><%=ds.Tables(0).Rows(i)(j).ToString()%></td>
                       <%Next%>
                       </tr>
                    <%Next
                    End If%>
                </table>
            </td>
        </tr>
    </table>
    <%End If%>
</asp:Content>

