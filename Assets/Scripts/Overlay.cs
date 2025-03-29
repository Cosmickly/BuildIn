using UnityEngine;

public class Overlay : MonoBehaviour
{
    private BrickManager _brickManager;
    private GameManager _gameManager;
    private Animator _animator;

    public bool HasBrick;
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

        if (!HasBrick)
        {
            HasBrick = _brickManager.AddTopBrick(transform.position);
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
    ///     Sets <see cref="HasBrick"/> to false.
    /// </summary>
    public void ClearOverlay()
    {
        Debug.Log($"Overlay {transform.position} cleared");
        HasBrick = false;
    }
}