using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Transactions;
using Vista.Information;

namespace Vista.DataAccess
{
    /// <summary>
    /// 資料庫連線
    /// </summary>
    public class baseDB
    {
        #region 基本處理
        public string UserID { get; set; }
        public string FunctionID { get; set; }

        //連線字串
        private string _dbInstanceName;

        //連線字串
        protected string DBInstanceName
        {
            get
            {
                return _dbInstanceName;
            }
            set
            {
                _dbInstanceName = value;
            }
        }

        //public DbTransaction btrans = null;
        //public DbConnection bconn = null;

        /// <summary>
        /// 使用InstanceName取得資料連線設定
        /// </summary>
        /// <param name="InstanceName"></param>
        /// <returns></returns>
        protected Database GetDatabase()
        {
            if (!string.IsNullOrEmpty(DBInstanceName))
            {
                return new SqlDatabase(DBSSEC.ConnectionPool.GetConnection(DBInstanceName));
                //return new SqlDatabase("Server=192.168.0.110;database=DBS_RES;uid=sa;pwd=1");
            }
            else
            {
                return null;
            }
        }

        protected Database GetPIPADatabase()
        {
            if (!string.IsNullOrEmpty("CONNPIPA"))
            {
                string strConnectionString = Vista.DBSSEC.ConnectionPool.GetConnection("CONNPIPA");
                if (strConnectionString.ToUpper().IndexOf("TIMEOUT") < 0)
                {
                    strConnectionString += ";Connection Timeout=3;";
                }
                return new SqlDatabase(strConnectionString);
                //return new SqlDatabase("Server=192.168.0.110;database=DBS_RES;uid=sa;pwd=1");
            }
            else
            {
                return null;
            }
        }

        public DbConnection CreateConnection()
        {
            Database db = GetDatabase();
            return db.CreateConnection();
        }

