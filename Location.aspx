<%@ Page Language="VB" MasterPageFile="~/Master.master" EnableViewState="true" AutoEventWireup="false" CodeFile="Location.aspx.vb" Inherits="Location" title="Location Master" %>

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
            document.getElementById(getControlID('txtcboLocation')).value=(tablerw.cells[1].childNodes[0].data);
        }
    </script>
    <table cellpadding='0' cellspacing='0' border='0' width="90%" align="center">
        <tr class="tdcolbg">
            <td>
                <table cellpadding='4' cellspacing='1' border='0' width='100%' align='center'>
                    <tr class="trheaderbg">
                        <td align="left" colspan="2">Add New Location
                          <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
            ShowMessageBox="True" ShowSummary="False" ValidationGroup="location" />
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext" style="width:50%">
                            Location1 :
                        </td>
                        <td class="tdtext style="width:50%">
                            <asp:DropDownList ID="cboLocation1" runat="server" Width="150" class="control" AutoPostBack="true">
                                <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">SIP-YG</asp:ListItem>
                                <asp:ListItem Value="2">SIP-CG</asp:ListItem>
                                <asp:ListItem Value="3">SIP-IH</asp:ListItem>
                                <asp:ListItem Value="4">NDPC</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext" style="padding-left:50px;">
                            Location2 :    
                        </td>
                        <td>    
                            <asp:DropDownList ID="cboLocation2" runat="server" Width="200" class="control" AutoPostBack="true">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext">
                            Location3 :
                         </td>
                         <td>
                            <asp:TextBox ID="txtcboLocation" class="control" Width="150" runat="server"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtcboLocation" Display="None" ErrorMessage="Enter Sub Location" 
                SetFocusOnError="True" ValidationGroup="location"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="padding-top:10px;">
            <td align="center">
                <asp:Button ID="btnSave" runat="server" Text="Save" class="lButton" Width="100" ValidationGroup="location"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="lButton" Width="100"/>
            </td>
        </tr>
    </table>
    <br />
    <%If cboLocation2.SelectedIndex > 0 Then%>
    <table cellpadding='0' cellspacing='0' border='0' width="90%" align="center" id="myTable">
        <tr class="tdcolbg">
            <td>              
                <asp:Panel ID="pnl" runat="server" Height="500px">
                    <div style="height:500px;overflow:scroll;">
                        <table cellpadding='4' cellspacing='1' border='0' width='100%' align='center'>
                            <tr class="trheaderbg">
                                <td align="center">S.No</td>
                                <td align="center">Sub Location Name</td>
                            </tr>
                            <%  Dim objDB As New DBFunctions
                            Dim ds As New DataSet
                            Dim sql As String
                            sql = "SELECT ROW_NUMBER() OVER (ORDER BY Sub.LocID ASC) AS SN,sub.SublocName " & _
                                    "FROM tbl_Asset_Sublocation sub,tbl_Asset_location ad " & _
                                    "WHERE sub.locid = ad.locid AND sub.locID = '" & cboLocation2.SelectedValue & "'"
                            ds = objDB.FillDataset(sql)
                            If ds.Tables(0).Rows.Count > 0 Then
                                For i = 0 To ds.Tables(0).Rows.Count - 1%>
                               <tr class="whitebg" onclick="ChangeColor(this);">
                               <%For j = 0 To ds.Tables(0).Columns.Count - 1%>
                                    <td align="center" class="tdtext"><%=ds.Tables(0).Rows(i)(j).ToString()%></td>
                               <%Next%>
                               </tr>
                            <%Next
                            End If%>
                        </table>
                    </div>
                </asp:Panel>
             </td>
        </tr>
    </table>
    <%End If%>
</asp:Content>

