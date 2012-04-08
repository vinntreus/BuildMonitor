using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public interface IBuildFactory
    {
        ISolutionBuild CreateSolutionBuild(ISolution solution);
        IProjectBuild CreateProjectBuild(IProject project);
    }
}