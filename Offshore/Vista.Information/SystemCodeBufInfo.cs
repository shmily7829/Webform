using System;
using System.Data;
using System.Data.SqlClient;

namespace Vista.Information
{
    public class SystemCodeBufInfo
    {

        /// <summary>
        /// Constructors
        /// </summary>
        public SystemCodeBufInfo()
        {
            Type = "";
            Value = "";
            Text = "";
            DisplayOrder = null;
            Value2 = null;
            REMARKS = "";
            StepID = "";
            MODItem = null;
            Maker = "";
            Maker_Time = DateTime.Now;
        }


        /// <summary>
        /// Constructors
        /// </summary>
        public SystemCodeBufInfo(SqlDataReader dr)
        {
            Type = dr.GetString(dr.GetOrdinal("Type"));

            Value = dr.GetString(dr.GetOrdinal("Value"));

            Text = dr.GetString(dr.GetOrdinal("Text"));

            if (dr["DisplayOrder"] == DBNull.Value)
                DisplayOrder = null;
            else
                DisplayOrder = dr.GetInt32(dr.GetOrdinal("DisplayOrder"));

            if (dr["Value2"] == DBNull.Value)
                Value2 = null;
            else
                Value2 = dr.GetString(dr.GetOrdinal("Value2"));

            REMARKS = dr.GetString(dr.GetOrdinal("REMARKS"));

            StepID = dr.GetString(dr.GetOrdinal("StepID"));

            if (dr["MODItem"] == DBNull.Value)
                MODItem = null;
            else
                MODItem = dr.GetString(dr.GetOrdinal("MODItem"));

            Maker = dr.GetString(dr.GetOrdinal("Maker"));

            Maker_Time = dr.GetDateTime(dr.GetOrdinal("Maker_Time"));

        }


        /// <summary>
        /// Constructors
        /// </summary>
        public SystemCodeBufInfo(DataRow dr)
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

            REMARKS = Convert.ToString(dr["REMARKS"]);

            StepID = Convert.ToString(dr["StepID"]);

            if (dr["MODItem"] == DBNull.Value)
                MODItem = null;
            else
                MODItem = Convert.ToString(dr["MODItem"]);

            Maker = Convert.ToString(dr["Maker"]);

            Maker_Time = Convert.ToDateTime(dr["Maker_Time"]);

        }


        /// <summary>
        /// Constructors
        /// </summary>
        public SystemCodeBufInfo(SystemCodeInfo DataInfo)
        {
            Type = DataInfo.Type;
            Value = DataInfo.Value;
            Text = DataInfo.Text;
            DisplayOrder = DataInfo.DisplayOrder;
            Value2 = DataInfo.Value2;
            REMARKS = DataInfo.REMARKS;
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
        public String StepID { get; set; }

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
        public DateTime Maker_Time { get; set; }

        #endregion
    }
}