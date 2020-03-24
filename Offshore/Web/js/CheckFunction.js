//<!--
//*****************************************************************
//File Name  : CheckEmail.js
//Purpose    : 檢查E-Mail格式程式
//Ref Object : 無
//Methods    : 
//Remark     : 
//Version    : 2002/11/18 by Folse Huang
//*****************************************************************
//-->
function do_ChkMail(sMail) {
   var iPos1 = sMail.indexOf("@");
   var iPos2 = sMail.indexOf(".",iPos1);
   
   if (iPos1 > 0 && iPos2 > 0 && iPos2 < sMail.length - 1) {
      return true;
   }
   else {
      return false;
   }
}

//Purpose    : 刪除數值欄位的Comma
function DelComma(obj)
{
    var txtObj = obj;    
    txtObj.innerText = txtObj.value.replace(/,/g,"");   
}


//數值欄位檢核
//參數1:要檢核的物件ID
//參數2: 該數值的整數長度
//參數3:是否要保留小數位，Y或N
function NanField(strID, maxLength, Isdecimal)
{
    thisObj = document.getElementById(strID);
    valueLength = thisObj.value.length;
    thisObj.value = thisObj.value.replace(/ /g,"").replace(/,/g,"");
    
    //Step1 : 判斷是否為數值,SHOW警告視窗
    if(valueLength > 0)
    {
        if(isNaN(thisObj.value) > 0)
        {
            alert("請輸入數值資料!");
            thisObj.value = "";
            thisObj.select();
            return false;
        }
     }
    else  
    {   return false;  }
    
    //Step2 : 將負數轉為正數
    if(thisObj.value<0)
        thisObj.value=-(thisObj.value); 
    
    //Step3：3位數1個Comma    
    sValuelen =""+Math.floor(thisObj.value);
    i = sValuelen.length;
    var mag = new Array();
    if(i > parseInt(maxLength)) 
    {    
        alert("輸入數值超出限制,請確認");  
        thisObj.select();
        return false; 
    }
    while(i > 0) 
    {
        word = sValuelen.substring(i,i-3); // 每隔3位元截取一組數字
        i-= 3;
        mag.unshift(word); 
    }
    
    //Step4：小數的判斷
    if(Isdecimal == "Y")
    {
        var digit = thisObj.value.indexOf("."); // 取得小數點的位置
        if(digit != "-1")
        {
            var digitInt = thisObj.value.substr(digit,3); // 取得小數中的整數部分-小數三位
            thisObj.value = mag + digitInt;
        }
        else thisObj.value = mag;
    }
    else thisObj.value = mag;
}

