using NUnit.Framework;
using Sue.Engine.Model;

namespace Sue.Engine.UnitTests.Model
{
    [TestFixture]
    public class FileTests
    {
        [TestCase(File.A, 0)]
        [TestCase(File.B, 1)]
        [TestCase(File.C, 2)]
        [TestCase(File.D, 3)]
        [TestCase(File.E, 4)]
        [TestCase(File.F, 5)]
        [TestCase(File.G, 6)]
        [TestCase(File.H, 7)]
        public void ShouldReturnCorrespondingIntegerIndexValue_GivenFile(File file, int index)
        {
            // Arrange
            // Act
            var actualIndex = file.Index();

            // Assert
            Assert.That(actualIndex, Is.EqualTo(index));
        }

        [TestCase(File.A, 0, File.A)]
        [TestCase(File.A, 1, File.B)]
        [TestCase(File.A, 7, File.H)]
        [TestCase(File.H, -1, File.G)]
        [TestCase(File.H, -7, File.A)]
        public void ShouldReturnCorrectFile_GivenFileAndOffsetToAdd(File baseFile, int offset, File expectedFile)
        {
            // Arrange
            // Act
            var actualFile = baseFile.Add(offset);

            // Assert
            Assert.That(actualFile, Is.EqualTo(expectedFile));
        }
    }
}