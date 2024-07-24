using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using Sue.Engine.Model;
using File = System.IO.File;

namespace Sue.Engine.UnitTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ChessEngineTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        var loggingConfiguration = new LoggingConfiguration();
        loggingConfiguration.AddRuleForAllLevels(new ConsoleTarget());
        LogManager.Configuration = loggingConfiguration;
    }

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
        var chessEngineSettings = new ChessEngineSettings
        {
            WhiteTime = TimeSpan.Zero,
            BlackTime = TimeSpan.Zero,
            FixedSearchTime = TimeSpan.FromSeconds(15)
        };

        // Act
        var actual = ChessEngine.FindBestMove(fen.ToString(), "", chessEngineSettings);

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
}