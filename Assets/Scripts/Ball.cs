using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rb;
    private ParticleSystem _breakEffect;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private Vector3 _direction;
    private float _speed;
    [SerializeField] private float _startSpeed = 7;
    [SerializeField] private Vector3 _respawnPoint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _breakEffect = GetComponent<ParticleSystem>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        transform.position = _respawnPoint;
        RandomDirection();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction * _speed;
    }

    public void ToggleBall(bool active)
    {
        _spriteRenderer.enabled = active;
        _collider.enabled = active;
        _speed = active ? _startSpeed : 0f;
    }

    private void RandomDirection()
    {
        _direction = new Vector3(Random.Range(-1f, 1f), -1, 0);
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Boundary"))
        {
            StartCoroutine(DestroyAndRespawn());
        }

        Vector3 newDirection = Vector3.Reflect(_direction, other.GetContact(0).normal);

        if (other.gameObject.CompareTag("Paddle"))
        {
            Vector3 offset = transform.position - other.transform.position;
            newDirection += new Vector3(offset.x, 0, 0);
        }

        _direction = newDirection.normalized;
        _speed += 0.04f;
    }

    private IEnumerator DestroyAndRespawn()
    {
        _breakEffect.Play();
        ToggleBall(false);

        yield return new WaitForSeconds(2);

        transform.position = _respawnPoint;
        RandomDirection();
        ToggleBall(true);
    }
}