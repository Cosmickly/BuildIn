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
        ///     Converts a position on the grid to the world
        /// </summary>
        public Vector3 GetWorldPosition(Vector2 position);
    }
}