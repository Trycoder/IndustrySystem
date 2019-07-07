<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false"
    CodeFile="ConsumableDetailsReport1.aspx.vb" Inherits="ConsumableDetailsReport1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
<ContentTemplate>
    <table border="0" cellpadding="0" cellspacing="0" width="90%" align="center">
        <tr class="trheaderbg">
            <td colspan="2" style="line-height: 25px;">
                <div>
                    User Assets Reports</div>
            </td>
        </tr>
        <tr class="tdcolbg">
            <td>
                <table width="100%">
                    <tr class="whitebg">
                        <td class="tdtext" align="center">
                            Select Employee : &nbsp;&nbsp;
                            <asp:DropDownList ID="drpemployee" CssClass="control" runat="server" AutoPostBack="true"
                                Width="300px">
                            </asp:DropDownList>
                              <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" PromptCssClass="ListExtend"
                                        TargetControlID="drpemployee">
                                    </asp:ListSearchExtender>
                            &nbsp;&nbsp;&nbsp;
                            </td>
                    </tr>
                    <tr class="whitebg" runat="server" id="trAssets" visible="false">
                        <td class="tdtext">
                           Assets:
                            <asp:GridView ID="grdassets" runat="server" CssClass="mGrid" GridLines="None" AlternatingRowStyle-CssClass="alt"
                                AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false" Width="100%">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            S.No</HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Asset Name" DataField="assettypecode" SortExpression="assettypecode" />
                                    <asp:BoundField HeaderText="Asset No" DataField="AssetNo" SortExpression="AssetNo" />
                                    <asp:BoundField HeaderText="Fin Asset No" DataField="FinAssetNo" SortExpression="FinAssetNo" />
                                    <asp:BoundField HeaderText="Order No" DataField="OrderNo" SortExpression="OrderNo" />
                                    <asp:BoundField HeaderText="Warranty Start" DataField="Warrantystart" SortExpression="Warrantystart" />
                                    <asp:BoundField HeaderText="Warranty End" DataField="warrantyend" SortExpression="warrantyend" />
                                </Columns>
                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr class="whitebg" runat="server" id="trconsumables" visible="false">
                        <td class="tdtext">
                            Consumables:
                            <asp:GridView ID="grdconsumables" runat="server" CssClass="mGrid" GridLines="None"
                                AlternatingRowStyle-CssClass="alt" AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false"
                                Width="100%" EmptyDataText="No Data Found !">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            S.No</HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Asset Name" DataField="assettypecode" SortExpression="assettypecode" />
                                    <asp:BoundField HeaderText="Cond#1" DataField="att1" SortExpression="att1" />
                                    <asp:BoundField HeaderText="Cond#2" DataField="att2" SortExpression="att2" />
                                    <asp:BoundField HeaderText="Cond#3" DataField="att3" SortExpression="att3" />
                                    <asp:BoundField HeaderText="Deploy Type" DataField="deploytype" SortExpression="deploytype" />
                                    <asp:BoundField HeaderText="Req Date" DataField="reqdate" SortExpression="reqdate" />
                                    <asp:BoundField HeaderText="Approved By" DataField="ApprovedBy" SortExpression="ApprovedBy" />
                                    <asp:BoundField HeaderText="Issued By" DataField="IssuedBy" SortExpression="IssuedBy" />
                                    <asp:BoundField HeaderText="Quantity" DataField="qty" SortExpression="qty" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
