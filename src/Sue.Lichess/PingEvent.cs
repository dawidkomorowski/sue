namespace Sue.Lichess;

public sealed class PingEvent : LichessEvent
{
    public override string ToString() => nameof(PingEvent);
}