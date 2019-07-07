<%@ Page Language="VB" AutoEventWireup="false"  CodeFile="asset_count_details.aspx.vb" Inherits="asset_count_details" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Asset Count Details</title>
 <link rel="stylesheet" type="text/css" href="css/ApplyCSS1.css"/> 
  <link rel="stylesheet" type="text/css" href="Css/grid.css"/> 
 <link rel="Stylesheet" type="text/css" href="css/tabs.css" />
  <script type="text/javascript">
    function openReportInfo()
        {
            var mywin = window.open("ReportInfo.aspx", "mywin", "location=1,status=1,scrollbars=1, width=100,height=100,resizable=1");
        }
  </script>
  </head>
<body style="margin-top:0; margin-left:0;">
 <form id="form1"  runat="server">
 <center>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr class="tdcolbg">
            <td align=  "center" class="tdtextbold" >
            <asp:Label ID ="LblCaption"  runat="server"> </asp:Label>
            <asp:Label ID ="LblSubCaption"  runat="server"> </asp:Label>
            </td>
        </tr>
    <tr>
    <td>
   <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
   		<tr  class="trheaderbg">
		<td align="center" style="font-family:Verdana;font-size:11px;"><asp:Label ID="lblmsg" runat="server"></asp:Label></td>
	    </tr>
        <tr class="whitebg">
            <td  align="center" class="tdtextbold" >
               <div id="" style="height:auto; overflow-x: hidden">
                <table width="100%" border="0" style="border-collapse:collapse">
                <asp:GridView ID="grdassets" runat="server" 
                    CssClass="mGrid"
                    GridLines="None"  
                    AlternatingRowStyle-CssClass="alt"
                    AllowPaging="false"
                    AutoGenerateColumns="false" Width="100%" >
                        <Columns>
                        </Columns>
                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                    </asp:GridView>
              </table> 
                </div>               
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
    </td>
    </tr>
    </table>
</center>
</form>
</body>
</html>
