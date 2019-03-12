using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Data;

namespace Utilities
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 邮箱地址正则表达式
        /// </summary>
        public static Regex RegexEamil = new Regex(@"^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$", RegexOptions.IgnoreCase);
        /// <summary>
        /// 手机号正则表达式
        /// </summary>
        public static Regex RegexMobilePhone = new Regex("^[1][0-9]{10}$");
        /// <summary>
        /// 固话号正则表达式
        /// </summary>
        public static Regex RegexPhone = new Regex(@"^(\d{3,4}-?)?\d{7,8}$");
        /// <summary>
        /// IP正则表达式
        /// </summary>
        public static Regex RegexIP = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        /// <summary>
        /// 日期正则表达式 yyyy-MM-dd,不是很准确
        /// </summary>
        public static Regex RegexDate = new Regex(@"[12](\d{3})-[01]?\d-[0123]?\d");
        /// <summary>
        /// 数值(包括整数和小数)正则表达式
        /// </summary>
        public static Regex RegexNumeric = new Regex(@"^[-]?[0-9]+(\.[0-9]+)?$");
        /// <summary>
        /// 邮政编码正则表达式
        /// </summary>
        public static Regex RegexZipcoder = new Regex(@"^\d{6}$");

        /// <summary>
        /// 将字符串数组拼接成用单引号括起来的字符串。
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string ToSqlIdString(this String[] ids)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ids.Length; i++)
            {
                if (i == 0)
                    sb.Append("'").Append(ids[i]).Append("'");
                else
                    sb.Append(",'").Append(ids[i]).Append("'");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 处理答案
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToChoiceAnswer(this string s)
        {
            string da = "ABCDEFGHIGKLMN";
            List<char> nd = new List<char>();
            foreach (var c in s)
            {
                var cx = c.ToString().ToUpper();
                if (da.Contains(cx))
                    nd.Add(cx[0]);
            }
            nd.Sort();
            var ans = "";
            foreach (var c in nd)
                ans += c;
            return ans.ToString();
        }

        static Dictionary<int, string> dic = new Dictionary<int, string>();
        static Regex OutWebURL = new Regex("://[\\s\\S]*?/", RegexOptions.IgnoreCase);
        static StringHelper()
        {
            dic.Add(0, "零");
            dic.Add(1, "一");
            dic.Add(2, "二");
            dic.Add(3, "三");
            dic.Add(4, "四");
            dic.Add(5, "五");
            dic.Add(6, "六");
            dic.Add(7, "七");
            dic.Add(8, "八");
            dic.Add(9, "九");
        }

        /// <summary>
        /// 将 Stream 转化成 string
        /// </summary>
        /// <param name="s">Stream流</param>
        /// <returns>string</returns>
        public static string ConvertStreamToString(Stream s)
        {
            string strResult = "";
            StreamReader sr = new StreamReader(s, Encoding.UTF8);

            Char[] read = new Char[256];
            // Read 256 charcters at a time.    
            int count = sr.Read(read, 0, 256);
            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console.
                string str = new String(read, 0, count);
                strResult += str;
                count = sr.Read(read, 0, 256);
            }
            // 释放资源
            sr.Close();
            return strResult;
        }

        /// <summary>
        /// 输出由同一字符组成的指定长度的字符串
        /// </summary>
        /// <param name="Char">输出字符，如：A</param>
        /// <param name="i">指定长度</param>
        /// <returns></returns>
        public static string Strings(char Char, int i)
        {
            string strResult = null;

            for (int j = 0; j < i; j++)
            {
                strResult += Char;
            }
            return strResult;
        }

        /// <summary>
        /// 返回字符串的字节长度
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>字节个数</returns>
        public static int GetLen(string str)
        {
            int intResult = 0;
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            byte[] bytes = gb2312.GetBytes(str);
            intResult = bytes.Length;
            return intResult;
        }
        #region 随机数
        /// <summary>
        /// 获取指定长度的纯数字随机数字串
        /// </summary>
        /// <param name="intLong">数字串长度</param>
        /// <returns>字符串</returns>
        public static string GetRandomNum(int intLong)
        {
            string strResult = "";

            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < intLong; i++)
            {
                strResult = strResult + r.Next(10);
            }

            return strResult;
        }

        /// <summary>
        /// 获取一个由26个小写字母组成的指定长度的随即字符串
        /// </summary>
        /// <param name="intLong">指定长度</param>
        /// <returns></returns>
        public static string GetRandomLetters(int intLong)
        {
            string strResult = "";
            string[] array = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            Random r = new Random();

            for (int i = 0; i < intLong; i++)
            {
                strResult += array[r.Next(26)];
            }

            return strResult;
        }

        /// <summary>
        /// 获取一个由数字和26个小写字母组成的指定长度的随即字符串
        /// </summary>
        /// <param name="intLong">指定长度</param>
        /// <returns></returns>
        public static string GetRandomNumAndLetters(int intLong)
        {
            string strResult = "";
            string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            Random r = new Random();

            for (int i = 0; i < intLong; i++)
            {
                strResult += array[r.Next(36)];
            }

            return strResult;
        }
        #endregion

        #region 正则表达式的使用

        /// <summary>
        /// 判断字符串是否为有效的邮件地址
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^.+\@(\[?)[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,3}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 判断字符串是否为有效的URL地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsValidURL(string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// 判断字符串是否为Int类型的
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsValidInt(string val)
        {
            return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
        }

        /// <summary>
        /// 检测字符串是否全为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNum(string str)
        {
            bool blResult = true;//默认状态下是数字

            if (str == "")
                blResult = false;
            else
            {
                foreach (char Char in str)
                {
                    if (!char.IsNumber(Char))
                    {
                        blResult = false;
                        break;
                    }
                }
                if (blResult)
                {
                    if (int.Parse(str) == 0)
                        blResult = false;
                }
            }
            return blResult;
        }
        /// <summary>
        /// 检测字符串是否全为数字型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(string str)
        {
            bool blResult = true;//默认状态下是数字

            if (str == "")
                blResult = false;
            else
            {
                foreach (char Char in str)
                {
                    if (!char.IsNumber(Char) && Char.ToString() != "-")
                    {
                        blResult = false;
                        break;
                    }
                }
            }
            return blResult;
        }
        /// <summary>
        /// 判断输入的字符串是否完全匹配正则
        /// </summary>
        /// <param name="RegexExpression">正则表达式</param>
        /// <param name="str">待判断的字符串</param>
        /// <returns></returns>
        public static bool IsValiable(string RegexExpression, string str)
        {
            bool blResult = false;

            Regex rep = new Regex(RegexExpression, RegexOptions.IgnoreCase);

            //blResult = rep.IsMatch(str);
            Match mc = rep.Match(str);

            if (mc.Success)
            {
                if (mc.Value == str) blResult = true;
            }
            return blResult;
        }

        /// <summary>
        /// 转换代码中的URL路径为绝对URL路径
        /// </summary>
        /// <param name="sourceString">源代码</param>
        /// <param name="replaceURL">替换要添加的URL</param>
        /// <returns>string</returns>
        public static string ConvertURL(string sourceString, string replaceURL)
        {
            Regex rep = new Regex(" (src|href|background|value)=('|\"|)([^('|\"|)http://].*?)('|\"| |>)");
            sourceString = rep.Replace(sourceString, " $1=$2" + replaceURL + "$3$4");
            return sourceString;
        }

        /// <summary>
        /// 获取代码中所有图片的以HTTP开头的URL地址
        /// </summary>
        /// <param name="sourceString">代码内容</param>
        /// <returns>ArrayList</returns>
        public static List<string> GetImgFileUrl(string sourceString)
        {
            List<string> imgArray = new List<string>();
            Regex r = new Regex("<IMG(.*?)src=('|\"|)(http://.*?)('|\"| |>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!imgArray.Contains(mc[i].Result("$3")))
                {
                    imgArray.Add(mc[i].Result("$3"));
                }
            }
            return imgArray;
        }

        /// <summary>
        /// 获取代码中所有文件的以HTTP开头的URL地址
        /// </summary>
        /// <param name="sourceString">代码内容</param>
        /// <returns>ArrayList</returns>
        public static Dictionary<int, string> GetFileUrlPath(string sourceString)
        {
            Dictionary<int, string> url = new Dictionary<int, string>();
            Regex r = new Regex(" (src|href|background|value)=('|\"|)(http://.*?)('|\"| |>)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!url.ContainsValue(mc[i].Result("$3")))
                {
                    url.Add(i, mc[i].Result("$3"));
                }
            }
            return url;
        }

        /// <summary>
        /// 获取一条SQL语句中的所参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static List<string> SqlParame(string sql)
        {
            List<string> list = new List<string>();
            Regex r = new Regex(@"@(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }
            return list;
        }

        /// <summary>
        /// 获取一条SQL语句中的所参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static List<string> OracleParame(string sql)
        {
            List<string> list = new List<string>();
            Regex r = new Regex(@":(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }

            return list;
        }

        /// <summary>
        /// 将HTML代码转化成纯文本
        /// </summary>
        /// <param name="sourceHTML">HTML代码</param>
        /// <returns></returns>
        public static string ConvertText(string sourceHTML)
        {
            string strResult = "";
            Regex r = new Regex("<(.*?)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sourceHTML);

            if (mc.Count == 0)
            {
                strResult = sourceHTML;
            }
            else
            {
                strResult = sourceHTML;
                for (int i = 0; i < mc.Count; i++)
                {
                    strResult = strResult.Replace(mc[i].ToString(), "");
                }
            }
            return strResult.Replace("&nbsp;", "");
        }
        #endregion

        /// <summary>
        /// 封装XML数据串
        /// </summary>
        /// <param name="str"></param>
        /// <returns>string</returns>
        public static string ConvertXmlString(string str)
        {
            return "<![CDATA[" + str + "]]>";
        }

        /// <summary>
        /// 对字符串进行 HTML 编码操作，用于无法使用Server类的场景。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        /// 对 HTML 字符串进行解码操作，用于无法使用Server类的场景。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string HtmlDecode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        /// <summary>
        /// 对脚本程序进行处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertScript(string str)
        {
            string strResult = "";
            if (str != "")
            {
                StringReader sr = new StringReader(str);
                string rl;
                do
                {
                    strResult += sr.ReadLine();
                } while ((rl = sr.ReadLine()) != null);
            }

            strResult = strResult.Replace("\"", "&quot;");
            return strResult;
        }

        /// <summary>
        /// 过滤屏蔽的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string FilterString(string source, string[] filter)
        {
            StringBuilder sb = new StringBuilder(source);

            foreach (String str in filter)
            {
                sb.Replace(str, "*");
            }

            return sb.ToString();
        }
        /// <summary>
        /// 随机产生一个Color字符串。例如 ffee99
        /// </summary>
        /// <returns></returns>
        public static string GetColorString()
        {
            Guid uid = Guid.NewGuid();
            string sid = uid.ToString().Replace("-", "");
            Random r = new Random(DateTime.Now.Millisecond);
            return uid.ToString().Substring(r.Next(24), 6);
        }
        /// <summary>
        /// 对source，按照“＃”进行分割，获取第num个字符串的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetSplitString(string source, int num)
        {
            return GetSplitString(source, "#", num);
        }
        /// <summary>
        /// 对source，按照指定的字符串以split进行分割，获取第num个字符串的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="split"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetSplitString(string source, string split, int num)
        {
            try
            {
                string[] strs = source.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
                return strs[num];
            }
            catch
            {
                return "Empty";
            }
        }
        /// <summary>
        /// 获取一个唯一值
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueString()
        {
            string str = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + (new Random()).Next();
            return str;
        }


        /// <summary>
        /// 格式化占用空间大小的输出
        /// </summary>
        /// <param name="size">大小</param>
        /// <returns>返回 String</returns>
        public static string FormatNUM(long size)
        {
            decimal NUM;
            string strResult;

            if (size > 1073741824)
            {
                NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1073741824));
                strResult = NUM.ToString("N") + " G";
            }
            else if (size > 1048576)
            {
                NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1048576));
                strResult = NUM.ToString("N") + " M";
            }
            else if (size > 1024)
            {
                NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1024));
                strResult = NUM.ToString("N") + " KB";
            }
            else
            {
                strResult = size + " 字节";
            }

            return strResult;
        }
        /// <summary>
        /// 获取指定汉字拼音的首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFirstLetter(string str)
        {
            int i = 0;
            ushort key = 0;
            string strResult = string.Empty;

            //创建两个不同的encoding对象
            Encoding unicode = Encoding.Unicode;
            //创建GBK码对象
            Encoding gbk = Encoding.GetEncoding(936);
            //将unicode字符串转换为字节
            byte[] unicodeBytes = unicode.GetBytes(str);
            //再转化为GBK码
            byte[] gbkBytes = Encoding.Convert(unicode, gbk, unicodeBytes);
            while (i < gbkBytes.Length)
            {
                //如果为数字\字母\其他ASCII符号
                if (gbkBytes[i] <= 127)
                {
                    strResult = strResult + (char)gbkBytes[i];
                    i++;
                }
                #region 否则生成汉字拼音简码,取拼音首字母
                else
                {

                    key = (ushort)(gbkBytes[i] * 256 + gbkBytes[i + 1]);
                    if (key >= '\uB0A1' && key <= '\uB0C4')
                    {
                        strResult = strResult + "A";
                    }
                    else if (key >= '\uB0C5' && key <= '\uB2C0')
                    {
                        strResult = strResult + "B";
                    }
                    else if (key >= '\uB2C1' && key <= '\uB4ED')
                    {
                        strResult = strResult + "C";
                    }
                    else if (key >= '\uB4EE' && key <= '\uB6E9')
                    {
                        strResult = strResult + "D";
                    }
                    else if (key >= '\uB6EA' && key <= '\uB7A1')
                    {
                        strResult = strResult + "E";
                    }
                    else if (key >= '\uB7A2' && key <= '\uB8C0')
                    {
                        strResult = strResult + "F";
                    }
                    else if (key >= '\uB8C1' && key <= '\uB9FD')
                    {
                        strResult = strResult + "G";
                    }
                    else if (key >= '\uB9FE' && key <= '\uBBF6')
                    {
                        strResult = strResult + "H";
                    }
                    else if (key >= '\uBBF7' && key <= '\uBFA5')
                    {
                        strResult = strResult + "J";
                    }
                    else if (key >= '\uBFA6' && key <= '\uC0AB')
                    {
                        strResult = strResult + "K";
                    }
                    else if (key >= '\uC0AC' && key <= '\uC2E7')
                    {
                        strResult = strResult + "L";
                    }
                    else if (key >= '\uC2E8' && key <= '\uC4C2')
                    {
                        strResult = strResult + "M";
                    }
                    else if (key >= '\uC4C3' && key <= '\uC5B5')
                    {
                        strResult = strResult + "N";
                    }
                    else if (key >= '\uC5B6' && key <= '\uC5BD')
                    {
                        strResult = strResult + "O";
                    }
                    else if (key >= '\uC5BE' && key <= '\uC6D9')
                    {
                        strResult = strResult + "P";
                    }
                    else if (key >= '\uC6DA' && key <= '\uC8BA')
                    {
                        strResult = strResult + "Q";
                    }
                    else if (key >= '\uC8BB' && key <= '\uC8F5')
                    {
                        strResult = strResult + "R";
                    }
                    else if (key >= '\uC8F6' && key <= '\uCBF9')
                    {
                        strResult = strResult + "S";
                    }
                    else if (key >= '\uCBFA' && key <= '\uCDD9')
                    {
                        strResult = strResult + "T";
                    }
                    else if (key >= '\uCDDA' && key <= '\uCEF3')
                    {
                        strResult = strResult + "W";
                    }
                    else if (key >= '\uCEF4' && key <= '\uD188')
                    {
                        strResult = strResult + "X";
                    }
                    else if (key >= '\uD1B9' && key <= '\uD4D0')
                    {
                        strResult = strResult + "Y";
                    }
                    else if (key >= '\uD4D1' && key <= '\uD7F9')
                    {
                        strResult = strResult + "Z";
                    }
                    else
                    {
                        strResult = strResult + "?";
                    }
                    i = i + 2;
                }
                #endregion
            }//end while

            return strResult;
        }
        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="filePath">包含有文件名的字符串</param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            if (filePath.IndexOf("\\") > 0)
            {
                return filePath.Substring(filePath.LastIndexOf("\\") + 1);
            }
            else
                return filePath.Substring(filePath.LastIndexOf("/") + 1);
        }
        /// <summary>
        /// 获取fileName的扩展名
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static string GetExtName(string fileName)
        {
            string fileExt = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
            if (fileExt == "jpeg")
                fileExt = "jpg";
            return fileExt;
        }
        /// <summary>
        /// 获取指定data中前len个字符构成的字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetSubstring(string data, int len)
        {
            if (data.Length > len)
                return data.Substring(0, len) + "...";
            else
                return data;
        }
        /// <summary>
        /// 获取指定data的指定类型的字符编号
        /// </summary>
        /// <param name="data">从1到999</param>
        /// <param name="type">从1到8</param>
        /// <returns></returns>
        public static string ToNum(this int data, int type)
        {
            switch (type)
            {
                case 1://汉字显示
                    return string.Format("{0}:", GetCh(data));
                case 2:
                    return string.Format("{0}、", data);
                case 3:
                    return string.Format("({0})、", data);
                case 4:
                    return string.Format("({0})、", GetZM(data, true));
                case 5:
                    return string.Format("({0})、", GetZM(data, false));
                case 6:
                    return "●";
                case 7:
                    return "★";
                default:
                    return "◆";
            }
        }
        static string GetCh(int data)
        {
            int b = data / 100;
            int s = (data - 100 * b) / 10;
            int g = data % 10;
            string ret = dic[b] + dic[s] + dic[g];
            if (ret[0] == '零')
                ret = ret.Substring(1);
            if (ret[0] == '零')
                ret = ret.Substring(1);
            return ret;
        }
        static string GetZM(int data, bool isBig)
        {
            int s = data / 26;
            int y = data % 26;
            string ret = "";
            char A = 'a';
            if (isBig)
                A = 'A';
            if (s > 0)
                ret = ((char)(A + s)).ToString();
            if (y > 0)
                ret += ((char)(A + y)).ToString();
            if (y == 0)
                ret += "Z";
            return ret;
        }

        #region 安全性

        /// <summary>
        /// 对传递的参数字符串进行处理，防止注入式攻击
        /// </summary>
        /// <param name="str">传递的参数字符串</param>
        /// <returns>String</returns>
        public static string ConvertSql(string str)
        {
            str = str.Trim();
            str = str.Replace("'", "''");
            str = str.Replace(";--", "");
            str = str.Replace("=", "");
            str = str.Replace(" or ", "");
            str = str.Replace(" and ", "");
            return str;
        }
        /// <summary>
        /// 移除不安全代码,防止页面攻击
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSafeCode(string str)
        {
            StringBuilder builder = new StringBuilder(str);
            Regex regex = new Regex(@"<script[\s\S]*?>[\s\S]*?</script>", RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(builder.ToString()))
            {
                builder.Replace(match.Value, "");
            }
            regex = new Regex(@"<script[\s\S]*?/>", RegexOptions.IgnoreCase);
            foreach (Match match2 in regex.Matches(builder.ToString()))
            {
                builder.Replace(match2.Value, "");
            }
            regex = new Regex(@"<iframe[\s\S]*?/>", RegexOptions.IgnoreCase);
            foreach (Match match3 in regex.Matches(builder.ToString()))
            {
                builder.Replace(match3.Value, "");
            }
            regex = new Regex(@"<iframe[\s\S]*?>[\s\S]*?</iframe>", RegexOptions.IgnoreCase);
            foreach (Match match4 in regex.Matches(builder.ToString()))
            {
                builder.Replace(match4.Value, "");
            }
            return builder.ToString();
        }
        #endregion

        #region Html源代码处理
        /// <summary>
        /// 移除HTML标记
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtmlCode(string str)
        {
            StringBuilder builder = new StringBuilder(str);
            Regex regex = new Regex(@"<[\s\S]*?>", RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(builder.ToString()))
            {
                builder.Replace(match.Value, "");
            }
            builder.Replace("&nbsp;", " ");
            return builder.ToString().Trim();
        }

        /// <summary>
        /// 移除HTML代码的某些标记
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtmlCode(string str, params String[] labels)
        {
            StringBuilder builder = new StringBuilder(str);

            foreach (String tempstr in labels)
            {
                Regex regex = new Regex(@"<\/{0,1}" + tempstr + @"[\s\S]*?>", RegexOptions.IgnoreCase);
                foreach (Match match in regex.Matches(builder.ToString()))
                {
                    builder.Replace(match.Value, "");
                }
            }

            builder.Replace("&nbsp;", " ");
            return builder.ToString().Trim();
        }

        /// <summary>
        /// 移除所有标签的某些属性
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtmlAttributeCode(string str, params String[] attributes)
        {
            StringBuilder builder = new StringBuilder(str);

            Regex regexHtmlLabel = new Regex(@"<[\s\S]*?>", RegexOptions.IgnoreCase);

            foreach (Match match in regexHtmlLabel.Matches(builder.ToString()))
            {   //原始标签赋值给os临时变量
                String tempLable = match.Value;
                //最外层需要替换掉的标签字符串
                String replaceLabel = tempLable;
                foreach (string attribute in attributes)
                {
                    Regex regexAttribute = new Regex(attribute + @"=\S*?( |>)", RegexOptions.IgnoreCase);
                    //找到标签中对应的属性
                    foreach (Match matchTemp in regexAttribute.Matches(replaceLabel))
                    {
                        //找到替换后的临时变量
                        tempLable = tempLable.Replace(matchTemp.Value.TrimEnd('>'), "");
                        //最外层替换
                        builder.Replace(replaceLabel, tempLable);
                        //替换掉的标签字符串修改
                        replaceLabel = tempLable;
                    }
                }

            }

            return builder.ToString().Trim();

        }

        /// <summary>
        /// 移除指定标签的某些属性
        /// </summary>
        /// <param name="htmlStr">html格式的字符串</param>
        /// <param name="label">指定的标签</param>
        /// <param name="attributes">属性名称集合</param>
        /// <returns>移除后的新字符串</returns>
        public static string RemoveHtmlAttributeCode(string htmlStr, String label, params String[] attributes)
        {
            StringBuilder builder = new StringBuilder(htmlStr);

            Regex regexHtmlLabel = new Regex(@"<\/{0,1}" + label + @"[\s\S]*?>", RegexOptions.IgnoreCase);

            foreach (Match match in regexHtmlLabel.Matches(builder.ToString()))
            {   //原始标签赋值给os临时变量
                String tempLable = match.Value;

                //最外层需要替换掉的标签字符串
                String replaceLabel = tempLable;
                foreach (string attribute in attributes)
                {

                    Regex regexAttribute = new Regex(attribute + @"=\S*?( |>)", RegexOptions.IgnoreCase);


                    //找到标签中对应的属性
                    foreach (Match matchTemp in regexAttribute.Matches(replaceLabel))
                    {

                        //找到替换后的临时变量
                        tempLable = tempLable.Replace(matchTemp.Value.TrimEnd('>'), "");

                        //最外层替换
                        builder.Replace(replaceLabel, tempLable);

                        //替换掉的标签字符串修改
                        replaceLabel = tempLable;
                    }

                }

            }

            return builder.ToString().Trim();

        }

        /// <summary>
        /// 移除指定标签的某个属性
        /// </summary>
        /// <param name="htmlStr">html格式的字符串</param>
        /// <param name="label">指定的标签</param>
        /// <param name="attribute">属性名称</param>
        /// <returns>移除后的新字符串</returns>
        public static string RemoveHtmlAttributeCode(string htmlStr, String label, String attribute)
        {
            StringBuilder builder = new StringBuilder(htmlStr);

            Regex regex = new Regex(@"<\/{0,1}" + label + @"[\s\S]*?>", RegexOptions.IgnoreCase);

            Regex regexAttribute = new Regex(attribute + @"=\S*?( |>)", RegexOptions.IgnoreCase);

            //找到匹配到标签
            foreach (Match match in regex.Matches(builder.ToString()))
            {
                //原始标签赋值给os临时变量
                String tempLable = match.Value;

                //最外层需要替换掉的标签字符串
                String replaceLabel = tempLable;

                //找到标签中对应的属性
                foreach (Match matchTemp in regexAttribute.Matches(replaceLabel))
                {

                    //找到替换后的临时变量
                    tempLable = tempLable.Replace(matchTemp.Value.TrimEnd('>'), "");

                    //最外层替换
                    builder.Replace(replaceLabel, tempLable);

                    //替换掉的标签字符串修改
                    replaceLabel = tempLable;
                }
            }

            return builder.ToString();
        }
        /// <summary>
        /// 移除除了指定域名的其他URL
        /// </summary>
        /// <param name="str">Html字符串</param>
        /// <param name="myDomain">自己的域名，如：fund123.cn</param>
        /// <returns>结果字符串</returns>
        public static string RemoveUrlLink(String str, String myDomain)
        {
            StringBuilder builder = new StringBuilder(str);

            //找到所有的href连接
            Regex regex = new Regex(@" href=[\s\S]*?( |>)", RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(builder.ToString()))
            {
                //如果连接中存在域名则替换
                if (match.Value.ToLower().IndexOf(myDomain.ToLower()) == -1)
                {
                    String replaceStr = match.Value.TrimEnd('>');
                    builder.Replace(replaceStr, " ");
                }
            }

            return builder.ToString().Trim();
        }
        #endregion


        /// <summary>
        /// 判断是否存在别人的连接
        /// </summary>
        /// <param name="str"></param>
        /// <param name="myDomain"></param>
        /// <returns></returns>
        public static Boolean HasUrlLink(String str, String myDomain)
        {

            //找到所有的href连接
            Regex regex = new Regex(@" href=[\s\S]*?( |>)", RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(str))
            {
                //如果连接中存在域名则返回true
                if (match.Value.ToLower().IndexOf(myDomain.ToLower()) == -1)
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// 处理外网连接替换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="myDomain"></param>
        public static String DealOutSiteURL(String str, String myDomain)
        {
            StringBuilder sb = new StringBuilder(str);

            foreach (Match match in OutWebURL.Matches(str))
            {
                if (match.Value.ToLower().IndexOf(myDomain) == -1)
                {
                    sb.Replace(match.Value, "");
                }
            }

            return sb.ToString();
        }


        #region 老式Sql分页
        /// <summary>
        /// 获取分页操作SQL语句(对于排序的字段必须建立索引，优化分页提取方式)
        /// </summary>
        /// <param name="tblName">操作表名称</param>
        /// <param name="fldName">排序的索引字段</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="OrderType">排序方式(0升序，1为降序)</param>
        /// <param name="strWhere">检索的条件语句，不需要再加WHERE关键字</param>
        /// <returns></returns>
        public static string ConstructSplitSQL(string tblName,
                                                string fldName,
                                                int PageIndex,
                                                int PageSize,
                                                int totalRecord,
                                                int OrderType,
                                                string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";
            string rtnFields = "*";

            // 构造检索条件语句字符串
            if (strWhere != "")
            {
                // 去除不合法的字符，防止SQL注入式攻击
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // 升序操作
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " >= ( SELECT MAX(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                    strSQL += strWhere + "ORDER BY " + fldName + " ASC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                }
            }
            // 降序操作
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " <= ( SELECT MIN(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                    strSQL += strWhere + "ORDER BY " + fldName + " DESC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                }
            }
            else // 异常处理
            {
                throw new DataException("未指定任何排序类型。0升序，1为降序");
            }

            return strSQL;
        }


        /// <summary>
        /// 获取分页操作SQL语句(对于排序的字段必须建立索引)
        /// </summary>
        /// <param name="tblName">操作表名</param>
        /// <param name="fldName">操作索引字段名称</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="rtnFields">返回字段集合，中间用逗号格开。返回全部用“*”</param>
        /// <param name="OrderType">排序方式(0升序，1为降序)</param>
        /// <param name="strWhere">检索的条件语句，不需要再加WHERE关键字</param>
        /// <returns></returns>
        public static string ConstructSplitSQL(string tblName,
                                                string fldName,
                                                int PageIndex,
                                                int PageSize,
                                                string rtnFields,
                                                int OrderType,
                                                string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";

            // 构造检索条件语句字符串
            if (strWhere != "")
            {
                // 去除不合法的字符，防止SQL注入式攻击
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // 升序操作
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " >= ( SELECT MAX(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                    strSQL += strWhere + "ORDER BY " + fldName + " ASC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                }
            }
            // 降序操作
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " <= ( SELECT MIN(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                    strSQL += strWhere + "ORDER BY " + fldName + " DESC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                }
            }
            else // 异常处理
            {
                throw new DataException("未指定任何排序类型。0升序，1为降序");
            }

            return strSQL;
        }


        /// <summary>
        /// 获取分页操作SQL语句(对于排序的字段必须建立索引)
        /// </summary>
        /// <param name="tblName">操作表名</param>
        /// <param name="fldName">操作索引字段名称</param>
        /// <param name="unionCondition">用于连接的条件，例如: LEFT JOIN UserInfo u ON (u.UserID = b.UserID)</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="rtnFields">返回字段集合，中间用逗号格开。返回全部用“*”</param>
        /// <param name="OrderType">排序方式，0升序，1为降序</param>
        /// <param name="strWhere">检索的条件语句，不需要再加WHERE关键字</param>
        /// <returns></returns>
        public static string ConstructSplitSQL(string tblName,
            string fldName,
            string unionCondition,
            int PageIndex,
            int PageSize,
            string rtnFields,
            int OrderType,
            string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";

            // 构造检索条件语句字符串
            if (strWhere != "")
            {
                // 去除不合法的字符，防止SQL注入式攻击
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // 升序操作
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ";

                    //strSQL += "WHERE (" + fldName + " >= ( SELECT MAX(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                    strSQL += strWhere + "ORDER BY " + fldName + " ASC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                }
            }
            // 降序操作
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ";

                    //strSQL += "WHERE (" + fldName + " <= ( SELECT MIN(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                    strSQL += strWhere + "ORDER BY " + fldName + " DESC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                }
            }
            else // 异常处理
            {
                throw new DataException("未指定任何排序类型。0升序，1为降序");
            }

            return strSQL;
        }


        /// <summary>
        /// 获取分页操作SQL语句(对于排序的字段必须建立索引)
        /// </summary>
        /// <param name="tblName">操作表名</param>
        /// <param name="fldName">操作索引字段名称</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="rtnFields">返回字段集合，中间用逗号格开。返回全部用“*”</param>
        /// <param name="OrderType">排序方式(0升序，1为降序)</param>
        /// <param name="strWhere">检索的条件语句，不需要再加WHERE关键字</param>
        /// <returns></returns>
        public static string ConstructSplitSQL_TOP(string tblName,
                                                    string fldName,
                                                    int PageIndex,
                                                    int PageSize,
                                                    string rtnFields,
                                                    int OrderType,
                                                    string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";

            // 构造检索条件语句字符串
            if (strWhere != "")
            {
                // 去除不合法的字符，防止SQL注入式攻击
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // 升序操作
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += strWhere + " ORDER BY " + fldName + " ASC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                }
            }
            // 降序操作
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += strWhere + " ORDER BY " + fldName + " DESC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                }
            }
            else // 异常处理
            {
                throw new DataException("未指定任何排序类型。0升序，1为降序");
            }

            return strSQL;
        }

        /// <summary>
        /// 获取分页操作SQL语句(对于排序的字段必须建立索引)
        /// </summary>
        /// <param name="tblName">操作表名</param>
        /// <param name="fldName">操作索引字段名称</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="rtnFields">返回字段集合，中间用逗号格开。返回全部用“*”</param>
        /// <param name="OrderType">排序方式(0升序，1为降序)</param>
        /// <param name="sort">排序表达式</param>
        /// <param name="strWhere">检索的条件语句，不需要再加WHERE关键字</param>
        /// <returns></returns>
        public static string ConstructSplitSQL_sort(string tblName,
            string fldName,
            int PageIndex,
            int PageSize,
            string rtnFields,
            int OrderType,
            string sort,
            string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";

            // 构造检索条件语句字符串
            if (strWhere != "")
            {
                // 去除不合法的字符，防止SQL注入式攻击
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            if (sort != "") sort = " ORDER BY " + sort;

            // 升序操作
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " >= ( SELECT MAX(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                    strSQL += strWhere + sort;
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + sort + " ) AS T )) ";

                    strSQL += strOldWhere + sort;
                }
            }
            // 降序操作
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " <= ( SELECT MIN(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                    strSQL += strWhere + sort;
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + sort + " ) AS T )) ";

                    strSQL += strOldWhere + sort;
                }
            }
            else // 异常处理
            {
                throw new DataException("未指定主索引排序类型。0升序，1为降序");
            }

            return strSQL;
        }

        #endregion

        /// <summary>
        /// 服务于AES/DES加解密类的方法
        /// </summary>
        /// <param name="p_SrcString"></param>
        /// <param name="p_Length"></param>
        /// <param name="p_TailString"></param>
        /// <returns></returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            string text = p_SrcString;
            if (p_Length < 0)
            {
                return text;
            }
            byte[] sourceArray = Encoding.Default.GetBytes(p_SrcString);
            if (sourceArray.Length <= p_Length)
            {
                return text;
            }
            int length = p_Length;
            int[] numArray = new int[p_Length];
            byte[] destinationArray = null;
            int num2 = 0;
            for (int i = 0; i < p_Length; i++)
            {
                if (sourceArray[i] > 0x7f)
                {
                    num2++;
                    if (num2 == 3)
                    {
                        num2 = 1;
                    }
                }
                else
                {
                    num2 = 0;
                }
                numArray[i] = num2;
            }
            if ((sourceArray[p_Length - 1] > 0x7f) && (numArray[p_Length - 1] == 1))
            {
                length = p_Length + 1;
            }
            destinationArray = new byte[length];
            Array.Copy(sourceArray, destinationArray, length);
            return (Encoding.Default.GetString(destinationArray) + p_TailString);
        }
    }
}