using System;
using System.Text.Json;

namespace Sue.Lichess.Api.GameEvents;

internal sealed class GameFullEvent : GameEvent
{
    public GameFullEvent(JsonDocument eventJson)
    {
        WhiteId = eventJson.RootElement.GetProperty("white").GetProperty("id").GetString() ?? throw new ArgumentException("Missing 'white.id'.");
        BlackId = eventJson.RootElement.GetProperty("black").GetProperty("id").GetString() ?? throw new ArgumentException("Missing 'black.id'.");
        InitialFen = eventJson.RootElement.GetProperty("initialFen").GetString() ?? throw new ArgumentException("Missing 'initialFen'.");
        HasClock = eventJson.RootElement.TryGetProperty("clock", out _);
        Moves = eventJson.RootElement.GetProperty("state").GetProperty("moves").GetString() ?? throw new ArgumentException("Missing 'state.moves'.");
        WhiteTimeMs = eventJson.RootElement.GetProperty("state").GetProperty("wtime").GetInt32();
        BlackTimeMs = eventJson.RootElement.GetProperty("state").GetProperty("btime").GetInt32();
    }

    public string WhiteId { get; }
    public string BlackId { get; }
    public string InitialFen { get; }
    public bool HasClock { get; }
    public string Moves { get; }
    public int WhiteTimeMs { get; }
    public int BlackTimeMs { get; }

    public override string ToString() => $"{nameof(GameFullEvent)}: {JsonSerializer.Serialize(this)}";
}