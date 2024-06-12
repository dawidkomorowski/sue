using System;
using System.Text;

namespace Sue.Engine.Model;

public sealed class Fen
{
    private readonly ChessPiece[] _chessboard = new ChessPiece[64];

    public const string Empty = "8/8/8/8/8/8/8/8 w - - 0 1";
    public const string StartPos = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public Color ActiveColor { get; set; }
    public bool WhiteKingSideCastlingAvailable { get; set; }
    public bool WhiteQueenSideCastlingAvailable { get; set; }
    public bool BlackKingSideCastlingAvailable { get; set; }
    public bool BlackQueenSideCastlingAvailable { get; set; }
    public Position? EnPassantTargetPosition { get; set; }
    public int HalfMoveClock { get; set; }
    public int FullMoveNumber { get; set; } = 1;

    public ChessPiece GetChessPiece(Position position)
    {
        return _chessboard[GetIndex(position)];
    }

    public void SetChessPiece(Position position, ChessPiece chessPiece)
    {
        _chessboard[GetIndex(position)] = chessPiece;
    }

    public static Fen FromString(string fenString)
    {
        var fen = new Fen();

        var index = 0;
        index = ParseRankLines(fen, fenString, index);
        index = ParseFenFieldSeparator(fenString, index);
        index = ParseColor(fen, fenString, index);
        index = ParseFenFieldSeparator(fenString, index);
        index = ParseCastlingAvailability(fen, fenString, index);
        index = ParseFenFieldSeparator(fenString, index);
        index = ParseEnPassant(fen, fenString, index);
        index = ParseFenFieldSeparator(fenString, index);
        index = ParseHalfMoveClock(fen, fenString, index);
        index = ParseFenFieldSeparator(fenString, index);
        ParseFullMoveNumber(fen, fenString, index);

        return fen;
    }

