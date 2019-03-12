using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Utilities
{
    /// <summary>
    /// 序列化工具类
    /// </summary>
    public static class IoHelper
    {
        //是否已经加载了JPEG编码解码器
        private static bool _isloadjpegcodec = false;
        //当前系统安装的JPEG编码解码器
        private static ImageCodecInfo _jpegcodec = null;

        /// <summary>
        /// 获得文件物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            return System.Web.Hosting.HostingEnvironment.MapPath(path);
        }

        #region Xml部分
        /// <summary>
        /// 序列化任意对象（XML）
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的 XML 字符串</returns>
        public static string SerializeXml(object obj)
        {
            XmlSerializer xml = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            xml.Serialize(ms, obj);
            byte[] tmpBytes = ms.ToArray();
            ms.Close();
            return System.Text.Encoding.UTF8.GetString(tmpBytes);
        }
        /// <summary>
        /// 反序列化任意对象（XML）
        /// </summary>
        /// <param name="xmlString">XML字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T DeSerializeXml<T>(string xmlString)
        {
            byte[] tmpBytes = System.Text.Encoding.Default.GetBytes(xmlString);
            MemoryStream ms = new MemoryStream(tmpBytes);
            ms.Position = 0;
            XmlSerializer xml = new XmlSerializer(typeof(T));
            T obj = (T)xml.Deserialize(ms);
            ms.Close();
            return obj;
        }
        #endregion

        #region Json部分
        /// <summary>
        /// 将obj对象序列化成JSON字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>JSON字符串</returns>
        public static string SerializeJson(Object obj)
        {
            string json = "";
            DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ser.WriteObject(ms, obj);
                json = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
            return json;
        }
        /// <summary>
        /// 将JSON字符串序反列化为指定T型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>T类型的对象</returns>
        public static T DeSerializeJson<T>(string json)
        {
            System.Text.UTF8Encoding utf = new System.Text.UTF8Encoding();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (System.IO.MemoryStream s = new System.IO.MemoryStream(utf.GetBytes(json)))
            {
                return (T)ser.ReadObject(s);
            }
        }
        #endregion

        #region IO部分
        /// <summary>
        /// 将指定对象obj序列化到文件fn中
        /// </summary>
        /// <param name="fn">文件路径</param>
        /// <param name="obj">对象</param>
        public static void Serialize(string fn, object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(fn, FileMode.OpenOrCreate, FileAccess.Write))
            {
                bf.Serialize(fs, obj);
            }
        }
        /// <summary>
        /// 从文件fn中反序列出类型为T的对象
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="fn">文件名称</param>
        /// <returns>对象</returns>
        public static T DeSerialize<T>(string fn)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(fn, FileMode.OpenOrCreate, FileAccess.Read))
            {
                return (T)bf.Deserialize(fs);
            }
        }
        /// <summary>
        /// 获取指定对象的深度克隆对象
        /// </summary>
        /// <typeparam name="T">被克隆者类型</typeparam>
        /// <param name="obj">被克隆者</param>
        /// <returns>克隆出来的对象</returns>
        public static T Clone<T>(T obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                ms.Position = 0;
                return (T)bf.Deserialize(ms);
            }
        }
        #endregion
    }
}
