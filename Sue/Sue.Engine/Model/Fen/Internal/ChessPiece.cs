using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.Fen.Internal
{
    public struct ChessPiece
    {
        public ChessPiece(Color color, ChessPieceKind chessPieceKind)
        {
            Color = color;
            ChessPieceKind = chessPieceKind;
        }

        public Color Color { get; }
        public ChessPieceKind ChessPieceKind { get; }
    }
}