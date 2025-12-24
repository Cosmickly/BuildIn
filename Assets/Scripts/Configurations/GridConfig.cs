using UnityEngine;

namespace Configurations
{
    public class GridConfig : IGridConfig
    {
        public Vector2Int GridSize { get; }

        public Vector2 BrickOffset { get; }

        public GridConfig(int gridSizeX, int gridSizeY, float brickOffsetX, float brickOffsetY)
        {
            GridSize = new Vector2Int(gridSizeX, gridSizeY);
            BrickOffset = new Vector2(brickOffsetX, brickOffsetY);
        }

        // private void OnValidate()
        // {
        //     _playingGridSize.x = Mathf.Clamp(_playingGridSize.x, 0, 10);
        //     _playingGridSize.y = Mathf.Clamp(_playingGridSize.y, 0, 12);
        // }

        public Vector2 GetHorizontalOffset(int columns)
        {
            return -new Vector2(columns - 1, 0) / 2;
        }
    }
}