using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitor.LocalData
{
    public class BuildTimes : IBuildTimes
    {
        public TimeSpan Total { get; protected set; }

        public IEnumerable<SolutionMonth> AvailableMonths
        {
            get
            {
                return this.solutionMonths.Keys;
            }
        }

        public IEnumerable<string> AvailableSolutions
        {
            get
            {
                return this.solutions.Keys;
            }
        }

        public TimeSpan SolutionMonth(string solution, int month, int year)
        {
            var solutionMonth = new SolutionMonth(solution: solution, month: month, year: year);

            return this.solutionMonths.ContainsKey(solutionMonth) ? this.solutionMonths[solutionMonth] : TimeSpan.FromSeconds(0);
        }

        public TimeSpan Solution(string solution)
        {
            return this.solutions[solution];
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
