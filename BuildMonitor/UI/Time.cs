namespace BuildMonitor.UI
{
    public class Time
    {
        public int Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }
        public int Milliseconds { get; private set; }

        public static Time FromMilliseconds(long ms)
        {
            var seconds = (int)(ms / 1000) % 60;
            var minutes = (int)((ms / (1000 * 60)) % 60);
            var hours = (int)((ms / (1000 * 60 * 60)) % 24);
            var milliseconds = (int)(ms - ((seconds * 1000) + (minutes * 1000 * 60) + (hours * 1000 * 60 * 60)) );
            return new Time(hours, minutes, seconds, milliseconds);
        }

        public Time(int hours, int minutes, int seconds, int milliseconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }
    }
}