using Sue.Engine.Model.Chessboard;

namespace Sue.Engine.Model.Fen.Internal
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