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
    /// ��Excel����
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// ��ָ����Excell�ļ��л�ȡ���ݣ�Ҫ������ǹ淶�Ķ�ά��������excel�ļ�������Ϊ�ֶ���
        /// </summary>
        /// <param name="fileUrl">�������ϵ��ļ�·��</param>
        /// <param name="head">��ȡ��Щ�ֶε�ֵ(���磺���,����,��ѧʱ��),���ṩʱ����������</param>
        /// <returns>�������ݵ�DataSet</returns>
        public static DataSet ImportDataFromExcell(string fileUrl, string head = "")
        {
            //string connExcel = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileUrl + ";Extended Properties=Excel 8.0";//07֮ǰ�汾ʹ��
            string connExcel = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + fileUrl + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //�����ӿ��Բ���.xls��.xlsx�ļ� (֧��Excel2003 �� Excel2007 �������ַ���)
            using (OleDbConnection oleDbConnection = new OleDbConnection(connExcel))
            {
                oleDbConnection.Open();
                //���Excel��ÿҳ�ϵ���Ϣ
                DataTable dataTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //���Excel�е�ҳ��
                int pages = dataTable.Rows.Count;
                string tableName = "";
                string query = "";
                DataSet ds = new DataSet();
                OleDbDataAdapter oleAdapter = null;
                for (int i = 0; i < pages; i++)
                {
                    //���ÿһҳ������
                    tableName = dataTable.Rows[i][2].ToString().Trim();
                    string tn = tableName.Remove(tableName.Length - 1);
                    tableName = "[" + tableName.Replace("'", "") + "]";
                    if (string.IsNullOrEmpty(head))
                        query = "SELECT * FROM " + tableName;
                    else
                        query = "SELECT " + head + " FROM " + tableName;
                    oleAdapter = new OleDbDataAdapter(query, connExcel);
                    //��ʾÿҳ�ϵ�����
                    DataTable dt = new DataTable(tn);
                    oleAdapter.Fill(dt);
                    ds.Tables.Add(dt);
                }
                return ds;
            }
        }

        /// <summary>
        /// ��excel�е����ݵ��뵽DataTable��
        /// </summary>
        /// <param name="fileName">excel���ļ���������·����</param>
        /// <param name="sheetName">excel������sheet������</param>
        /// <param name="isFirstRowColumn">��һ���Ƿ���DataTable������</param>
        /// <returns>���ص�DataTable</returns>
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
                if (fileName.IndexOf(".xlsx") > 0) // 2007�汾
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003�汾
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //���û���ҵ�ָ����sheetName��Ӧ��sheet�����Ի�ȡ��һ��sheet
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
                    int cellCount = firstRow.LastCellNum; //һ�����һ��cell�ı�� ���ܵ�����

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

                    //���һ�еı��
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //û�����ݵ���Ĭ����null��������������

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //ͬ��û�����ݵĵ�Ԫ��Ĭ����null
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
