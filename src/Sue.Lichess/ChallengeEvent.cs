using System;
using System.Text.Json;

namespace Sue.Lichess;

public sealed class ChallengeEvent : LichessEvent
{
    public ChallengeEvent(JsonDocument eventJson)
    {
        ChallengeId = eventJson.RootElement.GetProperty("challenge").GetProperty("id").GetString() ?? throw new ArgumentException("Missing 'challenge.id'.");
        ChallengerId = eventJson.RootElement.GetProperty("challenge").GetProperty("challenger").GetProperty("id").GetString() ??
                       throw new ArgumentException("Missing 'challenge.challenger.id'.");
        DestinationUserId = eventJson.RootElement.GetProperty("challenge").GetProperty("destUser").GetProperty("id").GetString() ??
                            throw new ArgumentException("Missing 'challenge.destUser.id'.");
    }

    public string ChallengeId { get; }
    public string ChallengerId { get; }
    public string DestinationUserId { get; }

    public override string ToString() => $"{nameof(ChallengeEvent)}: {JsonSerializer.Serialize(this)}";
}

public sealed class GameStartEvent : LichessEvent
{
    public GameStartEvent(JsonDocument eventJson)
    {
        GameId = eventJson.RootElement.GetProperty("game").GetProperty("id").GetString() ?? throw new ArgumentException("Missing 'game.id'.");
    }

    public string GameId { get; }

    public override string ToString() => $"{nameof(GameStartEvent)}: {JsonSerializer.Serialize(this)}";
}