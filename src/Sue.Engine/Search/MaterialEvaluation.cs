using System;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal static class MaterialEvaluation
{
    public static int Eval(Chessboard chessboard)
    {
        var score = 0;

        foreach (var position in Position.All)
        {
            var chessPiece = chessboard.GetChessPiece(position);

            score += chessPiece switch
            {
                ChessPiece.None => 0,
                ChessPiece.WhiteKing => 200,
                ChessPiece.WhiteQueen => 9,
                ChessPiece.WhiteRook => 5,
                ChessPiece.WhiteBishop => 3,
                ChessPiece.WhiteKnight => 3,
                ChessPiece.WhitePawn => 1,
                ChessPiece.BlackKing => -200,
                ChessPiece.BlackQueen => -9,
                ChessPiece.BlackRook => -5,
                ChessPiece.BlackBishop => -3,
                ChessPiece.BlackKnight => -3,
                ChessPiece.BlackPawn => -1,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return score;
    }
}