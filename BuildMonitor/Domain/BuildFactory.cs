using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public class BuildFactory : IBuildFactory
    {
        public ISolutionBuild CreateSolutionBuild(Solution solution)
        {
            return new SolutionBuild(new BuildTimer(), solution);
        }
    }
}