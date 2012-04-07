using BuildMonitor.Domain;

namespace BuildMonitor.UnitTests.Fakes
{
    public class TimerFake : ITimer
    {
        public int StartedCount { get; set; }
        public int StopCount { get; set; }

        public void Start()
        {
            StartedCount++;
        }

        public void Stop()
        {
            StopCount++;
        }
        public bool IsRunning { get; set; }
        public long MillisecondsElapsed { get; set; }
    }
}