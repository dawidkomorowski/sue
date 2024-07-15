using NLog;
using System;

namespace Sue.Engine.ProfilingApp;

internal static class Program
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private static void Main()
    {
        var startTime = DateTime.Now;

        while (DateTime.Now - startTime < TimeSpan.FromMinutes(1))
        {
            Logger.Info("FindBestMove started.");
            ChessEngine.FindBestMove("1rbr2k1/5pp1/1b3nnp/1pp1pN2/4P3/2P1BN1P/1PB2PP1/R3R1K1 b - - 11 25", "");
            Logger.Info("FindBestMove completed.");
        }
    }
}