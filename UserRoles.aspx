<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="UserRoles.aspx.vb"
    Inherits="UserRoles" Title="User Roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function FnChkRNull(Indx, Indx1) {
               //alert(Indx);
            var ss1 = document.getElementById(Indx).checked;
                     //alert(ss1);
            if (ss1 == true) {
                document.getElementById(Indx1).checked = false;
            }
        }

        function FnChkFNull(Indx, Indx1) {
                //alert(Indx);
            var ss1 = document.getElementById(Indx).checked;
            //alert(ss1);
          //  alert(Indx1);
            if (ss1 == true) {
                document.getElementById(Indx1).checked = false;
            }
        }
        function checkAll(cName, status) {
            //alert(status);
            if (status == 'true') {
                document.getElementById("divchktrue").style.display = "none";
                document.getElementById("divchkfalse").style.display = "block";
                for (i = 0, n = document.forms[0].elements.length; i < n; i++)
                    if (document.forms[0].elements[i].className.indexOf(cName) != -1) {
                        document.forms[0].elements[i].checked = true;
                    }
                document.getElementById("divchktrueRead").style.display = "block";
                document.getElementById("divchkfalseRead").style.display = "none";
                for (i = 0, n = document.forms[0].elements.length; i < n; i++)
                    if (document.forms[0].elements[i].className.indexOf('results2') != -1) {
                        document.forms[0].elements[i].checked = false;
                    }
            }
            else {
                document.getElementById("divchktrue").style.display = "block";
                document.getElementById("divchkfalse").style.display = "none";
                for (i = 0, n = document.forms[0].elements.length; i < n; i++)
                    if (document.forms[0].elements[i].className.indexOf(cName) != -1) {
                        document.forms[0].elements[i].checked = false;
                    }
            }
        }

        function checkAllRead(cName, status) {
            //alert(status);
            if (status == 'true') {
                document.getElementById("divchktrueRead").style.display = "none";
                document.getElementById("divchkfalseRead").style.display = "block";
                for (i = 0, n = document.forms[0].elements.length; i < n; i++)
                    if (document.forms[0].elements[i].className.indexOf(cName) != -1) {
                        document.forms[0].elements[i].checked = true;
                    }
                document.getElementById("divchktrue").style.display = "block";
                document.getElementById("divchkfalse").style.display = "none";
                for (i = 0, n = document.forms[0].elements.length; i < n; i++)
                    if (document.forms[0].elements[i].className.indexOf('results1') != -1) {
                        document.forms[0].elements[i].checked = false;
                    }
            }
            else {
                document.getElementById("divchktrueRead").style.display = "block";
                document.getElementById("divchkfalseRead").style.display = "none";
                for (i = 0, n = document.forms[0].elements.length; i < n; i++)
                    if (document.forms[0].elements[i].className.indexOf(cName) != -1) {
                        document.forms[0].elements[i].checked = false;

                    }
            }
        } 
                                                                                                                                                                                                                                                                                                                                                                                                                            
    </script>
    <table border="0" align="center" cellpadding="0" cellspacing="0" width="90%">
        <tr class="tdcolbg">
            <td>
                <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%" style="overflow: scroll;">
                    <tr class="trheaderbg">
                        <td colspan="2">
                            Set User Roles
                        </td>
                    </tr>
                    <tr class="whitebg" visible="false" id="trmsg" runat="server">
                        <td colspan="2">
                            <asp:Label ID="lblmessage" runat="server" CssClass="tdtext"></asp:Label>
                        </td>
                    </tr>
                    <tr class="whitebg">
                        <td style="width: 50%;" class="tdtext" align="right">
                            User Group :
                        </td>
                        <td style="width: 50%;" class="tdtext">
                            <asp:DropDownList ID="drpusergroup" runat="server" AutoPostBack="true" CssClass="control"
                                Width="130">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; overflow-x: scroll;" height="auto">
                    <% If drpusergroup.SelectedValue <> "" Then%>
                    <table cellpadding="4" cellspacing="1" style="width: 100%; border-bottom: #cccccc 1px solid;
                        border-right: #cccccc 1px solid; border-left: #cccccc 1px solid;">
                        <tr class="trheaderbg">
                            <td align="center">
                                Page Name
                            </td>
                            <td align="center">
                                Full Access
                            </td>
                            <td align="center" width="22%">
                                Read Only
                            </td>
                        </tr>
                        <tr class="whitebg">
                            <td align="center">
                               
                            </td>
                            <td align="center" style="color:Blue;">
                               <div id="divchkfalse" style="display: none">
                                    <a onclick="checkAll('results1', 'False');" style="cursor: pointer; text-decoration: underline">
                                        Uncheck All </a>
                                </div>
                                <div id="divchktrue">
                                    <a onclick="checkAll('results1', 'true');" style="cursor: pointer; text-decoration: underline">
                                        Check All </a>
                                </div>
                            </td>
                            <td align="center"  style="color:Blue;">
                               <div id="divchkfalseRead" style="display: none">
                                    <a onclick="checkAllRead('results2', 'False');" style="cursor: pointer; text-decoration: underline">
                                        Uncheck All </a>
                                </div>
                                <div id="divchktrueRead">
                                    <a onclick="checkAllRead('results2', 'true');" style="cursor: pointer; text-decoration: underline">
                                        Check All </a>
                                </div>
                            </td>
                        </tr>
                        <%

                            Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
                            Dim cmd1 As New System.Data.SqlClient.SqlCommand
                            Dim dtable As New System.Data.DataTable
                            Dim dtable1 As New DataTable
                            Dim dtable2 As New DataTable
                            dtable = GetMenuMasters()
                            'Dim I As Integer
                            Dim altcolor As Integer = 1
                            If dtable.Rows.Count > 0 Then
                                For i As Integer = 0 To dtable.Rows.Count - 1
                        %>
                        <tr class="tdcolbg">
                            <td style="padding-left: 3px;" class="tdtextbold">
                                <% = dtable.Rows(i)("menudesc")%>
                            </td>
                            <td style="width: 10%; font-weight: bold;" align="center">
                              <% If dtable.Rows(i)("mtag").ToString() = "1" Then%>
                                <input type="checkbox" id="chkfullm<%=dtable.rows(i)("id") %>" checked="checked" name="chkfullm<%=dtable.rows(i)("id")%>"
                                    onclick="FnChkRNull('chkfullm<%=dtable.rows(i)("id") %>','chkreadm<%=dtable.rows(i)("id") %>')"
                                    class="results1" />
                               <% End If%>
                            </td>
                            <td style="width: 10%; font-weight: bold;" align="center">
                                <% If dtable.Rows(i)("mtag").ToString() = "1" Then%>
                                <input type="checkbox" id="chkreadm<%=dtable.rows(i)("id") %>" checked="checked" name="chkreadm<%=dtable.rows(i)("id")%>"
                                    onclick="FnChkFNull('chkreadm<%=dtable.rows(i)("id") %>','chkfullm<%=dtable.rows(i)("id") %>')"
                                    class="results2" />
                                <% End If%>
                            </td>
                        </tr>
                        <% dtable1 = GetSubMenuList(dtable.Rows(i)("mainmenuid").ToString())%>
                       
                        <% 
                            For j As Integer = 0 To dtable1.Rows.Count - 1
                          dtable2 = GetSubMenuListExtn(dtable1.Rows(j)("id").ToString())
                         %>
                        <tr class="whitebg">
                            <td style="padding-left: 3px; width: 30%" class="tdtext">
                                <% = dtable1.Rows(j)("menudesc")%>
                            </td>
                            <td style="width: 10%; font-weight: bold;" align="center">
                                <% If dtable2.Rows.Count > 0 Then%>
                                      <% If dtable2.Rows(0)("rights").ToString() = "1" Then%>
                                                  <input type="checkbox" id="chkfull<%=dtable1.rows(j)("id")%>" checked="checked"
                                                onclick="FnChkRNull('chkfull<%=dtable1.rows(j)("id") %>','chkread<%=dtable1.rows(j)("id") %>')"
                                                name="chkfull<%=dtable1.rows(j)("id")%>" class="results1" />
                                       <% Else%>
                                                     <input type="checkbox" id="chkfull<%=dtable1.rows(j)("id") %>" onclick="FnChkRNull('chkfull<%=dtable1.rows(j)("id") %>','chkread<%=dtable1.rows(j)("id") %>')"
                                            name="chkfull<%=dtable1.rows(j)("id")%>" class="results1" />
                                      <% End If%>
                                <% Else%>
                                 <input type="checkbox" id="chkfull<%=dtable1.rows(j)("id") %>" onclick="FnChkRNull('chkfull<%=dtable1.rows(j)("id") %>','chkread<%=dtable1.rows(j)("id") %>')"
                                            name="chkfull<%=dtable1.rows(j)("id")%>" class="results1" />
                                <%End If%>
                            </td>
                            <td style="width: 10%; font-weight: bold;" align="center">
                             <% If dtable2.Rows.Count > 0 Then%>
                                      <% If dtable2.Rows(0)("rights").ToString() = "2" Then%>
                                         <input type="checkbox" id="chkread<%=dtable1.rows(j)("id")%>" checked="checked"
                                    onclick="FnChkFNull('chkread<%=dtable1.rows(j)("id") %>','chkfull<%=dtable1.rows(j)("id") %>')"
                                    name="chkread<%=dtable1.rows(j)("id")%>" class="results2" />
                                      <% Else%>
                                                           <input type="checkbox" id="chkread<%=dtable1.rows(j)("id")%>" 
                                                onclick="FnChkFNull('chkread<%=dtable1.rows(j)("id") %>','chkfull<%=dtable1.rows(j)("id") %>')"
                                                name="chkread<%=dtable1.rows(j)("id")%>" class="results2" />
                                      <% End if %>
                            <% else %>
                                    <input type="checkbox" id="chkread<%=dtable1.rows(j)("id") %>" onclick="FnChkFNull('chkread<%=dtable1.rows(j)("id") %>','chkfull<%=dtable1.rows(j)("id") %>')"
                                    name="chkread<%=dtable1.rows(j)("id")%>" class="results2" />
                            <% End If%>
                               
                            </td>
                        </tr>
                        <%
                        Next
                    Next
                End If
                        %>
                    </table>
                    <% End If%>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="assetname" CssClass="lButton"
                    Width="80px" />&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width="80px" />
            </td>
        </tr>
    </table>
</asp:Content>
