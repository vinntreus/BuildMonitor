using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public class SolutionBuildData
    {
        public SolutionBuildData(ISolutionBuild solutionBuild, int sessionBuildCount, long sessionMillisecondsElapsed)
        {
            SolutionBuild = solutionBuild;
            SessionBuildCount = sessionBuildCount;
            SessionMillisecondsElapsed = sessionMillisecondsElapsed;
        }

        public string SolutionName
        {
            get { return SolutionBuild.Solution.Name; }
        }

        public long SolutionBuildTime
        {
            get { return SolutionBuild.MillisecondsElapsed; }
        }

        private ISolutionBuild SolutionBuild { get; set; }
        public int SessionBuildCount { get; private set; }
        public long SessionMillisecondsElapsed { get; private set; }
    }
}