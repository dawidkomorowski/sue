using System;

namespace Sue.Engine.Model;

internal sealed class Fen
{
    private readonly ChessPiece[] _chessboard = new ChessPiece[64];

    public Color ActiveColor { get; set; }
    public bool WhiteKingSideCastlingAvailable { get; set; }
    public bool WhiteQueenSideCastlingAvailable { get; set; }
    public bool BlackKingSideCastlingAvailable { get; set; }
    public bool BlackQueenSideCastlingAvailable { get; set; }
    public Position? EnPassantTargetField { get; set; }
    public int HalfMoveClock { get; set; }
    public int FullMoveNumber { get; set; } = 1;

    public ChessPiece GetChessPiece(Position position)
    {
        return _chessboard[GetIndex(position)];
    }

    public void SetChessPiece(Position position, ChessPiece chessPiece)
    {
        _chessboard[GetIndex(position)] = chessPiece;
    }

    public static Fen FromString(string fenString)
    {
        throw new NotImplementedException("TODO");
    }

    public override string ToString()
    {
        throw new NotImplementedException("TODO");
    }

    private static int GetIndex(Position position)
    {
        return position.File.Index() * 8 + position.Rank.Index();
    }
}