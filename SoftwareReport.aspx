<%@ Page Title="Software Report" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="SoftwareReport.aspx.vb" Inherits="SoftwareReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="upanel1" runat="server">
<ContentTemplate>
  <table align="center" cellpadding="0" cellspacing="0" width="90%">
<tr class="tdcolbg">
    <td valign="middle">
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	<tr class="trheaderbg">
		<td  colspan = "2">	 Add New Report 
		
		    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />	
              
		</td>
	</tr>
    <tr class="tdtext" runat="server" id="trmessage" visible ="false">
		<td colspan="4"><asp:Label ID="lblmessage" runat="server"></asp:Label></td>
	</tr>
<%--	<tr class="whitebg">
		<td style="width:20%" align="right" class="tdtext">Select Report Category :</td>
		<td style="width:780%;" class="tdtext">
        <asp:DropDownList ID="drpreportcategory" runat="server" CssClass="control" AutoPostBack="true">
        </asp:DropDownList>
        </td>
	</tr>--%>
<%--    <tr class="whitebg">
		<td align="right" class="tdtext">Report Name :</td>
		<td class="tdtext">
            <asp:TextBox ID="txtreportname" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Enter Report Name" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="txtreportname" 
                Display="None"></asp:RequiredFieldValidator>
        </td>
	</tr>--%>
    <tr class="whitebg">
		<td align="right" class="tdtext">Select Software Category :</td>		
		<td class="tdtext">
		<table>
		<tr>
		<td>
		 <asp:DropDownList ID="drpassetcategory" runat="server" AutoPostBack="True" 
                CssClass="control">
                 <asp:ListItem value="0">--Select--</asp:ListItem>
          </asp:DropDownList>
		</td>
		<td><asp:CheckBox ID="chkall" runat="server" Text="All Asset Category" 
                AutoPostBack="True" /></td>
		</tr>
		</table>
		 
		</td>
	</tr>
    <tr class="whitebg">
		<td align="right" class="tdtext">Asset Type :</td>		
		<td class="tdtext">
		    <asp:CheckBoxList ID="chkassettype" runat="server" CssClass="control" 
                RepeatDirection="Horizontal" RepeatColumns="6" AutoPostBack="True">
            </asp:CheckBoxList>
		</td>
	</tr>
    <tr class="whitebg">
		<td align="right" class="tdtext">Report Header :</td>		
		<td class="tdtext">
		    <asp:CheckBoxList ID="chkreportheader" runat="server" CssClass="control" RepeatDirection="Horizontal" RepeatColumns="5">
		    
            </asp:CheckBoxList>
		</td>
	</tr>
	   <tr class="whitebg">
		<td align="right" class="tdtext">Access Rights :</td>		
		<td class="tdtext">
		<asp:CheckBox ID="chkrights" runat="server" />Visible To All Users	
		</td>
	</tr>
	   <tr class="whitebg">
		<td align="right" class="tdtext">Group By :</td>		
		<td class="tdtext">
		    <asp:DropDownList ID="drpgroupby" runat="server" 
                    CssClass="control" AutoPostBack="true">
                    <asp:ListItem Text="--Select--" Value="0"> </asp:ListItem>
                </asp:DropDownList>	
		</td>
	</tr>
    <tr class="whitebg">
		<td align="left" class="tdtext" colspan="2">
		<asp:UpdatePanel runat="server" ID="pnlcondition">
		<ContentTemplate>
		<table>
            <tr>
            <td class="tdtext">Condition</td>
            <td><asp:DropDownList ID="drpattributetype" runat="server" CssClass="control" AutoPostBack="true">
                </asp:DropDownList></td>
             <td><asp:DropDownList ID="DrpOperator" class="control" runat="server" Width ="100px" AutoPostBack="true" >
                        <asp:ListItem Text="--Operator--" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="=" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="<>" Value="2"> </asp:ListItem>
                        <asp:ListItem Text=">" Value="3"> </asp:ListItem>
                        <asp:ListItem Text="<" Value="4"> </asp:ListItem>
                        <asp:ListItem Text=">=" Value="5"> </asp:ListItem>
                        <asp:ListItem Text="<=" Value="6"> </asp:ListItem>
                        <asp:ListItem Text="Between" Value="7"> </asp:ListItem>
              </asp:DropDownList></td>
                 <td runat="server" id="tdcondition"></td>
                <td>
                <asp:DropDownList ID="DrpANDOR" class="control" runat="server" Width ="100px"  AutoPostBack="true">
                    <asp:ListItem Text="---" Value="0"> </asp:ListItem>
                    <asp:ListItem Text="OK" Value="1"> </asp:ListItem>
                    <asp:ListItem Text="AND" Value="2"> </asp:ListItem>
                    <asp:ListItem Text="OR" Value="3"> </asp:ListItem>
                 </asp:DropDownList>
                </td>
                <td runat="server" id="tdclearcondition">
                
                <asp:Button ID="btnclearcondtion" runat="server" Text="Clear" CssClass="lButton" Width="80px"/>
                
                </td>
                
            </tr>
            </table>
		</ContentTemplate>
		<Triggers>
		<asp:AsyncPostBackTrigger ControlID="DrpOperator" EventName="SelectedIndexChanged" />
		</Triggers>
		</asp:UpdatePanel>
	</td>		
	</tr>
	  <tr class="whitebg">
                <td class="tdtextbold" colspan="2">
                	  <asp:UpdatePanel ID="pnlconditiontext" runat="server">
	  <ContentTemplate>
	   &nbsp;Condition :
                    <asp:Label ID="LblCondText" Text ="" runat="server"></asp:Label>
                    <asp:Label ID="LblInvConText" Text ="" Visible = "false" runat="server"></asp:Label>
	  </ContentTemplate>
	  </asp:UpdatePanel>
               
                </td>
            </tr>
	<tr class="whitebg">
		<td align="center" class="tdtext" colspan="2">&nbsp;
	<asp:Button ID="btnviewreport" runat="server" Text="View Report" CssClass="lButton" Width ="80px" />&nbsp;&nbsp;
