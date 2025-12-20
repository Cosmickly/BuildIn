using Extensions;
using Records;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Factories
{
    /// <summary>
    ///     Manages the creation of <see cref="PlayingBrickView"/>s and their <see cref="BrickState"/>s.
    /// </summary>
    public class BrickFactory : IBrickFactory
    {
        // PREFABS
        private readonly PlayingBrickView _playingBrickViewPrefab;
        private readonly ProtoBrickView _protoBrickViewPrefab;

        // STATES
        private readonly BrickState[] _topBrickStates;

        // SPRITES
        private readonly Sprite[] _brickSprites;

        public BrickFactory(PlayingBrickView playingBrickViewPrefab, ProtoBrickView protoBrickViewPrefab,
            Sprite[] brickSprites)
        {
            _playingBrickViewPrefab = playingBrickViewPrefab;
            _protoBrickViewPrefab = protoBrickViewPrefab;
            _brickSprites = brickSprites;
        }

        /// <inheritdoc/>
        public PlayingBrickView InstantiatePlayingBrickView(Transform parent, Vector3 position, float localScaleMultiplier)
        {
            return InstantiateBrickView(_playingBrickViewPrefab, parent, position, localScaleMultiplier) as
                PlayingBrickView;
        }

        /// <inheritdoc/>
        public ProtoBrickView InstantiateProtoBrickView(Transform parent, Vector3 position, float localScaleMultiplier)
        {
            return InstantiateBrickView(_protoBrickViewPrefab, parent, position,
                localScaleMultiplier) as ProtoBrickView;
        }

        private BrickView InstantiateBrickView(BrickView prefab, Transform parent, Vector3 position, float localScaleMultiplier)
        {
            // Debug.Log($"Instantiating Brick View at {position.ToString()}");
            var newBrickView = Object.Instantiate(prefab, parent, false);
            newBrickView.transform.localPosition = position;
            newBrickView.transform.localScale *= localScaleMultiplier;
            return newBrickView;
        }

        /// <inheritdoc/>
        public BrickState CreateBrickState()
        {
            var color = BrickColorPalette.GetRandomBrickColor();
            return new BrickState
            {
                Active = true,
                Sprite = _brickSprites[(int)color],
                BrickColor = color
            };
        }
    }
}