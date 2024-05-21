namespace Sue.Common.Model.Fen.Internal
{
    public interface IFenStringExtractor
    {
        ExtractedFenString Extract(string fenString);
    }
}