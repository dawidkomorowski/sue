using System;
using System.Collections.Generic;
using System.Linq;
using Sue.Common.Model.ChessPiece;

namespace Sue.Common.Model.Chessboard.Internal
{
    internal class ArrayChessboard : ISettableChessboard
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

        public Color CurrentPlayer { get; set; }
        public bool WhiteKingsideCastlingAvailable { get; set; }
        public bool WhiteQueenssideCastlingAvailable { get; set; }
        public bool BlackKingsideCastlingAvailable { get; set; }
        public bool BlackQueensideCastlingAvailable { get; set; }
        public IChessboardField EnPassantTargetField { get; set; }
        public int HalfmoveClock { get; set; }
        public int FullmoveNumber { get; set; }

        public IChessboardField GetChessboardField(File file, Rank rank)
        {
            return _chessBoard[file.Index(), rank.Index()];
        }

        public IChessPiece GetChessPiece(File file, Rank rank)
        {
            return GetChessboardField(file, rank).ChessPiece;
        }

        public IEnumerable<IChessPiece> GetChessPieces(Color color)
        {
            return
                _chessBoard.OfType<IChessboardField>()
                    .Where(cf => !cf.Empty)
                    .Select(cf => cf.ChessPiece)
                    .Where(cp => cp.Color == color);
        }

        public void SetChessPiece(ChessPieceKind chessPieceKind, Color color, File file, Rank rank)
        {
            var chessboardField = _chessBoard[file.Index(), rank.Index()];
            chessboardField.ChessPiece = _chessPieceFactory.Create(chessPieceKind, color, chessboardField);
        }

        private void SetupPieces()
        {
            #region White Pieces Setup

            for (var j = 0; j < 8; j++)
            {
                SetChessPiece(ChessPieceKind.Pawn, Color.White, j.ToFile(), Rank.Two);
            }

            SetChessPiece(ChessPieceKind.Bishop, Color.White, File.C, Rank.One);
            SetChessPiece(ChessPieceKind.Bishop, Color.White, File.F, Rank.One);

            SetChessPiece(ChessPieceKind.Knight, Color.White, File.B, Rank.One);
            SetChessPiece(ChessPieceKind.Knight, Color.White, File.G, Rank.One);

            SetChessPiece(ChessPieceKind.Rook, Color.White, File.A, Rank.One);
            SetChessPiece(ChessPieceKind.Rook, Color.White, File.H, Rank.One);

            SetChessPiece(ChessPieceKind.Queen, Color.White, File.D, Rank.One);
            SetChessPiece(ChessPieceKind.King, Color.White, File.E, Rank.One);

            #endregion

            #region Black Pieces Setup

            for (var j = 0; j < 8; j++)
            {
                SetChessPiece(ChessPieceKind.Pawn, Color.Black, j.ToFile(), Rank.Seven);
            }

            SetChessPiece(ChessPieceKind.Bishop, Color.Black, File.C, Rank.Eight);
            SetChessPiece(ChessPieceKind.Bishop, Color.Black, File.F, Rank.Eight);

            SetChessPiece(ChessPieceKind.Knight, Color.Black, File.B, Rank.Eight);
            SetChessPiece(ChessPieceKind.Knight, Color.Black, File.G, Rank.Eight);

            SetChessPiece(ChessPieceKind.Rook, Color.Black, File.A, Rank.Eight);
            SetChessPiece(ChessPieceKind.Rook, Color.Black, File.H, Rank.Eight);

            SetChessPiece(ChessPieceKind.Queen, Color.Black, File.D, Rank.Eight);
            SetChessPiece(ChessPieceKind.King, Color.Black, File.E, Rank.Eight);

            #endregion
        }
    }
}