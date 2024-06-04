using System;
using Sue.Engine.Model;

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

    public static string? FindBestMove(string fenString, string uciMoves)
    {
        var fen = Fen.FromString(fenString);
        var moves = Move.ParseUciMoves(uciMoves);
        var chessboard = Chessboard.FromFen(fen);

        foreach (var move in moves)
        {
            chessboard.MakeMove(move);
        }

        var moveCandidates = chessboard.GetMoveCandidates();

        if (moveCandidates.Count == 0)
        {
            return null;
        }

        var bestMove = moveCandidates[Random.Shared.Next(moveCandidates.Count)];

        //IRookMovesFinder rookMovesFinder = new RookMovesFinder();
        //IBishopMovesFinder bishopMovesFinder = new BishopMovesFinder();
        //IChessPieceFactory chessPieceFactory = new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
        //var chessboardFactory = new ChessboardFactory(chessPieceFactory);
        //var chessboardOld = chessboardFactory.Create(chessboard.ToFen().ToString());

        //var availableMoves = chessboardOld.GetChessPieces(chessboardOld.CurrentPlayer).SelectMany(cp => cp.Moves).ToList();
        //var bestMove = availableMoves.MinBy(_ => Guid.NewGuid());

        //if (bestMove == null)
        //{
        //    return null;
        //}

        //var bm = new Move(new Position(bestMove.From.File, bestMove.From.Rank), new Position(bestMove.To.File, bestMove.To.Rank));

        return bestMove.ToUci();
    }
}