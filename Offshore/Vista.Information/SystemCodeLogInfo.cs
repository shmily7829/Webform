using System;
using System.Data;
using System.Data.SqlClient;

namespace Vista.Information
{
    public class SystemCodeLogInfo
    {

        /// <summary>
        /// Constructors
        /// </summary>
        public SystemCodeLogInfo()
        {
            Type = null;
            Value = null;
            Text = null;
            DisplayOrder = null;
            Value2 = null;
            REMARKS = null;
            MODItem = null;
            Maker = null;
            Maker_Time = null;
            Checker = null;
            Checker_Time = null;
        }


        /// <summary>
        /// Constructors
        /// </summary>
        public SystemCodeLogInfo(SqlDataReader dr)
        {
            if (dr["Type"] == DBNull.Value)
                Type = null;
            else
                Type = dr.GetString(dr.GetOrdinal("Type"));

            if (dr["Value"] == DBNull.Value)
                Value = null;
            else
                Value = dr.GetString(dr.GetOrdinal("Value"));

            if (dr["Text"] == DBNull.Value)
                Text = null;
            else
                Text = dr.GetString(dr.GetOrdinal("Text"));

            if (dr["DisplayOrder"] == DBNull.Value)
                DisplayOrder = null;
            else
                DisplayOrder = dr.GetInt32(dr.GetOrdinal("DisplayOrder"));

            if (dr["Value2"] == DBNull.Value)
                Value2 = null;
            else
                Value2 = dr.GetString(dr.GetOrdinal("Value2"));

            if (dr["REMARKS"] == DBNull.Value)
                REMARKS = null;
            else
                REMARKS = dr.GetString(dr.GetOrdinal("REMARKS"));

            if (dr["MODItem"] == DBNull.Value)
                MODItem = null;
            else
                MODItem = dr.GetString(dr.GetOrdinal("MODItem"));

            if (dr["Maker"] == DBNull.Value)
                Maker = null;
            else
                Maker = dr.GetString(dr.GetOrdinal("Maker"));

            if (dr["Maker_Time"] == DBNull.Value)
                Maker_Time = null;
            else
                Maker_Time = dr.GetDateTime(dr.GetOrdinal("Maker_Time"));

            if (dr["Checker"] == DBNull.Value)
                Checker = null;
            else
                Checker = dr.GetString(dr.GetOrdinal("Checker"));

            if (dr["Checker_Time"] == DBNull.Value)
                Checker_Time = null;
            else
                Checker_Time = dr.GetDateTime(dr.GetOrdinal("Checker_Time"));

        }


        /// <summary>
        /// Constructors
        /// </summary>
        public SystemCodeLogInfo(DataRow dr)
        {
            if (dr["Type"] == DBNull.Value)
                Type = null;
            else
                Type = Convert.ToString(dr["Type"]);

            if (dr["Value"] == DBNull.Value)
                Value = null;
            else
                Value = Convert.ToString(dr["Value"]);

            if (dr["Text"] == DBNull.Value)
                Text = null;
            else
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

            if (dr["MODItem"] == DBNull.Value)
                MODItem = null;
            else
                MODItem = Convert.ToString(dr["MODItem"]);

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


        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? DisplayOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Value2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String REMARKS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String MODItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Maker { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Maker_Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Checker { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Checker_Time { get; set; }

        #endregion
    }
}