using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitParser.Helpers
{
    public static class Extensions
    {
        public static string Append(this string source, string addition)
        {
            return string.Format("{0}{1}", source, addition);
        }
    }
}
