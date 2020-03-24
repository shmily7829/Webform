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

        public enum SelectType { Count, Query}

        public enum WorkType { Maker, Checker}

        public enum TableType { Buffer, Official }       

        #region Buffer基本Method

        public bool LoadSystemCodeBuff(string iType, string iValue, out SystemCodeBufInfo iInfo)
        {
            bool Result = false;

            Database db = base.GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" select * from SystemCodeBuf with (nolock) ");
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
                iInfo = new SystemCodeBufInfo();
            }
            else
            {
                Result = true;
                iInfo = new SystemCodeBufInfo(dtTemp.Rows[0]);
            }

            return Result;
        }

        //新增
        public bool InsSystemCodeBuff(SystemCodeBufInfo info)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" INSERT INTO SystemCodeBuf ");
            sbCmd.AppendLine(" (Type, Value, Text, DisplayOrder, Value2, REMARKS, StepID, MODItem, Maker, Maker_Time) ");
            sbCmd.AppendLine(" Values ");
            sbCmd.AppendLine(" (@Type, @Value, @Text, @DisplayOrder, @Value2, @REMARKS, @StepID, @MODItem, @Maker, @Maker_Time) ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, info.Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, info.Value);
            db.AddInParameter(dbCommand, "@Text", DbType.String, info.Text);
            db.AddInParameter(dbCommand, "@DisplayOrder", DbType.Int16, info.DisplayOrder);
            db.AddInParameter(dbCommand, "@Value2", DbType.String, info.Value2);
            db.AddInParameter(dbCommand, "@REMARKS", DbType.String, info.REMARKS);
            db.AddInParameter(dbCommand, "@StepID", DbType.String, info.StepID);
            db.AddInParameter(dbCommand, "@MODItem", DbType.String, info.MODItem);
            db.AddInParameter(dbCommand, "@Maker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Maker_Time", DbType.DateTime, info.Maker_Time);
            #endregion

            return ExecuteNonQuery(db, dbCommand);
        }

        //更新
        public bool UpdSystemCodeBuff(SystemCodeBufInfo info)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" UPDATE SystemCodeBuf ");
            sbCmd.AppendLine("    SET Text = @Text ");
            sbCmd.AppendLine("       ,DisplayOrder = @DisplayOrder ");
            sbCmd.AppendLine("       ,Value2 = @Value2 ");
            sbCmd.AppendLine("       ,REMARKS = @REMARKS ");
            sbCmd.AppendLine("       ,StepID = @StepID ");
            sbCmd.AppendLine("       ,MODItem = @MODItem ");
            sbCmd.AppendLine("       ,Maker = @Maker ");
            sbCmd.AppendLine("       ,Maker_Time = @Maker_Time ");
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
            db.AddInParameter(dbCommand, "@StepID", DbType.String, info.StepID);
            db.AddInParameter(dbCommand, "@MODItem", DbType.String, info.MODItem);
            db.AddInParameter(dbCommand, "@Maker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Maker_Time", DbType.DateTime, info.Maker_Time);
            #endregion

            return ExecuteNonQuery(db, dbCommand);
        }

        //刪除
        public bool DelSysteCodeBuff(SystemCodeBufInfo info, DbTransaction iTxn = null)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" delete from SystemCodeBuf ");
            sbCmd.AppendLine("   where Type = @Type ");
            sbCmd.AppendLine("   and Value = @Value ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, info.Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, info.Value);
            #endregion

            return ExecuteNonQuery(db, dbCommand, iTxn);
        }

        #endregion

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
        public bool InsSystemCode(SystemCodeInfo info, DbTransaction iTxn = null)
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

            return ExecuteNonQuery(db, dbCommand, iTxn);
        }

        //更新
        public bool UpdSystemCode(SystemCodeInfo info, DbTransaction iTxn = null)
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
            db.AddInParameter(dbCommand, "@Checker", DbType.String, info.Checker);
            db.AddInParameter(dbCommand, "@Checker_Time", DbType.DateTime, info.Checker_Time);
            #endregion

            return ExecuteNonQuery(db, dbCommand, iTxn);
        }

        //刪除
        public bool DelSystemCode(SystemCodeInfo info, DbTransaction iTxn = null)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" delete from SystemCode ");
            sbCmd.AppendLine("   where Type = @Type ");
            sbCmd.AppendLine("   and Value = @Value ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, info.Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, info.Value);
            #endregion

            return ExecuteNonQuery(db, dbCommand, iTxn);
        }

        #endregion

        #region Log資料基本Method

        //新增
        public bool InsSystemCodeLog(SystemCodeLogInfo info, DbTransaction iTxn = null)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" INSERT INTO SystemCodeLog ");
            sbCmd.AppendLine(" (Type, Value, Text, DisplayOrder, Value2, REMARKS, MODItem, Maker, Maker_Time, Checker, Checker_Time) ");
            sbCmd.AppendLine(" Values ");
            sbCmd.AppendLine(" (@Type, @Value, @Text, @DisplayOrder, @Value2, @REMARKS, @MODItem, @Maker, @Maker_Time, @Checker, @Checker_Time) ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, info.Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, info.Value);
            db.AddInParameter(dbCommand, "@Text", DbType.String, info.Text);
            db.AddInParameter(dbCommand, "@DisplayOrder", DbType.Int16, info.DisplayOrder);
            db.AddInParameter(dbCommand, "@Value2", DbType.String, info.Value2);
            db.AddInParameter(dbCommand, "@REMARKS", DbType.String, info.REMARKS);
            db.AddInParameter(dbCommand, "@MODItem", DbType.String, info.MODItem);
            db.AddInParameter(dbCommand, "@Maker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Maker_Time", DbType.DateTime, info.Maker_Time);
            db.AddInParameter(dbCommand, "@Checker", DbType.String, info.Checker);
            db.AddInParameter(dbCommand, "@Checker_Time", DbType.DateTime, info.Checker_Time);
            #endregion

            return ExecuteNonQuery(db, dbCommand, iTxn);
        }

        #endregion

        public DataTable getSystemCode_distinct()
        {
            DataTable dtTemp = new DataTable();
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" select distinct type from SystemCode ");
            sbCmd.AppendLine(" where (1=1) ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 參數指定

            #endregion

            dtTemp = ExecuteDataSet(db, dbCommand, false).Tables[0];
            return dtTemp;
        }

        //依條件找出Maker或Checker資料
        public DataTable getSystemCode(string Type, string Value, string Text, SelectType SelType, WorkType WKType, int OffsetCount = 0, int FetchCount = 999)
        {
            DataTable dtTemp = new DataTable();
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            //sbCmd.AppendLine(@"");

            sbCmd.Append(" select count(*) as ROW_COUNT from systemcode where 1 = 1 ");
            if (!string.IsNullOrEmpty(Type)) sbCmd.AppendLine(" and Type = @Type ");
            if (!string.IsNullOrEmpty(Value)) sbCmd.AppendLine(" and Value = @Value ");
            if (!string.IsNullOrEmpty(Text)) sbCmd.AppendLine(" and Text like @Text ");

            sbCmd.Append(" select * from systemcode where 1 = 1 ");
            if (!string.IsNullOrEmpty(Type)) sbCmd.AppendLine(" and Type = @Type ");
            if (!string.IsNullOrEmpty(Value)) sbCmd.AppendLine(" and Value = @Value ");
            if (!string.IsNullOrEmpty(Text)) sbCmd.AppendLine(" and Text like @Text ");
            sbCmd.AppendLine(" order by Type, DisplayOrder ");
            sbCmd.AppendLine(" OFFSET @OFFSET ROWS ");
            sbCmd.AppendLine(" FETCH NEXT @FETCH ROWS ONLY ");

            #region

            /*
            if (!string.IsNullOrEmpty(Text))
                Text = '%' + Text + '%';

            if (SelType == SelectType.Count)
                sbCmd.AppendLine(" select count(*) as ROW_COUNT from ( ");

            sbCmd.AppendLine(" select Type, Value, Text, DisplayOrder, Value2, REMARKS, StepID, MODItem ");
            sbCmd.AppendLine("    from SystemCodeBuf with (nolock) ");
            sbCmd.AppendLine("    where (1=1) ");

            if (!string.IsNullOrEmpty(Type)) sbCmd.AppendLine(" and Type = @Type ");
            if (!string.IsNullOrEmpty(Value)) sbCmd.AppendLine(" and Value = @Value ");
            if (!string.IsNullOrEmpty(Text)) sbCmd.AppendLine(" and Text like @Text ");

            if (WKType == WorkType.Maker)
            {
                sbCmd.AppendLine(" and StepID <> '20' ");
                sbCmd.AppendLine(" union all ");
                sbCmd.AppendLine(" select A.Type, A.Value, A.Text, A.DisplayOrder, A.Value2, A.REMARKS, ");
                sbCmd.AppendLine("   '' as StepID, '' as MODItem ");
                sbCmd.AppendLine("   from SystemCode A with (nolock) ");
                sbCmd.AppendLine("   left join SystemCodeBuf B with (nolock) on A.Type = B.Type and A.Value = B.Value ");
                sbCmd.AppendLine("   where B.Type is null and B.Value is null ");

                if (!string.IsNullOrEmpty(Type)) sbCmd.AppendLine(" and A.Type = @Type ");
                if (!string.IsNullOrEmpty(Value)) sbCmd.AppendLine(" and A.Value = @Value ");
                if (!string.IsNullOrEmpty(Text)) sbCmd.AppendLine(" and A.Text like @Text ");
            }
            else
            {
                sbCmd.AppendLine(" and StepID = '20' ");
                sbCmd.AppendLine(" and Maker <> @Maker ");
            }

            if (SelType == SelectType.Count)
            {
                sbCmd.AppendLine(" ) A ");
            }
            else if (SelType == SelectType.Query)
            {
                sbCmd.AppendLine(" order by Type, DisplayOrder ");
                sbCmd.AppendLine(" OFFSET @OFFSET ROWS ");
                sbCmd.AppendLine(" FETCH NEXT @FETCH ROWS ONLY ");
            }
            */

            #endregion

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 參數指定
            db.AddInParameter(dbCommand, "@Type", DbType.String, Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, Value);
            db.AddInParameter(dbCommand, "@Text", DbType.String, Text);
            //db.AddInParameter(dbCommand, "@Maker", DbType.String, base.UserID);

            db.AddInParameter(dbCommand, "@Type", DbType.String, Type);
            db.AddInParameter(dbCommand, "@Value", DbType.String, Value);
            db.AddInParameter(dbCommand, "@Text", DbType.String, Text);
            db.AddInParameter(dbCommand, "@OFFSET", DbType.Int32, OffsetCount);
            db.AddInParameter(dbCommand, "@FETCH", DbType.Int32, FetchCount);
            #endregion

            dtTemp = ExecuteDataSet(db, dbCommand).Tables[0];
            return dtTemp;
        }

        //依Key值確認是否已存在於Table中
        public bool chkSystemCode(SystemCodeBufInfo info, TableType TableType, string CheckType = "")
        {
            return chkSystemCode(info.Type, info.Value, TableType, CheckType);
        }

        public bool chkSystemCode(string iType, string iValue, TableType TableType, string CheckType = "")
        {
            bool chkResult = false;
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine("SELECT COUNT(*) ");

            if (TableType == TableType.Buffer)
                sbCmd.AppendLine("  FROM SystemCodeBuf with (nolock) ");
            else
                sbCmd.AppendLine("  FROM SystemCode with (nolock) ");

            sbCmd.AppendLine(" WHERE 1 = 1 ");
            sbCmd.AppendLine("   AND Type = @Type ");
            sbCmd.AppendLine("   AND Value = @Value ");

            if (CheckType == "INCHECKER") sbCmd.AppendLine(" AND StepID = '20' ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@Type", DbType.String, iType);
            db.AddInParameter(dbCommand, "@Value", DbType.String, iValue);
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
    }
}