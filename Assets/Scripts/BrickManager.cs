using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public GameManager GameManager;

    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private Overlay _overlayPrefab;
    /// <summary>
    ///     Horizontal and Vertical distance between each brick
    /// </summary>
    [SerializeField] private Vector2 _offset = new(1, 0.5f);
    [SerializeField] private Vector2Int _gridSize = new(5, 4);

    private readonly Dictionary<Vector3, Brick> _activeBricks = new();
    private readonly Dictionary<Vector3, Brick> _topBricks = new();
    private readonly Dictionary<Vector3, Overlay> _overlays = new();
    private Overlay _selectedOverlay;

    private readonly List<Brick> _brickQueue = new();

    /// <summary>
    ///     Physical position of the queue
    /// </summary>
    [SerializeField] private Vector3 _brickQueuePos = new(7, -1, 0);

    /// <summary>
    ///     Physical size of the queue
    /// </summary>
    [SerializeField] private float _brickQueueSize = 1;

    /// <summary>
    ///     Number of bricks in the queue
    /// </summary>
    [SerializeField] private int _brickQueueCount = 5;

    private Brick _heldBrick;
    private BrickHold _brickHold;

    public List<Sprite> BrickSprites = new(3);
    public readonly List<Color> BrickColours = new()
    {
        new Color32(255, 0, 77, 255),
        new Color32(41, 173, 255, 255),
        new Color32(255, 236, 39, 255)
    };

    private void OnValidate()
    {
        _gridSize.x = Mathf.Clamp(_gridSize.x, 0, 8);
        _gridSize.y = Mathf.Clamp(_gridSize.y, 0, 6);
    }

    private void Start()
    {
        _brickHold = GetComponentInChildren<BrickHold>(true);

        // Creates Overlays and active bricks
        for (int i = 0; i < _gridSize.x; i++)
        {
            CreateOverlay(transform.position + new Vector3(((_gridSize.x - 1) * .5f - i) * _offset.x, 0, 0));

            for (int j = 0; j < _gridSize.y; j++)
            {
                AddActiveBrick(CreateBrick(transform.position + new Vector3(((_gridSize.x - 1) * .5f - i) * _offset.x, -(j * _offset.y) - 1, 0), 1));
            }
        }

        for (int i = 0; i < _brickQueueCount; i++)
        {
            _brickQueue.Add(CreateBrick(transform.position + _brickQueuePos + new Vector3(0, -i * _brickQueueSize * _offset.y, 0), _brickQueueSize));
        }
    }

    private void Update()
    {
        if (!GameManager.Playing) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelectedOverlay(new Vector3(-_offset.x, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelectedOverlay(new Vector3(_offset.x, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space) && _selectedOverlay.AddBrickToOverlay())
        {
            AddTopBrick(_selectedOverlay.transform.position);
        }
    }

    /// <summary>
    ///     Updates the position of each brick in the brick queue.
    /// </summary>
    // 0 = first
    private void UpdateBrickQueue()
    {
        if (_brickHold.isActiveAndEnabled && _heldBrick)
        {
            _heldBrick.transform.position = _brickHold.transform.position;
        }

        for (int i = 0; i < _brickQueue.Count; i++)
        {
            _brickQueue[i].transform.position = transform.position + _brickQueuePos + new Vector3(0, -i * _brickQueueSize * _offset.y, 0);
        }
    }

    /// <summary>
    ///     Instantiates a new <see cref="Brick"/>.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="sizeMultiplier"></param>
    /// <returns></returns>
    private Brick CreateBrick(Vector3 pos, float sizeMultiplier)
    {
        Brick newBrick = Instantiate(_brickPrefab, transform);
        newBrick.transform.localScale *= sizeMultiplier;
        newBrick.transform.position = pos;
        return newBrick;
    }

    /// <summary>
    ///     Adds a <see cref="Brick"/> to the set of active Bricks.
    /// </summary>
    /// <param name="brick"></param>
    private void AddActiveBrick(Brick brick)
    {
        _activeBricks.TryAdd(brick.transform.position, brick);
    }

    /// <summary>
    ///     Removes a <see cref="Brick"/> from the set of active Bricks, and triggers game loss if active Bricks is empty.
    /// </summary>
    /// <param name="pos"></param>
    public void RemoveActiveBrick(Vector3 pos)
    {
        if (_activeBricks.ContainsKey(pos))
        {
            _activeBricks.Remove(pos);
        }

        if (_activeBricks.Count == 0)
        {
            GameManager.LoseGame();
        }
    }

    /// <summary>
    ///     Changes which <see cref="Overlay"/> is selected.
    /// </summary>
    /// <param name="newOverlay"></param>
    public void SelectOverlay(Overlay newOverlay)
    {
        if (_selectedOverlay)
        {
            _selectedOverlay.ToggleHighlight(false);
        }
        _selectedOverlay = newOverlay;
        _selectedOverlay.ToggleHighlight(true);
    }

    /// <summary>
    ///     Moves to a new <see cref="Overlay"/>, offset from the currently selected Overlay.
    /// </summary>
    /// <param name="overlayOffset">Distance to move left or right</param>
    private void MoveSelectedOverlay(Vector3 overlayOffset)
    {
        Vector3 pos = _selectedOverlay.transform.position; // TODO: Change this to select overlays discretly, not with floating position values
        pos += overlayOffset;

        while (_overlays.TryGetValue(pos, out Overlay newOverlay))
        {
            if (newOverlay != _selectedOverlay && !_topBricks.ContainsKey(pos))
            {
                SelectOverlay(newOverlay);
                return;
            }

            pos += overlayOffset;
        }
    }

    /// <summary>
    ///     Dequeues a <see cref="Brick"/> off the <see cref="_brickQueue"/> and adds it to the top row.
    ///     Creates a new Brick in the brickQueue.
    /// </summary>
    /// <param name="pos">Overlay position to add a new brick to.</param>
    /// <returns>Boolean on if a brick was added to top row.</returns>
    public bool AddTopBrick(Vector3 pos)
    {
        if (_topBricks.ContainsKey(pos))
        {
            return false;
        }

        Brick newBrick = _brickQueue[0];
        newBrick.transform.position = pos;
        newBrick.transform.localScale = new Vector3(1, 1, 0);
        newBrick.ToggleProtoBrick(true);
        _topBricks.Add(pos, newBrick);

        _brickQueue.RemoveAt(0);
        _brickQueue.Add(CreateBrick(Vector3.zero, _brickQueueSize));

        if (_brickHold.isActiveAndEnabled)
        {
            _brickHold.UsedHold(false);
        }

        UpdateBrickQueue();

        // If a brick was merged, we don't need to check for a row shift.
        if (CheckTopBricks(pos))
        {
            return false;
        }

        if (_topBricks.Count == _gridSize.x)
        {
            ShiftRow();
        }

        return true;
    }

    /// <summary>
    ///     Destroys a <see cref="Brick"/> in the top row and clears the overlay.
    /// </summary>
    /// <param name="pos">Position of the brick.</param>
    /// <param name="target">Where the brick should merge to.</param>
    private void MergeTopBrick(Vector3 pos, Vector3 target)
    {
        if (_topBricks.Remove(pos, out Brick brick))
        {
            brick.MergeBrick(target);
        }

        if (_overlays.TryGetValue(pos, out Overlay overlay))
        {
            overlay.RemoveBrickFromOverlay();
        }
    }

    //TODO: Unsupported
    public void HoldBrick()
    {
        if (!_brickHold.isActiveAndEnabled) return;

        if (_heldBrick)
        {
            var tempBrick = _heldBrick;
            _heldBrick = _brickQueue[0];
            _brickQueue.RemoveAt(0);
            _brickQueue.Insert(0, tempBrick);
        }
        else
        {
            _heldBrick = _brickQueue[0];
            _brickQueue.RemoveAt(0);
            _brickQueue.Add(CreateBrick(Vector3.zero, _brickQueueSize));
        }

        _brickHold.UsedHold(true);

        UpdateBrickQueue();
    }

    /// <summary>
    ///     Shifts all rows of active and top <see cref="Brick"/>s down one unit.
    /// </summary>
    private void ShiftRow()
    {
        // Clear the full row
        foreach (var overlay in _overlays)
        {
            overlay.Value.RemoveBrickFromOverlay();
        }

        // Move active bricks to temp
        List<Brick> temp = new();

        foreach (var brick in _activeBricks)
        {
            temp.Add(brick.Value);
        }

        _activeBricks.Clear();

        foreach (Brick brick in temp)
        {
            brick.transform.position += new Vector3(0, -_offset.y, 0);
            AddActiveBrick(brick);
        }

        temp.Clear();

        // Move top bricks
        temp.AddRange(_topBricks.Select(brick => brick.Value));

        _topBricks.Clear();

        foreach (Brick brick in temp)
        {
            brick.transform.position += new Vector3(0, -1, 0);
            brick.ToggleProtoBrick(false);
            AddActiveBrick(brick);
        }
    }

    /// <summary>
    ///     Creates an <see cref="Overlay"/>.
    /// </summary>
    /// <param name="pos"></param>
    private void CreateOverlay(Vector3 pos)
    {
        Overlay newOverlay = Instantiate(_overlayPrefab, transform);

        newOverlay.transform.localScale = _offset;
        newOverlay.transform.position = pos;

        _overlays.Add(pos, newOverlay);

        SelectOverlay(newOverlay);
    }

    /// <summary>
    ///     Checks top <see cref="Brick"/>s for matching colors, and removes them.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Boolean on if a <see cref="Brick"/> was removed.</returns>
    private bool CheckTopBricks(Vector3 pos)
    {
        var bricksToRemove = new List<Vector3>();

        if (_topBricks.TryGetValue(pos, out Brick brick))
        {
            if (_topBricks.TryGetValue(pos + new Vector3(-_offset.x, 0, 0), out Brick left) && left.SpriteId == brick.SpriteId)
            {
                bricksToRemove.Add(left.transform.position);
            }

            if (_topBricks.TryGetValue(pos + new Vector3(_offset.x, 0, 0), out Brick right) && right.SpriteId == brick.SpriteId)
            {
                bricksToRemove.Add(right.transform.position);
            }
        }

        if (bricksToRemove.Count != 0)
        {
            bricksToRemove.Add(pos);
            var target = new Vector3(
                bricksToRemove.Average(b => b.x),
                bricksToRemove.Average(b => b.y),
                bricksToRemove.Average(b => b.z));

            foreach (var b in bricksToRemove)
            {
                MergeTopBrick(b, target);
            }
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Converts a
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    private Vector3 gridToWorld(Vector2Int gridPos)
    {
        return gameObject.transform.position;
    }
}