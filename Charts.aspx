<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false"
    CodeFile="Charts.aspx.vb" Inherits="Charts" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        var arrRange= new Array();
        var ID = "ctl00_ContentPlaceHolder1_";
      
        function GroupRange(xValue, yValue) {
            this.xValue = xValue;
            this.yValue = yValue;
        }
        function drawChart() {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Range');
            data.addColumn('number', 'Count')
            data.addRows(arrRange.length);
            for (var i = 0; i < arrRange.length; i++) {
                data.setValue(i, 0, arrRange[i].xValue);
                data.setValue(i, 1, arrRange[i].yValue);
            }
            var chart = new google.visualization.PieChart(document.getElementById("divChart"));
            chart.draw(data, { width: 1000, height: 600, pieSliceText: 'value', chartArea: { width: "75%", height: "65%" }, legend: { position: 'right', textStyle: { color: 'black', fontSize: 12}} });

            google.visualization.events.addListener(chart, 'select', selectSlice);

            function selectSlice(e) {
                if (arrRange.length > 0) {
                    var ageRange = "";
                    grpRange = data.getValue(chart.getSelection()[0].row, 0);
                    document.getElementById(ID + "hdnRange").value = grpRange;
                    window.open("ChartDetails.aspx?Range=" + escape(grpRange), 'popupwindow', 'width=1000,height=400,left=100,top=300,scrollbars,resizable=1');
                    chart.setSelection();
                }
            }
        }
        
    </script>
    <table border="0" align="center" cellpadding="0" cellspacing="0" width="90%">
        <tr class="tdcolbg">
            <td>
                <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
                    <tr class="trheaderbg">
                        <td colspan="2">
                            Charts
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                ShowSummary="False" ValidationGroup="assetname" />
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td style="width: 50%;" class="tdtext" align="right" valign="top">
                            Select Assets :
                        </td>
                        <td style="width: 50%;" class="tdtext">
                            <%-- <asp:DropDownList ID="drpassets" runat="server" CssClass="control" Width="120" Visible="false"></asp:DropDownList>--%>
                            <asp:ListBox ID="lstassets" runat="server" CssClass="control" Width="250px" Height="75px"
                                SelectionMode="Multiple"></asp:ListBox>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext">
                            Chart Group By :
                        </td>
                        <td class="tdtext">
                            <asp:DropDownList ID="drpgroupby" runat="server" CssClass="control" Width="250px"
                                AutoPostBack="true">
                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                <asp:ListItem Value="Age">Age Wise</asp:ListItem>
                                <asp:ListItem Value="Building">BuildingUnit Wise</asp:ListItem>
                                <asp:ListItem Value="Department">Department Wise</asp:ListItem>
                                <asp:ListItem Value="Make">Make Wise</asp:ListItem>
                                <asp:ListItem Value="Status">Status Wise</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext">
                            Condition :
                        </td>
                        <td class="tdtext" align="left">
                            <asp:UpdatePanel runat="server" ID="pnlcondition">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="drpCondition" runat="server" CssClass="control" Width="120"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="">--Select--</asp:ListItem>
                                                    <asp:ListItem Value="Age">Age</asp:ListItem>
                                                    <asp:ListItem Value="Building">BuildingUnit</asp:ListItem>
                                                    <asp:ListItem Value="Department">Department</asp:ListItem>
                                                    <asp:ListItem Value="Make">Make</asp:ListItem>
                                                    <asp:ListItem Value="Status">Status</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                =
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpconsvalue" AutoPostBack="true" runat="server" CssClass="control" Width="120">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DrpANDOR" class="control" runat="server" Width="100px" AutoPostBack="true">
                                                    <asp:ListItem Text="---" Value="0"> </asp:ListItem>
                                                    <asp:ListItem Text="OK" Value="1"> </asp:ListItem>
                                                    <asp:ListItem Text="AND" Value="2"> </asp:ListItem>
                                                    <asp:ListItem Text="OR" Value="3"> </asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnclearcondtion" runat="server" Text="Clear" CssClass="lButton"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td class="tdtextbold" colspan="2">
                            <asp:UpdatePanel ID="pnlconditiontext" runat="server">
                                <ContentTemplate>
                                    &nbsp;Condition :
                                    <asp:Label ID="LblCondText" Text="" runat="server"></asp:Label>
                                    <asp:Label ID="LblInvConText" Text="" Visible="false" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td colspan="2" align="center" class="tdtext">
                            &nbsp;
                            <asp:Button ID="btnSave" runat="server" Text="View Chart" CssClass="lButton" Width="80px" />&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width="80px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="whitebg">
            <td colspan="2" align="center">
                <table style="padding-top: 5px;" width="100%">
                    <tr class="">
                        <td class="tdtext">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <br />
                                    <div id="divChartTitle" style="font-weight:bold;" runat="server"></div>
                                    <div id="divChart">
                                    </div>
                                    <asp:HiddenField ID="hdnRange" runat="server" />
                                    <%--<asp:Chart ID="Chart1" runat="server" Width="950px" Height="600px" Visible="false"
                                        Palette="BrightPastel" BackColor="WhiteSmoke" BorderlineDashStyle="Solid" BackSecondaryColor="White"
                                        BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105">
                                        <Titles>
                                            <asp:Title Name="title1">
                                            </asp:Title>
                                        </Titles>
                                        <Series>
                                            <asp:Series Name="piecharts" ChartType="Pie">
                                            </asp:Series>
                                        </Series>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartArea1" BackGradientStyle="TopBottom" BorderColor="64, 64, 64, 64"
                                                BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent"
                                                BorderWidth="0">
                                                <AxisY LineColor="64, 64, 64, 64">
                                                    <LabelStyle Font="verdana, 9px, style=Bold" />
                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                </AxisY>
                                                <AxisX LineColor="64, 64, 64, 64">
                                                    <LabelStyle Font="verdana, 9px, style=Bold" />
                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                </AxisX>
                                            </asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
