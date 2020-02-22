/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microting.eForm.Dto;

namespace Microting.eForm
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
            this.core = core;
            this.logWriter = logWriter;
            this.logLevel = logLevel;
            logQue = new Queue();
        }

        #region public
        public async Task LogEverything(string type, string message)
        {
            await LogLogic(new LogEntry(4, type, message));
        }

        #region public void     LogVariable (string type, ... variableName, string variableContent)
        public async Task LogVariable(string type, string variableName, string variableContent)
        {
            if (variableContent == null)
                variableContent = "[null]";

            await LogLogic(new LogEntry(3, type, "Variable Name:" + variableName.ToString() + " / Content:" + variableContent.ToString()));
        }

        public async Task LogVariable(string type, string variableName, int? variableContent)
        {
            await LogVariable(type, variableName, variableContent.ToString());
        }

        public async Task LogVariable(string type, string variableName, bool? variableContent)
        {
            await LogVariable(type, variableName, variableContent.ToString());
        }

        public async Task LogVariable(string type, string variableName, DateTime? variableContent)
        {
            await LogVariable(type, variableName, variableContent.ToString());
        }
        #endregion

        public async Task LogStandard(string type, string message)
        {
            await LogLogic(new LogEntry(2, type, message));
        }

        public async Task LogCritical(string type, string message)
        {
            await LogLogic(new LogEntry(1, type, message));
        }

        public async Task LogWarning(string type, string message)
        {
            await LogLogic(new LogEntry(0, type, message));
        }

        public async Task LogException(string type, string exceptionDescription, Exception exception, bool restartCore)
        {
            try
            {
                string fullExceptionDescription = t.PrintException(exceptionDescription, exception);
                if (fullExceptionDescription.Contains("Message    :Core is not running"))
                    return;

                await LogLogic(new LogEntry(-1, type, fullExceptionDescription));
                await LogVariable(type, nameof(restartCore), restartCore);

                ExceptionClass exCls = new ExceptionClass(fullExceptionDescription, DateTime.Now);
                exceptionLst.Add(exCls);

                int sameExceptionCount = await CheckExceptionLst(exCls);
                int sameExceptionCountMax = 0;

                foreach (var item in exceptionLst)
                    if (sameExceptionCountMax < item.Occurrence)
                        sameExceptionCountMax = item.Occurrence;

                if (restartCore)
                    await core.Restart(sameExceptionCount, sameExceptionCountMax);
            }
            catch { }
        }

        public async Task LogFatalException(string exceptionDescription, Exception exception)
        {
            try
            {
                string fullExceptionDescription = t.PrintException(exceptionDescription, exception);
                await LogLogic(new LogEntry(-3, "FatalException", PrintCache(-3, fullExceptionDescription)));
            }
            catch { }
        }

        public string PrintCache(int level, string initialMessage)
        {
            string text = "";

            foreach (LogEntry item in logQue)
                text = item.Time + " // " + "L:" + item.Level + " // " + item.Message + Environment.NewLine + text;

            text = DateTime.Now + " // L:" + level + " // ###########################################################################" + Environment.NewLine +
                    initialMessage + Environment.NewLine +
                    Environment.NewLine +
                    text + Environment.NewLine +
                    DateTime.Now + " // L:" + level + " // ###########################################################################";

            return text;
        }
        #endregion

        #region private
        private async Task<int> CheckExceptionLst(ExceptionClass exceptionClass)
        {
            int count = 0;
            #region find count
            try
            {
                //remove Exceptions older than an hour
                for (int i = exceptionLst.Count; i < 0; i--)
                    if (exceptionLst[i].Time < DateTime.Now.AddHours(-1))
                        exceptionLst.RemoveAt(i);

                //keep only the last 12 Exceptions
                while (exceptionLst.Count > 12)
                    exceptionLst.RemoveAt(0);

                //find court of the same Exception
                if (exceptionLst.Count > 0)
                {
                    string thisOne = t.Locate(exceptionClass.Description, "######## EXCEPTION FOUND; BEGIN ########", "######## EXCEPTION FOUND; ENDED ########");

                    foreach (ExceptionClass exCls in exceptionLst)
                    {
                        string fromLst = t.Locate(exCls.Description, "######## EXCEPTION FOUND; BEGIN ########", "######## EXCEPTION FOUND; ENDED ########");

                        if (thisOne == fromLst)
                            count++;
                    }
                }
            }
            catch { }
            #endregion

            exceptionClass.Occurrence = count;
            await LogStandard(t.GetMethodName("Log"), count + ". time the same Exception, within the last hour");
            return count;
        }

        private async Task LogLogic(LogEntry logEntry)
        {
            try
            {
                string reply = "";

                LogCache(logEntry);
                if (logLevel >= logEntry.Level)
                    reply = await logWriter.WriteLogEntry(logEntry);

                //if prime log writer failed
                if (reply != "")
                    logWriter.WriteIfFailed(PrintCache(-2, reply));
            }
            catch { }
        }

        private void LogCache(LogEntry logEntry)
        {
            try
            {
                if (logQue.Count == 12)
                    logQue.Dequeue();

                logQue.Enqueue(logEntry);
            }
            catch { }
        }
        #endregion
    }

    public class CoreBase
    {
        public virtual Task Restart(int sameExceptionCount, int sameExceptionCountMax)
        {
            throw new Exception("CoreBase." + "Restart" + " method should never actually be called. Core should override");
        }
    }

    public class LogWriter
    {
#pragma warning disable 1998
        public virtual async Task<string> WriteLogEntry(LogEntry logEntry)
        {
            throw new Exception("SqlControllerBase." + "LogText" + " method should never actually be called. SqlController should override");
        }
#pragma warning restore 1998

        public virtual void WriteIfFailed(string str)
        {
            throw new Exception("SqlControllerBase." + "LogText" + " method should never actually be called. SqlController should override");
        }
    }

    public class LogEntry
    {
        public LogEntry(int level, string type, string message)
        {
            Time = DateTime.Now;
            Level = level;
            Type = type;
            Message = message;
        }

        public DateTime Time { get; }
        public int Level { get; }
        public string Type { get; }
        public string Message { get; }
    }
}