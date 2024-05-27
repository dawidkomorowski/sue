using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel.Fen.Internal
{
    public interface IFenStringParser
    {
        void Parse(string fenString, ISettableChessboard settableChessboard);
    }
}