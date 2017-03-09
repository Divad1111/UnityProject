using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UICodeGenerator
{
    public class Utility
    {
        public static string SubString(string strSrc, char chLeft, char chRight)
        {
            int nLeft = strSrc.IndexOf(chLeft);
            int nRight = strSrc.IndexOf(chRight);
            if (nLeft >= nRight)
            {
                return string.Empty;
            }
            return strSrc.Substring(nLeft + 1, nRight - nLeft - 1);
        }
    }

}
