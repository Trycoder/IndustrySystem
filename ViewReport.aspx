<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="ViewReport.aspx.vb" Inherits="ViewReport" title="View Reports" %>

<%@ Import Namespace="System.Data.SqlClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
    function expandcollapse(obj,row)
    {
    
        var div = document.getElementById('div' + obj);
        var img = document.getElementById('img' + obj);
        
        if (div.style.display == "none")
        {
            div.style.display = "block";            
            img.src = "images/collapse.gif";
            img.alt = "Close to Hide Details";
        }
        else
        {
            div.style.display = "none";
            img.src = "images/expand.gif";
            img.alt = "Click to Show Details";
        }
    } 
</script>
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="95%" >
 <tr class="tdcolbg">
<td>
<table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
    <tr  class="trheaderbg">
    <td >
  View Report 
</td>
</tr>
    <tr class="whitebg">
<td class="tdtext">
  <asp:LinkButton ID="lnkback" runat="server" Text="Back"></asp:LinkButton>
</td>
</tr>
    <tr class="whitebg">
<td class="tdtext">
<table>
<tr>
<td class="tdtextbold" align="right">Report Name :</td>
<td class="tdtext"><asp:Label ID="lblreportname" runat="server" ></asp:Label></td>
</tr>
<tr>
<td class = "tdtextbold" align="right">Condition :</td>
<td class = "tdtext"><asp:Label ID="lblcondition" runat="server"></asp:Label></td>
</tr>
</table>
</td>
</tr>
	<tr class="whitebg">
	    <td  align="center" style="width:100%">
	        <asp:GridView ID="grdreports" runat="server"  DataKeyNames="id" 
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" Width="100%" >
            <Columns>
                        <asp:TemplateField>
                        <HeaderTemplate>Sno</HeaderTemplate>
                        <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    </asp:TemplateField> 
<%--            <asp:TemplateField><ItemTemplate>
                <table><tr>
                <td align="left" colspan="100%">
                <div id='div1<%# Eval("Name") %>' 
                        style="display:block;position:relative;left:32px;OVERFLOW: auto; top:-10px; WIDTH:97%; border-style:NONE;">
                <asp:GridView ID="grdreports1" runat="server" AllowPaging="false" 
                        AllowSorting="FALSE" AutoGenerateColumns="false" Font-Names="arial" 
                        GridLines="None" HorizontalAlign="right" ShowFooter="false" Showheader="false" 
                        Width="100%">

                <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="left" ItemStyle-Width="100%">
                <ItemTemplate>
                            <a id="lnk_assetattCode" 
                        href='asset_count_details.aspx?transtype=u&amp;assettypecode=u' 
                        style="text-align: center; text-decoration:none; color:#736F6E; font-weight:normal;"> </a>   

                
                    </ItemTemplate>
                    
                 </asp:TemplateField>
                 
                    </Columns>
                    
                    </asp:GridView>
                 </div>
                 </td>
                 </tr></table>
               </ItemTemplate>

                <ItemStyle Width="0%"></ItemStyle>
            </asp:TemplateField>--%>
           </Columns>
            </asp:GridView>	   
            
           <asp:GridView ID="GridView1" runat="server"  
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AutoGenerateColumns="false" Width="100%" >
            <Columns>
<%--                      
            <asp:TemplateField  HeaderText="Details" ItemStyle-Width="6%" HeaderStyle-BorderColor="Transparent">
            <ItemTemplate>
             <a href="javascript:expandcollapse('<%# Eval("name") %>', 'one');">
                <img id="img<%# Eval("name") %>" alt="Click to show/hide Employees under <%# Eval("name") %>"   border="0" src="images/expand.gif"/>
            </a>
            </ItemTemplate>
        </asp:TemplateField>

  <asp:TemplateField ItemStyle-Width="0%">
            <ItemTemplate>
