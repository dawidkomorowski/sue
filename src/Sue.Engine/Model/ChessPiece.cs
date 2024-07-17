using System;
using System.Runtime.CompilerServices;

namespace Sue.Engine.Model;

public enum ChessPiece
{
    None = 0,
    WhiteKing = 1,
    WhiteQueen = 2,
    WhiteRook = 3,
    WhiteBishop = 4,
    WhiteKnight = 5,
    WhitePawn = 6,
    BlackKing = 7,
    BlackQueen = 8,
    BlackRook = 9,
    BlackBishop = 10,
    BlackKnight = 11,
    BlackPawn = 12
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWhite(this ChessPiece chessPiece)
    {
        return chessPiece is >= ChessPiece.WhiteKing and <= ChessPiece.WhitePawn;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBlack(this ChessPiece chessPiece)
    {
        return chessPiece is >= ChessPiece.BlackKing and <= ChessPiece.BlackPawn;
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