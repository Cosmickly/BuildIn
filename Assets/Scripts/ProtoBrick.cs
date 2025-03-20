using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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