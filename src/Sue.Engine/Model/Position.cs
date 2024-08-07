﻿using System;

namespace Sue.Engine.Model;

public readonly struct Position : IEquatable<Position>
{
    public Position(File file, Rank rank)
    {
        File = file;
        Rank = rank;
    }

    public File File { get; }
    public Rank Rank { get; }


    private static readonly Position[] AllArray;
    public static ReadOnlySpan<Position> All => AllArray;

    static Position()
    {
        var index = 0;
        AllArray = new Position[64];

        foreach (var file in FileExtensions.Files())
        {
            foreach (var rank in RankExtensions.Ranks())
            {
                AllArray[index++] = new Position(file, rank);
            }
        }
    }

    internal Position MoveUp() => new(File, Rank.Add(1));
    internal Position MoveDown() => new(File, Rank.Add(-1));
    internal Position MoveRight() => new(File.Add(1), Rank);
    internal Position MoveLeft() => new(File.Add(-1), Rank);
    internal Position MoveBy(int right, int up) => new(File.Add(right), Rank.Add(up));

    public override string ToString() => $"{nameof(File)}: {File}, {nameof(Rank)}: {Rank}";

    public bool Equals(Position other) => File == other.File && Rank == other.Rank;
    public override bool Equals(object? obj) => obj is Position other && Equals(other);
    public override int GetHashCode() => HashCode.Combine((int)File, (int)Rank);
    public static bool operator ==(Position left, Position right) => left.Equals(right);
    public static bool operator !=(Position left, Position right) => !left.Equals(right);
}