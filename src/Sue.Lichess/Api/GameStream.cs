using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using NLog;
using Sue.Lichess.Api.GameEvents;

namespace Sue.Lichess.Api;

internal sealed class GameStream : IDisposable
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly Stream _stream;
    private readonly StreamReader _reader;

    public GameStream(Stream stream)
    {
        _stream = stream;
        _reader = new StreamReader(stream);
    }

    public bool EndOfStream => _reader.EndOfStream;

    public async Task<GameEvent> ReadEventAsync()
    {
        var eventData = await _reader.ReadLineAsync();
        Logger.Debug("Event data received: {0}", eventData);

        if (string.IsNullOrWhiteSpace(eventData))
        {
            return new PingGameEvent();
        }

        var eventJson = JsonDocument.Parse(eventData);
        if (!eventJson.RootElement.TryGetProperty("type", out var typeProperty))
        {
            return new UnknownGameEvent(eventData);
        }

        var type = typeProperty.GetString();

        return type switch
        {
            "gameFull" => new GameFullEvent(eventJson),
            _ => new UnknownGameEvent(eventData)
        };
    }

    public void Dispose()
    {
        _stream.Dispose();
        _reader.Dispose();
    }
}