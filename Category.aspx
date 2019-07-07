<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="Category.aspx.vb" Inherits="Category" title="Category Master" %>

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
                        <td align="left" colspan="2">Add New/Update Asset Category</td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext" style="width:50%;border-right-style:solid;">
                         Category Code :
                        </td>
                        <td align="left" style="width:50%;border-left-style:solid;">
                            <asp:TextBox ID="txtCategoryCode" class="control" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext">
                            Category Description :
                         </td>
                         <td align="left"  style="border-left-style:none;">
                            <asp:TextBox ID="txtCategoryDesc" class="control" Width="450" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td align="right" class="tdtext">
                            Group Name :
                        </td>
                        <td align="left" class="tdtext"  style="border-left-style:none;">
                            <asp:Label ID="Label1" runat="server" Text="Hardware"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="padding-top:10px;">
            <td align="center">
                <asp:Button ID="btnSave" runat="server" Text="Save" class="lButton" Width="100"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="lButton" Width="100"/>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding='0' cellspacing='0' border='0' width="90%" align="center" id="myTable">
        <tr class="tdcolbg">
            <td>
                <table cellpadding='4' cellspacing='1' border='0' width='100%' align='center'>
                    <tr class="trheaderbg">
                        <td align="center">S.No</td>
                        <td align="center">Category Code</td>
                        <td align="center">Category Description</td>
                        <td align="center" style="display:none;">GroupName</td>
                    </tr>
                    <%FillData() %>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

