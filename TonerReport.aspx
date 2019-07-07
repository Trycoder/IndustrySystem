<%@ Page Title="Toner Report" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="TonerReport.aspx.vb" Inherits="TonerReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function winwidth() {
        winW = document.body.offsetWidth;
        document.getElementById("divmain").style.width = (winW - 100) + 'px';
       document.getElementById("table1").style.width = (winW - 100) + 'px';
    }
</script>
    <table border="0" align="center" cellpadding="0" cellspacing="0">
        <tr class="tdcolbg">
            <td>
                <table border="0" align="center" cellpadding="4" cellspacing="1" id="table1" >
                    <tr class="trheaderbg">
                        <td>
                            Printer - Toner Report
                        </td>
                        <td align="right"><asp:LinkButton ID="lnkexport" runat="server" ForeColor="Gold" Text="Export To Excel"></asp:LinkButton></td>
                    </tr>
                    <tr class="whitebg" runat="server" id="trExport">
                        <td colspan="2">
                        <div id="divmain" style="overflow:scroll;vertical-align:top;height:600px;">
                           <asp:GridView ID="grdattributes" runat="server" CssClass="mGrid" GridLines="None"
                                AlternatingRowStyle-CssClass="alt" AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false"
                                 EmptyDataText="No Data Found !">
                                <Columns>
                                </Columns>
                            </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
               
            </td>
        </tr>
    </table>
</asp:Content>
