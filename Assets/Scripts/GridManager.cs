using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private Grid grid;
    public Vector2Int gridSize;
    private Vector3 offset;

    public GameObject brickPrefab;
    public Tilemap overlay;
    public Color32 highlightColour;
    private Vector3Int prevMousePos;

    private List<Vector3> bricks = new List<Vector3>();

    private void Awake() {
        grid = GetComponent<Grid>();

    }

    // Start is called before the first frame update
    void Start()
    {
        //overlayPrefab.CompressBounds();
        //foreach (Vector3Int pos in overlayPrefab.cellBounds.allPositionsWithin) {
        //    //if (overlayPrefab.HasTile(pos)) Debug.Log(pos);
        //    overlayPrefab.SetTileFlags(pos, TileFlags.None);
        //    overlayPrefab.SetColor(pos, new Color32(0, 0, 0, 0));
        //}

        offset.x = -(((gridSize.x * (1 + grid.cellGap.x)) - grid.cellGap.x) / 2);
        offset.y = 1;
        //transform.position = new Vector3 (-offset ,0,0);

        for (int i = 0; i < gridSize.x; i++) {
            for (int j = 0; j < gridSize.y; j++) {
                Vector3 pos = grid.CellToWorld(new Vector3Int(i, j, 0));
                pos += offset;
                GameObject brick = Instantiate(brickPrefab, pos, transform.rotation);
                //activeBricks.Add(pos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceBrick(Vector2 pos) {
        Vector3Int newPos = grid.WorldToCell(pos);
        if (overlay.HasTile(newPos)) {
            Instantiate(brickPrefab, newPos, transform.rotation);
            //activeBricks.Add(newPos);
        }
    }

    public void Highlight(Vector2 pos) {
        Vector3Int newPos = grid.WorldToCell(pos);
        if (newPos != prevMousePos) {
            overlay.SetColor(prevMousePos, new Color32(0, 0, 0, 0));
            overlay.SetColor(newPos, highlightColour);
            prevMousePos = newPos;
        }
    }
}
