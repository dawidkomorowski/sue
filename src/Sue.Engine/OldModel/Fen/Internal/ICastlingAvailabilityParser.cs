namespace Sue.Engine.OldModel.Fen.Internal
{
    public interface ICastlingAvailabilityParser
    {
        CastlingAvailability Parse(string castlingAvailabilityString);
    }
}