namespace BuildMonitor.UI
{
    public static class LongExtensions
    {
        public static string ToTime(this long ms)
        {
            return new TimePresenter(Time.FromMilliseconds(ms)).Display();
        }
    }
}