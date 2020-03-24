using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class ucJQCalendar : System.Web.UI.UserControl
{
    [Browsable(true)]
    public string Text
    {
        set
        {
            DateTime dt2 = new DateTime();
            if (DateTime.TryParse(value, out dt2))
            {
                txtCalendar.Text = dt2.ToString("yyyy\\/MM\\/dd");//強制轉成yyyy/MM/DD格式 避免電腦時間不一樣(註:要加2個反斜線 才會生效)
            }
            else
            {
                txtCalendar.Text = "";
            }
        }
        get
        {
            return txtCalendar.Text.Trim();
        }
    }

    [Browsable(true)]
    public bool Enabled
    {
        get
        {
            return txtCalendar.Enabled;
        }
        set
        {
            txtCalendar.Enabled = value;
        }
    }

    private string _PlaceHolderText;
    [Browsable(true), Description("PlaceHolder Text"), DefaultValue("")]
    public string PlaceHolderText
    {
        get
        {
            return _PlaceHolderText;
        }
        set
        {
            _PlaceHolderText = value;
        }
    }

    public enum CultureEnum
    {
        en_US,
        zh_TW
    }
    private CultureEnum _CultureName;
    [Browsable( true ), Description("語系"), DefaultValue(CultureEnum.en_US)]
    public CultureEnum CultureName
    {
        get
        {
            return _CultureName;
        }
        set
        {
            _CultureName = value;

            switch (value)
            {
                case CultureEnum.en_US:
                    hdCultureName.Value = "en-US";
                    break;

                case CultureEnum.zh_TW:
                    hdCultureName.Value = "zh-CN";
                    break;

                default:
                    hdCultureName.Value = "en-US";
                    break;
            }
        }
    }

    private string _DateFormat;
    [Browsable(true), Description("日期格式"), DefaultValue("yyyy/mm/dd")]
    public string DateFormat
    {
        get
        {
            return _DateFormat;
        }
        set
        {
            _DateFormat = value;
            hdDateFormat.Value = value;
        }
    }

    [Browsable(true), Description("顯示提示格式"), DefaultValue(false)]
    public bool ShowPlaceholder
    {
        get;
        set;
    }

    [Browsable(true), Description("Div CssClass"), DefaultValue(""), CssClassProperty]
    public string DivCssClass
    {
        get
        {
            return (string)(ViewState[this.ClientID+"_DivCssClass"] ?? "");
        }
        set
        {
            ViewState[this.ClientID+"_DivCssClass"] = value;
        }
    }

    [Browsable(true), Description("Text CssClass"), DefaultValue(""), CssClassProperty]
    public string TextCssClass
    {
        get
        {
            return (string)(ViewState[this.ClientID + "_TextCssClass"] ?? "");
        }
        set
        {
            ViewState[this.ClientID + "_TextCssClass"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ShowPlaceholder)
            {
                if (string.IsNullOrEmpty(_PlaceHolderText))
                {
                    if (!string.IsNullOrEmpty(_DateFormat))
                        txtCalendar.Attributes.Add("placeholder", _DateFormat);
                    else
                        txtCalendar.Attributes.Add("placeholder", "yyyy/mm/dd");
                }
                else
                {
                    txtCalendar.Attributes.Add("placeholder", _PlaceHolderText);
                }
            }
        }

        #region 若有UpdatePanel則需透過ScriptManager註冊JS

        StringBuilder sbScript = new StringBuilder();

        sbScript.Append(@"
                $(function () {
                    $('#" + txtCalendar.ClientID+ @"').datepicker({
                        language: $('#" + hdCultureName.ClientID + @"').val(),
                        format: $('#" + hdDateFormat.ClientID + @"').val(),
                        autoHide: true
                    });

                    $('#"+ txtCalendar.ClientID + @"').inputmask('datetime', {
                        inputFormat: $('#" + hdDateFormat.ClientID + @"').val(),
                        outputFormat: $('#" + hdDateFormat.ClientID + @"').val(),
                        inputEventOnly: true,
                        showMaskOnFocus: false,
                        showMaskOnHover: false,
                        clearIncomplete: true
                    });
                });
        ");

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), txtCalendar.ClientID, sbScript.ToString(),true);

        #endregion
    }

    /// <summary>
    /// 填入日期
    /// </summary>
    /// <param name="theDate">日期(DateTime型態)</param>
    public void SetDate(DateTime theDate)
    {
        txtCalendar.Text = theDate.ToString("yyyy\\/MM\\/dd");
    }

    /// <summary>
    /// 添加onBlur屬性
    /// </summary>
    /// <param name="sJavaScrpit">要觸發的JS</param>
    public void Addonblur(string sJavaScrpit)
    {
        txtCalendar.Attributes.Add("onblur", sJavaScrpit);
    }
}