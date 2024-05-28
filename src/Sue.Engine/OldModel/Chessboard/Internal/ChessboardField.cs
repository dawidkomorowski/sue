﻿using Sue.Engine.Model;
using Sue.Engine.OldModel.ChessPiece;

namespace Sue.Engine.OldModel.Chessboard.Internal
{
    public class ChessboardField : IChessboardField
    {
        public ChessboardField(File file, Rank rank, ISettableChessboard chessboard)
        {
            File = file;
            Rank = rank;
            SettableChessboard = chessboard;
        }

        public File File { get; }
        public Rank Rank { get; }
        public IChessPiece ChessPiece { get; set; }
        public bool Empty => ChessPiece == null;
        public IChessboard Chessboard => SettableChessboard;
        public ISettableChessboard SettableChessboard { get; }
    }
}