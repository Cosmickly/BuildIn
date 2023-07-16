using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BrickManager brickManager;
    public Color col;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        brickManager = GetComponentInParent<BrickManager>();
        col = brickManager.colors[Random.Range(0, brickManager.colors.Count)];
        sprite.color = col;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            brickManager.RemoveActiveBrick(transform.position);
            Destroy(gameObject);
        }
    }

    public void Activate(bool active) {
        if (active) {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        } else {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        }
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    //public void Move(Vector3 distance) {
    //    transform.position = transform.position + distance;
    //}
}
