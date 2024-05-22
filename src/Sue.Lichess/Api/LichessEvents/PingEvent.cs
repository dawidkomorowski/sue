namespace Sue.Lichess.Api.LichessEvents;

internal sealed class PingEvent : LichessEvent
{
    public override string ToString() => nameof(PingEvent);
}