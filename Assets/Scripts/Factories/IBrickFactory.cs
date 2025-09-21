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