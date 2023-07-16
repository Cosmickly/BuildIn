using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    private BrickManager brickManager;
    private GameManager gameManager;
    private Animator animator;

    public bool hasBrick = false;
    public bool viewHighlight = false;
    private Vector3 origin; 

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start() {
        brickManager = GetComponentInParent<BrickManager>();
        gameManager = brickManager.gameManager;
        origin = transform.position;
    }

    // Update is called once per frame
    private void Update() {
        animator.SetBool("HasBrick", hasBrick);
        transform.position = origin;
    }

    private void OnMouseOver() {
        if (!gameManager.playing) return;

        animator.SetBool("Highlight", true);

        if (Input.GetMouseButtonDown(0)) {
            if (!hasBrick) {
                hasBrick = brickManager.AddTopBrick(transform.position);

            } else {
                animator.SetTrigger("Invalid");
            }
        }
    }

    private void OnMouseExit() {
        animator.SetBool("Highlight", false);
    }

    public void ClearOverlay() {
        hasBrick = false;
    }
}
