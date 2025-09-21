using Configurations;
using Factories;
using Records;
using UnityEngine;

namespace Managers
{
    public class ProtoBrickManager
    {
        private ProtoBrickView[] _brickViews;
        private BrickState[] _brickStates;

        private IBrickFactory _brickFactory;
        private readonly IGridConfig _gridConfig;

        private Transform _selectionAreaTransform;

        public ProtoBrickManager(IGridConfig gridConfig, IBrickFactory brickFactory, Transform selectionAreaTransform)
        {
            _gridConfig = gridConfig;
            _brickFactory = brickFactory;
            _selectionAreaTransform = selectionAreaTransform;
        }

        /// <summary>
        ///     Instantiates <see cref="ProtoBrickView"/> in the selection area
        /// </summary>
        public void InitialiseProtoBricks()
        {
            Debug.Log("Initialising ProtoBricks");
            for (var i = 0; i < _gridConfig.GridSize.x; i++)
            {
                var protoBrickView = _brickFactory.InstantiateProtoBrickView(
                    _selectionAreaTransform,
                    new Vector3(((_gridConfig.GridSize.x - 1) * .5f - i) * _gridConfig.BrickOffset.x, 0, 0),
                    0);

                _brickViews[i] = protoBrickView;
            }
        }

        /// <summary>
        ///     Dequeues a <see cref="PlayingBrickView"/> off the <see cref="_brickQueue"/> and adds it to the top row.
        ///     Creates a new Brick in the brickQueue.
        /// </summary>
        public void AddTopBrick(int overlayIndex)
        {
            //todo
        }

        /// <summary>
        ///     Checks top <see cref="PlayingBrickView"/>s for matching colors, and removes them.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>Boolean on if a <see cref="PlayingBrickView"/> was removed.</returns>
        private void CheckTopBricks(Vector3 pos)
        {
            // todo
        }

        /// <summary>
        ///     Destroys a <see cref="PlayingBrickView"/> in the top row and clears the overlay.
        /// </summary>
        /// <param name="pos">Position of the brick.</param>
        /// <param name="target">Where the brick should merge to.</param>
        private void MergeTopBrick(Vector3 pos, Vector3 target)
        {
            // todo
        }
    }
}