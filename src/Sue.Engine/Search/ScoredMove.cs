using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal readonly struct ScoredMove
{
    public ScoredMove(Move move, Score score)
    {
        Move = move;
        Score = score;
    }

    public Move Move { get; }
    public Score Score { get; }
}