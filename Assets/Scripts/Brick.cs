using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer _spriteRend;
    private BrickManager _brickManager;
    [SerializeField] private ParticleSystem _breakEffectPrefab;
    public int SpriteId;

    private ProtoBrick _protoBrick;

    private void Awake()
    {
        _spriteRend = GetComponent<SpriteRenderer>();
        _protoBrick = GetComponentInChildren<ProtoBrick>();
    }

    private void Start()
    {
        _brickManager = GetComponentInParent<BrickManager>();
        SpriteId = Random.Range(0, _brickManager.BrickSprites.Count);
        _spriteRend.sprite = _brickManager.BrickSprites[SpriteId];

        _protoBrick.SetColor(_brickManager.BrickColours[SpriteId]);
        ToggleProtoBrick(false);
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

    public void DestroyBrick()
    {
        Destroy(gameObject);
    }

    private void Break()
    {
        var main = _breakEffectPrefab.main;
        main.startColor = new ParticleSystem.MinMaxGradient(_brickManager.BrickColours[SpriteId]);
        Instantiate(_breakEffectPrefab, transform.position, transform.rotation);
        _brickManager.RemoveActiveBrick(transform.position);
        Destroy(gameObject);
    }

    public void ToggleProtoBrick(bool active)
    {
        _spriteRend.enabled = !active;
        _protoBrick.gameObject.SetActive(active);
    }
}