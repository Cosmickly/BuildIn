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
        private int _focusedOverlay;

        private readonly IGridConfig _gridConfig;
        private readonly IOverlayFactory _overlayFactory;
        private readonly Transform _overlayTransform;

        private readonly ProtoBrickManager _protoBrickManager;

        public OverlayManager(IGridConfig gridConfig, IOverlayFactory overlayFactory, Transform overlayTransform, ProtoBrickManager protoBrickManager)
        {
            _gridConfig = gridConfig;
            _overlayFactory = overlayFactory;
            _overlayTransform = overlayTransform;
            _protoBrickManager = protoBrickManager;

            _overlayViews = new OverlayView[_gridConfig.GridSize.x];
            _overlayStates = new OverlayState[_gridConfig.GridSize.x];
        }

        /// <summary>
        ///     Initialises the Overlays
        /// </summary>
        public void InitialiseOverlays()
        {
            Debug.Log("Initialising Overlays");

            var offset = _gridConfig.GetHorizontalOffset(_gridConfig.GridSize.x);

            for (var i = 0; i < _gridConfig.GridSize.x; i++)
            {
                _overlayStates[i] = new OverlayState
                {
                    Id = i
                };

                _overlayViews[i] = _overlayFactory
                    .InstantiateOverlayView(
                        _overlayTransform,
                        new Vector2(i, 0) * _gridConfig.BrickOffset + offset
                    );

                _overlayViews[i].SetOverlayManager(this);
                // new Vector3(((_gridConfig.GridSize.x - 1) * .5f - i) * _gridConfig.BrickOffset.x, 0, 0));
            }

            _focusedOverlay = 0;
            UpdateOverlayViews();
        }

        /// <summary>
        ///     Selects the overlay with
        /// </summary>
        /// <param name="overlayId"></param>
        public void FocusOverlayById(int overlayId)
        {
            UnfocusOverlayById(_focusedOverlay);
            _focusedOverlay = overlayId;
            UpdateOverlayViews();
        }

        /// <summary>
        ///     Sets the <see cref="OverlayState.Focused"/> property on the corresponding <see cref="OverlayState"/> to false.
        /// </summary>
        private void UnfocusOverlayById(int overlayId)
        {
            _overlayStates[overlayId].Focused = false;
        }

        /// <summary>
        ///     Applys the <see cref="OverlayState"/> to all <see cref="OverlayView"/>s.
        /// </summary>
        private void UpdateOverlayViews()
        {
            _overlayStates[_focusedOverlay].Focused = true;
            for (var i = 0; i < _overlayViews.Length; i++)
            {
                _overlayViews[i].ApplyOverlayState(_overlayStates[i]);
            }
        }

        /// <summary>
        ///     Moves to a new <see cref="OverlayView"/>, offset from the currently selected Overlay.
        /// </summary>
        public void MoveSelectedOverlay(int offset)
        {
            UnfocusOverlayById(_focusedOverlay);
            _focusedOverlay += offset;
            _focusedOverlay = Mathf.Clamp(_focusedOverlay, 0, _gridConfig.GridSize.x - 1);
            UpdateOverlayViews();
        }

        /// <summary>
        ///     Call when an <see cref="OverlayView"/> is selected with the mouse/touchscreen.
        /// </summary>
        /// <param name="id"></param>
        public void OverlaySelected(int id)
        {
            if (!_overlayStates[id].Focused)
            {
                Debug.LogError($"Overlay {id} selected with mouse, but was not focused. Something has gone wrong.");
                return;
            }

            TryPlaceFocusedBrick();
        }

        /// <summary>
        ///     Attempts to place a Brick as the focused <see cref="OverlayView"/>.
        /// </summary>
        public void TryPlaceFocusedBrick()
        {
            if (_overlayStates[_focusedOverlay].HasBrick)
            {
                _overlayViews[_focusedOverlay].PlayInvalidAnimation();
                return;
            }

            _protoBrickManager.AddTopBrick(_focusedOverlay);
        }
    }
}