using NUnit.Framework;
using Sue.Common.Model;

namespace Sue.Common.UnitTests.Model
{
    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void ShouldReturnBlack_AsOppositeToWhite()
        {
            var color = Color.White;
            Assert.That(color.Opposite(), Is.EqualTo(Color.Black));
        }

        [Test]
        public void ShouldReturnWhite_AsOppositeToBlack()
        {
            var color = Color.Black;
            Assert.That(color.Opposite(), Is.EqualTo(Color.White));
        }
    }
}
