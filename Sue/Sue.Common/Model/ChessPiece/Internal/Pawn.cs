using System.Collections.Generic;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;
using Sue.Common.Model.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal class Pawn : Internal.ChessPiece
    {
        public Pawn(Color color, ChessboardField chessboardField) : base(color, chessboardField)
        {
        }

        public override IEnumerable<IMove> Moves
        {
            get
            {
                IList<IMove> moves = new List<IMove>();

                switch (Color)
                {
                    case Color.White:
                        if (ChessboardField.Rank == Rank.Two &&
                            Chessboard.GetChessboardField(ChessboardField.File, Rank.Four).Empty)
                        {
                            moves.Add(new Move(ChessboardField, Chessboard.GetChessboardField(ChessboardField.File, Rank.Four)));
                        }
                        break;
                    case Color.Black:
                        break;
                }

                return moves;
            }
        }
    }
}