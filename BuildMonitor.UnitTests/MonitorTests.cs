using BuildMonitor.Domain;
using BuildMonitor.UnitTests.Fakes;
using NUnit.Framework;

namespace BuildMonitor.UnitTests
{
    public class MonitorTests
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

            Assert.That(solutionBuildFake.StartedCount, Is.EqualTo(1));
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

            Assert.That(solutionBuildFake.StopCount, Is.EqualTo(1));
        }

        [Test]
        public void SolutionBuildStop_WithoutSolution_DoesNothing()
        {
            monitor.SolutionBuildStop();

            Assert.That(solutionBuildFake.StopCount, Is.EqualTo(0));
            Assert.That(buildRepositoryFake.SaveCount, Is.EqualTo(0));
        }

        [Test]
        public void SolutionBuildStop_StopTwice_OnlyStopsOnce()
        {
            var solution = GetDefaultSolution();

            monitor.SolutionBuildStart(solution);
            monitor.SolutionBuildStop();
            monitor.SolutionBuildStop();

            Assert.That(solutionBuildFake.StopCount, Is.EqualTo(1));
        }

        [Test]
        public void SolutionBuildStop_WithSolution_PersistBuild()
        {
            var solution = GetDefaultSolution();

            monitor.SolutionBuildStart(solution);
            monitor.SolutionBuildStop();

            Assert.That(buildRepositoryFake.SaveCount, Is.EqualTo(1));
        }

        private static Solution GetDefaultSolution()
        {
            return new Solution{ Name = "a" };
        }

        [Test]
        public void SolutionBuildStop_WithSolution_RaisesSolutionBuildFinished()
        {
            var finished = false;
            monitor.SolutionBuildFinished = d => finished = true;

            monitor.SolutionBuildStart(GetDefaultSolution());
            monitor.SolutionBuildStop();

            Assert.That(finished, Is.True);
        }

        [Test]
        public void SolutionBuildStop_WithSolution_CountsBuilds()
        {
            var count = 0;
            monitor.SolutionBuildFinished = buildData => count = buildData.SessionBuildCount;

            monitor.SolutionBuildStart(GetDefaultSolution());
            monitor.SolutionBuildStop();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void SolutionBuildStop_WithSolutionTwice_CountsBuilds()
        {
            var count = 0;
            monitor.SolutionBuildFinished = buildData => count = buildData.SessionBuildCount;

            monitor.SolutionBuildStart(GetDefaultSolution());
            monitor.SolutionBuildStop();
            monitor.SolutionBuildStart(GetDefaultSolution());
            monitor.SolutionBuildStop();

            Assert.That(count, Is.EqualTo(2));
        }
    }
}
