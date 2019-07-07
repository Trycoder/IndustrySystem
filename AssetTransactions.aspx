<%@ Page Language="VB" EnableEventValidation="true" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="AssetTransactions.aspx.vb" Inherits="AssetTransactions" title="Asset Transactions" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script language="javascript" type="text/javascript">
 function fnStatus(stat)
    {
      //  alert(stat);
        if(stat=="Expand")
        {
            document.getElementById("divStatus").style.display="block";
            document.getElementById("imgExp").style.display="none";
            document.getElementById("imgCol").style.display="block";
        }
        else if(stat=="Collapse")
        {
            document.getElementById("divStatus").style.display="none";
            document.getElementById("imgCol").style.display="none";
            document.getElementById("imgExp").style.display="block";
        }
    }
 </script> 
<table border="0" align="center" cellpadding="0" cellspacing="0"  width="90%">
    <tr class="tdcolbg">
	    <td>
	    <asp:UpdatePanel ID="upanel3" runat="server">
             <ContentTemplate>
         <table border="0" align="center" cellpadding="4" cellspacing="1"  width="100%">
             <tr class="trheaderbg">
	    <td colspan="6">
            <asp:Label ID="lbltransactions" runat="server"></asp:Label>&nbsp;Transactions
        </td>	
	</tr>
	<tr class="tdtext" runat="server" id="trmessage" visible ="false">
		<td colspan="6"><asp:Label ID="lblmessage" runat="server"></asp:Label></td>
	</tr>
	       <tr class="whitebg">
                 <td align="right" class="tdtext" style="width:12%">
                     Category Code</td>
                 <td class="tdtext" style="width:15%">
                     <asp:DropDownList ID="drpcategory" runat="server" AutoPostBack="true" 
                         CssClass="control" ValidationGroup="assets">
                         <asp:ListItem Value="">--Select--</asp:ListItem>
                     </asp:DropDownList>
                </td>
                 <td align="right" class="tdtext" style="width:10%">
                     Asset Type</td>
                 <td class="tdtext" style="width:13%">
                     <asp:DropDownList ID="drpassettype" runat="server" AutoPostBack="true" 
                         CssClass="control">
                         <asp:ListItem Value="">--Select--</asp:ListItem>
                     </asp:DropDownList>
                </td>
             <td align="right" class="tdtext" style="width:15%">
             <span style="">Transaction Date :</span> </td>
             <td  align="left" class="tdtext" style="width:35%">
             		    <asp:TextBox ID="txtdate" runat="server" CssClass="control"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"  
                        Enabled="True" TargetControlID="txtdate" Format="dd-MMM-yyyy">
                    </asp:CalendarExtender></td>
             
           </tr>
           	<tr class="whitebg">
		            <td style="" align="right" class="tdtext">Asset No</td>
		            <td style="" class="tdtext" colspan="3">
            		                <asp:DropDownList ID="cboassetno" runat="server" CssClass="control" AutoPostBack="true">
                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                          &nbsp;&nbsp;
                  <asp:ListSearchExtender ID="ListSearchExtender1" runat="server"  PromptCssClass="ListExtend" TargetControlID="cboassetno" ></asp:ListSearchExtender >
                  <asp:LinkButton ID="lnkbulkupdate" runat="server"  Text="Bulk Update" 
                                        Width="80px" Visible="false" />
		            </td>
		         <td  align="left" class="tdtext" colspan="2">
		             &nbsp;</td>
		            
	        </tr>
	         <tr class="trheaderbg" runat="server"  id="truserdetailsheader" >
                 <td  colspan="4" >
                 Current User Details</td>
                 <td  colspan="2">
                 Target User Details</td>
             </tr>
	        
	        <tr  class="whitebg"  runat="server"  id="truserdetails">
	          
	               <td align="left" colspan="4" class="tdtext" valign="top">
	                  <table width="100%">
	                      <tr runat="server" id="trcuruser" visible="false">
	                      <td style="width:20%" align="right">User Name:</td>
	                      <td style="width:35%"> <asp:Label ID="lblcuruser" runat="server"></asp:Label></td>
	                      <td style="width:13%" align="right">Phone No:</td>
	                      <td style="width:22%"> <asp:Label ID="lblcurphone" runat="server"></asp:Label></td>
	                      </tr>
	                      <tr runat="server" id="trcurvendor" visible="false">
	                      <td align="right" style="width:20%">Vendor Name:</td>
	                      <td colspan="3"><asp:Label ID="lblvendor" runat="server"></asp:Label></td>
	                   
	                      </tr>
	                      <tr runat="server" id="trcurdept" visible="false">
	                      <td align="right">Department:</td>
	                      <td><asp:Label class = "controlNB" ID="lblcurdept" runat="server" Text=""></asp:Label></td>
	                      <td align="right">Location:</td>
	                      <td> <asp:Label class = "controlNB" ID="lblcurlocation" runat="server" Text=""> </asp:Label></td>
	                      </tr>
	                      <tr runat="server" id="trcurlocation" visible="false">
	                      <td align="right">Location:</td>
	                      <td colspan="3"><asp:Label class = "controlNB" ID="lblcuradminloc" runat="server"></asp:Label>  </td>
	                      </tr>
	                      <tr runat="server" id="trcursubloc" visible="false">
	                      <td align="right">Sub Location:</td>
	                      <td colspan="3"><asp:Label ID="lblcuradminsubloc" runat="server" class="controlNB"></asp:Label></td>
	                      </tr>
	                  </table>
	               </td>
	              <td align="left" colspan="2" class="tdtext" valign="top">
	                  <table width="100%">
	                      <tr runat="server" id="trtaruser" visible="false">
	                      <td style="width:30%" align="right">User Name:</td>
	                      <td style="width:70%" colspan="3" align="left"> 	               
	                           <asp:DropDownList ID="cbouser" runat="server" CssClass="control" AutoPostBack="true" Width="300px">
                                   <asp:ListItem Value="">--Select--</asp:ListItem>
                               </asp:DropDownList>
                                <asp:ListSearchExtender ID="ListSearchExtender2" runat="server"  PromptCssClass="ListExtend" TargetControlID="cbouser"  ></asp:ListSearchExtender >
                          </td>
	                      </tr>
	                      <tr runat="server" id="trtarvendor" visible="false">
	                      <td style="width:30%" align="right">Vendor Name:</td>
	                      <td style="width:70%" colspan="3"> 	               
	                           <asp:DropDownList ID="cbovendor" runat="server" AutoPostBack="true" 
                                        CssClass="control" Width="300px">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" 
                                        PromptCssClass="ListExtend" TargetControlID="cbovendor">
                                    </asp:ListSearchExtender>
                          </td>
	                      </tr>
	                      <tr runat="server" id="trtardept" visible="false">
	                      <td align="right" >
	                      Department:
	                      </td>
	                      <td colspan="3">
	                      <asp:Label class = "controlNB" ID="lbltrdept" runat="server" Text=""></asp:Label>&nbsp;&nbsp;
	                     
	                      <asp:Label class = "controlNB" ID="lbltrlocation" runat="server" Text=""> </asp:Label>
	                      </td>
	                     
	                      </tr>
	                      <tr runat="server" id="trtarloc" visible="false">
	                          <td align="right">Target Location:</td>
	                          <td colspan="3">
	                            <asp:DropDownList ID="drptargetloc" runat="server" CssClass="control" AutoPostBack="true" Width="300px">
                                <asp:ListItem Value="">--Select--</asp:ListItem></asp:DropDownList>
                                <asp:ListSearchExtender ID="ListSearchExtender12" runat="server"  PromptCssClass="ListExtend" TargetControlID="drptargetloc" ></asp:ListSearchExtender >
                              </td>
	                      </tr>
	                      <tr runat="server" id="trtarsubloc" visible="false">
	                            <td align="right">Target Sub Location:</td>
	                          <td colspan="3"> 
	                            <asp:DropDownList ID="drptargetsubloc" runat="server" CssClass="control"  Width="300px">
                                <asp:ListItem Value="">--Select--</asp:ListItem></asp:DropDownList>
                                <asp:ListSearchExtender ID="ListSearchExtender13" runat="server"  PromptCssClass="ListExtend" TargetControlID="drptargetsubloc" ></asp:ListSearchExtender >
	                          </td>
	                      </tr>
	                  </table>
	               </td>
    
              </tr> 
            <tr class="whitebg">
		          <td style="" align="right" class="tdtext">Last Trans Date:</td>
		          <td style="" class="tdtext" colspan="3">
		          <asp:Label ID="lblcurtransdate" runat="server" ></asp:Label>
               	  </td>
            	  <td style="" align="right" class="tdtext">Expected Return Date:</td>
		          <%If Request.QueryString("TransTag") = "2" Or Request.QueryString("TransTag") = "5" Then%>
		          <td style="" class="tdtext">
		              <asp:TextBox ID="txtreturndate" runat="server" CssClass="control"></asp:TextBox>
                       <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                        Format="dd-MMM-yyyy" TargetControlID="txtreturndate">
                       </asp:CalendarExtender>
                      
            	  </td>
            	   <% Else%>
                  <td align="right" class="tdtext">
                  </td>
                  <% End If%>
            	  
	        </tr>
	        <tr class="whitebg">
		          <td style="" align="right" class="tdtext">Last Trans Reason:</td>
		          <td style="" class="tdtext" colspan="3">
		            <asp:Label ID="lblcurtransreason" runat="server"></asp:Label>
            	  </td>
		         <td style="" align="right" class="tdtext">Select Reason:
		         </td>
		          <td style="" class="tdtext">
                        <asp:DropDownList ID="drpmaintanance" runat="server" CssClass="control"  Width="350px">
                             <asp:ListItem Value="">--Select--</asp:ListItem>
                        </asp:DropDownList>
            	  </td>
		         
	        </tr>
	        <tr class="whitebg">
		          <td align="right" class="tdtext" valign="top">Last Trans Remarks:</td>
		          <td class="tdtext" colspan="3" valign="top">
		            <asp:Label ID="lblcurlastremarks" runat="server" Width="250px"></asp:Label>
            	  </td>
		         <td  align="right" class="tdtext" valign="top">Remarks:
		         </td>
		          <td class="tdtext">
                       <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" Width="350px" Height="50px"></asp:TextBox>
            	  </td>
		         
	        </tr>
            <tr class="whitebg">
                                <td  align="left" class="tdtext" colspan="6" 
                                    style="padding-left:400px;">
                                    <asp:Button ID="btnsubmit" ValidationGroup="assets" runat="server" CssClass="lButton" Text="Save" 
                                        Width="80px" />
                                    &nbsp;
                                    <asp:Button ID="btncancel" runat="server" CssClass="lButton" Text="Cancel" 
                                        Width="80px" />

                                </td>
             </tr>

