using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildMonitor.UnitTests.LocalData
{
    public struct SolutionName
    {
        public string Name;
    }

    public struct SolutionTimes
    {
        public int Time;
        public SolutionName Solution;
        public string Name { get { return Solution.Name; } }
    }

    public class AnalyseBuildTimes
    {
        private string json;

        public TimeSpan Total { get; protected set; }

        public AnalyseBuildTimes(string json)
        {
            this.json = json;

            var d = JsonConvert.DeserializeObject<IEnumerable<SolutionTimes>>(json);
            //var jsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            //jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter());

            Total = TimeSpan.FromMilliseconds(d.Sum(s => s.Time));
        }
    }
}