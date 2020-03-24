using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Vista.CTI.Business
{
    public class S_ParameterBiz
    {
        public S_ParameterBiz()
        { }



        /// <summary>
        /// 取得問題單型別序號參數
        /// </summary>
        /// <returns></returns>
        public DataTable GetParaFormID()
        {
            Vista.CTI.DataAccess.S_ParameterDB myDB = new Vista.CTI.DataAccess.S_ParameterDB();
            DataTable dt = myDB.GetParaFormID();

            return dt;
        }
    }

}
