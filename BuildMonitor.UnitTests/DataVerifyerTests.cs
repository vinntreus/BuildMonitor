using NUnit.Framework;

namespace BuildMonitor.UnitTests
{
    [TestFixture]
    public class DataVerifyerTests
    {
        [TestCase("")]
        [TestCase(null)]
        public void IsValidData_EmptyString_IsValid(string jsonText)
        {
            var result = DataVerifyer.IsValidData(jsonText);

            Assert.That(result, Is.True);
        }

        [TestCase("[{}]")]
        [TestCase("[{}, {}]")]
        public void IsValidData_ArrayOfObjectsInJson_IsValid(string jsonText)
        {
            var result = DataVerifyer.IsValidData(jsonText);

            Assert.That(result, Is.True);
        }

        [TestCase("[{}{}]")]
        [TestCase("[{} {}]")]
        public void IsValidData_ArrayWithMissingComma_IsNotValid(string jsonText)
        {
            var result = DataVerifyer.IsValidData(jsonText);

            Assert.That(result, Is.False);
        }

        
    }
}