using UnityEngine;

namespace Configurations
{
    public class GridConfig : IGridConfig
    {
        public Vector2Int PlayingGridSize { get; }

        public int MaxGridSizeY => 13;

        public Vector2 BrickOffset { get; }

        /// <summary>
        ///     X is columns, Y is rows
        /// </summary>
        /// <param name="playingGridSizeX"></param>
        /// <param name="playingGridSizeY"></param>
        /// <param name="brickOffsetX"></param>
        /// <param name="brickOffsetY"></param>
        public GridConfig(int playingGridSizeX, int playingGridSizeY, float brickOffsetX, float brickOffsetY)
        {
            PlayingGridSize = new Vector2Int(playingGridSizeX, playingGridSizeY);
            BrickOffset = new Vector2(brickOffsetX, brickOffsetY);
        }

        // private void OnValidate()
        // {
        //     _playingGridSize.x = Mathf.Clamp(_playingGridSize.x, 0, 10);
        //     _playingGridSize.y = Mathf.Clamp(_playingGridSize.y, 0, 12);
        // }

        public Vector2 GetGridOffset(int x)
        {
            return -new Vector2(x - 1, MaxGridSizeY / 2f) / 2;
        }
    }
}