<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="AssetAttributes.aspx.vb" Inherits="AssetAttributes" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script language="javascript" type="text/javascript">
        function ChangeColor(tablerw)
        {
            var i;
            var table = document.getElementById('myTable');
            var rows = table.getElementsByTagName("tr");
            for (i=2; i<rows.length; i++)
            {            
                rows[i].style.backgroundColor = '#FFFFFF';
            }    
            tablerw.style.backgroundColor = '#E6E8FA';
            document.getElementById(getControlID('txtCategoryCode')).value=(tablerw.cells[1].childNodes[0].data);
            document.getElementById(getControlID('txtCategoryDesc')).value=(tablerw.cells[2].childNodes[0].data);
        }
    </script>
    <table cellpadding='0' cellspacing='0' border='0' width="90%" align="center">
        <tr class="tdcolbg">
            <td>                 
                <table cellpadding='4' cellspacing='1' border='0' width='100%' align='center'>
                    <tr class="trheaderbg">
                        <td align="left">Add New Asset Attributes</td>
                    </tr>
                    <tr class="whitebg">
                        <td align="center" class="tdtext">
                            Select Category :
                            <asp:DropDownList ID="cboCategory" class="control" Width="150" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="center" class="tdtext">
                            Attribute Description :
                            <asp:TextBox ID="txtAttributeDesc" class="control" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="center" class="tdtext">
                            Attribute ToolTip Text :
                            <asp:TextBox ID="txtToolTipText" class="control" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="center" class="tdtext">
                            Attribute Type :
                            <asp:DropDownList ID="cboAttributeType" class="control" Width="150" runat="server">
                                <asp:ListItem>Text(Variable)</asp:ListItem>
                                <asp:ListItem>Text(Fixed)</asp:ListItem>
                                <asp:ListItem>Sequence</asp:ListItem>
                                <asp:ListItem>Random</asp:ListItem>
                                <asp:ListItem>Date</asp:ListItem>
                                <asp:ListItem>Yes/No</asp:ListItem>
                                <asp:ListItem>SingleSelection</asp:ListItem>
                                <asp:ListItem>MultiSelection</asp:ListItem>
                                <asp:ListItem>FileUpload</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="center" class="tdtext">
                            Occurance :
                            <asp:DropDownList ID="cboOccurance" class="control" Width="150" runat="server">
                                <asp:ListItem>Non-Mandatory</asp:ListItem>
                                <asp:ListItem>Mandatory</asp:ListItem>                                
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="center" class="tdtext">
                            Header :
                            <asp:DropDownList ID="cboHeader" class="control" Width="150" runat="server">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">PO Number</asp:ListItem>
                                <asp:ListItem Value="2">PO Date</asp:ListItem>
                                <asp:ListItem Value="3">Warranty Start Date</asp:ListItem>
                                <asp:ListItem Value="4">Warranty End Date</asp:ListItem>
                                <asp:ListItem Value="5">Primary</asp:ListItem>
                                <asp:ListItem Value="8">Lock No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
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

