using System;
using System.Collections.Generic;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    public class Pawn : ChessPiece
    {
        public Pawn(Color color, ChessboardField chessboardField) : base(color, chessboardField)
        {
        }

        public override void MakeMove(IMove move)
        {
            base.MakeMove(move);

            // Reset halfmove clock
            SettableChessboard.HalfmoveClock = 0;
        }

        public override IEnumerable<IMove> Moves
        {
            get
            {
                switch (Color)
                {
                    case Color.White:
                        return GetWhitePawnMoves();
                    case Color.Black:
                        return GetBlackPawnMoves();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private IEnumerable<IMove> GetWhitePawnMoves()
        {
            IList<IMove> moves = new List<IMove>();

            var file = ChessboardField.File;
            var rank = ChessboardField.Rank;

            if (rank == Rank.Two && Chessboard.GetChessboardField(file, Rank.Three).Empty &&
                Chessboard.GetChessboardField(file, Rank.Four).Empty) this.TryAddMove(file, Rank.Four, moves);

            if (rank != Rank.Seven)
            {
                var front = Chessboard.GetChessboardField(file, rank.Add(1));
                if (front.Empty) this.TryAddMove(front.File, front.Rank, moves);

                if (file != File.A)
                {
                    var frontLeft = Chessboard.GetChessboardField(file.Add(-1), rank.Add(1));
                    if (!frontLeft.Empty && this.IsOpponent(frontLeft.ChessPiece))
                        this.TryAddMove(frontLeft.File, frontLeft.Rank, moves);
                }

                if (file != File.H)
                {
                    var frontRight = Chessboard.GetChessboardField(file.Add(1), rank.Add(1));
                    if (!frontRight.Empty && this.IsOpponent(frontRight.ChessPiece))
                        this.TryAddMove(frontRight.File, frontRight.Rank, moves);
                }
            }

            return moves;
        }

        private IEnumerable<IMove> GetBlackPawnMoves()
        {
            IList<IMove> moves = new List<IMove>();

            var file = ChessboardField.File;
            var rank = ChessboardField.Rank;

            if (rank == Rank.Seven && Chessboard.GetChessboardField(file, Rank.Six).Empty &&
                Chessboard.GetChessboardField(file, Rank.Five).Empty) this.TryAddMove(file, Rank.Five, moves);

            if (rank != Rank.Two)
            {
                var front = Chessboard.GetChessboardField(file, rank.Add(-1));
                if (front.Empty) this.TryAddMove(front.File, front.Rank, moves);

                if (file != File.A)
                {
                    var frontLeft = Chessboard.GetChessboardField(file.Add(-1), rank.Add(-1));
                    if (!frontLeft.Empty && this.IsOpponent(frontLeft.ChessPiece))
                        this.TryAddMove(frontLeft.File, frontLeft.Rank, moves);
                }

                if (file != File.H)
                {
                    var frontRight = Chessboard.GetChessboardField(file.Add(1), rank.Add(-1));
                    if (!frontRight.Empty && this.IsOpponent(frontRight.ChessPiece))
                        this.TryAddMove(frontRight.File, frontRight.Rank, moves);
                }
            }

            return moves;
        }
    }
}