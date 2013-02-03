using System;
using System.Collections.Generic;
using BuildMonitor.Domain;

namespace BuildMonitor.UnitTests.Fakes
{
    internal class BuildRepositoryFake : IBuildRepository
    {
        private readonly IList<string> savedData;
        public IEnumerable<string> SavedData { get { return savedData; } }
        public BuildRepositoryFake()
        {
            savedData = new List<string>();
        }

        public int SaveCount { get; private set; }
        public void Save(IPersistable build)
        {
            SaveCount++;
        }

        public void Save(string data)
        {
            if (ThrowOnSave != null)
                throw ThrowOnSave;
            savedData.Add(data);
        }

        public Exception ThrowOnSave { get; set; }

        public string RawData { get; set; }

        public int GetRawCount { get; private set; }

        public string Source { get; set; }

        public string GetRawData()
        {
            GetRawCount++;
            return RawData;
        }
    }
}