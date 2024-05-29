using Sue.Engine.Model;
using Sue.Engine.OldModel.ChessPiece;

namespace Sue.Engine.OldModel.Chessboard.Internal
{
    public interface IChessPieceFactory
    {
        IChessPiece Create(ChessPieceKind chessPieceKind, Color color, ChessboardField chessboardField);
    }
}