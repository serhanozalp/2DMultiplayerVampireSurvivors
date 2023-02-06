using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class StringListExtensions
    {
        public static string ToStringSeperatedByComma(this List<string> stringList)
        {
            return String.Join(',', stringList);
        }
    }
}

