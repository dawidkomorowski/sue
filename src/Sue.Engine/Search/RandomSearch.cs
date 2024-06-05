using Sue.Engine.Model;
using System;

namespace Sue.Engine.Search;

internal sealed class RandomSearch : ISearch
{
    public Move? FindBestMove(Chessboard chessboard)
    {
        var moveCandidates = chessboard.GetMoveCandidates();

        if (moveCandidates.Count == 0)
        {
            return null;
        }

        return moveCandidates[Random.Shared.Next(moveCandidates.Count)];
    }
}