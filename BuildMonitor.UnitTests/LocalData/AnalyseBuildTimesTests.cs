using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BuildMonitor.LocalData;

namespace BuildMonitor.UnitTests.LocalData
{
    internal struct SolutionBuildTime
    {
        public string Solution { get; set; }
        public DateTime BuildDateTime { get; set; }
        public int BuildTimeInMilliseconds { get; set; }
    }

    internal struct SolutionMonth
    {
        public string Solution { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    [TestFixture]
    public class AnalyseBuildTimesTests
    {
        [TestCase(19366)]
        [TestCase(1)]
        [TestCase(395)]
        public void OneBuildTimeTotal(int singleBuildTimeInMilliseconds)
        {
            var json = CreateBuildsJSON(CreateBuildJSON(singleBuildTimeInMilliseconds));

            var buildTimes = new AnalyseBuildTimes().Calculate(json);

            Assert.That(buildTimes.Total == TimeSpan.FromMilliseconds(singleBuildTimeInMilliseconds));
        }

        [TestCase(1, 2)]
        [TestCase(21341, 123642, 21389734)]
        [TestCase(543, 0, 34534567)]
        public void MultipleBuildsSameSolution(params int[] buildTimeInMilliseconds)
        {
            var json = CreateBuildsJSON(buildTimeInMilliseconds.Select(b => CreateBuildJSON(timeInMilliseconds: b)));

            var buildTimes = new AnalyseBuildTimes().Calculate(json);

            Assert.That(buildTimes.Total == TimeSpan.FromMilliseconds(buildTimeInMilliseconds.Sum()));
        }

        // Add a testcasesource here if more test cases are wanted
        [Test]
        public void MultipleBuildsMultipleSolutions()
        {
            var solutionbuilds = CreateBuilds(
                CreateBuild("BuildMonitor", 1)
                , CreateBuild("Cedd", 2)
                , CreateBuild("BuildMonitor", 982734789)
                , CreateBuild("Cedd", 83468)
                );

            var json = CreateBuildsJSON(solutionbuilds.Select(b => CreateBuildJSON(timeInMilliseconds: b.BuildTimeInMilliseconds, solutionName: b.Solution)));

            var buildTimes = new AnalyseBuildTimes().Calculate(json);

            foreach (var solution in solutionbuilds.Select(s => s.Solution).Distinct())
                Assert.That(buildTimes.Solution(solution) == TimeSpan.FromMilliseconds(solutionbuilds.Where(s => s.Solution == solution).Sum(s => s.BuildTimeInMilliseconds)));
        }

        // Add a testcasesource here if more test cases are wanted
        [Test]
        public void MultipleBuildsMultipleMonthsMultipleSolutions()
        {
            var solutionbuilds = CreateBuilds(
                CreateBuild("BuildMonitor", 1, JANUARY1())
                , CreateBuild("Cedd", 2, JANUARY31())
                , CreateBuild("BuildMonitor", 982734789, FEBRUARY())
                , CreateBuild("Cedd", 83468, MARCH())
                );

            var json = CreateBuildsJSON(solutionbuilds.Select(b => CreateBuildJSON(timeInMilliseconds: b.BuildTimeInMilliseconds, solutionName: b.Solution, start: b.BuildDateTime)));

            //act
            var buildTimes = new AnalyseBuildTimes().Calculate(json); 

            // assert
            var solutionMonths = solutionbuilds.Select(s => new SolutionMonth() { Solution = s.Solution, Month = s.BuildDateTime.Month, Year = s.BuildDateTime.Year }).Distinct();
            foreach (var solutionMonth in solutionMonths)
                Assert.That(buildTimes.SolutionMonth(solutionMonth.Solution, solutionMonth.Month, solutionMonth.Year) == TimeSpan.FromMilliseconds(solutionbuilds.Where(s => s.Solution == solutionMonth.Solution && s.BuildDateTime.Month == solutionMonth.Month && s.BuildDateTime.Year == solutionMonth.Year).Sum(s => s.BuildTimeInMilliseconds)));
        }

        private static string CreateBuildsJSON(string build)
        {
            return CreateBuildsJSON(new List<string>() { build });
        }

        private static string CreateBuildsJSON(IEnumerable<string> builds)
        {
            return "[" + string.Join("\n,\n", builds) + "]";
        }

        private static string CreateBuildJSON(int timeInMilliseconds, string solutionName = "blah", DateTime start = new DateTime())
        {
            return @"{
    'Start': '" + start.ToString() + @"',
    'Time': " + timeInMilliseconds.ToString() + @",
    'Solution': {
                'Name': '" + solutionName + @"'
                },
  'Projects': [{
      'Start': '2016-01-21T18:53:33.6172723+00:00',
      'Time': 2,
      'Project': {
        'Name': 'blah',
        'Id': '700a9d28-e9de-428b-a8f2-b40baf4a3e87'
      }
   },
      {
      'Start': '2016-01-22T18:53:33.6172723+00:00',
      'Time': 4,
      'Project': {
        'Name': 'blah2',
        'Id': '700a9d28-e9de-428b-a8f2-b40baf4a3e88'
      }
    }]
}";
        }

        private static IEnumerable<SolutionBuildTime> CreateBuilds(params SolutionBuildTime[] solutionBuildTimes)
        {
            return solutionBuildTimes;
        }

        private static SolutionBuildTime CreateBuild(string solution, int buildTimeInMilliseconds, DateTime buildDateTime = new DateTime())
        {
            return new SolutionBuildTime() { Solution = "BuildMonitor", BuildTimeInMilliseconds = 1, BuildDateTime = buildDateTime };
        }

        private DateTime MARCH()
        {
            return new DateTime(2000, 3, 1);
        }

        private DateTime FEBRUARY()
        {
            return new DateTime(2000, 2, 1);
        }

        private DateTime JANUARY1()
        {
            return new DateTime(2000, 1, 1);
        }

        private DateTime JANUARY31()
        {
            return new DateTime(2000, 1, 31);
        }

    }
}