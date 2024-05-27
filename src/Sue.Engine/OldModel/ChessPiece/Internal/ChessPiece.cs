using System;
using System.Collections.Generic;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    public abstract class ChessPiece : IChessPiece
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
        protected ISettableChessboard SettableChessboard => _chessboardField.SettableChessboard;

        public virtual void MakeMove(IMove move)
        {
            if (Color != Chessboard.CurrentPlayer)
            {
                throw new InvalidOperationException(
                    $"Opposite player turn. Player of {Color} color is not allowed to move now.");
            }

            if (move.ChessPiece != this)
            {
                throw new InvalidOperationException(
                    "Invalid move. Given move is not one of available moves for this chess piece.");
            }

            if (!move.To.Empty && !this.IsOpponent(move.To.ChessPiece))
            {
                throw new InvalidOperationException("Invalid move. Chess piece of the same color is on target field.");
            }

            var capture = !move.To.Empty && this.IsOpponent(move.To.ChessPiece);

            // Change position of chess piece
            _chessboardField.ChessPiece = null;
            _chessboardField = (ChessboardField)move.To;
            _chessboardField.ChessPiece = this;

            // Change current player color
            SettableChessboard.CurrentPlayer = Chessboard.CurrentPlayer.Opposite();

            // Increment halfmove clock or reset if capture
            if (capture) SettableChessboard.HalfmoveClock = 0;
            else SettableChessboard.HalfmoveClock = Chessboard.HalfmoveClock + 1;

            // Increment fullmove number after black's move
            if (Color == Color.Black) SettableChessboard.FullmoveNumber = Chessboard.FullmoveNumber + 1;
        }
    }
}