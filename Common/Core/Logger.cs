using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Common.Core
{
    /// <summary>
    /// Helper class that support to write information to log file
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Private log object
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Levels of log
        /// </summary>
        public enum Levels
        {
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL
        }

        #region public methods
        /// <summary>
        /// Create log infomation
        /// </summary>
        /// <param name="level">Level of log, default will do nothing</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="info">Custom message about error/info/etc...</param>
        /// <param name="message">Error message</param>
        public static void CreateLog(Levels level, object className, string methodName, string info = null, string message = null)
        {
            CreateLog(level, className, methodName, new string[] { }, info, message);
        }

        /// <summary>
        /// Create log infomation
        /// </summary>
        /// <param name="level">Level of log, default will do nothing</param>
        /// <param name="className">Name of class</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="exception">Exception object</param>
        /// <param name="info">Custom message about error/info/etc...</param>
        /// <param name="message">Error message</param>
        public static void CreateLog(Levels level, object className, string methodName, Exception exception = null, string info = null, string message = null)
        {
            CreateLog(level, className, methodName, new string[] { }, info, message, exception);
        }

        /// <summary>
        /// Create log information
        /// </summary>
        /// <param name="level">Level of log, default will do nothing</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="param">Parameters input</param>
        /// <param name="info">Custom message about error/info/etc...</param>
        /// <param name="message">Error message</param>
        public static void CreateLog(Levels level, object className, string methodName, string[] param = null, string info = null, string message = null)
        {
            CreateLog(level, className, methodName, param, info, message, null);
        }

        /// <summary>
        /// Create log information
        /// </summary>
        /// <param name="level">Level of log, default will do nothing</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="param">Parameters input</param>
        /// <param name="info">Custom message about error/info/etc...</param>
        /// <param name="message">Error message</param>
        /// <param name="exception">Exception object</param>
        public static void CreateLog(Levels level, object className, string methodName, string[] param, string info, string message, Exception exception)
        {
            var errorMessageFormat = "{0}.{1}"
                                    + (param == null ? string.Empty : "({2})")
                                    + (string.IsNullOrWhiteSpace(info) ? string.Empty : "\r\n\t- {3}")
                                    + (string.IsNullOrWhiteSpace(message) ? string.Empty : "\r\n\t- {4}");
            var mess = string.Format(errorMessageFormat, className.ToString(), methodName, string.Join(", ", param), info, message);
            WriteLog(level, mess, exception);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Write log to file
        /// </summary>
        /// <param name="level">Level of log, default will do nothing</param>
        /// <param name="message">Message error</param>
        /// <param name="exception">Exception</param>
        private static void WriteLog(Levels level, string message, Exception exception)
        {
            switch (level)
            {
                case Levels.DEBUG:
                    _log.Debug(message, exception);
                    break;
                case Levels.INFO:
                    _log.Info(message, exception);
                    break;
                case Levels.WARN:
                    _log.Warn(message, exception);
                    break;
                case Levels.FATAL:
                    _log.Fatal(message, exception);
                    break;
                default:
                    _log.Error(message, exception);
                    break;
            }
        }
        #endregion
    }
}
