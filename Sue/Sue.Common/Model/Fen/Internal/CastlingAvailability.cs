namespace Sue.Common.Model.Fen.Internal
{
    internal struct CastlingAvailability
    {
        public CastlingAvailability(bool whiteKingsideCastlingAvailable, bool whiteQueenssideCastlingAvailable, bool blackKingsideCastlingAvailable, bool blackQueensideCastlingAvailable)
        {
            WhiteKingsideCastlingAvailable = whiteKingsideCastlingAvailable;
            WhiteQueenssideCastlingAvailable = whiteQueenssideCastlingAvailable;
            BlackKingsideCastlingAvailable = blackKingsideCastlingAvailable;
            BlackQueensideCastlingAvailable = blackQueensideCastlingAvailable;
        }

        public bool WhiteKingsideCastlingAvailable { get; }
        public bool WhiteQueenssideCastlingAvailable { get; }
        public bool BlackKingsideCastlingAvailable { get; }
        public bool BlackQueensideCastlingAvailable { get; }
    }
}