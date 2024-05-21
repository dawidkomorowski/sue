namespace Sue.Engine.Model.Fen.Internal
{
    public interface IFenStringExtractor
    {
        ExtractedFenString Extract(string fenString);
    }
}