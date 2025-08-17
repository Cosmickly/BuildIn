using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
///     The Brick game object.
/// </summary>
public class Brick : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRend;
    private BrickManager _brickManager;
    [SerializeField] private ParticleSystem _breakEffectPrefab;
    public int SpriteId;

    private ProtoBrick _protoBrick;

    private Vector3? _target;
    private bool _merge;
    private float _shakeIntensity = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _protoBrick = GetComponentInChildren<ProtoBrick>();
    }

    private void Start()
    {
        _brickManager = GetComponentInParent<BrickManager>();
        SpriteId = Random.Range(0, _brickManager.BrickSprites.Count);
        _spriteRend.sprite = _brickManager.BrickSprites[SpriteId];

        _protoBrick.SetColor(_brickManager.BrickColours[SpriteId]);
        SetProtoBrickActive(false);
    }

    private void FixedUpdate()
    {
        var distance = _target - transform.position;
        while (_merge && distance.HasValue)
        {
            _shakeIntensity += Time.deltaTime; // todo CAUSES A CRASH
            _rb.AddForce(distance.Value.normalized * (_rb.mass * 10));
        }

        if (distance?.magnitude < 0.1f)
        {
            Break();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Break();
        }

        if (collision.gameObject.CompareTag("Boundary"))
        {
            Debug.Log("Brick hit boundary");
            Break();
        }
    }

    public void MergeBrick(Vector3 target)
    {
        _target = target;
        _merge = true;
    }

    /// <summary>
    ///     Breaks the Brick.
    /// </summary>
    private void Break()
    {
        _merge = false;
        var main = _breakEffectPrefab.main;
        main.startColor = new ParticleSystem.MinMaxGradient(_brickManager.BrickColours[SpriteId]);
        Instantiate(_breakEffectPrefab, transform.position, transform.rotation);
        _brickManager.RemoveActiveBrick(transform.position);
        Destroy(gameObject);
    }


    public void SetProtoBrickActive(bool active)
    {
        _spriteRend.enabled = !active;
        _protoBrick.gameObject.SetActive(active);
    }
}