<%  If cboassetno.selectedvalue <> "" Then%>
<tr class="whitebg">
    <td colspan = "6" align="left"  style="border-right:none;" class="tdtext">
    <table border="0" style="border-collapse: collapse; color: #000080" align="left" width="100%" id="table2">
    <tr class="whitebg">
    <td width= "1%">
    <img src="Images/down-arrow.png" id="imgExp" onclick="fnStatus('Expand')" alt="Click to Expand" style="cursor:pointer; display:block" />
    <img src="Images/up-arrow.png" id="imgCol" onclick="fnStatus('Collapse')" alt="Click to Collapse" style="cursor:pointer; display:none" />
    </td>
    <td width= "99%" align = "left" style="border-bottom:none; font-size:11px; font-weight:bold; color:blue">Asset Details</td>
    </tr>
    <tr class="whitebg">

    <td  colspan = "2">
    <div id="divStatus">
    <%
       
        Dim con As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Dim con1 As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("IMS").ConnectionString)
        Dim cmd1 As New System.Data.SqlClient.SqlCommand
        Dim rdr1 As System.Data.SqlClient.SqlDataReader

        Dim CurrrStat As String
        con.Open()
        cmd.CommandText = "select * from tbl_asset_status where assetid =" & cboassetno.SelectedValue
        cmd.Connection = con
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                If rdr!status = "S" Then
                    CurrrStat = "Stock"
                ElseIf rdr!status = "N" Then
                    CurrrStat = "New"
                ElseIf rdr!status = "R" Then
                    CurrrStat = "Repair(Inhouse)"
                ElseIf rdr!status = "O" Then
                    CurrrStat = "Repair(Outside)"
                ElseIf rdr!status = "U" Then
                    CurrrStat = "Deployed to User"
                ElseIf rdr!status = "E" Then
                    CurrrStat = "Expired"
                ElseIf rdr!status = "X" Then
                    CurrrStat = "Sold"
                End If
            End While
        End If
        rdr.Close()
        con.Close()
        
        %>
   
            <b style="font-size:12px;">Current Status : <%=CurrrStat%> </b>
                 <asp:TabContainer ID="tabReportinfo"  width="100%" 
         cssclass="yui"   runat="server" ActiveTabIndex="3">
            <asp:TabPanel border = "1px" ID="Tb1" HeaderText="Technical Specifications" runat="server">
                <ContentTemplate>
                            <asp:DetailsView ID="grdassets" runat="server"  CssClass="mGrid"  GridLines="None"
                                AllowPaging="false" AutoGenerateColumns="false" Width="100%"   AlternatingRowStyle-CssClass="alt">
                            </asp:DetailsView>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel border = "1px" ID="Tb2" HeaderText="Deployment Details" runat="server" >
                <ContentTemplate>
               <asp:GridView ID="grddeployments" runat="server"             CssClass="mGrid"
            GridLines="None"   EmptyDataText="No Deployment/Undeployment Details Available In This Asset"
             AllowSorting="True"
            AutoGenerateColumns="False" Width="100%"  >
                  <Columns>
                    <asp:TemplateField HeaderText="S.No" ItemStyle-Width="20px">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1%>
                        </ItemTemplate>
                         <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField SortExpression="Emp_Name" DataField="Emp_Name"  HeaderStyle-Width="150px" HeaderText="Employname" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                    <asp:BoundField SortExpression="TransType" DataField="TransType"   HeaderStyle-Width="150px" HeaderText="Tranaction Type" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                    <asp:BoundField SortExpression="Deploydate" DataField="Deploydate"  HeaderStyle-Width="100px" HeaderText="Transaction Date">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                    <asp:BoundField SortExpression="ConsultantName" DataField="ConsultantName" HeaderText="Consultatnt Name" HeaderStyle-Width="170px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField SortExpression="remarks" DataField="remarks"  HeaderText="Remarks" HeaderStyle-Width="250px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                       </asp:BoundField>
                    </Columns>
                  <AlternatingRowStyle CssClass="alt" />
            </asp:GridView>
              </ContentTemplate>
            </asp:TabPanel>
               
            <asp:TabPanel border = "1px" ID="Tb3" HeaderText="Maintenance History" runat="server">
                <ContentTemplate>
                     <asp:GridView ID="grdmaintainance" runat="server"             CssClass="mGrid"
            GridLines="None"   EmptyDataText="No Repair/Return History in this Asset"
             AllowSorting="True"
            AutoGenerateColumns="False" Width="100%"  >
                  <Columns>
                    <asp:TemplateField HeaderText="S.No" ItemStyle-Width="20px">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1%>
                        </ItemTemplate>
                         <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField SortExpression="Username" DataField="Username"  HeaderStyle-Width="150px" HeaderText="Vendor/User Name" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                    <asp:BoundField SortExpression="TransType" DataField="TransType"   HeaderStyle-Width="150px" HeaderText="Tranaction" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                    <asp:BoundField SortExpression="RepairDate" DataField="RepairDate"  HeaderStyle-Width="100px" HeaderText="Repair/Return Date">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                    <asp:BoundField SortExpression="ConsultantName" DataField="ConsultantName" HeaderText="Consultatnt Name" HeaderStyle-Width="170px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField SortExpression="remarks" DataField="remarks"  HeaderText="Remarks" HeaderStyle-Width="250px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                       </asp:BoundField>
                    </Columns>
                  <AlternatingRowStyle CssClass="alt" />
            </asp:GridView>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel border = "1px" ID="Tb4" HeaderText="Complaints" runat="server">
                <ContentTemplate>
                <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
	<tr class="whitebg">
		<td class="tdtext">
            <asp:GridView ID="grdcompliants" runat="server"             CssClass="mGrid"
            GridLines="None"   DataKeyNames="complaintid" EmptyDataText="No Compliants Available In This Asset"
             AllowSorting="True"
            AutoGenerateColumns="False" Width="100%"  >
                  <Columns>
                    <asp:TemplateField HeaderText="S.No"  ItemStyle-Width="20px">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1%>
                        </ItemTemplate>
                         <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField SortExpression="Emp_Name" DataField="Emp_Name"  HeaderStyle-Width="150px"
                          HeaderText="Employee Name" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                   <asp:TemplateField HeaderText="Compliant" SortExpression="complaint"  HeaderStyle-Width="200px">
                  <ItemTemplate>
                  <a target="_blank" href="//SIPSV0020/Applications/HelpDesk/ComplaintConsView.aspx?CId=<%#Eval("complaintid")%>"><%#Eval("complaint")%></a>
                     </ItemTemplate>
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="left" />
                     </asp:TemplateField>  
                    <asp:BoundField SortExpression="cdate" DataField="cdate"   HeaderStyle-Width="75px"
                          HeaderText="Date" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                    <asp:BoundField SortExpression="closetag" DataField="closetag"  HeaderStyle-Width="30px"
                          HeaderText="Status">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                    <asp:BoundField SortExpression="ConsultantName" DataField="ConsultantName" 
                          HeaderText="Consultatnt Name" HeaderStyle-Width="150px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                    </Columns>
                  <AlternatingRowStyle CssClass="alt" />
            </asp:GridView>
           </td>
	</tr>
