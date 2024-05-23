using System;
using Sue.Engine.Model.Chessboard;

namespace Sue.Engine.NewModel;

internal struct Move : IEquatable<Move>
{
    public Move(Position from, Position to, Promotion promotion = Promotion.None)
    {
        From = from;
        To = to;
        Promotion = promotion;
    }

    public Position From { get; }
    public Position To { get; }
    public Promotion Promotion { get; }

    public override string ToString() => $"{nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(Promotion)}: {Promotion}";

    public bool Equals(Move other) => From.Equals(other.From) && To.Equals(other.To) && Promotion == other.Promotion;
    public override bool Equals(object? obj) => obj is Move other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(From, To, (int)Promotion);
    public static bool operator ==(Move left, Move right) => left.Equals(right);
    public static bool operator !=(Move left, Move right) => !left.Equals(right);
}

internal struct Position : IEquatable<Position>
{
    public Position(File file, Rank rank)
    {
        File = file;
        Rank = rank;
    }

    public File File { get; }
    public Rank Rank { get; }

    public override string ToString() => $"{nameof(File)}: {File}, {nameof(Rank)}: {Rank}";

    public bool Equals(Position other) => File == other.File && Rank == other.Rank;
    public override bool Equals(object? obj) => obj is Position other && Equals(other);
    public override int GetHashCode() => HashCode.Combine((int)File, (int)Rank);
    public static bool operator ==(Position left, Position right) => left.Equals(right);
    public static bool operator !=(Position left, Position right) => !left.Equals(right);
}

internal enum Promotion
{
    None,
    Queen,
    Rook,
    Bishop,
    Knight
}