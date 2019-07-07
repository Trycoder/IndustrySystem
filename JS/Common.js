//document.onselectstart=new Function ("return false");

//Opens popup window from Default.aspx.
function openWinDetail(AssetTypeID,TransTypeID,CatID,Pnl)
{
    window.open('AssetTypeDetails.aspx?AssetTypeID=' + AssetTypeID + '&TransTypeID=' + TransTypeID + '&CatID=' + CatID + '&pnl=' + Pnl, 'name', 'left=100,top=100,height=750px,width=1100px,scrollbars,resizable=1');
}
function openassettransactions(AssetTypeID,TransTypeID,Days,Rej)
{
    //window.open('AssetandTransactionwiseview.aspx?AssetTypeID=' + AssetTypeID + '&TransTypeID=' + TransTypeID,'name','left=100,top=100,height=750px,width=1100px,scrollbars');
    window.open('AssetandTransactionwiseview.aspx?AssetTypeID=' + AssetTypeID + '&TransTypeID=' + TransTypeID + '&Days=' + Days + '&Rej=' + Rej, 'name', 'left=100,top=100,height=750px,scrollbars,width=1100px,resizable=1');
}
function openassettransactions1(AssetTypeID,TType)
{
    window.open('asset_count_details.aspx?assettypeid=' + AssetTypeID + '&TType=' + TType, 'name', 'left=100,top=100,height=750px,scrollbars,width=1100px,resizable=1');
}
function openstatuswindow(status, assettypeid, assettypecode) {
    window.open('Status_count_details.aspx?status=' + status + '&assettypeid=' + assettypeid + '&assettypecode=' + assettypecode + '', 'name', 'left=100,top=100,height=750px,scrollbars,width=1100px,resizable=1');
}
//*****************************************************************************************************************
function fnStatusAsset(stat,divid)
{
    var divStatus = 'divStatus' + divid;
    var imgExp = 'imgExp' + divid;
    var imgCol = 'imgCol' + divid;
    if(stat == 1)
    {//Expand
        document.getElementById(divStatus).style.display="block";
        document.getElementById(imgExp).style.display="none";
        document.getElementById(imgCol).style.display="inline-block";
    } 
    else if(stat == 2)
    {//Collapse.
        document.getElementById(divStatus).style.display="none";
        document.getElementById(imgExp).style.display="inline-block";
        document.getElementById(imgCol).style.display="none";
    }
}

function fnConsumable(stat,divid)
{
    var divStatus = 'divConsumable' + divid;
    var imgExp = 'imgExpCons' + divid;
    var imgCol = 'imgColCons' + divid;
    if(stat == 1)
    {//Expand
        document.getElementById(divStatus).style.display="block";
        document.getElementById(imgExp).style.display="none";
        document.getElementById(imgCol).style.display="inline-block";
    } 
    else if(stat == 2)
    {//Collapse.
        document.getElementById(divStatus).style.display="none";
        document.getElementById(imgExp).style.display="inline-block";
        document.getElementById(imgCol).style.display="none";
    }
}

function fnExpWar(stat,divid)
{
    var divStatus = 'divWarrenty' + divid;
    var imgExp = 'imgExpWar' + divid;
    var imgCol = 'imgColWar' + divid;
    if(stat == 1)
    {//Expand
        document.getElementById(divStatus).style.display="block";
        document.getElementById(imgExp).style.display="none";
        document.getElementById(imgCol).style.display="inline-block";
    } 
    else if(stat == 2)
    {//Collapse.
        document.getElementById(divStatus).style.display="none";
        document.getElementById(imgExp).style.display="inline-block";
        document.getElementById(imgCol).style.display="none";
    }
}
//*****************************************************************************************************************//

function getControlID(controlID)
{
    var concat1 = 'ctl00_';
    var concat2 = 'ContentPlaceHolder1_';
    var concat3 = controlID;
    var ChangedControlID = concat1 + concat2 + concat3;
    return ChangedControlID;
}
function getControlName(controlID)
{
    var concat1 = 'ctl00$';
    var concat2 = 'ContentPlaceHolder1$';
    var concat3 = controlID;
    var ChangedControlID = concat1 + concat2 + concat3;
    return ChangedControlID;
}