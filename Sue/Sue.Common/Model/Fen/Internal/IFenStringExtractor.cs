namespace Sue.Common.Model.Fen.Internal
{
    internal interface IFenStringExtractor
    {
        ExtractedFenString Extract(string fenString);
    }
}