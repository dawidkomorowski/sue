using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using Sue.Lichess.Api;
using Sue.Lichess.Api.LichessEvents;

namespace Sue.Lichess.Bot;

public sealed class LichessBot : IDisposable
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly LichessClient _lichessClient;
    private readonly Dictionary<string, GameWorker> _gameWorkers = new();
    private string _botId = string.Empty;

    public LichessBot(string apiToken)
    {
        _lichessClient = new LichessClient(apiToken);
    }

    public async Task RunAsync()
    {
        Logger.Info("Connecting to Lichess.");

        _botId = await _lichessClient.GetAccountId();
        Logger.Info("Retrieved account id: {0}", _botId);

        while (true)
        {
            try
            {
                using var eventStream = await _lichessClient.OpenEventStreamAsync();

                while (!eventStream.EndOfStream)
                {
                    var lichessEvent = await eventStream.ReadEventAsync();
                    Logger.Info("Event received: {0}", lichessEvent);

                    await DispatchEventAsync(lichessEvent);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                Logger.Warn("Reconnecting to Lichess.");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }

    public void Dispose()
    {
        _lichessClient.Dispose();
    }

    private async Task DispatchEventAsync(LichessEvent lichessEvent)
    {
        switch (lichessEvent)
        {
            case PingEvent pingEvent:
                await HandleEventAsync(pingEvent);
                break;
            case ChallengeEvent challengeEvent:
                await HandleEventAsync(challengeEvent);
                break;
            case GameStartEvent gameStartEvent:
                await HandleEventAsync(gameStartEvent);
                break;
            case GameFinishEvent gameFinishEvent:
                await HandleEventAsync(gameFinishEvent);
                break;
            default:
                Logger.Warn("DispatchEventAsync - No event handler for event: {0}", lichessEvent.ToString());
                break;
        }
    }

    private Task HandleEventAsync(PingEvent pingEvent)
    {
        return Task.CompletedTask;
    }

    private async Task HandleEventAsync(ChallengeEvent challengeEvent)
    {
        if (challengeEvent.DestinationUserId == _botId && !challengeEvent.IsRated)
        {
            await _lichessClient.AcceptChallengeAsync(challengeEvent.ChallengeId);
            Logger.Info("Challenge accepted: {0}", challengeEvent.ChallengeId);
        }
        else
        {
            Logger.Info("Challenge ignored: {0}", challengeEvent.ChallengeId);
        }
    }

    private Task HandleEventAsync(GameStartEvent gameStartEvent)
    {
        if (_gameWorkers.ContainsKey(gameStartEvent.GameId))
        {
            Logger.Warn("GameWorker for game {0} already exists.", gameStartEvent.GameId);
        }
        else
        {
            var gameWorker = new GameWorker(_lichessClient, _botId, gameStartEvent.GameId);
            _gameWorkers.Add(gameStartEvent.GameId, gameWorker);
            gameWorker.Start();
        }

        return Task.CompletedTask;
    }

    private Task HandleEventAsync(GameFinishEvent gameFinishEvent)
    {
        if (_gameWorkers.TryGetValue(gameFinishEvent.GameId, out var gameWorker))
        {
            gameWorker.Stop();
            _gameWorkers.Remove(gameFinishEvent.GameId);
        }
        else
        {
            Logger.Warn("GameWorker for game {0} no longer exists.", gameFinishEvent.GameId);
        }

        return Task.CompletedTask;
    }
}