using Records;
using UnityEngine;

/// <summary>
///     The ProtoBrickView game object.
/// </summary>
public class ProtoBrickView : BrickView
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    /// <inheritdoc/>
    public override void ApplyBrickState(BrickState state)
    {
        var main = _particleSystem.main;
        main.startColor = new ParticleSystem.MinMaxGradient(state.SpriteColor);
    }
}