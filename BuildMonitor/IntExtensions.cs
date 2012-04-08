using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor
{
    public static class IntExtensions
    {
        public static string Times(this int i, string s)
        {
            return string.Join("", Enumerable.Range(0, i).Select(d => s));
        }
    }
}