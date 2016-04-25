using BuildMonitor.Domain;

namespace BuildMonitor.UnitTests.Fakes
{
    internal class BuildFactoryFake : IBuildFactory
    {
        public ISolutionBuild Build { get; set; }

        public ISolutionBuild CreateSolutionBuild(ISolution solution, bool isRebuildAll)
        {
            return Build;
        }

        public IProjectBuild CreateProjectBuild(IProject project)
        {
            throw new System.NotImplementedException();
        }
    }
}