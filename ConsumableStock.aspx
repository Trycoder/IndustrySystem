<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="ConsumableStock.aspx.vb" Inherits="ConsumableStock" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding='0' cellspacing='0' border='0' width="90%" align="center">
        <tr class="tdcolbg">
            <td>
                <table cellpadding='4' cellspacing='1' border='0' width='100%' align='center'>
                    <tr class="trheaderbg">
                        <td align="left">Consumables List</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="padding-top:10px;">
            <td align="center">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" class="lButton" Width="100"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="lButton" Width="100"/>
            </td>
        </tr>
    </table>
</asp:Content>