//數值欄位檢核
//參數1:要檢核的物件ID
//參數2: 該數值的整數長度
//參數3:是否要無條件進位至整數位，Y或N
function NanField1(strID, maxLength, IsCeil)
{
    thisObj = document.getElementById(strID);
    valueLength = thisObj.value.length;
    thisObj.value = thisObj.value.replace(/ /g,"").replace(/,/g,"");
    
    //Step1 : 判斷是否為數值,SHOW警告視窗
    if(valueLength > 0)
    {
        if(isNaN(thisObj.value) > 0)
        {
            alert("請輸入數值資料!");
            thisObj.value = "";
            thisObj.select();
            return false;
        }
     }
    else  
    {   return false;  }
    
    //Step2 : 將負數轉為正數
    if(thisObj.value<0)
        thisObj.value=-(thisObj.value); 
    
    //Step3：3位數1個Comma    
    sValuelen =""+Math.floor(thisObj.value);
    i = sValuelen.length;
    var mag = new Array();
    if(i > parseInt(maxLength)) 
    {    
        alert("輸入數值超出限制,請確認");  
        thisObj.select();
        return false; 
    }
    while(i > 0) 
    {
        word = sValuelen.substring(i,i-3); // 每隔3位元截取一組數字
        i-= 3;
        mag.unshift(word); 
    }
    
    //Step4：無條件進位至整數位
    if(IsCeil == "Y")
    {
         thisObj.value=Math.ceil(thisObj.value);
    }
    else thisObj.value = mag;
}
//<!--
//*****************************************************************
//File Name  : CheckEmail.js
//Purpose    : 檢查日期格式程式
//Ref Object : 無
//Methods    : 
//Remark     : 
//Version    : 2007/11/19
//*****************************************************************
//-->
function CheckDateReg(objdate)   
{   
    var strPattern=/^\d{1,4}\/\d{1,2}\/\d{1,2}$/;   
    var reg=new RegExp(strPattern);   
    if (!reg.test(objdate.value))   
    {   
        objdate.focus();   
        return false;   
    }   
    var arr=new Array();   
    arr=objdate.value.split('/');   
    if (arr.length!=3){return false;}   
   
    var daynum;   
            
    if (parseFloat(arr[1])==2)   
    {   
        if ((arr[0]%4==0 && arr[0]%100!=0) || (arr[0]%400==0))   
        {   
            daynum=29;   
        }   
        else   
        {   
            daynum=28;   
        }   
    }   
    else   
    {   
         switch (parseFloat(arr[1]))   
        {   
             case 1:   
             case 3:   
             case 5:   
             case 7:   
             case 8:   
             case 10:   
             case 12:   
                 daynum=31;   
                 break;   
             case 4:   
             case 6:   
             case 9:   
             case 11:   
                 daynum=30;   
                 break;   
             default:   
                 daynum=0;   
         }   
     }   

     if ((parseFloat(arr[2])==0)||(parseFloat(arr[2])>daynum)||(parseFloat(arr[1])>12))   
     {   
         objdate.focus();   
         return false;   
     }   
     else   
     {   
         return true;   
     }   

 }   

//<!--
//*****************************************************************
//File Name  : CheckEmail.js
//Purpose    : 比較輸入日期是否大於今日
//Ref Object : 無
//Methods    : 
//Remark     : 
//Version    : 2007/11/19
//*****************************************************************
//-->
function CheckDateOutNow(objdate)
{
    var dtToday=new Date();
    var strvalue = document.forms["frm"].txtdate.value;
    var mydate = new Date(strvalue);

    if (Date.parse(objdate.value) < dtToday)
    {
        return false;
    }
    else 
    {
        return true;
    }
}


//<!--
//*****************************************************************
//功能: 跳出日期選單
//參數: ID  回寫的控制項名稱
//Version    : 2008/03/11
//*****************************************************************
//-->
function ShowCalendar(id)
{
    retValue =  showModalDialog("../../DateTimePicker.aspx?strDate="+document.getElementById(id).value,window, "dialogWidth:350px;dialogHeight:300px;scroll:no;status:no;help:no");
    if(retValue != "" && retValue != null) 
    {
        if(retValue=="Clear")
        {document.getElementById(id).value="";}
        else
        {
            document.getElementById(id).value = retValue;
            return false;
        }
    }
    return false;
}


//<!--Add By YuYu 2008/05/02 將return true or false 改成傳參數
//*****************************************************************
//功能: 跳出日期選單
//參數: ID  回寫的控制項名稱
//Version    : 2008/03/11
//*****************************************************************
//-->
function ShowCalendarCtrlReturnValue(id,ctrl)
{
    retValue =  showModalDialog("../../DateTimePicker.aspx?strDate="+document.getElementById(id).value,window, "dialogWidth:350px;dialogHeight:300px;scroll:no;status:no;help:no");
    if(retValue != "" && retValue != null) 
    {
        if(retValue=="Clear")
        {document.getElementById(id).value="";}
        else
        {
            document.getElementById(id).value = retValue;  
        }
    }
    
    return ctrl;
}

