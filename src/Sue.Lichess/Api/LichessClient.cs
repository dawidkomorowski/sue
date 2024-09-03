using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
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

    public async Task<string> GetAccountId()
    {
        Logger.Debug("GetAccountId.");

        var response = await _httpClient.GetAsync(new Uri("api/account", UriKind.Relative));
        var stringContent = await response.Content.ReadAsStringAsync();
        Logger.Debug("Request response: {0}", stringContent);
        response.EnsureSuccessStatusCode();

        using var jsonDocument = JsonDocument.Parse(stringContent);
        return jsonDocument.RootElement.GetProperty("id").GetString() ?? throw new InvalidOperationException("Missing 'id'.");
    }

    public async Task<int> GetBlitzRating()
    {
        Logger.Debug("GetBlitzRating.");

        var response = await _httpClient.GetAsync(new Uri("api/account", UriKind.Relative));
        var stringContent = await response.Content.ReadAsStringAsync();
        Logger.Debug("Request response: {0}", stringContent);
        response.EnsureSuccessStatusCode();

        using var jsonDocument = JsonDocument.Parse(stringContent);
        return jsonDocument.RootElement.GetProperty("perfs").GetProperty("blitz").GetProperty("rating").GetInt32();
    }

    public async Task<IReadOnlyList<OnlineBot>> GetOnlineBots()
    {
        Logger.Debug("GetBotsOnline.");

        var response = await _httpClient.GetAsync(new Uri("api/bot/online", UriKind.Relative));
        var stringContent = await response.Content.ReadAsStringAsync();
        Logger.Debug("Request response: {0}", stringContent);
        response.EnsureSuccessStatusCode();

        var onlineBots = new List<OnlineBot>();
        var lines = stringContent.Split('\n');
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            using var json = JsonDocument.Parse(line);
            var onlineBot = new OnlineBot(json);
            onlineBots.Add(onlineBot);
        }

        return onlineBots.AsReadOnly();
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

    public async Task CreateChallengeAsync(string username, bool rated)
    {
        Logger.Debug("Sending challenge to player: {0}.", username);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("rated", rated.ToString().ToLowerInvariant()),
            new KeyValuePair<string, string>("clock.limit", "300"),
            new KeyValuePair<string, string>("clock.increment", "0"),
            new KeyValuePair<string, string>("color", "random"),
            new KeyValuePair<string, string>("variant", "standard"),
        });
        var response = await _httpClient.PostAsync(new Uri($"api/challenge/{username}", UriKind.Relative), content);
        Logger.Debug("Request response: {0}", await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();

        Logger.Debug("Challenge sent to player: {0}.", username);
    }

    public async Task AcceptChallengeAsync(string challengeId)
    {
        Logger.Debug("Accepting challenge: {0}.", challengeId);

        var response = await _httpClient.PostAsync(new Uri($"api/challenge/{challengeId}/accept", UriKind.Relative), new StringContent(string.Empty));
        Logger.Debug("Request response: {0}", await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();

        Logger.Debug("Challenge accepted: {0}.", challengeId);
    }

    public async Task DeclineChallengeAsync(string challengeId)
    {
        Logger.Debug("Declining challenge: {0}.", challengeId);

        var response = await _httpClient.PostAsync(new Uri($"api/challenge/{challengeId}/decline", UriKind.Relative), new StringContent(string.Empty));
        Logger.Debug("Request response: {0}", await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();

        Logger.Debug("Challenge declined: {0}.", challengeId);
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

    public async Task ResignGameAsync(string gameId)
    {
        Logger.Debug("ResignGameAsync: gameId: {0}.", gameId);

        var response = await _httpClient.PostAsync(new Uri($"api/bot/game/{gameId}/resign", UriKind.Relative), new StringContent(string.Empty));
        Logger.Debug("Request response: {0}", await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}