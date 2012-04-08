using System;
using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public interface ISolution
    {
        string Name { get; }
    }

    public class Solution : ISolution
    {
        public string Name { get; set; }
    }
}