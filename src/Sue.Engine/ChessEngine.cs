using System;
using System.Linq;
using Sue.Engine.Model;
using Sue.Engine.OldModel;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;
using Sue.Engine.OldModel.ChessPiece.Internal;
using Move = Sue.Engine.OldModel.Internal.Move;

namespace Sue.Engine;

public static class ChessEngine
{
    public static string? FindMove(string fenString, string uciMoves)
    {
        var fen = Fen.FromString(fenString);
        var moves = Model.Move.ParseUciMoves(uciMoves);
        var chessboard = Chessboard.FromFen(fen);

        foreach (var move in moves)
        {
            chessboard.MakeMove(move);
        }

        IRookMovesFinder rookMovesFinder = new RookMovesFinder();
        IBishopMovesFinder bishopMovesFinder = new BishopMovesFinder();
        IChessPieceFactory chessPieceFactory = new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
        var chessboardFactory = new ChessboardFactory(chessPieceFactory);
        var chessboardOld = chessboardFactory.Create(chessboard.ToFen().ToString());

        var availableMoves = chessboardOld.GetChessPieces(chessboardOld.CurrentPlayer).SelectMany(cp => cp.Moves).ToList();
        var bestMove = availableMoves.MinBy(_ => Guid.NewGuid());

        if (bestMove == null)
        {
            return null;
        }

        var bm = new Model.Move(new Position(bestMove.From.File, bestMove.From.Rank), new Position(bestMove.To.File, bestMove.To.Rank));
        return bm.ToUci();
    }
}