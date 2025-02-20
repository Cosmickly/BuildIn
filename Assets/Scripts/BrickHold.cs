using UnityEngine;

public class BrickHold : MonoBehaviour
{
    private BrickManager _brickManager;
    private GameManager _gameManager;
    private Animator _animator;
    private bool _usedHold;
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
        transform.position = _origin;
    }

    private void OnMouseOver()
    {
        if (!_gameManager.Playing) return;

        _animator.SetBool("Highlight", true);

        if (Input.GetMouseButtonDown(0))
        {
            if (_usedHold)
            {
                _animator.SetTrigger("Invalid");
                return;
            }
            _brickManager.HoldBrick();
        }
    }

    private void OnMouseExit()
    {
        _animator.SetBool("Highlight", false);
    }

    public void UsedHold(bool used)
    {
        _usedHold = used;
        _animator.SetBool("UsedHold", used);
    }
}