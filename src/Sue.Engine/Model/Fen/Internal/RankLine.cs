using Sue.Engine.Model.Chessboard;

namespace Sue.Engine.Model.Fen.Internal
{
    public struct RankLine
    {
        public RankLine(string s, Rank rank)
        {
            String = s;
            Rank = rank;
        }

        public string String { get; }
        public Rank Rank { get; }
    }
}