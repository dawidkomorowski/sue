using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sue.Common.Model
{
    public enum Color
    {
        White,
        Black
    }

    public static class ColorExtensions
    {
        public static Color Opposite(this Color color)
        {
            switch (color)
            {
                case Color.White:
                    return Color.Black;
                case Color.Black:
                    return Color.White;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }
    }
}