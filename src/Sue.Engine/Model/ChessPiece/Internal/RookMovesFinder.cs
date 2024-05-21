using System.Collections.Generic;
using Sue.Engine.Model.Chessboard;

namespace Sue.Engine.Model.ChessPiece.Internal
{
    public class RookMovesFinder : IRookMovesFinder
    {
        public IEnumerable<IMove> FindMoves(IChessPiece chessPiece)
        {
            var chessboardField = chessPiece.ChessboardField;
            var chessboard = chessPiece.Chessboard;

            var moves = new List<IMove>();
            var file = chessboardField.File;
            var rank = chessboardField.Rank;

            for (var fileIndex = file.Index(); fileIndex <= File.H.Index(); fileIndex++)
            {
                if (chessboard.GetChessboardField(fileIndex.ToFile(), rank) == chessboardField) continue;
                chessPiece.TryAddMove(fileIndex.ToFile(), rank, moves);
                if (!chessboard.GetChessboardField(fileIndex.ToFile(), rank).Empty) break;
            }

            for (var fileIndex = file.Index(); fileIndex >= File.A.Index(); fileIndex--)
            {
                if (chessboard.GetChessboardField(fileIndex.ToFile(), rank) == chessboardField) continue;
                chessPiece.TryAddMove(fileIndex.ToFile(), rank, moves);
                if (!chessboard.GetChessboardField(fileIndex.ToFile(), rank).Empty) break;
            }

            for (var rankIndex = rank.Index(); rankIndex <= Rank.Eight.Index(); rankIndex++)
            {
                if (chessboard.GetChessboardField(file, rankIndex.ToRank()) == chessboardField) continue;
                chessPiece.TryAddMove(file, rankIndex.ToRank(), moves);
                if (!chessboard.GetChessboardField(file, rankIndex.ToRank()).Empty) break;
            }

            for (var rankIndex = rank.Index(); rankIndex >= Rank.One.Index(); rankIndex--)
            {
                if (chessboard.GetChessboardField(file, rankIndex.ToRank()) == chessboardField) continue;
                chessPiece.TryAddMove(file, rankIndex.ToRank(), moves);
                if (!chessboard.GetChessboardField(file, rankIndex.ToRank()).Empty) break;
            }

            return moves;
        }
    }
}