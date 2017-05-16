using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eFormShared
{
    public class Tools
    {
        #region Entity Framework Get
        public bool Bool(short? input)
        {
            if (input == null)
                return false;
            string temp = input.ToString();
            if (temp == "0" || temp.ToLower() == "false" || temp == "")
                return false;
            if (temp == "1" || temp.ToLower() == "true")
                return true;
            throw new Exception(temp + ": was not found to be a bool");
        }

        public bool Bool(string input)
        {
            if (input == null)
                return false;
            if (input == "0" || input.ToLower() == "false" || input == "")
                return false;
            if (input == "1" || input.ToLower() == "true")
                return true;
            throw new Exception(input + ": was not found to be a bool");
        }

        public short Bool(bool inputBool)
        {
            if (inputBool == false)
                return 0;
            else
                return 1;
        }

        public int Int(int? input)
        {
            if (input == null)
                return 0;

            string str = input.ToString();
            return int.Parse(str);
        }

        public int Int(string input)
        {
            return int.Parse(input);
        }

        public DateTime? Date(string input)
        {
            if (input == "")
                return null;
            else
                return DateTime.Parse(input);
        }

        public DateTime Date(DateTime? input)
        {
            if (input != null)
                return (DateTime)input;
            else
                return DateTime.MinValue;
        }

        public string IntLst(List<int> siteIds)
        {
            if (siteIds == null)
                return null;

            if (siteIds.Count == 0)
                return "";

            string joined = string.Join(",", siteIds);
            return joined;
        }

        public List<int> IntLst(string str)
        {
            if (str == null)
                return null;

            List<int> intLst = new List<int>();

            if (str == "")
                return intLst;

            intLst = str.Split(',').Select(int.Parse).ToList();
            return intLst;
        }

        public string TextLst(List<string> strLst)
        {
            if (strLst == null)
                return null;

            if (strLst.Count == 0)
                return "";

            string joined = string.Join(",", strLst);
            return joined;
        }

        public List<string> TextLst(string str)
        {
            if (str == null)
                return null;

            List<string> strLst = new List<string>();

            if (str == "")
                return strLst;

            strLst = str.Split(',').ToList();
            return strLst;
        }
        #endregion

        #region Text Manipulation
        public string           Locate(string textStr, string startStr, string endStr)
        {
            try
            {
                if (!textStr.Contains(startStr))
                    return "";

                if (!textStr.Contains(endStr))
                    return "";

                int startIndex = textStr.IndexOf(startStr) + startStr.Length;
                int lenght = textStr.IndexOf(endStr, startIndex) - startIndex;
                return textStr.Substring(startIndex, lenght);
            }
            catch
            {
                return "";
            }
        }

        public List<string>     LocateList(string textStr, string startStr, string endStr)
        {
            try
            {
                if (!textStr.Contains(startStr))
                    return null;

                if (!textStr.Contains(endStr))
                    return null;

                List<string> lst = new List<string>();

                int marker = 0;
                bool flag = true;
                string temp = "";

                while (flag)
                {
                    temp = Locate(textStr, startStr, endStr);
                    if (temp != "")
                    {
                        lst.Add(temp);
                        marker = textStr.IndexOf(startStr);
                        marker = textStr.IndexOf(endStr, marker) + endStr.Length;
                        textStr = textStr.Remove(0, marker);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                return lst;
            }
            catch
            {
                return null;
            }
        }

        public string           LocateReplace(string textStr, string startStr, string endStr, string newStr)
        {
            try
            {
                if (!textStr.Contains(startStr))
                    return "";

                if (!textStr.Contains(endStr))
                    return "";

                int startIndex = textStr.IndexOf(startStr) + startStr.Length;
                int lenght = textStr.IndexOf(endStr, startIndex) - startIndex;
                textStr = textStr.Substring(0, startIndex) + newStr + textStr.Substring(startIndex + lenght);
                return textStr;
            }
            catch
            {
                return "";
            }
        }

        public string           LocateReplaceAll(string textStr, string startStr, string endStr, string newStr)
        {
            try
            {
                if (!textStr.Contains(startStr))
                    return null;

                if (!textStr.Contains(endStr))
                    return null;

                string returnStr = "";

                int marker = 0;
                bool flag = true;
                string temp;

                while (flag)
                {
                    temp = LocateReplace(textStr, startStr, endStr, newStr);

                    if (temp != textStr && temp != "")
                    {
                        marker = temp.IndexOf(startStr);
                        marker = temp.IndexOf(endStr, marker) + endStr.Length;

                        returnStr += temp.Substring(0, marker);

                        textStr = temp.Remove(0, marker);
                    }
                    else
                    {
                        returnStr += textStr;
                        flag = false;
                    }
                }
                return returnStr;
            }
            catch
            {
                return null;
            }
        }

        public string           ReadFirst(string textStr, string startStr, string endStr, bool keepStartAndEnd)
        {
            try
            {
                int startIndex = textStr.IndexOf(startStr) + startStr.Length;
                int lenght = textStr.IndexOf(endStr, startIndex) - startIndex;
                if (keepStartAndEnd)
                    return startStr + textStr.Substring(startIndex, lenght) + endStr;
                else
                    return textStr.Substring(startIndex, lenght).Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to find:'" + startStr + "' or '" + endStr + "'.", ex);
            }
        }

        public string           SplitToList(string textToBeSplit, byte index)
        {
            try
            {
                if (string.IsNullOrEmpty(textToBeSplit))
                    throw new ArgumentException("SplitFirst failed, due to textToBeSplit:'" + textToBeSplit.ToString() + "'");

                if (!textToBeSplit.Contains('|'))
                    throw new ArgumentException("SplitFirst failed, due to '|' not found in textToBeSplit");

                List<string> partsLst = textToBeSplit.Split('|').ToList();

                if (partsLst.Count == 2)
                    return partsLst[index];

                //Not two resultats...
                //more logic needed 

                throw new ArgumentException("SplitFirst failed, due to count != 2");
            }
            catch (Exception ex)
            {
                throw new Exception("SplitFirst failed.", ex);
            }
        }

        public string           SplitToList(string textToBeSplit, byte index, bool lastInstedOfFirst)
        {
            try
            {
                if (string.IsNullOrEmpty(textToBeSplit))
                    throw new ArgumentException("SplitFirst failed, due to textToBeSplit:'" + textToBeSplit.ToString() + "'");

                if (!textToBeSplit.Contains('|'))
                    throw new ArgumentException("SplitFirst failed, due to '|' not found in textToBeSplit");

                List<string> partsLst = textToBeSplit.Split('|').ToList();

                if (partsLst.Count == 2)
                    return partsLst[index];

                //Not two resultats... More logic needed 
                if (partsLst.Count < 2)
                    throw new ArgumentException("SplitFirst failed, due to count < 2");

                int mark = -1;
                if (lastInstedOfFirst)
                    mark = textToBeSplit.LastIndexOf('|');
                else
                    mark = textToBeSplit.IndexOf('|');

                if (index == 0)
                    return textToBeSplit.Substring(0, mark);
                else
                    return textToBeSplit.Remove(0, mark + 1);
            }
            catch (Exception ex)
            {
                throw new Exception("SplitFirst failed.", ex);
            }
        }
        #endregion

        #region PrintException
        public string PrintException(string exceptionDescription, Exception ex)
        {
            string fullExceptionDescription = "";

            if (exceptionDescription == null)
                exceptionDescription = "";

            fullExceptionDescription =
                "" + Environment.NewLine +
                "######## " + exceptionDescription + Environment.NewLine +
                "######## " + DateTime.Now.ToString("yyyy MM/dd HH.mm:ss") + Environment.NewLine +
                "######## EXCEPTION FOUND; BEGIN ########" + Environment.NewLine +
                PrintInnerException(ex, 1) + Environment.NewLine +
                "######## EXCEPTION FOUND; ENDED ########" + Environment.NewLine +
                "";

            return fullExceptionDescription;
        }

        private string PrintInnerException(Exception ex, int level)
        {
            if (ex == null)
                return "";

            return
            "######## -Expection at level " + level + "- ########" + Environment.NewLine +
            "Message    :" + ex.Message + Environment.NewLine +
            "Source     :" + ex.Source + Environment.NewLine +
            "StackTrace :" + ex.StackTrace + Environment.NewLine +
            PrintInnerException(ex.InnerException, level + 1).TrimEnd();
        }
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetMethodName()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}