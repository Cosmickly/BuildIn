using UnityEngine;

/// <summary>
///     The Overlay game object. Selectable area where <see cref="ProtoBrick"/>s are placed.
/// </summary>
public class Overlay : MonoBehaviour
{
    private BrickManager _brickManager;
    private GameManager _gameManager;
    private Animator _animator;

    private bool _hasBrick;
    private Vector3 _origin;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _brickManager = GetComponentInParent<BrickManager>();
        _gameManager = _brickManager.GameManager;
        _origin = transform.position;
    }

    private void Update()
    {
        // _animator.SetBool("HasBrick", HasBrick);
        transform.position = _origin;
    }

    private void OnMouseEnter()
    {
        if (!_gameManager.Playing) return;

        _brickManager.SelectOverlay(this);
    }

    private void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (AddBrickToOverlay())
        {
            _brickManager.AddTopBrick(transform.position);
        }
        else
        {
            _animator.SetTrigger("Invalid");
        }
    }

    public void ToggleHighlight(bool toggle)
    {
        _animator.SetBool("Highlight", toggle);
    }

    /// <summary>
    ///     Adds a brick to overlay.
    /// </summary>
    public bool AddBrickToOverlay()
    {
        if (_hasBrick)
        {
            return false;
        }

        _hasBrick = true;
        return true;
    }

    /// <summary>
    ///     Sets <see cref="_hasBrick"/> to false.
    /// </summary>
    public bool RemoveBrickFromOverlay()
    {
        if (!_hasBrick)
        {
            return false;
        }

        _hasBrick = false;
        return true;
    }
}