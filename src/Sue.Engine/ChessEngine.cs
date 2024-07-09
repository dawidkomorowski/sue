﻿using System;
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

    public static string? FindBestMove(string fenString, string uciMoves)
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

        var search = new MiniMaxSearch();
        var bestMove = search.FindBestMove(chessboard);

        Logger.Trace("Best move for position: '{0}' move {1}", chessboard.ToFen(), bestMove?.ToUci());

        return bestMove?.ToUci();
    }
}