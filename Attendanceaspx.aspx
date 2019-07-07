<%@ Page Language="VB" MasterPageFile="~/Master.master" EnableEventValidation="true" AutoEventWireup="false" CodeFile="Attendanceaspx.aspx.vb" Inherits="Attendanceaspx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="JS/jquery-3.3.1.min.js"></script>
    <link href="CSS/slidingdoors.css" rel="stylesheet" type="text/css" />
    <link href="Css/ApplyCSS1.css" rel="stylesheet" type="text/css" />
  
    <link href="Css/tabs.css" rel="stylesheet" type="text/css" />
    <link href="Css/ToolTip.css" rel="Stylesheet" type="text/css" />
    <link rel="Stylesheet" href="Css/panel.css" />
      <link href="Css/grid.css" rel="Stylesheet" type="text/css" />
    <script src="JS/jquery-3.3.1.min.js"></script>
       

        <div>Date
                    <asp:TextBox ID="txtattDt" runat="server"></asp:TextBox> 
             <asp:ImageButton ID="imgsrch" runat="server"  style="width: 16px; height: 16px" ImageUrl="~/Images/searchs.png"  />
                             
                    <asp:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtattDt" Format="dd/MM/yyyy"> </asp:CalendarExtender>  
                </div>  

     <asp:GridView ID="GrdDetails" runat="server"  DataKeyNames="empid" AutoGenerateColumns="false" 
              CssClass="mGrid"
            GridLines="None"  
            AlternatingRowStyle-CssClass="alt"
            PagerStyle-CssClass="pgr"  
            AllowPaging="true" AllowSorting="false" EmptyDataText="No Data Found !">
      <Columns>

            <asp:TemplateField HeaderText="Emp Code"  ItemStyle-Width="30">
            <ItemTemplate>
                <asp:Label ID="txtEmpCode" runat="server"  Text='<%# Bind("empcode") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    
        <asp:TemplateField HeaderText="Emp Name" ItemStyle-Width="150">
            <ItemTemplate>
                <asp:Label ID="txtEmpName" runat="server" Text='<%# Bind("empName") %>'/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="In Time" ItemStyle-Width="150">
            <ItemTemplate>
                <asp:TextBox ID="txtInTime" placeholder="hh:mm" runat="server"  Text=' <%# Bind("Time1") %>' />
                 
            <asp:RegularExpressionValidator runat="server" ID="valdtIntime" ForeColor="Red" ValidationExpression="^$|(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$" ControlToValidate="txtInTime" ErrorMessage="Invalid Format"></asp:RegularExpressionValidator>
            </ItemTemplate>
        </asp:TemplateField>
                 <asp:TemplateField HeaderText="LO" ItemStyle-Width="150">
            <ItemTemplate>
                <asp:TextBox ID="txtLo" placeholder="hh:mm" runat="server"  Text='<%# Bind("Time2") %>' />
                 <asp:RegularExpressionValidator runat="server" ID="valdtLo" ForeColor="Red" ValidationExpression="^$|(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$" ControlToValidate="txtLo" ErrorMessage="Invalid Format"></asp:RegularExpressionValidator>
            </ItemTemplate>
        </asp:TemplateField>
                 <asp:TemplateField HeaderText="LI" ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:TextBox ID="txtLi" placeholder="hh:mm" runat="server"  Text='<%# Bind("Time3") %>' />
                 <asp:RegularExpressionValidator runat="server" ID="valdtLI" ForeColor="Red" ValidationExpression="^$|(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$" ControlToValidate="txtLi" ErrorMessage="Invalid Format"></asp:RegularExpressionValidator>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="Out Time" ItemStyle-Width="150">
            <ItemTemplate>
                <asp:TextBox ID="txtoutTime" placeholder="hh:mm" runat="server"  Text='<%# Bind("Time4") %>' />
                 <asp:RegularExpressionValidator runat="server" ID="valdtoutTime" ForeColor="Red" ValidationExpression="^$|(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$" ControlToValidate="txtoutTime" ErrorMessage="Invalid Format"></asp:RegularExpressionValidator>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="Lunch Tag" ItemStyle-Width="150">
            <ItemTemplate>
                  <asp:Label ID="lbllunch" runat="server" style="display:none" Text='<%# Bind("Islunch") %>' />
                <asp:CheckBox ID="chkbxlunch" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="WorkHours" ItemStyle-Width="150">
            <ItemTemplate>
                <asp:Label ID="lblwhrs" runat="server" Text='<%# Bind("whrs") %>' />
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="OverTime" ItemStyle-Width="150">
            <ItemTemplate>
                <asp:Label ID="lblOt" runat="server" Text='<%# Bind("Othrs") %>' />
            </ItemTemplate>
        </asp:TemplateField>
          </Columns>
    </asp:GridView>
    <div>
         <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CssClass="lButton" 
                Width ="80px" />&nbsp;
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="lButton" 
                ValidationGroup="category" Width ="80px" />&nbsp;
        
		    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="lButton" Width ="80px" /></div>
      
    
    
    
      <script type="text/javascript" language="javascript">
           function CalculateWrkhrs(tb) {
              
               alert(tb.value);
               var row = tb.parentNode.parentNode;
               var T1 = row.cells[2].childNodes[1].value;
               //alert("T1 :"+T1)
               var T2 = row.cells[3].childNodes[1].value;
               //alert("T2 :" + T2)
               var T3 = row.cells[4].childNodes[1].value;
               //alert("T3 :" + T3)
               var T4 = row.cells[5].childNodes[1].value;
               //alert("T4 :" + T4)
               //for (j = 0; j < col1.childNodes.length; j++) {
               //    if (col1.childNodes[j].type == "text") {
                       
               //        if (col1.childNodes[j].value != ""&&col1.childNodes[j].value !="undefined") {
               //          alert(col1.childNodes[j].value)
               //        }
               //    }
               //}
              
              
                 
                   if(Validatetime(T1)&&Validatetime(T2)&&Validatetime(T3)&&Validatetime(T4))
                   {
                       if (ValidateForEmpty(T1) || ValidateForEmpty(T4))
                       {
                           alert("1 or 4 empty");
                           row.cells[6].childNodes[1].value = "NA";
                           row.cells[6].innerText = "NA";
                           
                       }
                       else if (ValidateForEmpty(T2) && ValidateForEmpty(T3))
                       {
                           alert("2 & 3 empty");
                           var wrkhrsinmillsec = FindDiffinmillsec(T1, T4);
                           var wrkhrsinhhmm = GetTimeinhhmm(wrkhrsinmillsec);
                         
                           alert(tr.find($("[id*=txtoutTime]")).val())
                           
                           row.cells[6].childNodes[1].value = wrkhrsinhhmm;
                           row.cells[6].textContent = wrkhrsinhhmm;
                       }
                       else if (ValidateForEmpty(T2) && ValidateForEmpty(T3) == false)
                       {
                           alert("2 || 3 empty");
                           row.cells[6].childNodes[1].value = "NA";
                           row.cells[6].textContent = "NA";
                        
                       }
                       else if (ValidateForEmpty(T3) && ValidateForEmpty(T2) == false)
                       {
                           alert("2 || 3 empty");
                           row.cells[6].childNodes[1].value = "NA";
                           row.cells[6].textContent = "NA";
                         
                       }
                       else
                       {
                           
                           alert("validated");
                           var frstdiff = FindDiffinmillsec(T1, T2);
                           alert(frstdiff);
                           var seconddiff = FindDiffinmillsec(T3, T4);
                           alert(seconddiff);
                           var wrkhrsinmillsec = frstdiff + seconddiff;
                           alert("Tot" + wrkhrsinmillsec)
                           var wrkhrsinhhmm = GetTimeinhhmm(wrkhrsinmillsec);
                           alert(wrkhrsinhhmm)
                           row.cells[6].childNodes[1].value = wrkhrsinhhmm;
                           row.cells[6].textContent = wrkhrsinhhmm;
                       }
                     
                   }
              
               
              
			    
           }

           function FindDiffinmillsec(time1, time2) {
               var t1 = time1;
               var t2 = time2;
               if(t1)
               var t1 = t1.split(':');
               var t2 = t2.split(':');
               var d1 = new Date(0, 0, 0, t1[0], t1[1]);
               var d2 = new Date(0, 0, 0, t2[0], t2[1]);
               var diff = d2 - d1;
               return diff;
           }

           function GetTimeinhhmm(tm)
           {            
               var dm = new Date(0, 0, 0, 0, 0, 0, tm);
               var hrsdiff = dm.getHours();
           
               var minsdiff = dm.getMinutes();
            
               var res = formatting(hrsdiff.toString()) + ":" + formatting(minsdiff.toString());
               return res;
           }
           function Validatetime(timestr)
           {
               var patt = new RegExp("^$|(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
               var res = patt.test(timestr);
               return res;
           }
           function ValidateForEmpty(timestr) {
               var patt = new RegExp("^$");
               var res = patt.test(timestr);
               return res;
           }
           function formatting(target) {
               return target < 10 ? '0' + target : target;
           }
           </script>

</asp:Content>
