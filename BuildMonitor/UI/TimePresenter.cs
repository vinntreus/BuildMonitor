using System.Globalization;

namespace BuildMonitor.UI
{
    public class TimePresenter
    {
        private readonly Time time;

        public TimePresenter(Time time)
        {
            this.time = time;
        }

        public string Display(string format = "{0}h {1}m {2}s {3}ms")
        {
            return string.Format(format, GetHours, GetMinutes, GetSeconds, GetMilliseconds);
        }

        private string GetMilliseconds{get
        {
            if (time.Milliseconds > 99)
                return time.Milliseconds.ToString(CultureInfo.InvariantCulture);
            if(time.Milliseconds > 9)
                return  "0" + time.Milliseconds;

            return "00" + time.Milliseconds;
        }}

        protected string GetSeconds { get { return GetFormatedValue(time.Seconds); } }

        protected string GetMinutes { get { return GetFormatedValue(time.Minutes); } }

        protected string GetHours { get { return GetFormatedValue(time.Hours); } }

        private static string GetFormatedValue(int value)
        {
            return value > 9 ? value.ToString(CultureInfo.InvariantCulture) : "0" + value;
        }
    }
}