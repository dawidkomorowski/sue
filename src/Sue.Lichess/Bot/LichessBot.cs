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

    public LichessBot(string apiToken)
    {
        _lichessClient = new LichessClient(apiToken);
    }

    public async Task RunAsync()
    {
        Logger.Info("Connecting to Lichess.");

        using var eventStream = await _lichessClient.OpenEventStreamAsync();

        while (!eventStream.EndOfStream)
        {
            var lichessEvent = await eventStream.ReadEventAsync();
            Logger.Info("Event received: {0}", lichessEvent);

            await DispatchEventAsync(lichessEvent);
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

    private async Task HandleEventAsync(PingEvent pingEvent)
    {
    }

    private async Task HandleEventAsync(ChallengeEvent challengeEvent)
    {
        if (challengeEvent.DestinationUserId == Constants.BotId && !challengeEvent.IsRated)
        {
            await _lichessClient.AcceptChallengeAsync(challengeEvent.ChallengeId);
            Logger.Info("Challenge accepted: {0}", challengeEvent.ChallengeId);
        }
        else
        {
            Logger.Info("Challenge ignored: {0}", challengeEvent.ChallengeId);
        }
    }

    private async Task HandleEventAsync(GameStartEvent gameStartEvent)
    {
        if (_gameWorkers.ContainsKey(gameStartEvent.GameId))
        {
            Logger.Warn("GameWorker for game {0} already exists.", gameStartEvent.GameId);
        }
        else
        {
            var gameWorker = new GameWorker(_lichessClient, gameStartEvent.GameId);
            _gameWorkers.Add(gameStartEvent.GameId, gameWorker);
            gameWorker.Start();
        }
    }

    private async Task HandleEventAsync(GameFinishEvent gameFinishEvent)
    {
        if (_gameWorkers.ContainsKey(gameFinishEvent.GameId))
        {
            _gameWorkers[gameFinishEvent.GameId].Stop();
            _gameWorkers.Remove(gameFinishEvent.GameId);
        }
        else
        {
            Logger.Warn("GameWorker for game {0} no longer exists.", gameFinishEvent.GameId);
        }
    }
}