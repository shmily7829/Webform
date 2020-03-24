using System;
using System.Data;
using System.IO;
using System.Web.UI;
using NPOI.SS.UserModel;
using System.Collections.Generic;

public partial class Forms_Sys_Forms_CASA7100 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //流程：1.上傳Excel檔。2. ASP.net讀Excel資料，然後Insert into Table。3.刪除上傳的Excel檔，避免硬碟空間不夠。
    protected void btnUploadExcel_Click(object sender, EventArgs e)
    {
        //NPOI 2.0起，要讀取舊的 Excel檔，副檔名 .xls  ( Excel 2003（含）以前的版本) 用 HSSF;
        //要讀取新的 Excel檔，副檔名 .xlsx  ( Excel 2007（含）以前的版本) 用 XSSF

        string SaveFolder = Server.MapPath(@"ExcelTemp/");

        try
        {
            #region 上傳、存檔、匯入Excel檔
            string FileName = string.Empty;
            string SavePath = string.Empty;

            if (!System.IO.Directory.Exists(SaveFolder))
                System.IO.Directory.CreateDirectory(SaveFolder);

            if (fuExcel != null && fuExcel.HasFile)
            {
                FileName = Guid.NewGuid().ToString() + ".xlsx";

                SavePath = SaveFolder + FileName;

                //同檔名則覆蓋
                fuExcel.SaveAs(SavePath);
                //string extention = FileName.Split('.')[1];
            }
            #endregion


            DataTable ExcelTable = ImportXLS(SavePath);

            GridView1.DataSource = ExcelTable;
            GridView1.DataBind();
        }
        catch (Exception err)
        {
            string sMsg = err.Message.Replace("\r\n", "\n").Replace("\n", "\\n");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), this.ClientID, string.Format("alert('{0}');", sMsg), true);
        }
    }

    /// <summary>
    /// 匯入副檔名為.xls的Excel檔th
    /// </summary>
    private DataTable ImportXLS(string file)
    {
        DataTable dt = new DataTable();
        IWorkbook workbook;
        string fileExt = Path.GetExtension(file).ToLower();
        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
        {
            //XSSFWorkbook 適用XLSX格式，HSSFWorkbook 適用XLS格式
            if (fileExt == ".xlsx") { workbook = new NPOI.XSSF.UserModel.XSSFWorkbook(fs); } else if (fileExt == ".xls") { workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs); } else { workbook = null; }
            if (workbook == null) { return null; }
            ISheet sheet = workbook.GetSheetAt(0);

            //表頭  
            IRow header = sheet.GetRow(sheet.FirstRowNum);
            List<int> columns = new List<int>();
            for (int i = 0; i < header.LastCellNum; i++)
            {
                object obj = GetValueType(header.GetCell(i));
                if (obj == null || obj.ToString() == string.Empty)
                {
                    dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                }
                else
                    dt.Columns.Add(new DataColumn(obj.ToString()));
                columns.Add(i);
            }
            //資料  
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                DataRow dr = dt.NewRow();
                bool hasValue = false;
                foreach (int j in columns)
                {
                    dr[j] = GetValueType(sheet.GetRow(i).GetCell(j));
                    if (dr[j] != null && dr[j].ToString() != string.Empty)
                    {
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    dt.Rows.Add(dr);
                }
            }
        }
        return dt;
    }

    private static object GetValueType(ICell cell)
    {
        if (cell == null)
            return null;
        switch (cell.CellType)
        {
            case CellType.Blank: //BLANK:  
                return null;
            case CellType.Boolean: //BOOLEAN:  
                return cell.BooleanCellValue;
            case CellType.Numeric: //NUMERIC:  
                return cell.NumericCellValue;
            case CellType.String: //STRING:  
                return cell.StringCellValue;
            case CellType.Error: //ERROR:  
                return cell.ErrorCellValue;
            case CellType.Formula: //FORMULA:  
            default:
                return "=" + cell.CellFormula;
        }
    }

}

//public partial class Site1_Page1 : System.Web.UI.Page
//{
//    string gsFileServerDir = @"\\11.12.13.14\abc";    //FileServer UNC Path
//    protected void Page_Load(object sender, EventArgs e)
//    {

//    }
 
//    protected void btnUploadExcel_Click(object sender, EventArgs e)
//    {
//        //NPOI 2.0起，要讀取舊的 Excel檔，副檔名 .xls  ( Excel 2003（含）以前的版本) 用 HSSF;
//        //要讀取新的 Excel檔，副檔名 .xlsx  ( Excel 2007（含）以前的版本) 用 XSSF

//        try
//        {
//            #region 上傳、存檔、匯入Excel檔
//            string fileName = string.Empty;
//            if (!System.IO.Directory.Exists(gsFileServerDir))
//                System.IO.Directory.CreateDirectory(gsFileServerDir);

//            if (fuExcel != null && fuExcel.HasFile)
//            {
//                fileName = fuExcel.FileName;
//                string savePath = gsFileServerDir + fileName;
//                //同檔名則覆蓋
//                fuExcel.SaveAs(savePath);
//                string extention = fileName.Split('.')[1];
//                if (extention == "xlsx")
//                    ImportXLSX();
//                else
//                    ImportXLS();
//            }
//            #endregion

//        }
//        catch (Exception err)
//        {
//            string sMsg = err.Message.Replace("\r\n", "\n").Replace("\n", "\\n");
//            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), this.ClientID, string.Format("alert('{0}');", sMsg), true);
//        }
//    }

