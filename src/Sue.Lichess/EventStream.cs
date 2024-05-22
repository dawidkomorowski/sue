using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using NLog;

namespace Sue.Lichess;

public sealed class EventStream : IDisposable
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly Stream _stream;
    private readonly StreamReader _reader;

    internal EventStream(Stream stream)
    {
        _stream = stream;
        _reader = new StreamReader(stream);
    }

    public bool EndOfStream => _reader.EndOfStream;

    public async Task<LichessEvent> ReadEventAsync()
    {
        var eventData = await _reader.ReadLineAsync();
        Logger.Debug("Event data received: {0}", eventData);

        if (string.IsNullOrWhiteSpace(eventData))
        {
            return new PingEvent();
        }

        var eventJson = JsonDocument.Parse(eventData);
        if (!eventJson.RootElement.TryGetProperty("type", out var typeProperty))
        {
            return new UnknownEvent(eventData);
        }

        var type = typeProperty.GetString();

        return type switch
        {
            "challenge" => new ChallengeEvent(eventJson),
            "gameStart" => new GameStartEvent(eventJson),
            _ => new UnknownEvent(eventData)
        };
    }

    public void Dispose()
    {
        _reader.Dispose();
        _stream.Dispose();
    }
}