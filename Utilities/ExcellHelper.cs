using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

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

        public static DataTable ExcelToDataTable(string url, string v1, bool v2)
        {
            throw new NotImplementedException();
        }
    }
}
