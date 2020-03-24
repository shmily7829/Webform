using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;

namespace Vista.Information
{
    /// <summary>
    /// 工作清單資訊
    /// </summary>
    public class LogTrackInfo : baseDB
    {

        /// <summary>
        /// 建構式 
        /// </summary>
        public LogTrackInfo()
        {
            base.DBInstanceName = "CONNDAR";


            ModifyFromIP = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
            ModifyUser = System.Web.HttpContext.Current.Session["UserID"].ToString();
            ModifyMode = "";
            ModifyTime = DateTime.Now.ToString();
            ModifyTable = "";
            ModifyKeyValue = "";
            ModifyBefore = "";
            ModifyAfter = "";
        }


        #region 公用變數

        /// <summary>
        /// 修改來源IP
        /// </summary>
        public string ModifyFromIP;

        /// <summary>
        /// 修改USER ID
        /// </summary>
        public string ModifyUser;

        /// <summary>
        /// 作業模式
        /// </summary>
        public string ModifyMode;

        /// <summary>
        /// 修改時間
        /// </summary>
        public string ModifyTime;

        /// <summary>
        /// 存取TABLE
        /// </summary>
        public string ModifyTable;

        /// <summary>
        /// 修改內容
        /// </summary>
        public string ModifyKeyValue;


        /// <summary>
        /// 修改前

        /// </summary>
        public string ModifyBefore;

        /// <summary>
        /// 修改後

        /// </summary>
        public string ModifyAfter;

        #endregion


        /// <summary>
        /// 新增異動紀錄

        /// </summary>
        /// <param name="infoObject">異動紀錄資訊</param>
        /// <returns></returns>
        public void Insert()
        {

            //不使用Transaction , 以正確的記錄發生那些Track事件
            using (System.Transactions.TransactionScope Ts = new System.Transactions.TransactionScope(TransactionScopeOption.Suppress))
            {

                //設定連結字串
                Database db = base.GetDatabase();

                StringBuilder sbCommand = new StringBuilder();

                sbCommand.Append("INSERT INTO Track_Log ");
                sbCommand.Append("  (ModifyFromIP, ModifyUser, ModifyMode, ModifyTable, ModifyKeyValue ,ModifyBefore , ModifyAfter) ");
                sbCommand.Append("VALUES ");
                sbCommand.Append("  (@ModifyFromIP, @ModifyUser, @ModifyMode,  @ModifyTable, @ModifyKeyValue,  @ModifyBefore, @ModifyAfter) ");

                DbCommand dbCommand = db.GetSqlStringCommand(sbCommand.ToString());

                db.AddInParameter(dbCommand, "@ModifyFromIP", DbType.String, this.ModifyFromIP);
                db.AddInParameter(dbCommand, "@ModifyUser", DbType.String, this.ModifyUser);
                db.AddInParameter(dbCommand, "@ModifyMode", DbType.String, this.ModifyMode);
                db.AddInParameter(dbCommand, "@ModifyTable", DbType.String, this.ModifyTable);
                db.AddInParameter(dbCommand, "@ModifyKeyValue", DbType.String, this.ModifyKeyValue);
                db.AddInParameter(dbCommand, "@ModifyKeyValue", DbType.String, this.ModifyBefore);
                db.AddInParameter(dbCommand, "@ModifyKeyValue", DbType.String, this.ModifyAfter);

                try
                {
                    db.ExecuteNonQuery(dbCommand);
                }
                catch 
                {
                    throw;
                }
            }
        }

    }
}
