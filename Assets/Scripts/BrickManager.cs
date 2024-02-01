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
    public Vector3 brickStackPos = new Vector3(7.5f,-2,0);
    public float brickStackSize = 1; //physical size of stack
    public int brickStackCount = 5; //number of bricks in stack

    private Brick heldBrick;
    private BrickHold brickHold;
    public bool activeBrickHold;

    public List<Sprite> brickSprites = new(3);
    public List<Color> brickColours = new();

    private void OnValidate() {
        gridSize.x = Mathf.Clamp(gridSize.x, 0, 8);
        gridSize.y = Mathf.Clamp(gridSize.y, 0, 6);
    }

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start() {
        brickColours.Add(gameManager.pico8Palette["red"]);
        brickColours.Add(gameManager.pico8Palette["blue"]);
        brickColours.Add(gameManager.pico8Palette["yellow"]);

        brickHold = GetComponentInChildren<BrickHold>(true);
        brickHold.gameObject.SetActive(activeBrickHold);

        for (int i = 0; i < gridSize.x; i++) {
            CreateOverlay(transform.position + new Vector3((float)((gridSize.x - 1) * .5f - i) * offset.x, 0, 0));

            for (int j = 0; j < gridSize.y; j++) {
                AddActiveBrick(CreateBrick(transform.position + new Vector3((float)((gridSize.x - 1) * .5f - i) * offset.x, -(j * offset.y) - 1, 0), 1));
            }
        }

        for (int i = 0; i < brickStackCount; i++) {
            brickStack.Add(CreateBrick(transform.position + brickStackPos + new Vector3(0, -i * brickStackSize * offset.y, 0), brickStackSize));
        }
    }

    private void Update() {
        //info = "Selected: " + selectedOverlay.transform.localPosition.ToString();
        if (gameManager.playing) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                CycleOverlay(new Vector3(-offset.x,0,0));
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                CycleOverlay(new Vector3(offset.x, 0, 0));
            }

            if (Input.GetKeyDown(KeyCode.Space) && !selectedOverlay.hasBrick) {
                selectedOverlay.hasBrick = AddTopBrick(selectedOverlay.transform.position);
            }                
        }            
    }

    // 0 = first
    private void UpdateBrickStack() {
        if (brickHold.enabled && heldBrick) {
            heldBrick.transform.position = brickHold.transform.position;
        }

        for (int i = 0; i < brickStack.Count; i++) {
            //brickStack[i].transform.position = transform.position + new Vector3(brickStackX, -(i * brickStackSize * offset.y) - 1, 0);
            brickStack[i].transform.position = transform.position + brickStackPos + new Vector3(0, -i * brickStackSize * offset.y, 0);
        }       
    }

    private Brick CreateBrick(Vector3 pos, float sizeMultiplier) {
        Brick newBrick = Instantiate(brickPrefab, transform);
        newBrick.transform.localScale *= sizeMultiplier;
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

    public void SelectOverlay(Overlay newOverlay) {
        if (selectedOverlay) selectedOverlay.ToggleHighlight(false);
        selectedOverlay = newOverlay;
        selectedOverlay.ToggleHighlight(true);
    }

    private void CycleOverlay(Vector3 overlayOffset) {
        Vector3 pos = selectedOverlay.transform.position;
        pos += overlayOffset;

        while (overlays.TryGetValue(pos, out Overlay newOverlay)) {
            Debug.Log("Trying overlay " + pos.ToString());

            if (newOverlay != selectedOverlay && !newOverlay.hasBrick) { 
                SelectOverlay(newOverlay);
                return;
            }
            pos += overlayOffset;
        }
    }

    public bool AddTopBrick(Vector3 pos) {
        if (topBricks.ContainsKey(pos)) {
            return false;
        }

        Brick newBrick = brickStack[0];
        newBrick.transform.position = pos;
        newBrick.transform.localScale = new Vector3(1, 1, 0);
        newBrick.Activate(false);
        topBricks.Add(pos, newBrick);

        brickStack.RemoveAt(0);
        brickStack.Add(CreateBrick(Vector3.zero, brickStackSize));

        if (brickHold.enabled) brickHold.UsedHold(false);

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
            topBricks.Remove(pos);
            brick.DestroyBrick();
        }

        if (overlays.TryGetValue(pos, out Overlay overlay)) {
            //Debug.Log("Removing Overlay");
            overlay.ClearOverlay();
        }
    }

    //TODO: Unsupported
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
            brickStack.Add(CreateBrick(Vector3.zero, brickStackSize));
        }

        if (brickHold.enabled) brickHold.UsedHold(true);

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

            if (brick.touchingFloor) gameManager.WinGame();
        }
    }

    private void CreateOverlay(Vector3 pos) {
        //Debug.Log("Creating overlay at " + pos);
        Overlay newOverlay = Instantiate(overlayPrefab, transform);
        newOverlay.transform.localScale = offset;
        newOverlay.transform.position = pos;
        overlays.Add(pos, newOverlay);
        SelectOverlay(newOverlay);
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
