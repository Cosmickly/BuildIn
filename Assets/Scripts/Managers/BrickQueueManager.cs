using System.Collections.Generic;
using Configurations;
using Factories;
using Records;
using UnityEngine;

namespace Managers
{
    public class BrickQueueManager
    {
        /// <summary>
        ///     The <see cref="BrickState"/>s in the queue. Index 0 is first in line.
        /// </summary>
        private Queue<BrickState> _brickQueueStates;

        /// <summary>
        ///     The <see cref="PlayingBrickView"/>s in the queue. Index 0 is first in line.
        /// </summary>
        private PlayingBrickView[] _brickQueueViews;

        //= new(7, -1, 0);
        private readonly Transform _brickQueueTransform;

        /// <summary>
        ///     Physical size of bricks in the queue
        /// </summary>
        private const float BrickQueueSize = 2;

        /// <summary>
        ///     Total number of bricks in the queue;
        /// </summary>
        private const int BrickQueueCount = 5;

        private readonly IBrickFactory _brickFactory;
        private readonly IGridConfig _gridConfig;

        public BrickQueueManager(IGridConfig gridConfig, IBrickFactory brickFactory, Transform brickQueueTransform)
        {
            _brickFactory = brickFactory;
            _gridConfig = gridConfig;
            _brickQueueTransform = brickQueueTransform;
        }

        /// <summary>
        ///     Intitialises the Brick queue.
        /// </summary>
        public void InitialiseBrickQueue()
        {
            Debug.Log("Initialising Brick Queue");
            _brickQueueStates = new Queue<BrickState>();
            _brickQueueViews = new PlayingBrickView[BrickQueueCount];

            for (var i = 0; i < BrickQueueCount; i++)
            {
                _brickQueueStates.Enqueue(_brickFactory.CreateBrickState());

                _brickQueueViews[i] = _brickFactory
                    .InstantiatePlayingBrickView(
                        _brickQueueTransform,
                        new Vector3(0, -i + _gridConfig.BrickOffset.y, 0),
                        BrickQueueSize);
            }

            UpdateBrickQueueViews();
        }

        /// <summary>
        ///     Dequeues a <see cref="BrickState"/> from the queue and creates a new one at the end.
        /// </summary>
        public BrickState DequeueBrick() //todo use
        {
            var dequeuedBrick = _brickQueueStates.Dequeue();

            _brickQueueStates.Enqueue(_brickFactory.CreateBrickState());

            UpdateBrickQueueViews();

            return dequeuedBrick;
        }

        private void UpdateBrickQueueViews()
        {
            // Cannot iterate through Queue with i
            var index = 0;

            foreach (var brickState in _brickQueueStates)
            {
                _brickQueueViews[index].ApplyBrickState(brickState);
                index++;
            }
        }
    }
}