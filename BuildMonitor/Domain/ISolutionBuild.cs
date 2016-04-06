using System;

namespace BuildMonitor.Domain
{
    public interface ISolutionBuild : ITimer, IPersistable
    {
        DateTime Started { get; }
        ISolution Solution { get;  }

        void AddProject(IProjectBuild projectBuild);
    }
}