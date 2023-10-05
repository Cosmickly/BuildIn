using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 direction;
    public float startSpeed = 7;
    private float speed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = startSpeed;
        RandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Boundary")) {
            speed = startSpeed;
            return;
        }

        Vector3 newDirection = Vector3.Reflect(direction, collision.GetContact(0).normal);

        if (collision.gameObject.CompareTag("Paddle")) {
            Vector3 offset = transform.position - collision.transform.position;
            newDirection += new Vector3(offset.x, 0, 0);
        }

        direction = newDirection.normalized;
        speed += 0.04f;
    }

    public void DestroyBall() {
        Destroy(gameObject);
    }

    public void RandomDirection() {
        direction = new Vector3(Random.Range(-1f, 1f), -1, 0);
    }
}
