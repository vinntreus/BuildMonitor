using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitor.LocalData
{
    internal class BuildTimes : IBuildTimes
    {
        public TimeSpan Total { get; protected set; }

        public TimeSpan SolutionMonth(string solution, int month, int year)
        {
            return solutionMonths[new SolutionMonth() { Solution = solution, Month = month, Year = year }];
        }

        public TimeSpan Solution(string solution)
        {
            return solutions[solution];
        }

        private Dictionary<SolutionMonth, TimeSpan> solutionMonths;
        private Dictionary<string, TimeSpan> solutions;

        public BuildTimes(TimeSpan total, Dictionary<string, TimeSpan> solutions, Dictionary<SolutionMonth, TimeSpan> solutionMonths)
        {
            Total = total;
            this.solutions = solutions;
            this.solutionMonths = solutionMonths;
        }
    }
}
