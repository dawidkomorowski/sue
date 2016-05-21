using System;
using System.Collections.Generic;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal abstract class ChessPiece : IChessPiece
    {
        private ChessboardField _chessboardField;

        public Color Color { get; }
        public IChessboardField ChessboardField => _chessboardField;
        public abstract IEnumerable<IMove> Moves { get; }

        protected ChessPiece(Color color, ChessboardField chessboardField)
        {
            Color = color;
            _chessboardField = chessboardField;
        }

        public void MakeMove(IMove move)
        {
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

        public IChessboard Chessboard => ChessboardField.Chessboard;

        private bool IsOpponent(IChessPiece chessPiece)
        {
            return Color != chessPiece.Color;
        }
    }
}