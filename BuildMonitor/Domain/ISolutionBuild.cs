using System;

namespace BuildMonitor.Domain
{
    public interface ISolutionBuild : ITimer, IPersistable
    {
        DateTime Started { get; }
        ISolution Solution { get;  }
        bool IsRebuildAll { get; set; } 

        void AddProject(IProjectBuild projectBuild);
    }
}