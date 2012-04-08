using System;
using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public interface IProjectBuild : ITimer, IPersistable
    {
        DateTime Started { get; }
        IProject Project { get; }
    }
}