using Sue.Engine.OldModel.Chessboard;

namespace Sue.Engine.OldModel.Fen.Internal
{
    public class PlacedChessPiece
    {
        public PlacedChessPiece(ChessPiece chessPiece, File file, Rank rank)
        {
            ChessPiece = chessPiece;
            File = file;
            Rank = rank;
        }

        public ChessPiece ChessPiece { get; }
        public File File { get; }
        public Rank Rank { get; }
    }
}