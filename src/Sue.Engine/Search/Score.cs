using System;

namespace Sue.Engine.Search;

internal readonly struct Score : IEquatable<Score>, IComparable<Score>
{
    private const int MaxMate = 1000;
    private const int MateOffset = 10000;

    public static Score Min { get; } = new(int.MinValue);
    public static Score Max { get; } = new(int.MaxValue);
    public static Score Zero { get; } = new(0, 0);
    public static Score CreateMate(int mateIn) => new(mateIn, 0);
    public static Score CreateEval(int eval) => new(0, eval);

    private readonly int _mateScore;

    private Score(int mateIn, int eval)
    {
        if (Math.Abs(mateIn) > MaxMate)
        {
            throw new ArgumentException("Mate must be in range -1000 to 1000.");
        }

        _mateScore = MateOffset * Math.Sign(mateIn) - mateIn;
        Eval = eval;
    }

    private Score(int mateScore)
    {
        _mateScore = mateScore;
        Eval = 0;
    }

    public bool IsMate => MateIn != 0;
    public int MateIn => MateOffset * Math.Sign(_mateScore) - _mateScore;
    public int Eval { get; }

    public override string ToString() => $"{nameof(IsMate)}: {IsMate}, {nameof(MateIn)}: {MateIn}, {nameof(Eval)}: {Eval}";

    public bool Equals(Score other) => _mateScore == other._mateScore && Eval == other.Eval;

    public override bool Equals(object? obj) => obj is Score other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(_mateScore, Eval);
    public static bool operator ==(Score left, Score right) => left.Equals(right);
    public static bool operator !=(Score left, Score right) => !left.Equals(right);

    public int CompareTo(Score other)
    {
        var mateInComparison = _mateScore.CompareTo(other._mateScore);
        return mateInComparison != 0 ? mateInComparison : Eval.CompareTo(other.Eval);
    }

    public static bool operator <(Score left, Score right) => left.CompareTo(right) < 0;
    public static bool operator >(Score left, Score right) => left.CompareTo(right) > 0;
    public static bool operator <=(Score left, Score right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Score left, Score right) => left.CompareTo(right) >= 0;
}