using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Sue.Engine.Model;

public enum File
{
    A = 0,
    B = 1,
    C = 2,
    D = 3,
    E = 4,
    F = 5,
    G = 6,
    H = 7
}

internal static class FileExtensions
{
    public static IReadOnlyList<File> Files() => [File.A, File.B, File.C, File.D, File.E, File.F, File.G, File.H];

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Index(this File file)
    {
        Debug.Assert(file is >= File.A and <= File.H, "file is >= File.A and <= File.H");
        return (int)file;
    }

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static File ToFile(this int index)
    {
        Debug.Assert(index is >= 0 and <= 7, "index is >= 0 and <= 7");
        return (File)index;
    }
}