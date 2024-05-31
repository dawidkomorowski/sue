using System;

namespace Sue.Engine.Model;

public enum ChessPiece
{
    None,
    WhiteKing,
    WhiteQueen,
    WhiteRook,
    WhiteBishop,
    WhiteKnight,
    WhitePawn,
    BlackKing,
    BlackQueen,
    BlackRook,
    BlackBishop,
    BlackKnight,
    BlackPawn
}

internal static class ChessPieceExtensions
{
    public static char ToChar(this ChessPiece chessPiece)
    {
        return chessPiece switch
        {
            ChessPiece.WhiteKing => 'K',
            ChessPiece.WhiteQueen => 'Q',
            ChessPiece.WhiteRook => 'R',
            ChessPiece.WhiteBishop => 'B',
            ChessPiece.WhiteKnight => 'N',
            ChessPiece.WhitePawn => 'P',
            ChessPiece.BlackKing => 'k',
            ChessPiece.BlackQueen => 'q',
            ChessPiece.BlackRook => 'r',
            ChessPiece.BlackBishop => 'b',
            ChessPiece.BlackKnight => 'n',
            ChessPiece.BlackPawn => 'p',
            _ => throw new ArgumentOutOfRangeException(nameof(chessPiece), chessPiece, null)
        };
    }

    public static bool IsWhite(this ChessPiece chessPiece)
    {
        return chessPiece switch
        {
            ChessPiece.WhiteKing => true,
            ChessPiece.WhiteQueen => true,
            ChessPiece.WhiteRook => true,
            ChessPiece.WhiteBishop => true,
            ChessPiece.WhiteKnight => true,
            ChessPiece.WhitePawn => true,
            _ => false
        };
    }

    public static bool IsBlack(this ChessPiece chessPiece)
    {
        return chessPiece switch
        {
            ChessPiece.BlackKing => true,
            ChessPiece.BlackQueen => true,
            ChessPiece.BlackRook => true,
            ChessPiece.BlackBishop => true,
            ChessPiece.BlackKnight => true,
            ChessPiece.BlackPawn => true,
            _ => false
        };
    }
}

internal static class CharExtensionsForChessPiece
{
    public static ChessPiece ToChessPiece(this char c)
    {
        return c switch
        {
            'K' => ChessPiece.WhiteKing,
            'Q' => ChessPiece.WhiteQueen,
            'R' => ChessPiece.WhiteRook,
            'B' => ChessPiece.WhiteBishop,
            'N' => ChessPiece.WhiteKnight,
            'P' => ChessPiece.WhitePawn,
            'k' => ChessPiece.BlackKing,
            'q' => ChessPiece.BlackQueen,
            'r' => ChessPiece.BlackRook,
            'b' => ChessPiece.BlackBishop,
            'n' => ChessPiece.BlackKnight,
            'p' => ChessPiece.BlackPawn,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }
}