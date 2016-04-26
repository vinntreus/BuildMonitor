using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BuildMonitor.LocalData;

namespace BuildMonitor.UnitTests.LocalData
{
    [TestFixture]
    public class BuildTimesTests
    {
        [Test]
        public void ReturnZeroForMissingMonth()
        {
            var buildTimes = new BuildTimes(TimeSpan.FromSeconds(0), new Dictionary<string, TimeSpan>(), new Dictionary<SolutionMonth, TimeSpan>());

            Assert.AreEqual(TimeSpan.FromSeconds(0), buildTimes.SolutionMonth("", 0, 0));
        }
    }
}
