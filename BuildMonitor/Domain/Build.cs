using System;
using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public class Build
    {
        protected readonly ITimer timer;

        public DateTime Started { get; private set; }
        public long MillisecondsElapsed { get; private set; }

        public Build(ITimer timer)
        {
            this.timer = timer;
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
    }
}