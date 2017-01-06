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
        #endregion
    }
}
