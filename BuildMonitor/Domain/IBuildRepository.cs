namespace BuildMonitor.Domain
{
    public interface IBuildRepository
    {
        void Save(IPersistable build);
    }
}