        public DbTransaction GetDbTransaction(DbConnection conn)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            return conn.BeginTransaction();
        }

        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbCommand"></param>
        /// <param name="InsLog">預設 False</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(Database db, DbCommand dbCommand, bool InsLog = true)
        {
            int iAffectedNum;
            return ExecuteNonQuery(db, dbCommand, null, out iAffectedNum, InsLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbCommand"></param>
        /// <param name="trans"></param>
        /// <param name="InsLog">預設 False</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(Database db, DbCommand dbCommand, DbTransaction trans, bool InsLog = true)
        {
            int iAffectedNum;
            return ExecuteNonQuery(db, dbCommand, trans, out iAffectedNum, InsLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbCommand"></param>
        /// <param name="iAffectedNum"></param>
        /// <param name="InsLog">預設 False</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(Database db, DbCommand dbCommand, out int iAffectedNum, bool InsLog = true)
        {
            return ExecuteNonQuery(db, dbCommand, null, out iAffectedNum, InsLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbCommand"></param>
        /// <param name="trans"></param>
        /// <param name="iAffectedNum"></param>
        /// <param name="InsLog">預設 False</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(Database db, DbCommand dbCommand, DbTransaction trans, out int iAffectedNum, bool InsLog = true)
        {
            DbTransaction _trans;
            DbConnection conn = null;
            if (trans == null)
            {
                conn = db.CreateConnection();
                conn.Open();
                _trans = GetDbTransaction(conn);
            }
            else
            {
                _trans = trans;
            }

            try
            {
                iAffectedNum = db.ExecuteNonQuery(dbCommand, _trans);

                ErrFlag = (iAffectedNum <= 0 ? false : true);

                if (trans == null)
                {
                    _trans.Commit();
                    conn.Close();
                }
                return ErrFlag;
            }
            catch (Exception ex)
            {
                //預防遇到某些致命錯誤導致Transaction中斷,所以重新再取得Trans物件作Rollback
                if (_trans.Connection == null)
                {
                    conn = db.CreateConnection();
                    conn.Open();
                    _trans = GetDbTransaction(conn);
                }

                if (trans == null)
                {
                    _trans.Rollback();
                    conn.Close();
                }

                #region 呼叫Base.LogExpInf 進行Exception Log 記錄 (固定寫法)
                //取得目前MethodName
                System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame();
                System.Reflection.MethodBase myMethodBase = stackFrame.GetMethod();

                ErrFlag = false;
                ErrMsg = ex.ToString();
                ErrMethodName = myMethodBase.Name.ToString();
                LogExpInf();
                #endregion

                throw; //將原來的 exception 再次的抛出去
            }
            finally
            {
                if (InsLog) InsPIPALog(dbCommand, UserID, FunctionID);
            }
        }
        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbCommand"></param>
        /// <param name="InsLog">預設 True</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(Database db, DbCommand dbCommand, bool InsLog = true)
        {
            return ExecuteDataSet(db, dbCommand, null, InsLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbCommand"></param>
        /// <param name="trans"></param>
        /// <param name="InsLog">預設 True</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(Database db, DbCommand dbCommand, DbTransaction trans, bool InsLog = true)
        {
            DataSet dsTemp = new DataSet();
            DbTransaction _trans;
            DbConnection conn = null;
            if (trans == null)
            {
                conn = db.CreateConnection();
                conn.Open();
                _trans = GetDbTransaction(conn);
            }
            else
            {
                _trans = trans;
            }

            try
            {
                dsTemp = db.ExecuteDataSet(dbCommand, _trans);


                if (trans == null)
                {
                    _trans.Commit();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //預防遇到某些致命錯誤導致Transaction中斷,所以重新再取得Trans物件作Rollback
                if (_trans.Connection == null)
                {
                    conn = db.CreateConnection();
                    conn.Open();
                    _trans = GetDbTransaction(conn);
                }

                if (trans == null)
                {
                    _trans.Rollback();
                    conn.Close();
                }
                #region 呼叫Base.LogExpInf 進行Exception Log 記錄 (固定寫法)
                //取得目前MethodName
                System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame();
                System.Reflection.MethodBase myMethodBase = stackFrame.GetMethod();

                ErrFlag = false;
                ErrMsg = ex.ToString();
                ErrMethodName = myMethodBase.Name.ToString();
                LogExpInf();
                #endregion

                throw; //將原來的 exception 再次的抛出去
            }
            finally
            {
                if (InsLog) InsPIPALog(dbCommand, UserID, FunctionID);
            }

            return dsTemp;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbCommand"></param>
        /// <param name="InsLog">預設 False</param>
        /// <returns></returns>
        public object ExecuteScalar(Database db, DbCommand dbCommand, bool InsLog = false)
        {
            return ExecuteScalar(db, dbCommand, null, InsLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbCommand"></param>
        /// <param name="trans"></param>
        /// <param name="InsLog">預設 False</param>
        /// <returns></returns>
        public object ExecuteScalar(Database db, DbCommand dbCommand, DbTransaction trans, bool InsLog = false)
        {
            object o = null;
            DbTransaction _trans;
            DbConnection conn = null;
            if (trans == null)
            {
                conn = db.CreateConnection();
                conn.Open();
                _trans = GetDbTransaction(conn);
            }
            else
            {
                _trans = trans;
            }

            try
            {
                o = db.ExecuteScalar(dbCommand, _trans);

                if (trans == null)
                {
                    _trans.Commit();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //預防遇到某些致命錯誤導致Transaction中斷,所以重新再取得Trans物件作Rollback
                if (_trans.Connection == null)
                {
                    conn = db.CreateConnection();
                    conn.Open();
                    _trans = GetDbTransaction(conn);
                }

                if (trans == null)
                {
                    _trans.Rollback();
                    conn.Close();
                }

                #region 呼叫Base.LogExpInf 進行Exception Log 記錄 (固定寫法)
                //取得目前MethodName
                System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame();
                System.Reflection.MethodBase myMethodBase = stackFrame.GetMethod();

                ErrFlag = false;
                ErrMsg = ex.ToString();
                ErrMethodName = myMethodBase.Name.ToString();
                LogExpInf();
                #endregion

                throw; //將原來的 exception 再次的抛出去
            }
            finally
            {
                if (InsLog) InsPIPALog(dbCommand, UserID, FunctionID);
            }

            return o;
        }
        #endregion

        #region PIPA LOG
        public bool InsPIPALog(DbCommand logdbCommand, string LogUser, string LogFunction)
        {
            string LogParameter = "";
            foreach (DbParameter iPara in logdbCommand.Parameters)
            {
                LogParameter += iPara.ParameterName + ":" + iPara.Value.ToString() + "; ";
            }

            try
            {
                Database db = GetPIPADatabase();
                if (db == null) return false;
                StringBuilder sbCmd = new StringBuilder();

                sbCmd.AppendLine(@"INSERT INTO dbo.PIPA_LOG
 (LogTime ,LogUser ,LogFunction ,LogSql ,LogParameter)
 VALUES
 (getdate() ,@LogUser ,@LogFunction ,@LogSql ,@LogParameter) ");

                DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());
                dbCommand.CommandTimeout = 3;

                #region 參數指定
                db.AddInParameter(dbCommand, "@LogUser", DbType.String, LogUser);
                db.AddInParameter(dbCommand, "@LogFunction", DbType.String, LogFunction);
                db.AddInParameter(dbCommand, "@LogSql", DbType.String, logdbCommand.CommandText);
                db.AddInParameter(dbCommand, "@LogParameter", DbType.String, LogParameter);
                #endregion



                int i = db.ExecuteNonQuery(dbCommand);
                ErrFlag = (i == 0 ? false : true);

                return ErrFlag;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Log Functions

        #region ErrorMessage

        //錯誤狀態Info
        public Vista.Information.ErrInfo ErrInfo = new Vista.Information.ErrInfo();

        //錯誤檢查 Trus為執行成功 / False為發生錯誤
        public bool ErrFlag
        {
            get { return ErrInfo.ErrFlag; }
            set
            {
                ErrInfo.ErrFlag = value;

                //狀態清除時重設相關欄位
                if (value)
                {
                    this.ErrInfo.ErrMethodName = "";
                    this.ErrInfo.ErrMsg = "";
                }
            }
        }

        //錯誤訊息
        public string ErrMsg
        {
            get { return ErrInfo.ErrMsg; }
            set { ErrInfo.ErrMsg = value; }
        }

        //錯誤Method
        public string ErrMethodName
        {
            get { return ErrInfo.ErrMethodName; }
            set { ErrInfo.ErrMethodName = value; }
        }
        #endregion

        #region TrackMessage
        /// <summary>
        /// Track Mode ( Add / Mod / Del )
        /// </summary>
        [NonSerialized()]
        public string TrackMode;

        /// <summary>
        /// Track Table Name
        /// </summary>
        [NonSerialized()]
        public string TrackTable;

        /// <summary>
        /// Track MSG / Key
        /// </summary>
        [NonSerialized()]
        public string TrackMsg;


        /// <summary>
        /// Track Before
        /// </summary>
        [NonSerialized()]
        public string TrackBefore;


        /// <summary>
        /// Track After
        /// </summary>
        [NonSerialized()]
        public string TrackAfter;
        #endregion

        #region Execute Log

        //記錄Exp資訊
        public void LogExpInf()
        {
            //記錄狀態為Exception
            this.ErrFlag = false;

            //寫入Log
            //不使用Transaction , 以正確的記錄發生那些Error
            using (System.Transactions.TransactionScope Ts = new System.Transactions.TransactionScope(TransactionScopeOption.Suppress))
            {

                //設定連結字串
                Database db = this.GetDatabase();

                StringBuilder sbCommand = new StringBuilder();

                sbCommand.Append("INSERT INTO ExpLog (ClassName, MethodName, ErrMsg, UDate) ");
                sbCommand.Append("VALUES (@ClassName,@MethodName,@ErrMsg, getdate()) ");

                DbCommand dbCommand = db.GetSqlStringCommand(sbCommand.ToString());

                db.AddInParameter(dbCommand, "@ClassName", DbType.String, this.GetType().FullName.ToString());
                db.AddInParameter(dbCommand, "@MethodName", DbType.String, this.ErrMethodName);
                db.AddInParameter(dbCommand, "@ErrMsg", DbType.String, this.ErrMsg);

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

        /// <summary>
        /// 記錄Track資訊
        /// </summary>
        protected void LogTrackInf()
        {
            //寫入Track
            //不使用Transaction , 以正確的記錄發生那些Track事件
            using (System.Transactions.TransactionScope Ts = new System.Transactions.TransactionScope(TransactionScopeOption.Suppress))
            {

                //設定連結字串
                Database db = this.GetDatabase();

                StringBuilder sbCommand = new StringBuilder();

                sbCommand.Append("INSERT INTO Track_Log ");
                sbCommand.Append("  (ModifyFromIP, ModifyUser, ModifyMode, ModifyTable, ModifyKeyValue ,ModifyBefore , ModifyAfter) ");
                sbCommand.Append("VALUES ");
                sbCommand.Append("  (@ModifyFromIP, @ModifyUser, @ModifyMode,  @ModifyTable, @ModifyKeyValue,  @ModifyBefore, @ModifyAfter) ");

                DbCommand dbCommand = db.GetSqlStringCommand(sbCommand.ToString());

                db.AddInParameter(dbCommand, "@ModifyFromIP", DbType.String, "");
                db.AddInParameter(dbCommand, "@ModifyUser", DbType.String, "");
                db.AddInParameter(dbCommand, "@ModifyMode", DbType.String, this.TrackMode);
                db.AddInParameter(dbCommand, "@ModifyTable", DbType.String, this.TrackTable);
                db.AddInParameter(dbCommand, "@ModifyKeyValue", DbType.String, this.TrackMsg);
                db.AddInParameter(dbCommand, "@ModifyBefore", DbType.String, this.TrackBefore);
                db.AddInParameter(dbCommand, "@ModifyAfter", DbType.String, this.TrackAfter);

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
        #endregion

        ////記錄Track Log Sample
        //base.TrackMode = "Select" / "ADD" / "MOD" / "DEL" 
        //base.TrackTable = "Cust";
        //base.TrackMsg = "OK" / "other description"
        //base.LogTrackInf();

        ///記錄Exception Log Sample
        #region 呼叫Base.LogExpInf 進行Exception Log 記錄 (固定寫法)
        ////取得目前MethodName
        //System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame();
        //System.Reflection.MethodBase myMethodBase = stackFrame.GetMethod();

        //base.ErrFlag = false;
        //base.ErrMsg = ex.ToString();
        //base.ErrMethodName = myMethodBase.Name.ToString();
        //base.LogExpInf(); 
        #endregion

        #endregion
    }
}