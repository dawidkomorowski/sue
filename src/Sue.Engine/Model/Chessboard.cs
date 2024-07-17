using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Sue.Engine.Model;

internal sealed class Chessboard
{
    private readonly ChessPiece[] _chessboard = new ChessPiece[64];
    private readonly Stack<RevertMoveData> _revertMoveStack = new(128);

    public const int MoveBufferSize = 512;

    public Color ActiveColor { get; set; }
    public bool WhiteKingSideCastlingAvailable { get; set; }
    public bool WhiteQueenSideCastlingAvailable { get; set; }
    public bool BlackKingSideCastlingAvailable { get; set; }
    public bool BlackQueenSideCastlingAvailable { get; set; }
    public Position? EnPassantTargetPosition { get; set; }
    public int HalfMoveClock { get; set; }
    public int FullMoveNumber { get; set; } = 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ChessPiece GetChessPiece(in Position position)
    {
        return _chessboard[GetIndex(position)];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetChessPiece(in Position position, ChessPiece chessPiece)
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

    public bool HasKingInCheck(Color kingColor)
    {
        Position? kingPosition = null;

        foreach (var position in Position.All)
        {
            if (kingColor is Color.White && GetChessPiece(position) is ChessPiece.WhiteKing)
            {
                kingPosition = position;
                break;
            }

            if (kingColor is Color.Black && GetChessPiece(position) is ChessPiece.BlackKing)
            {
                kingPosition = position;
                break;
            }
        }

        if (!kingPosition.HasValue)
        {
            return false;
        }

        return IsAttackedBy(kingPosition.Value, kingColor.Opposite());
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

        var revertMoveData = CaptureRevertMoveData(move, cpFrom, cpTo);
        PerformMove(move, cpFrom, ref revertMoveData);
        UpdateCastlingAvailability(move, cpFrom, cpTo);
        UpdateEnPassantTargetPosition(move, cpFrom);
        UpdateHalfMoveClock(cpFrom, cpTo);

        // Update FullMoveNumber
        if (ActiveColor is Color.Black)
        {
            FullMoveNumber++;
        }

        // Update ActiveColor
        ActiveColor = ActiveColor.Opposite();

        // Push revert move data onto stack
        _revertMoveStack.Push(revertMoveData);
    }

    #region MakeMove implementation

    private InvalidOperationException CreateInvalidMoveError(Move move)
    {
        return new InvalidOperationException($"Invalid move '{move.ToUci()}' in position '{ToFen()}'.");
    }

    private RevertMoveData CaptureRevertMoveData(Move move, ChessPiece cpFrom, ChessPiece cpTo)
    {
        return new RevertMoveData
        {
            Position1 = move.From,
            ChessPiece1 = cpFrom,
            Position2 = move.To,
            ChessPiece2 = cpTo,
            ActiveColor = ActiveColor,
            WhiteKingSideCastlingAvailable = WhiteKingSideCastlingAvailable,
            WhiteQueenSideCastlingAvailable = WhiteQueenSideCastlingAvailable,
            BlackKingSideCastlingAvailable = BlackKingSideCastlingAvailable,
            BlackQueenSideCastlingAvailable = BlackQueenSideCastlingAvailable,
            EnPassantTargetPosition = EnPassantTargetPosition,
            HalfMoveClock = HalfMoveClock,
            FullMoveNumber = FullMoveNumber
        };
    }

    private void PerformMove(Move move, ChessPiece cpFrom, ref RevertMoveData revertMoveData)
    {
        if (cpFrom is ChessPiece.WhiteKing && move.From is { File: File.E, Rank: Rank.One } && move.To is { File: File.G, Rank: Rank.One })
        {
            if (WhiteKingSideCastlingAvailable)
            {
                revertMoveData.Position3 = new Position(File.H, Rank.One);
                revertMoveData.ChessPiece3 = GetChessPiece(revertMoveData.Position3.Value);
                revertMoveData.Position4 = new Position(File.F, Rank.One);
                revertMoveData.ChessPiece4 = GetChessPiece(revertMoveData.Position4.Value);

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
                revertMoveData.Position3 = new Position(File.A, Rank.One);
                revertMoveData.ChessPiece3 = GetChessPiece(revertMoveData.Position3.Value);
                revertMoveData.Position4 = new Position(File.D, Rank.One);
                revertMoveData.ChessPiece4 = GetChessPiece(revertMoveData.Position4.Value);

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
                revertMoveData.Position3 = new Position(File.H, Rank.Eight);
                revertMoveData.ChessPiece3 = GetChessPiece(revertMoveData.Position3.Value);
                revertMoveData.Position4 = new Position(File.F, Rank.Eight);
                revertMoveData.ChessPiece4 = GetChessPiece(revertMoveData.Position4.Value);

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
                revertMoveData.Position3 = new Position(File.A, Rank.Eight);
                revertMoveData.ChessPiece3 = GetChessPiece(revertMoveData.Position3.Value);
                revertMoveData.Position4 = new Position(File.D, Rank.Eight);
                revertMoveData.ChessPiece4 = GetChessPiece(revertMoveData.Position4.Value);

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
            var capturedPosition = new Position(EnPassantTargetPosition.Value.File, EnPassantTargetPosition.Value.Rank.Add(-1));
            revertMoveData.Position3 = capturedPosition;
            revertMoveData.ChessPiece3 = GetChessPiece(revertMoveData.Position3.Value);

            SetChessPiece(move.From, ChessPiece.None);
            SetChessPiece(move.To, cpFrom);
            SetChessPiece(capturedPosition, ChessPiece.None);
        }
        else if (cpFrom is ChessPiece.BlackPawn && EnPassantTargetPosition.HasValue && move.To == EnPassantTargetPosition.Value)
        {
            var capturedPosition = new Position(EnPassantTargetPosition.Value.File, EnPassantTargetPosition.Value.Rank.Add(1));
            revertMoveData.Position3 = capturedPosition;
            revertMoveData.ChessPiece3 = GetChessPiece(revertMoveData.Position3.Value);

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

    private void UpdateCastlingAvailability(Move move, ChessPiece cpFrom, ChessPiece cpTo)
    {
        if (cpFrom is ChessPiece.WhiteRook && move.From is { File: File.H, Rank: Rank.One })
        {
            WhiteKingSideCastlingAvailable = false;
        }

        if (cpTo is ChessPiece.WhiteRook && move.To is { File: File.H, Rank: Rank.One })
        {
            WhiteKingSideCastlingAvailable = false;
        }

        if (cpFrom is ChessPiece.WhiteRook && move.From is { File: File.A, Rank: Rank.One })
        {
            WhiteQueenSideCastlingAvailable = false;
        }

        if (cpTo is ChessPiece.WhiteRook && move.To is { File: File.A, Rank: Rank.One })
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

        if (cpTo is ChessPiece.BlackRook && move.To is { File: File.H, Rank: Rank.Eight })
        {
            BlackKingSideCastlingAvailable = false;
        }

        if (cpFrom is ChessPiece.BlackRook && move.From is { File: File.A, Rank: Rank.Eight })
        {
            BlackQueenSideCastlingAvailable = false;
        }

        if (cpTo is ChessPiece.BlackRook && move.To is { File: File.A, Rank: Rank.Eight })
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

    public void RevertMove()
    {
        if (_revertMoveStack.Count == 0)
        {
            throw new InvalidOperationException("There are no moves to revert.");
        }

        var data = _revertMoveStack.Pop();
        SetChessPiece(data.Position1, data.ChessPiece1);
        SetChessPiece(data.Position2, data.ChessPiece2);

        if (data.Position3.HasValue)
        {
            SetChessPiece(data.Position3.Value, data.ChessPiece3);
        }

        if (data.Position4.HasValue)
        {
            SetChessPiece(data.Position4.Value, data.ChessPiece4);
        }

        ActiveColor = data.ActiveColor;
        WhiteKingSideCastlingAvailable = data.WhiteKingSideCastlingAvailable;
        WhiteQueenSideCastlingAvailable = data.WhiteQueenSideCastlingAvailable;
        BlackKingSideCastlingAvailable = data.BlackKingSideCastlingAvailable;
        BlackQueenSideCastlingAvailable = data.BlackQueenSideCastlingAvailable;
        EnPassantTargetPosition = data.EnPassantTargetPosition;
        HalfMoveClock = data.HalfMoveClock;
        FullMoveNumber = data.FullMoveNumber;
    }

    public int GetMoveCandidates(Span<Move> moveBuffer)
    {
        Debug.Assert(moveBuffer.Length >= MoveBufferSize, $"moveBuffer.Length >= MoveBufferSize ({MoveBufferSize})");

        var index = 0;

        foreach (var position in Position.All)
        {
            var chessPiece = GetChessPiece(position);
            if (ActiveColor is Color.White && chessPiece.IsWhite() || ActiveColor is Color.Black && chessPiece.IsBlack())
            {
                switch (chessPiece)
                {
                    case ChessPiece.WhiteKing:
                    case ChessPiece.BlackKing:
                        index = AppendKingMoves(position, moveBuffer, index);
                        index = AppendCastlingMoves(position, moveBuffer, index);
                        break;
                    case ChessPiece.WhiteQueen:
                    case ChessPiece.BlackQueen:
                        index = AppendRookMoves(position, moveBuffer, index);
                        index = AppendBishopMoves(position, moveBuffer, index);
                        break;
                    case ChessPiece.WhiteRook:
                    case ChessPiece.BlackRook:
                        index = AppendRookMoves(position, moveBuffer, index);
                        break;
                    case ChessPiece.WhiteBishop:
                    case ChessPiece.BlackBishop:
                        index = AppendBishopMoves(position, moveBuffer, index);
                        break;
                    case ChessPiece.WhiteKnight:
                    case ChessPiece.BlackKnight:
                        index = AppendKnightMoves(position, moveBuffer, index);
                        break;
                    case ChessPiece.WhitePawn:
                        index = AppendWhitePawnMoves(position, moveBuffer, index);
                        break;
                    case ChessPiece.BlackPawn:
                        index = AppendBlackPawnMoves(position, moveBuffer, index);
                        break;
                    case ChessPiece.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        return index;
    }

    #region GetMoveCandidates implementation

    private int AppendWhitePawnMoves(Position position, Span<Move> moveBuffer, int index)
    {
        var pawn = GetChessPiece(position);
        Debug.Assert(pawn is ChessPiece.WhitePawn, "pawn is ChessPiece.WhitePawn");

        var front = position.MoveUp();

        if (position.Rank is Rank.Seven)
        {
            if (GetChessPiece(front) is ChessPiece.None)
            {
                moveBuffer[index++] = new Move(position, front, Promotion.Queen);
                moveBuffer[index++] = new Move(position, front, Promotion.Rook);
                moveBuffer[index++] = new Move(position, front, Promotion.Bishop);
                moveBuffer[index++] = new Move(position, front, Promotion.Knight);
            }

            if (position.File is not File.A)
            {
                var frontLeft = front.MoveLeft();
                if (GetChessPiece(frontLeft).IsBlack())
                {
                    moveBuffer[index++] = new Move(position, frontLeft, Promotion.Queen);
                    moveBuffer[index++] = new Move(position, frontLeft, Promotion.Rook);
                    moveBuffer[index++] = new Move(position, frontLeft, Promotion.Bishop);
                    moveBuffer[index++] = new Move(position, frontLeft, Promotion.Knight);
                }
            }

            if (position.File is not File.H)
            {
                var frontRight = front.MoveRight();
                if (GetChessPiece(frontRight).IsBlack())
                {
                    moveBuffer[index++] = new Move(position, frontRight, Promotion.Queen);
                    moveBuffer[index++] = new Move(position, frontRight, Promotion.Rook);
                    moveBuffer[index++] = new Move(position, frontRight, Promotion.Bishop);
                    moveBuffer[index++] = new Move(position, frontRight, Promotion.Knight);
                }
            }
        }
        else
        {
            var front2 = front.MoveUp();

            if (position.Rank is Rank.Two && GetChessPiece(front) is ChessPiece.None && GetChessPiece(front2) is ChessPiece.None)
            {
                moveBuffer[index++] = new Move(position, front2);
            }

            if (GetChessPiece(front) is ChessPiece.None)
            {
                moveBuffer[index++] = new Move(position, front);
            }

            if (position.File is not File.A)
            {
                var frontLeft = front.MoveLeft();
                if (GetChessPiece(frontLeft).IsBlack())
                {
                    moveBuffer[index++] = new Move(position, frontLeft);
                }

                if (position.Rank is Rank.Five && frontLeft == EnPassantTargetPosition)
                {
                    moveBuffer[index++] = new Move(position, frontLeft);
                }
            }

            if (position.File is not File.H)
            {
                var frontRight = front.MoveRight();
                if (GetChessPiece(frontRight).IsBlack())
                {
                    moveBuffer[index++] = new Move(position, frontRight);
                }

                if (position.Rank is Rank.Five && frontRight == EnPassantTargetPosition)
                {
                    moveBuffer[index++] = new Move(position, frontRight);
                }
            }
        }

        return index;
    }

    private int AppendBlackPawnMoves(Position position, Span<Move> moveBuffer, int index)
    {
        var pawn = GetChessPiece(position);
        Debug.Assert(pawn is ChessPiece.BlackPawn, "pawn is ChessPiece.BlackPawn");

        var front = position.MoveDown();

        if (position.Rank is Rank.Two)
        {
            if (GetChessPiece(front) is ChessPiece.None)
            {
                moveBuffer[index++] = new Move(position, front, Promotion.Queen);
                moveBuffer[index++] = new Move(position, front, Promotion.Rook);
                moveBuffer[index++] = new Move(position, front, Promotion.Bishop);
                moveBuffer[index++] = new Move(position, front, Promotion.Knight);
            }

            if (position.File is not File.A)
            {
                var frontLeft = front.MoveLeft();
                if (GetChessPiece(frontLeft).IsWhite())
                {
                    moveBuffer[index++] = new Move(position, frontLeft, Promotion.Queen);
                    moveBuffer[index++] = new Move(position, frontLeft, Promotion.Rook);
                    moveBuffer[index++] = new Move(position, frontLeft, Promotion.Bishop);
                    moveBuffer[index++] = new Move(position, frontLeft, Promotion.Knight);
                }
            }

            if (position.File is not File.H)
            {
                var frontRight = front.MoveRight();
                if (GetChessPiece(frontRight).IsWhite())
                {
                    moveBuffer[index++] = new Move(position, frontRight, Promotion.Queen);
                    moveBuffer[index++] = new Move(position, frontRight, Promotion.Rook);
                    moveBuffer[index++] = new Move(position, frontRight, Promotion.Bishop);
                    moveBuffer[index++] = new Move(position, frontRight, Promotion.Knight);
                }
            }
        }
        else
        {
            var front2 = front.MoveDown();

            if (position.Rank is Rank.Seven && GetChessPiece(front) is ChessPiece.None && GetChessPiece(front2) is ChessPiece.None)
            {
                moveBuffer[index++] = new Move(position, front2);
            }

            if (GetChessPiece(front) is ChessPiece.None)
            {
                moveBuffer[index++] = new Move(position, front);
            }

            if (position.File is not File.A)
            {
                var frontLeft = front.MoveLeft();
                if (GetChessPiece(frontLeft).IsWhite())
                {
                    moveBuffer[index++] = new Move(position, frontLeft);
                }

                if (position.Rank is Rank.Four && frontLeft == EnPassantTargetPosition)
                {
                    moveBuffer[index++] = new Move(position, frontLeft);
                }
            }

            if (position.File is not File.H)
            {
                var frontRight = front.MoveRight();
                if (GetChessPiece(frontRight).IsWhite())
                {
                    moveBuffer[index++] = new Move(position, frontRight);
                }

                if (position.Rank is Rank.Four && frontRight == EnPassantTargetPosition)
                {
                    moveBuffer[index++] = new Move(position, frontRight);
                }
            }
        }

        return index;
    }

    private int AppendKnightMoves(Position position, Span<Move> moveBuffer, int index)
    {
        var knight = GetChessPiece(position);
        Debug.Assert(knight is ChessPiece.WhiteKnight or ChessPiece.BlackKnight, "knight is ChessPiece.WhiteKnight or ChessPiece.BlackKnight");

        ReadOnlySpan<(int right, int up)> offsets =
        [
            (-1, 2), (1, 2), (-1, -2), (1, -2), (-2, 1), (-2, -1), (2, 1), (2, -1)
        ];
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
                moveBuffer[index++] = new Move(position, targetPosition);
            }
        }

        return index;
    }

    private int AppendBishopMoves(Position position, Span<Move> moveBuffer, int index)
    {
        var topRightMaximumOffset = Math.Min(File.H.Index() - position.File.Index(), Rank.Eight.Index() - position.Rank.Index());
        var topLeftMaximumOffset = Math.Min(position.File.Index() - File.A.Index(), Rank.Eight.Index() - position.Rank.Index());
        var bottomRightMaximumOffset = Math.Min(File.H.Index() - position.File.Index(), position.Rank.Index() - Rank.One.Index());
        var bottomLeftMaximumOffset = Math.Min(position.File.Index() - File.A.Index(), position.Rank.Index() - Rank.One.Index());

        for (var offset = 1; offset <= topRightMaximumOffset; offset++)
        {
            var targetPosition = position.MoveBy(offset, offset);
            if (ShouldBreak_AndAlso_TryAddMove(position, targetPosition, moveBuffer, ref index))
            {
                break;
            }
        }

        for (var offset = 1; offset <= topLeftMaximumOffset; offset++)
        {
            var targetPosition = position.MoveBy(-offset, offset);
            if (ShouldBreak_AndAlso_TryAddMove(position, targetPosition, moveBuffer, ref index))
            {
                break;
            }
        }

        for (var offset = 1; offset <= bottomRightMaximumOffset; offset++)
        {
            var targetPosition = position.MoveBy(offset, -offset);
            if (ShouldBreak_AndAlso_TryAddMove(position, targetPosition, moveBuffer, ref index))
            {
                break;
            }
        }

        for (var offset = 1; offset <= bottomLeftMaximumOffset; offset++)
        {
            var targetPosition = position.MoveBy(-offset, -offset);
            if (ShouldBreak_AndAlso_TryAddMove(position, targetPosition, moveBuffer, ref index))
            {
                break;
            }
        }

        return index;
    }

    private int AppendRookMoves(Position position, Span<Move> moveBuffer, int index)
    {
        for (var fileIndex = position.File.Index() + 1; fileIndex <= File.H.Index(); fileIndex++)
        {
            if (ShouldBreak_AndAlso_TryAddMove(position, new Position(fileIndex.ToFile(), position.Rank), moveBuffer, ref index))
            {
                break;
            }
        }

        for (var fileIndex = position.File.Index() - 1; fileIndex >= File.A.Index(); fileIndex--)
        {
            if (ShouldBreak_AndAlso_TryAddMove(position, new Position(fileIndex.ToFile(), position.Rank), moveBuffer, ref index))
            {
                break;
            }
        }

        for (var rankIndex = position.Rank.Index() + 1; rankIndex <= Rank.Eight.Index(); rankIndex++)
        {
            if (ShouldBreak_AndAlso_TryAddMove(position, new Position(position.File, rankIndex.ToRank()), moveBuffer, ref index))
            {
                break;
            }
        }

        for (var rankIndex = position.Rank.Index() - 1; rankIndex >= Rank.One.Index(); rankIndex--)
        {
            if (ShouldBreak_AndAlso_TryAddMove(position, new Position(position.File, rankIndex.ToRank()), moveBuffer, ref index))
            {
                break;
            }
        }

        return index;
    }

    private bool ShouldBreak_AndAlso_TryAddMove(Position position, Position targetPosition, Span<Move> moveBuffer, ref int index)
    {
        var myChessPiece = GetChessPiece(position);
        var chessPiece = GetChessPiece(targetPosition);

        if ((myChessPiece.IsWhite() && chessPiece.IsWhite()) || (myChessPiece.IsBlack() && chessPiece.IsBlack()))
        {
            return true;
        }

        moveBuffer[index++] = new Move(position, targetPosition);

        return chessPiece is not ChessPiece.None;
    }

    private int AppendKingMoves(Position position, Span<Move> moveBuffer, int index)
    {
        var king = GetChessPiece(position);
        Debug.Assert(king is ChessPiece.WhiteKing or ChessPiece.BlackKing, "king is ChessPiece.WhiteKing or ChessPiece.BlackKing");

        ReadOnlySpan<(int right, int up)> offsets =
        [
            (-1, 1), (-1, 0), (-1, -1), (0, 1), (0, -1), (1, 1), (1, 0), (1, -1)
        ];
        foreach (var (right, up) in offsets)
        {
            var fileIndex = position.File.Index() + right;
            var rankIndex = position.Rank.Index() + up;
            if (fileIndex < 0 || fileIndex > 7 || rankIndex < 0 || rankIndex > 7)
            {
                continue;
            }

            var targetPosition = position.MoveBy(right, up);

            if (king.IsWhite() && IsAttackedBy(targetPosition, Color.Black))
            {
                continue;
            }

            if (king.IsBlack() && IsAttackedBy(targetPosition, Color.White))
            {
                continue;
            }

            var chessPiece = GetChessPiece(targetPosition);
            if ((king.IsWhite() && !chessPiece.IsWhite()) || (king.IsBlack() && !chessPiece.IsBlack()))
            {
                moveBuffer[index++] = new Move(position, targetPosition);
            }
        }

        return index;
    }

    private int AppendCastlingMoves(Position position, Span<Move> moveBuffer, int index)
    {
        var king = GetChessPiece(position);
        Debug.Assert(king is ChessPiece.WhiteKing or ChessPiece.BlackKing, "king is ChessPiece.WhiteKing or ChessPiece.BlackKing");

        if (king is ChessPiece.WhiteKing && WhiteKingSideCastlingAvailable)
        {
            var canCastle = true;

            canCastle = canCastle && GetChessPiece(new Position(File.F, Rank.One)) is ChessPiece.None;
            canCastle = canCastle && GetChessPiece(new Position(File.G, Rank.One)) is ChessPiece.None;

            canCastle = canCastle && !IsAttackedBy(new Position(File.E, Rank.One), Color.Black);
            canCastle = canCastle && !IsAttackedBy(new Position(File.F, Rank.One), Color.Black);
            canCastle = canCastle && !IsAttackedBy(new Position(File.G, Rank.One), Color.Black);

            if (canCastle)
            {
                moveBuffer[index++] = new Move(new Position(File.E, Rank.One), new Position(File.G, Rank.One));
            }
        }

        if (king is ChessPiece.WhiteKing && WhiteQueenSideCastlingAvailable)
        {
            var canCastle = true;

            canCastle = canCastle && GetChessPiece(new Position(File.D, Rank.One)) is ChessPiece.None;
            canCastle = canCastle && GetChessPiece(new Position(File.C, Rank.One)) is ChessPiece.None;
            canCastle = canCastle && GetChessPiece(new Position(File.B, Rank.One)) is ChessPiece.None;

            canCastle = canCastle && !IsAttackedBy(new Position(File.E, Rank.One), Color.Black);
            canCastle = canCastle && !IsAttackedBy(new Position(File.D, Rank.One), Color.Black);
            canCastle = canCastle && !IsAttackedBy(new Position(File.C, Rank.One), Color.Black);

            if (canCastle)
            {
                moveBuffer[index++] = new Move(new Position(File.E, Rank.One), new Position(File.C, Rank.One));
            }
        }

        if (king is ChessPiece.BlackKing && BlackKingSideCastlingAvailable)
        {
            var canCastle = true;

            canCastle = canCastle && GetChessPiece(new Position(File.F, Rank.Eight)) is ChessPiece.None;
            canCastle = canCastle && GetChessPiece(new Position(File.G, Rank.Eight)) is ChessPiece.None;

            canCastle = canCastle && !IsAttackedBy(new Position(File.E, Rank.Eight), Color.White);
            canCastle = canCastle && !IsAttackedBy(new Position(File.F, Rank.Eight), Color.White);
            canCastle = canCastle && !IsAttackedBy(new Position(File.G, Rank.Eight), Color.White);

            if (canCastle)
            {
                moveBuffer[index++] = new Move(new Position(File.E, Rank.Eight), new Position(File.G, Rank.Eight));
            }
        }

        if (king is ChessPiece.BlackKing && BlackQueenSideCastlingAvailable)
        {
            var canCastle = true;

            canCastle = canCastle && GetChessPiece(new Position(File.D, Rank.Eight)) is ChessPiece.None;
            canCastle = canCastle && GetChessPiece(new Position(File.C, Rank.Eight)) is ChessPiece.None;
            canCastle = canCastle && GetChessPiece(new Position(File.B, Rank.Eight)) is ChessPiece.None;

            canCastle = canCastle && !IsAttackedBy(new Position(File.E, Rank.Eight), Color.White);
            canCastle = canCastle && !IsAttackedBy(new Position(File.D, Rank.Eight), Color.White);
            canCastle = canCastle && !IsAttackedBy(new Position(File.C, Rank.Eight), Color.White);

            if (canCastle)
            {
                moveBuffer[index++] = new Move(new Position(File.E, Rank.Eight), new Position(File.C, Rank.Eight));
            }
        }

        return index;
    }

    #endregion

    private bool IsAttackedBy(Position position, Color attacker)
    {
        if (attacker is Color.White && IsAttackedByWhitePawn(position))
        {
            return true;
        }

        if (attacker is Color.Black && IsAttackedByBlackPawn(position))
        {
            return true;
        }

        if (IsAttackedByKnight(position, attacker))
        {
            return true;
        }

        if (IsAttackedByKing(position, attacker))
        {
            return true;
        }

        if (IsAttackedByBishopOrQueen(position, attacker))
        {
            return true;
        }

        if (IsAttackedByRookOrQueen(position, attacker))
        {
            return true;
        }

        return false;
    }

    #region IsAttackedBy implementation

    private bool IsAttackedByWhitePawn(Position position)
    {
        if (HasChessPiece(position, -1, -1, ChessPiece.WhitePawn))
        {
            return true;
        }

        if (HasChessPiece(position, 1, -1, ChessPiece.WhitePawn))
        {
            return true;
        }

        return false;
    }

    private bool IsAttackedByBlackPawn(Position position)
    {
        if (HasChessPiece(position, -1, 1, ChessPiece.BlackPawn))
        {
            return true;
        }

        if (HasChessPiece(position, 1, 1, ChessPiece.BlackPawn))
        {
            return true;
        }

        return false;
    }

    private bool IsAttackedByKnight(Position position, Color attacker)
    {
        ReadOnlySpan<(int right, int up)> knightOffsets =
        [
            (-1, 2), (1, 2), (-1, -2), (1, -2), (-2, 1), (-2, -1), (2, 1), (2, -1)
        ];
        var knight = attacker is Color.White ? ChessPiece.WhiteKnight : ChessPiece.BlackKnight;
        foreach (var (right, up) in knightOffsets)
        {
            if (HasChessPiece(position, up, right, knight))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsAttackedByKing(Position position, Color attacker)
    {
        ReadOnlySpan<(int right, int up)> kingOffsets =
        [
            (-1, 1), (-1, 0), (-1, -1), (0, 1), (0, -1), (1, 1), (1, 0), (1, -1)
        ];
        var king = attacker is Color.White ? ChessPiece.WhiteKing : ChessPiece.BlackKing;
        foreach (var (right, up) in kingOffsets)
        {
            if (HasChessPiece(position, up, right, king))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasChessPiece(Position position, int fileOffset, int rankOffset, ChessPiece chessPiece)
    {
        var fileIndex = position.File.Index() + fileOffset;
        var rankIndex = position.Rank.Index() + rankOffset;
        if (fileIndex < 0 || fileIndex > 7 || rankIndex < 0 || rankIndex > 7)
        {
            return false;
        }

        var targetPosition = position.MoveBy(fileOffset, rankOffset);
        return GetChessPiece(targetPosition) == chessPiece;
    }

    private bool IsAttackedByBishopOrQueen(Position position, Color attacker)
    {
        var topRightMaximumOffset = Math.Min(File.H.Index() - position.File.Index(), Rank.Eight.Index() - position.Rank.Index());
        var topLeftMaximumOffset = Math.Min(position.File.Index() - File.A.Index(), Rank.Eight.Index() - position.Rank.Index());
        var bottomRightMaximumOffset = Math.Min(File.H.Index() - position.File.Index(), position.Rank.Index() - Rank.One.Index());
        var bottomLeftMaximumOffset = Math.Min(position.File.Index() - File.A.Index(), position.Rank.Index() - Rank.One.Index());

        for (var offset = 1; offset <= topRightMaximumOffset; offset++)
        {
            var targetPosition = position.MoveBy(offset, offset);

            if (IsBishopOrQueen(targetPosition, attacker))
            {
                return true;
            }

            if (ShouldBreak(targetPosition))
            {
                break;
            }
        }

        for (var offset = 1; offset <= topLeftMaximumOffset; offset++)
        {
            var targetPosition = position.MoveBy(-offset, offset);

            if (IsBishopOrQueen(targetPosition, attacker))
            {
                return true;
            }

            if (ShouldBreak(targetPosition))
            {
                break;
            }
        }

        for (var offset = 1; offset <= bottomRightMaximumOffset; offset++)
        {
            var targetPosition = position.MoveBy(offset, -offset);

            if (IsBishopOrQueen(targetPosition, attacker))
            {
                return true;
            }

            if (ShouldBreak(targetPosition))
            {
                break;
            }
        }

        for (var offset = 1; offset <= bottomLeftMaximumOffset; offset++)
        {
            var targetPosition = position.MoveBy(-offset, -offset);

            if (IsBishopOrQueen(targetPosition, attacker))
            {
                return true;
            }

            if (ShouldBreak(targetPosition))
            {
                break;
            }
        }

        return false;

        bool IsBishopOrQueen(in Position targetPosition, Color attackerColor)
        {
            var chessPiece = GetChessPiece(targetPosition);

            if (attackerColor is Color.White)
            {
                if (chessPiece is ChessPiece.WhiteBishop || chessPiece is ChessPiece.WhiteQueen)
                {
                    return true;
                }
            }
            else
            {
                if (chessPiece is ChessPiece.BlackBishop || chessPiece is ChessPiece.BlackQueen)
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool ShouldBreak(in Position targetPosition)
        {
            var chessPiece = GetChessPiece(targetPosition);
            return chessPiece is not ChessPiece.None;
        }
    }

    private bool IsAttackedByRookOrQueen(Position position, Color attacker)
    {
        for (var fileIndex = position.File.Index() + 1; fileIndex <= File.H.Index(); fileIndex++)
        {
            var targetPosition = new Position(fileIndex.ToFile(), position.Rank);

            if (IsRookOrQueen(targetPosition, attacker))
            {
                return true;
            }

            if (ShouldBreak(targetPosition))
            {
                break;
            }
        }

        for (var fileIndex = position.File.Index() - 1; fileIndex >= File.A.Index(); fileIndex--)
        {
            var targetPosition = new Position(fileIndex.ToFile(), position.Rank);

            if (IsRookOrQueen(targetPosition, attacker))
            {
                return true;
            }

            if (ShouldBreak(targetPosition))
            {
                break;
            }
        }

        for (var rankIndex = position.Rank.Index() + 1; rankIndex <= Rank.Eight.Index(); rankIndex++)
        {
            var targetPosition = new Position(position.File, rankIndex.ToRank());

            if (IsRookOrQueen(targetPosition, attacker))
            {
                return true;
            }

            if (ShouldBreak(targetPosition))
            {
                break;
            }
        }

        for (var rankIndex = position.Rank.Index() - 1; rankIndex >= Rank.One.Index(); rankIndex--)
        {
            var targetPosition = new Position(position.File, rankIndex.ToRank());

            if (IsRookOrQueen(targetPosition, attacker))
            {
                return true;
            }

            if (ShouldBreak(targetPosition))
            {
                break;
            }
        }

        return false;

        bool IsRookOrQueen(in Position targetPosition, Color attackerColor)
        {
            var chessPiece = GetChessPiece(targetPosition);

            if (attackerColor is Color.White)
            {
                if (chessPiece is ChessPiece.WhiteRook || chessPiece is ChessPiece.WhiteQueen)
                {
                    return true;
                }
            }
            else
            {
                if (chessPiece is ChessPiece.BlackRook || chessPiece is ChessPiece.BlackQueen)
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool ShouldBreak(in Position targetPosition)
        {
            var chessPiece = GetChessPiece(targetPosition);
            return chessPiece is not ChessPiece.None;
        }
    }

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetIndex(in Position position)
    {
        return position.File.Index() * 8 + position.Rank.Index();
    }

    private struct RevertMoveData
    {
        public Position Position1 { get; set; }
        public ChessPiece ChessPiece1 { get; set; }
        public Position Position2 { get; set; }
        public ChessPiece ChessPiece2 { get; set; }

        public Position? Position3 { get; set; }
        public ChessPiece ChessPiece3 { get; set; }
        public Position? Position4 { get; set; }
        public ChessPiece ChessPiece4 { get; set; }

        public Color ActiveColor { get; set; }
        public bool WhiteKingSideCastlingAvailable { get; set; }
        public bool WhiteQueenSideCastlingAvailable { get; set; }
        public bool BlackKingSideCastlingAvailable { get; set; }
        public bool BlackQueenSideCastlingAvailable { get; set; }
        public Position? EnPassantTargetPosition { get; set; }
        public int HalfMoveClock { get; set; }
        public int FullMoveNumber { get; set; }
    }
}