<%--
            <tr>
            <td colspan="100%" align="left">
            <div id="div<%# Eval("name") %>" style="display:none;position:relative;left:2px;OVERFLOW: auto;WIDTH:97%">
            <asp:GridView ID="GridView2" Width="45%" AutoGenerateColumns="false" class="tabulardata" runat="server" HorizontalAlign  ="right" >
           <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="center" BackColor="#ffe89a" Font-Bold="True" ForeColor="Black" Font-Underline="false" />
        <AlternatingRowStyle BackColor="#ebedf0" />
            <Columns>
               <asp:TemplateField HeaderText="" ItemStyle-Bordercolor="Transparent" HeaderStyle-BorderWidth = "1" HeaderStyle-BorderColor="Transparent" ItemStyle-HorizontalAlign="center" ItemStyle-Width="0%">
                <ItemTemplate>
                    <asp:Label runat="server" ID="Sno"></asp:Label>
                </ItemTemplate>
              </asp:TemplateField>
            <asp:TemplateField HeaderText="Category " SortExpression="Category" HeaderStyle-HorizontalAlign ="Left" ItemStyle-HorizontalAlign = "Left">
                <ItemTemplate> 
                   <%-- <%#Eval("name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sub Category" SortExpression="Subcatname" HeaderStyle-HorizontalAlign ="Left" ItemStyle-HorizontalAlign = "Left">
                <ItemTemplate>
               <%--<%#Eval("name")%>
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
            </asp:GridView>
            </div>
