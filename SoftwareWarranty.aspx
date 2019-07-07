<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false"
    CodeFile="SoftwareWarranty.aspx.vb" Inherits="SoftwareWarranty" %>
    
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
                                    Software Renewal
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" ValidationGroup="warranty" />
                                </td>
                            </tr>
                            <tr class="whitebg" id="trmessage" runat="server" visible="false">
                                <td colspan="2" class="tdtext">
                                    <asp:Label ID="lblmessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="whitebg">
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
                                    <asp:TextBox ID="txtdate" runat="server" CssClass="control" ValidationGroup="warranty"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="required1" runat="server" ControlToValidate="txtdate"
                                        ValidationGroup="warranty" Display="None" ErrorMessage="Enter Date" SetFocusOnError="True"></asp:RequiredFieldValidator>
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
                                    <span style="">Software Type:</span>
                                </td>
                                <td align="left" class="tdtext">
                                    <asp:DropDownList ID="drpsoftwaretype" runat="server" CssClass="control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="right" class="tdtext">
                                    <span style="">Version :</span>
                                </td>
                                <td align="left" class="tdtext">
                                    <asp:DropDownList ID="drpversion" runat="server" CssClass="control">
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
                                                    ValidationGroup="warranty" />
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
