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
            _playingBrickStates = new BrickState[_gridConfig.GridSize.x, _gridConfig.GridSize.y];
        }

        /// <summary>
        ///     Initialises the bricks in the playing field
        /// </summary>
        public void InitialisePlayingBricks()
        {
            Debug.Log("Initialising Playing Bricks");

            var offset = _gridConfig.GetGridOffset(_gridConfig.GridSize);

            for (var i = 0; i < _gridConfig.GridSize.x; i++)
            {
                for (var j = 0; j < _gridConfig.GridSize.y; j++)
                {
                    _playingBrickStates[i, j] = _brickFactory.CreateBrickState();

                    var brickView = _brickFactory
                        .InstantiatePlayingBrickView(
                            _gridTransform,
                            new Vector2(i, j) * _gridConfig.BrickOffset + offset,
                            1);

                    brickView.ApplyBrickState(_playingBrickStates[i, j]);
                }
            }
        }

        /// <summary>
        ///     Shifts all rows of active and top <see cref="PlayingBrickView"/>s down one unit.
        /// </summary>
        private void ShiftRow()
        {
            // todo
        }
    }
}