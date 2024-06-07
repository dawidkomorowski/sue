using System;
using NLog;
using Sue.Engine.Model;
using Sue.Engine.Search;

namespace Sue.Engine;

public static class ChessEngine
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

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
        Logger.Trace("FindBestMove - fen '{0}' moves '{1}'", fenString, uciMoves);

        var fen = Fen.FromString(fenString);
        var moves = Move.ParseUciMoves(uciMoves);
        var chessboard = Chessboard.FromFen(fen);

        foreach (var move in moves)
        {
            chessboard.MakeMove(move);
        }

        Logger.Trace("Finding move for position: '{0}'", chessboard.ToFen());

        var search = CreateSearch(strategy);
        var bestMove = search.FindBestMove(chessboard);

        Logger.Trace("Best move for position: '{0}' move {1}", chessboard.ToFen(), bestMove?.ToUci());

        return bestMove?.ToUci();
    }

    private static ISearch CreateSearch(SearchStrategy strategy)
    {
        return strategy switch
        {
            SearchStrategy.Random => new RandomSearch(),
            SearchStrategy.PureMonteCarlo => new PureMonteCarloSearch(),
            SearchStrategy.MiniMax => new MiniMaxSearch(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unknown search strategy.")
        };
    }
}