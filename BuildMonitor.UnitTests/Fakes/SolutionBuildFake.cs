using System;
using BuildMonitor.Domain;

namespace BuildMonitor.UnitTests.Fakes
{
    internal class SolutionBuildFake : ISolutionBuild
    {
        public int StartedCount { get; private set; }
        public int StopCount { get; private set; }

        public void Start()
        {
            StartedCount++;
            IsRunning = true;
        }

        public void Stop()
        {
            StopCount++;
            IsRunning = false;
        }

        public bool IsRunning { get; private set; }
        public object Data()
        {
            return new {};
        }

        public DateTime Started { get; set; }

        public long MillisecondsElapsed { get; set; }

        public ISolution Solution { get; set; }

        public bool IsRebuildAll
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddProject(IProjectBuild projectBuild)
        {
            
        }
    }
}