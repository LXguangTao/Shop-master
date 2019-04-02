using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace Utilities
{
    /// <summary>
    /// 简化Excel操作
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 从指定的Excell文件中获取数据，要求必须是规范的二维表数据且excel文件的首行为字段名
        /// </summary>
        /// <param name="fileUrl">服务器上的文件路径</param>
        /// <param name="head">获取哪些字段的值(例如：编号,姓名,入学时间),不提供时返回所有列</param>
        /// <returns>承载数据的DataSet</returns>
        public static DataSet ImportDataFromExcell(string fileUrl, string head = "")
        {
            //string connExcel = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileUrl + ";Extended Properties=Excel 8.0";//07之前版本使用
            string connExcel = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + fileUrl + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
            using (OleDbConnection oleDbConnection = new OleDbConnection(connExcel))
            {
                oleDbConnection.Open();
                //获得Excel的每页上的信息
                DataTable dataTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //获得Excel中的页数
                int pages = dataTable.Rows.Count;
                string tableName = "";
                string query = "";
                DataSet ds = new DataSet();
                OleDbDataAdapter oleAdapter = null;
                for (int i = 0; i < pages; i++)
                {
                    //获得每一页的名称
                    tableName = dataTable.Rows[i][2].ToString().Trim();
                    string tn = tableName.Remove(tableName.Length - 1);
                    tableName = "[" + tableName.Replace("'", "") + "]";
                    if (string.IsNullOrEmpty(head))
                        query = "SELECT * FROM " + tableName;
                    else
                        query = "SELECT " + head + " FROM " + tableName;
                    oleAdapter = new OleDbDataAdapter(query, connExcel);
                    //表示每页上的内容
                    DataTable dt = new DataTable(tn);
                    oleAdapter.Fill(dt);
                    ds.Tables.Add(dt);
                }
                return ds;
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="fileName">excel的文件名（完整路径）</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            FileStream fs = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }
}
