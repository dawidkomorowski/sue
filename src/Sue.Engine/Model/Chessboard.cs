using System;

namespace Sue.Engine.Model;

internal sealed class Chessboard
{
    private readonly ChessPiece[] _chessboard = new ChessPiece[64];

    public Color ActiveColor { get; set; }
    public bool WhiteKingSideCastlingAvailable { get; set; }
    public bool WhiteQueenSideCastlingAvailable { get; set; }
    public bool BlackKingSideCastlingAvailable { get; set; }
    public bool BlackQueenSideCastlingAvailable { get; set; }
    public Position? EnPassantTargetPosition { get; set; }
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

    public static Chessboard FromFen(Fen fen)
    {
        var chessboard = new Chessboard();

        foreach (var position in Position.All)
        {
            chessboard.SetChessPiece(position, fen.GetChessPiece(position));
        }

        chessboard.ActiveColor = fen.ActiveColor;
        chessboard.WhiteKingSideCastlingAvailable = fen.WhiteKingSideCastlingAvailable;
        chessboard.WhiteQueenSideCastlingAvailable = fen.WhiteQueenSideCastlingAvailable;
        chessboard.BlackKingSideCastlingAvailable = fen.BlackKingSideCastlingAvailable;
        chessboard.BlackQueenSideCastlingAvailable = fen.BlackQueenSideCastlingAvailable;
        chessboard.EnPassantTargetPosition = fen.EnPassantTargetPosition;
        chessboard.HalfMoveClock = fen.HalfMoveClock;
        chessboard.FullMoveNumber = fen.FullMoveNumber;

        return chessboard;
    }

    public Fen ToFen()
    {
        var fen = new Fen();

        foreach (var position in Position.All)
        {
            fen.SetChessPiece(position, GetChessPiece(position));
        }

        fen.ActiveColor = ActiveColor;
        fen.WhiteKingSideCastlingAvailable = WhiteKingSideCastlingAvailable;
        fen.WhiteQueenSideCastlingAvailable = WhiteQueenSideCastlingAvailable;
        fen.BlackKingSideCastlingAvailable = BlackKingSideCastlingAvailable;
        fen.BlackQueenSideCastlingAvailable = BlackQueenSideCastlingAvailable;
        fen.EnPassantTargetPosition = EnPassantTargetPosition;
        fen.HalfMoveClock = HalfMoveClock;
        fen.FullMoveNumber = FullMoveNumber;

        return fen;
    }

    public void MakeMove(Move move)
    {
        var cpFrom = GetChessPiece(move.From);
        var cpTo = GetChessPiece(move.To);

        if (move.IsWhiteKingSideCastling)
        {
            if (cpFrom is ChessPiece.WhiteKing && WhiteKingSideCastlingAvailable)
            {
                SetChessPiece(new Position(File.E, Rank.One), ChessPiece.None);
                SetChessPiece(new Position(File.G, Rank.One), ChessPiece.WhiteKing);
                SetChessPiece(new Position(File.H, Rank.One), ChessPiece.None);
                SetChessPiece(new Position(File.F, Rank.One), ChessPiece.WhiteRook);
            }
            else
            {
                throw new InvalidOperationException($"Invalid move '{move.ToUci()}' in position '{ToFen()}'.");
            }
        }
        else
        {
            SetChessPiece(move.From, ChessPiece.None);
            SetChessPiece(move.To, cpFrom);
        }

        if (cpFrom is ChessPiece.WhiteKing)
        {
            WhiteKingSideCastlingAvailable = false;
            WhiteQueenSideCastlingAvailable = false;
        }

        if (cpFrom is ChessPiece.BlackKing)
        {
            BlackKingSideCastlingAvailable = false;
            BlackQueenSideCastlingAvailable = false;
        }

        // Update HalfMoveClock
        HalfMoveClock++;

        if (cpFrom is ChessPiece.WhitePawn || cpFrom is ChessPiece.BlackPawn)
        {
            HalfMoveClock = 0;
        }

        if ((cpFrom.IsWhite() && cpTo.IsBlack()) || (cpFrom.IsBlack() && cpTo.IsWhite()))
        {
            HalfMoveClock = 0;
        }

        // Update FullMoveNumber
        if (ActiveColor is Color.Black)
        {
            FullMoveNumber++;
        }

        // Update ActiveColor
        ActiveColor = ActiveColor.Opposite();
    }

    private static int GetIndex(Position position)
    {
        return position.File.Index() * 8 + position.Rank.Index();
    }
}