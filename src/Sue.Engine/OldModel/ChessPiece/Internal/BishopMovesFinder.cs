using System;
using System.Collections.Generic;
using Sue.Engine.OldModel.Chessboard;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    public class BishopMovesFinder : IBishopMovesFinder
    {
        public IEnumerable<IMove> FindMoves(IChessPiece chessPiece)
        {
            var chessboardField = chessPiece.ChessboardField;
            var chessboard = chessPiece.Chessboard;

            var moves = new List<IMove>();
            var file = chessboardField.File;
            var rank = chessboardField.Rank;

            for (var offset = 1; offset <= GetTopRightMaximumOffset(chessboardField); offset++)
            {
                var targetFile = file.Add(offset);
                var targetRank = rank.Add(offset);
                chessPiece.TryAddMove(targetFile, targetRank, moves);
                if (!chessboard.GetChessboardField(targetFile, targetRank).Empty) break;
            }

            for (var offset = 1; offset <= GetTopLeftMaximumOffset(chessboardField); offset++)
            {
                var targetFile = file.Add(-offset);
                var targetRank = rank.Add(offset);
                chessPiece.TryAddMove(targetFile, targetRank, moves);
                if (!chessboard.GetChessboardField(targetFile, targetRank).Empty) break;
            }

            for (var offset = 1; offset <= GetBottomRightMaximumOffset(chessboardField); offset++)
            {
                var targetFile = file.Add(offset);
                var targetRank = rank.Add(-offset);
                chessPiece.TryAddMove(targetFile, targetRank, moves);
                if (!chessboard.GetChessboardField(targetFile, targetRank).Empty) break;
            }

            for (var offset = 1; offset <= GetBottomLeftMaximumOffset(chessboardField); offset++)
            {
                var targetFile = file.Add(-offset);
                var targetRank = rank.Add(-offset);
                chessPiece.TryAddMove(targetFile, targetRank, moves);
                if (!chessboard.GetChessboardField(targetFile, targetRank).Empty) break;
            }

            return moves;
        }

        private static int GetTopRightMaximumOffset(IChessboardField chessboardField)
        {
            var file = chessboardField.File;
            var rank = chessboardField.Rank;

            var fileOffset = File.H.Index() - file.Index();
            var rankOffset = Rank.Eight.Index() - rank.Index();

            return Math.Min(fileOffset, rankOffset);
        }

        private static int GetTopLeftMaximumOffset(IChessboardField chessboardField)
        {
            var file = chessboardField.File;
            var rank = chessboardField.Rank;

            var fileOffset = file.Index() - File.A.Index();
            var rankOffset = Rank.Eight.Index() - rank.Index();

            return Math.Min(fileOffset, rankOffset);
        }

        private static int GetBottomRightMaximumOffset(IChessboardField chessboardField)
        {
            var file = chessboardField.File;
            var rank = chessboardField.Rank;

            var fileOffset = File.H.Index() - file.Index();
            var rankOffset = rank.Index() - Rank.One.Index();

            return Math.Min(fileOffset, rankOffset);
        }

        private static int GetBottomLeftMaximumOffset(IChessboardField chessboardField)
        {
            var file = chessboardField.File;
            var rank = chessboardField.Rank;

            var fileOffset = file.Index() - File.A.Index();
            var rankOffset = rank.Index() - Rank.One.Index();

            return Math.Min(fileOffset, rankOffset);
        }
    }
}