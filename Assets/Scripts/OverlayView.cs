using Managers;
using Records;
using UnityEngine;

/// <summary>
///     The Overlay game object. Selectable area where <see cref="ProtoBrickView"/>s are placed.
/// </summary>
public class OverlayView : MonoBehaviour
{
    private Animator _animator;
    private int _invalidAnimationHash;
    private int _highlightAnimationHash;

    private int _id;
    private OverlayManager _overlayManager;

    public OverlayView(OverlayManager overlayManager)
    {
        _overlayManager = overlayManager;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _invalidAnimationHash = Animator.StringToHash("Invalid");
        _highlightAnimationHash = Animator.StringToHash("Highlight");
    }

    private void OnMouseEnter()
    {
        _overlayManager.SelectOverlay(_id);
    }

    private void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        // todo

        _animator.SetTrigger(_invalidAnimationHash);
    }

    public void ToggleHighlight(bool toggle)
    {
    }

    public void UpdateOverlayState(OverlayState state)
    {
        _animator.SetBool(_highlightAnimationHash, state.Selected);
    }
}