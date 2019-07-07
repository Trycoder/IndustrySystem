<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="CompanyInfo.aspx.vb" Inherits="CompanyInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript"src="JS/jquery-3.3.1.min.js"></script>
   
<table align="center" cellpadding="0" cellspacing="0" width="60%">
<tr class="tdcolbg">
    <td valign="top">
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="trheaderbg">
		    <td  colspan = "4">		 Company Information
		        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		    </td>
	    </tr>
    	
        <tr class="whitebg">
	    	<td align="right" style="width:20%;" class="tdtext">Company Code :</td>
		    <td style="width:40%;" class="tdtext">
                <asp:TextBox ID="TxtCompanyCode" runat="server" 
                Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                Display="None" ErrorMessage="Enter Company Code" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtCompanyCode"></asp:RequiredFieldValidator>
            </td>
            <td style="width:40%;" class="tdtext" rowspan="5" colspan="2">

                <table width="100%" class="tdtext" style="vertical-align:top; height: 207px;">
                    <tr align="center" style="height=10%;vertical-align:top">
                        <td style="font-size: small">
                            <strong><u>Company Logo</u>
                        </strong>
                        </td>
                    </tr>


                    <tr>
                        
                        <td align ="center">
                            <asp:FileUpload ID="ImgUpload" runat="server" style="display:none"/>
                        
                       
                        <asp:Image ID="Image1" runat="server" Height = "150 " Width = "150" />
                        </td>
                    </tr>

                    <tr>
                        <td align="center">
                            <%--<asp:LinkButton id="BtnLogo" runat="server" ><img src="Images/UpArrow.jpg" width="30px" height="30px" /></asp:LinkButton>--%>
                            <asp:ImageButton id="BtnLogo" runat="server" ImageUrl="~/Images/uparrow.jpg" />
                            <asp:ImageButton id="BtnPreview" runat="server" ImageUrl="~/Images/Undeploy.png" Height="25px" OnClick="BtnPreview_Click" />
                        </td>

                    </tr>

                </table>
                

            
            </td>
	    </tr>

        <tr class="whitebg">
	    	<td align="right" class="tdtext">Company Name :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="TxtCompanyName" runat="server" 
                Width="250px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                Display="None" ErrorMessage="Enter Company Name" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtCompanyName"></asp:RequiredFieldValidator>
            </td>
	    </tr>

        <tr class="whitebg">
	    	<td align="right"  class="tdtext">Display Name :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="TxtDisplayName" runat="server" 
                Width="250px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                Display="None" ErrorMessage="Enter Display Name" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtDisplayName"></asp:RequiredFieldValidator>
            </td>
	    </tr>

        
        <tr class="whitebg">
	    	<td align="right"  class="tdtext" valign="top">Address :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="TxtAddress1" runat="server" 
                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                Display="None" ErrorMessage="Enter Address1" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtAddress1"></asp:RequiredFieldValidator>
                &nbsp;
                <asp:TextBox ID="TxtAddress2" runat="server" 
                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                Display="None" ErrorMessage="Enter Address2" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtAddress2"></asp:RequiredFieldValidator>
                &nbsp;
                <asp:TextBox ID="TxtAddress3" runat="server" 
                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                Display="None" ErrorMessage="Enter Address3" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtAddress3"></asp:RequiredFieldValidator>
                &nbsp;
                <asp:TextBox ID="TxtAddress4" runat="server" 
                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                Display="None" ErrorMessage="Enter Address4" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtAddress4"></asp:RequiredFieldValidator>
           </td>
	    </tr>

    <tr class="whitebg">
	    	<td align="right" class="tdtext" valign="top">Phone :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="TxtPhone1" runat="server" 
                Width="200px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                Display="None" ErrorMessage="Enter Phone1" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtPhone1"></asp:RequiredFieldValidator>
                &nbsp;
                <asp:TextBox ID="TxtPhone2" runat="server" 
                Width="200px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                Display="None" ErrorMessage="Enter Address2" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtPhone2"></asp:RequiredFieldValidator>
            </td>
	    </tr>



        <tr class="whitebg">
	    	<td align="right"  class="tdtext" valign="top">Fax :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="Txtfax" runat="server" 
                Width="200px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                Display="None" ErrorMessage="Enter Phone1" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtFax"></asp:RequiredFieldValidator>
            </td>

            <td align="right" class="tdtext">GST No :</td>
		    <td class="tdtext">
                <asp:TextBox ID="TxtGSTNo" runat="server" 
                Width="250px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                Display="None" ErrorMessage="Enter GST No" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtGSTNo"></asp:RequiredFieldValidator>
            </td>

	    </tr>

        <tr class="whitebg">
	    	<td align="right" class="tdtext" valign="top">Email :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="TxtEmail" runat="server" 
                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                Display="None" ErrorMessage="Enter Fax1" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtEmail"></asp:RequiredFieldValidator>
            </td>

            <td align="right" class="tdtext">Tax No1 :</td>
		    <td class="tdtext">
                <asp:TextBox ID="TxtTaxNo1" runat="server" 
                Width="250px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" 
                Display="None" ErrorMessage="Enter TaxNo1" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtTaxNo1"></asp:RequiredFieldValidator>
            </td>


	    </tr>

        <tr class="whitebg">
	    	<td align="right" class="tdtext" valign="top">Website :</td>
		    <td style="width:10%;" class="tdtext">
                <asp:TextBox ID="TxtWebSite" runat="server" 
                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                Display="None" ErrorMessage="Enter Phone1" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtWebSite"></asp:RequiredFieldValidator>
            </td>

              <td align="right" class="tdtext">Tax No2 :</td>
		      <td class="tdtext">
                <asp:TextBox ID="TxtTaxNo2" runat="server" 
                Width="250px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" 
                Display="None" ErrorMessage="Enter Tax No2" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtTaxNo2"></asp:RequiredFieldValidator>
            </td>


	    </tr>



    	<tr class="whitebg">
	    	<td align="center" class="tdtext" colspan="4">&nbsp;
		    <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="lButton" 
                ValidationGroup="category" Width ="80px" />&nbsp;
		    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	    </tr>
	</table>
    </td>
	</tr>
    </table>
             
</asp:Content>


