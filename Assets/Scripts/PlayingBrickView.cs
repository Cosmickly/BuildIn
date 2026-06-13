using Extensions;
using Records;
using UnityEngine;

/// <summary>
///     The BrickView game object.
/// </summary>
public class PlayingBrickView : BrickView
{
    private ParticleSystem _breakEffectPrefab;
    private SpriteRenderer _spriteRend;
    private Collider2D _collider2D;

    private void Awake()
    {
        _breakEffectPrefab = GetComponent<ParticleSystem>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            _spriteRend.enabled = false;
            _collider2D.enabled = false;
            PlayBreakEffect();
        }
    }
    
    // Plays the Brick break effect
    private void PlayBreakEffect()
    {
        Debug.Log("Break: " + transform.position);
        _breakEffectPrefab.Play();
    }

    /// <inheritdoc/>
    public override void ApplyBrickState(BrickState state)
    {
        _spriteRend.enabled = state.Active;
        _collider2D.enabled = state.Active;

        _spriteRend.sprite = state.Sprite;
        var main = _breakEffectPrefab.main;
        main.startColor = new ParticleSystem.MinMaxGradient(state.BrickColor.ToColor());
    }
}