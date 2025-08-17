using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
///     The game ball.
/// </summary>
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

    /// <summary>
    ///     Sets the ball active state.
    /// </summary>
    /// <param name="active"></param>
    public void SetBallActive(bool active)
    {
        _spriteRenderer.enabled = active;
        _collider.enabled = active;
        _speed = active ? _startSpeed : 0f;
    }

    /// <summary>
    ///     Sets the Ball direction to a random upwards value.
    /// </summary>
    private void RandomDirection()
    {
        _direction = new Vector3(Random.Range(-1f, 1f), -1, 0);
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Boundary"))
        {
            StartCoroutine(DisableAndRespawn());
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

    /// <summary>
    ///     Disables the Ball for 2 seconds, then respawans.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableAndRespawn()
    {
        _breakEffect.Play();
        SetBallActive(false);

        yield return new WaitForSeconds(2);

        transform.position = _respawnPoint;
        RandomDirection();
        SetBallActive(true);
    }
}