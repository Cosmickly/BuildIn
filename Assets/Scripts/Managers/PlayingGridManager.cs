using Configurations;
using Factories;
using Records;
using UnityEngine;

namespace Managers
{
    /// <summary>
    ///     Manages the Playing Grid
    /// </summary>
    public class PlayingGridManager
    {
        private readonly BrickState[,] _playingBrickStates;
        private readonly BrickView[,] _brickViews;

        private readonly IBrickFactory _brickFactory;
        private readonly Transform _gridTransform;
        private readonly IGridConfig _gridConfig;

        public PlayingGridManager(IGridConfig gridConfig, IBrickFactory brickFactory, Transform gridTransform)
        {
            _brickViews = new BrickView[gridConfig.PlayingGridSize.x, gridConfig.MaxGridSizeY];
            _brickFactory = brickFactory;
            _gridConfig = gridConfig;
            _gridTransform = gridTransform;
            _playingBrickStates = new BrickState[_gridConfig.PlayingGridSize.x, _gridConfig.MaxGridSizeY];
        }

        /// <summary>
        ///     Initializes the bricks in the playing field
        /// </summary>
        public void InitialisePlayingBricks()
        {
            Debug.Log("Initializing Playing Bricks with grid size: " + _gridConfig.PlayingGridSize);

            // We create the bricks starting from top left, so we need to offset
            // the starting position to centre the bricks.
            var gridOffset = _gridConfig.GetGridOffset(_gridConfig.PlayingGridSize.x);

            int thing = _gridConfig.MaxGridSizeY - _gridConfig.PlayingGridSize.y;

            // Left to right
            for (var i = 0; i < _gridConfig.PlayingGridSize.x; i++)
            {
                // Top to bottom
                for (int j = _gridConfig.MaxGridSizeY - 1; j >= 0; j--)
                {
                    _playingBrickStates[i, j] = _brickFactory.CreateBrickState();

                    // Deactivate BrickViews beyond the starting area
                    if (j < thing)
                    {
                        _playingBrickStates[i, j].Active = false;
                    }

                    // Instantiate BrickView
                    var brickView = _brickFactory
                        .InstantiatePlayingBrickView(
                            _gridTransform,
                            new Vector2(i, j) * _gridConfig.BrickOffset + gridOffset,
                            1);

                    _brickViews[i, j] = brickView;

                    // Apply BrickState
                    brickView.ApplyBrickState(_playingBrickStates[i, j]);
                }
            }
        }

        /// <summary>
        ///     Shifts all rows of active and top <see cref="PlayingBrickView"/>s down one unit.
        /// </summary>
        public void ShiftGrid(BrickState[] protoBrickStates)
        {
            // Left to right
            for (var i = 0; i < _gridConfig.PlayingGridSize.x; i++)
            {
                // Bottom to top
                for (var j = 0; j < _gridConfig.MaxGridSizeY; j++)
                {
                    // If top row, set copy ProtoBrick colour
                    if (j == _gridConfig.MaxGridSizeY - 1)
                    {
                        _playingBrickStates[i, j] = _brickFactory.CreateBrickState(protoBrickStates[i].BrickColor);
                        continue;
                    }

                    var brickState = _playingBrickStates[i, j + 1];
                    _playingBrickStates[i, j] = brickState;
                }
            }

            UpdateBrickStates();
        }

        private void UpdateBrickStates()
        {
            // Left to right
            for (var i = 0; i < _gridConfig.PlayingGridSize.x; i++)
            {
                // Top to bottom
                for (var j = 0; j < _gridConfig.MaxGridSizeY; j++)
                {
                    // Apply BrickState
                    _brickViews[i, j].ApplyBrickState(_playingBrickStates[i, j]);
                }
            }
        }
    }
}