using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitor.LocalData
{
    public class BuildTimes : IBuildTimes
    {
        public BuildTimes(TimeSpan total, Dictionary<string, TimeSpan> solutions, Dictionary<SolutionMonth, TimeSpan> solutionMonths)
        {
            Total = total;
            this.solutions = solutions;
            this.solutionMonths = solutionMonths;
        }

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

        // this is for the UI, in MVC land it would be a ViewModel, but that seems a bit excessive here.
        public IEnumerable<ExpandoObject> SolutionMonthTable()
        {
            var table = new List<ExpandoObject>();

            foreach (var solution in AvailableSolutions)
            {
                dynamic row = new ExpandoObject();
                var p = row as IDictionary<string, object>;
                p["Solution"] = solution;
                foreach (var month in AvailableMonths)
                {
                    var buildTime = SolutionMonth(solution, month.Month, month.Year);

                    if (buildTime == TimeSpan.FromSeconds(0))
                        p[$"{month.Month:mmm} {month.Year}"] = "";
                    else
                        p[$"{month.Month:mmm} {month.Year}"] = $@"{buildTime:hh\:mm\:ss}";
                }

                table.Add(row);
           }

           return table;
        }

        private Dictionary<SolutionMonth, TimeSpan> solutionMonths;
        private Dictionary<string, TimeSpan> solutions;
    }
}
