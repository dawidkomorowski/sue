using System.Collections.Generic;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal class Rook : Internal.ChessPiece
    {
        public Rook(Color color, ChessboardField chessboardField) : base(color, chessboardField)
        {
        }

        public override IEnumerable<IMove> Moves
        {
            get
            {
                var moves = new List<IMove>();
                var file = ChessboardField.File;
                var rank = ChessboardField.Rank;

                for (var fileIndex = file.Index(); fileIndex <= File.H.Index(); fileIndex++)
                {
                    if(Chessboard.GetChessboardField(fileIndex.ToFile(), rank) == ChessboardField) continue;
                    TryAddMove(fileIndex.ToFile(), rank, moves);
                    if (!Chessboard.GetChessboardField(fileIndex.ToFile(), rank).Empty) break;
                }

                for (var fileIndex = file.Index(); fileIndex >= File.A.Index(); fileIndex--)
                {
                    if(Chessboard.GetChessboardField(fileIndex.ToFile(), rank) == ChessboardField) continue;
                    TryAddMove(fileIndex.ToFile(), rank, moves);
                    if (!Chessboard.GetChessboardField(fileIndex.ToFile(), rank).Empty) break;
                }

                for (var rankIndex = rank.Index(); rankIndex <= Rank.Eight.Index(); rankIndex++)
                {
                    if(Chessboard.GetChessboardField(file, rankIndex.ToRank()) == ChessboardField) continue;
                    TryAddMove(file, rankIndex.ToRank(), moves);
                    if (!Chessboard.GetChessboardField(file, rankIndex.ToRank()).Empty) break;
                }

                for (var rankIndex = rank.Index(); rankIndex >= Rank.One.Index(); rankIndex--)
                {
                    if(Chessboard.GetChessboardField(file, rankIndex.ToRank()) == ChessboardField) continue;
                    TryAddMove(file, rankIndex.ToRank(), moves);
                    if (!Chessboard.GetChessboardField(file, rankIndex.ToRank()).Empty) break;
                }

                return moves;
            }
        }
    }
}