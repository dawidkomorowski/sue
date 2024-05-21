using System;
using System.Collections.Generic;

namespace Sue.Engine.Model.Chessboard
{
    public enum Rank
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight
    }

    public static class RankExtensions
    {
        public static int Index(this Rank rank)
        {
            switch (rank)
            {
                case Rank.One:
                    return 0;
                case Rank.Two:
                    return 1;
                case Rank.Three:
                    return 2;
                case Rank.Four:
                    return 3;
                case Rank.Five:
                    return 4;
                case Rank.Six:
                    return 5;
                case Rank.Seven:
                    return 6;
                case Rank.Eight:
                    return 7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rank), rank, null);
            }
        }

        public static IEnumerable<Rank> Enumerable()
        {
            return new[] {Rank.One, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight};
        }

        public static Rank Add(this Rank rank, int offset)
        {
            var rankIndex = rank.Index();
            var newRankIndex = rankIndex + offset;
            return newRankIndex.ToRank();
        }
    }
}