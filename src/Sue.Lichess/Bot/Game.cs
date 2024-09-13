using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Sue.Engine;
using Sue.Engine.Model;
using Sue.Lichess.Api;

namespace Sue.Lichess.Bot;

internal sealed class Game : IDisposable
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly LichessClient _lichessClient;
    private readonly string _gameId;
    private readonly string _initialFen;
    private readonly Color _myColor;
    private readonly bool _hasClock;
    private readonly ManualResetEventSlim _chessEngineIsReady = new(true);
    private readonly ChessEngine _chessEngine;

    private string _moves = string.Empty;
    private TimeSpan _whiteTime;
    private TimeSpan _blackTime;

    public Game(LichessClient lichessClient, string gameId, string initialFen, Color myColor, bool hasClock)
    {
        _lichessClient = lichessClient;
        _gameId = gameId;
        _initialFen = initialFen;
        _myColor = myColor;
        _hasClock = hasClock;
        _chessEngine = new ChessEngine();
    }

    public bool HasError { get; private set; }

    public void Update(string moves, TimeSpan whiteTime, TimeSpan blackTime)
    {
        if (!ItIsMyTurn(moves))
        {
            Logger.Debug("It is not my turn. Skipping call to chess engine, gameId: {0}", _gameId);
            return;
        }

        _chessEngineIsReady.Wait();

        if (HasError)
        {
            throw new InvalidOperationException("Game is in error state.");
        }

        _chessEngineIsReady.Reset();

        _moves = moves;
        _whiteTime = whiteTime;
        _blackTime = blackTime;

        Logger.Debug("Starting search for best move, gameId: {0}", _gameId);

        Task.Run(FindAndMakeMove_ThreadFunc);
    }

    private bool ItIsMyTurn(string moves)
    {
        var activeColor = _chessEngine.GetActiveColor(_initialFen, moves);
        return (_myColor is Color.White && activeColor is Color.White) || (_myColor is Color.Black && activeColor is Color.Black);
    }

    private async Task FindAndMakeMove_ThreadFunc()
    {
        using (ScopeContext.PushProperty(Constants.GameIdLogProperty, _gameId))
        {
            try
            {
                var chessEngineSettings = new ChessEngineSettings
                {
                    WhiteTime = _whiteTime,
                    BlackTime = _blackTime,
                    FixedSearchTime = _hasClock ? null : TimeSpan.FromSeconds(15)
                };

                var move = _chessEngine.FindBestMove(_initialFen, _moves, chessEngineSettings);
                if (move != null)
                {
                    Logger.Debug("Best move: {0}, gameId: {1}", move, _gameId);
                    await _lichessClient.MakeMoveAsync(_gameId, move);
                }
                else
                {
                    Logger.Error("Did not find any move! gameId: {0}", _gameId);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                HasError = true;
            }
            finally
            {
                _chessEngineIsReady.Set();
            }
        }
    }

    public void Dispose()
    {
        _chessEngineIsReady.Wait();
        _chessEngineIsReady.Dispose();
    }
}