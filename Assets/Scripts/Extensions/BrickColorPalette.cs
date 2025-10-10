using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    ///     Enums and helper methods for the possible <see cref="BrickView"/>s colors.
    /// </summary>
    public static class BrickColorPalette
    {
        public enum BrickColor
        {
            Red = 0,
            Yellow = 1,
            Blue = 2
        }

        private static readonly Dictionary<BrickColor, Color> ColorMap = new()
        {
            { BrickColor.Red, new Color32(255, 0, 77, 255) },
            { BrickColor.Yellow, new Color32(255, 236, 39, 255) },
            { BrickColor.Blue, new Color32(41, 173, 255, 255) }
        };

        public static Color32 ToColor(this BrickColor brickColor)
        {
            return ColorMap[brickColor];
        }

        public static BrickColor GetRandomBrickColor()
        {
            return ColorMap.ElementAt(Random.Range(0, ColorMap.Count)).Key;
        }
    }
}