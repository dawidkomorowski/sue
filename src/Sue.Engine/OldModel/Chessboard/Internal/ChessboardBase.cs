using System.Collections.Generic;
using Sue.Engine.Model;
using Sue.Engine.OldModel.ChessPiece;

namespace Sue.Engine.OldModel.Chessboard.Internal
{
    internal abstract class ChessboardBase : ISettableChessboard
    {
        public Color CurrentPlayer { get; set; }
        public bool WhiteKingsideCastlingAvailable { get; set; }
        public bool WhiteQueensideCastlingAvailable { get; set; }
        public bool BlackKingsideCastlingAvailable { get; set; }
        public bool BlackQueensideCastlingAvailable { get; set; }
        public IChessboardField EnPassantTargetField { get; set; }
        public int HalfmoveClock { get; set; }
        public int FullmoveNumber { get; set; }

        public abstract IChessboardField GetChessboardField(File file, Rank rank);
        public abstract IChessPiece GetChessPiece(File file, Rank rank);
        public abstract IEnumerable<IChessPiece> GetChessPieces(Color color);
        public abstract void SetChessPiece(ChessPieceKind chessPieceKind, Color color, File file, Rank rank);
    }
}