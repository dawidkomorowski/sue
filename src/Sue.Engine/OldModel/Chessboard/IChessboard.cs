using System.Collections.Generic;
using Sue.Engine.Model;
using Sue.Engine.OldModel.ChessPiece;

namespace Sue.Engine.OldModel.Chessboard
{
    public interface IChessboard
    {
        Color CurrentPlayer { get; }
        bool WhiteKingsideCastlingAvailable { get; }
        bool WhiteQueensideCastlingAvailable { get; }
        bool BlackKingsideCastlingAvailable { get; }
        bool BlackQueensideCastlingAvailable { get; }
        IChessboardField EnPassantTargetField { get; }
        int HalfmoveClock { get; }
        int FullmoveNumber { get; }
        IChessboardField GetChessboardField(File file, Rank rank);
        IChessPiece GetChessPiece(File file, Rank rank);
        IEnumerable<IChessPiece> GetChessPieces(Color color);
    }
}