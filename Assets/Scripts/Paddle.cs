using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _pauseLength = 0.1f;
    private float _pauseTimer;

    private GameManager _gameManager;
    [SerializeField] private Ball _ball;

    private ParticleSystem _breakEffect;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _breakEffect = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        _gameManager = GetComponentInParent<GameManager>();
        _ball = _gameManager.GetBall();
    }

    private void Update()
    {
        if (_pauseTimer <= 0)
        {
            Vector3 ballDistance = _ball.transform.position - transform.position;

            if (Mathf.Abs(ballDistance.magnitude) >= .75f)
            {
                Vector3 newPosition = transform.position + (ballDistance.x * _maxSpeed * Time.deltaTime * Vector3.right);
                newPosition.x = Mathf.Clamp(newPosition.x, -4.25f, 4.25f);
                transform.position = newPosition;
            }
        }
        else
        {
            _pauseTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            _pauseTimer = _pauseLength;
        }

        if (collision.gameObject.CompareTag("Brick"))
        {
            _gameManager.WinGame();
            BreakPaddle();
        }
    }

    private void BreakPaddle()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _breakEffect.Play();
    }

    public void SpawnPaddle()
    {
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
    }
}