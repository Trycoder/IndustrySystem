<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false"
    CodeFile="PrinterTonerMapping.aspx.vb" Inherits="PrinterTonerMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0" width="90%">
        <tr class="tdcolbg">
            <td>
                <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%" style="overflow: scroll;">
                    <tr class="trheaderbg">
                        <td colspan="2">
                            Toner Mapping with Printers
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td style="width: 50%;" align="right" class="tdtext">
                            Select Printer :
                        </td>
                        <td style="width: 50%;" class="tdtext">
                            <asp:DropDownList ID="ddlPrinter" runat="server" CssClass="control" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr class="whitebg">
            <td class="tdtext">
                <b>Toners :</b>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <div id="divTonerGrid" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnSave" runat="server" Visible="false" Text="Save"  CssClass="lButton"
                    Width="80px" />&nbsp;
                <asp:Button ID="btnCancel" runat="server" Visible="false" Text="Cancel" CssClass="lButton" Width="80px" />
            </td>
        </tr>
    </table>
</asp:Content>
