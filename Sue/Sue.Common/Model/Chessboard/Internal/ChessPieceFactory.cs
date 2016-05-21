using System;
using Sue.Common.Model.ChessPiece;
using Sue.Common.Model.ChessPiece.Internal;

namespace Sue.Common.Model.Chessboard.Internal
{
    internal class ChessPieceFactory : IChessPieceFactory
    {
        public IChessPiece Create(ChessPieceKind chessPieceKind, Color color,
            ChessboardField chessboardField)
        {
            switch (chessPieceKind)
            {
                case ChessPieceKind.Pawn:
                    return new Pawn(color, chessboardField);
                case ChessPieceKind.Bishop:
                    return new Bishop(color, chessboardField);
                case ChessPieceKind.Knight:
                    return new Knight(color, chessboardField);
                case ChessPieceKind.Rook:
                    return new Rook(color, chessboardField);
                case ChessPieceKind.Queen:
                    return new Queen(color, chessboardField);
                case ChessPieceKind.King:
                    return new King(color, chessboardField);
                default:
                    throw new ArgumentOutOfRangeException(nameof(chessPieceKind), chessPieceKind, null);
            }
        }
    }
}