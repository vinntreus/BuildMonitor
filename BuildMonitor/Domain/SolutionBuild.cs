using System.Linq;
using System.Collections.Generic;
using System;

namespace BuildMonitor.Domain
{
    public class SolutionBuild : Build, ISolutionBuild
    {
        private readonly IList<IProjectBuild> projects;
        public ISolution Solution { get; private set; }

        public bool IsRebuildAll { get; set;  }
  
        public void AddProject(IProjectBuild projectBuild)
        {
            projects.Add(projectBuild);
        }

        public SolutionBuild(ITimer timer, ISolution solution) : base(timer)
        {
            Solution = solution;
            projects = new List<IProjectBuild>();
        }

        public object Data()
        {
            return new
            {
                Start = Started,
                Time = MillisecondsElapsed,
                Solution,
                Projects = projects.Select(p => p.Data())
            };
        }
    }
}