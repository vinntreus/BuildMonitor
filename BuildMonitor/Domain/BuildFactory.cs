namespace BuildMonitor.Domain
{
    public class BuildFactory : IBuildFactory
    {
        public ISolutionBuild CreateSolutionBuild(ISolution solution)
        {
            return new SolutionBuild(new BuildTimer(), solution);
        }

        public IProjectBuild CreateProjectBuild(IProject project)
        {
            return new ProjectBuild(new BuildTimer(), project);
        }
    }
}