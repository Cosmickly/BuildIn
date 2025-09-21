using Configurations;
using Factories;
using Records;
using UnityEngine;

namespace Managers
{
    public class OverlayManager
    {
        private readonly OverlayState[] _overlayStates;
        private readonly OverlayView[] _overlays;
        private int _selectedOverlay;

        private readonly IGridConfig _gridConfig;
        private readonly IOverlayFactory _overlayFactory;
        private readonly Transform _overlayTransform;

        public OverlayManager(IGridConfig gridConfig, IOverlayFactory overlayFactory, Transform overlayTransform)
        {
            _gridConfig = gridConfig;
            _overlayFactory = overlayFactory;
            _overlayTransform = overlayTransform;
            _overlays = new OverlayView[_gridConfig.GridSize.x];
        }

        /// <summary>
        ///     Initialises the Overlays
        /// </summary>
        public void InitialiseOverlays()
        {
            Debug.Log("Initialising Overlays");

            var offset = -new Vector2(_gridConfig.GridSize.x - 1, 0) / 2;

            for (var i = 0; i < _gridConfig.GridSize.x; i++)
            {
                _overlays[i] = _overlayFactory.InstantiateOverlayView(
                    _overlayTransform,
                    new Vector2(i, 0) * _gridConfig.BrickOffset + offset
                );
                // new Vector3(((_gridConfig.GridSize.x - 1) * .5f - i) * _gridConfig.BrickOffset.x, 0, 0));
            }

            SelectOverlay(0);
        }

        /// <summary>
        ///     Changes which <see cref="OverlayView"/> is selected.
        /// </summary>
        public void SelectOverlay(int overlayIndex)
        {
            _overlays[_selectedOverlay].ToggleHighlight(false);
            _selectedOverlay = overlayIndex;
            _overlays[_selectedOverlay].ToggleHighlight(true);
        }

        /// <summary>
        ///     Moves to a new <see cref="OverlayView"/>, offset from the currently selected Overlay.
        /// </summary>
        public void MoveSelectedOverlay(int offset)
        {
            _selectedOverlay += offset;
            _selectedOverlay = Mathf.Clamp(_selectedOverlay, 0, _gridConfig.GridSize.x - 1);
            SelectOverlay(_selectedOverlay);
        }
    }
}