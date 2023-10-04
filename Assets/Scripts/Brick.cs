using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private BrickManager brickManager;
    public ParticleSystem breakEffect;
    public int spriteId;
    public bool touchingFloor = false;

    List<Color> colours = new() {
            new Color32(255,0,77,255),
            new Color32(41,173,255,255),
            new Color32(255,236,39,255)
    };

    private void Awake() {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        brickManager = GetComponentInParent<BrickManager>();
        spriteId = Random.Range(0, brickManager.brickSprites.Count);
        spriteRend.sprite = brickManager.brickSprites[spriteId];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            Break();
        }

        if (collision.gameObject.CompareTag("Boundary")) {
            Debug.Log("Brick hit boundary");
            Break();
        }
    }

    public void Activate(bool active) {
        if (active) {
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
        } else {
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 0.5f);
        }
    }

    public void DestroyBrick() {
        Destroy(gameObject);
    }

    private void Break() {
        brickManager.RemoveActiveBrick(transform.position);
        var main = breakEffect.main;
        main.startColor = new ParticleSystem.MinMaxGradient(colours[spriteId]);
        Instantiate(breakEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    } 

}
