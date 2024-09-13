using System;
using System.Collections.Generic;
using System.Linq;
using Sue.Engine.Model;

namespace Sue.Engine.Book;

internal static class BookInspector
{
    public static int ComputeMaxDepth(OpeningBookAbk openingBook)
    {
        return ComputeMaxDepth(openingBook, [], 0);
    }

    private static int ComputeMaxDepth(OpeningBookAbk openingBook, IReadOnlyCollection<Move> initialMoves, int depth)
    {
        if (depth == OpeningBookAbk.MaxDepth)
        {
            Console.WriteLine($"Max depth sequence ID: {Guid.NewGuid()}");
            foreach (var initialMove in initialMoves)
            {
                Console.WriteLine(initialMove);
            }

            Console.WriteLine();
        }

        var moves = openingBook.GetNextMoves(initialMoves);

        if (moves.Length == 0)
        {
            return depth;
        }

        var maxDepth = 0;

        foreach (var move in moves)
        {
            maxDepth = Math.Max(maxDepth, ComputeMaxDepth(openingBook, initialMoves.Append(move).ToArray(), depth + 1));
        }

        return maxDepth;
    }
}