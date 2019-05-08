using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisPlatformWeb.Common
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class NlogHelper
    {

        public static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="throwMsg">操作信息</param>
        public static void ErrorLog(string throwMsg)
        {
            logger.Error(throwMsg);
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="operateMsg">操作信息</param>
        public static void InfoLog(string operateMsg)
        {
            logger.Info(operateMsg);
        }
        /// <summary>
        /// 跟踪
        /// </summary>
        /// <param name="operateMsg">操作信息</param>
        public static void Trace(string operateMsg)
        {
            logger.Info(operateMsg);
        }
    }
}
