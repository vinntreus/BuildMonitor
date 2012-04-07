using BuildMonitor.Domain;

namespace BuildMonitor.UnitTests.Fakes
{
    internal class BuildRepositoryFake : IBuildRepository
    {
        public int SaveCount { get; private set; }
        public void Save(IPersistable build)
        {
            SaveCount++;
        }
    }
}