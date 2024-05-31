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
        IRookMovesFinder rookMovesFinder = new RookMovesFinder();
        IBishopMovesFinder bishopMovesFinder = new BishopMovesFinder();
        IChessPieceFactory chessPieceFactory = new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
        var chessboardFactory = new ChessboardFactory(chessPieceFactory);

        var chessboard = chessboardFactory.Create(fenString);
        PlayUciMoves(chessboard, uciMoves);

        var availableMoves = chessboard.GetChessPieces(chessboard.CurrentPlayer).SelectMany(cp => cp.Moves).ToList();
        var move = availableMoves.MinBy(_ => Guid.NewGuid());

        if (move == null)
        {
            return null;
        }

        var p0 = move.From.File.ToString().ToLower();
        var p1 = (move.From.Rank.Index() + 1).ToString();
        var p2 = move.To.File.ToString().ToLower();
        var p3 = (move.To.Rank.Index() + 1).ToString();

        return $"{p0}{p1}{p2}{p3}";
    }

    private static void PlayUciMoves(IChessboard chessboard, string uciMoves)
    {
        if (string.IsNullOrWhiteSpace(uciMoves))
        {
            return;
        }

        var moves = uciMoves.Split(" ");
        foreach (var move in moves)
        {
            if (move.Length != 4)
            {
                throw new InvalidOperationException($"Unsupported UCI move: {move}");
            }

            var chessPiece = chessboard.GetChessPiece(move[0].ToFile(), move[1].ToRank());
            chessPiece.MakeMove(new Move(chessPiece.ChessboardField, chessboard.GetChessboardField(move[2].ToFile(), move[3].ToRank())));
        }
    }
}