using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class StringExtensions
    {
        public static List<string> ToListSplitByComma(this string stringObject)
        {
            List<string> stringList = new List<string>();
            foreach (var stringItem in stringObject.Split(','))
            {
                if (!String.IsNullOrEmpty(stringItem)) stringList.Add(stringItem);
            }
            return stringList;
        }
    }
}

