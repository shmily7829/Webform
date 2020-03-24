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

public partial class RESW9021 : basePage
{
    #region 定義變數

    public enum ModeType { Query, AddNew, Modify };

    //public ModeType Mode;

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

    #region Initial Function

    protected void Page_Load(object sender, EventArgs e)
    {
        //判斷user是不是第一次進入頁面,是的話就進行初始化
        if (!IsPostBack)
        {
            InitialForm();
            SetPlatform(ModeType.Query); //控制panel顯示
        }
    }

    public void InitialForm()
    {
        BindBankcode(ddlqCode); //呼叫bind()把ddlqCode傳進去
    }

    //作業平台欄位值初始化(AddNew)
    protected void InitialAddNew()
    {
        lbModItem.Text = "新增";
        txtiBANK_CODE.Text = "";
        txtiBANK_NAME.Text = "";
        txtiMAIL_1.Text = "";
        txtiMAIL_2.Text = "";
        txtiMAIL_3.Text = "";
        
        //原本在編輯狀態下的key值欄位預設是false,
        //在add new的時候要打開
        txtiBANK_CODE.Enabled = true;
    }

    //作業平台欄位值初始化(Modify)
    protected void InitialModify()
    {
        txtiBANK_CODE.Enabled = false;

        btnDel.Enabled = true;
    }
    #endregion

    #region Click Button Function
    //查詢
    protected void btnQ_Click(object sender, EventArgs e)
    {
        //按下btnQ後進行Query()欄位檢查
        //開始撈db
        if (Query())
        {
            DataListLoad();
        }
    }

