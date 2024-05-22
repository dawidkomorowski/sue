namespace Sue.Lichess.Api.LichessEvents;

internal sealed class UnknownEvent : LichessEvent
{
    public UnknownEvent(string eventData)
    {
        EventData = eventData;
    }

    public string EventData { get; }

    public override string ToString() => $"{nameof(UnknownEvent)}: {EventData}";
}