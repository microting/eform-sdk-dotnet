using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eFormShared
{
    public class Log
    {
        #region var
        CoreBase core;
        LogWriter logWriter;
        int logLevel;
        Queue logQue;
        Tools t = new Tools();
        List<ExceptionClass> exceptionLst = new List<ExceptionClass>();
        #endregion

        // con
        public Log(CoreBase core, LogWriter logWriter, int logLevel)
        {
            try
            {
                this.core = core;
                this.logWriter = logWriter;
                this.logLevel = logLevel;
                logQue = new Queue();
            }
            catch (Exception ex)
            {
                core.FatalExpection(t.GetMethodName() + "failed", ex);
            }
        }

        #region public
        public void LogEverything(string str)
        {
            LogLogic(new LogEntry(4, str));
        }

        public void LogVariable(string variableName, string variableContent)
        {
            LogLogic(new LogEntry(3, "Variable Name:" + variableName.ToString() + " / Content:" + variableContent.ToString()));
        }

        public void LogVariable(string variableName, int? variableContent)
        {
            LogVariable(variableName, variableContent.ToString());
        }

        public void LogVariable(string variableName, bool? variableContent)
        {
            LogVariable(variableName, variableContent.ToString());
        }

        public void LogVariable(string variableName, DateTime? variableContent)
        {
            LogVariable(variableName, variableContent.ToString());
        }

        public void LogStandard(string str)
        {
            LogLogic(new LogEntry(2, str));
        }

        public void LogCritical(string str)
        {
            LogLogic(new LogEntry(1, str));
        }

        public void LogWarning(string str)
        {
            LogLogic(new LogEntry(0, str));
        }

        public void LogException(string exceptionDescription, Exception exception, bool restartCore)
        {
            try
            {
                string fullExceptionDescription = t.PrintException(exceptionDescription, exception);

                LogLogic(new LogEntry(-1, fullExceptionDescription));
                LogVariable("restartCore", restartCore);
                
                ExceptionClass exCls = new ExceptionClass(fullExceptionDescription, DateTime.Now);
                exceptionLst.Add(exCls);

                int secondsDelay = CheckExceptionLst(exCls);
                if (restartCore)
                {
                    Thread coreRestartThread = new Thread(() => core.Restart(secondsDelay));
                    coreRestartThread.Start();
                }
            }
            catch (Exception ex)
            {
                core.FatalExpection(t.GetMethodName() + "failed", ex);
            }
        }
        #endregion

        #region private
        private int  CheckExceptionLst(ExceptionClass exceptionClass)
        {
            int secondsDelay = 1;

            int count = 0;
            #region find count
            try
            {
                //remove Exceptions older than an hour
                for (int i = exceptionLst.Count; i < 0; i--)
                {
                    if (exceptionLst[i].Time < DateTime.Now.AddHours(-1))
                        exceptionLst.RemoveAt(i);
                }

                //keep only the last 10 Exceptions
                if (exceptionLst.Count > 10)
                {
                    exceptionLst.RemoveAt(0);
                }

                //find highest court of the same Exception
                if (exceptionLst.Count > 1)
                {
                    foreach (ExceptionClass exCls in exceptionLst)
                    {
                        if (exceptionClass.Description == exCls.Description)
                        {
                            count++;
                        }
                    }
                }
            }
            catch { }
            #endregion

            LogStandard(count + ". time the same Exception, within the last hour");
            if (count == 2) secondsDelay = 6;
            if (count == 3) secondsDelay = 60;
            if (count == 4) secondsDelay = 600;
            if (count > 4) throw new Exception("The same Exception repeated to many times (5+) within one hour");
            return secondsDelay;
        }

        private void LogLogic(LogEntry logEntry)
        {
            string reply = "";
            string entry = "";

            if (logLevel >= logEntry.Level)
            {
                LogCache(logEntry);
                reply = logWriter.WriteLogEntry(logEntry);
            }

            if (reply != "")
            {
                reply += Environment.NewLine;

                foreach (LogEntry item in logQue)
                {
                    entry = item.Time + " // " + "L:" + item.Level + " // " + item.Content;
                    reply += Environment.NewLine + entry;
                }

                logWriter.WriteIfFailed(reply);
            }
        }

        private void LogCache(LogEntry logEntry)
        {
            try
            {
                if (logQue.Count == 11)
                    logQue.Dequeue();

                logQue.Enqueue(logEntry);
            }
            catch { }
        }
        #endregion
    }

    public class CoreBase
    {
        public virtual void FatalExpection(string reason, Exception exception)
        {
            throw new Exception("CoreBase." + "FatalExpection" + " method should never actually be called. Core should override");
        }

        public virtual void Restart(int secondsDelay)
        {
            throw new Exception("CoreBase." + "Restart" + " method should never actually be called. Core should override");
        }
    }

    public class LogWriter
    {
        public virtual string WriteLogEntry(LogEntry logEntry)
        {
            throw new Exception("SqlControllerBase." + "LogText" + " method should never actually be called. SqlController should override");
        }

        public virtual void   WriteIfFailed(string str)
        {
            throw new Exception("SqlControllerBase." + "LogText" + " method should never actually be called. SqlController should override");
        }
    }

    public class LogEntry
    {
        public LogEntry (int level, string content)
        {
            Level   = level;
            Content = content;
            Time    = DateTime.Now;
        }

        public int Level { get; }
        public string Content { get; }
        public DateTime Time { get; }
    }
}