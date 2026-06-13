using System.Linq;
using Configurations;
using Factories;
using Records;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Managers
{
    public class ProtoBrickManager
    {
        private readonly ProtoBrickView[] _protoBrickViews;
        private readonly BrickState[] _protoBrickStates;

        private readonly IBrickFactory _brickFactory;
        private readonly IGridConfig _gridConfig;

        private readonly Transform _selectionAreaTransform;

        private readonly BrickQueueManager _brickQueueManager;
        private readonly PlayingGridManager _playingGridManager;

        public ProtoBrickManager(IGridConfig gridConfig, IBrickFactory brickFactory, Transform selectionAreaTransform, BrickQueueManager brickQueueManager, PlayingGridManager playingGridManager)
        {
            _gridConfig = gridConfig;
            _brickFactory = brickFactory;
            _selectionAreaTransform = selectionAreaTransform;
            _brickQueueManager = brickQueueManager;
            _playingGridManager = playingGridManager;

            _protoBrickViews = new ProtoBrickView[_gridConfig.PlayingGridSize.x];
            _protoBrickStates = new BrickState[_gridConfig.PlayingGridSize.x];
        }

        /// <summary>
        ///     Instantiates <see cref="ProtoBrickView"/> in the selection area
        /// </summary>
        public void InitialiseProtoBricks()
        {
            Debug.Log("Initialising ProtoBricks");

            var offset = -new Vector2(_gridConfig.PlayingGridSize.x - 1, 0) / 2;

            for (var i = 0; i < _gridConfig.PlayingGridSize.x; i++)
            {
                _protoBrickStates[i] = new BrickState
                {
                    Active = false
                };

                _protoBrickViews[i] = _brickFactory.InstantiateProtoBrickView(
                    _selectionAreaTransform,
                    new Vector2(i, 0) * _gridConfig.BrickOffset + offset,
                    1);
            }

            UpdateBrickStates();
        }

        /// <summary>
        ///     Updates all <see cref="ProtoBrickView" />s to match corresponding BrickStates
        /// </summary>
        private void UpdateBrickStates()
        {
            for (var i = 0; i < _protoBrickViews.Length; i++)
            {
                _protoBrickViews[i].ApplyBrickState(_protoBrickStates[i]);
            }
        }

        /// <summary>
        ///     Attempts to dequeue a <see cref="PlayingBrickView"/> off the BrickQueue and adds it to the top row.
        /// </summary>
        /// <returns>false if there is already an Active brick, returns true otherwise.</returns>
        public bool TryAddTopBrick(int overlayId)
        {
            if (_protoBrickStates[overlayId].Active)
            {
                return false;
            }

            var brickToAdd = _brickQueueManager.DequeueBrick();

            _protoBrickStates[overlayId] = brickToAdd;

            CheckTopBricks();

            UpdateBrickStates();

            return true;
        }

        /// <summary>
        ///     Checks top <see cref="PlayingBrickView"/>s for a full row
        /// </summary>
        private void CheckTopBricks()
        {
            // TODO: bug - wrong bricks are being removed, and sometimes not being removed
            var bricksToRemove = new bool[_protoBrickStates.Length];

            for (var i = 0; i < _protoBrickStates.Length - 1; i++)
            {
                if (_protoBrickStates[i].BrickColor == _protoBrickStates[i + 1].BrickColor && _protoBrickStates[i].Active && _protoBrickStates[i + 1].Active)
                {
                    bricksToRemove[i] = true;
                    bricksToRemove[i + 1] = true;
                }
            }

            if (bricksToRemove.Any(b => b))
            {
                for (var z = 0; z < _protoBrickStates.Length; z++)
                {
                    if (bricksToRemove[z])
                    {
                        _protoBrickStates[z].Active = false;
                    }
                }

                UpdateBrickStates();

                // If we have removed a brick, we don't need to check for a full row
                return;
            }

            if (_protoBrickStates.All(b => b.Active))
            {
                _playingGridManager.ShiftGrid(_protoBrickStates);
                for (var j = 0; j < _protoBrickStates.Length; j++)
                {
                    _protoBrickStates[j] = new BrickState { Active = false };
                }

                UpdateBrickStates();
            }
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