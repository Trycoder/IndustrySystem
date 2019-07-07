<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="Reports.aspx.vb" Inherits="Reports" title="Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">
    function keyDownNumber()
    {     
    var key;
    if(navigator.appName == 'Microsoft Internet Explorer')        
        key = event.keyCode;    
    else
        key = event.which     
        if ( !(key >= 48 && key <= 57) && key != 8 && key != 46 && key != 36 && key != 37) 
         {        
         event.returnValue = false;            
         }
    }
</script>
<script type="text/javascript" language="javascript">
var divid;

        function fnStatus(stat,divid,imgidxp,imgidcol)
    {
       // alert(stat);
        if(stat=="Expand")
        {
            document.getElementById(divid).style.display="block";
            document.getElementById(imgidxp).style.display="none";
            document.getElementById(imgidcol).style.display="block";
        } 
        else if(stat=="Collapse")
        {
            document.getElementById(divid).style.display="none";
            document.getElementById(imgidcol).style.display="none";
            document.getElementById(imgidxp).style.display="block";
        }
    }
    function NavigatetoReports(rtype,fid,catid,samepagexl,rpt) {
          if (samepagexl=='1')
            location.href = "ViewReport.aspx?RType=" + rtype + "&Fid=" + fid + "&Catid=" + catid + "&Rpt=" + rpt;
        if (samepagexl=='2')
            location.href = "ReportinExcel.aspx?report=other&RType=" + rtype + "&Fid=" + fid + "&Catid=" + catid;
    }
