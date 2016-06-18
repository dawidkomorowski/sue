using System;
using System.Collections.Generic;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal class Bishop : ChessPiece
    {
        public Bishop(Color color, ChessboardField chessboardField) : base(color, chessboardField)
        {
        }

        public override IEnumerable<IMove> Moves
        {
            get
            {
                var moves = new List<IMove>();
                var file = ChessboardField.File;
                var rank = ChessboardField.Rank;

                for (var offset = 1; offset <= GetTopRightMaximumOffset(); offset++)
                {
                    var targetFile = file.Add(offset);
                    var targetRank = rank.Add(offset);
                    TryAddMove(targetFile, targetRank, moves);
                    if (!Chessboard.GetChessboardField(targetFile, targetRank).Empty) break;
                }

                for (var offset = 1; offset <= GetTopLeftMaximumOffset(); offset++)
                {
                    var targetFile = file.Add(-offset);
                    var targetRank = rank.Add(offset);
                    TryAddMove(targetFile, targetRank, moves);
                    if (!Chessboard.GetChessboardField(targetFile, targetRank).Empty) break;
                }

                for (var offset = 1; offset <= GetBottomRightMaximumOffset(); offset++)
                {
                    var targetFile = file.Add(offset);
                    var targetRank = rank.Add(-offset);
                    TryAddMove(targetFile, targetRank, moves);
                    if (!Chessboard.GetChessboardField(targetFile, targetRank).Empty) break;
                }

                for (var offset = 1; offset <= GetBottomLeftMaximumOffset(); offset++)
                {
                    var targetFile = file.Add(-offset);
                    var targetRank = rank.Add(-offset);
                    TryAddMove(targetFile, targetRank, moves);
                    if (!Chessboard.GetChessboardField(targetFile, targetRank).Empty) break;
                }

                return moves;
            }
        }

        private int GetTopRightMaximumOffset()
        {
            var file = ChessboardField.File;
            var rank = ChessboardField.Rank;

            var fileOffset = File.H.Index() - file.Index();
            var rankOffset = Rank.Eight.Index() - rank.Index();

            return Math.Min(fileOffset, rankOffset);
        }

        private int GetTopLeftMaximumOffset()
        {
            var file = ChessboardField.File;
            var rank = ChessboardField.Rank;

            var fileOffset = file.Index() - File.A.Index();
            var rankOffset = Rank.Eight.Index() - rank.Index();

            return Math.Min(fileOffset, rankOffset);
        }

        private int GetBottomRightMaximumOffset()
        {
            var file = ChessboardField.File;
            var rank = ChessboardField.Rank;

            var fileOffset = File.H.Index() - file.Index();
            var rankOffset = rank.Index() - Rank.One.Index();

            return Math.Min(fileOffset, rankOffset);
        }

        private int GetBottomLeftMaximumOffset()
        {
            var file = ChessboardField.File;
            var rank = ChessboardField.Rank;

            var fileOffset = file.Index() - File.A.Index();
            var rankOffset = rank.Index() - Rank.One.Index();

            return Math.Min(fileOffset, rankOffset);
        }
    }
}