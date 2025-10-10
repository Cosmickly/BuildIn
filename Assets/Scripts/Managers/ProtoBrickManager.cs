using Configurations;
using Factories;
using Records;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Managers
{
    public class ProtoBrickManager
    {
        private readonly ProtoBrickView[] _brickViews;
        private readonly BrickState[] _brickStates;

        private readonly IBrickFactory _brickFactory;
        private readonly IGridConfig _gridConfig;

        private readonly Transform _selectionAreaTransform;

        private readonly BrickQueueManager _brickQueueManager;

        public ProtoBrickManager(IGridConfig gridConfig, IBrickFactory brickFactory, Transform selectionAreaTransform, BrickQueueManager brickQueueManager)
        {
            _gridConfig = gridConfig;
            _brickFactory = brickFactory;
            _selectionAreaTransform = selectionAreaTransform;
            _brickQueueManager = brickQueueManager;

            _brickViews = new ProtoBrickView[_gridConfig.GridSize.x];
            _brickStates = new BrickState[_gridConfig.GridSize.x];
        }

        /// <summary>
        ///     Instantiates <see cref="ProtoBrickView"/> in the selection area
        /// </summary>
        public void InitialiseProtoBricks()
        {
            Debug.Log("Initialising ProtoBricks");

            var offset = -new Vector2(_gridConfig.GridSize.x - 1, 0) / 2;

            for (var i = 0; i < _gridConfig.GridSize.x; i++)
            {
                _brickStates[i] = new BrickState
                {
                    Active = false
                };

                _brickViews[i] = _brickFactory.InstantiateProtoBrickView(
                    _selectionAreaTransform,
                    new Vector2(i, 0) * _gridConfig.BrickOffset + offset,
                    1);
            }

            UpdateBrickStates();
        }

        private void UpdateBrickStates()
        {
            for (var i = 0; i < _brickViews.Length; i++)
            {
                _brickViews[i].ApplyBrickState(_brickStates[i]);
            }
        }

        /// <summary>
        ///     Dequeues a <see cref="PlayingBrickView"/> off the BrickQueue and adds it to the top row.
        ///     Creates a new Brick in the brickQueue.
        /// </summary>
        public void AddTopBrick(int overlayId)
        {
            var brickToAdd = _brickQueueManager.DequeueBrick();

            _brickStates[overlayId] = brickToAdd;

            UpdateBrickStates();
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