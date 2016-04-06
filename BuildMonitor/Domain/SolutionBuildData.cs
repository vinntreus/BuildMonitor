namespace BuildMonitor.Domain
{
    public class SolutionBuildData
    {
        public SolutionBuildData(ISolutionBuild solutionBuild, bool isRebuildAll, int sessionBuildCount, long sessionMillisecondsElapsed)
        {
            SolutionBuild = solutionBuild;
            IsRebuildAll = isRebuildAll;
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
        public bool IsRebuildAll { get; } 
        public int SessionBuildCount { get; private set; }
        public long SessionMillisecondsElapsed { get; private set; }
    }
}