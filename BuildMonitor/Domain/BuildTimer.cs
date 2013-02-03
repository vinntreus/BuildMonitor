using System.Diagnostics;

namespace BuildMonitor.Domain
{
    public class BuildTimer : ITimer
    {
        private readonly Stopwatch stopwatch;
        public BuildTimer()
        {
            stopwatch = new Stopwatch();
        }
        public void Start()
        {
            stopwatch.Restart();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public bool IsRunning
        {
            get { return stopwatch.IsRunning; }
        }

        public long MillisecondsElapsed
        {
            get { return stopwatch.ElapsedMilliseconds; }
        }
    }
}