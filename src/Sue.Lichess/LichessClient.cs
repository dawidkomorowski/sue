using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NLog;

namespace Sue.Lichess;

public sealed class LichessClient : IDisposable
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly HttpClient _httpClient;

    public LichessClient(string apiToken)
    {
        _httpClient = new HttpClient(new HttpClientHandler());
        _httpClient.BaseAddress = new Uri("https://lichess.org/");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
    }

    public async Task<EventStream> OpenEventStreamAsync()
    {
        Logger.Debug("Opening event stream.");

        var stream = await _httpClient.GetStreamAsync(new Uri("api/stream/event", UriKind.Relative));

        Logger.Debug("Event stream opened.");

        return new EventStream(stream);
    }

    public async Task AcceptChallenge(string challengeId)
    {
        Logger.Debug("Accepting challenge: {0}.", challengeId);

        var response = await _httpClient.PostAsync(new Uri($"api/challenge/{challengeId}/accept", UriKind.Relative), new StringContent(string.Empty));
        response.EnsureSuccessStatusCode();

        Logger.Debug("Challenge accepted: {0}.", challengeId);
    }

    public async Task WriteChatMessage(string gameId, string message)
    {
        Logger.Debug("WriteChatMessage: gameId: {0}, message: {1}.", gameId, message);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("room", "player"),
            new KeyValuePair<string, string>("text", message)
        });
        var response = await _httpClient.PostAsync(new Uri($"api/bot/game/{gameId}/chat", UriKind.Relative), content);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}