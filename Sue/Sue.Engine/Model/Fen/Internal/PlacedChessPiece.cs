using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.Fen.Internal
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
