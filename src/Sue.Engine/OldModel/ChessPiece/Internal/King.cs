using System;
using System.Collections.Generic;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    public class King : ChessPiece
    {
        public King(Color color, ChessboardField chessboardField) : base(color, chessboardField)
        {
        }

        public override void MakeMove(IMove move)
        {
            base.MakeMove(move);

            if (Color == Color.White &&
                (Chessboard.WhiteKingsideCastlingAvailable || Chessboard.WhiteQueensideCastlingAvailable))
            {
                SettableChessboard.WhiteKingsideCastlingAvailable = false;
                SettableChessboard.WhiteQueensideCastlingAvailable = false;
            }

            if (Color == Color.Black &&
                (Chessboard.BlackKingsideCastlingAvailable || Chessboard.BlackQueensideCastlingAvailable))
            {
                SettableChessboard.BlackKingsideCastlingAvailable = false;
                SettableChessboard.BlackQueensideCastlingAvailable = false;
            }
        }

        public override IEnumerable<IMove> Moves
        {
            get
            {
                var moves = new List<IMove>();
                var potentialMoveCoordinates = GetPotentialMoveCoordinates();

                foreach (var potentialMoveCoordinate in potentialMoveCoordinates)
                {
                    this.TryAddMove(potentialMoveCoordinate.Item1, potentialMoveCoordinate.Item2, moves);
                }

                return moves;
            }
        }

        private IEnumerable<Tuple<File, Rank>> GetPotentialMoveCoordinates()
        {
            var file = ChessboardField.File;
            var rank = ChessboardField.Rank;

            var potentialMoveCoordinates = new List<Tuple<File, Rank>>();

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(-1), rank.Add(-1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(-1), rank.Add(0)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(-1), rank.Add(1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(0), rank.Add(-1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(0), rank.Add(1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(1), rank.Add(-1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(1), rank.Add(0)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(1), rank.Add(1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            return potentialMoveCoordinates;
        }
    }
}