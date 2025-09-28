using Configurations;
using Factories;
using Records;
using UnityEngine;

namespace Managers
{
    public class OverlayManager
    {
        private readonly OverlayState[] _overlayStates;
        private readonly OverlayView[] _overlayViews;
        private int _selectedOverlay;

        private readonly IGridConfig _gridConfig;
        private readonly IOverlayFactory _overlayFactory;
        private readonly Transform _overlayTransform;

        public OverlayManager(IGridConfig gridConfig, IOverlayFactory overlayFactory, Transform overlayTransform)
        {
            _gridConfig = gridConfig;
            _overlayFactory = overlayFactory;
            _overlayTransform = overlayTransform;
            _overlayViews = new OverlayView[_gridConfig.GridSize.x];
            _overlayStates = new OverlayState[_gridConfig.GridSize.x];
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
                _overlayStates[i] = new OverlayState
                {
                    Id = i
                };

                _overlayViews[i] = _overlayFactory.InstantiateOverlayView(
                    _overlayTransform,
                    new Vector2(i, 0) * _gridConfig.BrickOffset + offset
                );

                _overlayViews[i].SetOverlayManager(this);
                // new Vector3(((_gridConfig.GridSize.x - 1) * .5f - i) * _gridConfig.BrickOffset.x, 0, 0));
            }

            _selectedOverlay = 0;
            UpdateOverlayViews();
        }

        public void OverlaySelected(int overlayId)
        {
            UnselectOverlay(_selectedOverlay);
            _selectedOverlay = overlayId;
            UpdateOverlayViews();
        }

        /// <summary>
        ///     Changes which <see cref="OverlayView"/> is selected.
        /// </summary>
        private void UnselectOverlay(int overlayIndex)
        {
            _overlayStates[overlayIndex].Selected = false;
        }

        /// <summary>
        ///     Moves to a new <see cref="OverlayView"/>, offset from the currently selected Overlay.
        /// </summary>
        public void MoveSelectedOverlay(int offset)
        {
            UnselectOverlay(_selectedOverlay);
            _selectedOverlay += offset;
            _selectedOverlay = Mathf.Clamp(_selectedOverlay, 0, _gridConfig.GridSize.x - 1);
            UpdateOverlayViews();
        }

        private void UpdateOverlayViews()
        {
            _overlayStates[_selectedOverlay].Selected = true;
            for (var i = 0; i < _overlayViews.Length; i++)
            {
                _overlayViews[i].ApplyOverlayState(_overlayStates[i]);
            }
        }
    }
}