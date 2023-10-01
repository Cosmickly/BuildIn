using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float pauseLength = 0.1f;
    private float pauseTimer = 0;


    private GameManager gameManager;
    public BallMovement ball;
    private Vector3 ballDistance;

    private void Start() {
        gameManager = GetComponentInParent<GameManager>();
        ball = gameManager.ball;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseTimer <= 0 && gameManager.playing) {
            ballDistance = ball.transform.position - transform.position;

            if (Mathf.Abs(ballDistance.magnitude) >= .75f) {
                Vector3 newPosition = transform.position + (Vector3.right * ballDistance.x * maxSpeed * Time.deltaTime);
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
    }
}
