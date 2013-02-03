using System;
using System.Linq;
using BuildMonitor.LocalData;
using BuildMonitor.UnitTests.Fakes;
using NUnit.Framework;

namespace BuildMonitor.UnitTests.LocalData
{
    [TestFixture]
    public class DataAdjusterTests
    {
        private BuildRepositoryFake buildRepositoryFake;
        private DataAdjuster adjuster;

        [SetUp]
        public void Setup()
        {
            buildRepositoryFake = new BuildRepositoryFake();
            adjuster = new DataAdjuster(buildRepositoryFake);
        }

        [Test]
        public void Adjust_Always_GetsRawDataFromRepository()
        {
            adjuster.Adjust();

            Assert.That(buildRepositoryFake.GetRawCount, Is.EqualTo(1));
        }

        [Test]
        public void Adjust_InvalidRawData_RaiseEvent()
        {
            var data = new InvalidData();
            adjuster.OnFoundInvalidData = d => { data = d; };
            buildRepositoryFake.Source = "a";
            buildRepositoryFake.RawData = "{}";

            adjuster.Adjust();

            Assert.That(data.Data, Is.EqualTo("{}"));
            Assert.That(data.Source, Is.EqualTo("a"));
        }

        [Test]
        public void Adjust_InvalidDataWithoutEventSetup_DoNotThrowException()
        {
            buildRepositoryFake.Source = "a";
            buildRepositoryFake.RawData = "{}";

            Assert.That(() => adjuster.Adjust(), Throws.Nothing);
        }

        [TestCase("[{}{}]")]
        [TestCase("{}{}")]
        public void Adjust_InvalidData_Adjust(string raw)
        {
            buildRepositoryFake.RawData = raw;

            adjuster.Adjust();

            var result = buildRepositoryFake.SavedData.First();
            Assert.That(result, Is.EqualTo("[{},{}]"));
        }

        [Test]
        public void Adjust_SavedData_RaisesEvent()
        {
            var raisedEvent = false;
            adjuster.OnFixedInvalidData = () => { raisedEvent = true; };
            buildRepositoryFake.RawData = "{}";

            adjuster.Adjust();

            Assert.That(raisedEvent, Is.True);
        }

        [Test]
        public void Adjust_ThrowsOnSavingData_RaiseEvent()
        {
            var raisedEvent = false;
            adjuster.OnCouldNotConvertData = e => { raisedEvent = true; };
            buildRepositoryFake.RawData = "{}";
            buildRepositoryFake.ThrowOnSave = new Exception();

            adjuster.Adjust();

            Assert.That(raisedEvent, Is.True);
        }
    }
}