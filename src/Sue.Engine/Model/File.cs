using System;
using System.Collections.Generic;

namespace Sue.Engine.Model;

public enum File
{
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H
}

internal static class FileExtensions
{
    public static char ToChar(this File file)
    {
        return file switch
        {
            File.A => 'a',
            File.B => 'b',
            File.C => 'c',
            File.D => 'd',
            File.E => 'e',
            File.F => 'f',
            File.G => 'g',
            File.H => 'h',
            _ => throw new ArgumentOutOfRangeException(nameof(file), file, null)
        };
    }

    public static int Index(this File file)
    {
        return file switch
        {
            File.A => 0,
            File.B => 1,
            File.C => 2,
            File.D => 3,
            File.E => 4,
            File.F => 5,
            File.G => 6,
            File.H => 7,
            _ => throw new ArgumentOutOfRangeException(nameof(file), file, null)
        };
    }

    public static IReadOnlyList<File> Enumerable() => [File.A, File.B, File.C, File.D, File.E, File.F, File.G, File.H];

    public static File Add(this File file, int offset)
    {
        var fileIndex = file.Index();
        var newFileIndex = fileIndex + offset;
        return newFileIndex.ToFile();
    }
}

internal static class CharExtensionsForFile
{
    public static File ToFile(this char c)
    {
        return c switch
        {
            'a' => File.A,
            'b' => File.B,
            'c' => File.C,
            'd' => File.D,
            'e' => File.E,
            'f' => File.F,
            'g' => File.G,
            'h' => File.H,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }
}

internal static class IntExtensionsForFile
{
    public static File ToFile(this int index)
    {
        return index switch
        {
            0 => File.A,
            1 => File.B,
            2 => File.C,
            3 => File.D,
            4 => File.E,
            5 => File.F,
            6 => File.G,
            7 => File.H,
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
    }
}