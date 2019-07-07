<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReportInfo.aspx.vb" Inherits="ReportInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Info</title>
    <link rel="stylesheet" type="text/css" href="css/ApplyCSS1.css" />
    <link rel="Stylesheet" type="text/css" href="css/tabs.css" />
    <link rel="Stylesheet" type="text/css" href="css/grid.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlReportinfo" runat="server" Width="900px" ScrollBars="None" Style="position: absolute;
        top: 10px; left: 15px; height: 700px; font-size: 8;">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:TabContainer ID="tabReportinfo" Height="740px" Width="100%" CssClass="yui" runat="server"
            ActiveTabIndex="3">
            <asp:TabPanel border="1px" ID="Tb1" HeaderText="Technical Specifications" runat="server">
                <ContentTemplate>
                    <asp:DetailsView ID="grdassets" runat="server" CssClass="mGrid" GridLines="None"
                        AllowPaging="false" AutoGenerateColumns="false" Width="100%" AlternatingRowStyle-CssClass="alt">
                    </asp:DetailsView>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel border="1px" ID="Tb2" HeaderText="Deployment Details" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grddeployments" runat="server" CssClass="mGrid" GridLines="None"
                        EmptyDataText="No Deployment/Undeployment Details Available In This Asset" AllowSorting="True"
                        AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField SortExpression="Emp_Name" DataField="Emp_Name" HeaderStyle-Width="150px"
                                HeaderText="Employname">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField SortExpression="TransType" DataField="TransType" HeaderStyle-Width="150px"
                                HeaderText="Tranaction Type">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField SortExpression="Deploydate" DataField="Deploydate" HeaderStyle-Width="100px"
                                HeaderText="Transaction Date">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField SortExpression="ConsultantName" DataField="ConsultantName" HeaderText="Consultatnt Name"
                                HeaderStyle-Width="170px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField SortExpression="remarks" DataField="remarks" HeaderText="Remarks"
                                HeaderStyle-Width="250px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <AlternatingRowStyle CssClass="alt" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel border="1px" ID="Tb3" HeaderText="Maintenance History" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdmaintainance" runat="server" CssClass="mGrid" GridLines="None"
                        EmptyDataText="No Repair/Return Available In This Asset" AllowSorting="True"
                        AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno" ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField SortExpression="Username" DataField="Username" HeaderStyle-Width="150px"
                                HeaderText="Vendor/User Name">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField SortExpression="TransType" DataField="TransType" HeaderStyle-Width="150px"
                                HeaderText="Tranaction">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField SortExpression="RepairDate" DataField="RepairDate" HeaderStyle-Width="100px"
                                HeaderText="Repair/Return Date">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField SortExpression="ConsultantName" DataField="ConsultantName" HeaderText="Consultatnt Name"
                                HeaderStyle-Width="170px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField SortExpression="remarks" DataField="remarks" HeaderText="Remarks"
                                HeaderStyle-Width="250px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <AlternatingRowStyle CssClass="alt" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel border="1px" ID="Tb4" HeaderText="Complaints" runat="server">
                <ContentTemplate>
                    <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
                        <tr class="whitebg">
                            <td class="tdtext">
                                <asp:GridView ID="grdcompliants" runat="server" CssClass="mGrid" GridLines="None"
                                    DataKeyNames="complaintid" EmptyDataText="No Compliants Available In This Asset"
                                    AllowSorting="True" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField SortExpression="Emp_Name" DataField="Emp_Name" HeaderStyle-Width="150px"
                                            HeaderText="Employee Name">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Compliant" SortExpression="complaint" HeaderStyle-Width="200px">
                                            <ItemTemplate>
                                                <a target="_blank" href="//SIPSV0020/Applications/HelpDesk/ComplaintConsView.aspx?CId=<%#Eval("complaintid")%>">
                                                    <%#Eval("complaint")%></a>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                        <asp:BoundField SortExpression="cdate" DataField="cdate" HeaderStyle-Width="75px"
                                            HeaderText="Date">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="closetag" DataField="closetag" HeaderStyle-Width="30px"
                                            HeaderText="Status">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="ConsultantName" DataField="ConsultantName" HeaderText="Consultatnt Name"
                                            HeaderStyle-Width="150px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass="alt" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel border="1px" ID="TabPanel1" HeaderText="Warranty" runat="server">
                <ContentTemplate>
                    <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
                        <tr class="whitebg">
                            <td class="tdtext">
                                <asp:GridView ID="grdwarranty" runat="server" CssClass="mGrid" GridLines="None" EmptyDataText="No warranty details in this Asset"
                                    AllowSorting="True" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField SortExpression="att1" DataField="att1" HeaderStyle-Width="100px"
                                            HeaderText="Asset Number">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="pono" DataField="pono" HeaderStyle-Width="150px" HeaderText="Purchase Order">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         <asp:BoundField SortExpression="Warrantystart" DataField="Warrantystart" HeaderStyle-Width="100px" HeaderText="Warranty Start">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="warranty" DataField="warranty" HeaderStyle-Width="100px"
                                            HeaderText="Warranty End">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="contractno" DataField="contractno" HeaderStyle-Width="100px"
                                            HeaderText="Contract No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="vendor" DataField="vendor" HeaderStyle-Width="100px"
                                            HeaderText="Vendor">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="ConsultantName" DataField="ConsultantName" HeaderText="Consultatnt Name"
                                            HeaderStyle-Width="150px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass="alt" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </asp:Panel>
    </form>
</body>
</html>
