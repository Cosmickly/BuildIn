using System;
using Factories;
using Managers;
using UnityEngine;

[Obsolete("BrickHold not part of game loop anymore")]
public class BrickHold : MonoBehaviour
{
    private BrickFactory _brickFactory;
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
        _brickFactory = GetComponentInParent<BrickFactory>();
        // _gameManager = _brickFactory.GameManager;
        _origin = transform.position;
    }

    private void Update()
    {
        transform.position = _origin;
    }

    private void OnMouseOver()
    {
        // if (!_gameManager.Playing) return;

        _animator.SetBool("Highlight", true);

        if (Input.GetMouseButtonDown(0))
            if (_usedHold)
            {
                _animator.SetTrigger("Invalid");
                return;
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