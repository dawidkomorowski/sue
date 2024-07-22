using System;

namespace Sue.Engine;

public sealed class ChessEngineSettings
{
    public TimeSpan WhiteTime { get; init; }
    public TimeSpan BlackTime { get; init; }
    public TimeSpan? FixedSearchTime { get; set; }
}