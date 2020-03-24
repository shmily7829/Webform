using System;
using System.Data;
using System.Data.SqlClient;

namespace Vista.Information
{
    public class RES_BANKInfo
    {
        //建立資料欄位
        public String BANK_CODE { get; set; }
        public String BANK_NAME { get; set; }
        public String TEL_NO { get; set; }
        public String MAIL_1 { get; set; }
        public String MAIL_2 { get; set; }
        public String MAIL_3 { get; set; }
        public String Maker { get; set; }
        public DateTime? Maker_Time { get; set; }
        public String Checker { get; set; }
        public DateTime? Checker_Time { get; set; }

        //初始化欄位值
        public RES_BANKInfo()
        {
            BANK_CODE = "";
            BANK_NAME = "";
            MAIL_1 = "";
            MAIL_2 = "";
            MAIL_3 = "";
            Maker = "";
            Maker_Time = new DateTime(); //或是初始化為null
            Checker = "";
            Checker_Time = new DateTime();
        }

        public RES_BANKInfo(DataRow dr)
        {
            //Type = Convert.ToString(dr["Type"]);

            BANK_CODE = Convert.ToString(dr["BANK_CODE"]);

            if (dr["BANK_NAME"] == DBNull.Value)
                BANK_NAME = null;
            else
            BANK_NAME = Convert.ToString(dr["BANK_NAME"]);

            if (dr["TEL_NO"] == DBNull.Value)
                MAIL_1 = null;
            else
                MAIL_1 = Convert.ToString(dr["TEL_NO"]);

            if (dr["MAIL_1"] == DBNull.Value)
                MAIL_1 = null;
            else
                MAIL_1 = Convert.ToString(dr["MAIL_1"]);

            if (dr["MAIL_2"] == DBNull.Value)
                MAIL_2 = null;
            else
                MAIL_2 = Convert.ToString(dr["MAIL_2"]);

            if (dr["MAIL_3"] == DBNull.Value)
                MAIL_3 = null;
            else
                MAIL_3 = Convert.ToString(dr["MAIL_3"]);

            if (dr["Maker"] == DBNull.Value)
                Maker = null;
            else
                Maker = Convert.ToString(dr["Maker"]);

            if (dr["Maker_Time"] == DBNull.Value)
                Maker_Time = null;
            else
                Maker_Time = Convert.ToDateTime(dr["Maker_Time"]);

            if (dr["Checker"] == DBNull.Value)
                Checker = null;
            else
                Checker = Convert.ToString(dr["Checker"]);

            if (dr["Checker_Time"] == DBNull.Value)
                Checker_Time = null;
            else
                Checker_Time = Convert.ToDateTime(dr["Checker_Time"]);

        }
    }
}
