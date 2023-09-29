using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public GameManager gameManager;

    public Brick brickPrefab;
    public Overlay overlayPrefab;
    public Vector2 offset;
    public Vector2Int gridSize;

    private Dictionary<Vector3, Brick> activeBricks = new();
    private Dictionary<Vector3, Brick> topBricks = new();
    private Dictionary<Vector3, Overlay> overlays = new();
    private Overlay selectedOverlay;
    public string info;

    private List<Brick> brickStack = new();
    private Brick heldBrick;
    public BrickHold brickHold;

    public List<Sprite> colors = new(3);

    private void OnValidate() {
        gridSize.x = Mathf.Clamp(gridSize.x, 0, 7);
        gridSize.y = Mathf.Clamp(gridSize.y, 0, 6);
    }

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < gridSize.x; i++) {
            CreateOverlay(transform.position + new Vector3((float)((gridSize.x - 1) * .5f - i) * offset.x, 0, 0));

            for (int j = 0; j < gridSize.y; j++) {
                Brick newBrick = CreateBrick(transform.position + new Vector3((float)((gridSize.x - 1) * .5f - i) * offset.x, -(j * offset.y) - 1, 0));
                AddActiveBrick(newBrick);
            }
        }

        for (int i = 0; i < 5; i++) {
            Brick newBrick = CreateBrick(transform.position + new Vector3(6.5f, -(i * offset.y)-1, 0));
            brickStack.Add(newBrick);
        }
    }

    private void Update() {
        //info = "Selected: " + selectedOverlay.transform.localPosition.ToString();

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            SelectOverlay(selectedOverlay.transform.position - new Vector3(offset.x,0,0));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            SelectOverlay(selectedOverlay.transform.position + new Vector3(offset.x, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space) && !selectedOverlay.hasBrick) {
            selectedOverlay.hasBrick = AddTopBrick(selectedOverlay.transform.position);
        }
    }

    // 0 = first
    private void UpdateBrickStack() {
        if (heldBrick) {
            heldBrick.transform.position = brickHold.transform.position;
        }

        for (int i = 0; i < brickStack.Count; i++) {
            brickStack[i].transform.position = transform.position + new Vector3(6.5f, -(i * offset.y)-1, 0);
        }       
    }

    private Brick CreateBrick(Vector3 pos) {
        Brick newBrick = Instantiate(brickPrefab, transform);
        newBrick.transform.position = pos;
        return newBrick;
    }

    public void AddActiveBrick(Brick brick) {
        if (!activeBricks.ContainsKey(brick.transform.position)) {
            activeBricks.Add(brick.transform.position, brick);
        }
    }

    public void RemoveActiveBrick(Vector3 pos) {
        if (activeBricks.ContainsKey(pos)) {
            activeBricks.Remove(pos);
        }

        if (activeBricks.Count == 0) {
            gameManager.LoseGame();
        }
    }

    public bool SelectOverlay(Vector3 pos) {
        if(overlays.TryGetValue(pos, out Overlay newOverlay) && newOverlay != selectedOverlay) {
            if (selectedOverlay) selectedOverlay.ToggleHighlight(false);
            selectedOverlay = newOverlay;
            selectedOverlay.ToggleHighlight(true);
            return true;
        }

        return false;
    }

    //public void UnselectOverlay() {
    //    selectedOverlay.toggleHighlight(false);
    //    //selectedOverlay = null;
    //}

    public bool AddTopBrick(Vector3 pos) {
        if (topBricks.ContainsKey(pos)) {
            return false;
        }

        Brick newBrick = brickStack[0];
        newBrick.transform.position = pos;
        newBrick.Activate(false);
        topBricks.Add(pos, newBrick);

        brickStack.RemoveAt(0);
        brickStack.Add(CreateBrick(Vector3.zero));
        brickHold.UsedHold(false);

        UpdateBrickStack();

        if (CheckTopBricks(pos)) {
            return false;
        }

        if (topBricks.Count == gridSize.x) {
            ShiftRow();
            return false;
        }
        return true;
    }

    private void RemoveTopBrick(Vector3 pos) {
        if (topBricks.TryGetValue(pos, out Brick brick)) {
            brick.DestroySelf();
            topBricks.Remove(pos);
        }

        if (overlays.TryGetValue(pos, out Overlay overlay)) {
            //Debug.Log("Removing Overlay");
            overlay.ClearOverlay();
        }
    }

    public void HoldBrick() {
        if (heldBrick) {
            var tempBrick = heldBrick;
            heldBrick = brickStack[0];
            brickStack.RemoveAt(0);
            brickStack.Insert(0, tempBrick);
        }
        else {
            heldBrick = brickStack[0];
            brickStack.RemoveAt(0);
            brickStack.Add(CreateBrick(Vector3.zero));
        }

        brickHold.UsedHold(true);
        UpdateBrickStack();
    }

    public void ShiftRow() {
        foreach (var overlay in overlays) {
            overlay.Value.ClearOverlay();
        }

        // Move active bricks to temp
        List<Brick> temp = new();

        foreach (var brick in activeBricks) {
            temp.Add(brick.Value);
        }
        activeBricks.Clear();

        foreach (Brick brick in temp) {
            brick.transform.position += new Vector3(0, -offset.y, 0);
            AddActiveBrick(brick);
        }
        temp.Clear();

        // Move top bricks
        foreach (var brick in topBricks) {
            temp.Add(brick.Value);
        }
        topBricks.Clear();

        foreach (Brick brick in temp) {
            brick.transform.position += new Vector3(0,-1,0);
            brick.Activate(true);
            AddActiveBrick(brick);
        }

    }

    private void CreateOverlay(Vector3 pos) {
        //Debug.Log("Creating overlay at " + pos);
        Overlay newOverlay = Instantiate(overlayPrefab, transform);
        newOverlay.transform.localScale = offset;
        newOverlay.transform.position = pos;
        overlays.Add(pos, newOverlay);
        SelectOverlay(pos);
    }

    private bool CheckTopBricks(Vector3 pos) {
        bool removed = false;
        if (topBricks.TryGetValue(pos, out Brick brick)) {
            //Debug.Log("Checking");
            if (topBricks.TryGetValue(pos + new Vector3(-offset.x,0,0), out Brick left) && left.spriteId == brick.spriteId) {
                RemoveTopBrick(left.transform.position);
                RemoveTopBrick(brick.transform.position);
                removed = true;
            }

            if (topBricks.TryGetValue(pos + new Vector3(offset.x, 0, 0), out Brick right) && right.spriteId == brick.spriteId) {
                RemoveTopBrick(right.transform.position);
                RemoveTopBrick(brick.transform.position);
                removed = true;
            }
        }

        return removed;
    }
}
