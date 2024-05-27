using System;

namespace Sue.Engine.OldModel.Chessboard
{
    public static class IntExtensions
    {
        public static File ToFile(this int index)
        {
            switch (index)
            {
                case 0:
                    return File.A;
                case 1:
                    return File.B;
                case 2:
                    return File.C;
                case 3:
                    return File.D;
                case 4:
                    return File.E;
                case 5:
                    return File.F;
                case 6:
                    return File.G;
                case 7:
                    return File.H;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index), index, null);
            }
        }

        public static Rank ToRank(this int index)
        {
            switch (index)
            {
                case 0:
                    return Rank.One;
                case 1:
                    return Rank.Two;
                case 2:
                    return Rank.Three;
                case 3:
                    return Rank.Four;
                case 4:
                    return Rank.Five;
                case 5:
                    return Rank.Six;
                case 6:
                    return Rank.Seven;
                case 7:
                    return Rank.Eight;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index), index, null);
            }
        }
    }
}