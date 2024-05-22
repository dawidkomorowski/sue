using System;
using System.Text.Json;

namespace Sue.Lichess.Api.LichessEvents;

internal sealed class GameStartEvent : LichessEvent
{
    public GameStartEvent(JsonDocument eventJson)
    {
        GameId = eventJson.RootElement.GetProperty("game").GetProperty("id").GetString() ?? throw new ArgumentException("Missing 'game.id'.");
    }

    public string GameId { get; }

    public override string ToString() => $"{nameof(GameStartEvent)}: {JsonSerializer.Serialize(this)}";
}