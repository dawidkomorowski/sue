namespace Sue.Engine.Model.Chessboard.Internal
{
    public interface ISettableChessboard : IChessboard
    {
        new Color CurrentPlayer { set; }
        new bool WhiteKingsideCastlingAvailable { set; }
        new bool WhiteQueensideCastlingAvailable { set; }
        new bool BlackKingsideCastlingAvailable { set; }
        new bool BlackQueensideCastlingAvailable { set; }
        new IChessboardField EnPassantTargetField { set; }
        new int HalfmoveClock { set; }
        new int FullmoveNumber { set; }
        void SetChessPiece(ChessPieceKind chessPieceKind, Color color, File file, Rank rank);
    }
}