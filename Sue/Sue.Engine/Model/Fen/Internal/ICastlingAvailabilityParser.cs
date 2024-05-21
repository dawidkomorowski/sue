namespace Sue.Common.Model.Fen.Internal
{
    public interface ICastlingAvailabilityParser
    {
        CastlingAvailability Parse(string castlingAvailabilityString);
    }
}