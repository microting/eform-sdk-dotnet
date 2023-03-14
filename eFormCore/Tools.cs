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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Microting.eForm
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
            return DateTime.Parse(input);
        }

        public DateTime Date(DateTime? input)
        {
            if (input != null)
                return (DateTime)input;
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

            foreach (var item in strLst)
            {
                if (item.Contains("|"))
                    throw new Exception("strings in the list, may not contain '|'");
            }

            string joined = string.Join("|", strLst);
            return joined;
        }

        public List<string> TextLst(string str)
        {
            if (str == null)
                return null;

            List<string> strLst = new List<string>();

            if (str == "")
                return strLst;

            strLst = str.Split('|').ToList();
            return strLst;
        }

        #endregion

        #region Text Manipulation

        public string Locate(string textStr, string startStr, string endStr)
        {
            try
            {
                if (!textStr.Contains(startStr))
                    return "";

                if (!textStr.Contains(endStr))
                    return "";

                int startIndex = textStr.IndexOf(startStr, StringComparison.Ordinal) + startStr.Length;
                int length = textStr.IndexOf(endStr, startIndex, StringComparison.Ordinal) - startIndex;
                //return textStr.Substring(startIndex, lenght);
                return textStr.AsSpan().Slice(start: startIndex, length).ToString();
            }
            catch
            {
                return "";
            }
        }

        public List<string> LocateList(string textStr, string startStr, string endStr)
        {
            try
            {
                if (!textStr.Contains(startStr))
                    return new List<string>();

                if (!textStr.Contains(endStr))
                    return new List<string>();

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
                        flag = false;
                }

                return lst;
            }
            catch
            {
                return null;
            }
        }

        // public string ReplaceInsensitive(string textStr, string oldStr, string newStr)
        // {
        //     try
        //     {
        //         string textStrLower = textStr.ToLower();
        //         string oldStrLower = oldStr.ToLower();
        //
        //         if (!textStrLower.Contains(oldStrLower))
        //             return textStr;
        //
        //         int startIndex;
        //         int marker = 0;
        //         int newMarker = 0;
        //         bool flag = true;
        //
        //         while (flag)
        //         {
        //             string temp = textStrLower.Remove(0, marker);
        //             if (!temp.Contains(oldStrLower))
        //                 break;
        //
        //             startIndex = textStrLower.IndexOf(oldStrLower, marker, StringComparison.Ordinal);
        //             //textStr = textStr.Substring(0, startIndex) + newStr + textStr.Substring(startIndex + oldStrLower.Length);
        //             textStr = textStr.AsSpan().Slice(0, startIndex).ToString() + newStr +
        //                       textStr.AsSpan().Slice(startIndex + oldStrLower.Length).ToString();
        //
        //             newMarker = startIndex + newStr.Length;
        //
        //             if (newMarker == marker)
        //                 flag = false;
        //             else
        //                 marker = newMarker;
        //         }
        //         return textStr;
        //     }
        //     catch
        //     {
        //         return null;
        //     }
        // }

        // public string ReplaceAtLocation(string textStr, string startStr, string endStr, string newStr, bool caseSensitive)
        // {
        //     try
        //     {
        //         if (caseSensitive)
        //         {
        //             if (!textStr.Contains(startStr))
        //                 return textStr;
        //
        //             if (!textStr.Contains(endStr))
        //                 return textStr;
        //
        //             int startIndex = textStr.IndexOf(startStr) + startStr.Length;
        //             int lenght = textStr.IndexOf(endStr, startIndex) - startIndex;
        //             //textStr = textStr.Substring(0, startIndex) + newStr + textStr.Substring(startIndex + lenght);
        //             textStr = textStr.AsSpan().Slice(0, startIndex).ToString() + newStr +
        //                       textStr.AsSpan().Slice(startIndex + lenght).ToString();
        //             return textStr;
        //         }
        //         else
        //         {
        //             string textStrLower = textStr.ToLower();
        //             string startStrLower = startStr.ToLower();
        //             string endStrLower = endStr.ToLower();
        //
        //             if (!textStrLower.Contains(startStrLower))
        //                 return textStr;
        //
        //             if (!textStrLower.Contains(endStrLower))
        //                 return textStr;
        //
        //             int startIndex = textStrLower.IndexOf(startStrLower) + startStrLower.Length;
        //             int lenght = textStrLower.IndexOf(endStrLower, startIndex) - startIndex;
        //             //textStr = textStr.Substring(0, startIndex) + newStr + textStr.Substring(startIndex + lenght);
        //             textStr = textStr.AsSpan().Slice(0, startIndex).ToString() + newStr +
        //                       textStr.AsSpan().Slice(startIndex + lenght).ToString();
        //             return textStr;
        //         }
        //     }
        //     catch
        //     {
        //         return null;
        //     }
        // }

        // public string ReplaceAtLocationAll(string textStr, string startStr, string endStr, string newStr, bool caseSensitive)
        // {
        //     try
        //     {
        //         string returnStr = "";
        //         string txtToBeProcessed = textStr;
        //         string startStrLow = startStr.ToLower();
        //         string endStrLow = endStr.ToLower();
        //         int marker = 0;
        //
        //         while (txtToBeProcessed.ToLower().Contains(startStrLow) && txtToBeProcessed.ToLower().Contains(endStrLow))
        //         {
        //             marker = txtToBeProcessed.ToLower().IndexOf(endStrLow) + endStrLow.Length;
        //
        //             //string txtBit = txtToBeProcessed.Substring(0, marker);
        //             string txtBit = textStr.AsSpan().Slice(0, marker).ToString();
        //
        //             returnStr += ReplaceAtLocation(txtBit, startStr, endStr, newStr, caseSensitive);
        //             txtToBeProcessed = txtToBeProcessed.Remove(0, marker);
        //         }
        //
        //         returnStr += txtToBeProcessed;
        //         return returnStr;
        //     }
        //     catch
        //     {
        //         return null;
        //     }
        // }

        // public string ReadFirst(string textStr, string startStr, string endStr, bool keepStartAndEnd)
        // {
        //     try
        //     {
        //         int startIndex = textStr.IndexOf(startStr) + startStr.Length;
        //         int lenght = textStr.IndexOf(endStr, startIndex) - startIndex;
        //         if (keepStartAndEnd)
        //             return startStr + textStr.Substring(startIndex, lenght) + endStr;
        //         return textStr.Substring(startIndex, lenght).Trim();
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception("Unable to find:'" + startStr + "' or '" + endStr + "'.", ex);
        //     }
        // }

        // public string SplitToList(string textToBeSplit, byte index)
        // {
        //     try
        //     {
        //         if (string.IsNullOrEmpty(textToBeSplit))
        //             throw new ArgumentException("SplitFirst failed, due to textToBeSplit:'" + textToBeSplit + "'");
        //
        //         if (!textToBeSplit.Contains('|'))
        //             throw new ArgumentException("SplitFirst failed, due to '|' not found in textToBeSplit");
        //
        //         List<string> partsLst = textToBeSplit.Split('|').ToList();
        //
        //         if (partsLst.Count == 2)
        //             return partsLst[index];
        //
        //         //Not two resultats...
        //         //more logic needed
        //
        //         throw new ArgumentException("SplitFirst failed, due to count != 2");
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception("SplitFirst failed.", ex);
        //     }
        // }

        public string SplitToList(string textToBeSplit, byte index, bool lastInstedOfFirst)
        {
            try
            {
                if (string.IsNullOrEmpty(textToBeSplit))
                    throw new ArgumentException("SplitFirst failed, due to textToBeSplit:'" + textToBeSplit + "'");

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
                "######## " + DateTime.UtcNow.ToString("yyyy MM/dd HH.mm:ss") + Environment.NewLine +
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
                "Type    :" + ex.GetType().Name + Environment.NewLine +
                "Message    :" + ex.Message + Environment.NewLine +
                "Source     :" + ex.Source + Environment.NewLine +
                "StackTrace :" + ex.StackTrace + Environment.NewLine +
                PrintInnerException(ex.InnerException, level + 1).TrimEnd();
        }

        #endregion

        public string GetRandomString(int lenght)
        {
            if (1 > lenght || lenght > 16)
                throw new ArgumentOutOfRangeException("lenght needs to between 1-16");

            string str = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            return str.Substring(0, lenght);
        }

        public int GetRandomInt(int lenght)
        {
            if (1 > lenght || lenght > 16)
                throw new ArgumentOutOfRangeException("lenght needs to between 1-16");

            string str = GetRandomString(lenght);
            str = str.GetHashCode().ToString().Replace("-", "") + "413165131968413";
            return int.Parse(str.Substring(0, lenght));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetMethodName(string className)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return className + "." + sf.GetMethod().Name;
        }
    }
}