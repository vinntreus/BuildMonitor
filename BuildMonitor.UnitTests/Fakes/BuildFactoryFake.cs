using BuildMonitor.Domain;

namespace BuildMonitor.UnitTests.Fakes
{
    internal class BuildFactoryFake : IBuildFactory
    {
        public ISolutionBuild Build { get; set; }

        public ISolutionBuild CreateSolutionBuild(Solution solution)
        {
            return Build;
        }
    }
}