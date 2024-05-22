using System;
using System.Threading.Tasks;
using NLog;
using Sue.Lichess.Bot;

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

        using var lichessBot = new LichessBot(apiToken);
        await lichessBot.RunAsync();
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