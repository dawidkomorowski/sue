using Sue.Engine.OldModel.Chessboard.Internal;
using Sue.Engine.OldModel.ChessPiece.Internal;
using Sue.Engine.OldModel.Fen.Internal;
using System;

namespace Sue.Engine.Model;

internal sealed class Fen
{
    private readonly ChessPiece[] _chessboard = new ChessPiece[64];

    public const string StartPos = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

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
        IRookMovesFinder rookMovesFinder = new RookMovesFinder();
        IBishopMovesFinder bishopMovesFinder = new BishopMovesFinder();
        IChessPieceFactory chessPieceFactory = new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
        IChessPieceParser chessPieceParser = new ChessPieceParser();
        IRankLineParser rankLineParser = new RankLineParser(chessPieceParser);
        IFenStringExtractor fenStringExtractor = new FenStringExtractor();
        ICastlingAvailabilityParser castlingAvailabilityParser = new CastlingAvailabilityParser();
        IChessFieldParser chessFieldParser = new ChessFieldParser();
        IFenStringParser fenStringParser = new FenStringParser(rankLineParser, fenStringExtractor,
            castlingAvailabilityParser, chessFieldParser);
        var chessboardFactory = new ChessboardFactory(chessPieceFactory, fenStringParser);

        var chessboard = chessboardFactory.Create(fenString);

        var fen = new Fen();

        fen.ActiveColor = chessboard.CurrentPlayer;
        fen.WhiteKingSideCastlingAvailable = chessboard.WhiteKingsideCastlingAvailable;
        fen.WhiteQueenSideCastlingAvailable = chessboard.WhiteQueensideCastlingAvailable;
        fen.BlackKingSideCastlingAvailable = chessboard.BlackKingsideCastlingAvailable;
        fen.BlackQueenSideCastlingAvailable = chessboard.BlackQueensideCastlingAvailable;

        var e = chessboard.EnPassantTargetField;
        fen.EnPassantTargetField = e is null ? null : new Position(e.File, e.Rank);

        fen.HalfMoveClock = chessboard.HalfmoveClock;
        fen.FullMoveNumber = chessboard.FullmoveNumber;

        foreach (var position in Position.All)
        {
            var chessPiece = chessboard.GetChessPiece(position.File, position.Rank);

            if (chessPiece is null)
            {
                fen.SetChessPiece(position, ChessPiece.None);
            }
            else
            {
                if (chessPiece.Color is Color.White)
                {
                    switch (chessPiece)
                    {
                        case Bishop bishop:
                            fen.SetChessPiece(position, ChessPiece.WhiteBishop);
                            break;
                        case King king:
                            fen.SetChessPiece(position, ChessPiece.WhiteKing);
                            break;
                        case Knight knight:
                            fen.SetChessPiece(position, ChessPiece.WhiteKnight);
                            break;
                        case Pawn pawn:
                            fen.SetChessPiece(position, ChessPiece.WhitePawn);
                            break;
                        case Queen queen:
                            fen.SetChessPiece(position, ChessPiece.WhiteQueen);
                            break;
                        case Rook rook:
                            fen.SetChessPiece(position, ChessPiece.WhiteRook);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(chessPiece));
                    }
                }
                else
                {
                    switch (chessPiece)
                    {
                        case Bishop bishop:
                            fen.SetChessPiece(position, ChessPiece.BlackBishop);
                            break;
                        case King king:
                            fen.SetChessPiece(position, ChessPiece.BlackKing);
                            break;
                        case Knight knight:
                            fen.SetChessPiece(position, ChessPiece.BlackKnight);
                            break;
                        case Pawn pawn:
                            fen.SetChessPiece(position, ChessPiece.BlackPawn);
                            break;
                        case Queen queen:
                            fen.SetChessPiece(position, ChessPiece.BlackQueen);
                            break;
                        case Rook rook:
                            fen.SetChessPiece(position, ChessPiece.BlackRook);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(chessPiece));
                    }
                }
            }
        }

        return fen;
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