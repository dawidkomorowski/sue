namespace Sue.Lichess.Api.GameEvents;

internal sealed class PingGameEvent : GameEvent
{
    public override string ToString() => nameof(PingGameEvent);
}