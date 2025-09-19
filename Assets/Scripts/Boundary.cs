using System;
using Managers;
using UnityEngine;

[Obsolete("Ball handles out of bounds checks")]
public class Boundary : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    //TODO: crash when respawning ball during block placement
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}