using System;
using System.Text.Json;

namespace Sue.Lichess.Api.GameEvents;

internal sealed class GameStateEvent : GameEvent
{
    public GameStateEvent(JsonDocument eventJson)
    {
        Moves = eventJson.RootElement.GetProperty("moves").GetString() ?? throw new ArgumentException("Missing 'moves'.");
        WhiteTimeMs = eventJson.RootElement.GetProperty("wtime").GetInt32();
        BlackTimeMs = eventJson.RootElement.GetProperty("btime").GetInt32();
    }

    public string Moves { get; }
    public int WhiteTimeMs { get; }
    public int BlackTimeMs { get; }

    public override string ToString() => $"{nameof(GameStateEvent)}: {JsonSerializer.Serialize(this)}";
}