using System;
using System.Text.Json;

namespace Sue.Lichess.Api.GameEvents;

internal sealed class GameStateEvent : GameEvent
{
    public GameStateEvent(JsonDocument eventJson)
    {
        Moves = eventJson.RootElement.GetProperty("moves").GetString() ?? throw new ArgumentException("Missing 'moves'.");
    }

    public string Moves { get; }

    public override string ToString() => $"{nameof(GameStateEvent)}: {JsonSerializer.Serialize(this)}";
}