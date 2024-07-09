using System;
using System.Diagnostics;
using NLog;
using Sue.Engine.Model;

namespace Sue.Engine.Search;

internal sealed class MiniMaxSearch
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private const int MaxPly = 3;
    private readonly Stopwatch _stopwatch = new();
    private int _nodesProcessed = 0;
    private int _nodesPerSecond = 0;

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

        var min = int.MaxValue;
        var max = int.MinValue;
        var bestMove = moveCandidates[0];

        foreach (var move in moveCandidates)
        {
            chessboard.MakeMove(move);
            var score = ScoreMove(chessboard, MaxPly);
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

    private int ScoreMove(Chessboard chessboard, int ply)
    {
        // Promote mates in less moves.
        if (chessboard.HasKingInCheck(chessboard.ActiveColor.Opposite()))
        {
            return 1000 * (ply + 1) * (chessboard.ActiveColor is Color.White ? 1 : -1);
        }

        if (KingIsGone(chessboard))
        {
            UpdateStatisticsForLeafNode();
            return 1000 * (ply + 1) * Math.Sign(MaterialEvaluation.Eval(chessboard));
        }

        if (ply == 0)
        {
            UpdateStatisticsForLeafNode();
            return MaterialEvaluation.Eval(chessboard);
        }

        var moveCandidates = chessboard.GetMoveCandidates();

        if (moveCandidates.Count == 0)
        {
            if (chessboard.HasKingInCheck(chessboard.ActiveColor))
            {
                return 1000 * (ply + 1) * (chessboard.ActiveColor is Color.White ? -1 : 1);
            }

            return 0;
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