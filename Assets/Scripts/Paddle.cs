using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _pauseLength = 0.1f;
    private float _pauseTimer;

    private GameManager _gameManager;
    private Ball _ball;

    [SerializeField] private ParticleSystem _breakEffectPrefab;

    private void Start()
    {
        _gameManager = GetComponentInParent<GameManager>();
        _ball = _gameManager.Ball;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_pauseTimer <= 0 && _gameManager.Playing)
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
        }
    }

    public void BreakPaddle()
    {
        var main = _breakEffectPrefab.main;
        main.startColor = new ParticleSystem.MinMaxGradient(new Color32(131, 118, 156, 255));
        Instantiate(_breakEffectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}