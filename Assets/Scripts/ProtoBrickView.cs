using Extensions;
using Records;
using UnityEngine;

/// <summary>
///     The ProtoBrickView game object.
/// </summary>
public class ProtoBrickView : BrickView
{
    private ParticleSystem _particleSystem;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <inheritdoc/>
    public override void ApplyBrickState(BrickState state)
    {
        Debug.Log("Setting proto brick sprite color to " + state.BrickColor);
        _spriteRenderer.enabled = state.Active;
        var main = _particleSystem.main;
        main.startColor = new ParticleSystem.MinMaxGradient(state.BrickColor.ToColor());

        if (state.Active)
        {
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Stop();
        }
    }
}