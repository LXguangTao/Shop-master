using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// 封装NLog组件进行日志管理
    /// </summary>
    public class LogHelper
    {
        NLog.Logger logger;
        /// <summary>
        /// 私有构造方法，只给当前类的其他构造方法调用
        /// </summary>
        /// <param name="logger"></param>
        private LogHelper(NLog.Logger logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// 通过指定的名称来创建日志对象
        /// </summary>
        /// <param name="name"></param>
        public LogHelper(string name): this(NLog.LogManager.GetLogger(name)){ }
        /// <summary>
        /// 提供1个默认的静态使用方式，例如：LogHelper.Default.Info("info");
        /// </summary>
        public static LogHelper Default { get; private set; }
        /// <summary>
        /// 私有的静态构造方法，初始化Default对象
        /// </summary>
        static LogHelper()
        {
            Default = new LogHelper(NLog.LogManager.GetCurrentClassLogger());
        }
        /// <summary>
        /// Trace等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="args">其他参数</param>
        public void Trace(string msg, params object[] args)
        {
            logger.Trace(msg, args);
        }
        /// <summary>
        /// Trace等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="err">需要记录的异常信息</param>
        public void Trace(string msg, Exception err)
        {
            logger.Trace(err, msg);
        }
        /// <summary>
        /// Debug等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="args">其他参数</param>
        public void Debug(string msg, params object[] args)
        {
            logger.Debug(msg, args);
        }
        /// <summary>
        /// Debug等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="err">需要记录的异常信息</param>
        public void Debug(string msg, Exception err)
        {
            logger.Debug( err, msg);
        }
        /// <summary>
        /// Info等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="args">其他参数</param>
        public void Info(string msg, params object[] args)
        {
            logger.Info(msg, args);
        }
        /// <summary>
        /// Info等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="err">需要记录的异常信息</param>
        public void Info(string msg, Exception err)
        {
            logger.Info(err, msg);
        }
        /// <summary>
        /// Warn等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="args">其他参数</param>
        public void Warn(string msg, params object[] args)
        {
            logger.Warn(msg, args);
        }
        /// <summary>
        /// Warn等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="err">需要记录的异常信息</param>
        public void Warn(string msg, Exception err)
        {
            logger.Warn(err, msg);
        }
        /// <summary>
        /// Error等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="args">其他参数</param>
        public void Error(string msg, params object[] args)
        {
            logger.Error(msg, args);
        }
        /// <summary>
        /// Error等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="err">需要记录的异常信息</param>
        public void Error(string msg, Exception err)
        {
            logger.Error(err, msg);
        }
        /// <summary>
        /// Fatal等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="args">其他参数</param>
        public void Fatal(string msg, params object[] args)
        {
            logger.Fatal(msg, args);
        }
        /// <summary>
        /// Fatal等级写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="err">需要记录的异常信息</param>
        public void Fatal(string msg, Exception err)
        {
            logger.Fatal(err, msg);
        }
    }
}