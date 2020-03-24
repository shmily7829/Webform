using System;
using System.Data;
using System.Data.SqlClient;

namespace Vista.Information
{
    public class SystemCodeInfo
    {
        public String Type { get; set; }

        public String Value { get; set; }

        public String Text { get; set; }

        public Int32? DisplayOrder { get; set; }
        
        public String Value2 { get; set; }

        public String REMARKS { get; set; }

        public String Maker { get; set; }

        public DateTime? Maker_Time { get; set; }

        public String Checker { get; set; }

        public DateTime? Checker_Time { get; set; }

        public SystemCodeInfo()
        {
            Type = "";
            Value = "";
            Text = "";
            DisplayOrder = null;
            Value2 = "";
            REMARKS = "";
            Maker = "";
            Maker_Time = new DateTime(); //或是初始化為null
            Checker = "";
            Checker_Time = new DateTime();
        }

        public SystemCodeInfo(DataRow dr)
        {
            Type = Convert.ToString(dr["Type"]);

            Value = Convert.ToString(dr["Value"]);

            Text = Convert.ToString(dr["Text"]);

            if (dr["DisplayOrder"] == DBNull.Value)
                DisplayOrder = null;
            else
                DisplayOrder = Convert.ToInt32(dr["DisplayOrder"]);

            if (dr["Value2"] == DBNull.Value)
                Value2 = null;
            else
                Value2 = Convert.ToString(dr["Value2"]);

            if (dr["REMARKS"] == DBNull.Value)
                REMARKS = null;
            else
                REMARKS = Convert.ToString(dr["REMARKS"]);

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