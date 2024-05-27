using System;

namespace Sue.Engine.OldModel.Chessboard
{
    public static class CharExtensions
    {
        public static File ToFile(this char c)
        {
            return c switch
            {
                'a' => File.A,
                'b' => File.B,
                'c' => File.C,
                'd' => File.D,
                'e' => File.E,
                'f' => File.F,
                'g' => File.G,
                'h' => File.H,
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };
        }

        public static Rank ToRank(this char c)
        {
            return c switch
            {
                '1' => Rank.One,
                '2' => Rank.Two,
                '3' => Rank.Three,
                '4' => Rank.Four,
                '5' => Rank.Five,
                '6' => Rank.Six,
                '7' => Rank.Seven,
                '8' => Rank.Eight,
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };
        }
    }
}