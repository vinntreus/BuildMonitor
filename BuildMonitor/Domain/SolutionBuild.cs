using System;
using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public class SolutionBuild : ISolutionBuild
    {
        private readonly ITimer timer;

        public DateTime Started { get; private set; }
        public long MillisecondsElapsed { get; private set; }
        public ISolution Solution { get; private set; }

        public SolutionBuild(ITimer timer, ISolution solution)
        {
            this.timer = timer;
            Solution = solution;
        }

        public void Start()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Solution build is running!");
            }

            Started = DateTime.Now;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            MillisecondsElapsed = timer.MillisecondsElapsed;
        }

        public bool IsRunning
        {
            get { return timer.IsRunning; }
        }

        public object Data()
        {
            return new
            {
                Start = Started,
                Time = MillisecondsElapsed,
                Solution
            };
        }
    }
}