<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false"
    CodeFile="ConsumableReturn.aspx.vb" Inherits="ConsumableReturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0" width="90%">
        <tr class="tdcolbg">
            <td>
                <asp:UpdatePanel ID="upanel3" runat="server">
                    <ContentTemplate>
                        <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
                            <tr class="trheaderbg">
                                <td colspan="4">
                                    <asp:Label ID="lbltransactions" runat="server"></asp:Label>&nbsp;Consumables Return
                                </td>
                            </tr>
                            <tr class="whitebg" runat="server" id="trmessage" visible="false">
                                <td colspan="4">
                                    <asp:Label ID="lblmessage" runat="server" class="red_text"></asp:Label>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="right" class="tdtext" style="width: 50%">
                                    UnDeploy From :
                                </td>
                                <td class="tdtext" align="left" colspan="3" style="width: 50%">
                                    <asp:RadioButtonList ID="rdoundeploytype" runat="server" CssClass="control" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="U" Selected>User</asp:ListItem>
                                        <asp:ListItem Value="P">Printer</asp:ListItem>
                                        <asp:ListItem Value="A">Asset</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="right" class="tdtext" style="width: 50%">
                                    Select Asset/User :
                                </td>
                                <td class="tdtext" align="left" colspan="3" style="width: 50%">
                                    <asp:DropDownList ID="drpuser" CssClass="control" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" PromptCssClass="ListExtend"
                                        TargetControlID="drpuser">
                                    </asp:ListSearchExtender>
                                    <asp:DropDownList ID="drpassettype" CssClass="control" runat="server" Visible="false"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="drpassets" CssClass="control" runat="server" Visible="false"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" PromptCssClass="ListExtend"
                                        TargetControlID="drpassets">
                                    </asp:ListSearchExtender>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td align="center" colspan="4" class="tdtext">
                                    <asp:GridView ID="grdassets" runat="server" DataKeyNames="id" CssClass="mGrid" GridLines="None"
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
                                            <asp:TemplateField HeaderText="AssetType">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblassettype" CssClass="tdtext" runat="server" Text='<%# Eval("assettypecode")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cond#1">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcond1" runat="server" Text='<%# Eval("att1")%>' /></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cond#2">
                                                <ItemTemplate><asp:Label ID="lblcond2" runat="server" Text='<%# Eval("att2")%>' /></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cond#3">
                                                <ItemTemplate><asp:Label ID="lblcond3" runat="server" Text='<%# Eval("att3")%>' /></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty">
                                                <ItemTemplate><asp:Label ID="lblqty" runat="server" Text='<%# Eval("qty")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rtn. Qty">
                                                <ItemTemplate><asp:TextBox ID="txtqty" runat="server" CssClass="control" Width="50px" Text='<%# Eval("qty")%>' /></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Complaint By" DataField="EmpName" SortExpression="EmpName" />
                                            <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpundeploytype" runat="server" CssClass="control">
                                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="S">To Stock</asp:ListItem>
                                                        <asp:ListItem Value="E">Expired</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="125px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drplocation" runat="server" CssClass="control" AutoPostBack="true"
                                                        Width="200px" OnSelectedIndexChanged="drplocation_SelectedIndexChanged">
                                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:ListSearchExtender ID="ListSearchExtender12" runat="server" PromptCssClass="ListExtend"
                                                        TargetControlID="drplocation">
                                                    </asp:ListSearchExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="125px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sub Location">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpsublocation" runat="server" CssClass="control" Width="200px">
                                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:ListSearchExtender ID="ListSearchExtender13" runat="server" PromptCssClass="ListExtend"
                                                        TargetControlID="drpsublocation">
                                                    </asp:ListSearchExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="125px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Select</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkapproved" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblconsid" runat="server" Text='<%# Eval("constypeid")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnsave" runat="server" Text="Save" ValidationGroup="assetname" CssClass="lButton"
                                        Width="80px" />&nbsp;
                                    <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="lButton" Width="80px" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
