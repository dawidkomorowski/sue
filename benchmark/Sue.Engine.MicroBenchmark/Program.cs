using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Sue.Engine.MicroBenchmark;

internal static class Program
{
    private static void Main()
    {
        BenchmarkRunner.Run<ChessEngineBenchmark>();
    }
}

[MemoryDiagnoser]
public class ChessEngineBenchmark
{
    [IterationCount(1)]
    [Benchmark]
    public void FindBestMove()
    {
        ChessEngine.FindBestMove("1rbr2k1/5pp1/1b3nnp/1pp1pN2/4P3/2P1BN1P/1PB2PP1/R3R1K1 b - - 11 25", "");
    }
}