//<!--
//*****************************************************************
//File Name  : CommonFunction.js
//Purpose    : 對 girdview 的 checkBox list 進行全選的動作
//Ref Object : 無
//Methods    : 
//Remark     :
//Version    : a. 2012/05/30 By Jimull
//             b. 2012/06/27 By Jimull 調整 全選的變數控制，排除換頁check Bug

//*****************************************************************
//-->

//選則全部
function SelectAll(ContainerName, sender, checkList) {

    var inputs = document.getElementsByTagName("input");

    for (var i in inputs) {
        if (inputs[i].type == "checkbox") {
            if (inputs[i].id.indexOf(ContainerName) > -1 && inputs[i].id.indexOf(checkList) > -1) {
                $get(inputs[i].id).checked = sender.checked;
            }
        }
    }
}

//<!--
//*****************************************************************
//File Name  : CommonFunction.js
//Purpose    : 判斷 gridview 的 checkBox list 是否有選取資料
//Ref Object : 無s
//Methods    : 
//Remark     : 
//Version    : 2012/05/30 By Jimull
//*****************************************************************
//-->
//判斷是否有勾選任何一個?
function GetQueueCheckToAction(ContainerName, action, checkList) {
    var varAction = action;

    var blnIsAtLeastOneChecked = false;

    var inputs = document.getElementsByTagName("input");
    for (var i in inputs) {
        if (inputs[i].type == "checkbox" && inputs[i].id.indexOf(ContainerName) > -1 && inputs[i].id.indexOf(checkList) > -1 && $get(inputs[i].id).checked) {
            blnIsAtLeastOneChecked = true;
            break;
        }
    }

    if (!blnIsAtLeastOneChecked) {
        alert('請勾選要' + varAction + '的資料！');
        return false;
    }

    if (!window.confirm('確定要' + varAction + '所勾選的資料？')) {
        return false;
    }
}
        

//<!--
//*****************************************************************
//File Name  : CommonFunction.js
//Purpose    : UserControl Select
//Ref Object : 無s
//Methods    : 
//Remark     : 
//Version    : 2012/11/07 By Jimull
//*****************************************************************
//-->
//判斷是否有勾選任何一個?
   function getSelected(a,b,c,d) {
      
            var v = document.getElementById(a);
            var select = v.selectedIndex;
            var tb =  document.getElementById(b);
            var hf =  document.getElementById(c);
            hf.value = v.options[select].value.replace("&nbsp;"," ");
            tb.value = v.options[select].text.replace("&nbsp;"," ");
           
            if(d!="True")
            {
               tb.focus();
               tb.select();
            }
            return false;
        }



//<!--
//*****************************************************************
//File Name  : CommonFunction.js
//Purpose    : UserControl Select
//Ref Object : 無s
//Methods    : 
//Remark     : 
//Version    : 2012/11/07 By Jimull
//*****

   function calendarShown(sender, args) { sender._popupBehavior._element.style.zIndex = 1000005; }

   function GetComma(NumValue) {
       NumValue = NumValue.replace(/[,]+/g, "");
       if (NumValue.length > 10) {
           NumValue = NumValue.substring(0, 10);
       }
       var re = /\d{1,3}(?=(\d{3})+$)/g;
       var n1 = NumValue.replace(/^(\d+)((\.\d+)?)$/, function (s, s1, s2) { return s1.replace(re, "$&,") + s2; });
       return n1;
   }


//2017-05-10 Zach:Textbox增加千分位符號
   function comdify(thisobj) {
       thisobj.value = thisobj.value.replace(/[,]+/g, "");
       if (thisobj.value.length > 10) {
           thisobj.value = thisobj.value.substring(0, 10);
       }
       var re = /\d{1,3}(?=(\d{3})+$)/g;
       var n1 = thisobj.value.replace(/^(\d+)((\.\d+)?)$/, function (s, s1, s2) { return s1.replace(re, "$&,") + s2; });
       thisobj.value = n1;
   }

//2017-05-10 Zach:數字欄位驗證
//1.值 2.總位數 3.小數佔幾位 4.該值為大值不超過xx 5.該值為大值不低於xx 6.Y/N 是否轉換成千分位(每三位加逗號)
//ex:onblur="this.value=this.value.replace(/^\s+|\s+$/g,''); blur_NumericBox(this, 4, 2, 99.99, 0,'N') --整數2位 小數2位 該值不超過99.99 不低於0 不轉換為千分位
   function blur_NumericBox(box, precision, scale, maxValue, minValue, CommaFlag) {
       if (box.value == '') return false;

       var boxValue = box.value.replace(/[,]+/g, "");

       if (isNaN(boxValue)) {
           alert("此欄位只能輸入數字");
           box.value = '';
           box.focus();
           return false;
       }

       var n = parseFloat(boxValue);
       box.value = n;

       if (n > maxValue) {
           alert("此欄位最大值為 " + maxValue + ".");
           box.value = maxValue;
           box.focus();
           return false;
       }
       if (n < minValue) {
           alert("此欄位最小值為 " + minValue + ".");
           box.value = minValue;
           box.focus();
           return false;
       }

       var a = box.value.split('.');
       precision -= scale;

       var n0 = a[0].length;
       if (a[0].substring(0, 1) == '-') n0--;

       if (n0 > precision && a[0] != '0') {
           alert("整數最多為" + precision + "位數,小數最多為 + " + scale + "位數");
           box.value = "";
           box.focus();
           return false;
       }

       if (a.length == 2) {

           if (scale == 0) {
               alert("此欄位無法輸入小數點.");
               box.value = "";
               box.focus();
               return false;
           }

           var n1 = a[1].length;

           if (n1 > scale) {
               alert("請輸入" + precision + "位整數 + " + scale + "位小數");
               box.value = "";
               box.focus();
               return false;
           }
       }

       if (CommaFlag == "Y") {
           comdify(box);
       }       

       return true;
   }   