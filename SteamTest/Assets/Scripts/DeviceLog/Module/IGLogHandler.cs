using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EscapeGame.Runtime.Log{
    public class IGLogHandler : ILogHandler{
        private readonly ILogHandler _defaultLogHandler;

        public IGLogHandler(ILogHandler logHandler){
            _defaultLogHandler = logHandler;
        }

        public void LogException(Exception exception, UnityEngine.Object context){
            _defaultLogHandler.LogException(exception, context);
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args){
            if (null == args || args.Length <= 0){
                return;
            }

            string formatStr = string.Empty;
            switch (logType){
                //这块还是做一下区分吧，editor输出的走 console 3 插件的filter，debug包才会写到文件去
                //debug消息太多了会频繁写入文件（写磁盘会卡吧），有时候只需要拿部分的需要的filter类型就行了
                //就是log的自定义过滤器
                //只有在输出debug文件时才有作用
                //这个Handle只是修改下Debug.Log来的日志的格式,Editor不需要,我在Analyzer中去除掉即可。
                case LogType.Log:
                    formatStr = LogAnalyzer.LOG_PRINT_KEY;
                    break;
                case LogType.Warning:
                    formatStr = LogAnalyzer.WARING_PRINT_KEY;
                    break;
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    formatStr = LogAnalyzer.ERROR_PRINT_KEY;
                    break;
            }

            if (null != context){
                
                formatStr = $"[{context}] {formatStr}";
            }

            _defaultLogHandler.LogFormat(logType, context, formatStr, args);
        }
    }
}