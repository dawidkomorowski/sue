namespace Sue.Lichess.Api.GameEvents;

internal sealed class UnknownGameEvent : GameEvent
{
    public UnknownGameEvent(string eventData)
    {
        EventData = eventData;
    }

    public string EventData { get; }

    public override string ToString() => $"{nameof(UnknownGameEvent)}: {EventData}";
}