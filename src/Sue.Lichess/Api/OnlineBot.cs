using System;
using System.Text.Json;

namespace Sue.Lichess.Api;

internal sealed class OnlineBot
{
    public OnlineBot(JsonDocument json)
    {
        Id = json.RootElement.GetProperty("id").GetString() ?? throw new ArgumentException("Missing 'id'.");

        var perfsJsonElement = json.RootElement.GetProperty("perfs");
        if (perfsJsonElement.TryGetProperty("blitz", out var blitzJsonElement))
        {
            BlitzRating = blitzJsonElement.GetProperty("rating").GetInt32();
        }
    }

    public string Id { get; }
    public int? BlitzRating { get; }

    public override string ToString() => $"{nameof(OnlineBot)}: {JsonSerializer.Serialize(this)}";
}