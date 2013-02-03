using BuildMonitor.LocalData;
using NUnit.Framework;

namespace BuildMonitor.UnitTests.LocalData
{
    [TestFixture]
    public class RawBuildDataTests
    {
        private static RawBuildData GetRawBuildData(string jsonText)
        {
            return new RawBuildData(jsonText);
        }

        [TestCase("")]
        [TestCase(null)]
        public void IsValidData_EmptyString_IsValid(string jsonText)
        {
            var result = GetRawBuildData(jsonText).IsValidData;

            Assert.That(result, Is.True);
        }

        [TestCase("[{}]")]
        [TestCase("[{}, {}]")]
        public void IsValidData_ArrayOfObjectsInJson_IsValid(string jsonText)
        {
            var result = GetRawBuildData(jsonText).IsValidData;

            Assert.That(result, Is.True);
        }

        [TestCase("[{}{}]")]
        [TestCase("[{} {}]")]
        public void IsValidData_ArrayWithMissingComma_IsNotValid(string jsonText)
        {
            var result = GetRawBuildData(jsonText).IsValidData;

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidData_NativeJavascriptDate_IsNotValid()
        {
            var result = GetRawBuildData("[{ d : new Date(1234455) }]").IsValidData;

            Assert.That(result, Is.False);
        }
    }
}