//    /// <summary>
//    /// 匯入副檔名為.xls的Excel檔
//    /// </summary>
//    private void ImportXLS()
//    {
//        HSSFWorkbook workbook = null;
//        HSSFSheet sheet = null;

//        try
//        {
//            #region 讀Excel檔，逐行寫入DataTable
//            workbook = new HSSFWorkbook(fuExcel.FileContent); //只能讀取 System.IO.Stream 
//            //FileContent 屬性會取得指向要上載之檔案的 Stream 物件。這個屬性可以用於存取檔案的內容 (做為位元組)。 
//            //   例如，您可以使用 FileContent 屬性傳回的 Stream 物件，將檔案的內容做為位元組進行讀取並將其以位元組陣列儲存。 
//            //FileContent 屬性，型別：System.IO.Stream 

//            sheet = (HSSFSheet)workbook.GetSheetAt(0);   //0表示：第一個 worksheet工作表
//            DataTable dt = new DataTable();

//            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);   //Excel 表頭列

//            for (int colIdx = 0; colIdx <= headerRow.LastCellNum; colIdx++) //表頭列，共有幾個 "欄位"?（取得最後一欄的數字） 
//            {
//                if (headerRow.GetCell(colIdx) != null)
//                    dt.Columns.Add(new DataColumn(headerRow.GetCell(colIdx).StringCellValue));
//                //欄位名有折行時，只取第一行的名稱做法是headerRow.GetCell(colIdx).StringCellValue.Replace("\n", ",").Split(',')[0]
//            }

//            //For迴圈的「啟始值」為1，表示不包含 Excel表頭列
//            for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)   //每一列做迴圈
//            {
//                HSSFRow exlRow = (HSSFRow)sheet.GetRow(rowIdx); //不包含 Excel表頭列的 "其他資料列"
//                DataRow newDataRow = dt.NewRow();

//                for (int colIdx = exlRow.FirstCellNum; colIdx <= exlRow.LastCellNum; colIdx++)   //每一個欄位做迴圈
//                {
//                    if (exlRow.GetCell(colIdx) != null)
//                        newDataRow[colIdx] = exlRow.GetCell(colIdx).ToString();    //每一個欄位，都加入同一列 DataRow
//                }
//                dt.Rows.Add(newDataRow);
//            }

//            GridView1.DataSource = dt;
//            GridView1.DataBind();
//            #endregion 讀Excel檔，逐行寫入DataTable
//        }
//        catch (Exception err)
//        {
//            throw err;
//        }
//        finally
//        {
//            //釋放 NPOI的資源
//            workbook = null;
//            sheet = null;
//        }
//    }

//    /// <summary>
//    /// 匯入副檔名為.xlsx的Excel檔
//    /// </summary>
//    private void ImportXLSX()
//    {
//        XSSFWorkbook workbook = null;
//        XSSFSheet sheet = null;

//        try
//        {
//            #region 讀Excel檔，逐行寫入DataTable
//            workbook = new XSSFWorkbook(fuExcel.FileContent); //只能讀取 System.IO.Stream 
//            //FileContent 屬性會取得指向要上載之檔案的 Stream 物件。這個屬性可以用於存取檔案的內容 (做為位元組)。 
//            //   例如，您可以使用 FileContent 屬性傳回的 Stream 物件，將檔案的內容做為位元組進行讀取並將其以位元組陣列儲存。 
//            //FileContent 屬性，型別：System.IO.Stream 

//            sheet = (XSSFSheet)workbook.GetSheetAt(0);   //0表示：第一個 worksheet工作表
//            DataTable dt = new DataTable();

//            XSSFRow headerRow = (XSSFRow)sheet.GetRow(0);   //Excel 表頭列


//            for (int colIdx = 0; colIdx <= headerRow.LastCellNum; colIdx++) //表頭列，共有幾個 "欄位"?（取得最後一欄的數字） 
//            {
//                if (headerRow.GetCell(colIdx) != null)
//                    dt.Columns.Add(new DataColumn(headerRow.GetCell(colIdx).StringCellValue));
//                //欄位名有折行時，只取第一行的名稱做法是headerRow.GetCell(colIdx).StringCellValue.Replace("\n", ",").Split(',')[0]
//            }

//            //For迴圈的「啟始值」為1，表示不包含 Excel表頭列
//            for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)   //每一列做迴圈
//            {
//                XSSFRow exlRow = (XSSFRow)sheet.GetRow(rowIdx); //不包含 Excel表頭列的 "其他資料列"
//                DataRow newDataRow = dt.NewRow();

//                for (int colIdx = exlRow.FirstCellNum; colIdx <= exlRow.LastCellNum; colIdx++)   //每一個欄位做迴圈
//                {
//                    if (exlRow.GetCell(colIdx) != null)
//                        newDataRow[colIdx] = exlRow.GetCell(colIdx).ToString();    //每一個欄位，都加入同一列 DataRow
//                }
//                dt.Rows.Add(newDataRow);
//            }

//            GridView1.DataSource = dt;
//            GridView1.DataBind();
//            #endregion 讀Excel檔，逐行寫入DataTable
//        }
//        catch (Exception err)
//        {

//            throw err;
//        }
//        finally
//        {
//            //釋放 NPOI的資源
//            workbook = null;
//            sheet = null;
//        }
//    }

//}