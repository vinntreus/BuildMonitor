using System;
using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public struct Solution
    {
        public readonly Guid Id;
        public readonly string Name;

        public Solution(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}