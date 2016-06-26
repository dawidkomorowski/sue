using System;
using Sue.Common.Model.ChessPiece;
using Sue.Common.Model.ChessPiece.Internal;

namespace Sue.Common.Model.Chessboard.Internal
{
    internal class ChessPieceFactory : IChessPieceFactory
    {
        private readonly IRookMovesFinder _rookMovesFinder;
        private readonly IBishopMovesFinder _bishopMovesFinder;

        public ChessPieceFactory(IRookMovesFinder rookMovesFinder, IBishopMovesFinder bishopMovesFinder)
        {
            _rookMovesFinder = rookMovesFinder;
            _bishopMovesFinder = bishopMovesFinder;
        }

        public IChessPiece Create(ChessPieceKind chessPieceKind, Color color,
            ChessboardField chessboardField)
        {
            switch (chessPieceKind)
            {
                case ChessPieceKind.Pawn:
                    return new Pawn(color, chessboardField);
                case ChessPieceKind.Bishop:
                    return new Bishop(color, chessboardField, _bishopMovesFinder);
                case ChessPieceKind.Knight:
                    return new Knight(color, chessboardField);
                case ChessPieceKind.Rook:
                    return new Rook(color, chessboardField, _rookMovesFinder);
                case ChessPieceKind.Queen:
                    return new Queen(color, chessboardField, _rookMovesFinder, _bishopMovesFinder);
                case ChessPieceKind.King:
                    return new King(color, chessboardField);
                default:
                    throw new ArgumentOutOfRangeException(nameof(chessPieceKind), chessPieceKind, null);
            }
        }
    }
}