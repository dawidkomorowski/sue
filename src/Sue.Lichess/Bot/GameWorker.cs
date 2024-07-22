using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Sue.Engine.Model;
using Sue.Lichess.Api;
using Sue.Lichess.Api.GameEvents;

namespace Sue.Lichess.Bot;

internal sealed class GameWorker
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly LichessClient _lichessClient;
    private readonly string _botId;
    private readonly string _gameId;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Game? _game;
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
        // TODO CancellationTokenSource is not disposed!
        _cancellationTokenSource.Cancel();
    }

    private async Task Run()
    {
        using (ScopeContext.PushProperty(Constants.GameIdLogProperty, _gameId))
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    Logger.Info("Connecting to game: {0}.", _gameId);

                    using var gameStream = await _lichessClient.OpenGameStreamAsync(_gameId);

                    if (_game is null)
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

            Logger.Debug("Disposing Game instance - gameId: {0}", _gameId);
            _game?.Dispose();

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
        if (_game is not null && _game.HasError)
        {
            throw new InvalidOperationException("Game is in error state.");
        }

        return Task.CompletedTask;
    }

    private async Task HandleEventAsync(GameFullEvent gameFullEvent)
    {
        await HandleEasterEgg(gameFullEvent);

        var initialFen = gameFullEvent.InitialFen == "startpos" ? Fen.StartPos : gameFullEvent.InitialFen;
        var myColor = gameFullEvent.WhiteId == _botId ? Color.White : Color.Black;

        _game?.Dispose();
        _game = new Game(_lichessClient, _gameId, initialFen, myColor, gameFullEvent.HasClock);

        var whiteTime = TimeSpan.FromMilliseconds(gameFullEvent.WhiteTimeMs);
        var blackTime = TimeSpan.FromMilliseconds(gameFullEvent.BlackTimeMs);

        _game.Update(gameFullEvent.Moves, whiteTime, blackTime);
    }

    private Task HandleEventAsync(GameStateEvent gameStateEvent)
    {
        var whiteTime = TimeSpan.FromMilliseconds(gameStateEvent.WhiteTimeMs);
        var blackTime = TimeSpan.FromMilliseconds(gameStateEvent.BlackTimeMs);

        _game?.Update(gameStateEvent.Moves, whiteTime, blackTime);

        return Task.CompletedTask;
    }

    private async Task HandleEasterEgg(GameFullEvent gameFullEvent)
    {
        const string kuphelId = "kuphel";
        if (gameFullEvent.WhiteId == kuphelId || gameFullEvent.BlackId == kuphelId)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            await _lichessClient.WriteChatMessageAsync(_gameId, "Dear Kuphel, I was waiting for you <3.");
        }
    }
}