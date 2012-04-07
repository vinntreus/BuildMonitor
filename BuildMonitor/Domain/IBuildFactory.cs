using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public interface IBuildFactory
    {
        ISolutionBuild CreateSolutionBuild(Solution solution);
    }
}