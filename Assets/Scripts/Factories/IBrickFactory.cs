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
        ///     Instantiates a new <see cref="BrickView"/>.
        /// </summary>
        /// <param name="parent">Parent for BrickView</param>
        /// <param name="position">Where to position the BrickView relative to parent</param>
        /// <param name="localScaleMultiplier">Local scale multiplier.</param>
        /// <returns>BrickView</returns>
        public BrickView InstantiateBrickView(Transform parent, Vector3 position, float localScaleMultiplier);
    }
}