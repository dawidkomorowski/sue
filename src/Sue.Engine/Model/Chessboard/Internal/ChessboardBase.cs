using System.Collections.Generic;
using Sue.Engine.Model.ChessPiece;

namespace Sue.Engine.Model.Chessboard.Internal
{
    public abstract class ChessboardBase : ISettableChessboard
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