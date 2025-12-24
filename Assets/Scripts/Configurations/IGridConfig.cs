using UnityEngine;

namespace Configurations
{
    public interface IGridConfig
    {
        /// <summary>
        ///     Horizontal and Vertical distance between each brick
        /// </summary>
        Vector2Int GridSize { get; }

        /// <summary>
        ///     Size of the playing grid.
        /// </summary>
        Vector2 BrickOffset { get; }

        /// <summary>
        ///     Given a number of columns, returns the offset in order to center them on the X axis
        /// </summary>
        /// <returns></returns>
        Vector2 GetGridOffset(Vector2Int gridSize);
    }
}