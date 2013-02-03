namespace BuildMonitor.Domain
{
    public class ProjectBuild : Build, IProjectBuild
    {
        public IProject Project { get; private set; }

        public ProjectBuild(ITimer timer, IProject project) : base(timer)
        {
            Project = project;
        }

        public object Data()
        {
            return new
            {
                Start = Started,
                Time = MillisecondsElapsed,
                Project
            };
        }
    }
}