using System;
using System.Text.Json;

namespace Sue.Lichess.Api.LichessEvents;

internal sealed class GameFinishEvent : LichessEvent
{
    public GameFinishEvent(JsonDocument eventJson)
    {
        GameId = eventJson.RootElement.GetProperty("game").GetProperty("id").GetString() ?? throw new ArgumentException("Missing 'game.id'.");
    }

    public string GameId { get; }

    public override string ToString() => $"{nameof(GameFinishEvent)}: {JsonSerializer.Serialize(this)}";
}