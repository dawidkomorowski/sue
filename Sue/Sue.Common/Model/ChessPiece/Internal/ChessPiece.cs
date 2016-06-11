using System;
using System.Collections.Generic;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;
using Sue.Common.Model.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal abstract class ChessPiece : IChessPiece
    {
        private ChessboardField _chessboardField;

        protected ChessPiece(Color color, ChessboardField chessboardField)
        {
            Color = color;
            _chessboardField = chessboardField;
        }

        public Color Color { get; }
        public IChessboardField ChessboardField => _chessboardField;
        public abstract IEnumerable<IMove> Moves { get; }
        public IChessboard Chessboard => ChessboardField.Chessboard;

        public void MakeMove(IMove move)
        {
            if (move.ChessPiece != this)
            {
                throw new InvalidOperationException(
                    "Invalid move. Given move is not one of available moves for this chess piece.");
            }

            if (move.To.Empty || IsOpponent(move.To.ChessPiece))
            {
                _chessboardField.ChessPiece = null;
                _chessboardField = (ChessboardField) move.To;
                _chessboardField.ChessPiece = this;
            }
            else
            {
                throw new InvalidOperationException("Invalid move. Chess piece of the same color is on target field.");
            }
        }

        protected bool IsOpponent(IChessPiece chessPiece)
        {
            return Color != chessPiece.Color;
        }

        protected IMove NewMove(IChessboardField to)
        {
            return new Move(ChessboardField, to);
        }


        protected bool IsEmptyOrOpponent(IChessboardField chessboardField)
        {
            if (chessboardField.Empty)
            {
                return true;
            }
            else
            {
                if (IsOpponent(chessboardField.ChessPiece))
                {
                    return true;
                }
            }

            return false;
        }

        protected void TryAddMove(File file, Rank rank, IList<IMove> moves)
        {
            var chessboardField = Chessboard.GetChessboardField(file, rank);
            if (IsEmptyOrOpponent(chessboardField))
            {
                moves.Add(NewMove(chessboardField));
            }
        }
    }
}