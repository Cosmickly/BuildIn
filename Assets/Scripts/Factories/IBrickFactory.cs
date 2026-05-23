using Extensions;
using Records;
using UnityEngine;

namespace Factories
{
    public interface IBrickFactory
    {
        /// <summary>
        ///     Creates a random <see cref="BrickState"/>.
        /// </summary>
        /// <returns>BrickState</returns>
        public BrickState CreateBrickState();

        /// <summary>
        ///     Creates a <see cref="BrickState" /> with a given colour.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public BrickState CreateBrickState(BrickColorPalette.BrickColor color);

        /// <summary>
        ///     Instantiates a new <see cref="PlayingBrickView"/>.
        /// </summary>
        /// <param name="parent">Parent for BrickView</param>
        /// <param name="position">Where to position the BrickView relative to parent</param>
        /// <param name="localScaleMultiplier">Local scale multiplier.</param>
        /// <returns>BrickView</returns>
        public PlayingBrickView InstantiatePlayingBrickView(Transform parent, Vector3 position, float localScaleMultiplier);

        /// <summary>
        ///     Instantiates a new <see cref="ProtoBrickView"/>.
        /// </summary>
        /// <param name="parent">Parent for ProtoBrickView</param>
        /// <param name="position">Where to position the ProtoBrickView relative to parent</param>
        /// <param name="localScaleMultiplier">Local scale multiplier.</param>
        /// <returns>BrickView</returns>
        public ProtoBrickView InstantiateProtoBrickView(Transform parent, Vector3 position, float localScaleMultiplier);
    }
}