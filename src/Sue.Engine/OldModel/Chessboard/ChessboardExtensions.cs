using System.Collections.Generic;
using System.Linq;
using Sue.Engine.Model;
using Sue.Engine.OldModel.ChessPiece;

namespace Sue.Engine.OldModel.Chessboard
{
    internal static class ChessboardExtensions
    {
        public static bool EqualsTo(this IChessboard thisChessboard, IChessboard otherChessboard)
        {
            #region CurrentPlayer

            if (thisChessboard.CurrentPlayer != otherChessboard.CurrentPlayer)
            {
                return false;
            }

            #endregion

            #region Castling

            if (thisChessboard.WhiteKingsideCastlingAvailable != otherChessboard.WhiteKingsideCastlingAvailable)
            {
                return false;
            }

            if (thisChessboard.WhiteQueensideCastlingAvailable != otherChessboard.WhiteQueensideCastlingAvailable)
            {
                return false;
            }

            if (thisChessboard.BlackKingsideCastlingAvailable != otherChessboard.BlackKingsideCastlingAvailable)
            {
                return false;
            }

            if (thisChessboard.BlackQueensideCastlingAvailable != otherChessboard.BlackQueensideCastlingAvailable)
            {
                return false;
            }

            #endregion

            #region EnPassant

            if (thisChessboard.EnPassantTargetField == null && otherChessboard.EnPassantTargetField != null)
            {
                return false;
            }

            if (thisChessboard.EnPassantTargetField != null && otherChessboard.EnPassantTargetField == null)
            {
                return false;
            }

            if (thisChessboard.EnPassantTargetField != null && otherChessboard.EnPassantTargetField != null)
            {
                if (thisChessboard.EnPassantTargetField.File != otherChessboard.EnPassantTargetField.File)
                {
                    return false;
                }

                if (thisChessboard.EnPassantTargetField.Rank != otherChessboard.EnPassantTargetField.Rank)
                {
                    return false;
                }
            }

            #endregion

            #region HalfMoveClock

            if (thisChessboard.HalfmoveClock != otherChessboard.HalfmoveClock)
            {
                return false;
            }

            #endregion

            #region FullmoveNumber

            if (thisChessboard.FullmoveNumber != otherChessboard.FullmoveNumber)
            {
                return false;
            }

            #endregion

            #region ChessPieces

            foreach (var file in FileExtensions.Enumerable())
            {
                foreach (var rank in RankExtensions.Enumerable())
                {
                    var thisChessboardField = thisChessboard.GetChessboardField(file, rank);
                    var otherChessboardField = otherChessboard.GetChessboardField(file, rank);

                    if (thisChessboardField.Empty != otherChessboardField.Empty)
                    {
                        return false;
                    }

                    if (thisChessboardField.Empty || otherChessboardField.Empty) continue;
                    if (thisChessboardField.ChessPiece.GetType() != otherChessboardField.ChessPiece.GetType())
                    {
                        return false;
                    }
                }
            }

            #endregion

            return true;
        }

        public static IEnumerable<IChessPiece> GetChessPieces(this IChessboard chessboard)
        {
            return chessboard.GetChessPieces(Color.White).Concat(chessboard.GetChessPieces(Color.Black));
        }
    }
}