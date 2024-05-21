using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.Fen.Internal
{
    public struct ChessField
    {
        public ChessField(File file, Rank rank)
        {
            File = file;
            Rank = rank;
        }

        public File File { get; }
        public Rank Rank { get; }
    }
}