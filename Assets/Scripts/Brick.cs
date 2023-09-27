using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private BrickManager brickManager;
    public int spriteId;

    private void Awake() {
        spriteRend = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        brickManager = GetComponentInParent<BrickManager>();
        spriteId = Random.Range(0, brickManager.colors.Count);
        spriteRend.sprite = brickManager.colors[spriteId];
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
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
        } else {
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 0.5f);
        }
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    //public void Move(Vector3 distance) {
    //    transform.position = transform.position + distance;
    //}
}
