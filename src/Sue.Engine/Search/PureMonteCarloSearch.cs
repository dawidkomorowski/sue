using System;
using System.Collections.Generic;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal sealed class PureMonteCarloSearch : ISearch
{
    private const int PlayOutsPerMove = 100;
    private const int MovesLimit = 200;

    public Move? FindBestMove(Chessboard chessboard)
    {
        var moveCandidates = chessboard.GetMoveCandidates();

        if (moveCandidates.Count == 0)
        {
            return null;
        }

        var bestScore = 0;
        var bestMove = moveCandidates[0];

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);

            var score = 0;
            for (var i = 0; i < PlayOutsPerMove; i++)
            {
                var won = PlayOut(chessboard, chessboard.ActiveColor);

                if (won)
                {
                    score += 1;
                }
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }

            chessboard.RevertMove();
        }

        return bestMove;
    }

    private bool PlayOut(Chessboard chessboard, Color myColor)
    {
        var movesPlayed = 0;
        bool won;

        while (true)
        {
            if (movesPlayed > MovesLimit)
            {
                throw new InvalidOperationException("Moves limit reached.");
            }

            var moveCandidates = chessboard.GetMoveCandidates();
            if (moveCandidates.Count == 0)
            {
                won = chessboard.ActiveColor != myColor;
                break;
            }

            if (EnemyKingIsCaptured(chessboard, moveCandidates))
            {
                won = chessboard.ActiveColor == myColor;
                break;
            }

            var randomMove = moveCandidates[Random.Shared.Next(moveCandidates.Count)];
            chessboard.MakeMove(randomMove);
            movesPlayed++;
        }

        for (var i = 0; i < movesPlayed; i++)
        {
            chessboard.RevertMove();
        }

        return won;
    }

    private bool EnemyKingIsCaptured(Chessboard chessboard, IReadOnlyList<Move> moves)
    {
        foreach (var move in moves)
        {
            var cp = chessboard.GetChessPiece(move.To);

            if ((chessboard.ActiveColor == Color.White && cp is ChessPiece.BlackKing) ||
                (chessboard.ActiveColor == Color.Black && cp is ChessPiece.WhiteKing))
            {
                return true;
            }
        }

        return false;
    }
}