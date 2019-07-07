<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ConsumableDetailReport.aspx.vb"
    Inherits="ConsumableDetailReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Asset Details</title>
    <script src="JS/Common.js" type="text/javascript"></script>
    <link href="Css/ApplyCSS1.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="Css/grid.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="90%" align="center">
                <tr class="trheaderbg">
                    <td style="line-height: 25px;">
                        <div>
                            <asp:Label ID="lblmessage" runat="server"></asp:Label></div>
                    </td>
                </tr>
                <tr class="tdcolbg">
                    <td>
                        <% If Request.QueryString("RType") = "1" Then%>
                        <table width="100%">
                            <tr class="whitebg">
                                <td align="left" class="tdtext">
                                    Employee Name : &nbsp;&nbsp;
                                    <asp:DropDownList ID="drpemployee1" CssClass="control" runat="server" Width="300px">
                                    </asp:DropDownList>
                                    <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" PromptCssClass="ListExtend"
                                        TargetControlID="drpemployee1">
                                    </asp:ListSearchExtender>
                                    Consumable Type : &nbsp;&nbsp;
                                    <asp:DropDownList ID="drpconsumable1" CssClass="control" runat="server" Width="300px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="whitebg">
                                <td class="tdtext">
                                    Issue Between: &nbsp;
                                    <asp:TextBox ID="txtfromdate" runat="server" CssClass="control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtfromdate"
                                        Format="dd-MMM-yyyy">
                                    </asp:CalendarExtender>
                                    &nbsp; To &nbsp;
                                    <asp:TextBox ID="txttodate" runat="server" CssClass="control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txttodate"
                                        Format="dd-MMM-yyyy">
                                    </asp:CalendarExtender>
                                    &nbsp;&nbsp; Order by: &nbsp;&nbsp;
                                    <asp:DropDownList ID="drporderby" CssClass="control" runat="server">
                                        <asp:ListItem Value="U">Users</asp:ListItem>
                                        <asp:ListItem Value="C">Consumables</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnSubmit" runat="server" Text="View Data" CssClass="lButton" Width="80px" />
                                </td>
                            </tr>
                            <tr class="whitebg" runat="server">
                                <td class="tdtext">
                                    Consumables:
                                    <asp:GridView ID="grdmainassets" runat="server" AutoGenerateColumns="False"
                                       CssClass="mGrid"  GridLines="none"
                                        BorderWidth="1px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Consumables and EmployeeName">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblassetname" Text='<%# Eval("EmpName") %>' />
                                                    <asp:GridView ID="grdconsumables3" runat="server" CssClass="mGrid" DataSource='<%# BindConsumablesorAssetsByUser(Eval("EmpNumber").ToString()) %>'
                                                         GridLines="Horizontal" AutoGenerateColumns="false">
                                                           <Columns>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                <HeaderTemplate>
                                                                    S.No</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex + 1%></ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
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
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#000000" Font-Bold="True" ForeColor="White" Font-Size="10pt" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <% ElseIf Request.QueryString("RType") = "2" Then%>
                        <table width="100%">
                            <tr class="whitebg">
                                <td class="tdtext" align="center">
                                    Select Employee : &nbsp;&nbsp;
                                    <asp:DropDownList ID="drpemployee2" CssClass="control" runat="server" AutoPostBack="true"
                                        Width="300px">
                                    </asp:DropDownList>
                                    <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" PromptCssClass="ListExtend"
                                        TargetControlID="drpemployee2">
                                    </asp:ListSearchExtender>
                                </td>
                            </tr>
                            <tr class="whitebg" runat="server" id="trAssets" visible="false">
                                <td class="tdtext">
                                    Assets:
                                    <asp:GridView ID="grdassets" runat="server" CssClass="mGrid" GridLines="None" AlternatingRowStyle-CssClass="alt"
                                        AllowPaging="false" AllowSorting="true" AutoGenerateColumns="false" Width="100%"
                                        EmptyDataText="No Data Found !">
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
                                            <asp:BoundField HeaderText="Quantity" DataField="qty" SortExpression="qty" ItemStyle-HorizontalAlign="Center"  />
                                        </Columns>
                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <% ElseIf Request.QueryString("RType") = "3" Then%>
                        <table width="100%">
                            <tr class="whitebg" runat="server">
                                <td class="tdtext">
                                    Consumables:
                                    <asp:GridView ID="grdconsumables1" runat="server" CssClass="mGrid" GridLines="None"
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
                                            <asp:BoundField HeaderText="Cond#1" DataField="att1" SortExpression="att1" />
                                            <asp:BoundField HeaderText="Cond#2" DataField="att2" SortExpression="att2" />
                                            <asp:BoundField HeaderText="Cond#3" DataField="att3" SortExpression="att3" />
                                            <asp:BoundField HeaderText="Complaint By" DataField="compby" SortExpression="compby" />
                                            <asp:BoundField HeaderText="Req Date" DataField="reqdate" SortExpression="reqdate" />
                                            <asp:BoundField HeaderText="Approved By" DataField="ApprovedBy" SortExpression="ApprovedBy" />
                                            <asp:BoundField HeaderText="Issued By" DataField="IssuedBy" SortExpression="IssuedBy" />
                                            <asp:BoundField HeaderText="Quantity" DataField="qty" SortExpression="qty" ItemStyle-HorizontalAlign="Center"  />
                                        </Columns>
                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <% End If%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