    public override string ToString()
    {
        var fenString = new StringBuilder();

        foreach (var rank in new[] { Rank.Eight, Rank.Seven, Rank.Six, Rank.Five, Rank.Four, Rank.Three, Rank.Two, Rank.One })
        {
            var emptyFields = 0;
            foreach (var file in FileExtensions.Files())
            {
                var chessPiece = GetChessPiece(new Position(file, rank));
                if (chessPiece is ChessPiece.None)
                {
                    emptyFields++;
                }
                else
                {
                    if (emptyFields != 0)
                    {
                        fenString.Append(emptyFields);
                        emptyFields = 0;
                    }

                    fenString.Append(chessPiece.ToChar());
                }
            }

            if (emptyFields != 0)
            {
                fenString.Append(emptyFields);
            }

            if (rank is not Rank.One)
            {
                fenString.Append('/');
            }
        }

        fenString.Append(' ');

        switch (ActiveColor)
        {
            case Color.White:
                fenString.Append('w');
                break;
            case Color.Black:
                fenString.Append('b');
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        fenString.Append(' ');

        if (WhiteKingSideCastlingAvailable is false && WhiteQueenSideCastlingAvailable is false && BlackKingSideCastlingAvailable is false &&
            BlackQueenSideCastlingAvailable is false)
        {
            fenString.Append('-');
        }
        else
        {
            if (WhiteKingSideCastlingAvailable)
            {
                fenString.Append('K');
            }

            if (WhiteQueenSideCastlingAvailable)
            {
                fenString.Append('Q');
            }

            if (BlackKingSideCastlingAvailable)
            {
                fenString.Append('k');
            }

            if (BlackQueenSideCastlingAvailable)
            {
                fenString.Append('q');
            }
        }

        fenString.Append(' ');

        if (EnPassantTargetPosition.HasValue)
        {
            fenString.Append(EnPassantTargetPosition.Value.File.ToChar());
            fenString.Append(EnPassantTargetPosition.Value.Rank.ToChar());
        }
        else
        {
            fenString.Append('-');
        }

        fenString.Append(' ');
        fenString.Append(HalfMoveClock);
        fenString.Append(' ');
        fenString.Append(FullMoveNumber);

        return fenString.ToString();
    }

    private static int GetIndex(Position position)
    {
        return position.File.Index() * 8 + position.Rank.Index();
    }

    private static int ParseRankLines(Fen fen, string fenString, int index)
    {
        index = ParseRankLine(fen, fenString, index, Rank.Eight);
        index = ParseRankLineSeparator(fenString, index);
        index = ParseRankLine(fen, fenString, index, Rank.Seven);
        index = ParseRankLineSeparator(fenString, index);
        index = ParseRankLine(fen, fenString, index, Rank.Six);
        index = ParseRankLineSeparator(fenString, index);
        index = ParseRankLine(fen, fenString, index, Rank.Five);
        index = ParseRankLineSeparator(fenString, index);
        index = ParseRankLine(fen, fenString, index, Rank.Four);
        index = ParseRankLineSeparator(fenString, index);
        index = ParseRankLine(fen, fenString, index, Rank.Three);
        index = ParseRankLineSeparator(fenString, index);
        index = ParseRankLine(fen, fenString, index, Rank.Two);
        index = ParseRankLineSeparator(fenString, index);
        index = ParseRankLine(fen, fenString, index, Rank.One);

        return index;
    }

    private static int ParseRankLine(Fen fen, string fenString, int index, Rank rank)
    {
        const string validRankLineCharacters = "12345678KQRBNPkqrbnp";
        var fileIndex = 0;

        while (fileIndex < 8)
        {
            if (fenString.Length <= index || !validRankLineCharacters.Contains(fenString[index]))
            {
                throw CreateParsingError(fenString, index);
            }

            if (char.IsAsciiDigit(fenString[index]))
            {
                var digit = int.Parse(fenString.AsSpan(index, 1));
                var allowed = 8 - fileIndex;
                if (digit > allowed)
                {
                    throw CreateParsingError(fenString, index);
                }

                fileIndex += digit;
                index++;
            }
            else
            {
                fen.SetChessPiece(new Position(fileIndex.ToFile(), rank), fenString[index].ToChessPiece());
                fileIndex++;
                index++;
            }
        }

        return index;
    }

    private static int ParseRankLineSeparator(string fenString, int index)
    {
        if (fenString.Length <= index || fenString[index] != '/')
        {
            throw CreateParsingError(fenString, index);
        }

        return ++index;
    }

    private static int ParseFenFieldSeparator(string fenString, int index)
    {
        if (fenString.Length <= index || fenString[index] != ' ')
        {
            throw CreateParsingError(fenString, index);
        }

        return ++index;
    }

    private static int ParseColor(Fen fen, string fenString, int index)
    {
        fen.ActiveColor = fenString[index] switch
        {
            'w' => Color.White,
            'b' => Color.Black,
            _ => throw CreateParsingError(fenString, index)
        };

        return ++index;
    }

    private static int ParseCastlingAvailability(Fen fen, string fenString, int index)
    {
        const string validCastlingCharacters = "KQkq";

        fen.WhiteKingSideCastlingAvailable = false;
        fen.WhiteQueenSideCastlingAvailable = false;
        fen.BlackKingSideCastlingAvailable = false;
        fen.BlackQueenSideCastlingAvailable = false;

        if (fenString[index] == '-')
        {
            index++;
        }
        else
        {
            if (!validCastlingCharacters.Contains(fenString[index]))
            {
                throw CreateParsingError(fenString, index);
            }

            while (validCastlingCharacters.Contains(fenString[index]))
            {
                switch (fenString[index])
                {
                    case 'K':
                        fen.WhiteKingSideCastlingAvailable = true;
                        break;
                    case 'Q':
                        fen.WhiteQueenSideCastlingAvailable = true;
                        break;
                    case 'k':
                        fen.BlackKingSideCastlingAvailable = true;
                        break;
                    case 'q':
                        fen.BlackQueenSideCastlingAvailable = true;
                        break;
                    default:
                        throw CreateParsingError(fenString, index);
                }

                index++;
            }
        }

        return index;
    }

    private static int ParseEnPassant(Fen fen, string fenString, int index)
    {
        const string validFileCharacters = "abcdefgh";
        const string validRankCharacters = "12345678";

        fen.EnPassantTargetPosition = null;

        if (fenString[index] == '-')
        {
            index++;
        }
        else
        {
            if (!validFileCharacters.Contains(fenString[index]))
            {
                throw CreateParsingError(fenString, index);
            }

            var file = fenString[index].ToFile();
            index++;

            if (!validRankCharacters.Contains(fenString[index]))
            {
                throw CreateParsingError(fenString, index);
            }

            var rank = fenString[index].ToRank();
            index++;

            fen.EnPassantTargetPosition = new Position(file, rank);
        }

        return index;
    }

    private static int ParseHalfMoveClock(Fen fen, string fenString, int index)
    {
        if (!char.IsAsciiDigit(fenString[index]))
        {
            throw CreateParsingError(fenString, index);
        }

        var length = 0;
        while (char.IsAsciiDigit(fenString[index]))
        {
            length++;
            index++;
        }

        fen.HalfMoveClock = int.Parse(fenString.AsSpan(index - length, length));

        return index;
    }

    private static int ParseFullMoveNumber(Fen fen, string fenString, int index)
    {
        if (!char.IsAsciiDigit(fenString[index]))
        {
            throw CreateParsingError(fenString, index);
        }

        var length = 0;
        while (index < fenString.Length && char.IsAsciiDigit(fenString[index]))
        {
            length++;
            index++;
        }

        fen.FullMoveNumber = int.Parse(fenString.AsSpan(index - length, length));

        return index;
    }

    private static ArgumentException CreateParsingError(string fenString, int index)
    {
        return new ArgumentException($"Invalid FEN string '{fenString}' at index {index}.");
    }
}