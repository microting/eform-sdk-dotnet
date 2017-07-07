using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eFormShared
{
    public class Logger
    {
        #region var
        CoreBase core;
        SqlControllerBase sqlController;
        Tools t = new Tools();
        int logLevel;
        List<ExceptionClass> exceptionLst = new List<ExceptionClass>();
        #endregion

        #region con
        public Logger(CoreBase core, SqlControllerBase sqlController, int logLevel)
        {
            try
            {
                this.core = core;
                this.sqlController = sqlController;
                this.logLevel = logLevel;
            }
            catch (Exception ex)
            {
                core.FatalExpection(t.GetMethodName() + "failed", ex);
            }
        }
        #endregion

        #region public log
        public void LogEverything(string str)
        {
            if (logLevel >= 4)
                sqlController.LogText(4, str);
        }

        public void LogVariable(string variableName, string variableContent)
        {
            if (logLevel >= 3)
                sqlController.LogVariable(variableName, variableContent);
        }

        public void LogStandard(string str)
        {
            if (logLevel >= 2)
                sqlController.LogText(2, str);
        }

        public void LogCritical(string str)
        {
            if (logLevel >= 1)
                sqlController.LogText(1, str);
        }

        public void LogWarning(string str)
        {
            if (logLevel >= 0)
                sqlController.LogText(0, str);
        }

        public void LogException(string exceptionDescription, Exception exception, bool restartCore)
        {
            try
            {
                string fullExceptionDescription = t.PrintException(exceptionDescription, exception);
                sqlController.LogText(-1, fullExceptionDescription);

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

        //private
        private int CheckExceptionLst(ExceptionClass exceptionClass)
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

    public class SqlControllerBase
    {
        public virtual void LogText(int level, string str)
        {
            throw new Exception("SqlControllerBase." + "LogText" + " method should never actually be called. SqlController should override");
        }

        public virtual void LogVariable(string variableName, string variableContent)
        {
            throw new Exception("SqlControllerBase." + "LogVariable" + " method should never actually be called. SqlController should override");
        }
    }
}