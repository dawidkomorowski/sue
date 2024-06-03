using System;
using System.Collections.Generic;
using System.Linq;

namespace Sue.Engine.Model;

internal readonly struct Move : IEquatable<Move>
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

    public static Move ParseUciMove(string uciMove)
    {
        uciMove = uciMove.Trim().ToLowerInvariant();
        if (uciMove.Length != 4 && uciMove.Length != 5)
        {
            throw new ArgumentException($"Invalid UCI move: {uciMove}");
        }

        const string validFiles = "abcdefgh";
        const string validRanks = "12345678";

        if (!validFiles.Contains(uciMove[0]) || !validFiles.Contains(uciMove[2]))
        {
            throw new ArgumentException($"Invalid UCI move: {uciMove}");
        }

        if (!validRanks.Contains(uciMove[1]) || !validRanks.Contains(uciMove[3]))
        {
            throw new ArgumentException($"Invalid UCI move: {uciMove}");
        }

        var promotion = Promotion.None;

        if (uciMove.Length == 5)
        {
            promotion = uciMove[4] switch
            {
                'q' => Promotion.Queen,
                'r' => Promotion.Rook,
                'b' => Promotion.Bishop,
                'n' => Promotion.Knight,
                _ => throw new ArgumentException($"Invalid UCI move: {uciMove}")
            };
        }

        var from = new Position(uciMove[0].ToFile(), uciMove[1].ToRank());
        var to = new Position(uciMove[2].ToFile(), uciMove[3].ToRank());
        return new Move(from, to, promotion);
    }

    public static IReadOnlyList<Move> ParseUciMoves(string uciMoves)
    {
        uciMoves = uciMoves.Trim().ToLowerInvariant();
        var splitUciMoves = uciMoves.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return splitUciMoves.Select(ParseUciMove).ToList().AsReadOnly();
    }

    public string ToUci()
    {
        var uciMove = $"{From.File.ToChar()}{From.Rank.ToChar()}{To.File.ToChar()}{To.Rank.ToChar()}";

        if (Promotion is not Promotion.None)
        {
            uciMove += Promotion switch
            {
                Promotion.Queen => 'q',
                Promotion.Rook => 'r',
                Promotion.Bishop => 'b',
                Promotion.Knight => 'n',
                _ => throw new InvalidOperationException($"Unexpected promotion value: {Promotion}")
            };
        }

        return uciMove;
    }

    public override string ToString() => ToUci();

    public bool Equals(Move other) => From.Equals(other.From) && To.Equals(other.To) && Promotion == other.Promotion;
    public override bool Equals(object? obj) => obj is Move other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(From, To, (int)Promotion);
    public static bool operator ==(Move left, Move right) => left.Equals(right);
    public static bool operator !=(Move left, Move right) => !left.Equals(right);
}