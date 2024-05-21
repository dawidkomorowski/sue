﻿using System;
using System.Linq;
using Sue.Engine.Model.Chessboard;

namespace Sue.Engine.Model.Fen.Internal
{
    public class ChessFieldParser : IChessFieldParser
    {
        public ChessField Parse(string chessFieldString)
        {
            if(chessFieldString.Length != 2) throw new ArgumentException($"{chessFieldString} is not valid format of field coordinates.", nameof(chessFieldString));
            return new ChessField(chessFieldString.First().ToFile(), chessFieldString.Last().ToRank());
        }
    }
}