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
    public class RES_BANKDB : baseDB
    {
        public RES_BANKDB()
        {

            DBInstanceName = "CONNSEC";
        }

        public DataTable getResBank_distinct()
        {

            //設定一個空的dataTable準備接資料
            DataTable dtTemp = new DataTable();
            //選取db路徑
            Database db = GetDatabase();

            //接資料字串的方法指定到stCmd
            StringBuilder stCmd = new StringBuilder();
            stCmd.AppendLine("select distinct BANK_CODE from RES_BANK");
            stCmd.AppendLine("where (1=1)");

            DbCommand dbCommand = db.GetSqlStringCommand(stCmd.ToString());
            //參數指定 需要顯示的下拉選單內容
            dtTemp = ExecuteDataSet(db, dbCommand, false).Tables[0];
            return dtTemp;
        }

        //依條件取出資料總筆數
        public DataTable getResBank_count(string BANK_CODE)
        {
            //設定一個空的dataTable準備接資料
            DataTable dtTemp = new DataTable();
            //選取db路徑
            Database db = GetDatabase();
            //建立一個具有接字串方法的sbCmd來放字串
            StringBuilder sbCmd = new StringBuilder();

            //根據SQL語法組字串
            sbCmd.Append(" select count(*) as ROW_COUNT from RES_BANK where 1 = 1 ");
            if (!string.IsNullOrEmpty(BANK_CODE)) sbCmd.AppendLine(" and BANK_CODE = @BANK_CODE ");

            //把組好的字串放到dbCommand
            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            //參數指定,產生指定變數的parm
            db.AddInParameter(dbCommand, "@BANK_CODE", DbType.String, BANK_CODE);

            //把撈到的資料設成一張table給dtTemp
            dtTemp = ExecuteDataSet(db, dbCommand, false).Tables[0];

            return dtTemp;

            //取出第一列第一欄的資料 DataTable.Rows[rowindex][columnindex]
            //DataRow dr = dtTemp.Rows[0];
            //object obj = dr[0];
            //int result = obj;
        }

        //依條件篩選資料
        public DataTable getResBank_select(string BANK_CODE, int OffsetCount, int FetchCount)
        {

            DataTable dtTemp = new DataTable();
            Database db = GetDatabase();

            //使用OFFSET和FETCH做分頁處理的時候一定要加order by
            StringBuilder sbCmd = new StringBuilder();
            sbCmd.Append(" select * from RES_BANK where 1 = 1 ");
            if (!string.IsNullOrEmpty(BANK_CODE)) sbCmd.AppendLine(" and BANK_CODE = @BANK_CODE ");
            sbCmd.AppendLine(" order by BANK_CODE ");
            sbCmd.AppendLine(" OFFSET @OFFSET ROWS ");
            sbCmd.AppendLine(" FETCH NEXT @FETCH ROWS ONLY ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            //參數指定
            db.AddInParameter(dbCommand, "@BANK_CODE", DbType.String, BANK_CODE);
            db.AddInParameter(dbCommand, "@OFFSET", DbType.Int32, OffsetCount);
            db.AddInParameter(dbCommand, "@FETCH", DbType.Int32, FetchCount);

            dtTemp = ExecuteDataSet(db, dbCommand).Tables[0];
            return dtTemp;
        }

        //依Key值確認是否已存在於Table中 insert,update,view
        public bool chkRES_BNAKDB(string BANK_CODE)
        {
            bool chkResult = false;
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine("SELECT COUNT(*) ");
            sbCmd.AppendLine("  FROM RES_BANK with (nolock) ");
            sbCmd.AppendLine(" WHERE 1 = 1 ");
            sbCmd.AppendLine("   AND BANK_CODE = @BANK_CODE ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@BANK_CODE", DbType.String, BANK_CODE);
            #endregion

            //ExecuteScalar 返回table 第一行第一列的值 ,這個值為obj
            object o = ExecuteScalar(db, dbCommand);

            //判斷返回的obj 若 == null || "0", 代表沒資料,若有代表有資料
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
        public bool LoadBANKCODE(string iBANK_CODE, out RES_BANKInfo myInfo)
        {
            bool Result = false;
            //到db透過key值找編輯中的那筆資料
            Database db = base.GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" select * from RES_BANK with (nolock) ");
            sbCmd.AppendLine("    where BANK_CODE = @BANK_CODE ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@BANK_CODE", DbType.String, iBANK_CODE);

            #endregion

            DataTable dtTemp = ExecuteDataSet(db, dbCommand).Tables[0];

            //判斷找到的資料是否存在
            if (dtTemp.Rows.Count == 0)
            {
                Result = false;
                //把info內的預設值給myinfo
                myInfo = new RES_BANKInfo();
            }
            else
            {
                Result = true;
                //把info內已存在的dtTemp.Rows[0]值指定給myinfo
                myInfo = new RES_BANKInfo(dtTemp.Rows[0]);
            }
            return Result;
        }

        //新增
        public bool InsRES_BANK(RES_BANKInfo info)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" INSERT INTO RES_BANK ");
            sbCmd.AppendLine(" (BANK_CODE, BANK_NAME, TEL_NO, MAIL_1, MAIL_2, MAIL_3, Maker, Maker_Time, Checker, Checker_Time) ");
            sbCmd.AppendLine(" Values ");
            sbCmd.AppendLine(" (@BANK_CODE, @BANK_NAME, @TEL_NO, @MAIL_1, @MAIL_2, @MAIL_3, @Maker, @Maker_Time, @Checker, @Checker_Time) ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@BANK_CODE", DbType.String, info.BANK_CODE);
            db.AddInParameter(dbCommand, "@BANK_NAME", DbType.String, info.BANK_NAME);
            db.AddInParameter(dbCommand, "@TEL_NO", DbType.String, info.TEL_NO);
            db.AddInParameter(dbCommand, "@MAIL_1", DbType.String, info.MAIL_1);
            db.AddInParameter(dbCommand, "@MAIL_2", DbType.String, info.MAIL_2);
            db.AddInParameter(dbCommand, "@MAIL_3", DbType.String, info.MAIL_3);
            db.AddInParameter(dbCommand, "@Maker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Maker_Time", DbType.DateTime, info.Maker_Time);
            db.AddInParameter(dbCommand, "@Checker", DbType.String, info.Checker);
            db.AddInParameter(dbCommand, "@Checker_Time", DbType.DateTime, info.Checker_Time);
            #endregion

            //ExecuteNonQuery 傳回受影響的資料列數目
            //也就是說要在新增 修改 刪除時 執行這一段 值才會傳回資料庫
            //要是沒這一段就不會把要改變的數值傳回
            return ExecuteNonQuery(db, dbCommand);
        }

        //更新
        public bool UpdRES_BANK(RES_BANKInfo info)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" UPDATE RES_BANK ");
            sbCmd.AppendLine("    SET BANK_NAME = @BANK_NAME ");
            sbCmd.AppendLine("       ,TEL_NO = @TEL_NO ");
            sbCmd.AppendLine("       ,MAIL_1 = @MAIL_1 ");
            sbCmd.AppendLine("       ,MAIL_2 = @MAIL_2 ");
            sbCmd.AppendLine("       ,MAIL_3 = @MAIL_3 ");
            sbCmd.AppendLine("       ,Maker = @Maker ");
            sbCmd.AppendLine("       ,Maker_Time = @Maker_Time ");
            sbCmd.AppendLine("       ,Checker = @Checker ");
            sbCmd.AppendLine("       ,Checker_Time = @Checker_Time ");
            sbCmd.AppendLine("  WHERE BANK_CODE = @BANK_CODE ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@BANK_CODE", DbType.String, info.BANK_CODE);
            db.AddInParameter(dbCommand, "@BANK_NAME", DbType.String, info.BANK_NAME);
            db.AddInParameter(dbCommand, "@TEL_NO", DbType.String, info.TEL_NO);
            db.AddInParameter(dbCommand, "@MAIL_1", DbType.String, info.MAIL_1);
            db.AddInParameter(dbCommand, "@MAIL_2", DbType.String, info.MAIL_2);
            db.AddInParameter(dbCommand, "@MAIL_3", DbType.String, info.MAIL_3);
            db.AddInParameter(dbCommand, "@Maker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Maker_Time", DbType.DateTime, info.Maker_Time);
            db.AddInParameter(dbCommand, "@Checker", DbType.String, info.Maker);
            db.AddInParameter(dbCommand, "@Checker_Time", DbType.DateTime, info.Maker_Time);
            #endregion

            return ExecuteNonQuery(db, dbCommand);
        }

        //刪除
        public bool DelRES_BANK(string BANK_CODE, string BANK_NAME)
        {
            Database db = GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.AppendLine(" delete from RES_BANK ");
            sbCmd.AppendLine("   where BANK_CODE = @BANK_CODE ");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region 指定參數
            db.AddInParameter(dbCommand, "@BANK_CODE", DbType.String, BANK_CODE);
            #endregion

            return ExecuteNonQuery(db, dbCommand);
        }

        #endregion
    }

}