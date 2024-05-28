using System.Collections.Generic;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    internal static class ChessPieceExtensions
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
            return new OldModel.Internal.Move(chessPiece.ChessboardField, to);
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