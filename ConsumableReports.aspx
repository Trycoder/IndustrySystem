<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false"
    CodeFile="ConsumableReports.aspx.vb" Inherits="ConsumableReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="90%" align="center">
        <tr class="trheaderbg">
            <td colspan="2" style="line-height: 25px;">
                <div>
                    Consumable Reports</div>
            </td>
        </tr>
        <tr class="tdcolbg">
            <td>
                <table border="0" align="left" width="100%" cellpadding="0" cellspacing="1">
                    <tr class="whitebg">
                        <td class="tdtext" style="padding: 5px;">
                            <asp:Panel runat="server" GroupingText="Consumable Reports">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkconsreport1" runat="server" Text="Consumable Position Report"> </asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkconsreport2" runat="server" Text="View Consumables Stock" PostBackUrl="~/ConsumableStockList.aspx"></asp:LinkButton>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkconsreport3" runat="server" Text="User Assets" ></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                              <asp:Panel ID="Panel2" runat="server" GroupingText="Consumables With User(Summary)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdconssummary" runat="server" CssClass="mGrid" GridLines="None"
                                                AlternatingRowStyle-CssClass="alt" AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false"
                                                Width="100%" EmptyDataText="No Data Found !" DataKeyNames="constypeid">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            S.No</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1%></ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="AssetType" SortExpression="assettypecode">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkassetcode" CssClass="tdtext" runat="server" Text='<%# Eval("assettypecode")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Quantity" DataField="Quantity" SortExpression="Quantity" ItemStyle-HorizontalAlign="Center" />
                                                </Columns>
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
