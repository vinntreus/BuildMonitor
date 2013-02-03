namespace BuildMonitor.Domain
{
    public interface IBuildFactory
    {
        ISolutionBuild CreateSolutionBuild(ISolution solution);
        IProjectBuild CreateProjectBuild(IProject project);
    }
}