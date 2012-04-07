using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public interface IBuildRepository
    {
        void Save(IPersistable build);
    }
}