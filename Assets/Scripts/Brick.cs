using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer _spriteRend;
    private BrickManager _brickManager;
    [SerializeField] private ParticleSystem _breakEffectPrefab;
    public int SpriteId;
    public bool TouchingFloor;

    private void Awake()
    {
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _brickManager = GetComponentInParent<BrickManager>();
        SpriteId = Random.Range(0, _brickManager.BrickSprites.Count);
        _spriteRend.sprite = _brickManager.BrickSprites[SpriteId];
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

    public void Activate(bool active)
    {
        _spriteRend.color = new Color(_spriteRend.color.r, _spriteRend.color.g, _spriteRend.color.b, active ? 1f : 0.5f);
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
}