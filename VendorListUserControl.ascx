<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VendorListUserControl.ascx.vb" Inherits="VendorListUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 <link href="CSS/slidingdoors.css" rel="stylesheet" type="text/css" />
    <link href="Css/ApplyCSS1.css" rel="stylesheet" type="text/css" />
    <link href="Css/grid.css" rel="Stylesheet" type="text/css" />
    <link href="Css/tabs.css" rel="stylesheet" type="text/css" />
    <link href="Css/ToolTip.css" rel="Stylesheet" type="text/css" />
    <link rel="Stylesheet" href="Css/panel.css" />


<asp:Panel ID="pnlPopup" runat="server" style="background-color:white" Height="400px" BorderColor="Black" BorderStyle="Solid" BorderWidth="3px">
       
    <table style="padding-top:5px;" width ="100%">
        <tr class="trheaderbg">
            <td align="center" style="width:95%;"> Search Employee</td>
            <td align="right" style="width:5%;"> 
                <asp:ImageButton ID="imgdelete" runat="server"  ImageUrl="~/Images/DeleteH.png" Width="30px" Height="30px" />
            </td>
        </tr>
    </table>
           
     
    <table style="padding-top:5px;" width =" "100%">
		<tr class="">
		    <td class ="tdtext;">Search By :</td>
		    <td class="tdtext">
		    <asp:DropDownList ID="drpreportsearch" runat="server" CssClass="control"> 
		         <asp:ListItem Value="empcode">Employee Code</asp:ListItem>
                 <asp:ListItem Value="empname">Employee Name</asp:ListItem>
            </asp:DropDownList></td>
		    <td class="tdtext">
		        <asp:TextBox ID="txtshsearch" runat="server" Width="150px" CssClass="control"></asp:TextBox>
		    </td>
		    <td class="tdtext">
		    <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="lButton" Width ="80px"  /></td>
</tr>
	        </table>
    
    
    <asp:GridView ID="GrdDetails" runat="server"  DataKeyNames="empid" AutoGenerateColumns="false" 
              CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            PagerStyle-CssClass="pgr"  
            AllowPaging="true" AllowSorting="false" EmptyDataText="No Data Found !">
      <Columns>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                        <HeaderTemplate>S.No</HeaderTemplate>
                        <ItemTemplate><%#Container.DataItemIndex + 1%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Code" DataField="empcode" SortExpression="empcode" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Employee Name" DataField="empname" SortExpression="empcode" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Department" DataField="deptname" SortExpression="empcode" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Designation" DataField="designame" SortExpression="empcode" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                      <HeaderTemplate>Edit</HeaderTemplate> 
                      <ItemTemplate>
                        <asp:HyperLink ID="imgedit" runat="server" ImageUrl="~/Images/edit.png" ></asp:HyperLink>
                      </ItemTemplate>
                  </asp:TemplateField>
      </Columns>              
          
    </asp:GridView>
  
    <asp:HiddenField ID="HiddenQuery" runat="server" />
    <asp:HiddenField ID="HiddenPageName" runat="server" />
</asp:Panel>
<p>

<asp:ModalPopupExtender ID="mpePopup" runat="server" TargetControlID="HiddenQuery"
    PopupControlID="pnlPopup"  Enabled="true">
</asp:ModalPopupExtender>
</p>