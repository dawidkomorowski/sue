using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sue.Common.Model.Chessboard
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
        public static int Index(this File file)
        {
            switch (file)
            {
                case File.A:
                    return 0;
                case File.B:
                    return 1;
                case File.C:
                    return 2;
                case File.D:
                    return 3;
                case File.E:
                    return 4;
                case File.F:
                    return 5;
                case File.G:
                    return 6;
                case File.H:
                    return 7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(file), file, null);
            }
        }

        public static IEnumerable<File> Enumerable()
        {
            return new[] {File.A, File.B, File.C, File.D, File.E, File.F, File.G, File.H};
        }

        public static File Add(this File file, int offset)
        {
            var fileIndex = file.Index();
            var newFileIndex = fileIndex + offset;
            return newFileIndex.ToFile();
        }
    }
}