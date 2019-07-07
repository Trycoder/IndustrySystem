<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AssetTypeDetails.aspx.vb" Inherits="AssetTypeDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Details</title>
    <script src="JS/Common.js" type="text/javascript"></script>
    <link href="Css/ApplyCSS1.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="Css/grid.css" />
    <script language="javascript" type="text/javascript">
        document.onselectstart=new Function ("return false");
        function ChangeColor(tablerw)
        {
            var i;
            var table = document.getElementById('myTable');
            var rows = table.getElementsByTagName("tr");
            for (i=1; i<rows.length; i++)
            {            
                rows[i].style.backgroundColor = '#FFFFFF';
            }    
            tablerw.style.backgroundColor = '#CAE1FF';
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:Panel ID="Panel2" runat="server" Height="745px" Width="100%">
        <div style="width:auto;height:730px;">
            <table cellpadding='0' cellspacing='0' border='0' width='100%'  align='center'>
                <tr>
                    <td style='vertical-align:top;'>
                        <%If Request("Pnl") = "1" Then
                            AssetTransaction()
                        ElseIf Request("Pnl") = "2" Then
                            ConsumableTransaction()
                        ElseIf Request("Pnl") = "3" Then
                            WarrantyExpiry()
                        ElseIf Request("pnl")="4"
                            RequestCons()
                        ElseIf Request("pnl")="5"
                                ApproveCons()
                            ElseIf Request("pnl") = "6" Then
                                BindOtherAssets()
                            End If
                        
                        %>
                    </td>
                </tr>
            <tr  class="whitebg">
            <td  align="center" class="tdtextbold" >
               <div id="Div1" style="height:auto; overflow-x: hidden">
                <table width="100%" border="0" style="border-collapse:collapse">
                <tr>
                 <td runat="server" id="tddata" class="tdtextbold">
                
                </td>
                </tr>
              </table> 
                </div>               
            </td>
             </tr>
            </table>
        </div>
    </asp:Panel>
</form>
</body>
</html>
