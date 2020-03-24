using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vista.DataAccess;
using Vista.Information;
using Microsoft.Security.Application;

public partial class RESW9003 : basePage
{
    #region 定義變數

    public enum ModeType { Query, AddNew, Modify };

    public ModeType Mode
    {
        get
        {
            return (ModeType)ViewState["Mode"];
        }
        set
        {
            ViewState["Mode"] = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitialForm();
            SetPlatform(ModeType.Query);            
        }
    }

    #region Button Function

    //查詢
    protected void btnQry_Click(object sender, EventArgs e)
    {
        if (Query())
        {
            DataListLoad();
        }
    }

    //新增
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SetPlatform(ModeType.AddNew);
        SetBtnStatus(false, false);
    }

    //暫存
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Save(Mode, true))
        {
            switch (Mode)
            {
                case ModeType.AddNew:
                    if (Insert("5"))
                    {
                        SetPlatform(ModeType.Query);

                        DataListLoad();
                    }
                    break;

                case ModeType.Modify:
                    if (Update("5"))
                    {
                        SetPlatform(ModeType.Query);

                        DataListLoad();
                    }
                    break;
            }
        }
    }

    //送審
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Save(Mode, false))
        {
            switch (Mode)
            {
                case ModeType.AddNew:
                    if (Insert("20"))
                    {
                        SetPlatform(ModeType.Query);

                        DataListLoad();
                    }
                    break;

                case ModeType.Modify:
                    if (Update("20"))
                    {
                        SetPlatform(ModeType.Query);

                        DataListLoad();
                    }
                    break;
            }
        }
    }

    //刪除
    protected void btnDel_Click(object sender, EventArgs e)
    {
        if (Delete())
        {
            SetPlatform(ModeType.Query);
            DataListLoad();
        }
    }

    //捨棄
    protected void btnDrop_Click(object sender, EventArgs e)
    {
        if (Drop())
        {
            SetPlatform(ModeType.Query);
            DataListLoad();
        }
    }

    //取消
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        SetPlatform(ModeType.Query);

        DataListLoad();
    }

    //編輯
    protected void rplbtnEdit_Click(object sender, EventArgs e)
    {
        RepeaterItem rp_item = ((sender as Button).NamingContainer) as RepeaterItem;

        string iType = ((Label)rp_item.FindControl("rplbType")).Text;
        string iValue = ((Label)rp_item.FindControl("rplbValue")).Text;

        if (View(iType, iValue))
            SetPlatform(ModeType.Modify);
        else
            DataListLoad();
    }

    #endregion        

    #region DataList Function

    protected void repDataList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Button btnEdit = ((Button)e.Item.FindControl("rplbtnEdit"));

            string StepID = drv["StepID"].ToString();

            switch (StepID)
            {
                case "5":
                    btnEdit.Text = "暫存中";
                    break;

                case "10":
                    btnEdit.Text = "被退件";
                    break;

                default:
                    btnEdit.Text = "編輯";
                    break;
            }
        }
    }

    protected void ucDataGridView1_ChangePageClick(int _OffsetCount, int _FetchCount)
    {
        string Type = ddlqType.SelectedValue;
        string Value = txtqValue.Text.Trim();
        string Text = txtqText.Text.Trim();

        SystemCodeDB myDB = new SystemCodeDB(base.strUserID, base.FormID);

        DataTable dt = myDB.getSystemCode(Type, Value, Text, SystemCodeDB.SelectType.Query, SystemCodeDB.WorkType.Maker, _OffsetCount, _FetchCount);

        repDataListBind(dt);
    }

    public void DataListLoad()
    {
        SystemCodeDB myDB = new SystemCodeDB(base.strUserID, base.FormID);

        string Type = ddlqType.SelectedValue;
        string Value = txtqValue.Text.Trim();
        string Text = txtqText.Text.Trim();

        #region 依查詢條件取得總筆數

        int TotalCount = 0;

        DataTable CountDt = myDB.getSystemCode(Type, Value, Text, SystemCodeDB.SelectType.Count, SystemCodeDB.WorkType.Maker);

        if (CountDt != null && CountDt.Rows.Count > 0)
        {
            DataRow CountDr = CountDt.Rows[0];

            TotalCount = Convert.ToInt32(CountDr["ROW_COUNT"]);
        }
        #endregion

        ucDataGridView1._TotalCount = TotalCount;
        ucDataGridView1.ToFirst();

    }

    protected void repDataListBind(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            plShowEmpty.Visible = false;
            plShowList.Visible = true;
        }
        else
        {
            plShowEmpty.Visible = true;
            plShowList.Visible = false;
        }

        repDataList.DataSource = dt;
        repDataList.DataBind();
        
    }

    #endregion

    #region 自訂Function

    /// <summary>
    /// 設定畫面顯示
    /// </summary>
    /// <param name="iMode">畫面狀態</param>
    public void SetPlatform(ModeType iMode)
    {
        Mode = iMode;
        switch (Mode)
        {
            case ModeType.Query:
                plQuery.Visible = true;
                plList.Visible = true;
                plDetails.Visible = false;

                QueryFocus();

                break;

            case ModeType.AddNew:
                plQuery.Visible = false;
                plList.Visible = false;
                plDetails.Visible = true;

                AddNewFocus();
                InitialAddNew();

                break;

            case ModeType.Modify:
                plQuery.Visible = false;
                plList.Visible = false;
                plDetails.Visible = true;

                ModifyFocus();
                InitialModify();

                break;
        }
    }

    /// <summary>
    /// 設定刪除跟捨棄按鈕狀態
    /// </summary>
    /// <param name="InBuff">是否在Buffer</param>
    /// <param name="InOfficial">是否在正式資料</param>
    public void SetBtnStatus(bool InBuff, bool InOfficial)
    {
        //Buffer有資料表示可以捨棄
        if (InBuff)
            btnDrop.Enabled = true;
        else
            btnDrop.Enabled = false;

        //有正式資料表示可以刪除
        if (InOfficial)
            btnDel.Enabled = true;
        else
            btnDel.Enabled = false;
    }

    /// <summary>
    /// 將 info 的值填入到 UI 中
    /// </summary>
    /// <param name="info"></param>
    private void FillModFieldValue(SystemCodeBufInfo iInfo)
    {
        //for 資料狀態
        lbModItem.Text = (iInfo.MODItem == "I") ? "新增"
            : (iInfo.MODItem == "U") ? "修改"
            : (iInfo.MODItem == "D") ? "刪除"
            : iInfo.MODItem;

        //data fields
        txtiType.Text = iInfo.Type;
        txtiValue.Text = iInfo.Value;
        txtiText.Text = iInfo.Text;
        txtiDisplayOrder.Text = iInfo.DisplayOrder.ToString();
        txtiValue2.Text = iInfo.Value2;
        txtiRemarks.Text = iInfo.REMARKS;
    }

    /// <summary>
    /// 將UI上的值指定到Info中
    /// </summary>
    private void GetModFieldValue(out SystemCodeBufInfo BuffInfo)
    {
        BuffInfo = new SystemCodeBufInfo();

        //將值指定到Info中
        BuffInfo.Type = txtiType.Text.Trim();
        BuffInfo.Value = txtiValue.Text.Trim();
        BuffInfo.Text = txtiText.Text.Trim();

        if (!string.IsNullOrEmpty(txtiDisplayOrder.Text.Trim()))
            BuffInfo.DisplayOrder = Convert.ToInt32(txtiDisplayOrder.Text);
        else
            BuffInfo.DisplayOrder = 0;

        BuffInfo.Value2 = txtiValue2.Text.Trim();
        BuffInfo.REMARKS = txtiRemarks.Text.Trim();
    }

    protected void BindSystemCodeType(DropDownList ddl)
    {
        SystemCodeDB myDB = new SystemCodeDB(base.strUserID, base.FormID);

        DataTable Dt = myDB.getSystemCode_distinct();

        ddlqType.DataSource = Dt;
        ddlqType.DataValueField = "type";
        ddlqType.DataTextField = "type";
        ddlqType.DataBind();
        ddlqType.Items.Insert(0, new ListItem("請選擇", ""));
    }

    #endregion

    #region Initial Function

    //作業平台欄位值初始化
    protected void InitialForm()
    {
        BindSystemCodeType(ddlqType);
    }

    //作業平台欄位值初始化(AddNew)
    protected void InitialAddNew()
    {
        lbModItem.Text = "新增";
        txtiType.Text = "";
        txtiValue.Text = "";
        txtiText.Text = "";
        txtiDisplayOrder.Text = "0";
        txtiValue2.Text = "";
        txtiRemarks.Text = "";

        //新增不能刪除跟捨棄
        //btnDrop.Enabled = false;
        //btnDel.Enabled = false;

        txtiType.Enabled = true;
        txtiValue.Enabled = true;
    }

    //作業平台欄位值初始化(Modify)
    protected void InitialModify()
    {
        txtiType.Enabled = false;
        txtiValue.Enabled = false;
    }

    #endregion

    #region Focus Function

    // 查詢 Focus 欄位
    protected void QueryFocus()
    {
        ddlqType.Focus();
    }

    //新增 Focus 欄位
    protected void AddNewFocus()
    {
        txtiType.Focus();
    }

    //更新 Focus 欄位
    protected void ModifyFocus()
    {

    }

    #endregion

    #region 欄位檢核

    //按下查詢按鈕(欄位檢核)
    protected bool Query()
    {
        return true;
    }

    //按下更新按鈕(欄位檢核)
    protected bool Save(ModeType Mode, bool SaveTemp)
    {
        if (string.IsNullOrEmpty(txtiType.Text.Trim()))
        {
            base.DoAlertinAjax(this, "SAVE", "選單類別為必填，請確認!");
            txtiType.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(txtiValue.Text.Trim()))
        {
            base.DoAlertinAjax(this, "SAVE", "代碼值為必填，請確認!");
            txtiValue.Focus();
            return false;
        }

        //若是暫存作業，無須完整檢查
        if (SaveTemp)
            return true;

        #region 此區塊暫存時不用檢查

        if (string.IsNullOrEmpty(txtiText.Text.Trim()))
        {
            base.DoAlertinAjax(this, "SAVE", "顯示名稱為必填，請確認!");
            txtiText.Focus();
            return false;
        }

        #endregion

        return true;
    }

    //按下編輯按鈕
    protected bool View(string Type, string Value)
    {
        SystemCodeDB myDB = new SystemCodeDB(base.strUserID, base.FormID);

        //是否有在正式資料中
        bool IsExistOfficial = myDB.chkSystemCode(Type, Value, SystemCodeDB.TableType.Official);

        //是否在Buffer資料中
        bool IsExistBuffer = myDB.chkSystemCode(Type, Value, SystemCodeDB.TableType.Buffer);

        if (IsExistBuffer || IsExistOfficial)
        {
            SystemCodeBufInfo iInfo = new SystemCodeBufInfo();

            if (IsExistBuffer)
            {
                myDB.LoadSystemCodeBuff(Type, Value, out iInfo);
            }
            else if (IsExistOfficial)
            {
                SystemCodeInfo myInfo;

                myDB.LoadSystemCode(Type, Value, out myInfo);

                iInfo = new SystemCodeBufInfo(myInfo);
            }

            FillModFieldValue(iInfo);

            SetBtnStatus(IsExistBuffer, IsExistOfficial);

            return true;
        }
        else
        {
            base.DoAlertinAjax(this, "SAVE", "該筆資料可能已被刪除，請重新查詢。");
            return false;
        }
    }

    #endregion

    #region 異動處理

    /// <summary>
    /// 資料新增
    /// </summary>
    protected bool Insert(string StepID)
    {
        SystemCodeDB myDB = new SystemCodeDB(base.strUserID, base.FormID);

        try
        {
            //取UI欄位值
            SystemCodeBufInfo BuffInfo;
            GetModFieldValue(out BuffInfo);

            //是否有在正式資料中
            bool IsExistOfficial = myDB.chkSystemCode(BuffInfo, SystemCodeDB.TableType.Official);

            //是否在Buffer資料中
            bool IsExistBuffer = myDB.chkSystemCode(BuffInfo, SystemCodeDB.TableType.Buffer);

            BuffInfo.StepID = StepID;
            BuffInfo.MODItem = IsExistOfficial ? "U" : "I";
            BuffInfo.Maker = base.strUserID;
            BuffInfo.Maker_Time = DateTime.Now;

            if (IsExistBuffer)
            {
                base.DoAlertinAjax(this, "SAVE", "該資料已存在，請重新查詢。");
                return false;
            }
            else
            {                
                myDB.InsSystemCodeBuff(BuffInfo);

                if(StepID == "5")
                    base.DoAlertinAjax(this, "SAVE", "暫存完成!");
                else
                    base.DoAlertinAjax(this, "SAVE", "送審完成!");

                return true;
            }
        }
        catch (Exception ex)
        {
            base.DoAlertinAjax(this, "SAVE", "作業發生錯誤，錯誤訊息:" + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// 資料更新
    /// </summary>
    protected bool Update(string StepID)
    {
        SystemCodeDB myDB = new SystemCodeDB(base.strUserID, base.FormID);

        try
        {
            //取UI欄位值
            SystemCodeBufInfo BuffInfo = null;
            GetModFieldValue(out BuffInfo);

            //是否有在正式資料中
            bool IsExistOfficial = myDB.chkSystemCode(BuffInfo, SystemCodeDB.TableType.Official);

            //是否在Buffer資料中
            bool IsExistBuffer = myDB.chkSystemCode(BuffInfo, SystemCodeDB.TableType.Buffer);

            BuffInfo.StepID = StepID;
            BuffInfo.MODItem = IsExistOfficial ? "U" : "I";
            BuffInfo.Maker = base.strUserID;
            BuffInfo.Maker_Time = DateTime.Now;

            //如果存在於Buffer就Update反之則新增
            if (IsExistBuffer)
                myDB.UpdSystemCodeBuff(BuffInfo);
            else
                myDB.InsSystemCodeBuff(BuffInfo);

            if (StepID == "5")
                base.DoAlertinAjax(this, "SAVE", "暫存完成!");
            else
                base.DoAlertinAjax(this, "SAVE", "送審完成!");

            return true;
        }
        catch (Exception ex)
        {
            base.DoAlertinAjax(this, "SAVE", "作業發生錯誤，錯誤訊息:" + ex.Message);
            return false;
        }        
    }

    /// <summary>
    /// 資料捨棄
    /// </summary>
    protected bool Drop()
    {
        SystemCodeDB myDB = new SystemCodeDB(base.strUserID, base.FormID);

        try
        {
            //取UI欄位值
            SystemCodeBufInfo BuffInfo = null;
            GetModFieldValue(out BuffInfo);

            //是否在Buffer資料且已經送審中
            bool IsExistBuffer = myDB.chkSystemCode(BuffInfo, SystemCodeDB.TableType.Buffer, "INCHECKER");

            if (IsExistBuffer)
            {
                base.DoAlertinAjax(this, "SAVE", "該筆資料已經送審，不可捨棄!");
                return false;
            }
            else
            {
                myDB.DelSysteCodeBuff(BuffInfo);

                base.DoAlertinAjax(this, "SAVE", "捨棄完成!");
                return true;
            }
        }
        catch (Exception ex)
        {
            base.DoAlertinAjax(this, "SAVE", "作業發生錯誤，錯誤訊息:" + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// 資料刪除
    /// </summary>
    protected bool Delete()
    {
        SystemCodeDB myDB = new SystemCodeDB(base.strUserID, base.FormID);

        try
        {
            //取UI欄位值
            SystemCodeBufInfo BuffInfo = null;
            GetModFieldValue(out BuffInfo);

            //是否有在正式資料中
            bool IsExistOfficial = myDB.chkSystemCode(BuffInfo, SystemCodeDB.TableType.Official);

            //是否在Buffer資料中
            bool IsExistBuffer = myDB.chkSystemCode(BuffInfo, SystemCodeDB.TableType.Buffer);

            BuffInfo.StepID = "20";
            BuffInfo.MODItem = "D";
            BuffInfo.Maker = base.strUserID;
            BuffInfo.Maker_Time = DateTime.Now;

            //若資料不在正式資料中就不可刪除
            if (!IsExistOfficial)
            {
                base.DoAlertinAjax(this, "SAVE", "該資料不存在於正式資料中，不可執行申請刪除作業!");
                return false;
            }

            //如果存在於Buffer就Update反之則新增
            if (IsExistBuffer)
                myDB.UpdSystemCodeBuff(BuffInfo);
            else
                myDB.InsSystemCodeBuff(BuffInfo);

            base.DoAlertinAjax(this, "SAVE", "申請刪除完成!");

            return true;
        }
        catch (Exception ex)
        {
            base.DoAlertinAjax(this, "SAVE", "作業發生錯誤，錯誤訊息:" + ex.Message);
            return false;
        }
    }

    #endregion    
}