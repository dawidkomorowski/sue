using System;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal static class MaterialEvaluation
{
    public static Score Eval(Chessboard chessboard)
    {
        var eval = 0;

        foreach (var position in Position.All)
        {
            var chessPiece = chessboard.GetChessPiece(position);

            // TODO Losing a king is not treated as mate.
            eval += chessPiece switch
            {
                ChessPiece.None => 0,
                ChessPiece.WhiteKing => 20000,
                ChessPiece.WhiteQueen => 900,
                ChessPiece.WhiteRook => 500,
                ChessPiece.WhiteBishop => 300,
                ChessPiece.WhiteKnight => 300,
                ChessPiece.WhitePawn => position.Rank.Index() - 1 + 100,
                ChessPiece.BlackKing => -20000,
                ChessPiece.BlackQueen => -900,
                ChessPiece.BlackRook => -500,
                ChessPiece.BlackBishop => -300,
                ChessPiece.BlackKnight => -300,
                ChessPiece.BlackPawn => -6 + position.Rank.Index() - 100,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return Score.CreateEval(eval);
    }
}