using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public GameManager gameManager;
    public Vector3 respawnPoint;

    //TODO: crash when respawning ball during block placement
    private IEnumerator OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            collision.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            collision.gameObject.transform.position = respawnPoint;
            collision.gameObject.SetActive(true);
        }
    }
}