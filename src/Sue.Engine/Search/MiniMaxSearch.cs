using System;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal sealed class MiniMaxSearch : ISearch
{
    private const int MaxPly = 3;

    public Move? FindBestMove(Chessboard chessboard)
    {
        var moveCandidates = chessboard.GetMoveCandidates();

        if (moveCandidates.Count == 0)
        {
            return null;
        }

        var min = int.MaxValue;
        var max = int.MinValue;
        var bestMove = moveCandidates[0];

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = ScoreMove(chessboard, MaxPly);
            chessboard.RevertMove();

            if (chessboard.ActiveColor is Color.White)
            {
                if (score > max)
                {
                    max = score;
                    bestMove = move;
                }
            }
            else
            {
                if (score < min)
                {
                    min = score;
                    bestMove = move;
                }
            }
        }

        // TODO
        Console.WriteLine("SCORE: " + (chessboard.ActiveColor is Color.White ? max : min));

        return bestMove;
    }

    private int ScoreMove(Chessboard chessboard, int ply)
    {
        if (ply == 0)
        {
            return Eval(chessboard);
        }

        var moveCandidates = chessboard.GetMoveCandidates();

        if (moveCandidates.Count == 0)
        {
            return chessboard.ActiveColor is Color.White ? -1 : 1;
        }

        var min = int.MaxValue;
        var max = int.MinValue;

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = ScoreMove(chessboard, ply - 1);
            chessboard.RevertMove();

            if (chessboard.ActiveColor is Color.White)
            {
                if (score > max)
                {
                    max = score;
                }
            }
            else
            {
                if (score < min)
                {
                    min = score;
                }
            }
        }

        return chessboard.ActiveColor is Color.White ? max : min;
    }

    private int Eval(Chessboard chessboard)
    {
        var score = 0;

        foreach (var position in Position.All)
        {
            var chessPiece = chessboard.GetChessPiece(position);

            score += chessPiece switch
            {
                ChessPiece.None => 0,
                ChessPiece.WhiteKing => 200,
                ChessPiece.WhiteQueen => 9,
                ChessPiece.WhiteRook => 5,
                ChessPiece.WhiteBishop => 3,
                ChessPiece.WhiteKnight => 3,
                ChessPiece.WhitePawn => 1,
                ChessPiece.BlackKing => -200,
                ChessPiece.BlackQueen => -9,
                ChessPiece.BlackRook => -5,
                ChessPiece.BlackBishop => -3,
                ChessPiece.BlackKnight => -3,
                ChessPiece.BlackPawn => -1,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return score;
    }
}