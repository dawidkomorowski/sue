using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NLog;

namespace Sue
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        static async Task Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            var token = Environment.GetEnvironmentVariable("LICHESS_API_TOKEN");

            using var httpClient = new HttpClient(new HttpClientHandler());
            httpClient.BaseAddress = new Uri("https://lichess.org/api/");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            await using var stream = await httpClient.GetStreamAsync(new Uri("stream/event", UriKind.Relative));
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                Logger.Info(line);
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
}