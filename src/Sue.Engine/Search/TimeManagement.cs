using System;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal static class TimeManagement
{
    private const int EstimatedMovesPerGame = 50;
    private const int EstimatedMovesNeeded = 10;

    public static TimeSpan ComputeSearchTime(ChessEngineSettings settings, Chessboard chessboard)
    {
        if (settings.FixedSearchTime.HasValue)
        {
            return settings.FixedSearchTime.Value;
        }

        var timeRemaining = chessboard.ActiveColor is Color.White ? settings.WhiteTime : settings.BlackTime;
        var estimatedMovesRemaining = EstimatedMovesPerGame - chessboard.FullMoveNumber;

        if (estimatedMovesRemaining < EstimatedMovesNeeded)
        {
            estimatedMovesRemaining = EstimatedMovesNeeded;
        }

        return timeRemaining / estimatedMovesRemaining;
    }
}