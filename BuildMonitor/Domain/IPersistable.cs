using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public interface IPersistable
    {
        /// <summary>
        /// Persistable data from build
        /// </summary>
        /// <returns></returns>
        object Data();
    }
}