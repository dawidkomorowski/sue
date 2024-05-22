using System;
using System.Text.Json;

namespace Sue.Lichess.Api.LichessEvents;

internal sealed class ChallengeEvent : LichessEvent
{
    public ChallengeEvent(JsonDocument eventJson)
    {
        ChallengeId = eventJson.RootElement.GetProperty("challenge").GetProperty("id").GetString() ?? throw new ArgumentException("Missing 'challenge.id'.");
        ChallengerId = eventJson.RootElement.GetProperty("challenge").GetProperty("challenger").GetProperty("id").GetString() ??
                       throw new ArgumentException("Missing 'challenge.challenger.id'.");
        DestinationUserId = eventJson.RootElement.GetProperty("challenge").GetProperty("destUser").GetProperty("id").GetString() ??
                            throw new ArgumentException("Missing 'challenge.destUser.id'.");
        IsRated = eventJson.RootElement.GetProperty("challenge").GetProperty("rated").GetBoolean();
    }

    public string ChallengeId { get; }
    public string ChallengerId { get; }
    public string DestinationUserId { get; }
    public bool IsRated { get; }

    public override string ToString() => $"{nameof(ChallengeEvent)}: {JsonSerializer.Serialize(this)}";
}