using UnityEngine;

/// <summary>
///     The ProtoBrick game object, a child object of the <see cref="Brick"/>.
/// </summary>
public class ProtoBrick : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void SetColor(Color32 color)
    {
        var main = _particleSystem.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color);
    }
}