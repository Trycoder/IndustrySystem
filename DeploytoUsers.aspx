<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="DeploytoUsers.aspx.vb" Inherits="DeploytoUsers" title="Deploy to Users" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >
    <asp:UpdatePanel ID="upanel1" runat="server">
       <ContentTemplate>
            <table border="0" align="center" cellpadding="0" cellspacing="0"  width="100%">
                <tr class="tdcolbg">
                <td>
               <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">
                <tr  class="trheaderbg">
                <td colspan="4">
                    <div>Deploy to User(s)</div>
    			            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                            ShowMessageBox="True" ShowSummary="False" ValidationGroup="assets" />
                        </td>
               </tr>
               <tr  class="whitebg" runat="server" id="trmessage">
                 <td colspan="4">
                   <asp:Label ID="lblMessage" runat="server"  class="red_text" ></asp:Label>
                 </td>
                   
               </tr>
                <tr class="whitebg">
                             <td align="right" class="blue_text" style="width:20%;">Name of Category :</td>
                             <td class="blue_text" style="width:30%;">
                                 <asp:DropDownList ID="drpCategory" runat="server" AutoPostBack="true" 
                                     CssClass="control" Width="100">
                                 </asp:DropDownList>
                             </td>
                             <td align="right" class="blue_text" style="width:20%;">
                                 Consumable Name:</td>
                             <td class="blue_text" style="width:30%;">
                                    <asp:DropDownList ID="drpConsType" runat="server" AutoPostBack="true" 
                                        CssClass="control" Width="100">
                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                             </td>
                 </tr>
                <tr class="whitebg">
                        <td align="right" class="blue_text" style="width:20%;">Select Location:</td>            
                        <td class="blue_text" colspan="3">
                            <asp:RadioButtonList ID="rdolocation" runat="server" AutoPostBack="true" 
                                RepeatColumns="3" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </td>

                </tr>
                   <tr class="whitebg" runat="server" id="trconsumables" visible = "false">
                       <td align="right" class="blue_text" style="width:20%;" valign="top">
                           &nbsp;ConsumableDetails :</td>
                       <td class="blue_text" align="left" colspan="3" runat="server" id="tdconsumables" valign="top">
                      </td>
                   </tr>
                <tr class="whitebg">
                        <td align="right" class="blue_text" style="width:20%;">
                           Available Quantity :</td>
                        <td class="blue_text" style="width:30%;">
                            <asp:Label ID="lblQuantity" runat="server"  CssClass="control" BorderStyle="None" ReadOnly="true" Width="90px"></asp:Label>
                        </td>
                    <td align="right" class="blue_text" style="width:20%;">Deployment Date :</td>
		            <td class="blue_text" style="width:30%;">
		            <asp:TextBox ID="txtdate" runat="server"  CssClass="control" ></asp:TextBox>
                       <asp:CalendarExtender ID="txtDeploydate_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtdate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                    </td>
               </tr>
                   <tr class="whitebg">
                       <td align="right" class="blue_text" style="width:20%;" valign="top">
                           User Name :</td>
                       <td class="blue_text" colspan="3">
                           <asp:ListBox ID="lstuser" runat="server" CssClass="control" Width="400px" 
                               Height="100px" SelectionMode="Multiple"></asp:ListBox>
                               <asp:ListSearchExtender ID="ListSearchExtender2" runat="server"  PromptCssClass="ListExtend" TargetControlID="lstuser"  ></asp:ListSearchExtender >
                       </td>
                   </tr>
                <tr class="whitebg">
                        <td  colspan="1" align="right" valign="top" class="blue_text">Remarks :</td>
                        <td colspan="3" class="blue_text">		
                            <asp:TextBox ID="txtremarks" runat="server" Height="50px" TextMode="MultiLine" 
                                Width="400px"></asp:TextBox>		
		                </td>
	            </tr>
               
                   <tr class="whitebg">
                       <td align="right" class="blue_text" colspan="1" valign="top">
                           &nbsp;</td>
                       <td class="blue_text" colspan="3">
                           <asp:Button ID="btn" runat="server" CssClass="lbutton" 
                               Text="Add Another Consumables" ValidationGroup="assets" Visible="true" 
                               Width="150px" />
                       </td>
                   </tr>
               
                <tr class="whitebg">
                         <td  colspan="4" class="blue_text" align="center" >
                            <asp:Button ID="btnsave" runat="server" Text="Save" ValidationGroup="assets" Visible ="true" CssClass="lbutton" Width ="80px" />
                            &nbsp;<asp:Button ID="btncancel" runat="server" Text="Cancel" Visible ="true" CssClass="lbutton" Width ="80px"  />
                        </td>
               </tr>
               </table>
               <table align="center" width="100%">
	                    <tr>
		                    <td class="blue_text" align ="center"><b></b>
                                <asp:GridView ID="GridView1" align="center" Width="100%" 
                                    AutoGenerateColumns = "False" AllowPaging="True" runat="server" 
                                    CellPadding="3" CssClass="mGrid" 
                                    GridLines="Vertical" HorizontalAlign="Center" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                <asp:TemplateField HeaderText="S.No" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate >
                                        <asp:Label runat="server" ID="Sno" ></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                  </asp:TemplateField>
                                
                                <asp:BoundField DataField="EmpNumber" HeaderText="EmployeeNumber" />
                                <asp:BoundField DataField="EmpName" HeaderText="EmployeeName"/>
                                <asp:BoundField DataField="ConsumableName" HeaderText="ConsumableName"/>
                                <asp:BoundField DataField="DeploymentDate" HeaderText="DeploymentDate"/>
                                <asp:BoundField DataField="Quantity" HeaderText="AssignQuantity"/>
                                <asp:BoundField DataField="Reason" HeaderText="Remarks"/>
                                </Columns>
                                
                                </asp:GridView>
                               
                            </td>
	             </tr>
	             </table> 
            </td>
            </tr>
            </table>
           
       </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

