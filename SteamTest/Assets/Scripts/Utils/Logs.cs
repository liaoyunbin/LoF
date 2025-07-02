    using System;
    using UnityEngine;
    using System.Diagnostics;
    using Debug = UnityEngine.Debug;


    public interface ILogger
    {
        void Log(string msg, string stack, LogType type);
    }

    public static class Logs
    {
        public static bool useLog = true;
        public static string threadStack = string.Empty;
        public static ILogger logger = (ILogger)null;

        //[Conditional("DEBUGLOG")]
        public static void Log(string str)
        {
            if (Logs.useLog)
            {
                Debug.Log((object)str);
            }
            else
            {
                if (Logs.logger == null)
                    return;
                Logs.logger.Log(str, string.Empty, LogType.Log);
            }
        }
        
        //[Conditional("DEBUGLOG")]
        public static void Log(object message)
        {
            Logs.Log(message.ToString());
        }

        //[Conditional("DEBUGLOG")]
        public static void Log(string str, object arg0)
        {
            Logs.Log(string.Format(str, arg0));
        }

        //[Conditional("DEBUGLOG")]
        public static void Log(string str, object arg0, object arg1)
        {
            Logs.Log(string.Format(str, arg0, arg1));
        }

        //[Conditional("DEBUGLOG")]
        public static void Log(string str, object arg0, object arg1, object arg2)
        {
            Logs.Log(string.Format(str, arg0, arg1, arg2));
        }

        //[Conditional("DEBUGLOG")]
        public static void Log(string str, params object[] param)
        {
            Logs.Log(string.Format(str, param));
        }

        //[Conditional("DEBUGLOG")]
        public static void LogWarning(string str)
        {
            if (Logs.useLog)
            {
                Debug.LogWarning((object)str);
            }
            else
            {
                if (Logs.logger == null)
                    return;
                string stackTrace = StackTraceUtility.ExtractStackTrace();
                Logs.logger.Log(str, stackTrace, LogType.Warning);
            }
        }

        //[Conditional("DEBUGLOG")]
        public static void LogWarning(object message)
        {
            Logs.LogWarning(message.ToString());
        }

        //[Conditional("DEBUGLOG")]
        public static void LogWarning(string str, object arg0)
        {
            Logs.LogWarning(string.Format(str, arg0));
        }

        //[Conditional("DEBUGLOG")]
        public static void LogWarning(string str, object arg0, object arg1)
        {
            Logs.LogWarning(string.Format(str, arg0, arg1));
        }

        //[Conditional("DEBUGLOG")]
        public static void LogWarning(string str, object arg0, object arg1, object arg2)
        {
            Logs.LogWarning(string.Format(str, arg0, arg1, arg2));
        }

        //[Conditional("DEBUGLOG")]
        public static void LogWarning(string str, params object[] param)
        {
            Logs.LogWarning(string.Format(str, param));
        }

        //[Conditional("DEBUGLOG")]
        public static void LogError(string str)
        {
            if (Logs.useLog)
            {
                Debug.LogError((object)str);
            }
            else
            {
                if (Logs.logger == null)
                    return;
                string stackTrace = StackTraceUtility.ExtractStackTrace();
                Logs.logger.Log(str, stackTrace, LogType.Error);
            }
        }

        //[Conditional("DEBUGLOG")]
        public static void LogError(object message)
        {
            Logs.LogError(message.ToString());
        }

        //[Conditional("DEBUGLOG")]
        public static void LogError(string str, object arg0)
        {
            Logs.LogError(string.Format(str, arg0));
        }

        //[Conditional("DEBUGLOG")]
        public static void LogError(string str, object arg0, object arg1)
        {
            Logs.LogError(string.Format(str, arg0, arg1));
        }

        //[Conditional("DEBUGLOG")]
        public static void LogError(string str, object arg0, object arg1, object arg2)
        {
            Logs.LogError(string.Format(str, arg0, arg1, arg2));
        }

        //[Conditional("DEBUGLOG")]
        public static void LogError(string str, params object[] param)
        {
            Logs.LogError(string.Format(str, param));
        }


        //[Conditional("DEBUGLOG")]
        public static void LogException(Exception e)
        {
            Logs.threadStack = e.StackTrace;
            if (Logs.useLog)
            {
                Debug.LogError((object)e.Message);
            }
            else
            {
                if (Logs.logger == null)
                    return;
                Logs.logger.Log(e.Message, Logs.threadStack, LogType.Exception);
            }
        }

        //[Conditional("DEBUGLOG")]
        public static void LogException(string str, Exception e)
        {
            Logs.threadStack = e.StackTrace;
            str += e.Message;
            if (Logs.useLog)
            {
                Debug.LogError((object)str);
            }
            else
            {
                if (Logs.logger == null)
                    return;
                Logs.logger.Log(str, Logs.threadStack, LogType.Exception);
            }
        }

        //[Conditional("DEBUGLOG")]
        public static void LogException(Exception e, UnityEngine.Object o)
        {
            Logs.threadStack = e.StackTrace;
            string msg = string.Format("{0} {1}", (object)o.ToString(), (object)e.Message);
            if (Logs.useLog)
            {
                Debug.LogException(e, o);
            }
            else
            {
                if (Logs.logger == null)
                    return;
                Logs.logger.Log(msg, Logs.threadStack, LogType.Exception);
            }
        }
    }
