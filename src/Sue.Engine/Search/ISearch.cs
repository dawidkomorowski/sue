using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal interface ISearch
{
    public Move? FindBestMove(Chessboard chessboard);
}