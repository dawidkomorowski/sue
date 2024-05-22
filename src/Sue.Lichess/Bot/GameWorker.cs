using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Sue.Lichess.Api;
using Sue.Lichess.Api.GameEvents;

namespace Sue.Lichess.Bot;

internal sealed class GameWorker
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly LichessClient _lichessClient;
    private readonly string _gameId;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public GameWorker(LichessClient lichessClient, string gameId)
    {
        _lichessClient = lichessClient;
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
    }

    private async Task Run()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            try
            {
                Logger.Info("Connecting to game: {0}.", _gameId);

                using var gameStream = await _lichessClient.OpenGameStreamAsync(_gameId);

                Logger.Info("Introduce yourself in chat - gameId: {0}", _gameId);

                await Task.Delay(TimeSpan.FromMilliseconds(100));
                await _lichessClient.WriteChatMessageAsync(_gameId, "Hello! I am Sue, also known as Simple UCI Engine.");
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                await _lichessClient.WriteChatMessageAsync(_gameId, "I am in early stage of development so most of my actions are silly.");

                while (!gameStream.EndOfStream && !_cancellationTokenSource.IsCancellationRequested)
                {
                    var gameEvent = await gameStream.ReadEventAsync();
                    Logger.Info("Event received: {0}", gameEvent);

                    await DispatchEventAsync(gameEvent);
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
            }
        }

        Logger.Debug("Game worker stopped - gameId: {0}", _gameId);
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
            default:
                Logger.Warn("DispatchEventAsync - No event handler for event: {0}", gameEvent.ToString());
                break;
        }
    }

    private async Task HandleEventAsync(PingGameEvent pingGameEvent)
    {
    }

    private async Task HandleEventAsync(GameFullEvent gameFullEvent)
    {
        if (gameFullEvent.WhiteId == Constants.BotId)
        {
            await _lichessClient.MakeMoveAsync(_gameId, "e2e4");
        }
        else
        {
            await _lichessClient.MakeMoveAsync(_gameId, "e7e5");
        }
    }
}