&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>
	</table>
	    </td>
	</tr>
	<%--<tr class="">
	    <td class="tdtext">
	    <table align="center" cellpadding="0" cellspacing="0" width="100%">
	        <tr class="">
	            <td>
                    <asp:GridView ID="grdformula" runat="server"  DataKeyNames="id" 
                    CssClass="mGrid"
                    GridLines="None"
                    AlternatingRowStyle-CssClass="alt"
                  AllowSorting="true"
                    AutoGenerateColumns="false" Width="100%">
                     <Columns>
                          <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                          <HeaderTemplate >S.No</HeaderTemplate>
                          <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Report Name" HeaderStyle-HorizontalAlign="Center" DataField="reportname" SortExpression="reportname"  />
                            <asp:BoundField HeaderText="Condition" HeaderStyle-HorizontalAlign="Center" DataField="condition" SortExpression="condition" />
                            <asp:BoundField HeaderText="Access Rights" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataField="rights" SortExpression="rights"  />
                          <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                          <HeaderTemplate>Delete</HeaderTemplate>
                          <ItemTemplate>
                          <asp:ImageButton ID="imgdelete" runat="server" CommandName="Deleting" CommandArgument='<%# Eval("id") %>' ImageUrl="~/Images/Delete.png" />
                          </ItemTemplate>
                             </asp:TemplateField>
                          <asp:TemplateField Visible="false">
                          <HeaderTemplate>User Id</HeaderTemplate>
                          <ItemTemplate>
                          <asp:Label ID="lblUser"  runat="server" Text='<%#Bind("createdby") %>'  ></asp:Label>
                          </ItemTemplate>
                             </asp:TemplateField>
                          </Columns>
                    </asp:GridView>	   
	            </td>
	        </tr>
	    </table>
	    </td>
	</tr>--%>
	</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

