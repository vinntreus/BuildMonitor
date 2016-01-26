using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitor.LocalData
{
    public interface IBuildTimes
    {
        TimeSpan Total { get; }

        TimeSpan SolutionMonth(string solution, int month, int year);

        TimeSpan Solution(string solution);
    }
}
