using BuildMonitor.UI;
using NUnit.Framework;

namespace BuildMonitor.UnitTests.UI
{
    [TestFixture]
    public class TimeDisplayTests
    {
        [Test]
        public void FromMilliseconds_Zero_ReturnsCorrectFormat()
        {
            var result = Time.FromMilliseconds(0);

            Assert.That(result.Hours, Is.EqualTo(0));
            Assert.That(result.Minutes, Is.EqualTo(0));
            Assert.That(result.Seconds, Is.EqualTo(0)); 
            Assert.That(result.Milliseconds, Is.EqualTo(0));
        }

        [Test]
        public void FromMilliseconds_One_ReturnsCorrectFormat()
        {
            var result = Time.FromMilliseconds(1);

            Assert.That(result.Hours, Is.EqualTo(0));
            Assert.That(result.Minutes, Is.EqualTo(0));
            Assert.That(result.Seconds, Is.EqualTo(0));
            Assert.That(result.Milliseconds, Is.EqualTo(1));
        }

        [Test]
        public void FromMilliseconds_OneThousand_ReturnsCorrectFormat()
        {
            var result = Time.FromMilliseconds(1000);

            Assert.That(result.Hours, Is.EqualTo(0));
            Assert.That(result.Minutes, Is.EqualTo(0));
            Assert.That(result.Seconds, Is.EqualTo(1));
            Assert.That(result.Milliseconds, Is.EqualTo(0));
        }

        [Test]
        public void FromMilliseconds_SixtyThousand_ReturnsCorrectFormat()
        {
            var result = Time.FromMilliseconds(60000);

            Assert.That(result.Hours, Is.EqualTo(0));
            Assert.That(result.Minutes, Is.EqualTo(1));
            Assert.That(result.Seconds, Is.EqualTo(0));
            Assert.That(result.Milliseconds, Is.EqualTo(0));
        }

        [Test]
        public void FromMilliseconds_ThreePointSixMillion_ReturnsCorrectFormat()
        {
            var result = Time.FromMilliseconds(3600000);

            Assert.That(result.Hours, Is.EqualTo(1));
            Assert.That(result.Minutes, Is.EqualTo(0));
            Assert.That(result.Seconds, Is.EqualTo(0));
            Assert.That(result.Milliseconds, Is.EqualTo(0));
        }

        [TestCase(1, "00h 00m 00s 001ms")]
        [TestCase(123, "00h 00m 00s 123ms")]
        [TestCase(1001, "00h 00m 01s 001ms")]
        [TestCase(60000, "00h 01m 00s 000ms")]
        [TestCase(61001, "00h 01m 01s 001ms")]
        [TestCase(3661010, "01h 01m 01s 010ms")]
        public void Display_Always_DisplayCorrectFormat(long ms, string expected)
        {
            var result = ms.ToTime();

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}