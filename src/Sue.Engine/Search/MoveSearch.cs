using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal sealed class MoveSearch
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private const int MaxPly = 4;
    private readonly Stopwatch _stopwatch = new();
    private int _nodesProcessed = 0;
    private int _nodesPerSecond = 0;

    private IReadOnlyList<Move> SortMoves(IReadOnlyList<Move> moves, Chessboard chessboard)
    {
        var sortedMoves = moves.ToArray();
        Array.Sort(sortedMoves, (m1, m2) =>
        {
            var mv1 = chessboard.GetChessPiece(m1.To) is not ChessPiece.None ? 0 : 1;
            var mv2 = chessboard.GetChessPiece(m2.To) is not ChessPiece.None ? 0 : 1;
            return mv1.CompareTo(mv2);
        });
        return sortedMoves;
    }

    public Move? FindBestMove(Chessboard chessboard)
    {
        _stopwatch.Restart();
        _nodesProcessed = 0;
        _nodesPerSecond = 0;

        var moveCandidates = chessboard.GetMoveCandidates();

        if (moveCandidates.Count == 0)
        {
            Logger.Trace("No moves available.");
            return null;
        }

        //var shuffledMoves = moveCandidates.ToArray();
        //Random.Shared.Shuffle(shuffledMoves);
        //moveCandidates = shuffledMoves;

        moveCandidates = SortMoves(moveCandidates, chessboard);

        var min = Score.Max;
        var max = Score.Min;
        var alpha = Score.Min;
        var beta = Score.Max;
        var bestMove = moveCandidates[0];

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = ScoreMove(chessboard, MaxPly, alpha, beta);
            chessboard.RevertMove();

            Logger.Trace("Move: {0} Score: {1}", move.ToUci(), score);

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

        Logger.Trace("Best move: {0} Score: {1}", bestMove.ToUci(), chessboard.ActiveColor is Color.White ? max : min);
        Logger.Trace("Nodes processed: {0}", _nodesProcessed);

        return bestMove;
    }

    private Score ScoreMove(Chessboard chessboard, int ply, Score alpha, Score beta)
    {
        var mateInMultiplier = (int)Math.Ceiling((1 + MaxPly - ply) / 2d);

        if (chessboard.HasKingInCheck(chessboard.ActiveColor.Opposite()))
        {
            UpdateStatisticsForLeafNode();
            var mateIn = mateInMultiplier * (chessboard.ActiveColor is Color.White ? 1 : -1);
            return Score.CreateMate(mateIn);
        }

        if (KingIsGone(chessboard))
        {
            UpdateStatisticsForLeafNode();
            var mateIn = mateInMultiplier * Math.Sign(MaterialEvaluation.Eval(chessboard).Eval);
            return Score.CreateMate(mateIn);
        }

        var moveCandidates = chessboard.GetMoveCandidates();
        if (moveCandidates.Count == 0)
        {
            UpdateStatisticsForLeafNode();

            if (chessboard.HasKingInCheck(chessboard.ActiveColor))
            {
                var mateIn = mateInMultiplier * (chessboard.ActiveColor is Color.White ? -1 : 1);
                return Score.CreateMate(mateIn);
            }

            return Score.Zero;
        }

        if (ply == 0)
        {
            UpdateStatisticsForLeafNode();
            return MaterialEvaluation.Eval(chessboard);
        }

        moveCandidates = SortMoves(moveCandidates, chessboard);

        var min = Score.Max;
        var max = Score.Min;

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = ScoreMove(chessboard, ply - 1, alpha, beta);
            chessboard.RevertMove();

            if (chessboard.ActiveColor is Color.White)
            {
                if (score > max)
                {
                    max = score;
                }

                if (max >= beta)
                {
                    break;
                }

                if (max > alpha)
                {
                    alpha = max;
                }
            }
            else
            {
                if (score < min)
                {
                    min = score;
                }

                if (min <= alpha)
                {
                    break;
                }

                if (min < beta)
                {
                    beta = min;
                }
            }
        }

        return chessboard.ActiveColor is Color.White ? max : min;
    }

    private static bool KingIsGone(Chessboard chessboard)
    {
        var whiteKingIsGone = true;
        var blackKingIsGone = true;

        foreach (var position in Position.All)
        {
            var chessPiece = chessboard.GetChessPiece(position);

            if (chessPiece is ChessPiece.WhiteKing)
            {
                whiteKingIsGone = false;
            }

            if (chessPiece is ChessPiece.BlackKing)
            {
                blackKingIsGone = false;
            }
        }

        return whiteKingIsGone || blackKingIsGone;
    }

    private void UpdateStatisticsForLeafNode()
    {
        _nodesProcessed++;
        _nodesPerSecond++;

        if (_stopwatch.Elapsed > TimeSpan.FromSeconds(1))
        {
            Logger.Trace("Nodes per second: {0}", _nodesPerSecond);
            _stopwatch.Restart();
            _nodesPerSecond = 0;
        }
    }
}