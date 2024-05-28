using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;

namespace Sue.Engine.OldModel.Fen.Internal
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