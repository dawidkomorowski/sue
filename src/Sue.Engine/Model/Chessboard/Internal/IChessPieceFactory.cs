using Sue.Engine.Model.ChessPiece;

namespace Sue.Engine.Model.Chessboard.Internal
{
    public interface IChessPieceFactory
    {
        IChessPiece Create(ChessPieceKind chessPieceKind, Color color, ChessboardField chessboardField);
    }
}