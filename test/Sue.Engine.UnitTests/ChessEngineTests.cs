using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Sue.Engine.Model;
using File = System.IO.File;

namespace Sue.Engine.UnitTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ChessEngineTests
{
    private static TestCaseData[] MateIn(int n)
    {
        var lines = File.ReadAllLines(Path.Combine("TestFiles", "Mate", $"mate_in_{n}.epd"));
        var testCases = new List<TestCaseData>();

        foreach (var line in lines)
        {
            var match = Regex.Match(line, "(.*) bm .?([a-h][1-9])[-x]([a-h][1-9][QRBN]?)");
            var fenString = match.Groups[1].Value + " 0 1";
            var bestMoveUci = match.Groups[2].Value + match.Groups[3].Value;

            var fen = Fen.FromString(fenString);
            var bestMove = Move.ParseUciMove(bestMoveUci);

            testCases.Add(new TestCaseData(fen, bestMove));
        }

        return testCases.ToArray();
    }

    private static void TestMateInN(Fen fen, Move bestMove)
    {
        // Arrange
        var engineSettings = new EngineSettings
        {
            RandomSeed = 0
        };

        var chessEngine = new ChessEngine(engineSettings);

        var searchSettings = new SearchSettings
        {
            FixedSearchTime = TimeSpan.FromSeconds(15)
        };

        // Act
        var actual = chessEngine.FindBestMove(fen.ToString(), "", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(bestMove.ToUci()));
    }

    private static TestCaseData[] MateIn1() => MateIn(1);

    [TestCaseSource(nameof(MateIn1))]
    public void Test_MateIn_1(Fen fen, Move bestMove) => TestMateInN(fen, bestMove);

    private static TestCaseData[] MateIn2() => MateIn(2);

    [TestCaseSource(nameof(MateIn2))]
    public void Test_MateIn_2(Fen fen, Move bestMove) => TestMateInN(fen, bestMove);

    private static TestCaseData[] MateIn3() => MateIn(3);

    [TestCaseSource(nameof(MateIn3))]
    public void Test_MateIn_3(Fen fen, Move bestMove) => TestMateInN(fen, bestMove);

    private static TestCaseData[] MateIn8() => MateIn(8);

    [TestCaseSource(nameof(MateIn8))]
    public void Test_MateIn_8(Fen fen, Move bestMove) => TestMateInN(fen, bestMove);

    [Test]
    public void FindBestMove_ShouldPlayFromOpeningBook()
    {
        // Arrange
        var engineSettings = new EngineSettings
        {
            RandomSeed = 3
        };

        var chessEngine = new ChessEngine(engineSettings);

        var searchSettings = new SearchSettings
        {
            FixedSearchTime = TimeSpan.FromSeconds(15)
        };

        // Act
        var actual = chessEngine.FindBestMove(Fen.StartPos, "", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("d2d4"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("g8f6"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("g1f3"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("e7e6"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("c2c4"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("b7b6"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("g2g3"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6 g2g3", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("c8a6"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6 g2g3 c8a6", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("d1a4"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6 g2g3 c8a6 d1a4", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("c7c5"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6 g2g3 c8a6 d1a4 c7c5", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("f1g2"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6 g2g3 c8a6 d1a4 c7c5 f1g2", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("a6b7"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6 g2g3 c8a6 d1a4 c7c5 f1g2 a6b7", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("d4c5"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6 g2g3 c8a6 d1a4 c7c5 f1g2 a6b7 d4c5", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("f8c5"));

        // Act
        actual = chessEngine.FindBestMove(Fen.StartPos, "d2d4 g8f6 g1f3 e7e6 c2c4 b7b6 g2g3 c8a6 d1a4 c7c5 f1g2 a6b7 d4c5 f8c5", searchSettings);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo("e1g1"));
    }
}