using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitor.LocalData
{
    internal struct JSONSolutionTimes
    {
        public int Time;
        public DateTime Start;
        public JSONSolutionName Solution;
        public string Name { get { return Solution.Name; } }
    }
}
