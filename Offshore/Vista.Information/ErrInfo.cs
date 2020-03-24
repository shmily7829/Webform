using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace Vista.Information
{
    public class ErrInfo
    {
        public ErrInfo()
        {
            this.ErrFlag = true;
            this.ErrMethodName = "";
            this.ErrMsg = "";
        }

        //錯誤檢查
        public bool ErrFlag;

        //錯誤Method
        public string ErrMethodName;

        //錯誤訊息
        public string ErrMsg;


    }
}
