using System;
using System.Diagnostics.CodeAnalysis;

namespace Sue.Engine;

public sealed class SearchSettings
{
    public TimeSpan WhiteTime { get; init; }
    public TimeSpan BlackTime { get; init; }
    public TimeSpan? FixedSearchTime { get; init; }
    public int? FixedDepth { get; init; }

    [MemberNotNullWhen(true, nameof(FixedDepth))]
    public bool UseFixedDepth => FixedDepth.HasValue;
}