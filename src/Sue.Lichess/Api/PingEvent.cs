namespace Sue.Lichess.Api;

public sealed class PingEvent : LichessEvent
{
    public override string ToString() => nameof(PingEvent);
}