    //新增
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SetPlatform(ModeType.AddNew);
        SetBtnStatus(false);
    }

    //取消
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //重回查詢畫面
        SetPlatform(ModeType.Query);
        //為防止user在過程中資料有異動,所以重新load一次DB資料
        DataListLoad();
    }

    //編輯
    protected void rplbtnEdit_Click(object sender, EventArgs e)
    {
        //*Review*
        //從RepeaterItem清單底部開始找按鈕,確認使用者按下編輯的資料列為何
        RepeaterItem rp_item = ((sender as Button).NamingContainer) as RepeaterItem;

        //設定物件為Label屬性,並取得text值
        string iBANK_CODE = ((Label)rp_item.FindControl("rplbBANK_CODE")).Text;
        //string iBANK_NAME = HttpUtility.HtmlDecode(((Label)rp_item.FindControl("rplbBANK_NAME")).Text);

        //透過view()檢查編輯的資料欄位狀態
        //如果選取的資料列可編輯則進入編輯狀態,若否則重load資料
        if (View(iBANK_CODE))
            SetPlatform(ModeType.Modify);
        else
            DataListLoad();
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

    //暫存
    protected void btnSave_Click(object sender, EventArgs e)
    {
        switch (Mode)
        {
            case ModeType.AddNew://新增
                if (Insert())
                {
                    SetPlatform(ModeType.Query);

                    DataListLoad();
                }
                break;

            case ModeType.Modify://編輯
                if (Update())
                {
                    SetPlatform(ModeType.Query);

                    DataListLoad();
                }
                break;
        }
    }

    #endregion

    #region DataList Funtion
    /// <summary>
    /// 查詢資料筆數
    /// </summary>
    public void DataListLoad()
    {
        RES_BANKDB myDB = new RES_BANKDB();

        string BANK_CODE = ddlqCode.SelectedValue;

        int TotalCount = 0;

        //透過getResBank_count取得DB資料
        DataTable CountDt = myDB.getResBank_count(BANK_CODE);

        //檢查CountDt內是否有資料
        if (CountDt != null && CountDt.Rows.Count > 0)
        {
            //取出ROW_COUNT內計算出來的資料筆數
            DataRow CountDr = CountDt.Rows[0];

            TotalCount = Convert.ToInt32(CountDr["ROW_COUNT"]);
        }

        //把資料筆數設到ucDataGridView1._TotalCoun
        //回到第一頁
        ucDataGridView1._TotalCount = TotalCount;
        ucDataGridView1.ToFirst();
    }

    /// <summary>
    /// 進行分頁處理
    /// </summary>
    protected void ucDataGridView1_ChangePageClick(int _OffsetCount, int _FetchCount)
    {
        string BANK_CODE = ddlqCode.SelectedValue;

        RES_BANKDB myDB = new RES_BANKDB();
        DataTable dt = myDB.getResBank_select(BANK_CODE,_OffsetCount, _FetchCount);
        repDataListBind(dt);
    }

    /// <summary>
    /// 查詢結果顯示判斷
    /// </summary>
    public void repDataListBind(DataTable dt)
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

        //取得或設定資料來源中提供的資料填入清單。
        repDataList.DataSource = dt;

        //將結果傳送至畫面
        repDataList.DataBind();

    }
    #endregion

    #region 自訂Function
    /// <summary>
    /// 設定畫面顯示區塊
    /// </summary>
    /// <param name="iMode">畫面狀態</param>
    public void SetPlatform(ModeType iMode)
    {
        Mode = iMode;

        switch (Mode)
        {
            //查詢顯示
            case ModeType.Query:
                plQuery.Visible = true;     //查詢區塊
                plList.Visible = true;      //清單區塊
                plDetails.Visible = false;  //細節區塊,addnew或modify的時候才預設顯示

                QueryFocus();

                break;

            //新增顯示
            case ModeType.AddNew:
                plQuery.Visible = false;
                plList.Visible = false;
                plDetails.Visible = true;

                AddNewFocus();
                InitialAddNew();

                break;

            //編輯顯示
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
    /// 設定刪除按鈕狀態
    /// </summary>
    /// <param name="InOfficial">是否在正式資料</param>
    public void SetBtnStatus(bool InOfficial)
    {

        //有正式資料表示可以刪除
        if (InOfficial)
            btnDel.Enabled = true;
        else
            btnDel.Enabled = false;
    }

    /// <summary>
    /// 將 info 的值填入到 UI 中
    /// </summary>
    private void FillModFieldValue(RES_BANKInfo myInfo)
    {
        //資料狀態
        lbModItem.Text = "修改";

        //data fields
        txtiBANK_CODE.Text = myInfo.BANK_CODE;
        txtiBANK_NAME.Text = myInfo.BANK_NAME;
        txtiTEL_NO.Text = myInfo.TEL_NO;
        txtiMAIL_1.Text = myInfo.MAIL_1;
        txtiMAIL_2.Text = myInfo.MAIL_2;
        txtiMAIL_3.Text = myInfo.MAIL_3;
    }


    //將UI上的值指定到Info中
    //傳值呼叫方法
    /*
    private RES_BANKInfo GetModFieldValue()
    {
        RES_BANKInfo iInfo = new RES_BANKInfo();

        //將值指定到Info中
        iInfo.BANK_CODE = txtiBANK_CODE.Text.Trim();
        iInfo.BANK_NAME = txtiBANK_NAME.Text.Trim();
        iInfo.TEL_NO = txtiTEL_NO.Text.Trim();
        iInfo.MAIL_1 = txtiMAIL_1.Text.Trim();
        iInfo.MAIL_2 = txtiMAIL_2.Text.Trim();
        iInfo.MAIL_3 = txtiMAIL_3.Text.Trim();
        iInfo.Maker_Time = DateTime.Now;
        iInfo.Checker_Time = DateTime.Now;

        return iInfo;

        //將值指定到Info中

        //if (!string.IsNullOrEmpty(txtiDisplayOrder.Text.Trim()))
        //    BuffInfo.DisplayOrder = Convert.ToInt32(txtiDisplayOrder.Text);
        //else
        //    BuffInfo.DisplayOrder = 0;

    }
    */
    //傳址呼叫方法
    private void GetModFieldValue(out RES_BANKInfo iInfo)
    {

        iInfo = new RES_BANKInfo();

        //將值指定到Info中
        iInfo.BANK_CODE = txtiBANK_CODE.Text.Trim();
        iInfo.BANK_NAME = txtiBANK_NAME.Text.Trim();
        iInfo.TEL_NO = txtiTEL_NO.Text.Trim();
        iInfo.MAIL_1 = txtiMAIL_1.Text.Trim();
        iInfo.MAIL_2 = txtiMAIL_2.Text.Trim();
        iInfo.MAIL_3 = txtiMAIL_3.Text.Trim();
        iInfo.Maker_Time = DateTime.Now;
        iInfo.Checker_Time = DateTime.Now;

    }


    ///<summary>
    ///取得欄位預設資料內容
    ///<summary>
    //定義bind(),呼叫這個方法時 指定需要傳入 DropDownList這個class類別的資料
    protected void BindBankcode(DropDownList ddlqCode)
    {
        //設定一個空的table來接資料
        RES_BANKDB myDB = new RES_BANKDB();

        //透過getResBank()把取出的table放到Dt
        DataTable Dt = myDB.getResBank_distinct();

        //*Review*
        //把dt的內容放到ddlqCode相應的欄位裡面
        ddlqCode.DataSource = Dt;
        ddlqCode.DataValueField = "BANK_CODE";//user實際選的文字的值
        ddlqCode.DataTextField = "BANK_CODE"; //顯示在畫面上的文字
        ddlqCode.DataBind();

        //加入顯示預設字串在0的位置
        ddlqCode.Items.Insert(0, new ListItem("請選擇", ""));

    }
    #endregion

    #region Focus Function

    // 查詢 Focus 欄位
    protected void QueryFocus()
    {
        ddlqCode.Focus();
    }

    //新增 Focus 欄位
    protected void AddNewFocus()
    {
        txtiBANK_CODE.Focus();
    }

    //更新 Focus 欄位
    protected void ModifyFocus()
    {

    }
    #endregion

    #region 欄位檢查
    protected bool Query()
    {
        return true;
    }

    //按下編輯時,檢查欄位狀態
    protected bool View(string BANK_CODE)
    {
        //初始化db
        RES_BANKDB myDB = new RES_BANKDB();

        //檢查資料是否存在
        bool isExisting = myDB.chkRES_BNAKDB(BANK_CODE);

        if (isExisting)
        {
            RES_BANKInfo myInfo;

            //檢查資料存不存在
            myDB.LoadBANKCODE(BANK_CODE, out myInfo);

            //透過load()得到的myinfo把資料傳進fillmodfile()
            //將資料填入UI中
            FillModFieldValue(myInfo);

            return true;
        }
        else
        {
            //無該筆資料
            base.DoAlertinAjax(this, "SAVE", "該筆資料可能已被刪除，請重新查詢。");
            return false;
        }


    }

    //按下儲存時檢查欄位狀態
    protected bool Save(ModeType Mode)
    {
        //檢查銀行代碼有沒有填
        if (string.IsNullOrEmpty(txtiBANK_CODE.Text.Trim()))
        {
            base.DoAlertinAjax(this, "SAVE", "銀行代碼為必填，請確認!");
            txtiBANK_CODE.Focus();
            return false;
        }

        //檢查銀行名稱有沒有填
        if (string.IsNullOrEmpty(txtiBANK_NAME.Text.Trim()))
        {
            base.DoAlertinAjax(this, "SAVE", "銀行名稱為必填，請確認!");
            txtiBANK_NAME.Focus();
            return false;
        }
        return true;
    }
    #endregion

    #region 異動處理

    /// <summary>
    /// 資料新增
    /// </summary>
    protected bool Insert()
    {
        RES_BANKDB myDB = new RES_BANKDB();

        try
        {
            //取KEY值
            string BANK_CODE = txtiBANK_CODE.Text.Trim();

            //把透過getModField取得的資料指定到info中
            //RES_BANKInfo InputInfo = GetModFieldValue();

            RES_BANKInfo iInfo;
            GetModFieldValue(out iInfo);

            //檢查是否有在正式資料中
            bool IsExisting = myDB.chkRES_BNAKDB(BANK_CODE);

            if (IsExisting)
            {
                base.DoAlertinAjax(this, "SAVE", "該資料已存在，請重新查詢。");
                return false;
            }
            else
            {
                //把資料寫進db
                myDB.InsRES_BANK(iInfo);
                base.DoAlertinAjax(this, "SAVE", "儲存完成!");
                return true;
            }
        }
        catch (Exception ex)
        {
            base.DoAlertinAjax(this, "SAVE", "遇到例外情況" + ex.ToString());
            return false;
        }

    }

    /// <summary>
    /// 資料更新
    /// </summary>
    protected bool Update()
    {
        RES_BANKDB myDB = new RES_BANKDB();

        try
        {
            //取KEY值
            string BANK_CODE = txtiBANK_CODE.Text.Trim();

            //是否有在正式資料中
            bool IsExisting = myDB.chkRES_BNAKDB(BANK_CODE);

            //把透過getModField取得的資料指定到info中
            //RES_BANKInfo iInfo = GetModFieldValue();
            RES_BANKInfo iInfo;
            GetModFieldValue(out iInfo);

            //把資料寫進db
            if (IsExisting)
            {
                myDB.UpdRES_BANK(iInfo);
                base.DoAlertinAjax(this, "SAVE", "更新完成!");
            }
            else
            {
                base.DoAlertinAjax(this, "SAVE", "資料不存在!");
                return false;
            }
        }
        catch (Exception ex)
        {
            base.DoAlertinAjax(this, "SAVE", "遇到例外情況" + ex.ToString());
        }
        return true;
    }

    /// <summary>
    /// 資料刪除
    /// </summary>
    protected bool Delete()
    {
        RES_BANKDB myDB = new RES_BANKDB();

        try
        {
            //取KEY值
            string BANK_CODE = txtiBANK_CODE.Text.Trim();
            string BANK_NAME = txtiBANK_NAME.Text.Trim();

            //呼叫db
            bool DelDt = myDB.DelRES_BANK(BANK_CODE, BANK_NAME);

            base.DoAlertinAjax(this, "SAVE", "申請刪除完成!");
        }
        catch (Exception ex)
        {
            base.DoAlertinAjax(this, "SAVE", "遇到例外情況" + ex.ToString());
        }
        return true;
    }
    #endregion

}