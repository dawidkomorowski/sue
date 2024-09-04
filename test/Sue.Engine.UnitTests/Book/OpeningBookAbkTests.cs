using System;
using NUnit.Framework;
using Sue.Engine.Book;
using Sue.Engine.Model;

namespace Sue.Engine.UnitTests.Book;

[TestFixture]
public class OpeningBookAbkTests
{
    [Test]
    public void TODO()
    {
        var openingBookAbk = new OpeningBookAbk();
        var moves = openingBookAbk.GetNextMoves(Move.ParseUciMoves(""));

        foreach (var move in moves)
        {
            Console.WriteLine(move);
        }

        Console.WriteLine("---");

        moves = openingBookAbk.GetNextMoves(Move.ParseUciMoves("e2e4"));

        foreach (var move in moves)
        {
            Console.WriteLine(move);
        }

        Console.WriteLine("---");

        moves = openingBookAbk.GetNextMoves(Move.ParseUciMoves("e2e4 c7c6"));

        foreach (var move in moves)
        {
            Console.WriteLine(move);
        }

        Console.WriteLine("---");

        moves = openingBookAbk.GetNextMoves(Move.ParseUciMoves("e2e4 c7c6 b1c3"));

        foreach (var move in moves)
        {
            Console.WriteLine(move);
        }

        Console.WriteLine("---");

        moves = openingBookAbk.GetNextMoves(Move.ParseUciMoves("e2e4 c7c6 b1c3 d7d5"));

        foreach (var move in moves)
        {
            Console.WriteLine(move);
        }

        Console.WriteLine("---");

        moves = openingBookAbk.GetNextMoves(Move.ParseUciMoves("e2e4 c7c6 b1c3 d2d3"));

        foreach (var move in moves)
        {
            Console.WriteLine(move);
        }
    }
}