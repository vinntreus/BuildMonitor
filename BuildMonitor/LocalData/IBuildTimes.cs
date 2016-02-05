using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitor.LocalData
{
    public interface IBuildTimes
    {
        TimeSpan Total { get; }
        IEnumerable<SolutionMonth> AvailableMonths { get; }
        IEnumerable<string> AvailableSolutions { get; }

        TimeSpan SolutionMonth(string solution, int month, int year);

        TimeSpan Solution(string solution);
    }
}
