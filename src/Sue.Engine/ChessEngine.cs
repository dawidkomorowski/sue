using System;
using Sue.Engine.Model;
using Sue.Engine.Search;

namespace Sue.Engine;

public static class ChessEngine
{
    public static Color GetActiveColor(string fenString, string uciMoves)
    {
        var fen = Fen.FromString(fenString);
        var moves = Move.ParseUciMoves(uciMoves);
        var chessboard = Chessboard.FromFen(fen);

        foreach (var move in moves)
        {
            chessboard.MakeMove(move);
        }

        return chessboard.ActiveColor;
    }

    public static string? FindBestMove(string fenString, string uciMoves, SearchStrategy strategy)
    {
        var fen = Fen.FromString(fenString);
        var moves = Move.ParseUciMoves(uciMoves);
        var chessboard = Chessboard.FromFen(fen);

        foreach (var move in moves)
        {
            chessboard.MakeMove(move);
        }

        var search = CreateSearch(strategy);
        var bestMove = search.FindBestMove(chessboard);
        return bestMove?.ToUci();
    }

    private static ISearch CreateSearch(SearchStrategy strategy)
    {
        return strategy switch
        {
            SearchStrategy.Random => new RandomSearch(),
            SearchStrategy.PureMonteCarlo => new PureMonteCarloSearch(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unknown search strategy.")
        };
    }
}