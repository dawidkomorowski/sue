using NUnit.Framework;
using Sue.Engine.Model;

namespace Sue.Engine.UnitTests.Model
{
    [TestFixture]
    public class FileTests
    {
        [TestCase(File.A, 0, File.A)]
        [TestCase(File.A, 1, File.B)]
        [TestCase(File.A, 7, File.H)]
        [TestCase(File.H, -1, File.G)]
        [TestCase(File.H, -7, File.A)]
        public void Add_ShouldReturnCorrectFile_GivenFileAndOffsetToAdd(File baseFile, int offset, File expectedFile)
        {
            // Arrange
            // Act
            var actualFile = baseFile.Add(offset);

            // Assert
            Assert.That(actualFile, Is.EqualTo(expectedFile));
        }
    }
}