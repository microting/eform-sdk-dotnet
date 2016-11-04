using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trools
{
    class Tools
    {
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
        #endregion
    }
}
