using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float pauseLength = 0.1f;
    private float pauseTimer = 0;

    private GameManager gameManager;
    private Ball ball;

    public ParticleSystem breakEffect;

    private void Start() {
        gameManager = GetComponentInParent<GameManager>();
        ball = gameManager.ball;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseTimer <= 0 && gameManager.playing) {
            Vector3 ballDistance = ball.transform.position - transform.position;

            if (Mathf.Abs(ballDistance.magnitude) >= .75f) {
                Vector3 newPosition = transform.position + (ballDistance.x * maxSpeed * Time.deltaTime * Vector3.right);
                newPosition.x = Mathf.Clamp(newPosition.x, -4.25f, 4.25f);
                transform.position = newPosition;
            }
        }
        else {
            pauseTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            pauseTimer = pauseLength;
        }

        if (collision.gameObject.CompareTag("Brick")) {
            Debug.Log("paddle hit brick");
            gameManager.WinGame();
        }
    }

    public void BreakPaddle() {
        var main = breakEffect.main;
        main.startColor = new ParticleSystem.MinMaxGradient(gameManager.pico8Palette["lavender"]);
        Instantiate(breakEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
