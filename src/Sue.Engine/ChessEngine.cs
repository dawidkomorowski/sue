using NLog;
using Sue.Engine.Model;
using Sue.Engine.Search;

namespace Sue.Engine;

public static class ChessEngine
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

        var searchTime = TimeManagement.ComputeSearchTime(settings, chessboard);

        Logger.Trace("Finding move for position: '{0}'", chessboard.ToFen());

        var search = new MoveSearch();
        var bestMove = search.FindBestMove(chessboard, searchTime);

        Logger.Trace("Best move for position: '{0}' move {1}", chessboard.ToFen(), bestMove?.ToUci());

        return bestMove?.ToUci();
    }
}