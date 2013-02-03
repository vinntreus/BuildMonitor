using System;
using BuildMonitor.Domain;
using BuildMonitor.UnitTests.Fakes;
using NUnit.Framework;

namespace BuildMonitor.UnitTests
{
    [TestFixture]
    public class SolutionBuildTests
    {
        private TimerFake timerFake;
        private SolutionBuild solutionBuild;

        [SetUp]
        public void Setup()
        {
            timerFake = new TimerFake();
            solutionBuild = new SolutionBuild(timerFake, new Solution());
        }

        [Test]
        public void Start_NotRunning_SetsStartedToNow()
        {
            solutionBuild.Start();

            Assert.That(solutionBuild.Started, Is.InRange(DateTime.Now.AddSeconds(-1.0), DateTime.Now.AddSeconds(1.0)));
        }

        [Test]
        public void Start_NotRunning_StartsTimer()
        {
            solutionBuild.Start();
            
            Assert.That(timerFake.StartedCount, Is.EqualTo(1));
        }

        [Test]
        public void Start_IsRunning_ThrowsException()
        {
            timerFake.IsRunning = true;

            Assert.That(() => solutionBuild.Start(), Throws.Exception);
        }

        [Test]
        public void IsRunning_NotRunning_ReturnsFalse()
        {
            timerFake.IsRunning = false;

            var isRunning = solutionBuild.IsRunning;

            Assert.That(isRunning, Is.False);
        }

        [Test]
        public void IsRunning_Running_ReturnsTrue()
        {
            timerFake.IsRunning = true;

            var isRunning = solutionBuild.IsRunning;

            Assert.That(isRunning, Is.True);
        }

        [Test]
        public void Stop_Always_StopsTimer()
        {
            solutionBuild.Stop();

            Assert.That(timerFake.StopCount, Is.EqualTo(1));
        }


        [Test]
        public void Stop_Always_SetsMillisecondsElapsed()
        {
            timerFake.MillisecondsElapsed = 1;

            solutionBuild.Stop();

            Assert.That(solutionBuild.MillisecondsElapsed, Is.EqualTo(1));
        }
    }
}