</table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel border="1px" ID="TabPanel1" HeaderText="Warranty" runat="server">
                <ContentTemplate>
                    <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
                        <tr class="whitebg">
                            <td class="tdtext">
                                <asp:GridView ID="grdwarranty" runat="server" CssClass="mGrid" GridLines="None" EmptyDataText="No warranty details in this Asset"
                                    AllowSorting="True" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField SortExpression="att1" DataField="att1" HeaderStyle-Width="100px"
                                            HeaderText="Asset Number">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="pono" DataField="pono" HeaderStyle-Width="150px" HeaderText="Purchase Order">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         <asp:BoundField SortExpression="Warrantystart" DataField="Warrantystart" HeaderStyle-Width="100px" HeaderText="Warranty Start">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="warranty" DataField="warranty" HeaderStyle-Width="100px"
                                            HeaderText="Warranty End">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="contractno" DataField="contractno" HeaderStyle-Width="100px"
                                            HeaderText="Contract No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="vendor" DataField="vendor" HeaderStyle-Width="100px"
                                            HeaderText="Vendor">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField SortExpression="ConsultantName" DataField="ConsultantName" HeaderText="Consultatnt Name"
                                            HeaderStyle-Width="150px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass="alt" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>

        </asp:TabContainer>
         
    </div>
    </td>
    </tr>
    </table>
    </td>
</tr>
<%End If%>
</table>    
             </ContentTemplate>                   	    	    
	     </asp:UpdatePanel>
    </td>
    </tr>
    </table>
</asp:Content>