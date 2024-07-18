using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Sue.Engine;
using Sue.Engine.Model;
using Sue.Lichess.Api;
using Sue.Lichess.Api.GameEvents;

namespace Sue.Lichess.Bot;

internal sealed class GameWorker
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private const string GameIdLogProperty = "gameId";
    private readonly LichessClient _lichessClient;
    private readonly string _botId;
    private readonly string _gameId;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private string _initialFen = string.Empty;
    private bool _myColorIsWhite = false;
    private int _consecutiveErrorsCounter = 0;

    public GameWorker(LichessClient lichessClient, string botId, string gameId)
    {
        _lichessClient = lichessClient;
        _botId = botId;
        _gameId = gameId;
    }

    public void Start()
    {
        Logger.Debug("Start - gameId: {0}", _gameId);
        Task.Run(Run);
    }

    public void Stop()
    {
        Logger.Debug("Stop - gameId: {0}", _gameId);
        _cancellationTokenSource.Cancel();
        // TODO CancellationTokenSource is not disposed!
    }

    private async Task Run()
    {
        using (ScopeContext.PushProperty(GameIdLogProperty, _gameId))
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    Logger.Info("Connecting to game: {0}.", _gameId);

                    using var gameStream = await _lichessClient.OpenGameStreamAsync(_gameId);

                    if (string.IsNullOrEmpty(_initialFen))
                    {
                        Logger.Info("Introduce yourself in chat - gameId: {0}", _gameId);

                        await Task.Delay(TimeSpan.FromMilliseconds(100));
                        await _lichessClient.WriteChatMessageAsync(_gameId, "Hello! I am Sue, also known as Simple UCI Engine.");
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
                        await _lichessClient.WriteChatMessageAsync(_gameId, "I am in early stage of development so most of my actions are silly.");
                    }

                    while (!gameStream.EndOfStream && !_cancellationTokenSource.IsCancellationRequested)
                    {
                        var gameEvent = await gameStream.ReadEventAsync();
                        Logger.Info("Event received: {0}", gameEvent);

                        await DispatchEventAsync(gameEvent);
                        _consecutiveErrorsCounter = 0;
                    }

                    if (gameStream.EndOfStream)
                    {
                        await _cancellationTokenSource.CancelAsync();
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    _consecutiveErrorsCounter++;

                    if (_consecutiveErrorsCounter > 5)
                    {
                        await _lichessClient.WriteChatMessageAsync(_gameId,
                            "I am so sorry but I need to resign. I got into an error state that I can't resolve.");
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        await _lichessClient.ResignGameAsync(_gameId);
                    }
                }
            }

            Logger.Debug("Game worker stopped - gameId: {0}", _gameId);
        }
    }

    private async Task DispatchEventAsync(GameEvent gameEvent)
    {
        switch (gameEvent)
        {
            case PingGameEvent pingGameEvent:
                await HandleEventAsync(pingGameEvent);
                break;
            case GameFullEvent gameFullEvent:
                await HandleEventAsync(gameFullEvent);
                break;
            case GameStateEvent gameStateEvent:
                await HandleEventAsync(gameStateEvent);
                break;
            default:
                Logger.Warn("DispatchEventAsync - No event handler for event: {0}", gameEvent.ToString());
                break;
        }
    }

    private Task HandleEventAsync(PingGameEvent pingGameEvent)
    {
        return Task.CompletedTask;
    }

    private async Task HandleEventAsync(GameFullEvent gameFullEvent)
    {
        _initialFen = gameFullEvent.InitialFen == "startpos" ? Fen.StartPos : gameFullEvent.InitialFen;
        _myColorIsWhite = gameFullEvent.WhiteId == _botId;

        await HandleEasterEgg(gameFullEvent);
        await TryMakeMoveAsync(gameFullEvent.Moves);
    }

    private async Task HandleEventAsync(GameStateEvent gameStateEvent)
    {
        await TryMakeMoveAsync(gameStateEvent.Moves);
    }

    private async Task TryMakeMoveAsync(string moves)
    {
        if (!IsItMyTurn(moves))
        {
            return;
        }

        Logger.Debug("Starting search for best move, gameId: {0}", _gameId);

        var move = ChessEngine.FindBestMove(_initialFen, moves);
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

    private bool IsItMyTurn(string moves)
    {
        var activeColor = ChessEngine.GetActiveColor(_initialFen, moves);
        return (_myColorIsWhite && activeColor is Color.White) || (!_myColorIsWhite && activeColor is Color.Black);
    }

    private async Task HandleEasterEgg(GameFullEvent gameFullEvent)
    {
        const string kuphelId = "kuphel";
        if (gameFullEvent.WhiteId == kuphelId || gameFullEvent.BlackId == kuphelId)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            await _lichessClient.WriteChatMessageAsync(_gameId, "Dear Kuphel, I was waiting for you ❤️.");
        }
    }
}