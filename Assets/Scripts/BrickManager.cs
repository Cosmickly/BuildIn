using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public GameManager GameManager;

    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private Overlay _overlayPrefab;
    [SerializeField] private Vector2 _offset = new(1, 0.5f);
    [SerializeField] private Vector2Int _gridSize = new(5, 4);

    private readonly Dictionary<Vector3, Brick> _activeBricks = new();
    private readonly Dictionary<Vector3, Brick> _topBricks = new();
    private readonly Dictionary<Vector3, Overlay> _overlays = new();
    private Overlay _selectedOverlay;

    private readonly List<Brick> _brickStack = new();
    [SerializeField] private Vector3 _brickStackPos = new(7, -1, 0);
    [SerializeField] private float _brickStackSize = 1; //physical size of stack
    [SerializeField] private int _brickStackCount = 5; //number of bricks in stack

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

        for (int i = 0; i < _gridSize.x; i++)
        {
            CreateOverlay(transform.position + new Vector3(((_gridSize.x - 1) * .5f - i) * _offset.x, 0, 0));

            for (int j = 0; j < _gridSize.y; j++)
            {
                AddActiveBrick(CreateBrick(transform.position + new Vector3(((_gridSize.x - 1) * .5f - i) * _offset.x, -(j * _offset.y) - 1, 0), 1));
            }
        }

        for (int i = 0; i < _brickStackCount; i++)
        {
            _brickStack.Add(CreateBrick(transform.position + _brickStackPos + new Vector3(0, -i * _brickStackSize * _offset.y, 0), _brickStackSize));
        }
    }

    private void Update()
    {
        if (!GameManager.Playing) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CycleOverlay(new Vector3(-_offset.x,0,0));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CycleOverlay(new Vector3(_offset.x, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space) && !_selectedOverlay.HasBrick)
        {
            _selectedOverlay.HasBrick = AddTopBrick(_selectedOverlay.transform.position);
        }
    }

    // 0 = first
    private void UpdateBrickStack()
    {
        if (_brickHold.isActiveAndEnabled && _heldBrick)
        {
            _heldBrick.transform.position = _brickHold.transform.position;
        }

        for (int i = 0; i < _brickStack.Count; i++)
        {
            //brickStack[i].transform.position = transform.position + new Vector3(brickStackX, -(i * brickStackSize * offset.y) - 1, 0);
            _brickStack[i].transform.position = transform.position + _brickStackPos + new Vector3(0, -i * _brickStackSize * _offset.y, 0);
        }
    }

    private Brick CreateBrick(Vector3 pos, float sizeMultiplier)
    {
        Brick newBrick = Instantiate(_brickPrefab, transform);
        newBrick.transform.localScale *= sizeMultiplier;
        newBrick.transform.position = pos;
        return newBrick;
    }

    private void AddActiveBrick(Brick brick)
    {
        _activeBricks.TryAdd(brick.transform.position, brick);
    }

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

    public void SelectOverlay(Overlay newOverlay)
    {
        if (_selectedOverlay) _selectedOverlay.ToggleHighlight(false);
        _selectedOverlay = newOverlay;
        _selectedOverlay.ToggleHighlight(true);
    }

    private void CycleOverlay(Vector3 overlayOffset)
    {
        Vector3 pos = _selectedOverlay.transform.position;
        pos += overlayOffset;

        while (_overlays.TryGetValue(pos, out Overlay newOverlay))
        {
            if (newOverlay != _selectedOverlay && !newOverlay.HasBrick)
            {
                SelectOverlay(newOverlay);
                return;
            }

            pos += overlayOffset;
        }
    }

    public bool AddTopBrick(Vector3 pos)
    {
        if (_topBricks.ContainsKey(pos))
        {
            return false;
        }

        Brick newBrick = _brickStack[0];
        newBrick.transform.position = pos;
        newBrick.transform.localScale = new Vector3(1, 1, 0);
        newBrick.ToggleProtoBrick(true);
        _topBricks.Add(pos, newBrick);

        _brickStack.RemoveAt(0);
        _brickStack.Add(CreateBrick(Vector3.zero, _brickStackSize));

        if (_brickHold.isActiveAndEnabled)
        {
            _brickHold.UsedHold(false);
        }

        UpdateBrickStack();

        if (CheckTopBricks(pos))
        {
            return false;
        }

        if (_topBricks.Count == _gridSize.x)
        {
            ShiftRow();
            return false;
        }

        return true;
    }

    private void RemoveTopBrick(Vector3 pos)
    {
        if (_topBricks.Remove(pos, out Brick brick))
        {
            brick.DestroyBrick();
        }

        if (_overlays.TryGetValue(pos, out Overlay overlay))
        {
            overlay.ClearOverlay();
        }
    }

    //TODO: Unsupported
    public void HoldBrick()
    {
        if (!_brickHold.isActiveAndEnabled) return;

        if (_heldBrick)
        {
            var tempBrick = _heldBrick;
            _heldBrick = _brickStack[0];
            _brickStack.RemoveAt(0);
            _brickStack.Insert(0, tempBrick);
        }
        else
        {
            _heldBrick = _brickStack[0];
            _brickStack.RemoveAt(0);
            _brickStack.Add(CreateBrick(Vector3.zero, _brickStackSize));
        }

        _brickHold.UsedHold(true);

        UpdateBrickStack();
    }

    private void ShiftRow()
    {
        foreach (var overlay in _overlays)
        {
            overlay.Value.ClearOverlay();
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

    private void CreateOverlay(Vector3 pos)
    {
        Overlay newOverlay = Instantiate(_overlayPrefab, transform);

        newOverlay.transform.localScale = _offset;
        newOverlay.transform.position = pos;

        _overlays.Add(pos, newOverlay);

        SelectOverlay(newOverlay);
    }

    private bool CheckTopBricks(Vector3 pos)
    {
        bool removed = false;

        if (_topBricks.TryGetValue(pos, out Brick brick))
        {
            if (_topBricks.TryGetValue(pos + new Vector3(-_offset.x, 0, 0), out Brick left) && left.SpriteId == brick.SpriteId)
            {
                RemoveTopBrick(left.transform.position);
                RemoveTopBrick(brick.transform.position);
                removed = true;
            }

            if (_topBricks.TryGetValue(pos + new Vector3(_offset.x, 0, 0), out Brick right) && right.SpriteId == brick.SpriteId)
            {
                RemoveTopBrick(right.transform.position);
                RemoveTopBrick(brick.transform.position);
                removed = true;
            }
        }

        return removed;
    }
}