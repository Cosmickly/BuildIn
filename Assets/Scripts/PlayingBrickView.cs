using Extensions;
using Records;
using UnityEngine;

/// <summary>
///     The BrickView game object.
/// </summary>
public class PlayingBrickView : BrickView
{
    [SerializeField] private ParticleSystem _breakEffectPrefab;
    private SpriteRenderer _spriteRend;

    private void Awake()
    {
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Break();
        }

        if (collision.gameObject.CompareTag("Boundary"))
        {
            // Break();
        }
    }

    /// <summary>
    ///     Starts the break sequence, playing break animation
    /// </summary>
    private void Break()
    {
        Instantiate(_breakEffectPrefab, transform.position, transform.rotation);
    }

    /// <inheritdoc/>
    public override void ApplyBrickState(BrickState state)
    {
        if (!state.Active)
        {
            Break();
            return;
        }

        _spriteRend.sprite = state.Sprite;
        var main = _breakEffectPrefab.main;
        main.startColor = new ParticleSystem.MinMaxGradient(state.BrickColor.ToColor());
    }
}