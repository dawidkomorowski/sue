namespace Sue.Engine.Model.Fen.Internal
{
    public interface ICastlingAvailabilityParser
    {
        CastlingAvailability Parse(string castlingAvailabilityString);
    }
}