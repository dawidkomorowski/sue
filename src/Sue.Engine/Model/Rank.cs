using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Sue.Engine.Model;

public enum Rank
{
    One = 0,
    Two = 1,
    Three = 2,
    Four = 3,
    Five = 4,
    Six = 5,
    Seven = 6,
    Eight = 7
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Index(this Rank rank)
    {
        Debug.Assert(rank is >= Rank.One and <= Rank.Eight, "rank is >= Rank.One and <= Rank.Eight");
        return (int)rank;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rank Add(this Rank rank, int offset)
    {
        return (rank.Index() + offset).ToRank();
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rank ToRank(this int index)
    {
        Debug.Assert(index is >= 0 and <= 7, "index is >= 0 and <= 7");
        return (Rank)index;
    }
}