</script>
<table border="0" cellpadding="0" cellspacing="0" width="90%" align="center">
    <tr class="tdcolbg">
        <td>
            <table border="0" align="center" width="100%" cellpadding="4" cellspacing="1">
            <% If Request.QueryString("Rpt") = "A" then %>
                <tr class="trheaderbg" style="background-color:Gray;">
                    <td>
                    <table border="0" align="left" width="100%">
                        <tr class="">
                            <td width="10px">
                                <img src="Images/down-arrow.png" id="imgExp"  onclick="fnStatus('Expand','divStatus','imgExp','imgCol')" alt="Click to Expand"/>
                                <img src="Images/up-arrow.png" id="imgCol" style ="display:none" onclick="fnStatus('Collapse','divStatus','imgExp','imgCol')" alt="Click to Collapse"/>
                            </td> 
                            <td align="left">Standard Reports</td>                             
                        </tr>
                    </table>
                    </td>
                </tr>
                <tr class="whitebg">
                    <td class="tdtext" colspan="2">
                        <div id="divStatus" style="display:none;">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" align="left">
                            <tr class="tdcolbg">
                                <td>
                                    <table border="0" align="center" width="100%" cellpadding="0" cellspacing="1">
                                        <tr class="whitebg" style="height:30px;">
                                            <td style="width:90%" align="left" class="tdtext" colspan="3">
                                                 Select Asset:
                                                <asp:DropDownList ID="cboAsset" class="control" runat="server" Width="150px"></asp:DropDownList>
                                                Age more than:
                                                <asp:TextBox ID="txtAge" class="control" runat="server" Width="60px" onkeypress="keyDownNumber();"></asp:TextBox>
                                                <asp:DropDownList ID="cboAge" class="control" runat="server" Width="100px">
                                                    <asp:ListItem Text="Day(s)"></asp:ListItem>
                                                    <asp:ListItem Text="Month(s)"></asp:ListItem>
                                                    <asp:ListItem Text="Year(s)"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width:5%" align="center" class="tdtext">
                                                <asp:ImageButton id="imgasset" runat="server" ImageUrl="~/Images/Viewreport1.png" ToolTip="Click To View Report"  />
                                            </td>
                                            <td style="width:5%" align="center" class="tdtext">
                                                <asp:ImageButton id="imgxlsasset" runat="server" ImageUrl="~/Images/xlsicon.gif" ToolTip="Click To View Report" OnClick="imgxlsasset_Click"  />
                                            </td>
                                        </tr>
                                       <tr class="whitebg" style="height:30px;background-color:#F8F8FF;">
                                            <td align="left" class="tdtext" colspan="3">
                                          Employee having more than:
                                                <asp:TextBox ID="txtCount" class="control" runat="server" Width="60px" onkeypress="keyDownNumber();"></asp:TextBox>
                                                <asp:DropDownList ID="cboAssetCount" class="control" runat="server" Width="150px"></asp:DropDownList>
                                            </td>
                                            <td style="width:5%" align="center" class="tdtext">
                                                <asp:ImageButton id="imageemployee" runat="server" ImageUrl="~/Images/Viewreport1.png" ToolTip="Click To View Report" />
                                            </td>
                                            <td style="width:5%" align="center" class="tdtext">
                                                <asp:ImageButton id="imgxlsemployee" runat="server" ImageUrl="~/Images/xlsicon.gif" ToolTip="Click To View Report"/>
                                            </td>
                                       </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <%--      <tr class="whitebg">
                                    <td class="tdtext" align="center" colspan="6">
                                        <div style="overflow:auto; height:300px;width:800px;">
                                            <table border="0" width="100%"  cellpadding="0" cellspacing="0" align="center">
                                                <asp:Panel ID="Panel1" ScrollBars="Both" runat="server">
                                                    <asp:GridView ID="GridView1" runat="server" CssClass="mGrid" DataKeyNames="id"  GridLines="None" AlternatingRowStyle-CssClass="alt"
                                                    AutoGenerateColumns="false">                
                                                    </asp:GridView>
                                                     <asp:GridView ID="GridView2" runat="server" CssClass="mGrid"  GridLines="None" AlternatingRowStyle-CssClass="alt"
                                                    AutoGenerateColumns="false" >                
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </table>
                                        </div>
                                    </td>
                               </tr>--%>
                        </div>
                    </td>
                </tr>
                <%

		     Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd1 As New System.Data.SqlClient.SqlCommand
		     Dim rdr1 As System.Data.SqlClient.SqlDataReader
		     Dim str1 As String
		     'Dim I As Integer
		     con1.Open()
                    str1 = "select * from tbl_Asset_reportcategory order by catcode"
		     cmd1.CommandText = str1
		     cmd1.Connection = con1
		     rdr1 = cmd1.ExecuteReader
		     While rdr1.Read
		                                 
              
                %>    
                <tr class="trheaderbg" style="background-color:Gray;">
                    <td>
                    <table border="0" align="left" width="100%">
                        <tr class="">
                            <td width="10px">
                                <img src="Images/down-arrow.png" id="img1<%=rdr1("id")%>"  onclick="fnStatus('Expand','div<%=rdr1("id")%>','img1<%=rdr1("id")%>','img2<%=rdr1("id")%>')" alt="Click to Expand"/>
                                <img src="Images/up-arrow.png" id="img2<%=rdr1("id")%>" style ="display:none" onclick="fnStatus('Collapse','div<%=rdr1("id")%>','img1<%=rdr1("id")%>','img2<%=rdr1("id")%>')" alt="Click to Collapse"/>
                            </td> 
                            <td align="left"><% =rdr1("catcode")%></td>                             
                        </tr>
                    </table>
                    </td>
                </tr>
                <tr class="whitebg">
                    <td class="tdtext" colspan="2">
                        <div id="div<%=rdr1("id")%>" style="display:none;">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" align="left">
                            <tr class="tdcolbg">
                                <td>
                            <table border="0" align="center" width="100%" cellpadding="4" cellspacing="1">
                		 <%
		   
		     Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd As New System.Data.SqlClient.SqlCommand
		     Dim rdr As System.Data.SqlClient.SqlDataReader
		     Dim con2 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
		     Dim cmd2 As New System.Data.SqlClient.SqlCommand
		     Dim rdr2 As System.Data.SqlClient.SqlDataReader
                		     Dim str As String
                		       Dim altcolor As Integer = 1
		     'Dim I As Integer
		     con.Open()
                		     str = "select * from tbl_Asset_reportformula where rights = '1' and repcatid=" & rdr1("id") & " order by id DESC"
		     cmd.CommandText = str
		     cmd.Connection = con
		     rdr = cmd.ExecuteReader
		     If rdr.HasRows Then
                		         While rdr.Read
                		             Dim rtype As String
                		             Dim fid As String
                		             Dim catid As String
                		             rtype = "User"
                		             fid = rdr("id")
                		             catid = rdr("assetcatid")
                		             
		        %>
                   <% If altcolor = 1 %>
		                        <tr class="whitebg"> 
		                        <% altcolor = 2 %>          
		                    <% else %>
		                        <tr class="whitebg" style="background-color:#F8F8FF;">
		                        <% altcolor = 1 %>          
		                    <%End If %>
                        <td style="width:90%" align="left" class="tdtext">
                            <% =rdr("reportname")%>
                        </td>                        
                        <td style="width:5%" align="center" class="tdtext">
                            <img alt="Click To View Report" id="imgReports<%=rdr1("id")& rdr("id")%>" src="Images/viewreport1.png" 
                            onmouseover="this.style.cursor='hand'" onclick="NavigatetoReports('<%=rtype%>',<%=fid%>,<%=catid%>,'1','A');" 
                            style="height: 16px"/>
                        </td>
                        <td style="width:5%" align="center" class="tdtext">  
                            <img alt="Click To View Report" id="img1" src="Images/xlsicon.gif" onmouseover="this.style.cursor='hand'" style="height: 16px"/ onclick="NavigatetoReports('<%=rtype%>',<%=fid%>,<%=catid%>,'2','A');">
