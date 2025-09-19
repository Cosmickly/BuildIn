using Configurations;
using Factories;
using Records;
using UnityEngine;

namespace Managers
{
    public class GridManager
    {
        private readonly BrickState[,] _playingBrickStates;

        private readonly IBrickFactory _brickFactory;
        private readonly Transform _gridTransform;
        private readonly IGridConfig _gridConfig;

        public GridManager(IGridConfig gridConfig, IBrickFactory brickFactory, Transform gridTransform)
        {
            _brickFactory = brickFactory;
            _gridConfig = gridConfig;
            _gridTransform = gridTransform;
            _playingBrickStates = new BrickState[_gridConfig.GridSize.y, _gridConfig.GridSize.x];
        }

        /// <summary>
        ///     Initialises the bricks in the playing field
        /// </summary>
        public void InitialisePlayingBricks()
        {
            for (var i = 0; i < _gridConfig.GridSize.x; i++)
            for (var j = 0; j < _gridConfig.GridSize.y; j++)
            {
                _playingBrickStates[i, j] = _brickFactory.CreateBrickState();

                var brickView = _brickFactory
                    .InstantiateBrickView(
                        _gridTransform,
                        new Vector3(((_gridConfig.GridSize.x - 1) * .5f - i) * _gridConfig.BrickOffset.x,
                            -(j * _gridConfig.BrickOffset.y) - 1, 0),
                        1);

                brickView.UpdateBrickState(_playingBrickStates[j, i]);
            }
        }

        /// <summary>
        ///     Shifts all rows of active and top <see cref="BrickView"/>s down one unit.
        /// </summary>
        private void ShiftRow()
        {
            // todo
        }
    }
}