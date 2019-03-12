using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;


namespace Utilities
{
    /// <summary>
    /// 需要使用nuget获取itextsharp
    /// </summary>
    public class PdfHelper
    {
        /// <summary>
        /// 注意事项
        /// 1:html标签必须闭合，符合规范
        /// 2:目前暂不支持link样式导入，只能写style
        /// 使用示例：
        /// WebClient wc = new WebClient();
        /// 从网址下载Html字串
        /// string htmlText = wc.DownloadString("http://localhost:6953/test.html");
        /// byte[] pdfFile = PdfHelper.ConvertHtmlTextToPdf(htmlText);
        /// return File(pdfFile, "application/pdf", "测试.pdf");
        /// </summary>
        /// <param name="htmlText">html格式的字符串</param>
        /// <returns></returns>
        public static byte[] ConvertHtmlTextToPdf(string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }
            //避免当htmlText无任何html tag标签的纯文字时，转PDF时会挂掉，所以一律加上<p>标签
            htmlText = "<p>" + htmlText + "</p>";

            MemoryStream outputStream = new MemoryStream();//要把PDF写到哪个串流
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串转成byte[]
            MemoryStream msInput = new MemoryStream(data);
            Document doc = new Document();//要写PDF的文件，建构子没填的话预设直式A4
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            //指定文件预设开档时的缩放为100%
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //开启Document文件 
            doc.Open();
            
            //使用XMLWorkerHelper把Html parse到PDF档里
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
            //将pdfDest设定的资料写到PDF档
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);
            doc.Close();
            msInput.Close();
            outputStream.Close();
            //回传PDF档案 
            return outputStream.ToArray();
        }
    }
    /// <summary>
    /// 字体工厂类
    /// </summary>
    public class UnicodeFontFactory : FontFactoryImp
    {
        /// <summary>
        /// 定义一个pdf使用的字体，这里选择的是微软雅黑
        /// </summary>
        private static readonly string yh = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
          "msyh.ttc,0");
        /// <summary>
        /// 定义一个pdf使用的字体，这里选择的是楷体
        /// </summary>
        private static readonly string 标楷体Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
          "simkai.ttf"); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontname"></param>
        /// <param name="encoding"></param>
        /// <param name="embedded"></param>
        /// <param name="size"></param>
        /// <param name="style"></param>
        /// <param name="color"></param>
        /// <param name="cached"></param>
        /// <returns></returns>
        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
          bool cached)
        {
            //可用雅黑字体或标楷体，自己选一个
            BaseFont baseFont = BaseFont.CreateFont(yh, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style, color);
        }
    }
}

