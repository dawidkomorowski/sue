using System;
using System.Collections.Generic;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    public class Knight : ChessPiece
    {
        public Knight(Color color, ChessboardField chessboardField) : base(color, chessboardField)
        {
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
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(1), rank.Add(2)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(2), rank.Add(1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(1), rank.Add(-2)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(2), rank.Add(-1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(-1), rank.Add(-2)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(-2), rank.Add(-1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(-1), rank.Add(2)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                potentialMoveCoordinates.Add(new Tuple<File, Rank>(file.Add(-2), rank.Add(1)));
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            return potentialMoveCoordinates;
        }
    }
}