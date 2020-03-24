using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration;

namespace Vista.Information 
{
    public class ExpLogInfo : baseDB
    {

        /// <summary>
        /// Constructors
        /// </summary>		
        public ExpLogInfo()
        {
            base.DBInstanceName = "CONNDAR"; 
            this.Init();
        }


        #region Init
        private void Init()
        {
            this._SID = 0;                              //流水號
            this._ClassName = "";                       //發生例外之Class名稱
            this._MethodName = "";                      //發生例外之Method名稱
            this._ErrMsg = "";                          //錯誤訊息
            this._UDate = null;                         //發生時間
        }
        #endregion


        #region Private Properties
        private int _SID;
        private string _ClassName;
        private string _MethodName;
        private string _ErrMsg;
        private DateTime? _UDate;
        #endregion


        #region Public Properties

        /// <summary>
        /// 流水號
        /// </summary>
        public int SID
        {
            get { return _SID; }
            set { _SID = value; }
        }

        /// <summary>
        /// 發生例外之Class名稱
        /// </summary>
        public string ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }

        /// <summary>
        /// 發生例外之Method名稱
        /// </summary>
        public string MethodName
        {
            get { return _MethodName; }
            set { _MethodName = value; }
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public new string ErrMsg
        {
            get { return _ErrMsg; }
            set { _ErrMsg = value; }
        }

        /// <summary>
        /// 發生時間
        /// </summary>
        public DateTime? UDate
        {
            get { return _UDate; }
            set { _UDate = value; }
        }
        #endregion


        #region Methods

        /// <summary>
        /// 依據PK載入一筆資料
        /// </summary>
        /// <returns>true代表成功載入，false代表找不到任何資料</returns>
        public bool Load(int iSID)
        {
            bool Result = false;

            this._SID = iSID;

            Database db = base.GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.Append("	SELECT * FROM [ExpLog] WITH (Nolock) ");
            sbCmd.Append("	WHERE (1=1) ");
            sbCmd.Append("		AND SID = @SID 		");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region Add In Parameter

            db.AddInParameter(dbCommand, "@SID", DbType.Int32, this._SID);

            #endregion

            try
            {
                base.ErrFlag = true;
                DataTable dtTemp = db.ExecuteDataSet(dbCommand).Tables[0];
                if (dtTemp.Rows.Count == 0)
                {
                    base.EditMode = EditType.Insert;
                    Result = false;
                }
                else
                {
                    base.EditMode = EditType.Update;
                    Result = true;

                    DataRow dr = dtTemp.Rows[0];
                    this._SID = Convert.ToInt32(dr["SID"]);
                    this._ClassName = Convert.ToString(dr["ClassName"]);
                    this._MethodName = Convert.ToString(dr["MethodName"]);
                    this._ErrMsg = Convert.ToString(dr["ErrMsg"]);
                    this._UDate = dr["UDate"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(dr["UDate"]);
                }
            }
            catch (Exception ex)
            {
                throw; //將原來的 exception 再次的抛出去
            }

            return Result;
        }


        /// <summary>
        /// Insert
        /// </summary>
        public void Insert()
        {
            Database db = base.GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.Append("	INSERT INTO [ExpLog]		");
            sbCmd.Append("		(				");
            sbCmd.Append("		ClassName		");
            sbCmd.Append("		,MethodName		");
            sbCmd.Append("		,ErrMsg		");
            sbCmd.Append("		,UDate		");
            sbCmd.Append("		)				");
            sbCmd.Append("	VALUES		");
            sbCmd.Append("		(				");
            sbCmd.Append("		@ClassName		");
            sbCmd.Append("		,@MethodName		");
            sbCmd.Append("		,@ErrMsg		");
            sbCmd.Append("		,@UDate		");
            sbCmd.Append("		)				");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region Add In Parameter
            db.AddInParameter(dbCommand, "@ClassName", DbType.String, this._ClassName);
            db.AddInParameter(dbCommand, "@MethodName", DbType.String, this._MethodName);
            db.AddInParameter(dbCommand, "@ErrMsg", DbType.String, this._ErrMsg);
            db.AddInParameter(dbCommand, "@UDate", DbType.DateTime, this._UDate);
            #endregion

            try
            {
                db.ExecuteNonQuery(dbCommand);
                base.ErrFlag = true;
            }
            catch (Exception ex)
            {
                throw; //將原來的 exception 再次的抛出去
            }
        }


        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            Database db = base.GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            sbCmd.Append("	UPDATE [ExpLog] SET 		");
            sbCmd.Append("		ClassName = @ClassName 		");
            sbCmd.Append("		,MethodName = @MethodName 		");
            sbCmd.Append("		,ErrMsg = @ErrMsg 		");
            sbCmd.Append("		,UDate = @UDate 		");
            sbCmd.Append("	WHERE (1=1) ");
            sbCmd.Append("		AND SID = @SID 		");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region Add In Parameter
            db.AddInParameter(dbCommand, "@SID", DbType.Int32, this._SID);
            db.AddInParameter(dbCommand, "@ClassName", DbType.String, this._ClassName);
            db.AddInParameter(dbCommand, "@MethodName", DbType.String, this._MethodName);
            db.AddInParameter(dbCommand, "@ErrMsg", DbType.String, this._ErrMsg);
            db.AddInParameter(dbCommand, "@UDate", DbType.DateTime, this._UDate);
            #endregion

            try
            {
                db.ExecuteNonQuery(dbCommand);
                base.ErrFlag = true;
            }
            catch (Exception ex)
            {
                throw; //將原來的 exception 再次的抛出去
            }
        }


        /// <summary>
        /// Delete
        /// </summary>
        public void Delete(int iSID)
        {
            Database db = base.GetDatabase();
            StringBuilder sbCmd = new StringBuilder();

            this._SID = iSID;

            sbCmd.Append("	DELETE [ExpLog]		");
            sbCmd.Append("	WHERE (1=1) 		");
            sbCmd.Append("		AND SID = @SID 		");

            DbCommand dbCommand = db.GetSqlStringCommand(sbCmd.ToString());

            #region Add In Parameter

            db.AddInParameter(dbCommand, "@SID", DbType.Int32, this._SID);

            #endregion

            try
            {
                db.ExecuteNonQuery(dbCommand);
                base.ErrFlag = true;
            }
            catch (Exception ex)
            {
                throw; //將原來的 exception 再次的抛出去
            }
        }


        /// <summary>
        /// Save用法： 1. Info.Load() 2. Set Value 3. Info.Save()
        /// </summary>	
        public void Save()
        {
            if (base.EditMode == EditType.Insert)
            {
                this.Insert();
            }
            else
            {
                this.Update();
            }
        }

        #endregion
    }
}


#region Use Sample
/*
Vista.Information.ExpLogInfo Info = new Vista.Information.ExpLogInfo();
Info.SID = 0  ;                              //流水號
Info.ClassName = ""  ;                       //發生例外之Class名稱
Info.MethodName = ""  ;                      //發生例外之Method名稱
Info.ErrMsg = ""  ;                          //錯誤訊息
Info.UDate = null  ;                         //發生時間
*/
#endregion
