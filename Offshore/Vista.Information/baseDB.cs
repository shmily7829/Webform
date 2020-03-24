using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Vista.Information
{
    /// <summary>
    /// 資料庫連線
    /// </summary>
    public class baseDB
    {
        #region 基本處理
        /// <summary>
        /// 連線字串
        /// </summary>
        private string _dbInstanceName;

        /// <summary>
        /// 連線字串
        /// </summary>
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

        /// <summary>
        /// 使用InstanceName取得資料連線設定
        /// </summary>
        /// <param name="InstanceName"></param>
        /// <returns></returns>
        protected Database GetDatabase()
        {
            if (!string.IsNullOrEmpty(DBInstanceName))
            {
                //Web UI 使用的Connection String不要用 Connection Pool
                return new SqlDatabase(Vista.SEC.ConnectionPool.GetConnection(DBInstanceName));
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region 物件狀態處理
        /// <summary>
        /// 列舉物件編輯狀態
        /// </summary>
        protected enum EditType
        {
            Insert,
            Update
        }

        protected EditType EditMode = EditType.Insert;

        #endregion

        #region Log Functions

        #region ErrorMessage
        /// <summary>
        /// 錯誤狀態Info
        /// </summary>
        public Vista.Information.ErrInfo ErrInfo = new Vista.Information.ErrInfo();

        /// <summary>
        /// 錯誤檢查 Trus為執行成功 / False為發生錯誤
        /// </summary>
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

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrMsg
        {
            get { return ErrInfo.ErrMsg; }
            set { ErrInfo.ErrMsg = value; }
        }

        /// <summary>
        /// 錯誤Method
        /// </summary>
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
        
        /// <summary>
        /// 記錄Exp資訊
        /// </summary>
        public void LogExpInf()   //UAT SIT peotected to public for odr002 test old data
        {
            //記錄狀態為Exception
            this.ErrFlag = false;

            //寫入Log   
            Vista.Information.ExpLogInfo myLogExpInfo = new Vista.Information.ExpLogInfo();
            myLogExpInfo.ClassName = this.GetType().FullName.ToString();
            myLogExpInfo.MethodName = this.ErrMethodName;
            myLogExpInfo.ErrMsg = this.ErrMsg;
            myLogExpInfo.UDate = DateTime.Now;
            myLogExpInfo.Insert();
        }
        
        /// <summary>
        /// 記錄Track資訊
        /// </summary>
        protected void LogTrackInf()
        {
            //寫入Track          
            Vista.Information.LogTrackInfo myTrackInfo = new Vista.Information.LogTrackInfo();
            myTrackInfo.ModifyMode = this.TrackMode;
            myTrackInfo.ModifyTable = this.TrackTable;
            myTrackInfo.ModifyKeyValue = this.TrackMsg;
            myTrackInfo.ModifyBefore = this.TrackBefore;
            myTrackInfo.ModifyAfter = this.TrackAfter;
            myTrackInfo.Insert();

        }
        #endregion

        ////記錄Track Log Sample
        //base.TrackMode = "Select" / "ADD" / "MOD" / "DEL" 
        //base.TrackTable = "Cust";
        //base.TrackMsg = "OK" / "other description"
        //base.LogTrackInf();

        ///記錄Exception Log Sample        ///
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

        #region 序列化處理
        /// <summary>
        /// 序列化物件
        /// </summary>
        /// <param name="strNameSpace">class名稱</param>
        /// <returns></returns>
        public string DoSerial(string strNameSpace)
        {
            System.Reflection.Assembly myAs = Assembly.Load("Vista.Information");
            Type myType = myAs.GetType("Vista.Information." + strNameSpace, true);

            XmlSerializer mySerializer = new XmlSerializer(myType);
            System.IO.StringWriter sw = new System.IO.StringWriter();
            //序列化

            mySerializer.Serialize(sw, this);
            string xmlForm = sw.ToString();
            sw.Close();

            #region 移除不需轉換的欄位 ErrFlag / ErrMsg / ErrMethodName
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlForm);

            xmlForm = xDoc.InnerXml.ToString();
            #endregion
            return xmlForm;
        }

        //解序列化
        /// <summary>
        /// 解序列化
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="strNameSpace">class名稱</param>
        /// <returns></returns>
        public object DeSerialize(string temp, string strNameSpace)
        {
            //取得物件名稱
            XmlDocument Xdoc = new XmlDocument();
            Xdoc.LoadXml(temp);

            System.Reflection.Assembly myAs = Assembly.Load("Vista.Information");

            Type myType = myAs.GetType("Vista.Information." + strNameSpace, true);
            StringReader sr = new StringReader(temp);

            object myPredict = new object();

            XmlSerializer mySerializer;

            mySerializer = new XmlSerializer(myType);
            myPredict = mySerializer.Deserialize(sr);

            sr.Close();

            return myPredict;
        }
        #endregion
    }
}
