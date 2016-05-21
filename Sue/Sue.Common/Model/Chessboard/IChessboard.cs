using System.Collections.Generic;
using Sue.Common.Model.ChessPiece;

namespace Sue.Common.Model.Chessboard
{
    public interface IChessboard
    {
        Color CurrentPlayer { get; }
        bool WhiteKingsideCastlingAvailable { get; }
        bool WhiteQueenssideCastlingAvailable { get; }
        bool BlackKingsideCastlingAvailable { get; }
        bool BlackQueensideCastlingAvailable { get; }
        IChessboardField EnPassantTargetField { get; }
        ushort HalfmoveClock { get; }
        ushort FullmoveNumber { get; }
        IChessboardField GetChessboardField(File file, Rank rank);
        IChessPiece GetChessPiece(File file, Rank rank);
        IEnumerable<IChessPiece> GetChessPieces(Color color);
    }
}