<%@ Page Title="Warranty Bulk Update" Language="VB" MasterPageFile="~/Master.master"
    AutoEventWireup="false" CodeFile="WarrantyBulkUpdate.aspx.vb" Inherits="WarrentyBulkUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upanel1" runat="server">
        <ContentTemplate>
            <table border="0" align="center" cellpadding="0" cellspacing="0" width="90%">
                <tr class="tdcolbg">
                    <td>
                        <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
                            <tr class="trheaderbg">
                                <td colspan="2">
                                    Update Warranty
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" ValidationGroup="warrenty" />
                                </td>
                            </tr>
                            <tr class="whitebg" id="trmessage" runat="server" visible="false">
                                <td colspan="2" class="tdtext">
                                    <asp:Label ID="lblmessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td style="width: 40%;" align="right" class="tdtext">
                                    Select By :
                                </td>
                                <td style="width: 60%;" class="tdtext">
                                    <asp:RadioButtonList ID="chkselectby" runat="server" AutoPostBack="true" CssClass="control" RepeatDirection="Horizontal" RepeatColumns="3">
                                    <asp:ListItem Value="P" Selected>Purchase Order Wise</asp:ListItem>
                                    <asp:ListItem Value="A">Asset Wise</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="whitebg" runat="server" id="trasset" visible="false">
                                <td style="width: 40%;" align="right" class="tdtext">
                                    Select Asset No :
                                </td>
                                <td style="width: 60%;" class="tdtext">
                                    <asp:DropDownList ID="drpasset" runat="server" AutoPostBack="True" CssClass="control">
                                    </asp:DropDownList>
                                    <asp:ListSearchExtender ID="ListSearchExtender1" runat="server"  PromptCssClass="ListExtend" TargetControlID="drpasset" ></asp:ListSearchExtender >
                                </td>
                            </tr>
                            <tr class="whitebg" runat="server" id="trpo" visible="false">
                                <td style="width: 40%;" align="right" class="tdtext">
                                    Select Purchase Order :
                                </td>
                                <td style="width: 60%;" class="tdtext">
                                    <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="True" CssClass="control">
                                        <asp:ListItem Text="--Select--"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="right" class="tdtext">
                                    <span style="">Warranty End Date :</span>
                                </td>
                                <td align="left" class="tdtext">
                                    <asp:TextBox ID="txtdate" runat="server" CssClass="control" ValidationGroup="warrenty"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="required1" runat="server" ControlToValidate="txtdate"
                                        ValidationGroup="warrenty" Display="None" ErrorMessage="Enter Date" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtdate"
                                        Format="dd-MMM-yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="right" class="tdtext">
                                    <span style="">Contrct Number :</span>
                                </td>
                                <td align="left" class="tdtext">
                                    <asp:TextBox ID="txtcontractno" runat="server" CssClass="control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="right" class="tdtext">
                                    <span style="">Contrct Vendor :</span>
                                </td>
                                <td align="left" class="tdtext">
                                    <asp:DropDownList ID="drpcontractvendor" runat="server" CssClass="control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="right" class="tdtext">
                                    <span style="">Remarks :</span>
                                </td>
                                <td align="left" class="tdtext">
                                    <asp:TextBox ID="txtremarks" runat="server" CssClass="control" TextMode="MultiLine"
                                         Height="50px" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="center" colspan="2" runat="server" id="tddata">
                                </td>
                            </tr>
                            <tr class="whitebg" runat="server" id="trbutton" visible="false">
                                <td colspan="2">
                                    <table style="width: 100%">
                                        <tr>
                                            <td class="tdtext" align="center">
                                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="lButton" Width="80px"
                                                    ValidationGroup="warrenty" />
                                                <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="lButton" Width="80px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
