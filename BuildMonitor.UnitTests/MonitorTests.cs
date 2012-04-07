using System;
using System.Collections.Generic;
using System.Linq;
using BuildMonitor.Domain;
using BuildMonitor.UnitTests.Fakes;
using NUnit.Framework;

namespace BuildMonitor.UnitTests
{
    public class MonitorTests : Spec
    {
        private BuildFactoryFake buildFactoryFake;
        private SolutionBuildFake solutionBuildFake;
        private BuildRepositoryFake buildRepositoryFake;
        private Monitor monitor;

        [SetUp]
        public void Setup()
        {
            buildFactoryFake = new BuildFactoryFake();
            buildRepositoryFake = new BuildRepositoryFake();
            solutionBuildFake = new SolutionBuildFake();
            buildFactoryFake.Build = solutionBuildFake;
            monitor = new Monitor(buildFactoryFake, buildRepositoryFake);
        }

        [Test]
        public void SolutionBuildStart_WithSolution_StartsTimer()
        {
            var solution = GetDefaultSolution();

            monitor.SolutionBuildStart(solution);

            Expect(solutionBuildFake.StartedCount).ToBe(1);
        }

        [Test]
        public void SolutionBuildStart_WithRunningSolution_ThrowsException()
        {
            var solution = GetDefaultSolution();

            monitor.SolutionBuildStart(solution);

            Assert.That(() => monitor.SolutionBuildStart(solution), Throws.Exception);
        }

        [Test]
        public void SolutionBuildStop_WithSolution_StopsTimer()
        {
            var solution = GetDefaultSolution();

            monitor.SolutionBuildStart(solution);
            monitor.SolutionBuildStop();

            Expect(solutionBuildFake.StopCount).ToBe(1);
        }

        [Test]
        public void SolutionBuildStop_WithoutSolution_DoesNothing()
        {
            monitor.SolutionBuildStop();

            Expect(solutionBuildFake.StopCount).ToBe(0);
            Expect(buildRepositoryFake.SaveCount).ToBe(0);
        }

        [Test]
        public void SolutionBuildStop_StopTwice_OnlyStopsOnce()
        {
            var solution = GetDefaultSolution();

            monitor.SolutionBuildStart(solution);
            monitor.SolutionBuildStop();
            monitor.SolutionBuildStop();

            Expect(solutionBuildFake.StopCount).ToBe(1);
        }

        [Test]
        public void SolutionBuildStop_WithSolution_PersistBuild()
        {
            var solution = GetDefaultSolution();

            monitor.SolutionBuildStart(solution);
            monitor.SolutionBuildStop();

            Expect(buildRepositoryFake.SaveCount).ToBe(1);
        }

        private static Solution GetDefaultSolution()
        {
            return new Solution(Guid.Empty, "a");
        }

        [Test]
        public void SolutionBuildStop_WithSolution_RaisesSolutionBuildFinished()
        {
            var finished = false;
            monitor.SolutionBuildFinished = (d) => finished = true;

            monitor.SolutionBuildStart(GetDefaultSolution());
            monitor.SolutionBuildStop();

            Expect(finished).ToBe(true);
        }

        [Test]
        public void SolutionBuildStop_WithSolution_CountsBuilds()
        {
            var count = 0;
            monitor.SolutionBuildFinished = (buildData) => count = buildData.SessionBuildCount;

            monitor.SolutionBuildStart(GetDefaultSolution());
            monitor.SolutionBuildStop();

            Expect(count).ToBe(1);
        }

        [Test]
        public void SolutionBuildStop_WithSolutionTwice_CountsBuilds()
        {
            var count = 0;
            monitor.SolutionBuildFinished = (buildData) => count = buildData.SessionBuildCount;

            monitor.SolutionBuildStart(GetDefaultSolution());
            monitor.SolutionBuildStop();
            monitor.SolutionBuildStart(GetDefaultSolution());
            monitor.SolutionBuildStop();

            Expect(count).ToBe(2);
        }
    }
}
