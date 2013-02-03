namespace BuildMonitor.Domain
{
    public interface ITimer
    {
        void Start();
        void Stop();
        bool IsRunning { get; }
        long MillisecondsElapsed { get; }
    }
}