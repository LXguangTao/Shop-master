using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Utilities
{
    /// <summary>
    /// 扩展代码之用
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 将DataTable对象转为html字符串
        /// </summary>
        /// <param name="dt">要转换的DataTable对象</param>
        /// <returns>html格式的table代码</returns>
        public static String ToHtml(this DataTable dt)
        {
            StringBuilder sb = new StringBuilder("<table><thead>");
            foreach (DataColumn c in dt.Columns)
            {//Columns就是图中的DataColumnCollection
                sb.AppendFormat("<th>{0}</th>", c.ColumnName);
            }
            sb.Append("</thead><tbody>");
            foreach (DataRow r in dt.Rows)
            {//Rows就是图中的DataRowCollection
                sb.Append("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.AppendFormat("<td>{0}</td>", r[i]);
                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody></table>");
            return sb.ToString();
        }
        /// <summary>
        /// 为List集合扩展1个获取json字符串的方法，此方法可用于LayUI的数据表格组件
        /// </summary>
        /// <typeparam name="T">集合的类型</typeparam>
        /// <param name="list">数据集合</param>
        /// <param name="count">满足条件的数据的条数</param>
        /// <param name="dateTimeFormat">统一的日期时间格式</param>
        /// <returns>layui数据表格组件可用的json格式字符串</returns>
        public static String ToJsonForLayUITable<T>(this IList<T> list, int count, String dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            StringBuilder strJson = new StringBuilder();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            settings.DateFormatString = dateTimeFormat;
            settings.MaxDepth = 1; //设置序列化的最大层数  
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;
            foreach (var item in list)
            {
                strJson.Append(JsonConvert.SerializeObject(item, settings) + ",");
            }
            return "{\"code\":0,\"msg\":\"\",\"count\":" + count + ",\"data\":[" + strJson.ToString().TrimEnd(',') + "]}";
        }
        /// <summary>
        /// 系统中统一禁用DataSet自己的WriteXml，使用下边定义的格式，在客户端便于解析
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string ToXml(this DataSet ds)
        {
            StringBuilder sb = new StringBuilder("<DataSet>  <Tables>");
            foreach (DataTable dt in ds.Tables)
                sb.Append(dt.ToXml());
            sb.Append("</Tables></DataSet>");
            return sb.ToString();
        }
        /// <summary>
        /// 系统中统一禁用DataTable自己的WriteXml，使用下边定义的格式，在客户端便于解析。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToXml(this DataTable dt)
        {
            StringBuilder sb = new StringBuilder(String.Format("<DataTable Name='{0}'>  <Columns>", dt.TableName));
            int colCount = dt.Columns.Count, rowCount = dt.Rows.Count;
            for (int i = 0; i < colCount; i++)
            {
                sb.Append("<").Append(dt.Columns[i].ColumnName).Append(" DataType='").Append(dt.Columns[i].DataType).Append("'/>");
            }
            sb.Append("  </Columns> <Rows>");

            for (int j = 0; j < rowCount; j++)
            {
                sb.Append("<Row ");
                for (int i = 0; i < colCount; i++)
                    sb.Append(dt.Columns[i].ColumnName + "='").Append(dt.Rows[j][i].ToString() + "' ");
                sb.Append("/>");
            }
            sb.Append(" </Rows> </DataTable>");
            return sb.ToString();
        }

        static string GetJsonStr( string obj, DataContractJsonSerializer ser,System.IO.MemoryStream ms)
        {
            ms.Flush();         
            ser.WriteObject(ms, obj);
            return System.Text.Encoding.UTF8.GetString(ms.ToArray());           
        }
        /// <summary>
        /// 将DataTable转为字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(this DataTable dt)
        {       
            StringBuilder sb = new StringBuilder("[");
            int colCount = dt.Columns.Count, rowCount = dt.Rows.Count;
            for (int j = 0; j < rowCount; j++)
            {
                if (j == 0)
                    sb.Append("{");
                else
                    sb.Append(",{");

                for (int i = 0; i < colCount; i++)
                {
                    if (i == 0)
                        sb.Append(string.Format(@"""{0}"":{1}", dt.Columns[i].ColumnName, IoHelper.SerializeJson(dt.Rows[j][i].ToString())));
                    else
                        sb.Append(string.Format(@",""{0}"":{1}", dt.Columns[i].ColumnName, IoHelper.SerializeJson(dt.Rows[j][i].ToString())));
                }
                sb.Append("}");
            }
            sb.Append("]");           
            return sb.ToString();
        }
        /// <summary>
        /// 返回可以直接用于EasyUI的分页数据
        /// </summary>
        /// <param name="dt">DataTable 对象</param>
        /// <param name="rowCount">本次分页查询条件符合记录的总数</param>
        /// <returns></returns>
        public static string ToJsonForEasyUI(this DataTable dt,int rowCount)
        {
            return "{" + string.Format(@"""total"":{0},""rows"":{1}", rowCount, dt.ToJson()) + "}";
        }


        /// <summary>
        /// 将字符串转换为字节数组
        /// </summary>
        /// <remarks>
        /// 在WebService服务端客户端的数据传输过程中，调用加密方法或者字符串中含有特殊符号，可能造成数据传输或解析错误
        /// 调用此法使用字节数组传输，避免此类问题。
        /// </remarks>
        /// <param name="str">源字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] ToPDSBytes(this string str)
        {
            System.Text.UTF8Encoding u = new System.Text.UTF8Encoding();
            return u.GetBytes(str);
        }
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <remarks>
        /// 这是ToPDSBytes方法的反向方法
        /// </remarks>
        /// <seealso cref="ToPDSBytes()"/>
        /// <param name="bs">源字节数组</param>
        /// <returns>字符串</returns>
        public static string ToPDSString(this byte[] bs)
        {
            System.Text.UTF8Encoding u = new System.Text.UTF8Encoding();
            return u.GetString(bs);
        }
    }
}
