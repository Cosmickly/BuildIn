using UnityEngine;

/// <summary>
///     The Overlay game object. Selectable area where <see cref="ProtoBrick"/>s are placed.
/// </summary>
public class Overlay : MonoBehaviour
{
    private BrickManager _brickManager;
    private GameManager _gameManager;
    private Animator _animator;
    private int _invalidAnimationHash;
    private int _highlightAnimationHash;

    private Vector3 _origin;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _invalidAnimationHash = Animator.StringToHash("Invalid");
        _highlightAnimationHash = Animator.StringToHash("Highlight");
    }

    private void Start()
    {
        _brickManager = GetComponentInParent<BrickManager>();
        _gameManager = _brickManager.GameManager;
        _origin = transform.position;
    }

    private void Update()
    {
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

        if (_brickManager.AddTopBrick(transform.position)) return;

        _animator.SetTrigger(_invalidAnimationHash);
    }

    public void ToggleHighlight(bool toggle)
    {
        _animator.SetBool(_highlightAnimationHash, toggle);
    }
}