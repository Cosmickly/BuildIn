using UnityEngine;

namespace Factories
{
    public interface IOverlayFactory
    {
        /// <summary>
        ///     Instantiates an <see cref="OverlayView"/>.
        /// </summary>
        /// <param name="parent">Parent of the OverlayView</param>
        /// <param name="position">Position relative to parent.</param>
        /// <returns>OverlayView</returns>
        public OverlayView InstantiateOverlayView(Transform parent, Vector3 position);
    }
}