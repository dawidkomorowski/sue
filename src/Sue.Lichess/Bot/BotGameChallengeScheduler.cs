using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Sue.Lichess.Api;

namespace Sue.Lichess.Bot;

internal sealed class BotGameChallengeScheduler
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly LichessClient _lichessClient;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(30);

    public BotGameChallengeScheduler(LichessClient lichessClient)
    {
        _lichessClient = lichessClient;
    }

    public void Start()
    {
        Logger.Debug("Start");
        Task.Run(Run);
    }

    public void Stop()
    {
        Logger.Debug("Stop");
        // TODO CancellationTokenSource is not disposed!
        _cancellationTokenSource.Cancel();
    }

    private async Task Run()
    {
        using (ScopeContext.PushProperty(Constants.IsSchedulerLogProperty, true))
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    Logger.Info("Waiting before next challenge: {0}.", _interval);
                    await Task.Delay(_interval);

                    var myRating = await _lichessClient.GetBlitzRating();
                    Logger.Info("My rating: {0}. Looking for next opponent.", myRating);

                    var onlineBots = await _lichessClient.GetOnlineBots();
                    var deviation = 100;

                    while (true)
                    {
                        var minOpponentRating = myRating - deviation;
                        var maxOpponentRating = myRating + deviation;

                        var candidates = onlineBots.Where(ob => ob.BlitzRating >= minOpponentRating && ob.BlitzRating <= maxOpponentRating).ToArray();
                        if (candidates.Length == 0)
                        {
                            Logger.Info("No suitable opponent found. Extending rating requirements.");

                            if (deviation > 1000)
                            {
                                Logger.Warn("Rating requirements limit exceeded. Skipping challenge this time.");
                                break;
                            }

                            deviation += 100;
                            continue;
                        }

                        Random.Shared.Shuffle(candidates);
                        var opponent = candidates[0];

                        Logger.Info("Sending challenge to opponent: {0}.", opponent);

                        await _lichessClient.CreateChallengeAsync(opponent.Id, true);
                        break;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
    }
}