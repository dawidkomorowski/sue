using System;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel
{
    public class ChessboardFactory
    {
        private readonly IChessPieceFactory _chessPieceFactory;

        public ChessboardFactory(IChessPieceFactory chessPieceFactory)
        {
            _chessPieceFactory = chessPieceFactory;
        }

        public IChessboard Create(string fenString)
        {
            var fen = Fen.FromString(fenString);
            var chessboard = new ArrayChessboard(_chessPieceFactory);

            chessboard.CurrentPlayer = fen.ActiveColor;
            chessboard.WhiteKingsideCastlingAvailable = fen.WhiteKingSideCastlingAvailable;
            chessboard.WhiteQueensideCastlingAvailable = fen.WhiteQueenSideCastlingAvailable;
            chessboard.BlackKingsideCastlingAvailable = fen.BlackKingSideCastlingAvailable;
            chessboard.BlackQueensideCastlingAvailable = fen.BlackQueenSideCastlingAvailable;
            chessboard.EnPassantTargetField = fen.EnPassantTargetPosition is null
                ? null
                : chessboard.GetChessboardField(fen.EnPassantTargetPosition.Value.File, fen.EnPassantTargetPosition.Value.Rank);
            chessboard.HalfmoveClock = fen.HalfMoveClock;
            chessboard.FullmoveNumber = fen.FullMoveNumber;

            foreach (var position in Position.All)
            {
                switch (fen.GetChessPiece(position))
                {
                    case Model.ChessPiece.None:
                        break;
                    case Model.ChessPiece.WhiteKing:
                        chessboard.SetChessPiece(ChessPieceKind.King, Color.White, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.WhiteQueen:
                        chessboard.SetChessPiece(ChessPieceKind.Queen, Color.White, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.WhiteRook:
                        chessboard.SetChessPiece(ChessPieceKind.Rook, Color.White, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.WhiteBishop:
                        chessboard.SetChessPiece(ChessPieceKind.Bishop, Color.White, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.WhiteKnight:
                        chessboard.SetChessPiece(ChessPieceKind.Knight, Color.White, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.WhitePawn:
                        chessboard.SetChessPiece(ChessPieceKind.Pawn, Color.White, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.BlackKing:
                        chessboard.SetChessPiece(ChessPieceKind.King, Color.Black, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.BlackQueen:
                        chessboard.SetChessPiece(ChessPieceKind.Queen, Color.Black, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.BlackRook:
                        chessboard.SetChessPiece(ChessPieceKind.Rook, Color.Black, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.BlackBishop:
                        chessboard.SetChessPiece(ChessPieceKind.Bishop, Color.Black, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.BlackKnight:
                        chessboard.SetChessPiece(ChessPieceKind.Knight, Color.Black, position.File, position.Rank);
                        break;
                    case Model.ChessPiece.BlackPawn:
                        chessboard.SetChessPiece(ChessPieceKind.Pawn, Color.Black, position.File, position.Rank);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return chessboard;
        }
    }
}