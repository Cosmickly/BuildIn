using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Obsolete("Bricks handled by BrickManager now")]
public class GridManager : MonoBehaviour
{
    private Grid _grid;
    public Vector2Int GridSize;
    private Vector3 _offset;

    public GameObject BrickPrefab;
    public Tilemap Overlay;
    public Color32 HighlightColour;
    private Vector3Int _prevMousePos;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _offset.x = -(((GridSize.x * (1 + _grid.cellGap.x)) - _grid.cellGap.x) / 2);
        _offset.y = 1;

        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                Vector3 pos = _grid.CellToWorld(new Vector3Int(i, j, 0));
                pos += _offset;
                Instantiate(BrickPrefab, pos, transform.rotation);
            }
        }
    }

    public void PlaceBrick(Vector2 pos)
    {
        Vector3Int newPos = _grid.WorldToCell(pos);
        if (Overlay.HasTile(newPos))
        {
            Instantiate(BrickPrefab, newPos, transform.rotation);
        }
    }

    public void Highlight(Vector2 pos)
    {
        Vector3Int newPos = _grid.WorldToCell(pos);
        if (newPos != _prevMousePos)
        {
            Overlay.SetColor(_prevMousePos, new Color32(0, 0, 0, 0));
            Overlay.SetColor(newPos, HighlightColour);
            _prevMousePos = newPos;
        }
    }
}