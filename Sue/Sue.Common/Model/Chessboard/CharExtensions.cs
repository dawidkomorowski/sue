using System;

namespace Sue.Common.Model.Chessboard
{
    public static class CharExtensions
    {
        public static File ToFile(this char c)
        {
            switch (c)
            {
                case 'a':
                    return File.A;
                case 'b':
                    return File.B;
                case 'c':
                    return File.C;
                case 'd':
                    return File.D;
                case 'e':
                    return File.E;
                case 'f':
                    return File.F;
                case 'g':
                    return File.G;
                case 'h':
                    return File.H;
                default:
                    throw new ArgumentOutOfRangeException(nameof(c), c, null);
            }
        }

        public static Rank ToRank(this char c)
        {
            switch (c)
            {
                case '1':
                    return Rank.One;
                case '2':
                    return Rank.Two;
                case '3':
                    return Rank.Three;
                case '4':
                    return Rank.Four;
                case '5':
                    return Rank.Five;
                case '6':
                    return Rank.Six;
                case '7':
                    return Rank.Seven;
                case '8':
                    return Rank.Eight;
                default:
                    throw new ArgumentOutOfRangeException(nameof(c), c, null);
            }
        }
    }
}