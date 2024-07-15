﻿using System;
using System.Diagnostics;
using NLog;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal sealed class MoveSearch
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private const int MaxDepth = 6;
    private readonly Stopwatch _stopwatch = new();
    private int _nodesProcessed = 0;
    private int _nodesPerSecond = 0;

    private static void SortMoves(Span<Move> moves, Chessboard chessboard)
    {
        moves.Sort((m1, m2) =>
        {
            var mv1 = chessboard.GetChessPiece(m1.To) is not ChessPiece.None ? 0 : 1;
            var mv2 = chessboard.GetChessPiece(m2.To) is not ChessPiece.None ? 0 : 1;
            return mv1.CompareTo(mv2);
        });
    }

    public Move? FindBestMove(Chessboard chessboard)
    {
        _stopwatch.Restart();
        _nodesProcessed = 0;
        _nodesPerSecond = 0;

        Span<Move> moveBuffer = stackalloc Move[Chessboard.MoveBufferSize];
        var moveCount = chessboard.GetMoveCandidates(moveBuffer);
        var moveCandidates = moveBuffer.Slice(0, moveCount);

        if (moveCandidates.Length == 0)
        {
            Logger.Trace("No moves available.");
            return null;
        }

        //var shuffledMoves = moveCandidates.ToArray();
        //Random.Shared.Shuffle(shuffledMoves);
        //moveCandidates = shuffledMoves;

        SortMoves(moveCandidates, chessboard);

        var min = Score.Max;
        var max = Score.Min;
        var alpha = Score.Min;
        var beta = Score.Max;
        var bestMove = moveCandidates[0];

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = ScoreMove(chessboard, MaxDepth - 1, alpha, beta);
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

    private Score ScoreMove(Chessboard chessboard, int depth, Score alpha, Score beta)
    {
        var mateInMultiplier = (int)Math.Ceiling((MaxDepth - depth) / 2d);

        if (chessboard.HasKingInCheck(chessboard.ActiveColor.Opposite()))
        {
            UpdateStatisticsForLeafNode();
            var mateIn = mateInMultiplier * (chessboard.ActiveColor is Color.White ? 1 : -1);
            return Score.CreateMate(mateIn);
        }

        Span<Move> moveBuffer = stackalloc Move[Chessboard.MoveBufferSize];
        var moveCount = chessboard.GetMoveCandidates(moveBuffer);
        var moveCandidates = moveBuffer.Slice(0, moveCount);

        if (moveCandidates.Length == 0)
        {
            UpdateStatisticsForLeafNode();

            if (chessboard.HasKingInCheck(chessboard.ActiveColor))
            {
                var mateIn = mateInMultiplier * (chessboard.ActiveColor is Color.White ? -1 : 1);
                return Score.CreateMate(mateIn);
            }

            return Score.Zero;
        }

        if (depth == 0)
        {
            UpdateStatisticsForLeafNode();
            return MaterialEvaluation.Eval(chessboard);
        }

        SortMoves(moveCandidates, chessboard);

        var min = Score.Max;
        var max = Score.Min;

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = ScoreMove(chessboard, depth - 1, alpha, beta);
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