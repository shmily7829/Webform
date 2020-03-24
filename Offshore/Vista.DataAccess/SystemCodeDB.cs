using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Vista.Information;
using System.Data.SqlClient;

namespace Vista.DataAccess
{
    public class SystemCodeDB : baseDB
    {
        public SystemCodeDB(string iUserID, string iFunctionID)
        {
            base.UserID = iUserID;
            base.FunctionID = iFunctionID;
            DBInstanceName = "CONNSEC";
        }

        public enum TableType { Buffer, Official }

        public DataTable getSystemCode_distinct()
        {
            //設定一個空的dataTable準備接資料
            DataTable dtTemp = new DataTable();
            //選取db路徑
            Database db = GetDatabase();

            //接資料字串的方法指定到stCmd
            StringBuilder stCmd = new StringBuilder();
            stCmd.AppendLine("select distinct type from SystemCode");
            stCmd.AppendLine("where (1=1)");

            DbCommand dbCommand = db.GetSqlStringCommand(stCmd.ToString());
            //參數指定 需要顯示的下拉選單內容
            dtTemp = ExecuteDataSet(db, dbCommand, false).Tables[0];
            return dtTemp;
        }

        public DataTable getSystemCode_count(string Type, string Value, string Text)
        {
            //設定一個空的dataTable準備接資料
            DataTable dtTemp = new DataTable();
            //選取db路徑
            Database db = GetDatabase();
            //接資料字串的方法指定到stCmd
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.Append(" select count(*) as ROW_COUNT from systemcode where 1 = 1 ");
            if (!string.IsNullOrEmpty(Type)) sbCmd.AppendLine(" and Type = @Type ");
            if (!string.IsNullOrEmpty(Value)) sbCmd.AppendLine(" and Value = @Value ");
            if (!string.IsNullOrEmpty(Text)) sbCmd.AppendLine(" and Text like '%'+ @Text +'%' ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            //參數指定
            db.AddInParameter(dbCommand, "@Type", DbType.String, Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, Value);
            db.AddInParameter(dbCommand, "@Text", DbType.String, Text);

            dtTemp = ExecuteDataSet(db, dbCommand, false).Tables[0];

            //取出第一列第一欄的資料 DataTable.Rows[rowindex][columnindex]

            // DataRow dr = dtTemp.Rows[0];
            // object obj = dr[0];
            // int result = obj;
            return dtTemp;
        }

        public DataTable getSystemCode_select(string Type, string Value, string Text, int OffsecCount, int FetchCount)
        {
            //設定一個空的dataTable準備接資料
            DataTable dtTemp = new DataTable();
            //選取db路徑
            Database db = GetDatabase();
            //接資料字串的方法指定到stCmd
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.Append(" select * from systemcode where 1 = 1 ");
            if (!string.IsNullOrEmpty(Type)) sbCmd.AppendLine(" and Type = @Type ");
            if (!string.IsNullOrEmpty(Value)) sbCmd.AppendLine(" and Value = @Value ");
            if (!string.IsNullOrEmpty(Text)) sbCmd.AppendLine(" and Text like '%'+ @Text +'%' ");
            sbCmd.AppendLine(" order by Type, DisplayOrder ");
            sbCmd.AppendLine(" OFFSET @OFFSET ROWS ");
            sbCmd.AppendLine(" FETCH NEXT @FETCH ROWS ONLY ");


            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            //參數指定
            db.AddInParameter(dbCommand, "@Type", DbType.String, Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, Value);
            db.AddInParameter(dbCommand, "@Text", DbType.String, Text);
            db.AddInParameter(dbCommand, "@OFFSET", DbType.Int32, OffsecCount);
            db.AddInParameter(dbCommand, "@FETCH", DbType.Int32, FetchCount);

            dtTemp = ExecuteDataSet(db, dbCommand).Tables[0];
            return dtTemp;
        }

        //依Key值確認是否已存在於Table中
        public bool chkSystemCode(string Type, string Value)
        {
            bool chkResult = false;
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine("SELECT COUNT(*) ");
            sbCmd.AppendLine("  FROM SystemCode with (nolock) ");
            sbCmd.AppendLine(" WHERE 1 = 1 ");
            sbCmd.AppendLine("   AND Type = @Type ");
            sbCmd.AppendLine("   AND Value = @Value ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, Value);
            #endregion

            object o = ExecuteScalar(db, dbCommand);

            if (o == null)
                chkResult = false;
            else
            {
                if (o.ToString() == "0")
                    chkResult = false;
                else
                    chkResult = true;
            }
            return chkResult;
        }

        #region 正式資料基本Method
        public bool LoadSystemCode(string iType, string iValue, out SystemCodeInfo iInfo)
        {
            bool Result = false;

            Database db = base.GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" select * from SystemCode with (nolock) ");
            sbCmd.AppendLine("    where Type = @Type ");
            sbCmd.AppendLine("    and Value = @Value ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, iType);
            db.AddInParameter(dbCommand, "@Value", DbType.String, iValue);
            #endregion

            base.ErrFlag = true;
            DataTable dtTemp = ExecuteDataSet(db, dbCommand).Tables[0];
            if (dtTemp.Rows.Count == 0)
            {
                Result = false;
                iInfo = new SystemCodeInfo();
            }
            else
            {
                Result = true;
                iInfo = new SystemCodeInfo(dtTemp.Rows[0]);
            }
            return Result;
        }

        //新增
        public bool InsSystemCode(SystemCodeInfo info)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" INSERT INTO SystemCode ");
            sbCmd.AppendLine(" (Type, Value, Text, DisplayOrder, Value2, REMARKS, Maker, Maker_Time, Checker, Checker_Time) ");
            sbCmd.AppendLine(" Values ");
            sbCmd.AppendLine(" (@Type, @Value, @Text, @DisplayOrder, @Value2, @REMARKS, @Maker, @Maker_Time, @Checker, @Checker_Time) ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, info.Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, info.Value);
            db.AddInParameter(dbCommand, "@Text", DbType.String, info.Text);
            db.AddInParameter(dbCommand, "@DisplayOrder", DbType.Int16, info.DisplayOrder);
            db.AddInParameter(dbCommand, "@Value2", DbType.String, info.Value2);
            db.AddInParameter(dbCommand, "@REMARKS", DbType.String, info.REMARKS);
            db.AddInParameter(dbCommand, "@Maker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Maker_Time", DbType.DateTime, info.Maker_Time);
            db.AddInParameter(dbCommand, "@Checker", DbType.String, info.Checker);
            db.AddInParameter(dbCommand, "@Checker_Time", DbType.DateTime, info.Checker_Time);
            #endregion

            return ExecuteNonQuery(db, dbCommand);
        }

        //更新
        public bool UpdSystemCode(SystemCodeInfo info)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" UPDATE SystemCode ");
            sbCmd.AppendLine("    SET Text = @Text ");
            sbCmd.AppendLine("       ,DisplayOrder = @DisplayOrder ");
            sbCmd.AppendLine("       ,Value2 = @Value2 ");
            sbCmd.AppendLine("       ,REMARKS = @REMARKS ");
            sbCmd.AppendLine("       ,Maker = @Maker ");
            sbCmd.AppendLine("       ,Maker_Time = @Maker_Time ");
            sbCmd.AppendLine("       ,Checker = @Checker ");
            sbCmd.AppendLine("       ,Checker_Time = @Checker_Time ");
            sbCmd.AppendLine("  WHERE Type = @Type ");
            sbCmd.AppendLine("    AND Value = @Value ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, info.Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, info.Value);
            db.AddInParameter(dbCommand, "@Text", DbType.String, info.Text);
            db.AddInParameter(dbCommand, "@DisplayOrder", DbType.Int16, info.DisplayOrder);
            db.AddInParameter(dbCommand, "@Value2", DbType.String, info.Value2);
            db.AddInParameter(dbCommand, "@REMARKS", DbType.String, info.REMARKS);
            db.AddInParameter(dbCommand, "@Maker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Maker_Time", DbType.DateTime, info.Maker_Time);
            db.AddInParameter(dbCommand, "@Checker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Checker_Time", DbType.DateTime, info.Maker_Time);
            #endregion

            return ExecuteNonQuery(db, dbCommand);
        }

        //刪除
        public bool DelSystemCode(string Type, string Value)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" delete from SystemCode ");
            sbCmd.AppendLine("   where Type = @Type ");
            sbCmd.AppendLine("   and Value = @Value ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, Value);
            #endregion

            return ExecuteNonQuery(db, dbCommand);
        }

        #endregion


    }
}