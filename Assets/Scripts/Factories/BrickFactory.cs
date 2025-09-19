using System.Collections.Generic;
using Records;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Factories
{
    /// <summary>
    ///     Manages the creation of <see cref="BrickView"/>s and their <see cref="BrickState"/>s.
    /// </summary>
    public class BrickFactory : IBrickFactory
    {
        // PREFABS
        private readonly BrickView _brickViewPrefab;

        // STATES
        private readonly BrickState[] _topBrickStates;

        // SPRITES
        private readonly Sprite[] _brickSprites = new Sprite[3];

        private readonly List<Color> _brickColours = new()
        {
            new Color32(255, 0, 77, 255),
            new Color32(41, 173, 255, 255),
            new Color32(255, 236, 39, 255)
        };

        public BrickFactory(BrickView brickViewPrefab)
        {
            _brickViewPrefab = brickViewPrefab;
        }

        /// <inheritdoc/>
        public BrickView InstantiateBrickView(Transform parent, Vector3 position, float localScaleMultiplier)
        {
            BrickView newBrickView = Object.Instantiate(_brickViewPrefab, position, Quaternion.identity, parent);
            newBrickView.transform.localScale *= localScaleMultiplier;
            return newBrickView;
        }

        /// <inheritdoc/>
        public BrickState CreateBrickState() => new()
        {
            Active = true,
            Sprite = _brickSprites[Random.Range(0, _brickSprites.Length)],
            SpriteColor = _brickColours[Random.Range(0, _brickColours.Count)],
        };


        /// <summary>
        ///     Dequeues a <see cref="BrickView"/> off the <see cref="_brickQueue"/> and adds it to the top row.
        ///     Creates a new Brick in the brickQueue.
        /// </summary>
        public void AddTopBrick(int overlayIndex)
        {
            //todo
        }

        /// <summary>
        ///     Destroys a <see cref="BrickView"/> in the top row and clears the overlay.
        /// </summary>
        /// <param name="pos">Position of the brick.</param>
        /// <param name="target">Where the brick should merge to.</param>
        private void MergeTopBrick(Vector3 pos, Vector3 target)
        {
            // todo
        }

        /// <summary>
        ///     Checks top <see cref="BrickView"/>s for matching colors, and removes them.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>Boolean on if a <see cref="BrickView"/> was removed.</returns>
        private void CheckTopBricks(Vector3 pos)
        {
            // todo
        }

    }
}