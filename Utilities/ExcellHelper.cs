using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

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

        public static DataTable ExcelToDataTable(string url, string v1, bool v2)
        {
            throw new NotImplementedException();
        }
    }
}
