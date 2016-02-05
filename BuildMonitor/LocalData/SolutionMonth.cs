using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitor.LocalData
{
    public class SolutionMonth 
    {
        private readonly string solution;
        private readonly int year;
        private readonly int month;

        public SolutionMonth(string solution = "", int year = 0, int month = 0)
        {
            this.solution = solution;
            this.year = year;
            this.month = month;
        }

        public string Solution { get { return this.solution; } }
        public int Year { get { return this.year; } }
        public int Month { get { return this.month; } }

        public override bool Equals(object obj)
        {
            if (!(obj is SolutionMonth))
                return base.Equals(obj);

            var other = (obj as SolutionMonth);

            return (Solution == other.Solution && Year == other.Year && Month == other.Month);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Solution.GetHashCode();
                hash = hash * 23 + Year.GetHashCode();
                hash = hash * 23 + Month.GetHashCode();
                return hash;
            }
        }
    }
}
