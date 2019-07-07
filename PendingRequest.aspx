<%@ Page Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="PendingRequest.aspx.vb" Inherits="PendingRequest" title="Pending Request" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" align="center" cellpadding="0" cellspacing="0"  width="95%" >
    <tr class="tdcolbg">
<td>
  <table border="0" align="center" cellpadding="4" cellspacing="1" width="100%">	
	<tr class="trheaderbg">
		<td colspan="2">Pending Request/Issue
		</td>
	</tr>
    </table>
</td>
</tr>
	<tr class="whitebg">
	    <td colspan="2" align="center" style="width:100%">
            <asp:GridView ID="grdassets" runat="server"  DataKeyNames="id" 
            CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            AllowPaging="false"
            AllowSorting="true"
            AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found !" >
              <Columns>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>S.No</HeaderTemplate>
                  <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AssetType">
                             <ItemTemplate>
                                 <asp:Label id="lblassettype" CssClass="tdtext" runat ="server" text='<%# Eval("assettypecode")%>' />
                             </ItemTemplate>
                      </asp:TemplateField> 
                     <asp:BoundField HeaderText="Cond#1" DataField="att1" SortExpression="att1" />
                     <asp:BoundField HeaderText="Cond#2" DataField="att2" SortExpression="att2" />
                     <asp:BoundField HeaderText="Cond#3" DataField="att3" SortExpression="att3" />
                     <asp:BoundField HeaderText="Location" DataField="location" SortExpression="location" />
                     <asp:BoundField HeaderText="SubLocation" DataField="sublocation" SortExpression="sublocation" />
                     <asp:BoundField HeaderText="Deploy Type" DataField="deploytype" SortExpression="deploytype" />
                     <asp:BoundField HeaderText="Request By" DataField="requestedby" SortExpression="requestedby" />
                     <asp:BoundField HeaderText="Req Date" DataField="reqdate" SortExpression="reqdate" />
                     <asp:BoundField HeaderText="Complaint By" DataField="CompBy" SortExpression="CompBy" />
                     <asp:BoundField HeaderText="Comp Date" DataField="compdate" SortExpression="compdate" />
                     <asp:BoundField HeaderText="Avail.Qty" DataField="qty" SortExpression="qty" />
                     <asp:BoundField HeaderText="Quantity" DataField="qty" SortExpression="qty" />
                   <asp:TemplateField ItemStyle-HorizontalAlign="Center" >
                  <HeaderTemplate>Approve</HeaderTemplate> 
                  <ItemTemplate>
                     <asp:CheckBox ID="chkapproved" runat="server" />
                  </ItemTemplate>
             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                   </asp:TemplateField>  
                 <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>Issue</HeaderTemplate> 
                  <ItemTemplate>
                     <asp:CheckBox ID="chkissued" runat="server" />
                  </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                   </asp:TemplateField>  
                 <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                  <HeaderTemplate>Reject</HeaderTemplate> 
                  <ItemTemplate>
                     <asp:CheckBox ID="chkreject" runat="server" />
                  </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                   </asp:TemplateField>  
<%--                    <asp:BoundField HeaderText="Constypeid" DataField="assettypeid" SortExpression="assettypeid" />
                     <asp:BoundField HeaderText="locid" DataField="locid" SortExpression="locid" />
                      <asp:BoundField HeaderText="sublocid" DataField="sublocid" SortExpression="sublocid" />--%>
                    <asp:TemplateField Visible="false">
                             <ItemTemplate>
                                 <asp:Label id="lblconstypeid" runat ="server" text='<%# Eval("assettypeid")%>' />
                             </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField Visible="false">
                             <ItemTemplate>
                                 <asp:Label id="lbllocid" runat ="server" text='<%# Eval("locid")%>' />
                             </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField Visible="false">
                             <ItemTemplate>
                                 <asp:Label id="lblsublocid" runat ="server" text='<%# Eval("sublocid")%>' />
                             </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField Visible="false">
                             <ItemTemplate>
                                 <asp:Label id="lblapp" runat ="server" text='<%# Eval("apptag")%>' />
                             </ItemTemplate>
                      </asp:TemplateField>
                     <asp:TemplateField Visible="false">
                             <ItemTemplate>
                                 <asp:Label id="lblissue" runat ="server" text='<%# Eval("isstag")%>' />
                             </ItemTemplate>
                      </asp:TemplateField>  
                      <asp:TemplateField Visible="false">
                             <ItemTemplate>
                                 <asp:Label id="lblcid" runat ="server" text='<%# Eval("complaintid")%>' />
                             </ItemTemplate>
                      </asp:TemplateField>  
                   </Columns>
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
           </asp:GridView>	   
       </td>
	</tr>
		<tr class="whitebg">
		<td colspan="2" align="center" class="tdtext">&nbsp;
		<asp:Button ID="btnUpdate" runat="server" Text="Update" 
               CssClass="lButton" Width ="80px" />&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	</tr>
	</table>
</asp:Content>

