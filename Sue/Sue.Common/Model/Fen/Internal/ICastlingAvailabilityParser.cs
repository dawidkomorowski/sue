namespace Sue.Common.Model.Fen.Internal
{
    internal interface ICastlingAvailabilityParser
    {
        CastlingAvailability Parse(string castlingAvailabilityString);
    }
}