using System;
using System.Collections.Generic;
using System.IO;
using Sue.Engine.Model;
using File = System.IO.File;

namespace Sue.Engine.Book;

internal readonly struct AbkEntry
{
    public Position From { get; init; }
    public Position To { get; init; }
    public Promotion Promotion { get; init; }
    public int Priority { get; init; }
    public int NumberOfGames { get; init; }
    public int NumberOfWon { get; init; }
    public int NumberOfLost { get; init; }
    public int PlyCount { get; init; }
    public int NextMove { get; init; }
    public int NextSibling { get; init; }

    public bool HasNextMove => NextMove >= 0;
    public bool HasNextSibling => NextSibling >= 0;
    public Move ToMove() => new(From, To, Promotion);

    public override string ToString() =>
        $"{nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(Promotion)}: {Promotion}, {nameof(Priority)}: {Priority}, {nameof(NumberOfGames)}: {NumberOfGames}, {nameof(NumberOfWon)}: {NumberOfWon}, {nameof(NumberOfLost)}: {NumberOfLost}, {nameof(PlyCount)}: {PlyCount}, {nameof(NextMove)}: {NextMove}, {nameof(NextSibling)}: {NextSibling}";
}

internal sealed class OpeningBookAbk
{
    private const int OffsetToFirstEntry = 900;
    private const int BytesOffsetToFirstEntry = OffsetToFirstEntry * 28;
    private readonly List<AbkEntry> _entries = [];

    public const int FirstEntryPtr = OffsetToFirstEntry;

    public OpeningBookAbk()
    {
        var filePath = Path.Combine("Assets", "Perfect_2023", "ABK", "Perfect2023.abk");
        using var file = File.OpenRead(filePath);
        using var reader = new BinaryReader(file);

        var fileLength = reader.BaseStream.Length;
        reader.BaseStream.Position = BytesOffsetToFirstEntry;

        while (reader.BaseStream.Position != fileLength)
        {
            var entry = ReadEntry(reader);
            _entries.Add(entry);
        }
    }

    public Move[] GetNextMoves(IReadOnlyCollection<Move> initialMoves)
    {
        var entry = GetEntry(FirstEntryPtr);

        if (initialMoves.Count == 0)
        {
            return GetAllSiblings(entry);
        }

        foreach (var move in initialMoves)
        {
            while (true)
            {
                if (move == entry.ToMove())
                {
                    if (entry.HasNextMove)
                    {
                        entry = GetEntry(entry.NextMove);
                        break;
                    }

                    return [];
                }

                if (entry.HasNextSibling)
                {
                    entry = GetEntry(entry.NextSibling);
                }
                else
                {
                    return [];
                }
            }
        }

        return GetAllSiblings(entry);
    }

    private AbkEntry GetEntry(int ptr)
    {
        return _entries[ptr - OffsetToFirstEntry];
    }

    private Move[] GetAllSiblings(AbkEntry entry)
    {
        var moves = new List<Move> { entry.ToMove() };

        while (entry.HasNextSibling)
        {
            entry = GetEntry(entry.NextSibling);
            moves.Add(entry.ToMove());
        }

        return moves.ToArray();
    }

    private static AbkEntry ReadEntry(BinaryReader reader)
    {
        return new AbkEntry
        {
            From = ByteToPosition(reader.ReadByte()),
            To = ByteToPosition(reader.ReadByte()),
            Promotion = ByteToPromotion(reader.ReadByte()),
            Priority = reader.ReadByte(),
            NumberOfGames = reader.ReadInt32(),
            NumberOfWon = reader.ReadInt32(),
            NumberOfLost = reader.ReadInt32(),
            PlyCount = reader.ReadInt32(),
            NextMove = reader.ReadInt32(),
            NextSibling = reader.ReadInt32()
        };
    }

    private static Position ByteToPosition(byte b)
    {
        var file = (b % 8).ToFile();
        var rank = (b / 8).ToRank();
        return new Position(file, rank);
    }

    private static Promotion ByteToPromotion(byte b) => b switch
    {
        0 => Promotion.None,
        1 => Promotion.Rook,
        2 => Promotion.Knight,
        4 => Promotion.Queen,
        _ => throw new ArgumentOutOfRangeException(nameof(b), b, null)
    };
}