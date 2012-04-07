using System;
using BuildMonitor.Domain;
using BuildMonitor.UnitTests.Fakes;
using NUnit.Framework;

namespace BuildMonitor.UnitTests
{
    [TestFixture]
    public class SolutionBuildTests : Spec
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
            
            Expect(timerFake.StartedCount).ToBe(1);
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

            Expect(isRunning).ToBe(false);
        }

        [Test]
        public void IsRunning_Running_ReturnsTrue()
        {
            timerFake.IsRunning = true;

            var isRunning = solutionBuild.IsRunning;

            Expect(isRunning).ToBe(true);
        }

        [Test]
        public void Stop_Always_StopsTimer()
        {
            solutionBuild.Stop();

            Expect(timerFake.StopCount).ToBe(1);
        }


        [Test]
        public void Stop_Always_SetsMillisecondsElapsed()
        {
            timerFake.MillisecondsElapsed = 1;

            solutionBuild.Stop();

            Expect(solutionBuild.MillisecondsElapsed).ToBe(1);
        }
    }
}