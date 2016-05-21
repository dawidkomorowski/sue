using System;
using System.Collections.Generic;
using System.Linq;
using Sue.Common.Model.ChessPiece;

namespace Sue.Common.Model.Chessboard.Internal
{
    internal class ArrayChessboard : IChessboard
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
                    _chessBoard[j, i] = new ChessboardField(FileOf(j), RankOf(i), this);
                }
            }

            SetupPieces();
        }

        public Color CurrentPlayer { get; }
        public bool WhiteKingsideCastlingAvailable { get; }
        public bool WhiteQueenssideCastlingAvailable { get; }
        public bool BlackKingsideCastlingAvailable { get; }
        public bool BlackQueensideCastlingAvailable { get; }
        public IChessboardField EnPassantTargetField { get; }
        public ushort HalfmoveClock { get; }
        public ushort FullmoveNumber { get; }

        public IChessboardField GetChessboardField(File file, Rank rank)
        {
            return _chessBoard[IndexOf(file), IndexOf(rank)];
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

        private void SetChessPiece(ChessPieceKind chessPieceKind, Color color, File file, Rank rank)
        {
            var chessboardField = _chessBoard[IndexOf(file), IndexOf(rank)];
            chessboardField.ChessPiece = _chessPieceFactory.Create(chessPieceKind, color, chessboardField);
        }

        private void SetupPieces()
        {
            #region White Pieces Setup

            for (var j = 0; j < 8; j++)
            {
                SetChessPiece(ChessPieceKind.Pawn, Color.White, FileOf(j), Rank.Two);
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
                SetChessPiece(ChessPieceKind.Pawn, Color.Black, FileOf(j), Rank.Seven);
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

        private int IndexOf(File file)
        {
            switch (file)
            {
                case File.A:
                    return 0;
                case File.B:
                    return 1;
                case File.C:
                    return 2;
                case File.D:
                    return 3;
                case File.E:
                    return 4;
                case File.F:
                    return 5;
                case File.G:
                    return 6;
                case File.H:
                    return 7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(file), file, null);
            }
        }

        private int IndexOf(Rank rank)
        {
            switch (rank)
            {
                case Rank.One:
                    return 0;
                case Rank.Two:
                    return 1;
                case Rank.Three:
                    return 2;
                case Rank.Four:
                    return 3;
                case Rank.Five:
                    return 4;
                case Rank.Six:
                    return 5;
                case Rank.Seven:
                    return 6;
                case Rank.Eight:
                    return 7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rank), rank, null);
            }
        }

        private File FileOf(int index)
        {
            switch (index)
            {
                case 0:
                    return File.A;
                case 1:
                    return File.B;
                case 2:
                    return File.C;
                case 3:
                    return File.D;
                case 4:
                    return File.E;
                case 5:
                    return File.F;
                case 6:
                    return File.G;
                case 7:
                    return File.H;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index), index, null);
            }
        }

        private Rank RankOf(int index)
        {
            switch (index)
            {
                case 0:
                    return Rank.One;
                case 1:
                    return Rank.Two;
                case 2:
                    return Rank.Three;
                case 3:
                    return Rank.Four;
                case 4:
                    return Rank.Five;
                case 5:
                    return Rank.Six;
                case 6:
                    return Rank.Seven;
                case 7:
                    return Rank.Eight;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index), index, null);
            }
        }
    }
}