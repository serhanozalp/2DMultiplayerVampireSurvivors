using System.Collections.Generic;

namespace Extensions
{
    public static class StringListExtensions
    {
        public static string ToStringSeperatedByComma(this List<string> stringList)
        {
            string stringSeperatedByComma = "";
            foreach (var stringItem in stringList)
            {
                stringSeperatedByComma += stringItem + ",";
            }
            return stringSeperatedByComma;
        }
    }
}

