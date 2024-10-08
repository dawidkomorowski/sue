﻿using System;
using NLog;
using Sue.Engine.Book;
using Sue.Engine.Model;
using Sue.Engine.Search;

namespace Sue.Engine;

public sealed class ChessEngine
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly OpeningBookAbk _openingBookAbk = new();
    private readonly Random _random;

    public ChessEngine() : this(new EngineSettings())
    {
    }

    public ChessEngine(EngineSettings settings)
    {
        var randomSeed = settings.RandomSeed ?? Random.Shared.Next();
        Logger.Trace("Random seed: {0}", randomSeed);
        _random = new Random(randomSeed);
    }

    public Color GetActiveColor(string fenString, string uciMoves)
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

    public string? FindBestMove(string fenString, string uciMoves, SearchSettings settings)
    {
        Logger.Trace("FindBestMove - fen '{0}' moves '{1}'", fenString, uciMoves);

        var fen = Fen.FromString(fenString);
        var moves = Move.ParseUciMoves(uciMoves);
        var chessboard = Chessboard.FromFen(fen);

        foreach (var move in moves)
        {
            chessboard.MakeMove(move);
        }

        if (fenString != Fen.StartPos)
        {
            Logger.Trace("FEN string is not initial position. Skipping book lookup. FEN: '{0}'", fenString);
        }
        else if (moves.Count > OpeningBookAbk.MaxDepth)
        {
            Logger.Trace("Already played moves exceed opening book depth. Skipping book lookup. FEN: '{0}', Moves: {1}, Book Depth: {2}", fenString,
                moves.Count, OpeningBookAbk.MaxDepth);
        }
        else
        {
            Logger.Trace("Looking for next moves in book.");
            var nextMovesFromBook = _openingBookAbk.GetNextMoves(moves);
            if (nextMovesFromBook.Length != 0)
            {
                Logger.Trace("Found next moves in book.");
                var nextMove = nextMovesFromBook[_random.Next(nextMovesFromBook.Length)];
                Logger.Trace("Next move from book for position: '{0}' move {1}", chessboard.ToFen(), nextMove.ToUci());
                return nextMove.ToUci();
            }
        }

        var searchTime = TimeManagement.ComputeSearchTime(settings, chessboard);

        Logger.Trace("Finding move for position: '{0}'", chessboard.ToFen());

        var search = new MoveSearch(_random, settings);
        var bestMove = search.FindBestMove(chessboard, searchTime);

        Logger.Trace("Best move for position: '{0}' move {1}", chessboard.ToFen(), bestMove?.ToUci());

        return bestMove?.ToUci();
    }
}