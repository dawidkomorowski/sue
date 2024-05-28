using Sue.Engine.Model;

namespace Sue.Engine.OldModel.Fen.Internal
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