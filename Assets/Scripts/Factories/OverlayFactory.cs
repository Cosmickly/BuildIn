using Configurations;
using UnityEngine;

namespace Factories
{
    public class OverlayFactory : IOverlayFactory
    {
        private readonly OverlayView _overlayViewPrefab;
        private readonly IGridConfig _gridConfig;

        public OverlayFactory(OverlayView overlayViewPrefab, IGridConfig gridConfig)
        {
            _overlayViewPrefab = overlayViewPrefab;
            _gridConfig = gridConfig;
        }

        /// <inheritdoc/>
        public OverlayView InstantiateOverlayView(Transform parent, Vector3 position)
        {
            var newOverlayView = Object.Instantiate(_overlayViewPrefab, position, Quaternion.identity, parent);
            newOverlayView.transform.localScale = _gridConfig.BrickOffset;
            return newOverlayView;
        }
    }
}