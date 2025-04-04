using UnityEngine;

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
            Debug.Log($"Attempted to Add brick from occupied overlay {transform.position}.");
            return false;
        }

        Debug.Log($"Added brick to Overlay {transform.position}.");
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
            Debug.Log($"Attempted to remove brick from empty overlay {transform.position}.");
            return false;
        }

        Debug.Log($"Removed brick from Overlay {transform.position}.");
        _hasBrick = false;
        return true;
    }
}