using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickHold : MonoBehaviour
{
    //private SpriteRenderer spriteRenderer;
    private BrickManager brickManager;
    private GameManager gameManager;
    private Animator animator;
    private bool usedHold = false;
    private Vector3 origin;


    private void Awake() {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        brickManager = GetComponentInParent<BrickManager>();
        gameManager = brickManager.gameManager;
        origin = transform.position;
    }

    private void Update() {
        transform.position = origin;
    }

    private void OnMouseOver() {
        if (!gameManager.playing) return;

        animator.SetBool("Highlight", true);

        if (Input.GetMouseButtonDown(0)) {
            if (usedHold) {
                animator.SetTrigger("Invalid");
                return;
            }
            brickManager.HoldBrick();
        }
    }

    private void OnMouseExit() {
        animator.SetBool("Highlight", false);
    }

    public void UsedHold(bool used) {
        usedHold = used;
        animator.SetBool("UsedHold", used);
    }

}
