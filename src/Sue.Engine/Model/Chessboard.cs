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

        if (cpFrom is ChessPiece.None)
        {
            throw CreateInvalidMoveError(move);
        }

        if (ActiveColor is Color.White && cpFrom.IsBlack())
        {
            throw CreateInvalidMoveError(move);
        }

        if (ActiveColor is Color.Black && cpFrom.IsWhite())
        {
            throw CreateInvalidMoveError(move);
        }

        if (cpFrom.IsWhite() && cpTo.IsWhite())
        {
            throw CreateInvalidMoveError(move);
        }

        if (cpFrom.IsBlack() && cpTo.IsBlack())
        {
            throw CreateInvalidMoveError(move);
        }

        PerformMove(move, cpFrom);
        UpdateCastlingAvailability(move, cpFrom);
        UpdateEnPassantTargetPosition(move, cpFrom);
        UpdateHalfMoveClock(cpFrom, cpTo);

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

    private InvalidOperationException CreateInvalidMoveError(Move move)
    {
        return new InvalidOperationException($"Invalid move '{move.ToUci()}' in position '{ToFen()}'.");
    }

    private void PerformMove(Move move, ChessPiece cpFrom)
    {
        if (cpFrom is ChessPiece.WhiteKing && move.From is { File: File.E, Rank: Rank.One } && move.To is { File: File.G, Rank: Rank.One })
        {
            if (WhiteKingSideCastlingAvailable)
            {
                SetChessPiece(new Position(File.E, Rank.One), ChessPiece.None);
                SetChessPiece(new Position(File.G, Rank.One), ChessPiece.WhiteKing);
                SetChessPiece(new Position(File.H, Rank.One), ChessPiece.None);
                SetChessPiece(new Position(File.F, Rank.One), ChessPiece.WhiteRook);
            }
            else
            {
                throw CreateInvalidMoveError(move);
            }
        }
        else if (cpFrom is ChessPiece.WhiteKing && move.From is { File: File.E, Rank: Rank.One } && move.To is { File: File.C, Rank: Rank.One })
        {
            if (WhiteQueenSideCastlingAvailable)
            {
                SetChessPiece(new Position(File.E, Rank.One), ChessPiece.None);
                SetChessPiece(new Position(File.C, Rank.One), ChessPiece.WhiteKing);
                SetChessPiece(new Position(File.A, Rank.One), ChessPiece.None);
                SetChessPiece(new Position(File.D, Rank.One), ChessPiece.WhiteRook);
            }
            else
            {
                throw CreateInvalidMoveError(move);
            }
        }
        else if (cpFrom is ChessPiece.BlackKing && move.From is { File: File.E, Rank: Rank.Eight } && move.To is { File: File.G, Rank: Rank.Eight })
        {
            if (BlackKingSideCastlingAvailable)
            {
                SetChessPiece(new Position(File.E, Rank.Eight), ChessPiece.None);
                SetChessPiece(new Position(File.G, Rank.Eight), ChessPiece.BlackKing);
                SetChessPiece(new Position(File.H, Rank.Eight), ChessPiece.None);
                SetChessPiece(new Position(File.F, Rank.Eight), ChessPiece.BlackRook);
            }
            else
            {
                throw CreateInvalidMoveError(move);
            }
        }
        else if (cpFrom is ChessPiece.BlackKing && move.From is { File: File.E, Rank: Rank.Eight } && move.To is { File: File.C, Rank: Rank.Eight })
        {
            if (BlackQueenSideCastlingAvailable)
            {
                SetChessPiece(new Position(File.E, Rank.Eight), ChessPiece.None);
                SetChessPiece(new Position(File.C, Rank.Eight), ChessPiece.BlackKing);
                SetChessPiece(new Position(File.A, Rank.Eight), ChessPiece.None);
                SetChessPiece(new Position(File.D, Rank.Eight), ChessPiece.BlackRook);
            }
            else
            {
                throw CreateInvalidMoveError(move);
            }
        }
        else if (cpFrom is ChessPiece.WhitePawn && EnPassantTargetPosition.HasValue && move.To == EnPassantTargetPosition.Value)
        {
            SetChessPiece(move.From, ChessPiece.None);
            SetChessPiece(move.To, cpFrom);
            SetChessPiece(new Position(EnPassantTargetPosition.Value.File, EnPassantTargetPosition.Value.Rank.Add(-1)), ChessPiece.None);
        }
        else if (cpFrom is ChessPiece.BlackPawn && EnPassantTargetPosition.HasValue && move.To == EnPassantTargetPosition.Value)
        {
            SetChessPiece(move.From, ChessPiece.None);
            SetChessPiece(move.To, cpFrom);
            SetChessPiece(new Position(EnPassantTargetPosition.Value.File, EnPassantTargetPosition.Value.Rank.Add(1)), ChessPiece.None);
        }
        else if (cpFrom is ChessPiece.WhitePawn && move.Promotion is not Promotion.None)
        {
            SetChessPiece(move.From, ChessPiece.None);
            var cpPromoted = move.Promotion switch
            {
                Promotion.Queen => ChessPiece.WhiteQueen,
                Promotion.Rook => ChessPiece.WhiteRook,
                Promotion.Bishop => ChessPiece.WhiteBishop,
                Promotion.Knight => ChessPiece.WhiteKnight,
                _ => throw new ArgumentOutOfRangeException()
            };
            SetChessPiece(move.To, cpPromoted);
        }
        else if (cpFrom is ChessPiece.BlackPawn && move.Promotion is not Promotion.None)
        {
            SetChessPiece(move.From, ChessPiece.None);
            var cpPromoted = move.Promotion switch
            {
                Promotion.Queen => ChessPiece.BlackQueen,
                Promotion.Rook => ChessPiece.BlackRook,
                Promotion.Bishop => ChessPiece.BlackBishop,
                Promotion.Knight => ChessPiece.BlackKnight,
                _ => throw new ArgumentOutOfRangeException()
            };
            SetChessPiece(move.To, cpPromoted);
        }
        else
        {
            SetChessPiece(move.From, ChessPiece.None);
            SetChessPiece(move.To, cpFrom);
        }
    }

    private void UpdateCastlingAvailability(Move move, ChessPiece cpFrom)
    {
        if (cpFrom is ChessPiece.WhiteRook && move.From is { File: File.H, Rank: Rank.One })
        {
            WhiteKingSideCastlingAvailable = false;
        }

        if (cpFrom is ChessPiece.WhiteRook && move.From is { File: File.A, Rank: Rank.One })
        {
            WhiteQueenSideCastlingAvailable = false;
        }

        if (cpFrom is ChessPiece.WhiteKing)
        {
            WhiteKingSideCastlingAvailable = false;
            WhiteQueenSideCastlingAvailable = false;
        }

        if (cpFrom is ChessPiece.BlackRook && move.From is { File: File.H, Rank: Rank.Eight })
        {
            BlackKingSideCastlingAvailable = false;
        }

        if (cpFrom is ChessPiece.BlackRook && move.From is { File: File.A, Rank: Rank.Eight })
        {
            BlackQueenSideCastlingAvailable = false;
        }

        if (cpFrom is ChessPiece.BlackKing)
        {
            BlackKingSideCastlingAvailable = false;
            BlackQueenSideCastlingAvailable = false;
        }
    }

    private void UpdateEnPassantTargetPosition(Move move, ChessPiece cpFrom)
    {
        EnPassantTargetPosition = null;

        if (cpFrom is ChessPiece.WhitePawn && move.From.Rank is Rank.Two && move.To.Rank is Rank.Four)
        {
            var fileIndex = move.To.File.Index();
            var left = fileIndex - 1;
            var right = fileIndex + 1;

            if (left >= 0)
            {
                var cpLeft = GetChessPiece(new Position(left.ToFile(), Rank.Four));
                if (cpLeft is ChessPiece.BlackPawn)
                {
                    EnPassantTargetPosition = new Position(move.From.File, Rank.Three);
                }
            }

            if (right < 8)
            {
                var cpRight = GetChessPiece(new Position(right.ToFile(), Rank.Four));
                if (cpRight is ChessPiece.BlackPawn)
                {
                    EnPassantTargetPosition = new Position(move.From.File, Rank.Three);
                }
            }
        }

        if (cpFrom is ChessPiece.BlackPawn && move.From.Rank is Rank.Seven && move.To.Rank is Rank.Five)
        {
            var fileIndex = move.To.File.Index();
            var left = fileIndex - 1;
            var right = fileIndex + 1;

            if (left >= 0)
            {
                var cpLeft = GetChessPiece(new Position(left.ToFile(), Rank.Five));
                if (cpLeft is ChessPiece.WhitePawn)
                {
                    EnPassantTargetPosition = new Position(move.From.File, Rank.Six);
                }
            }

            if (right < 8)
            {
                var cpRight = GetChessPiece(new Position(right.ToFile(), Rank.Five));
                if (cpRight is ChessPiece.WhitePawn)
                {
                    EnPassantTargetPosition = new Position(move.From.File, Rank.Six);
                }
            }
        }
    }

    private void UpdateHalfMoveClock(ChessPiece cpFrom, ChessPiece cpTo)
    {
        HalfMoveClock++;

        if (cpFrom is ChessPiece.WhitePawn or ChessPiece.BlackPawn)
        {
            HalfMoveClock = 0;
        }

        if ((cpFrom.IsWhite() && cpTo.IsBlack()) || (cpFrom.IsBlack() && cpTo.IsWhite()))
        {
            HalfMoveClock = 0;
        }
    }
}