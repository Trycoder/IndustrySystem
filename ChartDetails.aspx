<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="ChartDetails.aspx.vb" Inherits="ChartDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Chart Details</title>
    <link href="Css/grid.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center" cellpadding="0" cellspacing="0" width="95%">
            <tr>
                <td valign="middle">
                    <div id="divTitle" style="font-weight:bold;font-size:14px;text-align:center;" runat="server"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divGrid" runat="server"></div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>


