using System;
using System.Threading.Tasks;
using NLog;
using Sue.Lichess;

namespace Sue;

internal class Program
{
    private const string LichessApiTokenEnvVar = "LICHESS_API_TOKEN";
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    static async Task Main()
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

        var apiToken = Environment.GetEnvironmentVariable(LichessApiTokenEnvVar);
        if (string.IsNullOrEmpty(apiToken))
        {
            Logger.Error("{0} environment variable is not set.", LichessApiTokenEnvVar);
            return;
        }

        Logger.Info("Connecting to Lichess.");

        using var lichessClient = new LichessClient(apiToken);
        using var eventStream = await lichessClient.OpenEventStreamAsync();

        while (!eventStream.EndOfStream)
        {
            var lichessEvent = await eventStream.ReadEventAsync();
            Logger.Info("Event received: {0}", lichessEvent);

            if (lichessEvent is ChallengeEvent challengeEvent)
            {
                if (challengeEvent.ChallengerId == "TODO_TODO_TODO" && challengeEvent.DestinationUserId == "sue_bot")
                {
                    await lichessClient.AcceptChallenge(challengeEvent.ChallengeId);
                    Logger.Info("Accepted challenge: {0}", challengeEvent.ChallengeId);
                }
                else
                {
                    Logger.Info("Ignored challenge: {0}", challengeEvent.ChallengeId);
                }
            }

            if (lichessEvent is GameStartEvent gameStartEvent)
            {
                Logger.Info("Introduce yourself in chat - gameId: {0}", gameStartEvent.GameId);

                await Task.Delay(TimeSpan.FromMilliseconds(100));
                await lichessClient.WriteChatMessage(gameStartEvent.GameId, "Hello! I am Sue, also known as Simple UCI Engine.");
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                await lichessClient.WriteChatMessage(gameStartEvent.GameId, "I am in early stage of development so most of my actions are silly.");
            }
        }
    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            Logger.Fatal(ex);
        }

        Environment.FailFast("Unhandled exception.", e.ExceptionObject as Exception);
    }
}