using System;
using System.Collections.Generic;
using System.Linq;
using Sue.Common.Model.ChessPiece;

namespace Sue.Common.Model.Chessboard.Internal
{
    internal class ArrayChessboard : ChessboardBase
    {
        private readonly ChessboardField[,] _chessBoard = new ChessboardField[8, 8];
        private readonly IChessPieceFactory _chessPieceFactory;

        public ArrayChessboard(IChessPieceFactory chessPieceFactory)
        {
            _chessPieceFactory = chessPieceFactory;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    _chessBoard[j, i] = new ChessboardField(j.ToFile(), i.ToRank(), this);
                }
            }
        }

        public override IChessboardField GetChessboardField(File file, Rank rank)
        {
            return _chessBoard[file.Index(), rank.Index()];
        }

        public override IChessPiece GetChessPiece(File file, Rank rank)
        {
            return GetChessboardField(file, rank).ChessPiece;
        }

        public override IEnumerable<IChessPiece> GetChessPieces(Color color)
        {
            return
                _chessBoard.OfType<IChessboardField>()
                    .Where(cf => !cf.Empty)
                    .Select(cf => cf.ChessPiece)
                    .Where(cp => cp.Color == color);
        }

        public override void SetChessPiece(ChessPieceKind chessPieceKind, Color color, File file, Rank rank)
        {
            var chessboardField = _chessBoard[file.Index(), rank.Index()];
            chessboardField.ChessPiece = _chessPieceFactory.Create(chessPieceKind, color, chessboardField);
        }
    }
}