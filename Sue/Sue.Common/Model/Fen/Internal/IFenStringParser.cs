using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.Fen.Internal
{
    internal interface IFenStringParser
    {
        void Parse(string fenString, ISettableChessboard settableChessboard);
    }
}
