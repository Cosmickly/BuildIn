using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector3 _direction;
    [SerializeField] private float _startSpeed = 7;
    private float _speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _speed = _startSpeed;
        RandomDirection();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            _speed = _startSpeed;
            return;
        }

        Vector3 newDirection = Vector3.Reflect(_direction, collision.GetContact(0).normal);

        if (collision.gameObject.CompareTag("Paddle"))
        {
            Vector3 offset = transform.position - collision.transform.position;
            newDirection += new Vector3(offset.x, 0, 0);
        }

        _direction = newDirection.normalized;
        _speed += 0.04f;
    }

    public void DestroyBall()
    {
        Destroy(gameObject);
    }

    public void RandomDirection()
    {
        _direction = new Vector3(Random.Range(-1f, 1f), -1, 0);
    }
}