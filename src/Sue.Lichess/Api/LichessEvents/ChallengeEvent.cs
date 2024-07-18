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
        VariantKey = eventJson.RootElement.GetProperty("challenge").GetProperty("variant").GetProperty("key").GetString() ??
                     throw new ArgumentException("Missing 'challenge.variant.key'.");
    }

    public string ChallengeId { get; }
    public string ChallengerId { get; }
    public string DestinationUserId { get; }
    public bool IsRated { get; }
    public string VariantKey { get; }

    public override string ToString() => $"{nameof(ChallengeEvent)}: {JsonSerializer.Serialize(this)}";

    public static class Variant
    {
        public const string Standard = "standard";
        public const string FromPosition = "fromPosition";
    }
}