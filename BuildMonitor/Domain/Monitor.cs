using System;
using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public class Monitor
    {
        private readonly IBuildFactory buildFactory;
        private readonly IBuildRepository buildRepository;
        private ISolutionBuild solutionBuild;
        private int buildCount;
        private long sessionMillisecondsElapsed;

        public Monitor(IBuildFactory buildFactory, IBuildRepository buildRepository)
        {
            this.buildFactory = buildFactory;
            this.buildRepository = buildRepository;
        }

        public Action<SolutionBuildData> SolutionBuildFinished;

        public void SolutionBuildStart(ISolution solution)
        {
            if(solutionBuild != null && solutionBuild.IsRunning)
                throw new InvalidOperationException("There is already a build running");

            solutionBuild = buildFactory.CreateSolutionBuild(solution);

            solutionBuild.Start();
        }

        public void SolutionBuildStop()
        {
            if (solutionBuild != null && solutionBuild.IsRunning)
            {
                solutionBuild.Stop();
                buildRepository.Save(solutionBuild);
                if (SolutionBuildFinished != null)
                {
                    SolutionBuildFinished(new SolutionBuildData(solutionBuild, ++buildCount, sessionMillisecondsElapsed += solutionBuild.MillisecondsElapsed));
                }
            }
        }
    }
}