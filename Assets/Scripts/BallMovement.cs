using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 direction;
    [SerializeField] private float speed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(Random.Range(-1f,1f),-1,0);
        //ballDistance = new Vector3(0,-1,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        // Get the new vector from the colliders normal
        Vector3 newDirection = Vector3.Reflect(direction, collision.GetContact(0).normal);
        //Debug.Log("New ballDistance pre: " + newDirection);

        // Set rotation and ballDistance

        //if (collision.gameObject.CompareTag("Paddle")) {
        //    Vector3 offset = transform.position - collision.gameObject.transform.position;
        //    Debug.Log("offset:" + offset);
        //    newDirection.x = Mathf.Clamp(offset.x/2 + newDirection.x, -1,1);
        //    //newDirection /= Mathf.Max(newDirection.x, Mathf.Max(newDirection.y, newDirection.z));
        //    Debug.Log("New ballDistance post: " + newDirection);

        //}


        //rb.SetRotation(Quaternion.LookRotation(Vector3.forward, newDirection));
        //rb.velocity = newDirection;

        direction = newDirection.normalized;
        speed += 0.05f;
    }
}
