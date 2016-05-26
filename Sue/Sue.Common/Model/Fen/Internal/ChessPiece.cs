using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.Fen.Internal
{
    internal struct ChessPiece
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