using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public GameManager gameManager;

    //TODO: crash when respawning ball during block placement
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            StartCoroutine(gameManager.RespawnBall());
        }
    }
}