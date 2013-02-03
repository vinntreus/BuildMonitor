namespace BuildMonitor.Domain
{
    public interface IBuildRepository
    {
        void Save(IPersistable build);
        void Save(string data);
        string GetRawData();
        string Source { get; }
    }
}