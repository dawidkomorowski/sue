using NUnit.Framework;
using Sue.Engine.Search;

namespace Sue.Engine.UnitTests.Search;

[TestFixture]
public class ScoreTests
{
    [TestCase(0)]
    [TestCase(15)]
    [TestCase(-15)]
    public void Constructor_ShouldCreateEvalScore(int eval)
    {
        // Arrange
        // Act
        var score = Score.CreateEval(eval);

        // Assert
        Assert.That(score.Eval, Is.EqualTo(eval));
        Assert.That(score.MateIn, Is.Zero);
        Assert.That(score.IsMate, Is.False);
    }

    [TestCase(3)]
    [TestCase(-3)]
    public void Constructor_ShouldCreateMateScore(int mateIn)
    {
        // Arrange
        // Act
        var score = Score.CreateMate(mateIn);

        // Assert
        Assert.That(score.Eval, Is.Zero);
        Assert.That(score.MateIn, Is.EqualTo(mateIn));
        Assert.That(score.IsMate, Is.True);
    }

    // Eval only
    [TestCase(false, 0, false, 0, 0)]
    [TestCase(false, 12, false, 12, 0)]
    [TestCase(false, -12, false, -12, 0)]
    [TestCase(false, 5, false, 3, 1)]
    [TestCase(false, -5, false, -3, -1)]
    [TestCase(false, 5, false, -3, 1)]
    [TestCase(false, -5, false, 3, -1)]
    // Mixed
    [TestCase(true, 1, false, 0, 1)]
    [TestCase(true, -1, false, 0, -1)]
    [TestCase(true, 1, false, 10, 1)]
    [TestCase(true, -1, false, 10, -1)]
    [TestCase(true, 1, false, -10, 1)]
    [TestCase(true, -1, false, -10, -1)]
    // Mate only
    [TestCase(true, 5, true, 5, 0)]
    [TestCase(true, -5, true, -5, 0)]
    [TestCase(true, 1, true, 1, 0)]
    [TestCase(true, -1, true, -1, 0)]
    [TestCase(true, 3, true, 10, 1)]
    [TestCase(true, 5, true, 3, -1)]
    [TestCase(true, -3, true, -10, -1)]
    [TestCase(true, -5, true, -3, 1)]
    [TestCase(true, 3, true, -3, 1)]
    [TestCase(true, 2, true, -3, 1)]
    [TestCase(true, -2, true, 3, -1)]
    public void RelationalMembers_Tests(bool isMate1, int value1, bool isMate2, int value2, int expected)
    {
        // Arrange
        var score1 = isMate1 ? Score.CreateMate(value1) : Score.CreateEval(value1);
        var score2 = isMate2 ? Score.CreateMate(value2) : Score.CreateEval(value2);

        // Act
        var compareResult1 = score1.CompareTo(score2);
        var compareResult2 = score2.CompareTo(score1);
        var equalsResult1 = score1.Equals(score2);
        var equalsResult2 = score2.Equals(score1);

        // Assert
        Assert.That(compareResult1, Is.EqualTo(expected));
        Assert.That(compareResult2, Is.EqualTo(-expected));
        Assert.That(equalsResult1, Is.EqualTo(expected == 0));
        Assert.That(equalsResult2, Is.EqualTo(expected == 0));
    }

    [TestCase(false, int.MaxValue)]
    [TestCase(true, 1)]
    public void Max_IsTheMaximumScore(bool isMate, int value)
    {
        // Arrange
        var score = isMate ? Score.CreateMate(value) : Score.CreateEval(value);

        // Act
        var compareResult1 = Score.Max.CompareTo(score);
        var compareResult2 = score.CompareTo(Score.Max);

        // Assert
        Assert.That(compareResult1, Is.EqualTo(1));
        Assert.That(compareResult2, Is.EqualTo(-1));
    }

    [TestCase(false, int.MinValue)]
    [TestCase(true, -1)]
    public void Min_IsTheMinimumScore(bool isMate, int value)
    {
        // Arrange
        var score = isMate ? Score.CreateMate(value) : Score.CreateEval(value);

        // Act
        var compareResult1 = Score.Min.CompareTo(score);
        var compareResult2 = score.CompareTo(Score.Min);

        // Assert
        Assert.That(compareResult1, Is.EqualTo(-1));
        Assert.That(compareResult2, Is.EqualTo(1));
    }
}