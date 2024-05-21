using System.Collections.Generic;
using Sue.Engine.Model.Chessboard;
using Sue.Engine.Model.Internal;

namespace Sue.Engine.Model.ChessPiece.Internal
{
    public static class ChessPieceExtensions
    {
        public static bool IsOpponent(this IChessPiece thisChessPiece, IChessPiece chessPiece)
        {
            return thisChessPiece.Color != chessPiece.Color;
        }

        private static bool IsEmptyOrOpponent(this IChessPiece chessPiece, IChessboardField chessboardField)
        {
            return chessboardField.Empty || chessPiece.IsOpponent(chessboardField.ChessPiece);
        }

        private static IMove NewMove(this IChessPiece chessPiece, IChessboardField to)
        {
            return new Move(chessPiece.ChessboardField, to);
        }

        public static void TryAddMove(this IChessPiece chessPiece, File file, Rank rank, IList<IMove> moves)
        {
            var chessboardField = chessPiece.Chessboard.GetChessboardField(file, rank);
            if (chessPiece.IsEmptyOrOpponent(chessboardField))
            {
                moves.Add(chessPiece.NewMove(chessboardField));
            }
        }
    }
}