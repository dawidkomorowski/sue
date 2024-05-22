using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using NLog;

namespace Sue.Lichess.Api;

internal sealed class LichessClient : IDisposable
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

    public async Task<GameStream> OpenGameStreamAsync(string gameId)
    {
        Logger.Debug("Opening game stream for game: {0}.", gameId);

        var stream = await _httpClient.GetStreamAsync(new Uri($"api/bot/game/stream/{gameId}", UriKind.Relative));

        Logger.Debug("Game stream open for game: {0}.", gameId);

        return new GameStream(stream);
    }

    public async Task AcceptChallengeAsync(string challengeId)
    {
        Logger.Debug("Accepting challenge: {0}.", challengeId);

        var response = await _httpClient.PostAsync(new Uri($"api/challenge/{challengeId}/accept", UriKind.Relative), new StringContent(string.Empty));
        Logger.Debug("Request response: {0}", await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();

        Logger.Debug("Challenge accepted: {0}.", challengeId);
    }

    public async Task WriteChatMessageAsync(string gameId, string message)
    {
        Logger.Debug("WriteChatMessageAsync: gameId: {0}, message: {1}.", gameId, message);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("room", "player"),
            new KeyValuePair<string, string>("text", message)
        });
        var response = await _httpClient.PostAsync(new Uri($"api/bot/game/{gameId}/chat", UriKind.Relative), content);
        Logger.Debug("Request response: {0}", await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();
    }

    public async Task MakeMoveAsync(string gameId, string uciMove)
    {
        Logger.Debug("MakeMoveAsync: gameId: {0}, uciMove: {1}.", gameId, uciMove);

        var response = await _httpClient.PostAsync(new Uri($"api/bot/game/{gameId}/move/{uciMove}", UriKind.Relative), new StringContent(string.Empty));
        Logger.Debug("Request response: {0}", await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}