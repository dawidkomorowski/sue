using System;
using System.Collections.Generic;
using System.Diagnostics;

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

    #region MakeMove implementation

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

    #endregion

    public IReadOnlyList<Move> GetMoveCandidates()
    {
        var moves = new List<Move>();

        foreach (var position in Position.All)
        {
            var chessPiece = GetChessPiece(position);
            if (ActiveColor is Color.White && chessPiece.IsWhite() || ActiveColor is Color.Black && chessPiece.IsBlack())
            {
                switch (chessPiece)
                {
                    case ChessPiece.WhiteKing:
                    case ChessPiece.BlackKing:
                        break;
                    case ChessPiece.WhiteQueen:
                    case ChessPiece.BlackQueen:
                        break;
                    case ChessPiece.WhiteRook:
                    case ChessPiece.BlackRook:
                        break;
                    case ChessPiece.WhiteBishop:
                    case ChessPiece.BlackBishop:
                        break;
                    case ChessPiece.WhiteKnight:
                    case ChessPiece.BlackKnight:
                        AppendKnightMoves(position, moves);
                        break;
                    case ChessPiece.WhitePawn:
                        AppendWhitePawnMoves(position, moves);
                        break;
                    case ChessPiece.BlackPawn:
                        AppendBlackPawnMoves(position, moves);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        return moves.AsReadOnly();
    }

    #region GetMoveCandidates implementation

    private void AppendWhitePawnMoves(Position position, List<Move> moves)
    {
        var pawn = GetChessPiece(position);
        Debug.Assert(pawn is ChessPiece.WhitePawn, "pawn is ChessPiece.WhitePawn");

        var front = position.MoveUp();

        if (position.Rank is Rank.Seven)
        {
            if (GetChessPiece(front) is ChessPiece.None)
            {
                moves.Add(new Move(position, front, Promotion.Queen));
                moves.Add(new Move(position, front, Promotion.Rook));
                moves.Add(new Move(position, front, Promotion.Bishop));
                moves.Add(new Move(position, front, Promotion.Knight));
            }

            if (position.File is not File.A)
            {
                var frontLeft = front.MoveLeft();
                if (GetChessPiece(frontLeft).IsBlack())
                {
                    moves.Add(new Move(position, frontLeft, Promotion.Queen));
                    moves.Add(new Move(position, frontLeft, Promotion.Rook));
                    moves.Add(new Move(position, frontLeft, Promotion.Bishop));
                    moves.Add(new Move(position, frontLeft, Promotion.Knight));
                }
            }

            if (position.File is not File.H)
            {
                var frontRight = front.MoveRight();
                if (GetChessPiece(frontRight).IsBlack())
                {
                    moves.Add(new Move(position, frontRight, Promotion.Queen));
                    moves.Add(new Move(position, frontRight, Promotion.Rook));
                    moves.Add(new Move(position, frontRight, Promotion.Bishop));
                    moves.Add(new Move(position, frontRight, Promotion.Knight));
                }
            }
        }
        else
        {
            var front2 = front.MoveUp();

            if (position.Rank is Rank.Two && GetChessPiece(front) is ChessPiece.None && GetChessPiece(front2) is ChessPiece.None)
            {
                moves.Add(new Move(position, front2));
            }

            if (GetChessPiece(front) is ChessPiece.None)
            {
                moves.Add(new Move(position, front));
            }

            if (position.File is not File.A)
            {
                var frontLeft = front.MoveLeft();
                if (GetChessPiece(frontLeft).IsBlack())
                {
                    moves.Add(new Move(position, frontLeft));
                }
            }

            if (position.File is not File.H)
            {
                var frontRight = front.MoveRight();
                if (GetChessPiece(frontRight).IsBlack())
                {
                    moves.Add(new Move(position, frontRight));
                }
            }
        }
    }

    private void AppendBlackPawnMoves(Position position, List<Move> moves)
    {
        var pawn = GetChessPiece(position);
        Debug.Assert(pawn is ChessPiece.BlackPawn, "pawn is ChessPiece.BlackPawn");

        var front = position.MoveDown();

        if (position.Rank is Rank.Two)
        {
            if (GetChessPiece(front) is ChessPiece.None)
            {
                moves.Add(new Move(position, front, Promotion.Queen));
                moves.Add(new Move(position, front, Promotion.Rook));
                moves.Add(new Move(position, front, Promotion.Bishop));
                moves.Add(new Move(position, front, Promotion.Knight));
            }

            if (position.File is not File.A)
            {
                var frontLeft = front.MoveLeft();
                if (GetChessPiece(frontLeft).IsWhite())
                {
                    moves.Add(new Move(position, frontLeft, Promotion.Queen));
                    moves.Add(new Move(position, frontLeft, Promotion.Rook));
                    moves.Add(new Move(position, frontLeft, Promotion.Bishop));
                    moves.Add(new Move(position, frontLeft, Promotion.Knight));
                }
            }

            if (position.File is not File.H)
            {
                var frontRight = front.MoveRight();
                if (GetChessPiece(frontRight).IsWhite())
                {
                    moves.Add(new Move(position, frontRight, Promotion.Queen));
                    moves.Add(new Move(position, frontRight, Promotion.Rook));
                    moves.Add(new Move(position, frontRight, Promotion.Bishop));
                    moves.Add(new Move(position, frontRight, Promotion.Knight));
                }
            }
        }
        else
        {
            var front2 = front.MoveDown();

            if (position.Rank is Rank.Seven && GetChessPiece(front) is ChessPiece.None && GetChessPiece(front2) is ChessPiece.None)
            {
                moves.Add(new Move(position, front2));
            }

            if (GetChessPiece(front) is ChessPiece.None)
            {
                moves.Add(new Move(position, front));
            }

            if (position.File is not File.A)
            {
                var frontLeft = front.MoveLeft();
                if (GetChessPiece(frontLeft).IsWhite())
                {
                    moves.Add(new Move(position, frontLeft));
                }
            }

            if (position.File is not File.H)
            {
                var frontRight = front.MoveRight();
                if (GetChessPiece(frontRight).IsWhite())
                {
                    moves.Add(new Move(position, frontRight));
                }
            }
        }
    }

    private void AppendKnightMoves(Position position, List<Move> moves)
    {
        var knight = GetChessPiece(position);
        Debug.Assert(knight is ChessPiece.WhiteKnight or ChessPiece.BlackKnight, "knight is ChessPiece.WhiteKnight or ChessPiece.BlackKnight");

        ReadOnlySpan<(int right, int up)> offsets = [(-1, 2), (1, 2), (-1, -2), (1, -2), (-2, 1), (-2, -1), (2, 1), (2, -1)];
        foreach (var (right, up) in offsets)
        {
            var fileIndex = position.File.Index() + right;
            var rankIndex = position.Rank.Index() + up;
            if (fileIndex < 0 || fileIndex > 7 || rankIndex < 0 || rankIndex > 7)
            {
                continue;
            }

            var targetPosition = position.MoveBy(right, up);
            var chessPiece = GetChessPiece(targetPosition);
            if ((knight.IsWhite() && !chessPiece.IsWhite()) || (knight.IsBlack() && !chessPiece.IsBlack()))
            {
                moves.Add(new Move(position, targetPosition));
            }
        }
    }

    #endregion

    private static int GetIndex(Position position)
    {
        return position.File.Index() * 8 + position.Rank.Index();
    }
}