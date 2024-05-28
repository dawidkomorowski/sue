using System;
using System.Collections.Generic;

namespace Sue.Engine.Model;

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

internal static class RankExtensions
{
    public static IReadOnlyList<Rank> Ranks() => [Rank.One, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight];

    public static char ToChar(this Rank rank)
    {
        return rank switch
        {
            Rank.One => '1',
            Rank.Two => '2',
            Rank.Three => '3',
            Rank.Four => '4',
            Rank.Five => '5',
            Rank.Six => '6',
            Rank.Seven => '7',
            Rank.Eight => '8',
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
        };
    }

    public static int Index(this Rank rank)
    {
        return rank switch
        {
            Rank.One => 0,
            Rank.Two => 1,
            Rank.Three => 2,
            Rank.Four => 3,
            Rank.Five => 4,
            Rank.Six => 5,
            Rank.Seven => 6,
            Rank.Eight => 7,
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
        };
    }

    public static Rank Add(this Rank rank, int offset)
    {
        var rankIndex = rank.Index();
        var newRankIndex = rankIndex + offset;
        return newRankIndex.ToRank();
    }
}

internal static class CharExtensionsForRank
{
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

internal static class IntExtensionsForRank
{
    public static Rank ToRank(this int index)
    {
        return index switch
        {
            0 => Rank.One,
            1 => Rank.Two,
            2 => Rank.Three,
            3 => Rank.Four,
            4 => Rank.Five,
            5 => Rank.Six,
            6 => Rank.Seven,
            7 => Rank.Eight,
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
    }
}