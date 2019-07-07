<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="cc1" Namespace="popup.Web" Assembly="popup.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<meta http-equiv="refresh" content="10">--%>
<%--<meta http-equiv="refresh" content="60" > --%>
        <script type="text/javascript" language="javascript">
            function fnStatus(stat) {
                if (stat == 1) {//Expand
                    document.getElementById('divStatus').style.display = "block";
                    document.getElementById('imgExp').style.display = "none";
                    document.getElementById('imgCol').style.display = "block";
                }
                else if (stat == 2) {//Collapse.
                    document.getElementById('divStatus').style.display = "none";
                    document.getElementById('imgExp').style.display = "block";
                    document.getElementById('imgCol').style.display = "none";
                }
            }
        </script>

    <%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Configuration" %>
 <table cellpadding='0' cellspacing='1' border='0' style="vertical-align:top;margin:0px;" width='99%' align='center'>
    
   
   
</table>
<script type="text/javascript">
    var i = 0;
    var count, i, TypeOfView, loopLimit, itemIndex, DivMinTop, DivMaxTop, DivMaxHeight, DivHeight, DivTop, pageLoadCount, IsIntervalStoped;
    var LoopIndex;
    LoopIndex = 0;
    loopLimit = 0;
    itemIndex = 0;
    count = 0;
    pageLoadCount = 0;

    TypeOfView = "B";
    DivMinTop = 82;
    DivMaxHeight = 90;
    DivMaxTop = 9;
    DivHeight = 15;
    DivTop = 85;
    IsIntervalStoped = "N";

    var str;
    str = ""
    var ResponseArray = new Array();
    var ResponseArrayEmpName = new Array();
    var cid = new Array();

    if (IsIntervalStoped == "N")
        var refreshIntervalId = setInterval(showPOPUP, 4000);

    if (IsIntervalStoped == "Y")
        clearInterval(intervalID);


    function showPOPUP() {
        var xmlhttp;
        if (window.XMLHttpRequest) {
            xmlhttp = new XMLHttpRequest();
        }
        else {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                document.getElementById("DivResponseHTML").innerHTML = xmlhttp.responseText;
                loopLimit = document.getElementById("Datacount").innerText;
                if (pageLoadCount == 0)  // weather the page is loaded first time
                {
                    document.getElementById("DivResponseHTML").style.height = DivHeight + "%";
                    document.getElementById("DivResponseHTML").style.top = DivTop + "%";

                    pageLoadCount = 1;
                }
                if (loopLimit > 0) {
                    document.getElementById("DivResponseHTML").style.backgroundColor = "#E3E4FA";
                    document.getElementById("DivResponseHTML").style.visibility = 'hidden';
                    for (i = 0; i < loopLimit; i++) {
                        ResponseArray[i] = document.getElementById("emp").innerText;
                        ResponseArrayEmpName[i] = document.getElementById("empp").innerText;
                        cid[i] = document.getElementById("cid").innerText;

                    }

                    FNpopup();

                }
            }

            else {
                document.getElementById("DivResponseHTML").style.visibility = 'hidden';

            }
        }

        xmlhttp.open("GET", "PopupData.aspx?a=" + Math.random(), true);
        xmlhttp.send();
    }


    function FNpopup() {
        var htmpSTR;
        htmpSTR = "<table  border=0 cellpadding=0 cellspacing=0 style =width:100%; ><tr>"
        htmpSTR = htmpSTR + "<td>"
        htmpSTR = htmpSTR + " <table  border=0 cellpadding='3' cellspacing=0 style = 'width:100%;position:absolute;;'><tr class='trheaderbg'>"
        htmpSTR = htmpSTR + "<td  align=left width=75%>1 New Complaint </td><td align='right' width='25%'> "
        htmpSTR = htmpSTR + "  <IMG name=Less SRC=images/popupclose.png width=13% height=13% style=cursor:hand onclick=stopORpause(refreshIntervalId,2)>";
        htmpSTR = htmpSTR + " </td></tr></table>"
        htmpSTR = htmpSTR + "   </td></tr>";

        htmpSTR = htmpSTR + "<tr><td style='padding-top:5px;'>"
        htmpSTR = htmpSTR + "<br />"

        htmpSTR = htmpSTR + " <table Class='ttsearch' style ='width:100%;' cellpadding='3'> "
        for (i = 0; i < loopLimit; i++) {
            htmpSTR = htmpSTR + " <tr><td align='left' width='100%' style='font-weight:bold;'>"
            htmpSTR = htmpSTR + ""
            htmpSTR = htmpSTR + "" + ResponseArray[i] + "</td></tr><tr><td align='left' ><a href='#' onclick='ViewComplaint(" + cid[i] + ")'>" + ResponseArrayEmpName[i] + "</a></td></tr>";
        }
        htmpSTR = htmpSTR + "</table> </td></tr></table>"

        document.getElementById("DivResponseHTML").innerHTML = htmpSTR;
        visibleDIV();
    }


    function visibleDIV() {
        document.getElementById("DivResponseHTML").style.visibility = 'visible';
    }

    function stopORpause(intervalID, Action) {
        if (Action == 2)  // Stop
        {
            clearInterval(intervalID);
            IsIntervalStoped = "Y";
            document.getElementById("DivResponseHTML").style.visibility = 'hidden';
        }
    }
    function ViewComplaint(CId) {
        window.open("ComplaintView.aspx?CId=" + CId, 'popupwindow', 'width=1000,height=600,left=100,top=100,scrollbars,resizable=1');
        return false;

    }
    </script>
  </asp:Content>
