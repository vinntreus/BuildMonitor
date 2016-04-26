using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitor.LocalData
{
    public interface IBuildTimes
    {
        IEnumerable<ExpandoObject> SolutionMonthTable();

        // these are only used in testing, but I can't think of a good way to take them out of the interface and keep the rest of the code ok.
        TimeSpan Total { get; }
        IEnumerable<SolutionMonth> AvailableMonths { get; }
        IEnumerable<string> AvailableSolutions { get; }
        TimeSpan SolutionMonth(string solution, int month, int year);
        TimeSpan Solution(string solution);
    }
}