<%--                            <a href="ViewReport.aspx?RType=User&Fid=<%=rdr("id")%>&Catid=<%=rdr("assetcatid")%>" id="Hy<%=rdr1("id")& rdr("id")%>" ><% =rdr("reportname")%></a>
--%>                    </td>
                    </tr>
                <% 
                End While
                rdr.Close()
                con2.Close()
            End If
                %>
                        </table>
                                </td>
                            </tr>
                        </table>
                        </div>
                    </td>
                </tr>  
            <% 	        
            End While
            rdr1.Close()
	        con1.Close()
	       	        %>  
       	     <% Else If Request.QueryString("Rpt") = "E" then %>   
       <tr class="trheaderbg" style="background-color:Gray;">
                    <td>
                    <table border="0" align="center" width="100%" cellpadding="4" cellspacing="1">
                        <tr class="">
                            <td width="10px">
                                <img src="Images/down-arrow.png" id="img123"  onclick="fnStatus('Expand','div12','img123','img234')" alt="Click to Expand"/>
                                <img src="Images/up-arrow.png" id="img234" style ="display:none" onclick="fnStatus('Collapse','div12','img123','img234')" alt="Click to Collapse"/>
                            </td> 
                            <td align="left">Exclusive Reports</td>                             
                        </tr>
                    </table>
                    </td>
                </tr>
                <tr class="whitebg">
                    <td class="tdtext" colspan="2">
                        <div id="div12" style="display:none;">
                         <table border="0" cellpadding="0" cellspacing="0" width="100%" align="left">
                            <tr class="tdcolbg">
                                <td>
                            <table border="0" align="center" width="100%" cellpadding="4" cellspacing="1">
                		 <%
		   
                		     Dim con3 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
                		     Dim cmd3 As New System.Data.SqlClient.SqlCommand
                		     Dim rdr3 As System.Data.SqlClient.SqlDataReader
                		     Dim str3, I As String
                		      Dim altcolor As Integer = 1
		             		 con3.Open()
                		     str3 = "select * from tbl_Asset_reportformula where createdby='" & Convert.ToString(Session("EmpNo")) & "' order by id desc "
                		     cmd3.CommandText = str3
                		     cmd3.Connection = con3
                		     rdr3 = cmd3.ExecuteReader
                		     If rdr3.HasRows Then
                		         While rdr3.Read
                		             Dim rtype As String
                		             Dim fid As String
                		             Dim catid As String
                		             rtype = "User"
                		             fid = rdr3("id")
                		             catid = rdr3("assetcatid")
                		             
		        %>
                   <% If altcolor = 1 %>
		                        <tr class="whitebg"> 
		                        <% altcolor = 2 %>          
		                    <% else %>
		                        <tr class="whitebg" style="background-color:#F8F8FF;">
		                        <% altcolor = 1 %>          
		                    <%End If %>
                        <td style="width:90%" align="left" class="tdtext">
                            <% =rdr3("reportname")%>
                        </td>                        
                        <td style="width:5%" align="center"  align="left" class="tdtext">
                            <img alt="Click To View Report" id="imgReports<%=rdr3("id")%>" src="Images/viewreport1.png" 
                            onmouseover="this.style.cursor='hand'" onclick="NavigatetoReports('<%=rtype%>',<%=fid%>,<%=catid%>,'1','E');" 
                            style="height: 16px"/>
                        </td>
                        <td style="width:5%" align="center"  align="left" class="tdtext">  
                            <img alt="Click To View Report" id="img133" src="Images/xlsicon.gif" onmouseover="this.style.cursor='hand'" style="height: 16px"/ onclick="NavigatetoReports('<%=rtype%>',<%=fid%>,<%=catid%>,'2','E');">
<%--                            <a href="ViewReport.aspx?RType=User&Fid=<%=rdr("id")%>&Catid=<%=rdr("assetcatid")%>" id="Hy<%=rdr1("id")& rdr("id")%>" ><% =rdr("reportname")%></a>
--%>                    </td>
                    </tr>
                <% 
                End While
                rdr3.Close()
                con3.Close()
            End If
                %>
                        </table>
                       </td>
                       </tr>
                       </table>
                       
                        </div>
                    </td>
                </tr>     
                <% end if %>              
            </table>           
        </td>
    </tr>
</table>
</asp:Content>

