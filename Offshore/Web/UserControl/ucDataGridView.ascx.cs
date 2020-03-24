using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ucDataGridView : System.Web.UI.UserControl
{
    #region 變數定義

    //目前頁數
    public int _NowPage
    {
        get
        {
            return Convert.ToInt32(txtPage.Text);
        }
        set
        {
            txtPage.Text = value.ToString();
            hdPage.Value = value.ToString();
        }
    }

    //總頁數
    public int _TotalPage
    {
        get
        {
            return Convert.ToInt32(ViewState["_TotalPage"].ToString().Trim());
        }
        set
        {
            ViewState["_TotalPage"] = value.ToString();
        }
    }

    //開始筆數
    public int _StartRec
    {
        get
        {
            return Convert.ToInt32(ViewState["_StartRec"].ToString().Trim());
        }
        set
        {
            ViewState["_StartRec"] = value.ToString();
        }
    }

    //結束筆數
    public int _EndRec
    {
        get
        {
            return Convert.ToInt32(ViewState["_EndRec"].ToString().Trim());
        }
        set
        {
            ViewState["_EndRec"] = value.ToString();
        }
    }

    //總筆數
    public int _TotalCount
    {
        get
        {
            return Convert.ToInt32(ViewState["_TotalCount"].ToString().Trim());
        }
        set
        {
            ViewState["_TotalCount"] = value.ToString();
        }
    }

    //設定一頁幾筆
    public int _SetPageCount
    {
        get
        {
            return Convert.ToInt32(ddlRecordCount.SelectedValue);
        }
        set
        {
            ddlRecordCount.SelectedValue = value.ToString();
            hdRecordCount.Value = value.ToString();
        }
    }

    #endregion

    #region 自訂事件

    public delegate void ClickEventHandler(int _StartRec, int _EndRec);
    public event ClickEventHandler ChangePageClick;

    public delegate bool ClickingEventHandler();
    public event ClickingEventHandler ChangePageClicking;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region Button事件

    protected void First_Click(object sender, EventArgs e)
    {
        if (ChangePageClicking != null)
        {
            if (!ChangePageClicking())
                return;
        }

        ToFirst();
    }

    protected void Prev_Click(object sender, EventArgs e)
    {
        if (ChangePageClicking != null)
        {
            if (!ChangePageClicking())
                return;
        }

        if (ChangePageClick != null)
        {
            int StartRec;
            int EndRec;
            int PageRecord = Convert.ToInt32(ddlRecordCount.SelectedValue.ToString());

            CalRecCount("PREV", out StartRec, out EndRec);

            ChangePageClick(StartRec - 1, PageRecord);
        }
    }

    protected void Next_Click(object sender, EventArgs e)
    {
        if (ChangePageClicking != null)
        {
            if (!ChangePageClicking())
                return;
        }

        if (ChangePageClick != null)
        {
            int StartRec;
            int EndRec;
            int PageRecord = Convert.ToInt32(ddlRecordCount.SelectedValue.ToString());

            CalRecCount("NEXT", out StartRec, out EndRec);

            ChangePageClick(StartRec - 1, PageRecord);
        }
    }

    protected void Last_Click(object sender, EventArgs e)
    {
        if (ChangePageClicking != null)
        {
            if (!ChangePageClicking())
                return;
        }

        if (ChangePageClick != null)
        {
            int StartRec;
            int EndRec;
            int PageRecord = Convert.ToInt32(ddlRecordCount.SelectedValue.ToString());

            CalRecCount("LAST", out StartRec, out EndRec);

            ChangePageClick(StartRec - 1, PageRecord);
        }
    }

    #endregion

    protected void txtPage_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtPage.Text.Trim()))
        {
            if (ChangePageClicking != null)
            {
                if (!ChangePageClicking())
                {
                    txtPage.Text = hdPage.Value;
                    return;
                }
            }

            if (ChangePageClick != null)
            {
                int StartRec = 0;
                int EndRec = 0;
                int PageRecord = Convert.ToInt32(ddlRecordCount.SelectedValue.ToString());

                #region 計算總頁數

                int TotalPageCount = 0;

                if ((_TotalCount % _SetPageCount) > 0)
                    TotalPageCount = (_TotalCount / _SetPageCount) + 1;
                else
                    TotalPageCount = _TotalCount / _SetPageCount;

                _TotalPage = TotalPageCount;

                #endregion

                _NowPage = Convert.ToInt32(txtPage.Text.Trim());

                if (_NowPage > _TotalPage)
                    _NowPage = _TotalPage;
                else if (_NowPage <= 0)
                    _NowPage = 1;

                #region 計算起訖筆數

                EndRec = _NowPage * _SetPageCount;

                if (EndRec > _TotalCount)
                {
                    EndRec = _TotalCount;

                    StartRec = ((_NowPage - 1) * _SetPageCount) + 1;
                }
                else
                    StartRec = EndRec - _SetPageCount + 1;

                _StartRec = StartRec;
                _EndRec = EndRec;

                #endregion

                SetPageButton();

                ShowPgaeInfo();

                ChangePageClick(StartRec - 1, PageRecord);
            }
        }
    }

    protected void ddlRecordCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChangePageClicking != null)
        {
            if (!ChangePageClicking())
            {
                ddlRecordCount.SelectedValue = hdRecordCount.Value;
                return;
            }
        }

        if (ChangePageClick != null)
        {
            int StartRec = 0;
            int EndRec = 0;
            int PageRecord = Convert.ToInt32(ddlRecordCount.SelectedValue.ToString());

            #region 計算總頁數

            int TotalPageCount = 0;

            if ((_TotalCount % _SetPageCount) > 0)
                TotalPageCount = (_TotalCount / _SetPageCount) + 1;
            else
                TotalPageCount = _TotalCount / _SetPageCount;

            _TotalPage = TotalPageCount;

            #endregion

            _SetPageCount = Convert.ToInt32(ddlRecordCount.SelectedValue.ToString());

            _NowPage = (int)Math.Ceiling((double)_StartRec / (double)_SetPageCount);

            #region 計算起訖筆數

            EndRec = _NowPage * _SetPageCount;

            if (EndRec > _TotalCount)
            {
                EndRec = _TotalCount;

                StartRec = ((_NowPage - 1) * _SetPageCount) + 1;
            }
            else
                StartRec = EndRec - _SetPageCount + 1;

            _StartRec = StartRec;
            _EndRec = EndRec;

            #endregion

            SetPageButton();

            ShowPgaeInfo();

            ChangePageClick(StartRec - 1, PageRecord);
        }
    }

    #region 自訂function

    public void ToFirst()
    {
        if (ChangePageClick != null)
        {
            int StartRec;
            int EndRec;
            int PageRecord = Convert.ToInt32(ddlRecordCount.SelectedValue.ToString());

            CalRecCount("FIRST", out StartRec, out EndRec);

            ChangePageClick(0, PageRecord);
        }
    }

    private void CalRecCount(string _PageType, out int StartRec, out int EndRec)
    {
        StartRec = 0;
        EndRec = 0;

        #region 計算總頁數

        int TotalPageCount = 0;

        if ((_TotalCount % _SetPageCount) > 0)
            TotalPageCount = (_TotalCount / _SetPageCount) + 1;
        else
            TotalPageCount = _TotalCount / _SetPageCount;

        _TotalPage = TotalPageCount;

        #endregion

        #region 計算動作後的頁數

        switch (_PageType)
        {
            case "FIRST":
                _NowPage = 1;
                break;
            case "PREV":
                if (_NowPage - 1 <= 0)
                    _NowPage = 1;
                else
                    _NowPage = _NowPage - 1;
                break;
            case "NEXT":
                if (_NowPage + 1 > TotalPageCount)
                    _NowPage = TotalPageCount;
                else
                    _NowPage = _NowPage + 1;
                break;
            case "LAST":
                _NowPage = TotalPageCount;
                break;
        }

        #endregion

        #region 計算起訖筆數

        EndRec = _NowPage * _SetPageCount;

        if (EndRec > _TotalCount)
        {
            EndRec = _TotalCount;

            StartRec = ((_NowPage - 1) * _SetPageCount) + 1;
        }
        else
            StartRec = EndRec - _SetPageCount + 1;

        _StartRec = StartRec;
        _EndRec = EndRec;

        #endregion

        SetPageButton();

        ShowPgaeInfo();
    }

    private void SetPageButton()
    {
        if (_NowPage + 1 > _TotalPage)
        {
            First.Enabled = true;
            Prev.Enabled = true;
            Next.Enabled = false;
            Last.Enabled = false;
        }
        else if (_NowPage - 1 <= 0)
        {
            First.Enabled = false;
            Prev.Enabled = false;
            Next.Enabled = true;
            Last.Enabled = true;
        }
        else
        {
            First.Enabled = true;
            Prev.Enabled = true;
            Next.Enabled = true;
            Last.Enabled = true;
        }

        if (_TotalPage == 1)
        {
            First.Enabled = false;
            Prev.Enabled = false;
            Next.Enabled = false;
            Last.Enabled = false;
        }
    }

    private void ShowPgaeInfo()
    {
        ltPageText.Text = _TotalPage.ToString();

        ltRecordText.Text = _TotalCount.ToString();
    }

    #endregion
}
