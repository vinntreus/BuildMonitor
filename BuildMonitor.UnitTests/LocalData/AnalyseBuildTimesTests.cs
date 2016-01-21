using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BuildMonitor.LocalData;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace BuildMonitor.UnitTests.LocalData
{
    [TestFixture]
    public class AnalyseBuildTimesTests
    {
        [TestCase(19366)]
        [TestCase(0)]
        [TestCase(395)]
        public void OneBuildTimeTotal(int SingleBuildTimeInMilliseconds)
        {
            var json = @"[{
  'Start': '2016 - 01 - 21T18: 53:33.1003757 + 00:00',
  'Time': " + SingleBuildTimeInMilliseconds.ToString() + @",
  'Solution': {
                'Name': 'BuildMonitor'
  },
  'Projects': [{
      'Start': '2016-01-21T18:53:33.6172723+00:00',
      'Time': 2,
      'Project': {
        'Name': 'BuildMonitor',
        'Id': '700a9d28-e9de-428b-a8f2-b40baf4a3e87'
      }
    }]
}]";

            var calculated = new AnalyseBuildTimes(json).Total;

            Assert.That(calculated == TimeSpan.FromMilliseconds(SingleBuildTimeInMilliseconds));
        }

        // add multiple builds
        // add multiple solutions and per solution breakdown
        // add multiple builds over multiple months per month and per solution breakdown

    }
}
