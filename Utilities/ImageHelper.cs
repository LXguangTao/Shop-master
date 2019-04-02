using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using QRCoder;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace Utilities
{
    /// <summary>
    /// 对图片的所有处理操作
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 生成网页验证码数据
        /// </summary>
        /// <param name="randomCode">验证码上显示的内容</param>
        /// <param name="imgWdith">宽度</param>
        /// <param name="imgHeight">高度</param>
        /// <returns></returns>
        public static byte[] CreateImage(string randomCode, int imgWdith = 70, int imgHeight = 28)
        {
            Bitmap map = new Bitmap(imgWdith, imgHeight);//创建图片背景  
            Graphics graph = Graphics.FromImage(map);
            graph.Clear(Color.AliceBlue);//清除画面，填充背景 
            Random rand = new Random();
            //背景噪点生成
            Brush blackPen = new SolidBrush(Color.LightGray);
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(0, map.Width);
                int y = rand.Next(0, map.Height);
                graph.FillRectangle(blackPen, x, y, 2, 2);
            }
            var chars = randomCode.ToCharArray();
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            float[] fontSize = { 10f, 12f, 14f, 16f };
            var offset = 5f;
            var sf = new StringFormat { LineAlignment = StringAlignment.Center };
            foreach (var t in chars)
            {
                var cindex = rand.Next(7);
                var findex = rand.Next(5);
                var f = new Font(font[findex], fontSize[rand.Next(0, 4)], FontStyle.Bold);
                Brush b = new SolidBrush(c[cindex]);
                var s = t.ToString();
                graph.DrawString(s, f, b, new RectangleF(offset, 0, map.Width, map.Height), sf);
                offset += graph.MeasureString(s, f).Width;

                b.Dispose();
                f.Dispose();
            }
            sf.Dispose();
            var ms = new MemoryStream();
            map.Save(ms, ImageFormat.Jpeg);
            var buffer = ms.ToArray();

            ms.Dispose();
            graph.Dispose();
            map.Dispose();

            return buffer;
        }

        /// <summary>
        /// 生成QR二维码
        /// </summary>
        /// <param name="strCode">二维码所含内容</param>        
        /// <returns></returns>
        public static byte[] CreateQRCode(string strCode)
        {
            if (strCode==null)
            {
                strCode = "此二维码无内容";
            }
            
            // 生成二维码的内容
            //string strCode = "http://www.walys.com";
            QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(strCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrcode = new QRCode(qrCodeData);
            // qrcode.GetGraphic 方法可参考最下发“补充说明”
            MemoryStream ms = new MemoryStream();
            Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
            //if (filename!=null)
            //{
            System.Drawing.Image img = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/111.jpg"));
            
            Bitmap icon = new Bitmap(img);
                
                icon.Save(ms, ImageFormat.Jpeg);
            //}           
            qrCodeImage.Save(ms, ImageFormat.Jpeg);
            // 如果想保存图片 可使用  qrCodeImage.Save(filePath);
            var buffer = ms.ToArray();
            return buffer;
            // 响应类型
            //context.Response.ContentType = "image/Jpeg";
            //输出字符流
            //context.Response.BinaryWrite(ms.ToArray());
            /* GetGraphic方法参数说明
                 public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon = null, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true)
             * 
                 int pixelsPerModule:生成二维码图片的像素大小 ，我这里设置的是5 
             * 
                 Color darkColor：暗色   一般设置为Color.Black 黑色
             * 
                 Color lightColor:亮色   一般设置为Color.White  白色
             * 
                 Bitmap icon :二维码 水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png")); 默认为NULL ，加上这个二维码中间会显示一个图标
             * 
                 int iconSizePercent： 水印图标的大小比例 ，可根据自己的喜好设置 
             * 
                 int iconBorderWidth： 水印图标的边框
             * 
                 bool drawQuietZones:静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true
             */

        }
        /// <summary>
        /// 获取指定路径的位图对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap GetImage(string path)
        {
            byte[] bs = GetImageData(path);
            return bs.ToImage();
        }
        /// <summary>
        /// 获取指定字节数组的位图
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static Bitmap ToImage(this byte[] bs)
        {
            if (bs != null && bs.Length > 50)
            {
                MemoryStream ms = new MemoryStream(bs);
                return new Bitmap(ms);
            }
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] GetImageData(string fileName)
        {
            FileStream fs = null;
            try
            {
                if (File.Exists(fileName))
                {
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    byte[] bs = new byte[fs.Length];
                    fs.Read(bs, 0, bs.Length);
                    return bs;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取【{0}】时出错:{1}", fileName, ex.Message);
                return new byte[1];
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        /// <summary>
        /// 将Image图像保存为png格式的文件
        /// </summary>
        /// <param name="img"></param>
        /// <param name="path"></param>
        public static void SavePng(Image img, string path)
        {
            img.Save(path, ImageFormat.Png);
        }
        /// <summary>
        /// 将Image图像保存为bmp格式的文件
        /// </summary>
        /// <param name="img"></param>
        /// <param name="path"></param>
        public static void SaveBmp(Image img, string path)
        {
            img.Save(path, ImageFormat.Bmp);
        }
        /// <summary>
        /// 将Image图像保存为jpg格式的文件
        /// </summary>
        /// <param name="img">Image对象</param>
        /// <param name="path">要保存的图片路径</param>
        /// <param name="quality">质量(0~100)</param>
        public static void SaveJpg(Image img, string path, long quality = 75)
        {
            using (EncoderParameters paras = new EncoderParameters(1))
            {
                paras.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
                img.Save(path, GetEncoderInfo("image/jpeg"), paras);
            }
        }
        /// <summary>
        /// 获取ImageCodecInfo信息
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放(常用于web开发中的头像截取)
        /// </summary>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="fileSaveUrl">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(System.IO.Stream fromFile, string fileSaveUrl, int side, int quality)
        {
            //创建目录
            string dir = Path.GetDirectoryName(fileSaveUrl);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);
            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= side && initImage.Height <= side)
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            else
            {
                //原始图片的宽、高
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;
                //非正方型先裁剪为正方型
                if (initWidth != initHeight)
                {
                    //截图对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;
                    //宽大于高的横图
                    if (initWidth > initHeight)
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                        Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                        //画图
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽
                        initWidth = initHeight;
                    }
                    else//高大于宽的竖图
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                        Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                        //画图
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置高
                        initHeight = initWidth;
                    }
                    //将截图对象赋给原图
                    initImage = (System.Drawing.Image)pickedImage.Clone();
                    //释放截图资源
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
                //缩略图对象
                using (System.Drawing.Image resultImage = new System.Drawing.Bitmap(side, side))
                {
                    using (System.Drawing.Graphics resultG = System.Drawing.Graphics.FromImage(resultImage))
                    {
                        //设置质量
                        resultG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        resultG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //用指定背景色清空画布
                        resultG.Clear(Color.White);
                        //绘制缩略图
                        resultG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initWidth, initHeight), System.Drawing.GraphicsUnit.Pixel);
                        //关键质量控制
                        //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                        ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                        ImageCodecInfo ici = null;
                        foreach (ImageCodecInfo i in icis)
                        {
                            if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                            {
                                ici = i;
                            }
                        }
                        using (EncoderParameters ep = new EncoderParameters(1))
                        {
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
                            //保存缩略图
                            resultImage.Save(fileSaveUrl, ici, ep);
                            //释放关键质量控制所用资源
                            ep.Dispose();
                        }
                        //释放缩略图资源
                        resultG.Dispose();
                    }
                    resultImage.Dispose();
                }
                //释放原始图片资源
                initImage.Dispose();
            }
        }
        /// <summary>
        /// 指定长宽裁剪(按模版比例最大范围的裁剪图片并缩放至模版尺寸)
        /// </summary>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="fileSaveUrl">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(System.IO.Stream fromFile, string fileSaveUrl, int maxWidth, int maxHeight, int quality)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            using (System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true))
            {
                CutForCustom(initImage, fileSaveUrl, maxWidth, maxHeight, quality);
            }
        }

        /// <summary>
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="initImage">原始图片</param>
        /// <param name="fileSaveUrl">要保存的路径</param>
        /// <param name="maxWidth">最大宽度px</param>
        /// <param name="maxHeight">最大高度px</param>
        /// <param name="quality">图片质量（范围0-100）</param>
        public static void CutForCustom(System.Drawing.Image initImage, string fileSaveUrl, int maxWidth, int maxHeight, int quality, float dpi = 96)
        {
            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            else
            {
                //模版的宽高比例
                double templateRate = (double)maxWidth / maxHeight;
                //原图片的宽高比例
                double initRate = (double)initImage.Width / initImage.Height;
                Bitmap templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight); //按模版大小生成最终图片
                templateImage.SetResolution(dpi, dpi);
                //原图与模版比例相等，直接缩放
                if (templateRate == initRate)
                {
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;
                    //定位
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位
                                                              //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象实例化
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)System.Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //裁剪源定位
                        fromR.X = 0;
                        fromR.Y = (int)System.Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        fromR.X = (int)System.Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //裁剪
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    ////保存缩略图
                    //templateImage.Save(fileSaveUrl, ici, ep);
                    EncoderParameters paras = new EncoderParameters(1);
                    paras.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
                    templateImage.Save(fileSaveUrl, GetEncoderInfo("image/jpeg"), paras);
                    //释放资源
                    templateG.Dispose();
                    templateImage.Dispose();
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
            }
        }
        //// <summary>
        /// 图片等比缩放并设置水印
        ///  </summary>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度</param>
        /// <param name="targetHeight">指定的最大高度</param>
        /// <param name="watermarkText">水印文字(为""表示不使用水印)</param>
        /// <param name="watermarkImage">水印图片路径(为""表示不使用水印)</param>
        public static void ZoomAuto(System.IO.Stream fromFile, string savePath, System.Double targetWidth, System.Double targetHeight, string watermarkText, string watermarkImage)
        {
            //创建目录
            string dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= targetWidth && initImage.Height <= targetHeight)
            {
                //文字水印
                if (watermarkText != "")
                {
                    using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(initImage))
                    {
                        System.Drawing.Font fontWater = new Font("黑体", 10);
                        System.Drawing.Brush brushWater = new SolidBrush(Color.White);
                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                        gWater.Dispose();
                    }
                }
                //透明图片水印
                if (watermarkImage != "")
                {
                    if (File.Exists(watermarkImage))
                    {
                        //获取水印图片
                        using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (initImage.Width >= wrImage.Width && initImage.Height >= wrImage.Height)
                            {
                                Graphics gWater = Graphics.FromImage(initImage);

                                //透明属性
                                ImageAttributes imgAttributes = new ImageAttributes();
                                ColorMap colorMap = new ColorMap();
                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remapTable = { colorMap };
                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                float[][] colorMatrixElements = {
                            new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                            new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                            new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                            new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                            new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                        };

                                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                gWater.DrawImage(wrImage, new Rectangle(initImage.Width - wrImage.Width, initImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);

                                gWater.Dispose();
                            }
                            wrImage.Dispose();
                        }
                    }
                }

                //保存
                initImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //缩略图宽、高计算
                double newWidth = initImage.Width;
                double newHeight = initImage.Height;
                //宽大于高或宽等于高（横图或正方）
                if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
                {
                    //如果宽大于模版
                    if (initImage.Width > targetWidth)
                    {
                        //宽按模版，高按比例缩放
                        newWidth = targetWidth;
                        newHeight = initImage.Height * (targetWidth / initImage.Width);
                    }
                }
                //高大于宽（竖图）
                else
                {
                    //如果高大于模版
                    if (initImage.Height > targetHeight)
                    {
                        //高按模版，宽按比例缩放
                        newHeight = targetHeight;
                        newWidth = initImage.Width * (targetHeight / initImage.Height);
                    }
                }
                //生成新图
                //新建一个bmp图片
                System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                //新建一个画板
                System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);
                //设置质量
                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //置背景色
                newG.Clear(Color.White);
                //画图
                newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);

                //文字水印
                if (watermarkText != "")
                {
                    using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(newImage))
                    {
                        System.Drawing.Font fontWater = new Font("宋体", 10);
                        System.Drawing.Brush brushWater = new SolidBrush(Color.White);
                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                        gWater.Dispose();
                    }
                }

                //透明图片水印
                if (watermarkImage != "")
                {
                    if (File.Exists(watermarkImage))
                    {
                        //获取水印图片
                        using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (newImage.Width >= wrImage.Width && newImage.Height >= wrImage.Height)
                            {
                                Graphics gWater = Graphics.FromImage(newImage);

                                //透明属性
                                ImageAttributes imgAttributes = new ImageAttributes();
                                ColorMap colorMap = new ColorMap();
                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remapTable = { colorMap };
                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                float[][] colorMatrixElements = {
                            new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                            new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                            new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                            new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                            new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                        };

                                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                gWater.DrawImage(wrImage, new Rectangle(newImage.Width - wrImage.Width, newImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                                gWater.Dispose();
                            }
                            wrImage.Dispose();
                        }
                    }
                }
                //保存缩略图
                newImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //释放资源
                newG.Dispose();
                newImage.Dispose();
                initImage.Dispose();
            }
        }
        /// <summary>
        /// 截取源图中指定部分区域的图片，并存储为指定的文件。
        /// </summary>
        /// <param name="sourceFileName">源图地址</param>
        /// <param name="saveFilePath">区域图的地址</param>
        /// <param name="width">截取图片的宽度</param>
        /// <param name="height">截取图片的高度</param>
        /// <param name="offsetX">开始截取图片的X坐标</param>
        /// <param name="offsetY">开始截取图片的Y坐标</param>
        /// <param name="qu">保存质量</param>
        /// <param name="xdpi">横向dpi值</param>
        /// <param name="ydpi">纵向dpi值</param>
        public static void SavePartOfImageRec(string sourceFileName, string saveFilePath, int width, int height, int offsetX, int offsetY, int qu = 100, int xdpi = 96, int ydpi = 96)
        {
            using (Bitmap sourceBitmap = new Bitmap(sourceFileName))
            {
                using (Bitmap resultBitmap = new Bitmap(width, height))
                {
                    resultBitmap.SetResolution(xdpi, ydpi);
                    using (Graphics g = Graphics.FromImage(resultBitmap))
                    {
                        Rectangle resultRectangle = new Rectangle(0, 0, width, height);
                        Rectangle sourceRectangle = new Rectangle(0 + offsetX, 0 + offsetY, sourceBitmap.Width, sourceBitmap.Height);
                        g.DrawImage(sourceBitmap, resultRectangle, sourceRectangle, GraphicsUnit.Pixel);
                    }
                    EncoderParameters paras = new EncoderParameters(1);
                    paras.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)qu);
                    resultBitmap.Save(saveFilePath, GetEncoderInfo("image/jpeg"), paras);
                }
            }
        }
        /// <summary>
        /// 获取一张图片的指定部分，在一个指定位置画一个要截取的图像       
        /// </summary>
        /// <param name="sourceFileName">原始图片路径名称</param>
        /// <param name="width">截取图片的宽度</param>
        /// <param name="height">截取图片的高度</param>
        /// <param name="offsetX">开始截取图片的X坐标</param>
        /// <param name="offsetY">开始截取图片的Y坐标</param>
        /// <returns>指定部分转换成的字节数组</returns>
        /// <param name="qu">保存质量</param>
        /// <param name="xdpi">横向dpi值</param>
        /// <param name="ydpi">纵向dpi值</param>
        public static byte[] GetPartOfImageRec(string sourceFileName, int width, int height, int offsetX, int offsetY, int qu = 100, int xdpi = 96, int ydpi = 96)
        {
            MemoryStream ms = new MemoryStream();
            using (Bitmap sourceBitmap = new Bitmap(sourceFileName))
            {
                using (Bitmap resultBitmap = new Bitmap(width, height))
                {
                    resultBitmap.SetResolution(xdpi, ydpi);
                    using (Graphics g = Graphics.FromImage(resultBitmap))
                    {
                        Rectangle resultRectangle = new Rectangle(0, 0, width, height);
                        Rectangle sourceRectangle = new Rectangle(0 + offsetX, 0 + offsetY, sourceBitmap.Width, sourceBitmap.Height);
                        g.DrawImage(sourceBitmap, resultRectangle, sourceRectangle, GraphicsUnit.Pixel);
                    }
                    EncoderParameters paras = new EncoderParameters(1);
                    paras.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)qu);
                    resultBitmap.Save(ms, GetEncoderInfo("image/jpeg"), paras);
                }
            }
            return ms.ToArray();
        }

        /// <summary>
        /// 设置图形颜色  边缘的色彩更换成新的颜色
        /// </summary>
        /// <param name="p_Image">图片</param>
        /// <param name="p_OldColor">老的边缘色彩</param>
        /// <param name="p_NewColor">新的边缘色彩</param>
        /// <param name="p_Float">溶差</param>
        /// <returns>清理后的图形</returns>
        public static Image SetImageColorBrim(Image p_Image, Color p_OldColor, Color p_NewColor, int p_Float)
        {
            int _Width = p_Image.Width;
            int _Height = p_Image.Height;

            Bitmap _NewBmp = new Bitmap(_Width, _Height, PixelFormat.Format32bppArgb);
            Graphics _Graphics = Graphics.FromImage(_NewBmp);
            _Graphics.DrawImage(p_Image, new Rectangle(0, 0, _Width, _Height));
            _Graphics.Dispose();

            BitmapData _Data = _NewBmp.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _Data.PixelFormat = PixelFormat.Format32bppArgb;
            int _ByteSize = _Data.Stride * _Height;
            byte[] _DataBytes = new byte[_ByteSize];
            Marshal.Copy(_Data.Scan0, _DataBytes, 0, _ByteSize);

            int _Index = 0;
            #region 列
            for (int z = 0; z != _Height; z++)
            {
                _Index = z * _Data.Stride;
                for (int i = 0; i != _Width; i++)
                {
                    Color _Color = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);

                    if (ScanColor(_Color, p_OldColor, p_Float))
                    {
                        _DataBytes[_Index + 3] = (byte)p_NewColor.A;
                        _DataBytes[_Index + 2] = (byte)p_NewColor.R;
                        _DataBytes[_Index + 1] = (byte)p_NewColor.G;
                        _DataBytes[_Index] = (byte)p_NewColor.B;
                        _Index += 4;
                    }
                    else
                    {
                        break;
                    }
                }
                _Index = (z + 1) * _Data.Stride;
                for (int i = 0; i != _Width; i++)
                {
                    Color _Color = Color.FromArgb(_DataBytes[_Index - 1], _DataBytes[_Index - 2], _DataBytes[_Index - 3], _DataBytes[_Index - 4]);

                    if (ScanColor(_Color, p_OldColor, p_Float))
                    {
                        _DataBytes[_Index - 1] = (byte)p_NewColor.A;
                        _DataBytes[_Index - 2] = (byte)p_NewColor.R;
                        _DataBytes[_Index - 3] = (byte)p_NewColor.G;
                        _DataBytes[_Index - 4] = (byte)p_NewColor.B;
                        _Index -= 4;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            #endregion

            #region 行

            for (int i = 0; i != _Width; i++)
            {
                _Index = i * 4;
                for (int z = 0; z != _Height; z++)
                {
                    Color _Color = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);
                    if (ScanColor(_Color, p_OldColor, p_Float))
                    {
                        _DataBytes[_Index + 3] = (byte)p_NewColor.A;
                        _DataBytes[_Index + 2] = (byte)p_NewColor.R;
                        _DataBytes[_Index + 1] = (byte)p_NewColor.G;
                        _DataBytes[_Index] = (byte)p_NewColor.B;
                        _Index += _Data.Stride;
                    }
                    else
                    {
                        break;
                    }
                }
                _Index = (i * 4) + ((_Height - 1) * _Data.Stride);
                for (int z = 0; z != _Height; z++)
                {
                    Color _Color = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);
                    if (ScanColor(_Color, p_OldColor, p_Float))
                    {
                        _DataBytes[_Index + 3] = (byte)p_NewColor.A;
                        _DataBytes[_Index + 2] = (byte)p_NewColor.R;
                        _DataBytes[_Index + 1] = (byte)p_NewColor.G;
                        _DataBytes[_Index] = (byte)p_NewColor.B;
                        _Index -= _Data.Stride;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            #endregion
            Marshal.Copy(_DataBytes, 0, _Data.Scan0, _ByteSize);
            _NewBmp.UnlockBits(_Data);
            return _NewBmp;
        }
        /// <summary>
        /// 设置图形颜色  所有的色彩更换成新的颜色
        /// </summary>
        /// <param name="p_Image">图片</param>
        /// <param name="p_OdlColor">老的颜色</param>
        /// <param name="p_NewColor">新的颜色</param>
        /// <param name="p_Float">溶差</param>
        /// <returns>清理后的图形</returns>
        public static Image SetImageColorAll(Image p_Image, Color p_OdlColor, Color p_NewColor, int p_Float)
        {
            int _Width = p_Image.Width;
            int _Height = p_Image.Height;

            Bitmap _NewBmp = new Bitmap(_Width, _Height, PixelFormat.Format32bppArgb);
            Graphics _Graphics = Graphics.FromImage(_NewBmp);
            _Graphics.DrawImage(p_Image, new Rectangle(0, 0, _Width, _Height));
            _Graphics.Dispose();

            BitmapData _Data = _NewBmp.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _Data.PixelFormat = PixelFormat.Format32bppArgb;
            int _ByteSize = _Data.Stride * _Height;
            byte[] _DataBytes = new byte[_ByteSize];
            Marshal.Copy(_Data.Scan0, _DataBytes, 0, _ByteSize);

            int _WhileCount = _Width * _Height;
            int _Index = 0;
            for (int i = 0; i != _WhileCount; i++)
            {
                Color _Color = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);
                if (ScanColor(_Color, p_OdlColor, p_Float))
                {
                    _DataBytes[_Index + 3] = (byte)p_NewColor.A;
                    _DataBytes[_Index + 2] = (byte)p_NewColor.R;
                    _DataBytes[_Index + 1] = (byte)p_NewColor.G;
                    _DataBytes[_Index] = (byte)p_NewColor.B;
                }
                _Index += 4;
            }
            Marshal.Copy(_DataBytes, 0, _Data.Scan0, _ByteSize);
            _NewBmp.UnlockBits(_Data);
            return _NewBmp;
        }
        /// <summary>
        /// 设置图形颜色  坐标的颜色更换成新的色彩 （漏斗）
        /// </summary>
        /// <param name="p_Image">新图形</param>
        /// <param name="p_Point">位置</param>
        /// <param name="p_NewColor">新的色彩</param>
        /// <param name="p_Float">溶差</param>
        /// <returns>清理后的图形</returns>
        public static Image SetImageColorPoint(Image p_Image, Point p_Point, Color p_NewColor, int p_Float)
        {
            int _Width = p_Image.Width;
            int _Height = p_Image.Height;

            if (p_Point.X > _Width - 1) return p_Image;
            if (p_Point.Y > _Height - 1) return p_Image;

            Bitmap _SS = (Bitmap)p_Image;
            Color _Scolor = _SS.GetPixel(p_Point.X, p_Point.Y);//获取指定坐标的颜色
                                                               //创建一个空位图，将源图绘制到此位图中
            Bitmap _NewBmp = new Bitmap(_Width, _Height, PixelFormat.Format32bppArgb);
            Graphics _Graphics = Graphics.FromImage(_NewBmp);
            _Graphics.DrawImage(p_Image, new Rectangle(0, 0, _Width, _Height));
            _Graphics.Dispose();

            BitmapData _Data = _NewBmp.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _Data.PixelFormat = PixelFormat.Format32bppArgb;
            int _ByteSize = _Data.Stride * _Height;
            byte[] _DataBytes = new byte[_ByteSize];
            Marshal.Copy(_Data.Scan0, _DataBytes, 0, _ByteSize);


            int _Index = (p_Point.Y * _Data.Stride) + (p_Point.X * 4);

            Color _OldColor = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);

            if (_OldColor.Equals(p_NewColor)) return p_Image;
            Stack<Point> _ColorStack = new Stack<Point>(1000);
            _ColorStack.Push(p_Point);

            _DataBytes[_Index + 3] = (byte)p_NewColor.A;
            _DataBytes[_Index + 2] = (byte)p_NewColor.R;
            _DataBytes[_Index + 1] = (byte)p_NewColor.G;
            _DataBytes[_Index] = (byte)p_NewColor.B;

            do
            {
                Point _NewPoint = (Point)_ColorStack.Pop();

                if (_NewPoint.X > 0) SetImageColorPoint(_DataBytes, _Data.Stride, _ColorStack, _NewPoint.X - 1, _NewPoint.Y, _OldColor, p_NewColor, p_Float);
                if (_NewPoint.Y > 0) SetImageColorPoint(_DataBytes, _Data.Stride, _ColorStack, _NewPoint.X, _NewPoint.Y - 1, _OldColor, p_NewColor, p_Float);

                if (_NewPoint.X < _Width - 1) SetImageColorPoint(_DataBytes, _Data.Stride, _ColorStack, _NewPoint.X + 1, _NewPoint.Y, _OldColor, p_NewColor, p_Float);
                if (_NewPoint.Y < _Height - 1) SetImageColorPoint(_DataBytes, _Data.Stride, _ColorStack, _NewPoint.X, _NewPoint.Y + 1, _OldColor, p_NewColor, p_Float);

            }
            while (_ColorStack.Count > 0);

            Marshal.Copy(_DataBytes, 0, _Data.Scan0, _ByteSize);
            _NewBmp.UnlockBits(_Data);
            return _NewBmp;
        }
        /// <summary>
        /// SetImageColorPoint 循环调用 检查新的坐标是否符合条件 符合条件会写入栈p_ColorStack 并更改颜色
        /// </summary>
        /// <param name="p_DataBytes">数据区</param>
        /// <param name="p_Stride">行扫描字节数</param>
        /// <param name="p_ColorStack">需要检查的位置栈</param>
        /// <param name="p_X">位置X</param>
        /// <param name="p_Y">位置Y</param>
        /// <param name="p_OldColor">老色彩</param>
        /// <param name="p_NewColor">新色彩</param>
        /// <param name="p_Float">溶差</param>
        private static void SetImageColorPoint(byte[] p_DataBytes, int p_Stride, Stack<Point> p_ColorStack, int p_X, int p_Y, Color p_OldColor, Color p_NewColor, int p_Float)
        {

            int _Index = (p_Y * p_Stride) + (p_X * 4);
            Color _OldColor = Color.FromArgb(p_DataBytes[_Index + 3], p_DataBytes[_Index + 2], p_DataBytes[_Index + 1], p_DataBytes[_Index]);

            if (ScanColor(_OldColor, p_OldColor, p_Float))
            {
                p_ColorStack.Push(new Point(p_X, p_Y));

                p_DataBytes[_Index + 3] = (byte)p_NewColor.A;
                p_DataBytes[_Index + 2] = (byte)p_NewColor.R;
                p_DataBytes[_Index + 1] = (byte)p_NewColor.G;
                p_DataBytes[_Index] = (byte)p_NewColor.B;
            }
        }

        /// <summary>
        /// 检查色彩(可以根据这个更改比较方式
        /// </summary>
        /// <param name="p_CurrentlyColor">当前色彩</param>
        /// <param name="p_CompareColor">比较色彩</param>
        /// <param name="p_Float">溶差</param>
        /// <returns></returns>
        private static bool ScanColor(Color p_CurrentlyColor, Color p_CompareColor, int p_Float)
        {
            int _R = p_CurrentlyColor.R;
            int _G = p_CurrentlyColor.G;
            int _B = p_CurrentlyColor.B;
            return (_R <= p_CompareColor.R + p_Float && _R >= p_CompareColor.R - p_Float) && (_G <= p_CompareColor.G + p_Float && _G >= p_CompareColor.G - p_Float) && (_B <= p_CompareColor.B + p_Float && _B >= p_CompareColor.B - p_Float);
        }
    }
}
