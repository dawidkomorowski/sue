using System;

namespace Sue.Engine.Search;

internal readonly struct Score : IEquatable<Score>, IComparable<Score>
{
    public static Score Min { get; } = CreateEval(int.MinValue);
    public static Score Max { get; } = CreateEval(int.MaxValue);
    public static Score CreateEval(int eval) => new(eval, 0, false);
    public static Score CreateMate(int mateIn) => new(0, mateIn, true);

    private Score(int eval, int mateIn, bool isMate)
    {
        Eval = eval;
        MateIn = mateIn;
        IsMate = isMate;
    }

    public int Eval { get; }
    public int MateIn { get; }
    public bool IsMate { get; }

    public override string ToString() => $"{nameof(Eval)}: {Eval}, {nameof(MateIn)}: {MateIn}, {nameof(IsMate)}: {IsMate}";

    public bool Equals(Score other) => Eval == other.Eval && MateIn == other.MateIn && IsMate == other.IsMate;

    public override bool Equals(object? obj) => obj is Score other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Eval, MateIn, IsMate);
    public static bool operator ==(Score left, Score right) => left.Equals(right);
    public static bool operator !=(Score left, Score right) => !left.Equals(right);

    public int CompareTo(Score other)
    {
        if (IsMate && other.IsMate)
        {
            return -MateIn.CompareTo(other.MateIn);
        }

        if (IsMate && !other.IsMate)
        {
            return 1;
        }

        if (!IsMate && other.IsMate)
        {
            return -1;
        }

        return Eval.CompareTo(other.Eval);
    }

    public static bool operator <(Score left, Score right) => left.CompareTo(right) < 0;
    public static bool operator >(Score left, Score right) => left.CompareTo(right) > 0;
    public static bool operator <=(Score left, Score right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Score left, Score right) => left.CompareTo(right) >= 0;
}