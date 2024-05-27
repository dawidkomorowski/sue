using System;
using System.Collections.Generic;

namespace Sue.Engine.Model.Chessboard
{
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

    public static class FileExtensions
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
}