//<!--
//*****************************************************************
//功能: 常用片語
//參數: strType ：1.退回補件 2.審核意見   3：延展原因   4：停業原因
//Version    : 2008/03/11
//*****************************************************************
//-->
function Phrase(strType) 
{
    var tmp="";
    if (strType == "1")//退回補件
    {
        tmp =  showModalDialog("../Flow/Common/Phrase/PhraseForm.aspx?strType=" + strType + "&NOTIFY_USER="+ document.getElementById("hdFormId").value);
        if(tmp != "" && tmp != null) 
        {
            if ( document.getElementById("txtAdminCommentReturn").value == "")
                document.getElementById("txtAdminCommentReturn").value +=  tmp ;
            else
                document.getElementById("txtAdminCommentReturn").value +=  "\r\n"+tmp ;
        }
    }
    else if (strType == "2")//審核意見
    {
        tmp =  showModalDialog("../Flow/Common/Phrase/PhraseForm.aspx?strType=" + strType + "&NOTIFY_USER="+ document.getElementById("hdFormId").value);
        if(tmp != "" && tmp != null) 
        {
            if ( document.getElementById("txtAdminComment").value == "")
                document.getElementById("txtAdminComment").value +=  tmp ;
            else
                document.getElementById("txtAdminComment").value +=  "\r\n"+tmp ;
        }
    }
     else if (strType == "3")//延展原因
        {
            tmp =  showModalDialog("../Flow/Common/Phrase/PhraseForm.aspx?strType=" + strType +"&NOTIFY_USER="+ document.getElementById("hdFormId").value);
            if(tmp != "" && tmp != null) 
            {
                if ( document.getElementById("txtDeferReason").value == "")
                    document.getElementById("txtDeferReason").value +=  tmp ;
                else
                    document.getElementById("txtDeferReason").value +=  "\r\n"+tmp ;
            }
        }
          else if (strType == "4")//停業原因
        {
            tmp =  showModalDialog("../Flow/Common/Phrase/PhraseForm.aspx?strType=" + strType +"&NOTIFY_USER="+ document.getElementById("hdFormId").value);
            if(tmp != "" && tmp != null) 
            {
                if ( document.getElementById("txtCloseReason").value == "")
                    document.getElementById("txtCloseReason").value +=  tmp ;
                else
                    document.getElementById("txtCloseReason").value +=  "\r\n"+tmp ;
            }
        }
    return false;
}

//<!--
//*****************************************************************
//功能: 跳出回傳的訊息 
//參數:  wReload 是否要重新載入， MsgType 判斷是OpenForm或showDialog方式
//Version    : 2008/03/11
//*****************************************************************
//-->
function DailogMsg(wReload,MsgType,MsgID,MsgNote,MsgNote2,wClose)
{  
    if(MsgType!="WindowOpen")
    {
        showModalDialog("../../Msg.aspx?MsgID=" + MsgID + "&MsgNote=" +MsgNote+ "&MsgNote2=" + MsgNote2 ,window,"edge:sunken;resizable:yes;dialogWidth:480px;dialogHeight:450px;"); 
    }
    else
    {
        window.open("../../Msg.aspx?MsgID=" + MsgID + "&MsgNote=" +MsgNote+ "&MsgNote2=" + MsgNote2); 
    }

    if(wClose == "true" || wClose== "True")   self.close();

    if(wReload == "true" || wReload== "True")
    {  
        try{window.opener.parent.location.reload(true); }
        catch(e){}    
    }
}

//<!--
//*****************************************************************
//功能: 檢查退回補件有無值用
//參數:  Mode 0:表示啟動匣或草稿匣  1.為收件匣
//Version    : 2008/03/11
//*****************************************************************
//-->
function ReturnInfo(strType)
{
    if (strType == "0")
    {
        return confirm('確認是否完成送出');
    }
    else if  (strType == "1")
    {
        if (document.getElementById ("txtAdminCommentReturn").value != "")
        {
            return confirm("「退回補件原因」有輸入資料，是否確定「完成‧簽核」?")
        }
        else 
        {
            return confirm('確認是否完成送出');
        }
    }
}


