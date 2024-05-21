using System;
using Sue.Engine.Model.Chessboard.Internal;

namespace Sue.Engine.Model.Fen.Internal
{
    public class ChessPieceParser : IChessPieceParser
    {
        public ChessPiece Parse(char fenChessPieceCode)
        {
            switch (fenChessPieceCode)
            {
                case 'P':
                    return new ChessPiece(Color.White, ChessPieceKind.Pawn);
                case 'B':
                    return new ChessPiece(Color.White, ChessPieceKind.Bishop);
                case 'N':
                    return new ChessPiece(Color.White, ChessPieceKind.Knight);
                case 'R':
                    return new ChessPiece(Color.White, ChessPieceKind.Rook);
                case 'Q':
                    return new ChessPiece(Color.White, ChessPieceKind.Queen);
                case 'K':
                    return new ChessPiece(Color.White, ChessPieceKind.King);
                case 'p':
                    return new ChessPiece(Color.Black, ChessPieceKind.Pawn);
                case 'b':
                    return new ChessPiece(Color.Black, ChessPieceKind.Bishop);
                case 'n':
                    return new ChessPiece(Color.Black, ChessPieceKind.Knight);
                case 'r':
                    return new ChessPiece(Color.Black, ChessPieceKind.Rook);
                case 'q':
                    return new ChessPiece(Color.Black, ChessPieceKind.Queen);
                case 'k':
                    return new ChessPiece(Color.Black, ChessPieceKind.King);
                default:
                    throw new ArgumentOutOfRangeException(nameof(fenChessPieceCode), fenChessPieceCode, null);
            }
        }
    }
}