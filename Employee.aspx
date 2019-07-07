<%@ Page Title="" Language="VB" MasterPageFile="~/Master.master" AutoEventWireup="false" CodeFile="Employee.aspx.vb" Inherits="Employee" %>
<%@ Register Src="VendorListUserControl.ascx" TagName="DetailCon" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="JS/jquery-3.3.1.min.js"></script>
<table align="center" cellpadding="0" cellspacing="0" width="80%">
<tr class="tdcolbg">
    <td valign="top">
        	  
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="trheaderbg">
		    <td align="center">		 Employee Information
		        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		    </td>
	    </tr>
    </table>
        
    <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="whitebg">
	    	<td align="right" style="width:15%;" class="tdtext">Employee Code :</td>
		    <td style="width:15%;" class="tdtext">
                <asp:TextBox ID="TxtEmpCode" runat="server" 
                Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                Display="None" ErrorMessage="Enter Material Code" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtEmpCode"></asp:RequiredFieldValidator>
                <asp:ImageButton ID="imgsrch" style="width: 16px; height: 16px" runat="server" ImageUrl="~/Images/searchs.png" OnClientClick="searchVendors()"  ></asp:ImageButton>
            </td>
	    
		    <td align="right" style="width:20%;" class="tdtext">Employee Name :</td>
		    <td class="tdtext"style="width:30%;" ><asp:TextBox ID="TxtEmpName" runat="server" Width="300px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
                        <uc1:DetailCon ID="ucModal" runat="server"  />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Enter Material Name" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtEmpName" 
                Display="None"></asp:RequiredFieldValidator>
            </td>

            <td align="right" style="width:10%;" class="tdtext">Initial :</td>
		    <td class="tdtext"style="width:10%;" ><asp:TextBox ID="TxtEmpInitial" runat="server" Width="40px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="Enter Material Name" SetFocusOnError="True" 
                ValidationGroup="category" ControlToValidate="TxtEmpInitial" 
                Display="None"></asp:RequiredFieldValidator>
            </td>
	    </tr>
          </table>

        <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="trheaderbg">
		    <td align="center">		 Personal Information
		        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		    </td>
	    </tr>
    </table>

        <table align="center" width="100%" cellpadding="4" cellspacing="1">
            <tr  class="whitebg">
                <td align="left" style="width:85%;" class="tdtext">
                    <table align="left" width="100%" cellpadding="4" cellspacing="1">
                        <tr class="whitebg" style="border-bottom:solid;border-bottom-color:Background">
                              <td align="right" style="width:15%;" class="tdtext">Father's Name :</td> 
                              <td style="width:35%;" class="tdtext">
                                <asp:TextBox ID="TxtFatherName" runat="server" Width="200px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                Display="None" ErrorMessage="Enter Father's Code" SetFocusOnError="True" 
                                ValidationGroup="category"  ControlToValidate="TxtFatherName"></asp:RequiredFieldValidator>
                            </td>

                            <td align="right" style="width:15%;" class="tdtext">DOB :</td> 
                              <td style="width:35%;" class="tdtext">
                                <asp:TextBox ID="TxtDOB" runat="server" Width="100px" CssClass="control"></asp:TextBox>
                                  <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="TxtDOB" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                            </td>
                         </tr>
                        
                        <tr class="whitebg">
		                    <td style="width:15%;vertical-align:top;" class="tdtext" align="right"  > Contact Address :</td>
                            <td style="width:35%;" class="tdtext"> 
                                <asp:TextBox ID="TxtAddress1" runat="server" 
                                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="TxtAddress2" runat="server" 
                                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="TxtAddress3" runat="server" 
                                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="TxtAddress4" runat="server" 
                                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                        </td>


                            <td style="width:15%;vertical-align:top;" class="tdtext" align="right"  > Perm. Address :</td> 
		                    <td style="width:35%;" class="tdtext"> 
                                <asp:TextBox ID="TxtAddress1P" runat="server" 
                                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="TxtAddress2P" runat="server" 
                                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="TxtAddress3P" runat="server" 
                                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="TxtAddress4P" runat="server" 
                                Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                        </td>
                       </tr>
                        <tr class="whitebg" style="border-bottom:solid;border-bottom-color:Background">
                              <td align="right" style="width:15%;" class="tdtext">City :</td> 
                              <td style="width:35%;" class="tdtext">
                                <asp:TextBox ID="TxtCity" runat="server" Width="200px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                            </td>

                            <td align="right" style="width:15%;" class="tdtext">State :</td> 
                              <td style="width:35%;" class="tdtext">
                                <asp:TextBox ID="TxtState" runat="server" Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>

                            </td>
                         </tr>  


                        <tr class="whitebg" style="border-bottom:solid;border-bottom-color:Background">
                              <td align="right" style="width:15%;" class="tdtext">Pincode :</td> 
                              <td style="width:35%;" class="tdtext">
                                <asp:TextBox ID="TxtPinCode" runat="server" Width="100px" CssClass="control" ValidationGroup="category"></asp:TextBox>    
                            </td>

                            <td align="right" style="width:15%;" class="tdtext">Email :</td> 
                              <td style="width:35%;" class="tdtext">
                                <asp:TextBox ID="TxtEmail" runat="server" Width="300px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                            </td>
                         </tr>  
                    </table>
                </td>

                <td align="right" style="width:15%;" class="tdtext">
                    <table width="100%" class="tdtext" style="vertical-align:top; height: 207px;">
                  
                    <tr>
                        <td align ="center">
                            <asp:FileUpload ID="ImgUpload" runat="server" style="display:none" />
                                           
                        <asp:Image ID="ImgEmp" runat="server" Height = "140" Width = "140" />
                        </td>
                    </tr>

                    <tr>
                        <td align="center">
                            <asp:ImageButton ID="BtnLogo" runat="server" ImageUrl="~/Images/uparrow.jpg" />
                            <asp:ImageButton ID="BtnPreview" runat="server"  ImageUrl="~/Images/Undeploy.png" Height="25px" />
                        
                        </td>
                   </tr>

                </table>    
                </td>
            </tr>
        </table>

     <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="trheaderbg">
		    <td align="center">		 Other Information
		        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		    </td>
	    </tr>
    </table>

     <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    
         <tr class="whitebg">
	    	<td align="right" style="width:15%;" class="tdtext">MobileNo :</td>
		    <td style="width:20%;" class="tdtext">
                <asp:TextBox ID="TxtMobileNo" runat="server" 
                Width="150px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" 
                Display="None" ErrorMessage="Enter Mobile No" SetFocusOnError="True" 
                ValidationGroup="category"  ControlToValidate="TxtMobileNo"></asp:RequiredFieldValidator>
            </td>
	    
		    <td align="right" style="width:15%;" class="tdtext">Emergency Contact:</td>
		    <td class="tdtext"style="width:20%;" ><asp:TextBox ID="TxtEmCo" runat="server" Width="150px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            </td>

            <td align="right" style="width:15%;" class="tdtext">No :</td>
		    <td class="tdtext"style="width:20%;" ><asp:TextBox ID="TxtEmNo" runat="server" Width="150px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            </td>
	    </tr>


         <tr class="whitebg">
	    	<td align="right" style="width:15%;" class="tdtext">Aadhar No :</td>
		    <td style="width:20%;" class="tdtext">
                <asp:TextBox ID="TxtAadharNo" runat="server" Width="150px" CssClass="control" 
                    ValidationGroup="category"></asp:TextBox>
            </td>
	    
		    <td align="right" style="width:15%;" class="tdtext">PAN No:</td>
		    <td class="tdtext"style="width:20%;" ><asp:TextBox ID="TxtPanNo" runat="server" Width="150px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            </td>

            <td align="right" style="width:15%;" class="tdtext">Licence No :</td>
		    <td class="tdtext"style="width:20%;" ><asp:TextBox ID="TxtLicenseNo" runat="server" Width="150px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
            </td>
	    </tr>
          </table>

        <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    <tr class="trheaderbg">
		    <td align="center">		 Job Profile
		        <asp:ValidationSummary ID="ValidationSummary4" runat="server" ShowMessageBox="True" ValidationGroup="category" ShowSummary="False" />		
		    </td>
	    </tr>
        </table>

          <table align="center" width="100%" cellpadding="4" cellspacing="1">
	    
         <tr class="whitebg">
	    	<td align="right" style="width:15%;" class="tdtext">Department :</td>
		    <td style="width:20%;" class="tdtext">
                 <asp:DropDownList ID="CboDept" runat="server" Width="150" class="control" AutoPostBack="true">
                 <asp:ListItem Value="0">--Select--</asp:ListItem>
                 </asp:DropDownList>
            </td>
	    
		    <td align="right" style="width:15%;" class="tdtext">Designation :</td>
		    <td class="tdtext"style="width:20%;" >
                <asp:DropDownList ID="CboDesig" runat="server" Width="150" class="control" AutoPostBack="true">
                 <asp:ListItem Value="0">--Select--</asp:ListItem>
                 </asp:DropDownList>
            </td>

            <td align="right" style="width:15%;" class="tdtext">Category :</td>
		    <td class="tdtext"style="width:20%;" >
                <asp:DropDownList ID="CboEmpCat" runat="server" Width="150" class="control" AutoPostBack="true">
                 <asp:ListItem Value="0">--Select--</asp:ListItem>
                 </asp:DropDownList>
            </td>
	    </tr>


         <tr class="whitebg">
	    	<td align="right" style="width:15%;" class="tdtext">Joining Date :</td>
		    <td style="width:20%;" class="tdtext">
                <asp:TextBox ID="TxtJDate" runat="server" Width="150px" CssClass="control" 
                    ValidationGroup="category"></asp:TextBox>
                 <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="TxtJDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
            </td>
	    
		    <td align="right" style="width:15%;" class="tdtext">Confimation Date:</td>
		    <td class="tdtext"style="width:20%;" ><asp:TextBox ID="TxtCDate" runat="server" Width="150px" 
                CssClass="control" ValidationGroup="category"></asp:TextBox>
                 <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="TxtCDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
            </td>

            <td align="right" style="width:15%;vertical-align:top;"" class="tdtext" > 
                <asp:CheckBox  runat="server" ID="chkinactive" text="Inactive" Height="5px" />
                Resign Date:</td>
		    <td class="tdtext"style="width:20%;" >
                  <asp:TextBox ID="TxtRDate" runat="server" Width="150px" CssClass="control" ValidationGroup="category"></asp:TextBox>
                 <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="TxtRDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
            </td>
	    </tr>


             <tr class="whitebg">
	    	<td align="right" style="width:15%;" class="tdtext">Basic Salary :</td>
		    <td style="width:20%;" class="tdtext">
                <asp:TextBox ID="TxtBasicSalary" runat="server" Width="150px" CssClass="control" 
                    ValidationGroup="category"></asp:TextBox>
            </td>
	    
		    <td align="right" style="width:15%;" class="tdtext">Avail Lunch:</td>
		    <td class="tdtext"style="width:20%;" >
                <asp:CheckBox  runat="server" ID="ChkLunch" text="" Height="5px" />
            </td>
            <td align="right" style="width:15%;" class="tdtext" colspan="2">
            </td>
	    </tr>



          </table>



        
        <table align="center" width="100%" cellpadding="4" cellspacing="1">
    	<tr class="whitebg">
	    	<td align="center" colspan="4" class="tdtext">&nbsp;
		    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="lButton" 
                ValidationGroup="category" Width ="80px" />&nbsp;
		    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></td>
	    </tr>
	</table>
    </td>
	</tr>
                
    </table>


        
	
</asp:Content>


