﻿using System;
using System.Diagnostics;
using NLog;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal sealed class MoveSearch
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private const int MaxDepth = 50;
    private readonly Random _random;
    private readonly SearchSettings _settings;
    private readonly Stopwatch _nodesPerSecondStopwatch = new();
    private readonly Stopwatch _searchTimeStopwatch = new();
    private int _nodesProcessed = 0;
    private int _nodesPerDepth = 0;
    private int _nodesPerSecond = 0;
    private TimeSpan _searchTime;
    private Comparison<Move>? _moveComparison;
    private bool _usePv;

    public MoveSearch(Random random, SearchSettings settings)
    {
        _random = random;
        _settings = settings;
    }

    public Move? FindBestMove(Chessboard chessboard, TimeSpan searchTime)
    {
        _nodesPerSecondStopwatch.Restart();
        _nodesProcessed = 0;
        _nodesPerDepth = 0;
        _nodesPerSecond = 0;

        _searchTimeStopwatch.Restart();
        _searchTime = searchTime;

        var depthLimit = MaxDepth;

        if (_settings.UseFixedDepth)
        {
            depthLimit = _settings.FixedDepth.Value;
            _searchTime = TimeSpan.MaxValue;
        }

        _moveComparison = (m1, m2) =>
        {
            var mv1 = chessboard.GetChessPiece(m1.To) is not ChessPiece.None ? 0 : 1;
            var mv2 = chessboard.GetChessPiece(m2.To) is not ChessPiece.None ? 0 : 1;
            return mv1.CompareTo(mv2);
        };

        ScoredMove? bestMove = null;
        var depthCompleted = 0;
        Span<Move> pvBuffer = stackalloc Move[depthLimit];

        for (var depth = 1; depth <= depthLimit; depth++)
        {
            var pvIn = pvBuffer.Slice(0, depth - 1);
            var pvOut = pvBuffer.Slice(0, depth);

            Logger.Trace("Start search with depth {0}.", depth);
            var alphaBetaMove = AlphaBetaRootSearch(chessboard, depth, pvIn, pvOut);

            if (SearchTimeIsOver())
            {
                Logger.Trace("Search time is over. Iterative deepening aborted at depth: {0}", depth);
                break;
            }

            depthCompleted = depth;
            bestMove = alphaBetaMove;

            if (bestMove is null)
            {
                Logger.Trace("Best move is null. Stop further search.");
                break;
            }

            if (bestMove.Value.Score.IsMate)
            {
                Logger.Trace("Best move is mate. Stop further search.");
                break;
            }
        }

        if (bestMove is null)
        {
            Logger.Trace("Best move is null. Returning null.");
            return null;
        }

        Logger.Trace("Final best move: {0} Score: {1} Depth: {2}", bestMove.Value.Move.ToUci(), bestMove.Value.Score, depthCompleted);
        Logger.Trace("Nodes processed: {0}", _nodesProcessed);

        return bestMove.Value.Move;
    }

    private ScoredMove? AlphaBetaRootSearch(Chessboard chessboard, int depth, ReadOnlySpan<Move> pvIn, Span<Move> pvOut)
    {
        _nodesPerSecondStopwatch.Restart();
        _nodesPerDepth = 0;
        _usePv = true;

        Span<Move> moveBuffer = stackalloc Move[Chessboard.MoveBufferSize];
        var moveCount = chessboard.GetMoveCandidates(moveBuffer);
        var moveCandidates = moveBuffer.Slice(0, moveCount);

        if (moveCandidates.Length == 0)
        {
            Logger.Trace("No moves available.");
            return null;
        }

        _random.Shuffle(moveCandidates);

        SortMoves(moveCandidates);

        if (_usePv)
        {
            SortPvMove(moveCandidates, pvIn, 0);
        }

        var min = Score.Max;
        var max = Score.Min;
        var alpha = Score.Min;
        var beta = Score.Max;
        var bestMove = moveCandidates[0];

        var totalSizeOfPvBuffers = (((1 + depth) * depth) / 2) + depth;
        Span<Move> pvBuffersWithResultBuffer = stackalloc Move[totalSizeOfPvBuffers];
        var pv = pvBuffersWithResultBuffer.Slice(0, depth);
        var pvCandidate = pvBuffersWithResultBuffer.Slice(depth, depth);
        var pvBuffers = pvBuffersWithResultBuffer.Slice(depth, totalSizeOfPvBuffers - depth);

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = AlphaBetaSearch(chessboard, depth - 1, alpha, beta, 1, pvBuffers, pvIn);
            chessboard.RevertMove();

            Logger.Trace("Move: {0} Score: {1}", move.ToUci(), score);

            pvCandidate[0] = move;

            if (chessboard.ActiveColor is Color.White)
            {
                if (score > max)
                {
                    max = score;
                    bestMove = move;
                    pvCandidate.CopyTo(pv);
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
                    bestMove = move;
                    pvCandidate.CopyTo(pv);
                }

                if (min < beta)
                {
                    beta = min;
                }
            }

            if (SearchTimeIsOver())
            {
                return null;
            }
        }

        var bestMoveScore = chessboard.ActiveColor is Color.White ? max : min;

        Logger.Trace("Best move: {0} Score: {1} Depth: {2}", bestMove.ToUci(), bestMoveScore, depth);
        Logger.Trace("Nodes processed per depth: {0} Depth: {1}", _nodesPerDepth, depth);

        for (var i = 0; i < depth; i++)
        {
            Logger.Trace("PV[{0}] = {1}", i, pv[i]);
        }

        pv.CopyTo(pvOut);

        return new ScoredMove(bestMove, bestMoveScore);
    }

    private Score AlphaBetaSearch(Chessboard chessboard, int depth, Score alpha, Score beta, int halfMove, Span<Move> pvBuffers, ReadOnlySpan<Move> pvIn)
    {
        var mateInMultiplier = (int)Math.Ceiling(halfMove / 2d);

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

        SortMoves(moveCandidates);

        if (_usePv)
        {
            SortPvMove(moveCandidates, pvIn, halfMove);
        }

        var min = Score.Max;
        var max = Score.Min;
        var depthPlusOne = depth + 1;
        var pvStartIndex = pvBuffers.Length - (((1 + depthPlusOne) * depthPlusOne) / 2) + 1;
        var pv = pvBuffers.Slice(pvStartIndex, depth);
        var pvCandidate = pvBuffers.Slice(pvStartIndex + depth, depth);

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = AlphaBetaSearch(chessboard, depth - 1, alpha, beta, halfMove + 1, pvBuffers, pvIn);
            chessboard.RevertMove();

            pvCandidate[0] = move;

            if (chessboard.ActiveColor is Color.White)
            {
                if (score > max)
                {
                    max = score;
                    pvCandidate.CopyTo(pv);
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
                    pvCandidate.CopyTo(pv);
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

            if (SearchTimeIsOver())
            {
                break;
            }
        }

        return chessboard.ActiveColor is Color.White ? max : min;
    }

    private void SortMoves(Span<Move> moves)
    {
        Debug.Assert(_moveComparison != null, nameof(_moveComparison) + " != null");
        moves.Sort(_moveComparison);
    }

    private void SortPvMove(Span<Move> moves, ReadOnlySpan<Move> pv, int pvMoveIndex)
    {
        if (pv.Length <= pvMoveIndex)
        {
            _usePv = false;
            return;
        }

        if (moves.Length < 2)
        {
            return;
        }

        var pvMove = pv[pvMoveIndex];

        for (var i = moves.Length - 1; i > 0; i--)
        {
            var move = moves[i];
            if (move == pvMove)
            {
                moves[i] = moves[i - 1];
                moves[i - 1] = pvMove;
            }
        }
    }

    private bool SearchTimeIsOver()
    {
        return _searchTimeStopwatch.Elapsed > _searchTime;
    }

    private void UpdateStatisticsForLeafNode()
    {
        _nodesProcessed++;
        _nodesPerDepth++;
        _nodesPerSecond++;

        if (_nodesPerSecondStopwatch.Elapsed > TimeSpan.FromSeconds(1))
        {
            Logger.Trace("Nodes per second: {0}", _nodesPerSecond);
            _nodesPerSecondStopwatch.Restart();
            _nodesPerSecond = 0;
        }
    }
}