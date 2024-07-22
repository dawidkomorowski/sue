using System;
using System.Diagnostics;
using NLog;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal sealed class MoveSearch
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private const int MaxDepth = 50;
    private readonly Stopwatch _nodesPerSecondStopwatch = new();
    private readonly Stopwatch _searchTimeStopwatch = new();
    private int _nodesProcessed = 0;
    private int _nodesPerDepth = 0;
    private int _nodesPerSecond = 0;
    private TimeSpan _searchTime;

    public Move? FindBestMove(Chessboard chessboard, TimeSpan searchTime)
    {
        _nodesPerSecondStopwatch.Restart();
        _nodesProcessed = 0;
        _nodesPerDepth = 0;
        _nodesPerSecond = 0;

        _searchTimeStopwatch.Restart();
        _searchTime = searchTime;

        ScoredMove? bestMove = null;

        for (var depth = 1; depth <= MaxDepth; depth++)
        {
            Logger.Trace("Start search with depth {0}.", depth);
            var alphaBetaMove = AlphaBetaRootSearch(chessboard, depth);

            if (SearchTimeIsOver())
            {
                Logger.Trace("Search time is over. Iterative deepening stopped at depth: {0}", depth);
                break;
            }

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

        Logger.Trace("Final best move: {0} Score: {1}", bestMove.Value.Move.ToUci(), bestMove.Value.Score);
        Logger.Trace("Nodes processed: {0}", _nodesProcessed);

        return bestMove.Value.Move;
    }

    private ScoredMove? AlphaBetaRootSearch(Chessboard chessboard, int depth)
    {
        _nodesPerSecondStopwatch.Restart();
        _nodesPerDepth = 0;

        Span<Move> moveBuffer = stackalloc Move[Chessboard.MoveBufferSize];
        var moveCount = chessboard.GetMoveCandidates(moveBuffer);
        var moveCandidates = moveBuffer.Slice(0, moveCount);

        if (moveCandidates.Length == 0)
        {
            Logger.Trace("No moves available.");
            return null;
        }

        SortMoves(moveCandidates, chessboard);

        // TODO Alpha and beta are not preserved between search starting from root.
        var min = Score.Max;
        var max = Score.Min;
        var alpha = Score.Min;
        var beta = Score.Max;
        var bestMove = moveCandidates[0];

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = AlphaBetaSearch(chessboard, depth - 1, alpha, beta, 1);
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

            if (SearchTimeIsOver())
            {
                return null;
            }
        }

        var bestMoveScore = chessboard.ActiveColor is Color.White ? max : min;

        Logger.Trace("Best move: {0} Score: {1} Depth: {2}", bestMove.ToUci(), bestMoveScore, depth);
        Logger.Trace("Nodes processed per depth: {0} Depth: {1}", _nodesPerDepth, depth);

        return new ScoredMove(bestMove, bestMoveScore);
    }

    private Score AlphaBetaSearch(Chessboard chessboard, int depth, Score alpha, Score beta, int halfMove)
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

        SortMoves(moveCandidates, chessboard);

        var min = Score.Max;
        var max = Score.Min;

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = AlphaBetaSearch(chessboard, depth - 1, alpha, beta, halfMove + 1);
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

            if (SearchTimeIsOver())
            {
                break;
            }
        }

        return chessboard.ActiveColor is Color.White ? max : min;
    }

    private static void SortMoves(Span<Move> moves, Chessboard chessboard)
    {
        moves.Sort((m1, m2) =>
        {
            var mv1 = chessboard.GetChessPiece(m1.To) is not ChessPiece.None ? 0 : 1;
            var mv2 = chessboard.GetChessPiece(m2.To) is not ChessPiece.None ? 0 : 1;
            return mv1.CompareTo(mv2);
        });
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