<%--            </td>
            </tr>
            </ItemTemplate>			       
        </asp:TemplateField> --%>  
            <asp:TemplateField>
                        <HeaderTemplate>Sno</HeaderTemplate>
                        <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    </asp:TemplateField> 
            </Columns>
            </asp:GridView>
            <table cellpadding="0" cellspacing="0" style="border-collapse:collapse;border:1px solid #ccccff;" border="1" width = "100%"  >
            <tr>
            <td>
           
            <%  RType = Request.QueryString("RType")
                If UCase(RType) = UCase("User") Then
                    Dim reportheaderarr() As String = ReportHeaderDesc.Split("|")
                    %>
                    <tr class="trheaderbg">
                    <td>S.No</td>
                    <%
                        For j As Integer = 0 To reportheaderarr.Length - 1
                            cntrh = j + 1
                    %>
                            <td style="font-weight:bold;"><%=reportheaderarr(j)%></td>         
                    <%                   
                    Next
                    %>
                              </tr>
                    <%
                        If ReportHeaders.Contains("v_emp") = True Or condition.Contains("v_emp") = True Or orderby.Contains("v_emp") = True Or ReportHeaders.Contains("BuildingUnit") = True Or ReportHeaders.Contains("Location") = True Or ReportHeaders.Contains("seatno") = True Then
                            V_Emp_Bool = True
                        End If
                        If ReportHeaders.Contains("v_trans") = True Or condition.Contains("v_trans") = True Or orderby.Contains("v_trans") = True Then
                            V_Trans_Bool = True
                        End If
                    
                    If V_Emp_Bool = True And V_Trans_Bool = True Then
                        SqlStr = "select "
                        If Len(Trim(orderby)) > 0 Then
                            SqlStr = SqlStr & orderby & ","
                        End If
                            SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset, view_SIP_Employees v_emp, view_asset_transactions v_trans"
                            SqlStr = SqlStr & " where v_asset.userid = v_emp.emp_number and v_asset.assetid  = V_trans.assetid  "
                    ElseIf V_Emp_Bool = True And V_Trans_Bool = False Then
                        SqlStr = "select "
                        If Len(Trim(orderby)) > 0 Then
                            SqlStr = SqlStr & orderby & ","
                        End If
                            SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset left join view_SIP_Employees v_emp on v_asset.userid = v_emp.emp_number  "
                            SqlStr = SqlStr & "    "
                    ElseIf V_Emp_Bool = False And V_Trans_Bool = True Then
                        SqlStr = "select "
                        If Len(Trim(orderby)) > 0 Then
                            SqlStr = SqlStr & orderby & ","
                        End If
                            SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset, view_asset_transactions v_trans "
                            SqlStr = SqlStr & " where v_asset.assetid  = V_trans.assetid  "
                    Else
                        SqlStr = "select "
                        If Len(Trim(orderby)) > 0 Then
                            SqlStr = SqlStr & orderby & ","
                            End If
                            If ReportHeaders = "" Then
                                SqlStr = SqlStr.Remove(SqlStr.Length - 1, 1)
                                SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset "
                            Else
                                SqlStr = SqlStr & ReportHeaders & " from view_assetmaster_status v_asset "
                            End If
                            SqlStr = SqlStr & " where 1 = 1  "
                        End If
                        If SqlStr.Contains("v_emp.Emp_Name") Then
                            SqlStr = SqlStr.Replace("v_emp.Emp_Name", "v_emp.Emp_Name + ' ' + V_emp.Emp_Initial")
                        End If
                        If SqlStr.Contains("v_emp.BuildingUnit") Then
                            SqlStr = SqlStr.Replace("v_emp.BuildingUnit", "v_emp.BuildingUnit + '-' + v_emp.seatno")
                        End If
                        If Len(Trim(condition)) > 0 Then
                            If SqlStr.Contains("where") Then
                                SqlStr = SqlStr & " and " & condition & " "
                            Else
                                SqlStr = SqlStr & " where " & condition & " "
                            End If
                        End If
                        If Len(Trim(assettypes)) > 0 Then
                            SqlStr = SqlStr & " and v_asset.assettypeid " & assettypes & " "
                        End If
                        
                        If ViewState("status") IsNot Nothing Then
                            SqlStr = SqlStr & " and v_asset.assettypeid " & assettypes & " and  " & ViewState("status")
                        End If
                            
                        If Len(Trim(orderby)) > 0 Then
                            If orderby.Contains("assettypecode") = True then
                                 SqlStr = SqlStr & " order by v_asset.assettypecode "    
                            Else
                                SqlStr = SqlStr & " order by isnull(" & orderby & ",'') "     
                            End If
                        End If
                         sqlcon.Open()
                        sqlcmd = New SqlCommand
                    sqlcmd.Connection = sqlcon
                    sqlcmd.CommandType = Data.CommandType.Text
                    sqlcmd.CommandText = SqlStr
                        Try
                            drAsset = sqlcmd.ExecuteReader
                        Catch ex As Exception
                            Dim myscript As String = "alert('Error - " & ex.Message.ToString() & "');"
                            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)
                            Exit Sub
                        End Try
                        Dim m As Integer = 1
                         Dim altcolor As Integer = 1
                        If drAsset.HasRows Then
                           While drAsset.Read
                            %>
                               <% If altcolor = 1 %>
		                        <tr class="whitebg"> 
		                        <% altcolor = 2 %>          
		                    <% else %>
		                        <tr class="whitebg" style="background-color:#F8F8FF;">
		                        <% altcolor = 1 %>          
		                    <%End If %>
                             <%
                             If orderby <> "" then
                             
                                If TempOrderBy = "" Or TempOrderBy <> Convert.ToString(drAsset(0)) Then
                            %>
                             
                                <td class = "tdtextbold"   style="font-weight:bold;font-size:11px;padding-left:2px;" align="left" colspan ="<%=cntrh + 1%>"  >
                                <%=drAsset(0)%>
                                </td>
                                </tr>
                                 <% If altcolor = 1 %>
		                        <tr class="whitebg"> 
		                        <% altcolor = 2 %>          
		                    <% else %>
		                        <tr class="whitebg" style="background-color:#F8F8FF;">
		                        <% altcolor = 1 %>          
		                    <%End If %>
                                <td class = "tdtext"><%=m%></td>
                            <%
                                TempOrderBy = drAsset(0)
                            Else
                                %> 
                                   <td class = "tdtext"><%=m%></td> 
                                <%
                            End If
                            Else
                             %> 
                                   <td class = "tdtext"><%=m%></td> 
                                <%
                            End if
                            m = m + 1
                            If orderby.Length > 0 then
                              For i = 1  To cntrh 
                                %>
                                <td class = "tdtext" align = "left">
                                <%=drAsset(i)%>
                                </td>
                            <%
                            Next
                            Else
                              For i = 0  To cntrh -1
                             %>
                                <td class = "tdtext" align = "left">
                                <%=drAsset(i)%>
                                </td>
                            <%
                            Next
                          End if
                            %>
                              </tr>
                            <%
                        End While
                            
                    End If
                    sqlcmd.Dispose()
                    drAsset.Close()
                End If
             %>
            </td>
            </tr>
            </table>
            </td>
           </tr>
           </table>
            </td>
	</tr>
	</table>
	</asp:Content>

