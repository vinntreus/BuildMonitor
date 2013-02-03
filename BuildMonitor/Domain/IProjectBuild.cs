using System;

namespace BuildMonitor.Domain
{
    public interface IProjectBuild : ITimer, IPersistable
    {
        DateTime Started { get; }
        IProject Project { get; }
    }
}