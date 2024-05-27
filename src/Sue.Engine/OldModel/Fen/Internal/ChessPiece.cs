using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel.Fen.Internal
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