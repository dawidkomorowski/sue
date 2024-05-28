using System.Collections.Generic;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    public class Rook : ChessPiece
    {
        private readonly IRookMovesFinder _rookMovesFinder;

        public Rook(Color color, ChessboardField chessboardField, IRookMovesFinder rookMovesFinder)
            : base(color, chessboardField)
        {
            _rookMovesFinder = rookMovesFinder;
        }

        public override void MakeMove(IMove move)
        {
            base.MakeMove(move);

            if (Chessboard.WhiteKingsideCastlingAvailable && Color == Color.White && ChessboardField.File == File.H)
            {
                SettableChessboard.WhiteKingsideCastlingAvailable = false;
            }

            if (Chessboard.WhiteQueensideCastlingAvailable && Color == Color.White && ChessboardField.File == File.A)
            {
                SettableChessboard.WhiteQueensideCastlingAvailable = false;
            }

            if (Chessboard.BlackKingsideCastlingAvailable && Color == Color.Black && ChessboardField.File == File.H)
            {
                SettableChessboard.BlackKingsideCastlingAvailable = false;
            }

            if (Chessboard.BlackQueensideCastlingAvailable && Color == Color.Black && ChessboardField.File == File.A)
            {
                SettableChessboard.BlackQueensideCastlingAvailable = false;
            }
        }

        public override IEnumerable<IMove> Moves => _rookMovesFinder.FindMoves(this);
    }
}