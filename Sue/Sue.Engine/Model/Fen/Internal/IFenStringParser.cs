using Sue.Engine.Model.Chessboard.Internal;

namespace Sue.Engine.Model.Fen.Internal
{
    public interface IFenStringParser
    {
        void Parse(string fenString, ISettableChessboard settableChessboard);
    }
}
