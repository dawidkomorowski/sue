using System;
using System.Threading.Tasks;
using NLog;
using Sue.Lichess.Api;
using Sue.Lichess.Api.LichessEvents;

namespace Sue.Lichess.Bot;

public sealed class LichessBot : IDisposable
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly LichessClient _lichessClient;

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
            case ChallengeEvent challengeEvent:
                await HandleEventAsync(challengeEvent);
                break;
            case GameStartEvent gameStartEvent:
                await HandleEventAsync(gameStartEvent);
                break;
            default:
                Logger.Warn("DispatchEventAsync - No event handler for event: {0}", lichessEvent.ToString());
                break;
        }
    }

    private async Task HandleEventAsync(ChallengeEvent challengeEvent)
    {
        if (challengeEvent.ChallengerId == "TODO_TODO_TODO" && challengeEvent.DestinationUserId == "sue_bot")
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
        Logger.Info("Introduce yourself in chat - gameId: {0}", gameStartEvent.GameId);

        await Task.Delay(TimeSpan.FromMilliseconds(100));
        await _lichessClient.WriteChatMessageAsync(gameStartEvent.GameId, "Hello! I am Sue, also known as Simple UCI Engine.");
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        await _lichessClient.WriteChatMessageAsync(gameStartEvent.GameId, "I am in early stage of development so most of my actions are silly.");
    }
}