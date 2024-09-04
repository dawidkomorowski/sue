using System;
using NLog;
using Sue.Engine.Book;
using Sue.Engine.Model;
using Sue.Engine.Search;

namespace Sue.Engine;

public static class ChessEngine
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private static readonly OpeningBookAbk OpeningBookAbk = new();

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

    public static string? FindBestMove(string fenString, string uciMoves, ChessEngineSettings settings)
    {
        Logger.Trace("FindBestMove - fen '{0}' moves '{1}'", fenString, uciMoves);

        var fen = Fen.FromString(fenString);
        var moves = Move.ParseUciMoves(uciMoves);
        var chessboard = Chessboard.FromFen(fen);

        foreach (var move in moves)
        {
            chessboard.MakeMove(move);
        }

        Logger.Trace("Looking for next moves in book.");
        var nextMovesFromBook = OpeningBookAbk.GetNextMoves(moves);
        if (nextMovesFromBook.Length != 0)
        {
            Logger.Trace("Found next moves in book.");

            Random.Shared.Shuffle(nextMovesFromBook);
            var nextMove = nextMovesFromBook[0];

            Logger.Trace("Next move from book for position: '{0}' move {1}", chessboard.ToFen(), nextMove.ToUci());
            return nextMove.ToUci();
        }

        var searchTime = TimeManagement.ComputeSearchTime(settings, chessboard);

        Logger.Trace("Finding move for position: '{0}'", chessboard.ToFen());

        var search = new MoveSearch(settings);
        var bestMove = search.FindBestMove(chessboard, searchTime);

        Logger.Trace("Best move for position: '{0}' move {1}", chessboard.ToFen(), bestMove?.ToUci());

        return bestMove?.ToUci();
    }
}